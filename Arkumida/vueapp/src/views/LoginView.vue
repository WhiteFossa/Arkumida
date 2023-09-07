<script setup>
    import LoginComponent from "@/components/Login/LoginComponent.vue";
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
    <LoginComponent />
</template>
