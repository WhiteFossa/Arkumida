<script setup>
import {defineProps, onMounted, ref} from "vue";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import {WebClientSendGetRequest} from "@/js/libWebClient";
import VotesHistoryItemComponent from "@/components/VotesHistory/VotesHistoryItemComponent.vue";

    const props = defineProps({
        textId: String
    })

    const isLoading = ref(true)

    const creatureId = ref(null)

    const criticsSettings = ref(null)

    const votes = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId

        criticsSettings.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Critics/GetSettings")).json()).criticsSettings

        votes.value = (await (await WebClientSendGetRequest("/api/TextsVotes/VotesList/" + props.textId)).json()).textVotes

        isLoading.value = false
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />

    <div v-if="!isLoading">
        <div class="votes-history-title">
            История голосов
        </div>

        <div
            v-if="!criticsSettings.isShowDislikes"
            class="votes-history-warning">
            <strong>Дизлайки скрыты</strong> (<a class="darkest-color1-link" href="/profile/texts#critics" target="_blank">настройки</a>)
        </div>

        <div
            v-if="criticsSettings.isShowDislikes && !criticsSettings.isShowDislikesAuthors"
            class="votes-history-warning">
            <strong>Авторы дизлайков скрыты</strong> (<a class="darkest-color1-link" href="/profile/texts#critics" target="_blank">настройки</a>)
        </div>

        <!-- Votes -->
        <VotesHistoryItemComponent
            v-for="vote in votes"
            :key="vote.id"
            :vote="vote" />

    </div>
</template>
