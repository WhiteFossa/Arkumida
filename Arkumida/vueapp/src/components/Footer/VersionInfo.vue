<!-- Shows version info -->
<script setup>
    import LoadingSymbol from '../Shared/LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const isLoading = ref(true)

    const versionInfo = ref(null)
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        versionInfo.value = await (await WebClientSendGetRequest("/api/SiteInfo/Version")).json()
        
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <div class="version-info">
            <div>
                Бэкенд: {{ versionInfo.versionString }}
            </div>
            <div>
                Фронтенд: Коири-Б мод. 1
            </div>
        </div>

        <div class="sources-link">
            <a :href="versionInfo.sourcesUrl" title="Исходные коды">Исходные коды</a>
        </div>
    </div>
    
    <div class="centered">
        <a href="https://www.gnu.org/licenses/agpl-3.0.html" title="Лицензировано под AGPLv3 или более поздней версией">
            <img src="/images/agplv3-with-text-162x68.webp" alt="AGPLv3 logo" />
        </a>
    </div>
</template>
