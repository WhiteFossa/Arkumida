<!-- Shows telegram group link -->
<script setup>
    import LoadingSymbol from './LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    // True if loading under way
    const isLoading = ref(true)

    // Group URL
    const groupUrl = ref(null)

    // Group title
    const groupTitle = ref(null)

    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })

    // Called when page is loaded
    async function OnLoad()
    {
        const groupInfo = await (await WebClientSendGetRequest("/api/SiteInfo/TelegramGroup")).json()
        groupUrl.value = groupInfo.groupUrl
        groupTitle.value = groupInfo.groupTitle
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
            <a :href="groupUrl" :title="groupTitle"><img class="telegram-group-logo" src="/images/telegram_logo.webp" :alt="groupTitle" /></a>
        </div>
    </div>
</template>