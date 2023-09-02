<!-- Shows site info for footer -->
<script setup>
import LoadingSymbol from '../Shared/LoadingSymbol.vue'

import { ref, onMounted } from 'vue'

import TelegramGroup from "@/components/Shared/TelegramGroup.vue";
import OpdsLink from "@/components/Footer/OpdsLink.vue";
import AtomLink from "@/components/Shared/AtomLink.vue";
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
            <a :href="siteUrl" :title="siteTitle"><img class="footer-site-logo" src="/images/logo.png" :alt="siteTitle" /></a>
        </div>
        <div>
            Крупнейшая русскоязычная библиотека фурри рассказов. Любая тематика от йиффа до экшена. В базе большое количество текстов.
        </div>
        <div class="footer-block horizontal-flex footer-icons-gapped-flex flex-center">
            <AtomLink />
            <TelegramGroup />
            <OpdsLink />
        </div>
    </div>
</template>
