<script setup>

    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import FooterComponent from "@/components/Footer/FooterComponent.vue";
    import MainPage from "@/components/MainPage/MainPage.vue";
    import {onMounted} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";

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

    <MainPage />

    <FooterComponent />

</template>