<!-- Shows central menu -->
<script setup>

    import AtomLink from "@/components/AtomLink.vue";
    import TelegramGroup from "@/components/TelegramGroup.vue";
    
    import { ref, onMounted } from 'vue'
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
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
        menuItems.value = (await (await fetch(apiBaseUrl + `/api/MainMenu/Items`)).json()).items
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <nav>
            <span v-for="menuItem in menuItems" :key="menuItem.url">
                <a class="central-menu-link" :href="menuItem.url" :title="menuItem.title">{{ menuItem.text }}</a>
            </span>
            
            <div class="central-menu-link-nohover">
                <AtomLink />
            </div>

            <div class="central-menu-link-nohover">
                <TelegramGroup />
            </div>
        </nav>
    </div>
</template>