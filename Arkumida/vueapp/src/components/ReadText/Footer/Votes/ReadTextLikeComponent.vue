<script setup>
    import {defineEmits, defineExpose, defineProps, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";

    defineExpose({
        ReloadState
    })

    const props = defineProps({
        textId: String
    })

    const emit = defineEmits(["likeStateAboutToChange", "likeStateChanged"])

    const isLoading = ref(true)

    const isLikedData = ref(null)
    const isDislikedData = ref(null)

    const isLiked = ref(false)
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

        isLiked.value = isLikedData.value.isLiked
        isActive.value = !isDislikedData.value.isDisliked

        buttonClass.value = isLiked.value ? "read-text-like-button-liked" : "read-text-like-button-not-liked"
        buttonClass.value += isActive.value ? " read-text-like-dislike-buttons-active" : " read-text-like-dislike-buttons-inactive"
    }

    async function ToggleLikeAsync()
    {
        emit("likeStateAboutToChange")

        // Like/dislike may happen in another tab
        await UpdateButtonState()

        if (!isActive.value)
        {
            return
        }

        if (isLiked.value)
        {
            await WebClientSendPostRequest(
                "/api/TextsVotes/Unlike/" + props.textId,
                {})
        }
        else
        {
            await WebClientSendPostRequest(
            "/api/TextsVotes/Like/" + props.textId,
            {})
        }

        await UpdateButtonState()

        emit("likeStateChanged", isLiked.value)
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
        @click="async() => await ToggleLikeAsync()">

        <div>
            <img class="read-text-like-dislike-image" src="/images/icons/icon_like.webp" alt="Произведение нравится" />
        </div>

        <div>
            Нравится
        </div>
    </div>
</template>
