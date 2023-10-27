<script setup>

    // API base URL
    import {onMounted, ref} from "vue";
    import CategoryComponent from "@/components/MainPage/Categories/CategoryComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const isLoading = ref(true)

    const categories = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        const response = await (await WebClientSendGetRequest("/api/Categories/List")).json()
        categories.value = response.categoriesTags

        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-else>
        <CategoryComponent
            :category="category"
            v-for="category in categories" :key="category.entityId" />
    </div>
</template>
