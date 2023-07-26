<script setup>
import {defineEmits, defineProps} from "vue";

    const props = defineProps({
        pagesCount: Number
    })

    const emit = defineEmits(['goToPage'])

    async function GoToPage(pageNumber)
    {
        emit('goToPage', pageNumber)
    }
</script>

<template>
    <div class="centered" v-if="props.pagesCount > 1">
        Pagination will be here. Total pages {{ props.pagesCount }}

        <div @click="GoToPage(props.pagesCount)">Last page</div>
    </div>
</template>