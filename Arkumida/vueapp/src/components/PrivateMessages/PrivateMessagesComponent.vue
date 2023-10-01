<script setup>

    import PrivateMessagesConversationSummary from "@/components/PrivateMessages/PrivateMessagesConversationSummary.vue";
    import {onMounted, onUpdated, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {AuthRedirectToLoginPageIfNotLoggedIn} from "@/js/auth";
    import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import PrivateMessagesConversationElement
    from "@/components/PrivateMessages/PrivateMessagesConversationElement.vue";
    import PrivateMessagesNewMessageComponent
    from "@/components/PrivateMessages/PrivateMessagesNewMessageComponent.vue";
    import {MarkPrivateMessageAsReadResult} from "@/js/constants";

    // Load this amount of private messages at once
    const loadBlockSize = 15

    const isLoading = ref(true)

    const conversationsSummaries = ref(null)
    const selectedConfidant = ref(null)

    const isPrivateMessagesScrollDownRequested = ref(false)
    const privateMessagesScrollContainer = ref(null)

    const privateMessagesCollection = ref([])

    const currentCreature = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    onUpdated(async() =>
    {
        await ScrollPrivateMessagesDown()
    })

    async function OnLoad()
    {
        await AuthRedirectToLoginPageIfNotLoggedIn()

        currentCreature.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature

        await LoadConversationSummaries()

        isLoading.value = false
    }

    async function OpenConversation(confidantId)
    {
        selectedConfidant.value = (await (await WebClientSendGetRequest("/api/Users/" + confidantId + "/Profile")).json()).creatureWithProfile

        await LoadConversation(selectedConfidant.value.entityId)
    }

    async function LoadConversation(confidantId)
    {
        // Initializing messages collection with last N messages
        privateMessagesCollection.value = []

        const initialLoadBefore = new Date();
        initialLoadBefore.setSeconds(initialLoadBefore.getSeconds() + 1); // To load message, which just arrived

        const initialMessages = (await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations/With/" + confidantId + "/Before/" + initialLoadBefore.toISOString() + "/Limit/" + loadBlockSize)).json())
            .messages

        privateMessagesCollection.value = privateMessagesCollection.value.concat(initialMessages)
        OrderPrivateMessagesToDisplay(privateMessagesCollection.value)

        isPrivateMessagesScrollDownRequested.value = true
    }

    // Order private messages before displaying
    function OrderPrivateMessagesToDisplay(privateMessages)
    {
        privateMessages.sort(function(a, b)
            {
                return a.sentTime.localeCompare(b.sentTime);
            });
    }

    async function ReloadConversation()
    {
        await LoadConversation(selectedConfidant.value.entityId)
    }

    async function LoadConversationSummaries()
    {
        conversationsSummaries.value = (await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations")).json()).conversationsSummaries
    }

    async function OnMessageBecameVisible(messageId)
    {
        if (messageId === privateMessagesCollection.value[0].id)
        {
            await OnFirstMessageBecameVisible()
        }

        await MarkMessageAsReadIfNeeded(messageId)
    }

    async function OnFirstMessageBecameVisible()
    {
        if (selectedConfidant.value === null)
        {
            // No conversation selected
            return
        }

        const firstMessageSentTime = privateMessagesCollection.value[0].sentTime

        const newMessages = (await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations/With/" + selectedConfidant.value.entityId + "/Before/" + firstMessageSentTime + "/Limit/" + loadBlockSize)).json())
            .messages

        privateMessagesCollection.value = privateMessagesCollection.value.concat(newMessages)
        OrderPrivateMessagesToDisplay(privateMessagesCollection.value)
    }

    async function ScrollPrivateMessagesDown()
    {
        if (privateMessagesScrollContainer.value !== null && isPrivateMessagesScrollDownRequested.value)
        {
            privateMessagesScrollContainer.value.scrollTop = privateMessagesScrollContainer.value.scrollHeight

            isPrivateMessagesScrollDownRequested.value = false
        }
    }

    async function MarkMessageAsReadIfNeeded(messageId)
    {
        let message = privateMessagesCollection
            .value
            .filter(function (pm) { return pm.id === messageId })[0]

        if (message.readTime === null && currentCreature.value.entityId === message.receiver.entityId)
        {
            const markingResult = (await (await WebClientSendPostRequest(
                "/api/PrivateMessages/MarkAsRead/" + message.id,
                {})).json())

            if (markingResult.result !== MarkPrivateMessageAsReadResult.Successful)
            {
                console.error("Failed to mark message " + message.id + " as read!")
                return
            }

            message.readTime = markingResult.markTime

            // Kinda dirty - reloading all conversations summaries to display new (lessened) number of unread messages
            await LoadConversationSummaries()
        }
    }
</script>

<template>

    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        class="body-container">
        <div class="private-messages-title">
            Личные сообщения
        </div>

        <div class="private-messages-top-level-container">

            <!-- Conversations container -->
            <div class="private-messages-conversations-list-container" :key="selectedConfidant">

                <PrivateMessagesConversationSummary
                    v-for="conversationSummary in conversationsSummaries" :key="conversationSummary"
                    :conversationSummary="conversationSummary"
                    :selectedConfidantId = "selectedConfidant?.entityId"
                    @openConversation="async (cid) => await OpenConversation(cid)"/>

            </div>

            <div class="private-messages-conversation-and-new-message-container">

                <div
                    class="private-messages-conversation-container"
                    ref="privateMessagesScrollContainer">

                    <div v-if="selectedConfidant === null">
                        Выберите диалог
                    </div>

                    <div
                        v-if="selectedConfidant !== null">

                        <!-- Conversation messages -->
                        <PrivateMessagesConversationElement
                            v-for="message in privateMessagesCollection" :key="message"
                            :message="message"
                            @messageBecameVisible="async (mid) => await OnMessageBecameVisible(mid)"/>
                    </div>

                </div>

                <!-- New message field -->
                <PrivateMessagesNewMessageComponent
                    :selectedConfidantId="selectedConfidant?.entityId"
                    @newMessageSent="async() => await ReloadConversation()" />

            </div>
        </div>
    </div>

</template>