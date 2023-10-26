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
    <div class="pagination-page-base pagination-page-base-active pagination-page" v-if="props.currentPage === props.targetPage">
        {{props.targetPage}}
    </div>
    <div v-else class="pagination-page-base pagination-page" @click="GoToTargetPage()">
        {{props.targetPage}}
    </div>
</template>