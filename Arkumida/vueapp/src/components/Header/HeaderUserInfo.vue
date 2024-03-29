<script setup>
    import {defineExpose, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import {AuthIsCreatureLoggedIn} from "@/js/auth";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";
import PrivateMessagesHeaderLink from "@/components/PrivateMessages/PrivateMessagesHeaderLink.vue";

    defineExpose({
        ReloadProfile,
        OnPrivateMessageMarkedAsRead
    })

    const isLoading = ref(true)

    const isUserLoggedIn = ref(false)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

    const privateMessagesLink = ref(null)

    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        isUserLoggedIn.value = await AuthIsCreatureLoggedIn()

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

    // Call it when private message marked as read
    async function OnPrivateMessageMarkedAsRead(messageId)
    {
        privateMessagesLink.value.OnPrivateMessageMarkedAsRead(messageId)
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-if="!isLoading">

        <!-- User isn't logged in -->
        <div v-if="!isUserLoggedIn">
            <a class="darkest-color1-link-without-underline" href="/login" title="Войти на сайт">Войти</a>
            /
            <a class="darkest-color1-link-without-underline" href="/register" title="Зарегистрироваться">Зарегистрироваться</a>
        </div>

        <!-- User is logged in -->
        <div
            class="header-user-info-container"
            v-if="isUserLoggedIn">

            <!-- Private messages -->
            <PrivateMessagesHeaderLink ref="privateMessagesLink" />

            <!-- Profile link -->
            <a class="darkest-color1-link-without-underline" href="/profile" title="Профиль">
                <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Small" :key="creatureProfile.currentAvatar"/>
            </a>

            <span class="spacer"></span>

            <a class="darkest-color1-link-without-underline" href="/profile" title="Профиль">{{ creatureProfile.name }}</a>
        </div>

    </div>

</template>
