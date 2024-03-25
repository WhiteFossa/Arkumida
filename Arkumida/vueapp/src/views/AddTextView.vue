<script setup>
    import {onMounted} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import FooterComponent from "@/components/Footer/FooterComponent.vue";
    import AddTextComponent from "@/components/AddText/AddTextComponent.vue";

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

    <AddTextComponent />

    <FooterComponent />
</template>
