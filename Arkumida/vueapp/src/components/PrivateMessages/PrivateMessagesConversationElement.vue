<script setup>
    import {defineProps, onMounted, ref} from "vue";
    import {AvatarClass} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import moment from "moment/moment";

    const props = defineProps({
        message: Object
    })

    const isLoading = ref(true)

    const senderProfile = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        senderProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + props.message.sender.entityId + "/Profile")).json()).creatureWithProfile

        isLoading.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        class="private-messages-message-outer-container">

        <div
            class="private-messages-message-top-line">
            {{ moment(props.message.sentTime).format('HH:mm DD.MM.YYYY') }}
        </div>

        <div class="private-messages-message-inner-container">
            <!-- Avatar -->
            <AvatarComponent :avatar="senderProfile.currentAvatar" :avatarClass="AvatarClass.Small" />

            <div>
                {{ props.message.content }}
            </div>
        </div>
    </div>
</template>