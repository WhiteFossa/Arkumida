// Add icon to icons list
function AddIconToList(sourceIcons, iconsList)
{
    sourceIcons.forEach(icon =>
    {
        iconsList.value.push({ "type": icon.type, "url": icon.url });
    })
}

export
{
    AddIconToList
}