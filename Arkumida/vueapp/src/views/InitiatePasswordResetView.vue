<script setup>
    import {onMounted} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import PasswordResetComponent from "@/components/PasswordReset/PasswordResetInitiationComponent.vue";

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
    <PasswordResetComponent />
</template>