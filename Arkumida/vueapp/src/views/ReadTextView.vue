<script setup>
    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import ReadTextComponent from "@/components/ReadText/ReadTextComponent.vue";
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