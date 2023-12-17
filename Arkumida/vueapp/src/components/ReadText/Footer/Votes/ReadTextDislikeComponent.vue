<script setup>
    import {defineEmits, defineExpose, defineProps, onMounted, ref} from "vue";
import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    defineExpose({
        ReloadState
    })

    const props = defineProps({
        textId: String
    })

    const emit = defineEmits(["dislikeStateAboutToChange", "dislikeStateChanged"])

    const isLoading = ref(true)

    const isLikedData = ref(null)
    const isDislikedData = ref(null)

    const isDisliked = ref(false)
    const isActive = ref(false)

    const buttonClass = ref("")

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await UpdateButtonState()

        isLoading.value = false
    }

    async function UpdateButtonState()
    {
        isLikedData.value = await (await WebClientSendGetRequest("/api/TextsVotes/IsLiked/" + props.textId)).json()
        isDislikedData.value = await (await WebClientSendGetRequest("/api/TextsVotes/IsDisliked/" + props.textId)).json()

        isDisliked.value = isDislikedData.value.isDisliked
        isActive.value = !isLikedData.value.isLiked

        buttonClass.value = isDisliked.value ? "read-text-dislike-button-disliked" : "read-text-dislike-button-not-disliked"
        buttonClass.value += isActive.value ? " read-text-like-dislike-buttons-active" : " read-text-like-dislike-buttons-inactive"
    }

    async function ToggleDislikeAsync()
    {
        emit("dislikeStateAboutToChange")

        // Like/dislike may happen in another tab
        await UpdateButtonState()

        if (!isActive.value)
        {
            return
        }

        if (isDisliked.value)
        {
            await WebClientSendPostRequest(
                "/api/TextsVotes/Undislike/" + props.textId,
                {})
        }
        else
        {
            await WebClientSendPostRequest(
                "/api/TextsVotes/Dislike/" + props.textId,
                {})
        }

        await UpdateButtonState()

        emit("dislikeStateChanged", isDisliked.value)
    }

    async function ReloadState()
    {
        await UpdateButtonState()
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        :class="buttonClass"
        @click="async() => await ToggleDislikeAsync()">

        <div>
            <img class="read-text-like-dislike-image" src="/images/icons/icon_dislike.webp" alt="Произведение не нравится" />
        </div>

        <div>
            Не нравится
        </div>
    </div>
</template>