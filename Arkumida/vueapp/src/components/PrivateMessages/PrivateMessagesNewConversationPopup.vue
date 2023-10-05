<script setup>

import {defineEmits, reactive, ref} from "vue";

    const emit = defineEmits([ "cancelPressed", "confidantSelected" ])

    const selectedConfidantId = ref(null)

    const newConversationFormData = reactive({
        confidantName: ""
    })


async function OnCancelPressed()
    {
        emit("cancelPressed")
    }

    async function OnConfidantSelected()
    {
        emit("confidantSelected", selectedConfidantId.value)
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
                        v-model="newConversationFormData.confidantName" />
                </div>

                <div class="popup-buttons-container">
                    <button
                        class="popup-button"
                        @click="async() => await OnCancelPressed()">
                        Отмена
                    </button>

                    <button
                        class="popup-button"
                        @click="async() => await OnConfidantSelected()">
                        Начать
                    </button>
                </div>
            </div>

        </div>

    </div>
</template>
