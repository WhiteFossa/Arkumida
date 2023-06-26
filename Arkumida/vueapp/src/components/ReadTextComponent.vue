<!-- Root component for reading texts -->

<script setup>
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    const props = defineProps({
        id: String
    })

    import {defineProps, onMounted, ref} from "vue";

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const textInfo = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        textInfo.value = await (await fetch(apiBaseUrl + `/api/Texts/GetInfo/` + props.id)).json()

        console.log(textInfo.value.textInfo.title)

        isLoading.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>
    <div v-else>
        <h1>{{ textInfo.textInfo.title }}</h1>

        <div>Text ID: {{ $route.params.id }}</div>
    </div>
</template>
