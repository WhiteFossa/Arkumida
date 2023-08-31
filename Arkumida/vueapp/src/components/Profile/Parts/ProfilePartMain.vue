<script setup>

    import {onMounted, ref} from "vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";

    const isLoading = ref(true)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile

        isLoading.value = false
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />
    <div v-else>
        <div class="profile-main-part-creature-name">
            {{ creatureProfile.name }}
        </div>

        <div class="profile-main-part-avatar-container">
            <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Big" />
        </div>

        <div>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
        </div>
    </div>
</template>