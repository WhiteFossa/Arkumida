<!-- Shows site info for footer -->
<script setup>
import LoadingSymbol from './LoadingSymbol.vue'

import { ref, onMounted } from 'vue'

// True if loading under way
const isLoading = ref(true)

// Site base URL
const siteUrl = ref(null)

const siteTitle = ref(null)

// OnMounted hook
onMounted(async () =>
{
    await OnLoad();
})

// Called when page is loaded
async function OnLoad()
{
    const siteInfo = await (await fetch(`api/SiteInfo/Url`)).json()
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
        <div class="footer-info-block">
            Крупнейшая русскоязычная библиотека фурри рассказов. Любая тематика от йиффа до экшена. В базе большое количество текстов.
        </div>
    </div>
</template>
