<script setup>

import {defineEmits, defineProps, onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendPostRequest} from "@/js/libWebClient";
    import {Messages} from "@/js/constants";

    // Add this height to new message textarea when calculating its height. It is needed to avoid vertical scrollbar
    const newMessageTextareaHeightAdd = 5

    const props = defineProps({
        selectedConfidantId: String
    })

    const emit = defineEmits(['newMessageSent'])

    const isLoading = ref(true)

    const newMessageFormData = reactive({
        message: ""
    })

    const newMessageFormRules = {
        message: {
            $autoDirty: true,
            required
        }
    }

    const newMessageFormValidator = useVuelidate(newMessageFormRules, newMessageFormData)

    const newMessageTextarea = ref(null)

    const isNewMessageBeingSent = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await newMessageFormValidator.value.$validate()

        isLoading.value = false
    }

    async function OnNewMessageTextareaContentChange()
    {
        // Resizing textarea vertically
        newMessageTextarea.value.style.height = "auto"
        newMessageTextarea.value.style.height = `${newMessageTextarea.value.scrollHeight + newMessageTextareaHeightAdd}px`
    }

    async function SendNewMessage()
    {
        isNewMessageBeingSent.value = true

        const sendMessageResult = (await (await WebClientSendPostRequest(
            "/api/PrivateMessages/SendTo/" + props.selectedConfidantId,
            {
                "message": newMessageFormData.message,
            })).json())

        if (!sendMessageResult.isSuccessful)
        {
            isNewMessageBeingSent.value = false
            alert(Messages.PrivateMessagesFailedToSend)
            return
        }

        newMessageFormData.message = ""

        emit('newMessageSent', sendMessageResult.message)
        isNewMessageBeingSent.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        class="private-messages-new-message-container">

        <div class="private-messages-new-message-textarea-container">
            <textarea
                ref="newMessageTextarea"
                :class="(newMessageFormValidator.message.$error)?'private-messages-new-message-textarea-invalid':'private-messages-new-message-textarea'"
                placeholder="Введите новое сообщение здесь"
                v-model="newMessageFormData.message"
                @input="async () => await OnNewMessageTextareaContentChange()"/>
        </div>

        <!-- Send new message button (or loading symbol if message being sent) -->
        <div class="private-messages-new-message-send-button-container">

            <div v-if="isNewMessageBeingSent">
                <LoadingSymbol />
            </div>

            <div v-if="!isNewMessageBeingSent">
                <button
                    class="button-with-image"
                    type="button"
                    title="Отправить сообщение"
                    :disabled="newMessageFormValidator.$errors.length > 0"
                    @click="async() => await SendNewMessage()">
                    <img class="private-messages-send-icon" src="/images/icons/icon_send.png" alt="Отправить" title="Отправить" />
                </button>
            </div>
        </div>

    </div>
</template>