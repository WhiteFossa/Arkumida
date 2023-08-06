<!-- Text short info -->
<script setup>
    import {ref, onMounted, watch, defineProps} from 'vue'
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import ShortTextInfo from "@/components/MainPage/TextInfos/ShortTextInfo.vue";

    const props = defineProps({
        dataSource: String // Data source for component, like "/api/Texts/Latest"
    })

    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
    // True if loading under way
    const isLoading = ref(true)
    
    // How many texts can be displayed at once
    const takeSize = 10;
    
    // Skip this amount of texts
    const skip = ref(0)
    
    // Texts
    const texts = ref([])
    
    // How many texts can be additionally loaded
    const remaining = ref(Number.MAX_VALUE)

    watch(skip, LoadTexts)
    
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

        const response = await (await fetch(apiBaseUrl + props.dataSource + `?skip=` + skip.value + `&take=` + takeSize)).json()
        texts.value = response.textInfos
        remaining.value = response.remainingTexts

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
        if (remaining.value > 0)
        {
            skip.value += takeSize
        }
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    
    <div class="vertical-flex texts-short-info-blocks-container">
        <div class="centered">
            <button v-if="skip > 0" class="texts-short-infos-container-arrows-buttons" @click="MoveUp"><img class="texts-short-infos-container-arrows-buttons-images" src="/images/arrow_up.svg" alt="Up arrow" /></button>
        </div>

        <div v-if="!isLoading">
            <ShortTextInfo :id="text.entityId" v-for="text in texts" :key="text.entityId"/>
        </div>

        <div class="centered">
            <button v-if="remaining > 0" class="texts-short-infos-container-arrows-buttons" @click="MoveDown"><img class="texts-short-infos-container-arrows-buttons-images" src="/images/arrow_down.svg" alt="Down arrow" /></button>
        </div>
    </div>
    
</template>