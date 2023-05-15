<script setup>
import { ref } from 'vue'

// True is loading under way
const isLoading = ref(true)
const versionString = ref(null)

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
        <!-- Main page code -->
        <h1>Furtails.pw</h1>
        <div>Version string: {{ versionString }}</div>
    </div>

    <button @click="OnLoad">Load</button>
</template>
