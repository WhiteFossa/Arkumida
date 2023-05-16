<!-- Shows site friends list -->
<script setup>
    import LoadingSymbol from './LoadingSymbol.vue'
    
    import { ref, onMounted } from 'vue'
    
    // True if loading under way
    const isLoading = ref(true)

    const friends = ref([
        { id: '1da68e2d-8e11-485f-a40e-78a4414e8792', text: 'Фосса', title: 'Блог Белой Фоссы', url: 'https://fossa.life' },
        { id: '1da68e2d-8e11-485f-a40e-78a4414e8793', text: 'Aaz', title: 'YiffYuff.RU', url: 'https://yiffyuff.ru' },
    ])
    
    // OnMounted hook
    onMounted(async () =>
    {
      await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
      //const versionInfo = await (await fetch(`api/VersionInfo/Get`)).json()
      //versionString.value = versionInfo.versionString
    
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
            <a :href="friend.url" :title="friend.title">{{ friend.text }}</a>
        </div>
    </div>
</template>