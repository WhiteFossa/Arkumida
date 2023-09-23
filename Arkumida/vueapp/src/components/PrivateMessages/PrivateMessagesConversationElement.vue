<script setup>
    import {defineProps, onMounted, ref} from "vue";
    import {AvatarClass, MarkPrivateMessageAsReadResult} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import moment from "moment/moment";

    const props = defineProps({
        message: Object
    })

    const isLoading = ref(true)

    const privateMessage = ref(props.message) // Dirty, but we need it to mutate the message (set read time)

    const currentCreature = ref(null)

    const senderProfile = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        currentCreature.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature
        senderProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + privateMessage.value.sender.entityId + "/Profile")).json()).creatureWithProfile

        if (privateMessage.value.readTime === null && currentCreature.value.entityId === privateMessage.value.receiver.entityId)
        {
            // Not our message, marking it as read
            const markingResult = (await (await WebClientSendPostRequest(
                "/api/PrivateMessages/MarkAsRead/" + privateMessage.value.id,
                {})).json()).result

            if (markingResult !== MarkPrivateMessageAsReadResult.Successful)
            {
                console.error("Failed to mark message " + privateMessage.value.id + " as read!")
            }
            else
            {
                privateMessage.value.readTime = new Date();
            }
        }

        isLoading.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        :class="(currentCreature.entityId === senderProfile.entityId)?'private-messages-message-outer-container-our-message':'private-messages-message-outer-container-not-our-message'">

        <div
            class="private-messages-message-top-line-container">
            <div>
                <img class="private-messages-sent-icon" src="/images/icons/icon_message_sent.png" alt="Отправлено" title="Отправлено" />
                {{ moment(privateMessage.sentTime).format('HH:mm DD.MM.YYYY') }}
            </div>

            <div>
                <div v-if="privateMessage.readTime === null">
                    <img class="private-messages-unread-icon" src="/images/icons/icon_message_unread.png" alt="Не прочитано" title="Не прочитано" />
                </div>
                <div v-else>
                    <img class="private-messages-read-icon" src="/images/icons/icon_message_read.png" alt="Прочитано" title="Прочитано" />
                    {{ moment(privateMessage.readTime).format('HH:mm DD.MM.YYYY') }}
                </div>

            </div>
        </div>

        <div class="private-messages-message-inner-container">
            <!-- Avatar -->
            <AvatarComponent :avatar="senderProfile.currentAvatar" :avatarClass="AvatarClass.Small" />

            <div>
                {{ privateMessage.content }}
            </div>
        </div>
    </div>
</template>