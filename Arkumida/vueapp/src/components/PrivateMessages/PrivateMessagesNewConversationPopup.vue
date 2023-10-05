<script setup>

    import {defineEmits, onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";

    const emit = defineEmits([ "cancelPressed", "confidantSelected" ])

    const selectedConfidantId = ref(null)

    const newConversationFormData = reactive({
        confidantName: ""
    })

    const newConversationFormRules = {
        confidantName: {
            $autoDirty: true,
            required
        }
    }

    const newConversationFormValidator = useVuelidate(newConversationFormRules, newConversationFormData)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await newConversationFormValidator.value.$validate()
    }

    async function OnCancelPressed()
    {
        emit("cancelPressed")
    }

    async function OnConfidantSelected()
    {
        emit("confidantSelected", selectedConfidantId.value)
    }

    async function OnConfidantNameType()
    {
        console.log(newConversationFormData.confidantName)
    }
</script>

<template>
    <!-- Lower layer -->
    <div class="popup-lower-layer"></div>

    <!-- Upper layer -->
    <div class="popup-upper-layer">

        <div class="popup">

            <div class="popup-content-container">

                <div class="popup-title">
                    Выберите собеседника:
                </div>

                <div>

                    <input
                        type="text"
                        placeholder="Имя собеседника"
                        v-model="newConversationFormData.confidantName"
                        :class="(newConversationFormValidator.confidantName.$error)?'private-messages-new-conversation-confidant-name-input-invalid':'private-messages-new-conversation-confidant-name-input'"
                        @input="async () => await OnConfidantNameType()" />
                </div>

                <div class="popup-buttons-container">
                    <button
                        class="popup-button"
                        @click="async() => await OnCancelPressed()">
                        Отмена
                    </button>

                    <button
                        class="popup-button"
                        @click="async() => await OnConfidantSelected()"
                        :disabled="newConversationFormValidator.$errors.length > 0">
                        Начать
                    </button>
                </div>
            </div>

        </div>

    </div>
</template>
