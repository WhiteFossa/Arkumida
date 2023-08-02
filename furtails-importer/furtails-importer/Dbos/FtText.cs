namespace furtails_importer.Dbos;

public class FtText
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    /// <summary>
    /// Crazy enough, but some texts have null here
    /// </summary>
    public int? UploaderUserId { get; set; }

    public string UploaderUserName { get; set; }

    public DateTime CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Author { get; set; }

    public string Translator { get; set; }

    public bool IsHidden { get; set; }

    public int Size { get; set; }

    public int FileSection { get; set; }

    public int Tid { get; set; } // Topic ID?

    public int SeriesId { get; set; }

    public int VotesPlus { get; set; }

    public int VotesMinus { get; set; }

    public int ReadsCount { get; set; }

    public int CommentsCount { get; set; }

    public int VotesCount { get; set; }

    public int BaseType { get; set; }

    public int Type { get; set; }

    public int IsDeleted { get; set; }

    public int PublishStatus { get; set; }

    public int ContestId { get; set; }
    
    public int IsAnonymous { get; set; }

    public bool IsNotFinished { get; set; }
}