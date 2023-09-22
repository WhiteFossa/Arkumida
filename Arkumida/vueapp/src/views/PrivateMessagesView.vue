<script setup>

    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import FooterComponent from "@/components/Footer/FooterComponent.vue";
    import {onMounted} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import PrivateMessagesComponent from "@/components/PrivateMessages/PrivateMessagesComponent.vue";

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

    <PrivateMessagesComponent />

    <FooterComponent />

</template>