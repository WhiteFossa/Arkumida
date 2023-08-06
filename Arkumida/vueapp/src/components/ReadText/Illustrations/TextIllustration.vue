<script setup>
    import {defineProps, onMounted, ref} from "vue";
    import FullsizeImagePopup from "@/components/ReadText/Illustrations/FullsizeImagePopup.vue";

    const props = defineProps({
        illustration: Object
    })

    const apiBaseUrl = process.env.VUE_APP_API_URL
    const fullsizeImageUrl = ref("")

    const isImagePopupShown = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        fullsizeImageUrl.value = apiBaseUrl + "/api/Files/Download/" + props.illustration.file.id
    }

    async function ShowImagePopup()
    {
        isImagePopupShown.value = true
    }

    async function HideImagePopup()
    {
        isImagePopupShown.value = false
    }
</script>

<template>
    <img :src="fullsizeImageUrl" :alt="props.illustration.file.name" class="illustration-preview" @click="ShowImagePopup"/>

    <FullsizeImagePopup v-if="isImagePopupShown === true" @closePopup="HideImagePopup" :id="props.illustration.file.id" />
</template>