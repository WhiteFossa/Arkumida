<script setup>

    import {onMounted} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import RegisterComponent from "@/components/Registration/RegisterComponent.vue";

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
    <RegisterComponent />
</template>