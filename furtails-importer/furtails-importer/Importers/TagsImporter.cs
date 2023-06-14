using Dapper;
using furtails_importer.Dbos;
using MySqlConnector;

namespace furtails_importer.Importers;

public class TagsImporter
{
    private readonly MySqlConnection _connection;

    public TagsImporter(MySqlConnection connection)
    {
        _connection = connection;
    }

    public async Task Import()
    {
        var tags = _connection.Query<FtTag>
            (
                @"select
                    id as Id,
                    name as Name,
                    isHidden as IsHidden,
                    isWarning as IsWarning,
                    groupId as GroupId,
                    icon as Icon,
                    access_mode as AccessMode,
                    class as Class
                from ft_tags"
            );

        foreach (var tag in tags)
        {
            Console.WriteLine(tag.Name);
        }
    }
}