#region License
// Furtails Importer - Importer from furtails.pw database to Arkumida
// Copyright (C) 2023  Earlybeasts
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Collections.Concurrent;
using System.Net.Http.Json;
using Dapper;
using furtails_importer.Dbos;
using furtails_importer.WebClientStuff.Requests;
using MySqlConnector;

namespace furtails_importer.Importers;

public class PrivateMessagesImporter
{
    private readonly MySqlConnection _connection;
    private readonly HttpClient _httpClient;
    private readonly UsersImporter _usersImporter;

    public PrivateMessagesImporter(MySqlConnection connection, HttpClient httpClient, UsersImporter usersImporter)
    {
        _connection = connection;
        _httpClient = httpClient;
        _usersImporter = usersImporter;
    }

    public async Task ImportAsync()
    {
        var privateMessages = _connection.Query<FtPrivateMessage>
            (
                @"select
                    id as Id,
                    toUserId as Receiver,
                    fromUserId as Sender,
                    sendDate as SendTime,
                    message as Content,
                    isRead as IsRead,
                    toIsDeleted as IsDeletedAtReceiver,
                    fromIsDeleted as IsDeletedAtSender,
                    messageType as MessageType,
                    messageTypeTag as MessageTypeTag
                from ft_private_messages"
            )
            .ToList();

        var oldFtCreaturesIds = privateMessages
            .Select(pm => pm.Sender)
            .ToList()
            .Concat
            (
                privateMessages
                    .Select(pm => pm.Receiver)
                    .ToList()
            )
            .Distinct()
            .ToList();
        
        // Mapping old FT users to Arkumida users
        var parallelismDegree = new ParallelOptions()
        {
            MaxDegreeOfParallelism = MainImporter.ParallelismDegree
        };

        Console.WriteLine("Mapping creatures:");

        var creaturesMapping = new ConcurrentDictionary<int, Guid>();
            
        await Parallel.ForEachAsync(oldFtCreaturesIds, parallelismDegree, async (oldCreatureId, token) =>
        {
            var arkumidaCreatureId = await _usersImporter.MapOldFtCreatureAsync(oldCreatureId);
            creaturesMapping.TryAdd(oldCreatureId, arkumidaCreatureId);
            
            Console.WriteLine($"{oldCreatureId} -> {arkumidaCreatureId}");
        });
        
        Console.WriteLine("Importing private messages...");
        
        await Parallel.ForEachAsync(privateMessages, parallelismDegree, async (privateMessage, token) =>
        {
            Console.WriteLine($"Importing message with ID = { privateMessage.Id }");
            await AddPirvateMessageToArkumidaAsync(privateMessage, creaturesMapping);
        });
    }

    private async Task AddPirvateMessageToArkumidaAsync(FtPrivateMessage privateMessage, ConcurrentDictionary<int, Guid> usersMapping)
    {
        var importRequest = new ImportPrivateMessageRequest()
        {
            Content = privateMessage.Content,
            SenderId = usersMapping[privateMessage.Sender],
            ReceiverId = usersMapping[privateMessage.Receiver],
            SentTime = privateMessage.SendTime.ToUniversalTime(),
            ReadTime = privateMessage.IsRead ? privateMessage.SendTime.ToUniversalTime() : null,
            IsDeletedOnSenderSide = privateMessage.IsDeletedAtSender,
            IsDeletedOnReceiverSide = privateMessage.IsDeletedAtReceiver
        };
        
        var response = await _httpClient.PostAsJsonAsync($"{MainImporter.BaseUrl}PrivateMessages/Import", importRequest);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
    }
}