<!-- Shows top button -->
<script setup>

    import { defineProps } from 'vue'
    
    const props = defineProps({
        text: String,
        smallText: { type: String, default: "" },
        href: String
    })

</script>

<template>
    <a :href="href">
        <div v-if="props.smallText !== ''" class="top-button top-button-with-small-text">
            {{ props.text }}
            <div class="top-button-small-text">{{ props.smallText }}</div>
        </div>
        <div v-else class="top-button">
            {{ props.text }}
        </div>
    </a>
</template>