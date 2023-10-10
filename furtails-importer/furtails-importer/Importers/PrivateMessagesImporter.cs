using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using furtails_importer.Dbos;
using furtails_importer.WebClientStuff.Requests;
using furtails_importer.WebClientStuff.Responses;
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
            var arkumidaCreatureId = await MapOldFtCreatureAsync(oldCreatureId);
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

    private async Task<Guid> MapOldFtCreatureAsync(int oldCreatureId)
    {
        await using var connection = new MySqlConnection(MainImporter.ConnectionString);
        
        var login = connection.Query<FtUser>
            (
                @"select
                    username as Username
                from ft_users
                where id = @id",
                new { id = oldCreatureId }
            )
            .Single()
            .Username;

        var arkumidaCreature = await _usersImporter.FindCreatureByLogin(login);
        
        return arkumidaCreature.Id;
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