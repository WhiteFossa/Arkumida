<!-- Small creature's avatar -->
<script setup>
    import {defineProps, ref} from "vue";
    import {AvatarClass} from "@/js/constants";

    const props = defineProps({
        avatar: Object,
        avatarClass: Object
    })

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const avatarUrl = ref(null)

    const avatarClassName = ref("")

    if (props.avatar !== null)
    {
        avatarUrl.value = apiBaseUrl + "/api/Files/Download/" + props.avatar.fileId
    }

    switch(props.avatarClass)
    {
        case AvatarClass.Small:
            avatarClassName.value = "avatar-small"
            break;

        case AvatarClass.Big:
            avatarClassName.value = "avatar-big"
            break;

        default:
            throw new Error("Unknown avatar class.")
    }
</script>

<template>
    <!-- Avatar not set -->
    <img v-if="props.avatar === null" :class="avatarClassName" src="/images/furnonymous.jpg" alt="Аватарка не задана" />

    <!-- Avatar set -->
    <img v-if="props.avatar !== null" :class="avatarClassName" :src="avatarUrl" :alt="props.avatar.name" />
</template>
