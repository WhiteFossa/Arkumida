<script setup>
import {defineExpose, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import {AuthIsUserLoggedIn} from "@/js/auth";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";

    defineExpose({
        ReloadProfile
    })

    const isLoading = ref(true)

    const isUserLoggedIn = ref(false)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

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
            creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId
            await LoadProfile()
        }

        isLoading.value = false
    }

    // Call it to reload profile
    async function ReloadProfile()
    {
        await LoadProfile()
    }

    async function LoadProfile()
    {
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-if="!isLoading">

        <!-- User isn't logged in -->
        <div v-if="!isUserLoggedIn">
            <a class="login-link" href="/login" title="Войти на сайт">Войти</a>
        </div>

        <!-- User is logged in -->
        <div v-if="isUserLoggedIn">

            <a class="creature-profile-link" href="/profile" title="Профиль">
                <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Small" :key="creatureProfile.currentAvatar"/>
            </a>

            <span class="spacer"></span>

            <a class="creature-profile-link" href="/profile" title="Профиль">{{ creatureProfile.name }}</a>
        </div>

    </div>

</template>
