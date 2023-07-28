<script setup>
    import {defineEmits, defineProps} from "vue";
    import ReadTextGoToPage from "@/components/ReadText/Pagination/ReadTextGoToPage.vue";
    import ReadTextPaginationFirstPage from "@/components/ReadText/Pagination/ReadTextPaginationFirstPage.vue";
    import ReadTextPaginationLastPage from "@/components/ReadText/Pagination/ReadTextPaginationLastPage.vue";

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
    <div class="read-text-pagination-container" v-if="props.pagesCount > 1">
        <ReadTextPaginationFirstPage :currentPage="props.currentPage" @goToPage="async (pn) => await GoToPage(pn)" />
        <ReadTextPaginationLastPage :currentPage="props.currentPage" :pagesCount="props.pagesCount" @goToPage="async (pn) => await GoToPage(pn)" />

        <ReadTextGoToPage :currentPage="props.currentPage" :pagesCount="props.pagesCount" @goToPage="async (pn) => await GoToPage(pn)"/>
    </div>
</template>