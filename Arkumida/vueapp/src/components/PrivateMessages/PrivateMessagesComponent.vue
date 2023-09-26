<script setup>

    import PrivateMessagesConversationSummary from "@/components/PrivateMessages/PrivateMessagesConversationSummary.vue";
    import {onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {AuthRedirectToLoginPageIfNotLoggedIn} from "@/js/auth";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import PrivateMessagesConversationElement
    from "@/components/PrivateMessages/PrivateMessagesConversationElement.vue";
    import PrivateMessagesNewMessageComponent
    from "@/components/PrivateMessages/PrivateMessagesNewMessageComponent.vue";

    const isLoading = ref(true)

    const conversationsSummaries = ref(null)
    const selectedConfidant = ref(null)

    const conversation = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await AuthRedirectToLoginPageIfNotLoggedIn()

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
        conversation.value = await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations/With/" + confidantId)).json()
    }

    async function ReloadConversation()
    {
        await LoadConversation(selectedConfidant.value.entityId)
    }

    async function LoadConversationSummaries()
    {
        conversationsSummaries.value = (await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations")).json()).conversationsSummaries
    }

    // TODO: Add more intellectual conversations summaries reload
    // Disabling the warning because we hope in future to make more intellectual conversations summaries reload
    // eslint-disable-next-line
    async function OnMessageMarkedAsRead(messageId)
    {
        await LoadConversationSummaries()
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

                <div class="private-messages-conversation-container">

                    <div v-if="conversation === null">
                        Выберите диалог
                    </div>

                    <div v-if="conversation !== null">

                        <!-- Conversation messages -->
                        <PrivateMessagesConversationElement
                            v-for="message in conversation.messages" :key="message"
                            :message="message"
                            @messageMarkedAsRead="async (mid) => await OnMessageMarkedAsRead(mid)"/>

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