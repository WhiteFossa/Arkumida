<script setup>
    import {defineEmits, defineProps, ref} from "vue";

    const props = defineProps({
        currentPage: Number,
        pagesCount: Number
    })

    const emit = defineEmits(['goToPage'])

    const targetPage = ref(props.currentPage)

    async function GoToPage(pageNumber)
    {
        if (pageNumber < 1 || pageNumber > props.pagesCount)
        {
            return;
        }

        emit('goToPage', pageNumber)
    }
</script>

<template>
    <form v-on:submit.prevent="GoToPage(Number(targetPage))">
        <input class="read-text-pagination-go-to-page-input" type="text" v-model="targetPage" />
        <button class="read-text-pagination-go-to-page-button">Перейти</button>
    </form>
</template>