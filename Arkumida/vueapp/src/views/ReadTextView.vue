<script setup>
    import HeaderComponent from "@/components/HeaderComponent.vue";
    import ReadTextComponent from "@/components/ReadTextComponent.vue";
    import {defineProps} from "vue";

    const props = defineProps({
        id: String
    })
</script>

<template>
    <HeaderComponent />

    <ReadTextComponent :id="props.id" />
</template>