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

    static PasswordChangedMessage = "Ваш пароль успешно изменён, сейчас вы будете перенаправлены на страницу входа на сайт."

    static PasswordChangeRequiredAfterImport = "Ваш аккаунт был перенесён с старой версии сайта. Вам необходимо сменить пароль. Сейчас вы будете перенаправлены на страницу изменения пароля."

    static EmailAddressConfirmationEmailSent = "Письмо с инструкциями по подтверждению адреса электронной почты отправлено."
    static EmailAddressConfirmationEmailNotSent = "Не удалось отправить письмо с инструкциями по подтверждению адреса электронной почты. Корректен-ли адрес?"

    static EmailAddressConfirmed = "Адрес электронной почты подтверждён."
    static EmailAddressFailedToConfirm = "Не удалось подтвердить адрес электронной почты. Корректна-ли ссылка? Не протухло-ли письмо?"

    static EmailAddressChanged = "Адрес электронной почты изменён."
    static EmailAddressFailedToChange = "Не удалось изменить адрес электронной почты. Корректна-ли ссылка? Не протухло-ли письмо?"

    static EmailAddressChangeEmailSent = "Письмо с инструкциями по изменению адреса электронной почты отправлено."
    static EmailAddressChangeRequestFailed = "Не удалось начать процесс изменения адреса электронной почты."

    static RegistrationSuccess = "Регистрация успешна, сейчас вы будете перенаправлены на страницу входа на сайт."
    static LoginIsTaken = "Этот логин уже занят."
    static PasswordTooWeak = "Вы выбрали слишком простой пароль, усложните его."
    static GenericRegistrationError = "Неизвестная ошибка в процессе регистрации."
    static RegistrationConfirmationTitle = "Корректны-ли данные?"
    static RegistrationConfirmationText = "Корректны-ли введённые данные? Указанный логин будет невозможно изменить (но можно будет изменить отображаемое имя пользователя)."

    static PasswordResetInstructionsSent = "Инструкции по сбросу пароля отправлены на вашу почту."
    static PasswordResetSuccessful = "Ваш пароль успешно сброшен, сейчас вы будете перенаправлены на страницу входа на сайт."
    static PasswordResetFailed = "Не удалось сбросить пароль. Корректна-ли ссылка? Не протухло-ли письмо?"

    static PrivateMessagesFailedToSend = "Не удалось отправить личное сообщение, повторите попытку позже или свяжитесь с администраторами."

    static UserNotFoundByName = "Пользователь с таким именем не найден."

    static CriticsSettingsFailedToUpdate = "Не удалось обновить настройки критики. Обновите страницу."

    static TextCommentFailedToSend = "Не удалось отправить комментарий к тексту, повторите попытку позже или свяжитесь с администраторами."
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
    static CreatureQuoteBegin = 35 // Creature's quote (for forum and comments)
    static CreatureQuoteEnd = 36
    static ExternalImage = 37 // External (hotlinked) image
}

// Prefixes for images (used to show popups in rendered HTML)
export class ImagesPrefixes
{
    static FullsizeImage = "e76eb871-7129-4ad2-be31-d94068051923_"

    static ComicsImage = "e7cd4703-ba8b-4241-a873-3f4298ad9ca9_";

    static ExternalImage = "124e0a36-51e3-4620-81dc-0503f7c829e9_";
}

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

// Possible user registration results
export class UserRegistrationResult
{
    static OK = 0

    static LoginIsTaken = 1

    static WeakPassword = 2

    static GenericError = 4
}

// Possible password reset initiation results
export class PasswordResetInitiationResult
{
    static Initiated = 0

    static CreatureNotFound = 1

    static CreatureHaveNoEmail = 2

    static CreatureHaveNoConfirmedEmail = 3

    static FailedToSendEmail = 4
}

export class PrivateMessagesConstants
{
    // Display this if we have more than 100 unread messages
    static MaxUnreadPrivateMessagesCountMessage = "99+"

    // Polling interval for unread messages in header
    static UnreadPrivateMessagesInHeaderPollingInterval = 60000

    // Load no more than this amount of private messages at once
    static LoadBatchSize = 20

    // Polling interval for private messages update (on private messages page)
    static PrivateMessagesPollingInterval = 5000
}

// Possible private message marking as read result
export class MarkPrivateMessageAsReadResult
{
    static Successful = 0

    static AlreadyMarkedAsRead = 1
}

// Common constants
export class CommonConstants
{
    // .NET int max value
    static DotnetIntMaxValue = 2147483647

    // .NET int min value
    static DotnetIntMinValue = -2147483648

    // Do requests for list of creatures by part of name only when part of name longer or equal to this value
    static NamePartMinimalLengthToLookup = 3

    // Enter button keycode
    static EnterKeycode = 13
}

// Constants, related to texts search
export class SearchConstants
{
    // Search page size
    static PageSize = 10
}

// Constants, related to texts comments
export class TextsCommentsConstants
{
    // Load no more than this amount of last comments for text
    static CommentsCountToLoad = 10
}

// Possible operation modes for text renderer
export class TextRendererOperationModes
{
    // Rendering text (i.e. text, not a comments/userinfo and so on)
    static Text = 0

    // Rendering anything else than text
    static NonText = 1
}
