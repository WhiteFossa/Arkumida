<script setup>

    import PrivateMessagesConversationSummary from "@/components/PrivateMessages/PrivateMessagesConversationSummary.vue";
    import {defineEmits, onBeforeUnmount, onMounted, onUpdated, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {AuthRedirectToLoginPageIfNotLoggedIn} from "@/js/auth";
    import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import PrivateMessagesConversationElement
    from "@/components/PrivateMessages/PrivateMessagesConversationElement.vue";
    import PrivateMessagesNewMessageComponent
    from "@/components/PrivateMessages/PrivateMessagesNewMessageComponent.vue";
    import {CommonConstants, MarkPrivateMessageAsReadResult, PrivateMessagesConstants} from "@/js/constants";
    import objectHash from "object-hash";
    import PrivateMessagesNewConversationPopup
        from "@/components/PrivateMessages/PrivateMessagesNewConversationPopup.vue";

    const emit = defineEmits(['messageWasMarkedAsRead'])

    const isLoading = ref(true)

    const conversationsSummaries = ref(null)
    const selectedConfidant = ref(null)

    const isPrivateMessagesScrollDownRequested = ref(false)
    const privateMessagesScrollContainer = ref(null)

    const privateMessagesCollection = ref([])

    const currentCreature = ref(null)

    const updateTimerHandle = ref(null)

    const isNewConversationPopupShown = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    onUpdated(async() =>
    {
        await ScrollPrivateMessagesDown()
    })

    onBeforeUnmount(async () =>
    {
        if (updateTimerHandle.value != null)
        {
            clearInterval(updateTimerHandle.value)
        }
    })

    async function OnLoad()
    {
        await AuthRedirectToLoginPageIfNotLoggedIn()

        currentCreature.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature

        await LoadConversationSummaries()

        updateTimerHandle.value = setInterval (
            function()
            {
                OnUpdateTimerTick()
            },
            PrivateMessagesConstants.PrivateMessagesPollingInterval)

        isLoading.value = false

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
        initialLoadBefore.setSeconds(initialLoadBefore.getSeconds() + 1) // To load message, which just arrived

        const initialMessages = (await
            (await WebClientSendGetRequest("/api/PrivateMessages/Conversations/With/" + confidantId + "/Before/" + initialLoadBefore.toISOString() + "/Limit/" + PrivateMessagesConstants.LoadBatchSize)).json())
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

    async function LoadConversationSummaries()
    {
        conversationsSummaries.value = (await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations")).json()).conversationsSummaries
        OrderConversationsSummariesToDisplay(conversationsSummaries.value)
    }

    // Order conversations summaries before displaying
    function OrderConversationsSummariesToDisplay(conversationsSummaries)
    {
        conversationsSummaries.sort(function(a, b)
        {
            return b.lastMessageSentTime.localeCompare(a.lastMessageSentTime);
        });
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

        const newMessages = (await
            (await WebClientSendGetRequest("/api/PrivateMessages/Conversations/With/" + selectedConfidant.value.entityId + "/Before/" + firstMessageSentTime + "/Limit/" + PrivateMessagesConstants.LoadBatchSize)).json())
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

            emit('messageWasMarkedAsRead', messageId)
        }
    }

    async function CheckForNewMessages()
    {
        const firstMessageTime = new Date(Math.min(...privateMessagesCollection.value.map(pm => (new Date(pm.sentTime)).getTime())));

        const loadMessagesAfterTime = firstMessageTime
        loadMessagesAfterTime.setSeconds(loadMessagesAfterTime.getSeconds() - 1) // To guarantee that first message in collection will be loaded too

        // Loading all existing messages (what if new message got somehow inserted inbetween / we need to process "message read" changes)
        const messagesAfter = (await
            (await WebClientSendGetRequest("/api/PrivateMessages/Conversations/With/" + selectedConfidant.value.entityId + "/After/" + loadMessagesAfterTime.toISOString() + "/Limit/" + CommonConstants.DotnetIntMaxValue)).json())
            .messages

        messagesAfter.forEach(serverMessage =>
        {
            // Do we have this message locally?
            var isExistsLocally = privateMessagesCollection.value.find((pm) => pm.id === serverMessage.id)

            if (isExistsLocally)
            {
                // Replacing old message with received from server ONLY IF IT CHANGED (any replace causes re-render)
                privateMessagesCollection.value = privateMessagesCollection.value.map(localMessage => {
                    if (localMessage.id === serverMessage.id && objectHash.sha1(localMessage) !== objectHash.sha1(serverMessage))
                    {
                        return serverMessage
                    }

                    return localMessage
                });
            }
            else
            {
                // New message, we need to insert it. We can just append, because re-ordering will be called a bit later
                privateMessagesCollection.value.push(serverMessage)

                isPrivateMessagesScrollDownRequested.value = true
            }
        })

        OrderPrivateMessagesToDisplay(privateMessagesCollection.value)
    }

    // Call this regularly to fetch new data from server
    async function OnUpdateTimerTick()
    {
        await LoadConversationSummaries()

        if (selectedConfidant.value !== null)
        {
            await CheckForNewMessages()
        }
    }

    async function OnNewMessageSent(newMessage)
    {
        privateMessagesCollection.value.push(newMessage)
        OrderPrivateMessagesToDisplay(privateMessagesCollection.value)

        await LoadConversationSummaries() // To display current conversation on top

        isPrivateMessagesScrollDownRequested.value = true
    }

    // Open a new conversation popup to determine confidant name
    async function OnNewConversationClicked()
    {
        isNewConversationPopupShown.value = true
    }

    async function OnNewConversationCreationCancelled()
    {
        isNewConversationPopupShown.value = false
    }

    async function OnNewConversationConfidantSelected(confidantId)
    {
        isNewConversationPopupShown.value = false

        alert("New conversation: " + confidantId)
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

                <div
                    class="private-messages-new-conversation-button"
                    @click="async() => await OnNewConversationClicked()">
                    <button
                        class="button-with-image"
                        type="button"
                        title="Начать новый диалог">
                        <img class="small-icon" src="/images/icons/icon_add.png" alt="Начать новый диалог" />
                    </button>
                </div>

                <div class="private-messages-conversations-summaries-container">

                    <PrivateMessagesConversationSummary
                        v-for="conversationSummary in conversationsSummaries" :key="conversationSummary"
                        :conversationSummary="conversationSummary"
                        :selectedConfidantId = "selectedConfidant?.entityId"
                        @openConversation="async (cid) => await OpenConversation(cid)"/>
                </div>

            </div>

            <div class="private-messages-conversation-and-new-message-container">

                <div
                    class="private-messages-conversation-container"
                    ref="privateMessagesScrollContainer">

                    <div
                        v-if="selectedConfidant === null"
                        class="private-messages-select-conversation-container">
                        Выберите диалог или начните новый
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
                    v-if="selectedConfidant !== null"
                    :selectedConfidantId="selectedConfidant?.entityId"
                    @newMessageSent="async(nm) => await OnNewMessageSent(nm)" />

            </div>
        </div>
    </div>

    <PrivateMessagesNewConversationPopup
        v-if="isNewConversationPopupShown"
        @cancelPressed="async() => await OnNewConversationCreationCancelled()"
        @confidantSelected="async(cid) => await OnNewConversationConfidantSelected(cid)" />
</template>