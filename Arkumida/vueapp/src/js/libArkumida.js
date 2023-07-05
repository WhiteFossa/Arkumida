// Add icon to icons list

import {SpecialTextType, TagMeaning, TextElementType, TextIconType, TextType} from "@/js/constants";

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

function RenderTextElement(element) {
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
        return "<div style='text-align: justify'>";
    }

    if (element.type === TextElementType.FullWidthAlignedTextEnd)
    {
        return "</div>";
    }

    throw new Error("Unknown element type!");
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
    RenderTextElement
}