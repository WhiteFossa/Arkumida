<!-- Shows site info for header -->
<script setup>
import LoadingSymbol from './LoadingSymbol.vue'

import { ref, onMounted } from 'vue'

// API base URL
const apiBaseUrl = process.env.VUE_APP_API_URL

// True if loading under way
const isLoading = ref(true)

// Site base URL
const siteUrl = ref(null)

// Site title
const siteTitle = ref(null)

// OnMounted hook
onMounted(async () =>
{
    await OnLoad();
})

// Called when page is loaded
async function OnLoad()
{
    const siteInfo = await (await fetch(apiBaseUrl + `/api/SiteInfo/Url`)).json()
    siteUrl.value = siteInfo.siteUrl
    siteTitle.value = siteInfo.siteTitle
    isLoading.value = false
}

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div>
            <a class="vertical-align-center" :href="siteUrl" :title="siteTitle"><img src="/images/logo.png" :alt="siteTitle" /></a>
        </div>
    </div>
</template>
