<script setup>
import {defineEmits, defineProps, onMounted, ref} from "vue";
    import ReadTextGoToPage from "@/components/Shared/Pagination/PaginationGoToPage.vue";
    import ReadTextPaginationFirstPage from "@/components/Shared/Pagination/PaginationFirstPage.vue";
    import ReadTextPaginationLastPage from "@/components/Shared/Pagination/PaginationLastPage.vue";
    import ReadTextPaginationPage from "@/components/Shared/Pagination/PaginationPage.vue";
import ReadTextPaginationInterpageGap from "@/components/Shared/Pagination/PaginationInterpagesGap.vue";

    const props = defineProps({
        currentPage: Number,
        pagesCount: Number
    })

    const emit = defineEmits(['goToPage'])

    const firstOrdinaryPage = ref(0)
    const lastOrdinaryPage = ref(0)
    const ordinaryPages = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        firstOrdinaryPage.value = props.currentPage - 1
        if (firstOrdinaryPage.value < 2)
        {
            firstOrdinaryPage.value = 2
        }

        lastOrdinaryPage.value = props.currentPage + 1

        if (lastOrdinaryPage.value > props.pagesCount - 1)
        {
            lastOrdinaryPage.value = props.pagesCount - 1
        }

        for (let pageNumber = firstOrdinaryPage.value; pageNumber <= lastOrdinaryPage.value; pageNumber++)
        {
            ordinaryPages.value.push(pageNumber)
        }
    }

    async function GoToPage(pageNumber)
    {
        emit('goToPage', pageNumber)
    }
</script>

<template>
    <div class="pagination-container" v-if="props.pagesCount > 1">

        <div class="pagination-pages-container">
            <ReadTextPaginationFirstPage :currentPage="props.currentPage" @goToPage="async (pn) => await GoToPage(pn)" />

            <ReadTextPaginationInterpageGap v-if="ordinaryPages.length > 0" />

            <ReadTextPaginationPage
                v-for="ordinaryPageNumber in ordinaryPages"
                :key="ordinaryPageNumber"
                :targetPage="ordinaryPageNumber"
                :currentPage="props.currentPage"
                @goToPage="async (pn) => await GoToPage(pn)" />

            <ReadTextPaginationInterpageGap v-if="ordinaryPages.length > 0" />

            <ReadTextPaginationLastPage :currentPage="props.currentPage" :pagesCount="props.pagesCount" @goToPage="async (pn) => await GoToPage(pn)" />
        </div>

        <!-- Go to page -->
        <ReadTextGoToPage :currentPage="props.currentPage" :pagesCount="props.pagesCount" @goToPage="async (pn) => await GoToPage(pn)"/>
    </div>
</template>