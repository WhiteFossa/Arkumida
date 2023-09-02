<!-- Shows site partners list -->
<script setup>
import LoadingSymbol from '../Shared/LoadingSymbol.vue'

import { ref, onMounted } from 'vue'
import {WebClientSendGetRequest} from "@/js/libWebClient";

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
    partners.value = (await (await WebClientSendGetRequest("/api/SitePartners/Get")).json()).partners
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