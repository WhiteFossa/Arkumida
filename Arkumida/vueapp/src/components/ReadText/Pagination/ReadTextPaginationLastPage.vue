<script setup>
    import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        currentPage: Number,
        pagesCount: Number
    })

    const emit = defineEmits(['goToPage'])

    async function GoToLastPage()
    {
        emit('goToPage', props.pagesCount)
    }
</script>

<template>
    <div class="read-text-pagination-page-base read-text-pagination-page-base-active read-text-pagination-last-page" v-if="props.currentPage === props.pagesCount">
        {{props.pagesCount}}
    </div>
    <div v-else class="read-text-pagination-page-base read-text-pagination-last-page" @click="GoToLastPage()">
        {{props.pagesCount}}
    </div>
</template>