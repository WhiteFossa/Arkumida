<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import {TagSizeCategory} from "@/js/constants";

    const props = defineProps({
        id: Guid,
        furryReadableId: String,
        sizeCategory: Number,
        tag: String
    })

    const tagHref = ref(null)

    const tagSizeClass = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        tagHref.value = "/texts/byTag/" + props.id

        switch (props.sizeCategory)
        {
            case TagSizeCategory.Cat0:
                tagSizeClass.value = "tag-size-class0";
                break;

            case TagSizeCategory.Cat1:
                tagSizeClass.value = "tag-size-class1";
                break;

            case TagSizeCategory.Cat2:
                tagSizeClass.value = "tag-size-class2";
                break;

            case TagSizeCategory.Cat3:
                tagSizeClass.value = "tag-size-class3";
                break;

            case TagSizeCategory.Cat4:
                tagSizeClass.value = "tag-size-class4";
                break;

            case TagSizeCategory.Cat5:
                tagSizeClass.value = "tag-size-class5";
                break;

            case TagSizeCategory.Cat6:
                tagSizeClass.value = "tag-size-class6";
                break;

            case TagSizeCategory.Cat7:
                tagSizeClass.value = "tag-size-class7";
                break;

            case TagSizeCategory.Cat8:
                tagSizeClass.value = "tag-size-class8";
                break;

            case TagSizeCategory.Cat9:
                tagSizeClass.value = "tag-size-class9";
                break;

            case TagSizeCategory.Cat10:
                tagSizeClass.value = "tag-size-class10";
                break;

            case TagSizeCategory.Cat11:
                tagSizeClass.value = "tag-size-class11";
                break;

            case TagSizeCategory.Cat12:
                tagSizeClass.value = "tag-size-class12";
                break;

            case TagSizeCategory.Cat13:
                tagSizeClass.value = "tag-size-class13";
                break;

            default:
                throw new Error("Unknown tag size category.")
        }
    }

</script>

<template>
    <a :class="tagSizeClass" :href="tagHref">{{ props.tag }}</a>
</template>
