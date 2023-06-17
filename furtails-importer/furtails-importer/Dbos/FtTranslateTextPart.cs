namespace furtails_importer.Dbos;

public class FtTranslateTextPart
{
    public int Id { get; set; }

    public int TextId { get; set; } // -> FtText.Id

    public int OrderNumber { get; set; }

    public string OriginalText { get; set; }

    public int LockUserId { get; set; }

    public DateTime LockTime { get; set; }
}