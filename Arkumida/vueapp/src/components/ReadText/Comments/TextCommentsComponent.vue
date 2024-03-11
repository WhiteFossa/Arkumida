<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import {TextsCommentsConstants} from "@/js/constants";
    import TextCommentComponent from "@/components/ReadText/Comments/TextCommentComponent.vue";
    import {AuthIsCreatureLoggedIn} from "@/js/auth";
    import NewTextCommentComponent from "@/components/ReadText/Comments/NewTextCommentComponent.vue";


    const props = defineProps({
        textId: String
    })

    const isLoading = ref(true)

    const isCreatureLoggedIn = ref(false)

    const commentsTopicId = ref(null)
    const commentsTopicInfo = ref(null)
    const lastComments = ref([])
    const skippedCommentsCount = ref(0)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        // Is creature logged in?
        isCreatureLoggedIn.value = await AuthIsCreatureLoggedIn()

        await LoadComments()

        isLoading.value = false
    }

    async function LoadComments()
    {
        commentsTopicId.value = (await (await WebClientSendGetRequest("/api/Texts/" + props.textId + "/GetCommentsTopic")).json()).topicId

        if (commentsTopicId.value !== null)
        {
            commentsTopicInfo.value = (await (await WebClientSendGetRequest("/api/Forum/Topics/" + commentsTopicId.value + "/GetInfo")).json()).forumTopicInfo

            skippedCommentsCount.value = commentsTopicInfo.value.messagesCount - TextsCommentsConstants.CommentsCountToLoad
            if (skippedCommentsCount.value < 0)
            {
                skippedCommentsCount.value = 0;
            }

            lastComments.value = (await (await WebClientSendGetRequest("/api/Forum/Topics/" + commentsTopicId.value + "/Messages?skip=" + skippedCommentsCount.value + "&take=" + TextsCommentsConstants.CommentsCountToLoad)).json())
                .messages
            OrderTextComments(lastComments.value)
        }
    }

    // Order text comments (for now newest comments are on top, i.e. in first positions)
    function OrderTextComments(comments)
    {
        comments.sort(function(a, b)
        {
            return b.postTime.localeCompare(a.postTime);
        });
    }

    async function OnNewCommentPosted()
    {
        await LoadComments()
    }

</script>

<template>

    <LoadingSymbol v-if="isLoading" />

    <div v-if="!isLoading">

        <!-- Add new comment -->
        <NewTextCommentComponent
            v-if="isCreatureLoggedIn"
            :textId="props.textId"
            @newCommentPosted="async() => await OnNewCommentPosted()" />

        <!-- No comments yet message -->
        <div
            v-if="commentsTopicId === null"
            class="read-text-comments-no-comments-yet-message">
            К этому тексту ещё нет комментариев.
        </div>

        <!-- Comments list -->
        <div
            v-if="commentsTopicId !== null"
            class="read-text-comments-comments-list">

            <TextCommentComponent
                v-for="comment in lastComments" :key="comment.id"
                :comment="comment" />

        </div>

        <!-- More comments button -->
        <div
            v-if="skippedCommentsCount > 0"
            class="read-text-comments-more-comments-button-container">

            <a
                class="read-text-comments-more-comments-link"
                :href="'/forum/topics/' + commentsTopicId">

                <div class="read-text-comments-more-comments-button">
                    Ещё {{ skippedCommentsCount }} старых комментариев на форуме
                </div>

            </a>

        </div>
    </div>

</template>
