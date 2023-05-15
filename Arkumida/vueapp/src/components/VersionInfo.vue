<!-- Shows version info -->
<script setup>
    import LoadingSymbol from './LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'
    
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
        const versionInfo = await (await fetch(`api/VersionInfo/Get`)).json()
        versionString.value = versionInfo.versionString

        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div class="version-info">Version: {{ versionString }}</div>
    </div>
</template>
