<!-- Shows version info -->
<script setup>
    import LoadingSymbol from '../Shared/LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    
    // True if loading under way
    const isLoading = ref(true)
    
    // Version string
    const versionString = ref(null)

    // Sources URL
    const sourcesUrl = ref(null)
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        const versionInfo = await (await WebClientSendGetRequest("/api/SiteInfo/Version")).json()
        
        versionString.value = versionInfo.versionString
        sourcesUrl.value = versionInfo.sourcesUrl
        
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div class="version-info">Версия: {{ versionString }}</div>
        
        <div class="sources-link">
            <a :href="sourcesUrl" title="Исходные коды">Исходные коды</a>
        </div>
    </div>
    
    <div class="centered">
        <a href="https://www.gnu.org/licenses/agpl-3.0.html" title="Лицензировано под AGPLv3 или более поздней версией">
            <img src="/images/agplv3-with-text-162x68.png" alt="AGPLv3 logo" />
        </a>
    </div>
</template>
