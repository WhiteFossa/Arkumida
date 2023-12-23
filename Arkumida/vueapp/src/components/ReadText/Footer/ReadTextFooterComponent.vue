<script setup>
    import ReadTextLikeComponent from "@/components/ReadText/Footer/Votes/ReadTextLikeComponent.vue";
    import ReadTextDislikeComponent from "@/components/ReadText/Footer/Votes/ReadTextDislikeComponent.vue";
    import {defineProps, onMounted, ref} from "vue";
    import ReadTextVotesHistoryComponent from "@/components/ReadText/Footer/Votes/ReadTextVotesHistoryComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const props = defineProps({
        textId: String
    })

    const likeComponent = ref(null);
    const dislikeComponent = ref(null);

    const isLoading = ref(true)

    const isVotesHistoryVisible = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        isVotesHistoryVisible.value = (await (await WebClientSendGetRequest("/api/TextsVotes/IsHistoryVisible/" + props.textId)).json()).isVotesHistoryVisible

        isLoading.value = false
    }


    async function UpdateDislikeStateAsync()
    {
        dislikeComponent.value.ReloadState()
    }

    async function UpdateLikeStateAsync()
    {
        likeComponent.value.ReloadState()
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />

    <div
        v-if="!isLoading"
        class="read-text-footer">

        <div class="read-text-votes-container">

            <!-- Like -->
            <ReadTextLikeComponent
                ref="likeComponent"
                :textId="props.textId"
                @likeStateAboutToChange="async() => await UpdateDislikeStateAsync()"
                @likeStateChanged="async() => await UpdateDislikeStateAsync()"/>

            <!-- Dislike -->
            <ReadTextDislikeComponent
                ref="dislikeComponent"
                :textId="props.textId"
                @dislikeStateAboutToChange="async() => await UpdateLikeStateAsync()"
                @dislikeStateChanged="async() => await UpdateLikeStateAsync()"/>

            <!-- Votes history -->
            <ReadTextVotesHistoryComponent
                v-if="isVotesHistoryVisible"
                :textId="props.textId" />

        </div>
    </div>
</template>