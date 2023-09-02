<!-- Popup with question -->
<script setup>
import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        title: String,
        text: String
    })

    const emit = defineEmits([ "noPressed", "yesPressed" ])

    async function OnNoPressed()
    {
        emit('noPressed')
    }

    async function OnYesPressed()
    {
        emit('yesPressed')
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
                    {{ props.title }}
                </div>

                <div>
                    {{ props.text }}
                </div>

                <div class="popup-buttons-container">
                    <button class="popup-button" @click="async() => await OnNoPressed()">Нет</button>

                    <button class="popup-button" @click="async() => await OnYesPressed()">Да</button>
                </div>

            </div>
        </div>

    </div>

</template>