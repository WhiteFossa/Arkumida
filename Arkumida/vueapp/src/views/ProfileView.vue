<script setup>
    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import {defineProps} from "vue";
    import ProfileComponent from "@/components/Profile/ProfileComponent.vue";

    const props = defineProps({
        part: String
    })
</script>

<template>
    <HeaderComponent />

    <ProfileComponent :part="props.part"/>
</template>
