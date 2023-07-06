// Messages
export class Messages
{
    static AllTextsByAuthor = "Все произведения автора "
    static AllTextsByPublisher = "Все произведения, размещённые пользователем "
    static AllTextsByTranslator = "Все произведения, переведённые пользователем "
    static Stories = "Рассказы"
    static AllStories = "Все рассказы"
    static Novels = "Повести и Романы"
    static AllNovels = "Все повести и романы"
    static Poetry = "Стихи"
    static AllPoetry = "Все стихи"
    static Comics = "Комиксы"
    static AllComics = "Все комиксы"

    static Participants = "Участники"
    static Species = "Виды"
    static Setting = "Сеттинг"
    static Actions = "Действия"
}

// Category tag types
export class CategoryTagType
{
    static Normal = 0
    static Snuff = 1
    static Sandbox = 2
    static Contest = 3
}

// Special text type
export class SpecialTextType
{
    static Normal = 0
    static Contest = 1
    static Sandbox = 2
    static Snuff = 3
}

// Text icon types
export class TextIconType
{
    static Contest = 0
    static Sandbox = 1
    static Snuff = 2
    static Illustrations = 3
    static Incomplete = 4
    static Mlp = 5
    static Series = 6
}

// Tags size categories
export class TagSizeCategory
{
    static Cat0 = 0
    static Cat1 = 1
    static Cat2 = 2
    static Cat3 = 3
    static Cat4 = 4
    static Cat5 = 5
    static Cat6 = 6
    static Cat7 = 7
    static Cat8 = 8
    static Cat9 = 9
    static Cat10 = 10
    static Cat11 = 11
    static Cat12 = 12
    static Cat13 = 13
}

// Tags subtypes
export class TagSubtype
{
    static Participants = 1
    static Species = 2
    static Setting = 3
    static Actions = 4
    static Category = 5
}

// Machine-readable tag meaning
export class TagMeaning
{
    static Unspecified = 0
    static Stories = 1
    static Novels = 2
    static Poetry = 3
    static Comics = 4
    static Contest = 5
    static Sandbox = 6
    static Snuff = 7
    static MLP = 8
}

// Text type
export class TextType
{
    static Story = 0
    static Novel = 1
    static Poetry = 2
    static Comics = 3
}

// Possible text element types
export class TextElementType
{
    static ParagraphBegin = 0
    static PlainText = 1
    static ParagraphEnd = 2
    static FullWidthAlignedTextBegin = 3
    static FullWidthAlignedTextEnd = 4
    static ItalicBegin = 5
    static ItalicEnd = 6
    static BoldBegin = 7
    static BoldEnd = 8
    static UnderlineBegin = 9
    static UnderlineEnd = 10
    static StrikeOutBegin = 11
    static StrikeOutEnd = 12
    static CentrallyAlignedTextBegin = 13
    static CentrallyAlignedTextEnd = 14
    static LeftAlignedTextBegin = 15
    static LeftAlignedTextEnd = 16
    static RightAlignedTextBegin = 17
    static RightAlignedTextEnd = 18
    static TitleBegin = 19
    static TitleEnd = 20
    static PreformattedTextBegin = 21
    static PreformattedTextEnd = 22
    static QuoteBegin = 23
    static QuoteEnd = 24
}