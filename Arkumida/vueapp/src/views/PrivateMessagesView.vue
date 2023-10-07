<script setup>

    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import FooterComponent from "@/components/Footer/FooterComponent.vue";
    import {onMounted, ref} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import PrivateMessagesComponent from "@/components/PrivateMessages/PrivateMessagesComponent.vue";

    const headerComponent = ref(null);

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await OnPageLoad()
    }

    async function OnMessageMarkedAsRead(messageId)
    {
        headerComponent.value.OnPrivateMessageMarkedAsRead(messageId)
    }

</script>

<template>

    <HeaderComponent ref="headerComponent" />

    <PrivateMessagesComponent
        @messageWasMarkedAsRead="async(mid) => await OnMessageMarkedAsRead(mid)" />

    <FooterComponent />

</template>