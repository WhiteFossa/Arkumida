<!-- Popup, able to load an image from arbitrary URL -->
<script setup>
import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        url: String
    })

    const emit = defineEmits(['closePopup'])

    async function ClosePopup()
    {
        emit("closePopup")
    }

    // Empty click handler for popup
    async function DoNothing()
    {

    }
</script>

<template>
    <!-- Lower layer -->
    <div class="urled-image-popup-lower-layer">
    </div>

    <!-- Upper layer -->
    <div class="urled-image-popup-upper-layer" @click="ClosePopup">

        <div class="urled-image-popup" @click.stop="DoNothing">
            <button class="urled-image-popup-close-button" @click="ClosePopup">
                <img class="urled-image-popup-close-button-image" src="/images/close.webp" alt="Close popup" />
            </button>

            <img class="urled-image-popup-image" :src="props.url" />
        </div>
    </div>
</template>

