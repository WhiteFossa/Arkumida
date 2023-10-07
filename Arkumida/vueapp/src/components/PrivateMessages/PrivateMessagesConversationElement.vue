<script setup>
    import {defineEmits, defineProps, ref} from "vue";
    import {AvatarClass} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import moment from "moment/moment";
    import { vElementVisibility } from '@vueuse/components'

    const props = defineProps({
        message: Object
    })

    const emit = defineEmits(['messageBecameVisible'])

    const isLoading = ref(true)

    const privateMessage = ref(props.message) // Dirty, but we need it to mutate the message (set read time)

    const currentCreature = ref(null)

    const senderProfile = ref(null)

    async function ReportThatMessageBecameVisible(messageId)
    {
        emit('messageBecameVisible', messageId)
    }

    async function OnPrivateMessageVisibility(isVisible)
    {
        if (isVisible)
        {
            await LoadPrivateMessage()

            ReportThatMessageBecameVisible(props.message.id)
        }
    }

    async function LoadPrivateMessage()
    {
        currentCreature.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature
        senderProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + privateMessage.value.sender.entityId + "/Profile")).json()).creatureWithProfile

        isLoading.value = false
    }

</script>

<template>
    <div
        :class="isLoading?'private-messages-message-visibility-container-not-loaded':'private-messages-message-visibility-container-loaded'"
        v-element-visibility="async (v) => await OnPrivateMessageVisibility(v)">
        
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
    </div>
</template>