<script setup>
    import {defineProps} from "vue";
    import TextIllustration from "@/components/ReadText/Illustrations/TextIllustration.vue";

    const props = defineProps({
        illustrations: Object
    })
</script>

<template>

    <div v-if="props.illustrations.length > 0">

        <div class="all-illustrations-title">
            Все иллюстрации:
        </div>

        <div class="illustrations-container">

            <div v-for="illustration in props.illustrations" :key="illustration.id">
                <TextIllustration :illustration="illustration" />
            </div>

        </div>

    </div>
</template>
