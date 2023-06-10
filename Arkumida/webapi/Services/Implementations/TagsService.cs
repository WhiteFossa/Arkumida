using webapi.Models;
using webapi.Models.Api.Enums;
using webapi.Services.Abstract;

namespace webapi.Services.Implementations;

public class TagsService : ITagsService
{
    private readonly IReadOnlyCollection<Tag> _tagsStorage = new List<Tag>()
    {
        new Tag() { Id = new Guid("18d7c497-e3d3-4f62-be02-486e39c0f2f6"), FurryReadableId = "f", Name = "F", TextsCount = 10, IsCategory = true, CategoryOrder = 100, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("916e6329-3975-468e-b765-405c7d487058"), FurryReadableId = "ff", Name = "F/F", TextsCount = 30, IsCategory = true, CategoryOrder = 200, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("ac795fe2-3b97-45d1-b3bc-7b98b01613e9"), FurryReadableId = "m", Name = "M", TextsCount = 10, IsCategory = true, CategoryOrder = 300, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("acc0abbf-4d12-432f-99db-8ffd5854ee22"), FurryReadableId = "mf", Name = "M/F", TextsCount = 100, IsCategory = true, CategoryOrder = 400, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("79bd0d5e-b34c-48a5-8989-8644524de5f6"), FurryReadableId = "mm", Name = "M/M", TextsCount = 90, IsCategory = true, CategoryOrder = 500, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("423af7cb-5f19-4133-b2df-f5ce79b4519e"), FurryReadableId = "noyiff", Name = "NO YIFF", TextsCount = 10, IsCategory = true, CategoryOrder = 600, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("f8e00608-15e5-4e45-8346-89be5a106927"), FurryReadableId = "stories", Name = "Рассказы", TextsCount = 10, IsCategory = true, CategoryOrder = 700, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("1353e17f-1e4e-4218-a95b-8d16215ba63a"), FurryReadableId = "novels", Name = "Повести и Романы", TextsCount = 10, IsCategory = true, CategoryOrder = 800, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("fb58a392-fc3d-4652-8d2f-92af4dccaa13"), FurryReadableId = "poetry", Name = "Стихи", TextsCount = 10, IsCategory = true, CategoryOrder = 900, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("a4b220ed-13f4-4c05-a706-36a978fb74b8"), FurryReadableId = "snuff", Name = "Логово Снарфа", TextsCount = 10, IsCategory = true, CategoryOrder = 1000, CategoryTagType = CategoryTagType.Snuff },
        new Tag() { Id = new Guid("1f37fec5-afab-4301-aa67-f56e9aafb397"), FurryReadableId = "editing", Name = "Мастерская Гайки", TextsCount = 10, IsCategory = true, CategoryOrder = 1100, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("8516fb94-6867-4f84-beb6-18cf58970b02"), FurryReadableId = "metamor", Name = "Цитадель Метамор", TextsCount = 10, IsCategory = true, CategoryOrder = 1200, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("9acb5c3f-6409-4431-b5ca-02f6c0476dc0"), FurryReadableId = "sandbox", Name = "Песочница", TextsCount = 10, IsCategory = true, CategoryOrder = 1300, CategoryTagType = CategoryTagType.Sandbox },
        new Tag() { Id = new Guid("53fe01b6-05f8-4fac-aa02-d167be7dd265"), FurryReadableId = "comics", Name = "Комиксы", TextsCount = 10, IsCategory = true, CategoryOrder = 1400, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("94f6eeae-ea29-4f3b-a09f-cec7c20843ad"), FurryReadableId = "contest", Name = "Конкурсные рассказы", TextsCount = 10, IsCategory = true, CategoryOrder = 1500, CategoryTagType = CategoryTagType.Contest },
        new Tag() { Id = new Guid("4b1ccd3e-f464-4c0b-a68c-75c513eedf11"), FurryReadableId = "chakona", Name = "Chakona", TextsCount = 10, IsCategory = true, CategoryOrder = 1600, CategoryTagType = CategoryTagType.Normal },
        new Tag() { Id = new Guid("235738f5-3918-4aeb-83ba-658c8671574a"), FurryReadableId = "series", Name = "Серии", TextsCount = 10, IsCategory = true, CategoryOrder = 1700, CategoryTagType = CategoryTagType.Normal }
    };

    public async Task<IReadOnlyCollection<Tag>> GetCategoriesTagsAsync()
    {
        return _tagsStorage
            .Where(t => t.IsCategory)
            .OrderBy(t => t.CategoryOrder)
            .ToList();
    }

    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        return _tagsStorage
            .Single(t => t.Id == id);
    }
}