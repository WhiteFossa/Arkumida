<script setup>

import {AvatarClass, PrivateMessagesConstants} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
import {defineEmits, defineProps, onMounted, ref} from "vue";
    import moment from 'moment';
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    const props = defineProps({
        conversationSummary: Object,
        selectedConfidantId: String
    })

    const emit = defineEmits(['openConversation'])

    const isLoading = ref(true)

    const unreadCountMessage = ref(null)
    const unreadCountMessageClass = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        if (props.conversationSummary.unreadMessagesCount < 100)
        {
            unreadCountMessage.value = props.conversationSummary.unreadMessagesCount
        }
        else
        {
            unreadCountMessage.value = PrivateMessagesConstants.MaxUnreadPrivateMessagesCountMessage;
        }

        if (props.conversationSummary.unreadMessagesCount < 10)
        {
            unreadCountMessageClass.value = "private-messages-conversation-summary-unread-icon-1-digit"
        }
        else if (props.conversationSummary.unreadMessagesCount >= 10 && props.conversationSummary.unreadMessagesCount < 100)
        {
            unreadCountMessageClass.value = "private-messages-conversation-summary-unread-icon-2-digit"
        }
        else
        {
            unreadCountMessageClass.value = "private-messages-conversation-summary-unread-icon-3-digit"
        }

        isLoading.value = false
    }

    async function OpenConversation(confidantId)
    {
        emit('openConversation', confidantId)
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div
        v-if="!isLoading"
        :class="(selectedConfidantId === conversationSummary.confidant.entityId)?'private-messages-conversation-summary-container-selected':'private-messages-conversation-summary-container'"
        @click="async () => await OpenConversation(props.conversationSummary.confidant.entityId)">

        <!-- Avatar -->
        <AvatarComponent :avatar="props.conversationSummary.confidant.currentAvatar" :avatarClass="AvatarClass.Small" />

        <!-- Info part -->
        <div class="private-messages-conversation-summary-info-part">

            <!-- Creature name -->
            <div class="private-messages-conversation-summary-creature-name">
                {{ props.conversationSummary.confidant.name }}
            </div>

            <!-- Last message time -->
            <div
                :class="(selectedConfidantId === conversationSummary.confidant.entityId)?'private-messages-conversation-summary-last-message-time-selected':'private-messages-conversation-summary-last-message-time'">
                {{ moment(props.conversationSummary.lastMessageSentTime).format('HH:mm DD.MM.YYYY') }}
            </div>

        </div>

        <!-- Unread messages count -->
        <div
            v-if="props.conversationSummary.unreadMessagesCount > 0"
            :class="unreadCountMessageClass">

            {{ unreadCountMessage }}

        </div>
    </div>
</template>