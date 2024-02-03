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

using Dapper;
using furtails_importer.Dbos;
using furtails_importer.WebClientStuff.Dtos;
using MySqlConnector;

namespace furtails_importer.Importers;

public class ForumImporter
{
    private readonly MySqlConnection _connection;
    private readonly HttpClient _httpClient;

    public ForumImporter
    (
        MySqlConnection connection,
        HttpClient httpClient
    )
    {
        _connection = connection;
        _httpClient = httpClient;
    }

    public List<FtForum> LoadForumsList(MySqlConnection connection)
    {
        return connection.Query<FtForum>
            (
                @"select
                    id as Id,
                    forum_name as ForumName,
                    forum_desc as ForumDescription,
                    redirect_url as RedirectUrl,
                    moderators as Moderatrs,
                    num_topics as TopicsCount,
                    num_posts as PostsCount,
                    last_post as LastPostTimestamp,
                    last_post_id as LastPostId,
                    last_poster as LastPoster,
                    sort_by as SortBy,
                    disp_position as DisplayPosition,
                    cat_id as ForumCategoryId
                from ft_forums"
            )
            .ToList();
    }

    public FtTopic GetTopicById(MySqlConnection connection, int id)
    {
        return connection.Query<FtTopic>
            (
                @"select
                id as Id,
                poster as Poster,
                subject as Subject,
                posted as PostedTimestamp,
                first_post_id as FirstPostId,
                last_post as LastPostTimestamp,
                last_post_id as LastPostId,
                last_poster as LastPoster,
                num_views as ViewsCount,
                num_replies as RepliesCount,
                closed as IsClosed,
                sticky as IsSticky,
                moved_to as MovedTo,
                forum_id as ForumId
            from ft_topics
            where id = @topicId",
                new { topicId = id }
            )
            .SingleOrDefault();
    }

    public List<FtPost> LoadOrderedPostsByTopicId(MySqlConnection connection, int topicId)
    {
        return connection.Query<FtPost>
            (
                @"select
                    id as Id,
                    poster as Poster,
                    poster_id as PosterId,
                    poster_ip as PosterIp,
                    poster_email as PosterEmail,
                    message as Message,
                    hide_smilies as IsHideSmiles,
                    posted as PostedTimestamp,
                    edited as EditedTimestamp,
                    edited_by as EditedBy,
                    topic_id as TopicId
                from ft_posts
                where topic_id = @topicIdToLoad
                order by posted asc",
                new { topicIdToLoad = topicId }
            )
            .ToList();
    }
}