<script setup>
    import {defineProps} from "vue";
    import TextIllustration from "@/components/TextIllustration.vue";

    const props = defineProps({
        illustrations: Object
    })
</script>

<template>
    <div v-if="props.illustrations.length > 0" class="illustrations-container">

        <div v-for="illustration in props.illustrations" :key="illustration.id">
            <TextIllustration :illustration="illustration" />
        </div>

    </div>
</template>
