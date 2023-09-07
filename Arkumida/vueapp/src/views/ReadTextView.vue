<script setup>
    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import ReadTextComponent from "@/components/ReadText/ReadTextComponent.vue";
    import {defineProps, onMounted} from "vue";
    import {Guid} from "guid-typescript";
    import {OnPageLoad} from "@/js/libArkumida";

    const props = defineProps({
        id: Guid,
        page: Number
    })

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await OnPageLoad()
    }
</script>

<template>
    <HeaderComponent />

    <ReadTextComponent :id="props.id" :page="props.page" />
</template>