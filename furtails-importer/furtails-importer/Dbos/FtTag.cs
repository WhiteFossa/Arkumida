namespace furtails_importer.Dbos;

public class FtTag
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool IsHidden { get; set; }

    public bool IsWarning { get; set; }

    public int GroupId { get; set; }

    public string Icon { get; set; }

    public int AccessMode { get; set; }

    public string Class { get; set; }
}