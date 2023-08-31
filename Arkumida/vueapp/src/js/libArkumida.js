// Add icon to icons list

import {
    ComicsImageIdPrefix,
    FullsizeImageIdPrefix,
    SpecialTextType,
    TagMeaning,
    TextElementType,
    TextIconType,
    TextType
} from "@/js/constants";

function AddIconToList(sourceIcons, iconsList)
{
    sourceIcons.forEach(icon =>
    {
        iconsList.value.push({ "type": icon.type, "url": icon.url });
    })
}

function BytesToKilobytesFormatted(sizeInBytes)
{
    return (sizeInBytes / 1024).toFixed(1);
}

function FilterCategoryTags(tags)
{
    return tags.filter(function (t) { return t.isCategory === true; });
}

function FilterOrdinaryTags(tags)
{
    return tags.filter(function (t) { return t.isCategory === false; });
}

function DetectTextType(tags)
{
    // Only one text type tag is allowed
    let typeTagsCount = tags
        .filter(
            function (t)
            {
                return ((t.meaning === TagMeaning.Stories)
                    || (t.meaning === TagMeaning.Novels)
                    || (t.meaning === TagMeaning.Poetry)
                    || (t.meaning === TagMeaning.Comics));
            }
        )
        .length;

    if (typeTagsCount !== 1)
    {
        throw new Error("One and only one text type tag is allowed and required.");
    }

    let isStory = tags.filter(function (t) { return t.meaning === TagMeaning.Stories; }).length === 1;
    let isNovel = tags.filter(function (t) { return t.meaning === TagMeaning.Novels; }).length === 1;
    let isPoetry = tags.filter(function (t) { return t.meaning === TagMeaning.Poetry; }).length === 1;
    let isComics = tags.filter(function (t) { return t.meaning === TagMeaning.Comics; }).length === 1;

    if (isStory)
    {
        return TextType.Story;
    }
    else if (isNovel)
    {
        return TextType.Novel
    }
    else if (isPoetry)
    {
        return TextType.Poetry
    }
    else if (isComics)
    {
        return TextType.Comics;
    }
}

function DetectSpecialTextType(tags)
{
    let isSnuff = tags.filter(function (t) { return t.meaning === TagMeaning.Snuff; }).length > 0;
    let isSandbox = tags.filter(function (t) { return t.meaning === TagMeaning.Sandbox; }).length > 0;
    let isContest = tags.filter(function (t) { return t.meaning === TagMeaning.Contest; }).length > 0;

    if (isSnuff)
    {
        return SpecialTextType.Snuff;
    }
    else if (isSandbox)
    {
        return SpecialTextType.Sandbox;
    }
    else if (isContest)
    {
        return SpecialTextType.Contest;
    }
    else
    {
        return SpecialTextType.Normal;
    }
}

function IsMlpText(tags)
{
    return tags.filter(function (t) { return t.meaning === TagMeaning.MLP; }).length > 0;
}

function InjectMlpIcon(icons)
{
    icons.value.push({ "type": TextIconType.Mlp, "url": "" });
}

function InjectInclompleteIcon(icons)
{
    icons.value.push({ "type": TextIconType.Incomplete, "url": "" });
}

function RenderTextElement(element)
{
    if (element.type === TextElementType.ParagraphBegin) {
        return "<div class='text-paragraph'>";
    }

    if (element.type === TextElementType.PlainText) {
        return element.content;
    }

    if (element.type === TextElementType.ParagraphEnd) {
        return "</div>";
    }

    if (element.type === TextElementType.FullWidthAlignedTextBegin)
    {
        return "<div class='text-justify-width'>";
    }

    if (element.type === TextElementType.FullWidthAlignedTextEnd)
    {
        return "</div>";
    }

    if (element.type === TextElementType.ItalicBegin)
    {
        return "<em>";
    }

    if (element.type === TextElementType.ItalicEnd)
    {
        return "</em>";
    }

    if (element.type === TextElementType.BoldBegin)
    {
        return "<strong>";
    }

    if (element.type === TextElementType.BoldEnd)
    {
        return "</strong>";
    }

    if (element.type === TextElementType.UnderlineBegin)
    {
        return "<u>";
    }

    if (element.type === TextElementType.UnderlineEnd)
    {
        return "</u>";
    }

    if (element.type === TextElementType.StrikeOutBegin)
    {
        return "<del>";
    }

    if (element.type === TextElementType.StrikeOutEnd)
    {
        return "</del>";
    }

    if (element.type === TextElementType.CentrallyAlignedTextBegin)
    {
        return "<div class='text-align-center'>";
    }

    if (element.type === TextElementType.CentrallyAlignedTextEnd)
    {
        return "</div>";
    }

    if (element.type === TextElementType.LeftAlignedTextBegin)
    {
        return "<div class='text-align-left'>";
    }

    if (element.type === TextElementType.LeftAlignedTextEnd)
    {
        return "</div>";
    }

    if (element.type === TextElementType.RightAlignedTextBegin)
    {
        return "<div class='text-align-right'>";
    }

    if (element.type === TextElementType.RightAlignedTextEnd)
    {
        return "</div>";
    }

    if (element.type === TextElementType.TitleBegin)
    {
        return "<h1>";
    }

    if (element.type === TextElementType.TitleEnd)
    {
        return "</h1>";
    }

    if (element.type === TextElementType.PreformattedTextBegin)
    {
        return "<pre>";
    }

    if (element.type === TextElementType.PreformattedTextEnd)
    {
        return "</pre>";
    }

    if (element.type === TextElementType.QuoteBegin)
    {
        return "<div class='text-quote'>";
    }

    if (element.type === TextElementType.QuoteEnd)
    {
        return "</div>";
    }

    if (element.type === TextElementType.AsciiArtBegin)
    {
        return "<pre class='text-ascii-art'>";
    }

    if (element.type === TextElementType.AsciiArtEnd)
    {
        return "</pre>";
    }

    if (element.type === TextElementType.UrlBegin)
    {
        return "<a href='" + element.parameters[0] + "'>";
    }

    if (element.type === TextElementType.UrlEnd)
    {
        return "</a>";
    }

    if (element.type === TextElementType.ColorBegin)
    {
        return "<span style='color: " + element.parameters[0] +";'>";
    }

    if (element.type === TextElementType.ColorEnd)
    {
        return "</span>";
    }

    if (element.type === TextElementType.SizedAsciiArtBegin)
    {
        return "<pre className='text-ascii-art' style='font-size: " + element.parameters[0] + "rem;'>";
    }

    if (element.type === TextElementType.SizedAsciiArtEnd)
    {
        return "</pre>";
    }

    if (element.type === TextElementType.EmbeddedImage)
    {
        return "<div class='centered'>" +
            "<img id='" + FullsizeImageIdPrefix + element.parameters[0] + "' class='text-image-preview' src='" + process.env.VUE_APP_API_URL + "/api/Files/Download/" + element.parameters[0] + "'/>" +
            "</div>";
    }

    if (element.type === TextElementType.ComicsImage)
    {
        return "<div class='comics-image-container'>" +
            "<img id='" + ComicsImageIdPrefix + element.parameters[0] + "' class='comics-image' src='" + process.env.VUE_APP_API_URL + "/api/Files/Download/" + element.parameters[0] + "'/>" +
            "</div>";
    }

    throw new Error("Unknown element type!");
}

function GenerateLinkToText(textId, pageNumber)
{
    return "/texts/" + textId + "/page/" + pageNumber;
}

// Postprocess creature profile (sort stuff and so on)
function PostprocessCreatureProfile(profile)
{
    // Ordering avatars alphabetically to have stable order
    profile.value.avatars.sort((a, b) => a.name.localeCompare(b.name));
}

export
{
    AddIconToList,
    BytesToKilobytesFormatted,
    FilterCategoryTags,
    FilterOrdinaryTags,
    DetectTextType,
    DetectSpecialTextType,
    IsMlpText,
    InjectMlpIcon,
    InjectInclompleteIcon,
    RenderTextElement,
    GenerateLinkToText,
    PostprocessCreatureProfile
}