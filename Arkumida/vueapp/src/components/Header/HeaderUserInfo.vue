<script setup>
    import {onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import {AuthIsUserLoggedIn} from "@/js/auth";
    import InlineLogoutComponent from "@/components/Login/InlineLogoutComponent.vue";

    const isLoading = ref(true)

    const isUserLoggedIn = ref(false)

    const userData = ref(null)

    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        isUserLoggedIn.value = await AuthIsUserLoggedIn()

        if (isUserLoggedIn.value)
        {
            userData.value = await (await WebClientSendGetRequest("/api/Users/Current")).json()
        }

        isLoading.value = false
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-if="!isLoading">

        <div v-if="!isUserLoggedIn">
            <a class="login-link" href="/login" title="Войти на сайт">Войти</a>
        </div>

        <div v-if="isUserLoggedIn">
            <div class="inline-block">
                {{ userData.creature.name }}
            </div>

            <span class="spacer"></span>
            
            <InlineLogoutComponent />
        </div>

    </div>

</template>
