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
    <div class="read-text-pagination-first-page read-text-pagination-first-page-active" v-if="props.currentPage === 1">
        1
    </div>
    <div v-else class="read-text-pagination-first-page" @click="GoToFirstPage()">
        1
    </div>
</template>