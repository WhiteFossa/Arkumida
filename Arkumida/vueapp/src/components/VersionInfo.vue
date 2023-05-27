<!-- Shows version info -->
<script setup>
    import LoadingSymbol from './LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'

    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
    // True if loading under way
    const isLoading = ref(true)
    
    // Version string
    const versionString = ref(null)
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        versionString.value = (await (await fetch(apiBaseUrl + `/api/SiteInfo/Version`)).json()).versionString
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
    </div>
</template>
