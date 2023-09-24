<script setup>

    import {onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    // Add this height to new message textarea when calculating its height. It is needed to avoid vertical scrollbar
    const newMessageTextareaHeightAdd = 5

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

        <div class="private-messages-new-message-send-button-container">
            <button
                class="button-with-image"
                type="button"
                title="Отправить сообщение"
                :disabled="newMessageFormValidator.$errors.length > 0">
                <img class="private-messages-send-icon" src="/images/icons/icon_send.png" alt="Отправить" title="Отправить" />
            </button>
        </div>

    </div>
</template>