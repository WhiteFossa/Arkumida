<script setup>
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
        isLoading.value = !isLoading.value
    
        const versionInfo = await (await fetch(`api/VersionInfo/Get`)).json()
        versionString.value = versionInfo.versionString
    }

</script>

<template>
    <div v-if="isLoading">
        <!-- Loading message -->
        <h1>Loading...</h1>
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div>Version: {{ versionString }}</div>
    </div>
</template>
