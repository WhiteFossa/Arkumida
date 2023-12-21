<script setup>
    import {defineProps} from "vue";
    import {Guid} from "guid-typescript";
    import VotesHistoryComponent from "@/components/VotesHistory/VotesHistoryComponent.vue";

    const props = defineProps({
        id: Guid
    })

</script>

<template>
    <VotesHistoryComponent :textId="props.id" />
</template>