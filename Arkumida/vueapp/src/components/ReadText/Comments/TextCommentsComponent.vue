<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import {TextsCommentsConstants} from "@/js/constants";
    import TextCommentComponent from "@/components/ReadText/Comments/TextCommentComponent.vue";


    const props = defineProps({
        textId: String
    })

    const isLoading = ref(true)

    const commentsTopicId = ref(null)
    const commentsTopicInfo = ref(null)
    const lastComments = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        // Getting comments topic and comments in it (if exist)
        commentsTopicId.value = (await (await WebClientSendGetRequest("/api/Texts/" + props.textId + "/GetCommentsTopic")).json()).topicId

        if (commentsTopicId.value !== null)
        {
            commentsTopicInfo.value = (await (await WebClientSendGetRequest("/api/Forum/Topics/" + commentsTopicId.value + "/GetInfo")).json()).forumTopicInfo

            let skipComments = commentsTopicInfo.value.messagesCount - TextsCommentsConstants.CommentsCountToLoad
            if (skipComments < 0)
            {
                skipComments = 0;
            }

            lastComments.value = (await (await WebClientSendGetRequest("/api/Forum/Topics/" + commentsTopicId.value + "/Messages?skip=" + skipComments + "&take=" + TextsCommentsConstants.CommentsCountToLoad)).json())
                .messages
            OrderTextComments(lastComments.value)
        }

        isLoading.value = false
    }

    // Order text comments (for now newest comments are on top, i.e. in first positions)
    function OrderTextComments(comments)
    {
        comments.sort(function(a, b)
        {
            return b.postTime.localeCompare(a.postTime);
        });
    }

</script>

<template>

    <LoadingSymbol v-if="isLoading" />

    <div v-if="!isLoading">

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
    </div>

</template>
