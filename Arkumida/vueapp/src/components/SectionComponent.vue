<!-- Component to display text section -->
<script setup>
import {defineEmits, defineProps, ref} from "vue";
import {RenderTextElement} from "@/js/libArkumida";
import {ComicsImageIdPrefix, FullsizeImageIdPrefix} from "@/js/constants";
import FullsizeImagePopup from "@/components/FullsizeImagePopup.vue";

    const props = defineProps({
        originalText: String, // Original text, it is used when section contains bilingual text
        variants: [Object] // Text variants, Russian texts are here
    })

    const emit = defineEmits(['goToNextPage'])

    const isImagePopupShown = ref(false)
    const fullSizeImageId = ref(null)

    // Ordering variants by time
    const orderedVariants = ref([])
    orderedVariants.value = [...props.variants];
    orderedVariants.value.sort((a, b) => b.creationTime - a.creationTime);

    // Last variant - for now we display only it

    // Rendering text
    const renderedText = ref("")
    orderedVariants.value[0].elements.forEach(e => renderedText.value += RenderTextElement(e))

    async function HandleClick(e)
    {
        const clickedElementId = e.target.id

        if (clickedElementId.startsWith(FullsizeImageIdPrefix))
        {
            // We have image to show
            const imageId = clickedElementId.substring(FullsizeImageIdPrefix.length)
            ShowImagePopup(imageId)
            return
        }

        if (clickedElementId.startsWith(ComicsImageIdPrefix))
        {
            await GoToNextPage()
            return
        }
    }

    async function ShowImagePopup(imageId)
    {
        fullSizeImageId.value = imageId
        isImagePopupShown.value = true
    }

    async function HideImagePopup()
    {
        isImagePopupShown.value = false
    }

    async function GoToNextPage()
    {
        emit('goToNextPage')
    }
</script>

<template>
    <div v-html="renderedText" @click="HandleClick"></div>

    <FullsizeImagePopup v-if="isImagePopupShown === true" @closePopup="HideImagePopup" :id="fullSizeImageId"/>
</template>