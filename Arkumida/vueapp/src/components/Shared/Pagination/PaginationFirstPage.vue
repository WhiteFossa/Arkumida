<script setup>
    import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        currentPage: Number
    })

    const emit = defineEmits(['goToPage'])

    async function GoToFirstPage()
    {
        emit('goToPage', 1)
    }
</script>

<template>
    <div class="pagination-page-base pagination-page-base-active pagination-first-page" v-if="props.currentPage === 1">
        1
    </div>
    <div v-else class="pagination-page-base pagination-first-page" @click="GoToFirstPage()">
        1
    </div>
</template>