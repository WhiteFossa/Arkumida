<script setup>

import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
import {AvatarClass, Messages} from "@/js/constants";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import {WebClientSendGetRequest} from "@/js/libWebClient";
import AvatarComponent from "@/components/Shared/AvatarComponent.vue";

    const props = defineProps({
        id: Guid,
        creatureRole: { type: String, default: "" }
    })

    const isLoading = ref(true)

    const creatureProfile = ref(null)

    const creatureLinkHref = ref(null)
    const creatureLinkTitle = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + props.id + "/Profile")).json()).creatureWithProfile

        creatureLinkHref.value = "/users/" + props.id
        creatureLinkTitle.value = Messages.CreatureUser + creatureProfile.value.name

        isLoading.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading" />

    <div class="creature-info-container" v-if="!isLoading">
        <div v-if="creatureRole !== ''" class="creature-info-creature-role">{{ props.creatureRole }}</div>

        <a :href="creatureLinkHref" :title="creatureLinkTitle">
            <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Small" />
        </a>

        <a class="creature-info-link" :href="creatureLinkHref" :title="creatureLinkTitle">{{ creatureProfile.name }}</a>
    </div>
</template>