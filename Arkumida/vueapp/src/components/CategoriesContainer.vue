<script setup>

    // API base URL
    import {onMounted, ref} from "vue";
    import CategoryComponent from "@/components/CategoryComponent.vue";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const categories = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        const response = await (await fetch(apiBaseUrl + '/api/Categories/List')).json()
        categories.value = response.categoriesTags

        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <CategoryComponent :id="category.entityId" v-for="category in categories" :key="category.entityId" />
</template>
