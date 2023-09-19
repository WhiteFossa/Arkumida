<script setup>
import {onMounted, ref} from "vue";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import {WebClientSendGetRequest} from "@/js/libWebClient";

    const isLoading = ref(true)

    const unreadInfo = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        unreadInfo.value = await (await WebClientSendGetRequest("/api/PrivateMessages/UnreadInfo")).json()

        isLoading.value = false
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-if="!isLoading">
        <div class="private-messages-icon-container">

            <div class="private-messages-unread-icon-3-digit">
                99+
            </div>

            <a href="/privateMessages" title="Личные сообщения">
                <img class="private-messages-icon" src="/images/icons/icon_messages.png" alt="Личные сообщения" />
            </a>

        </div>
    </div>

</template>