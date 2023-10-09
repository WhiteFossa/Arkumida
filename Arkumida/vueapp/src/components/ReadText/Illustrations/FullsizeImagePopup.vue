<script setup>
    import {defineEmits, defineProps} from "vue";
    import {Guid} from "guid-typescript";

    const emit = defineEmits(['closePopup'])

    const props = defineProps({
        id: Guid
    })

    const imageUrl = process.env.VUE_APP_API_URL + "/api/Files/Download/" + props.id

    async function ClosePopup()
    {
        emit('closePopup')
    }

    // Empty click handler for popup
    async function DoNothing()
    {

    }
</script>

<template>
    <!-- Lower layer -->
    <div class="full-size-image-popup-lower-layer">
    </div>

    <!-- Upper layer -->
    <div class="full-size-image-popup-upper-layer" @click="ClosePopup">

        <div class="full-size-image-popup" @click.stop="DoNothing">
            <button class="full-size-image-popup-close-button" @click="ClosePopup">
                <img class="full-size-image-popup-close-button-image" src="/images/close.webp" alt="Close popup" />
            </button>

            <img class="full-size-image-popup-image" :src="imageUrl" />
        </div>
    </div>

</template>