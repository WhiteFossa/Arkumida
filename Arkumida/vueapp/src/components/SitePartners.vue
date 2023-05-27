<!-- Shows site partners list -->
<script setup>
import LoadingSymbol from './LoadingSymbol.vue'

import { ref, onMounted } from 'vue'

// API base URL
const apiBaseUrl = process.env.VUE_APP_API_URL

// True if loading under way
const isLoading = ref(true)

// Site partners
const partners = ref([])

// OnMounted hook
onMounted(async () =>
{
    await OnLoad();
})

// Called when page is loaded
async function OnLoad()
{
    partners.value = (await (await fetch(apiBaseUrl + `/api/SitePartners/Get`)).json()).partners
    isLoading.value = false
}

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div class="footer-block" v-for="partner in partners" :key="partner.id">
            <a :href="partner.url" :title="partner.title"><img :src="partner.bannerUrl" :alt="partner.bannerAlt" /></a>
        </div>
    </div>
</template>