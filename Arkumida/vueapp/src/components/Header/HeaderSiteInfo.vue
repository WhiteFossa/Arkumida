<!-- Shows site info for header -->
<script setup>
    import LoadingSymbol from '../Shared/LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'
    import {WebClientSendGetRequest} from "@/js/libWebClient";

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
        const siteInfo = await (await WebClientSendGetRequest("/api/SiteInfo/Url")).json()
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
            <a class="vertical-align-center" :href="siteUrl" :title="siteTitle"><img src="/images/logo.webp" :alt="siteTitle" /></a>
        </div>
    </div>
</template>
