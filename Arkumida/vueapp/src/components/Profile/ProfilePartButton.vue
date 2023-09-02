<script setup>
    import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        part: Object,
        currentPartId: Number
    })

    const emit = defineEmits(['goToPart'])

    async function GoToPart(partId)
    {
        emit('goToPart', partId)
    }
</script>

<template>
    <div v-if="props.part.id === props.currentPartId">
        <!-- Inactive button (current part) -->
        <div class="profile-part-button-current">
            {{ props.part.name }}
        </div>
    </div>
    <div v-else>
        <!-- Active button -->
        <div class="profile-part-button-active" @click="async () => await GoToPart(props.part.id)">
            {{ props.part.name }}
        </div>
    </div>
</template>