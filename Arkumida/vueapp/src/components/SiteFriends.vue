<!-- Shows site friends list -->
<script setup>
    import LoadingSymbol from './LoadingSymbol.vue'
    
    import { ref, onMounted } from 'vue'
    
    // True if loading under way
    const isLoading = ref(true)

    // Site friends
    const friends = ref([])
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        friends.value = (await (await fetch(`api/SiteFriends/Get`)).json()).friends
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div class="footer-block" v-for="friend in friends" :key="friend.id">
            <a :href="friend.url" :title="friend.title">{{ friend.name }}</a>
        </div>
    </div>
</template>