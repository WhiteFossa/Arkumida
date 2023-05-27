<!-- Shows right menu -->
<script setup>

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
        menuItems.value = (await (await fetch(apiBaseUrl + `/api/RightMenu/Items`)).json()).items
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <nav>
            <div class="inline-block" v-for="menuItem in menuItems" :key="menuItem.url">
                
                <a v-if="menuItem.imageUrl !== ''" class="right-menu-item" :href="menuItem.url" :title="menuItem.title">
                    <img :class="menuItem.imageClass" :src="menuItem.imageUrl" :alt="menuItem.imageAlt" />
                    {{ menuItem.text }}
                </a>
                <a v-else class="right-menu-item right-menu-item-no-image" :href="menuItem.url" :title="menuItem.title">
                    {{ menuItem.text }}
                </a>
            </div>

        </nav>
    </div>
</template>