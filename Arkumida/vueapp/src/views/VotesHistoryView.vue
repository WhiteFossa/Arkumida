<script setup>
    import {defineProps} from "vue";
    import {Guid} from "guid-typescript";

    const props = defineProps({
        id: Guid
    })

</script>

<template>
    Лайки и дизлайки для {{ props.id }}
</template>