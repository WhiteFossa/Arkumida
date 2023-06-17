namespace furtails_importer.Dbos;

public class FtRawTextSection
{
    public int Id { get; set; }

    public int TextId { get; set; }

    public int SectionNumber { get; set; }

    public string SectionRus { get; set; }

    public int LockUserId { get; set; }

    public DateTime LockDateTime { get; set; }

    public string SectionEng { get; set; }

    public int TranslatorId { get; set; }

    public DateTime SaveDate { get; set; }
}