<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    const props = defineProps({
        id: Guid
    })

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const categoryInfo = ref(null)

    const tagLinkHref = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        categoryInfo.value = (await (await fetch(apiBaseUrl + `/api/Categories/` + props.id)).json()).categoryTag

        tagLinkHref.value = "/texts/byTag/" + categoryInfo.value.entityId

        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <div class="category-block"><a class="category-link" :href="tagLinkHref" :title="categoryInfo.tag">{{ categoryInfo.tag }}</a> ({{ categoryInfo.textsCount }})</div>
    </div>

</template>