<!-- Shows central menu -->
<script setup>

    import AtomLink from "@/components/Shared/AtomLink.vue";
    import TelegramGroup from "@/components/Shared/TelegramGroup.vue";
    
    import { ref, onMounted } from 'vue'
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    
    // True if loading under way
    const isLoading = ref(true)
    
    // Menu items
    const menuItems = ref([])
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        menuItems.value = (await (await WebClientSendGetRequest("/api/MainMenu/Items")).json()).items
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <nav class="horizontal-flex flex-center">
            <div class="inline-block" v-for="menuItem in menuItems" :key="menuItem.url">
                <a class="central-menu-link" :href="menuItem.url" :title="menuItem.title">{{ menuItem.text }}</a>
            </div>
            
            <div class="central-menu-link-nohover">
                <AtomLink />
            </div>

            <div class="central-menu-link-nohover">
                <TelegramGroup />
            </div>
        </nav>
    </div>
</template>