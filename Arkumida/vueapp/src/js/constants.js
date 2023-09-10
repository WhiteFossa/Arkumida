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

    static CreatureUser = "Пользователь "

    static LogOutTitle = "Выход"
    static LogOutText = "Вы действительно хотите выйти с сайта?"

    static DeleteAvatarTitle = "Удалить аватарку?"
    static DeleteAvatarTextFirstPart = 'Вы действительно хотите удалить аватарку "'
    static DeleteAvatarTextSecondPart = '"?'

    static SelectOnlyOneFileForUploadAsAvatar = "Выберите ровно один файл для загрузки в качестве аватарки."

    static PasswordChangedMessage = "Ваш пароль успешно изменён, сейчас вы будете перенаправлены на страницу входа для ввода нового пароля."

    static PasswordChangeRequiredAfterImport = "Ваш аккаунт был перенесён с старой версии сайта. Вам необходимо сменить пароль. Сейчас вы будете перенаправлены на страницу изменения пароля."

    static EmailAddressConfirmationEmailSent = "Письмо с инструкциями по подтверждению адреса электронной почты отправлено."

    static EmailAddressConfirmationEmailNotSent = "Не удалось отправить письмо с инструкциями по подтверждению адреса электронной почты. Корректен-ли адрес?"

    static EmailAddressConfirmed = "Адрес электронной почты подтверждён."

    static EmailAddressFailedToConfirm = "Не удалось подтвердить адрес электронной почты. Корректна-ли ссылка? Не протухло-ли письмо?"
}

export class ProfileConsts
{
    // If this action is specified for profile/security, then force password change
    static ForcePasswordChangeActionName = "forcePasswordChange"
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
    static AsciiArtBegin = 25
    static AsciiArtEnd = 26
    static UrlBegin = 27
    static UrlEnd = 28
    static ColorBegin = 29
    static ColorEnd = 30
    static SizedAsciiArtBegin = 31
    static SizedAsciiArtEnd = 32
    static EmbeddedImage = 33
    static ComicsImage = 34
}

// Prefix for IDs of fullsize images
export const FullsizeImageIdPrefix = "e76eb871-7129-4ad2-be31-d94068051923_";

// Prefix for IDs of comics images
export const ComicsImageIdPrefix = "e7cd4703-ba8b-4241-a873-3f4298ad9ca9_";

// Login result
export class LoginResult
{
    static OK = 0
    static InvalidCredentials = 1
    static GenericError = 2
}

// Possible avatar classes
export class AvatarClass
{
    static Small = 0
    static Big = 1
}