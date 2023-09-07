<script setup>
    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import {defineProps, onMounted, ref} from "vue";
    import ProfileComponent from "@/components/Profile/ProfileComponent.vue";
    import {OnPageLoad} from "@/js/libArkumida";

    const props = defineProps({
        part: String,
        action: String
    })

    const headerComponent = ref(null);

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await OnPageLoad()
    }

    async function ReloadProfile()
    {
        headerComponent.value.ReloadProfile()
    }
</script>

<template>
    <HeaderComponent ref="headerComponent" />

    <ProfileComponent
        :part="props.part"
        :action="props.action"
        @reloadProfile="async () => await ReloadProfile()"/>
</template>
