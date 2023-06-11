<script setup>
    import TagComponent from "@/components/TagComponent.vue";
    import {onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const tags = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        const response = await (await fetch(apiBaseUrl + '/api/Tags/List')).json()
        tags.value = response.tags

        isLoading.value = false
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-else>
        <span v-for="tag in tags" :key="tag.entityId">
            <TagComponent :id="tag.entityId" />
            <span v-if="tag.entityId !== tags[tags.length - 1].entityId"><pre class="inline-block"> </pre></span>
        </span>
    </div>
</template>
