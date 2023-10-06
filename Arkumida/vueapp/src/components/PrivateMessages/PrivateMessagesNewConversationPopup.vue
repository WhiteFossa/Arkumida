<script setup>

    import {defineEmits, onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

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

    const foundConfidantsList = ref([])

    const newConversationFormValidator = useVuelidate(newConversationFormRules, newConversationFormData)

    const lastConfidantsListRequestId = ref(null)

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
        lastConfidantsListRequestId.value = crypto.randomUUID();

        if (newConversationFormData.confidantName === "")
        {
            foundConfidantsList.value = []
            return
        }

        DoPotentialConfidantsLookup(newConversationFormData.confidantName, lastConfidantsListRequestId.value)
            .then((result) => {
                // This code will be executed on request completion, co lastConfidantsListRequestId may change
                if (result.requestId !== lastConfidantsListRequestId.value)
                {
                    // Discarding the result because new request was made
                    return
                }

                foundConfidantsList.value = result.result
            })
    }

    async function DoPotentialConfidantsLookup(partOfName, requestId)
    {
        return {
            "result": (await (await WebClientSendGetRequest("/api/Users/Find/ByDisplayName/" + newConversationFormData.confidantName)).json()).creatures,
            "requestId": requestId
        }
    }
</script>

<template>
    <!-- Lower layer -->
    <div class="popup-lower-layer"></div>

    <!-- Upper layer -->
    <div class="popup-upper-layer">

        <div class="private-messages-new-conversation-popup">

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

                <!-- Found creatures list -->
                <div
                    v-if="foundConfidantsList.length > 0"
                    class="private-messages-new-conversation-confidants-list-container-outer">
                    <div class="private-messages-new-conversation-confidants-list-container">

                        <div
                            v-for="foundConfidant in foundConfidantsList" :key="foundConfidant.entityId"
                            class="private-messages-new-conversation-potential-confidant">
                            {{ foundConfidant.name }}
                        </div>

                    </div>
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
