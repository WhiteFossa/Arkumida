<!-- Text short info -->
<script setup>
    import { ref, onMounted } from 'vue'
    import LoadingSymbol from "@/components/LoadingSymbol.vue";
    import ShortTextInfo from "@/components/ShortTextInfo.vue";
    //import ShortTextInfo from "@/components/ShortTextInfo.vue";
    
    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
    // True if loading under way
    const isLoading = ref(true)

    // Texts
    const texts = ref([])
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        texts.value = (await (await fetch(apiBaseUrl + `/api/Texts/Latest?skip=0&take=100`)).json()).textInfos
        isLoading.value = false
    }


</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else class="texts-short-info-blocks-container">
            <ShortTextInfo :id="text.entityId" v-for="text in texts" :key="text.entityId"/>
    </div>
</template>