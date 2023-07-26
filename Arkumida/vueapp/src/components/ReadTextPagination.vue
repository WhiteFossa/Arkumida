<script setup>
    import {defineEmits, defineProps} from "vue";
    import ReadTextGoToPage from "@/components/ReadTextGoToPage.vue";

    const props = defineProps({
        currentPage: Number,
        pagesCount: Number
    })

    const emit = defineEmits(['goToPage'])

    async function GoToPage(pageNumber)
    {
        emit('goToPage', pageNumber)
    }
</script>

<template>
    <div class="pagination-container" v-if="props.pagesCount > 1">
        Pagination will be here. Total pages {{ props.pagesCount }}

        <ReadTextGoToPage :currentPage="props.currentPage" :pagesCount="props.pagesCount" @goToPage="async (pn) => await GoToPage(pn)"/>
    </div>
</template>