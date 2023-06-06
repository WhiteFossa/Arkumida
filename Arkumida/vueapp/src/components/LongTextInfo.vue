<script setup>

    import {defineEmits} from 'vue'

    const emit = defineEmits(['closePopup'])

    async function ClosePopup()
    {
        emit('closePopup')
    }

</script>

<template>

    <!-- Lower layer -->
    <div class="popup-lower-layer">

    </div>

    <!-- Upper layer -->
    <div class="popup-upper-layer">
        <div class="popup">

            <button class="popup-close-button" @click="ClosePopup">
                <img class="popup-close-button-image" src="/images/close.svg" alt="Close" />
            </button>

            Long info will be here
        </div>
    </div>

</template>