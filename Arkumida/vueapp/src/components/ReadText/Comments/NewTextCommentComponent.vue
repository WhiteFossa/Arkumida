<script setup>
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {defineEmits, defineProps, onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {IsCtrlEnterKeyupEvent} from "@/js/libArkumida";
    import {WebClientSendPostRequest} from "@/js/libWebClient";
    import {Messages} from "@/js/constants";

    const props = defineProps({
        textId: String
    })

    const emit = defineEmits(['newCommentPosted'])

    // Add this height to new comment textarea when calculating its height. It is needed to avoid vertical scrollbar
    const newCommentTextareaHeightAdd = 10

    const isLoading = ref(true)

    const newCommentTextarea = ref(null)
    const isNewCommentBeingSent = ref(false)

    const newCommentFormData = reactive({
        comment: ""
    })

    const newCommentFormRules = {
        comment: {
            $autoDirty: true,
            required
        }
    }

    const newCommentFormValidator = useVuelidate(newCommentFormRules, newCommentFormData)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await newCommentFormValidator.value.$validate()

        isLoading.value = false
    }

    async function OnNewCommentTextareaContentChange()
    {
        // Resizing textarea vertically
        newCommentTextarea.value.style.height = "auto"
        newCommentTextarea.value.style.height = `${newCommentTextarea.value.scrollHeight + newCommentTextareaHeightAdd}px`
    }

    async function OnNewCommentTextareaKeyup(e)
    {
        if (await IsCtrlEnterKeyupEvent(e))
        {
            await PostNewComment()
        }
    }

    async function PostNewComment()
    {
        isNewCommentBeingSent.value = true

        const sendCommentResponse = await WebClientSendPostRequest(
            "/api/Texts/" + props.textId + "/AddComment",
            {
                "message": {
                    "message": newCommentFormData.comment
                },
            })

        if (!sendCommentResponse.ok)
        {
            isNewCommentBeingSent.value = false
            alert(Messages.TextCommentFailedToSend)
            return
        }

        newCommentFormData.comment = ""

        emit("newCommentPosted")

        isNewCommentBeingSent.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        class="read-text-comments-new-comment-container">

        <!-- Comment text -->
        <div class="read-text-comments-new-comment-textarea-container">
            <textarea
                ref="newCommentTextarea"
                :class="(newCommentFormValidator.comment.$error)?'read-text-comments-new-comment-textarea-invalid':'read-text-comments-new-comment-textarea'"
                placeholder="Введите комментарий здесь"
                v-model="newCommentFormData.comment"
                @keyup="async(e) => await OnNewCommentTextareaKeyup(e)"
                @input="async () => await OnNewCommentTextareaContentChange()"
            />
        </div>

        <!-- Send button -->
        <div class="read-text-comments-new-comment-send-button-container">

            <div v-if="isNewCommentBeingSent">
                <LoadingSymbol />
            </div>

            <div v-if="!isNewCommentBeingSent">

                <button
                    class="button-with-image"
                    type="button"
                    title="Отправить комментарий"
                    :disabled="newCommentFormValidator.$errors.length > 0"
                    @click="async() => await PostNewComment()">
                    <img class="read-text-comments-send-new-comment-icon" src="/images/icons/icon_send.webp" alt="Отправить" title="Отправить" />
                </button>

            </div>

        </div>

    </div>
</template>

