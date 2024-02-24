<script setup>
    import {defineProps, onMounted, ref} from "vue";
    import moment from "moment";
    import {AvatarClass, Messages, TextRendererOperationModes} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {RenderTextElement} from "@/js/libArkumida";

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
                    {{ moment(props.comment.postTime).format('HH:mm DD.MM.YYYY')  }}
                </div>
            </div>

            <!-- Message itself -->
            <div v-html="renderedComment">
            </div>

        </div>

    </div>
</template>
