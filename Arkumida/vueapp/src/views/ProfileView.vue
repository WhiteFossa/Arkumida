<script setup>
    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import {defineProps, ref} from "vue";
    import ProfileComponent from "@/components/Profile/ProfileComponent.vue";

    const props = defineProps({
        part: String
    })

    const headerComponent = ref(null);

    async function ReloadProfile()
    {
        headerComponent.value.ReloadProfile()
    }
</script>

<template>
    <HeaderComponent ref="headerComponent" />

    <ProfileComponent :part="props.part" @reloadProfile="async () => await ReloadProfile()"/>
</template>
