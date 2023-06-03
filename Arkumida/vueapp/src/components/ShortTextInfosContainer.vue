<!-- Text short info -->
<script setup>
    import { ref, onMounted, watch } from 'vue'
    import LoadingSymbol from "@/components/LoadingSymbol.vue";
    import ShortTextInfo from "@/components/ShortTextInfo.vue";
    
    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
    // True if loading under way
    const isLoading = ref(true)
    
    // How many texts can be displayed at once
    const takeSize = 7;
    
    // Skip this amount of texts
    const skip = ref(0)
    
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
        await LoadTexts()
    }

    // Load texts with skipping
    async function LoadTexts()
    {
        isLoading.value = true
        
        texts.value = (await (await fetch(apiBaseUrl + `/api/Texts/Latest?skip=` + skip.value + `&take=` + takeSize)).json()).textInfos

        isLoading.value = false
    }
    
    // Move up (e.g. skip --)
    async function MoveUp()
    {
        skip.value -= takeSize
        
        if (skip.value < 0)
        {
            skip.value = 0;
        }
    }
    
    // Move down (e.g. skip ++)
    async function MoveDown()
    {
        skip.value += takeSize
    }

    watch(skip, LoadTexts)

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    
    <div class="vertical-flex texts-short-info-blocks-container">
        <button @click="MoveUp">&lt;&lt;</button>

        <div v-if="!isLoading">
            <ShortTextInfo :id="text.entityId" v-for="text in texts" :key="text.entityId"/>
        </div>

        <button @click="MoveDown">&gt;&gt;</button>
    </div>
    
</template>