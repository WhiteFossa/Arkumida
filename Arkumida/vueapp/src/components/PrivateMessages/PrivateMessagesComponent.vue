<script setup>

    import PrivateMessagesConversationSummary from "@/components/PrivateMessages/PrivateMessagesConversationSummary.vue";
    import {onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {AuthRedirectToLoginPageIfNotLoggedIn} from "@/js/auth";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const isLoading = ref(true)

    const conversationsSummaries = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await AuthRedirectToLoginPageIfNotLoggedIn()

        conversationsSummaries.value = (await (await WebClientSendGetRequest("/api/PrivateMessages/Conversations")).json()).conversationsSummaries

        isLoading.value = false
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
            <div class="private-messages-conversations-list-container">

                <PrivateMessagesConversationSummary
                    v-for="conversationSummary in conversationsSummaries" :key="conversationSummary"
                    :conversationSummary="conversationSummary" />

            </div>

            <div class="private-messages-conversation-container">
                Тут будет диалог
            </div>
        </div>
    </div>

</template>