<!-- Logout link (inline) -->
<script setup>
    import {onMounted, ref} from "vue";
    import {AuthIsUserLoggedIn, AuthLogUserOut} from "@/js/auth";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    const isLoading = ref(true)

    const isUserLoggedIn = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        isUserLoggedIn.value = await AuthIsUserLoggedIn()
        isLoading.value = false
    }

    async function LogOut()
    {
        await AuthLogUserOut()
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-if="!isLoading" class="logout-pseudolink" @click="async () => await LogOut()">
        [Выйти]
    </div>
</template>