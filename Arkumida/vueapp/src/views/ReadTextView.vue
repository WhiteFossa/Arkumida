<script setup>
    import HeaderComponent from "@/components/HeaderComponent.vue";
    import ReadTextComponent from "@/components/ReadTextComponent.vue";
    import {defineProps} from "vue";
    import {Guid} from "guid-typescript";

    const props = defineProps({
        id: Guid,
        page: Number
    })
</script>

<template>
    <HeaderComponent />

    <ReadTextComponent :id="props.id" :page="props.page" />
</template>