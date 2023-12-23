<script setup>
    import {defineProps} from "vue";
    import VotesHistoryComponent from "@/components/VotesHistory/VotesHistoryComponent.vue";

    const props = defineProps({
        id: String
    })

</script>

<template>
    <VotesHistoryComponent :textId="props.id" />
</template>