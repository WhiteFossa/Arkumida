<script setup>
    import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        targetPage: Number,
        currentPage: Number
    })

    const emit = defineEmits(['goToPage'])

    async function GoToTargetPage()
    {
        emit('goToPage', props.targetPage)
    }
</script>

<template>
    <div class="read-text-pagination-page-base read-text-pagination-page-base-active read-text-pagination-page" v-if="props.currentPage === props.targetPage">
        {{props.targetPage}}
    </div>
    <div v-else class="read-text-pagination-page-base read-text-pagination-page" @click="GoToTargetPage()">
        {{props.targetPage}}
    </div>
</template>