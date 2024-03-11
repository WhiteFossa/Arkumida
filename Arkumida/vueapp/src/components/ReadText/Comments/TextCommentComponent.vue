<script setup>
    import {defineProps, onMounted, ref} from "vue";
    import moment from "moment";
    import {
        AvatarClass,
        ImagesPrefixes,
        Messages,
        TextRendererOperationModes
    } from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {RenderTextElement} from "@/js/libArkumida";
    import PopupUrledImage from "@/components/Shared/Popups/PopupUrledImage.vue";

    const isLoading = ref(true)

    const props = defineProps({
        comment: Object
    })

    const authorLink =
    {
        href: "/users/" + props.comment.author.entityId,
        name: props.comment.author.name,
        title: Messages.CreatureUser + props.comment.author.name
    }

    const renderedComment = ref("")

    const isExternalImagePopupShown = ref(false)
    const popupExternalImageUrl = ref("")

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        renderedComment.value = ""
        props.comment.messageParsed.forEach(e => renderedComment.value += RenderTextElement(e, TextRendererOperationModes.NonText))

        isLoading.value = false
    }

    // Handle clicks on rendered elements
    async function HandleClick(e)
    {
        const clickedElementId = e.target.id

        if (clickedElementId.startsWith(ImagesPrefixes.ExternalImage))
        {
            // We've got external image preview clicked
            await ShowExternalImagePopup(e.target.src)

            return
        }
    }

    async function ShowExternalImagePopup(imageUrl)
    {
        popupExternalImageUrl.value = imageUrl
        isExternalImagePopupShown.value = true
    }

    async function HideExternalImagePopup()
    {
        isExternalImagePopupShown.value = false
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />

    <div
        v-if="!isLoading"
        class="read-text-comments-comment-container">

        <!-- Author's avatar -->
        <div>
            <a
                class="read-text-comments-comment-author-link"
                :href="authorLink.href"
                :title="authorLink.title">
                <AvatarComponent :avatar="props.comment.author.currentAvatar" :avatarClass="AvatarClass.Small" />
            </a>
        </div>

        <div class="read-text-comment-comment-area">

            <!-- Author and post / edit times -->
            <div class="read-text-comments-info-line">

                <!-- Author -->
                <div class="read-text-comments-comment-author-name">
                    <a
                        class="read-text-comments-comment-author-link"
                        :href="authorLink.href"
                        :title="authorLink.title">{{ authorLink.name }}</a>
                </div>

                <!-- Post time (always visible) -->
                <div class="read-text-comments-comment-dates">
                    {{ moment(props.comment.postTime).format('HH:mm DD.MM.YYYY') }}
                    <span v-if="props.comment.postTime !== props.comment.lastUpdateTime">, отредактировано {{ moment(props.comment.lastUpdateTime).format('HH:mm DD.MM.YYYY') }}</span>
                </div>
            </div>

            <!-- Message itself -->
            <div v-html="renderedComment" @click="async (e) => await HandleClick(e)">
            </div>

            <PopupUrledImage v-if="isExternalImagePopupShown" :url="popupExternalImageUrl" @closePopup="async () => await HideExternalImagePopup()" />
        </div>

    </div>
</template>
