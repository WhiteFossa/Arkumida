<script setup>
    import {defineExpose, onBeforeUnmount, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import {PrivateMessagesConstants} from "@/js/constants";

    defineExpose({
        OnPrivateMessageMarkedAsRead
    })

    const isLoading = ref(true)

    const unreadInfo = ref(null)

    const unreadCountMessage = ref(null)
    const unreadCountMessageClass = ref(null)

    const unreadInfoPollingHandle = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    onBeforeUnmount(async () =>
    {
        // Stopping unread messages polling
        if (unreadInfoPollingHandle.value != null)
        {
            clearInterval(unreadInfoPollingHandle.value)
        }
    })

    async function OnLoad()
    {
        await GetUnreadMessagesCount()

        unreadInfoPollingHandle.value = setInterval (
            function()
            {
                GetUnreadMessagesCount()
            },
            PrivateMessagesConstants.UnreadPrivateMessagesInHeaderPollingInterval)

        isLoading.value = false
    }

    async function GetUnreadMessagesCount()
    {
        unreadInfo.value = await (await WebClientSendGetRequest("/api/PrivateMessages/UnreadInfo")).json()

        if (unreadInfo.value.unreadMessagesCount < 100)
        {
            unreadCountMessage.value = unreadInfo.value.unreadMessagesCount
        }
        else
        {
            unreadCountMessage.value = PrivateMessagesConstants.MaxUnreadPrivateMessagesCountMessage;
        }

        if (unreadInfo.value.unreadMessagesCount < 10)
        {
            unreadCountMessageClass.value = "private-messages-unread-icon-1-digit"
        }
        else if (unreadInfo.value.unreadMessagesCount >= 10 && unreadInfo.value.unreadMessagesCount < 100)
        {
            unreadCountMessageClass.value = "private-messages-unread-icon-2-digit"
        }
        else
        {
            unreadCountMessageClass.value = "private-messages-unread-icon-3-digit"
        }
    }

    // Call it when private message marked as read
    // eslint-disable-next-line no-unused-vars
    async function OnPrivateMessageMarkedAsRead(messageId)
    {
        await GetUnreadMessagesCount()
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-if="!isLoading">
        <div class="private-messages-icon-container">

            <div
                v-if="unreadInfo.unreadMessagesCount > 0"
                :class="unreadCountMessageClass">
                {{ unreadCountMessage }}
            </div>

            <a href="/privateMessages" title="Личные сообщения">
                <img class="private-messages-icon" src="/images/icons/icon_messages.webp" alt="Личные сообщения" />
            </a>

        </div>
    </div>

</template>