namespace furtails_importer.Dbos;

public class FtTextLink
{
    public int Id { get; set; }

    public int TextId { get; set; }

    public string Link { get; set; }

    public bool IsOk { get; set; }

    public int ErrorsCount { get; set; }

    public DateTime LastCheck { get; set; }

}