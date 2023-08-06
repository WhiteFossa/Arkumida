<!-- Like creature component, but for non-existing creature -->
<script setup>

import {defineProps, onMounted } from "vue";

const props = defineProps({
    creatureRole: { type: String, default: "" }
})

onMounted(async () =>
{
    await OnLoad();
})

async function OnLoad()
{
}

</script>

<template>
    <div class="creature-info-container">
        <div v-if="creatureRole !== ''" class="creature-info-creature-role">{{ props.creatureRole }}</div>

        <img class="creature-info-avatar" src="/images/furnonymous.jpg" alt="Furry anonymous" />
        <span class="creature-info-link">Нет</span>
    </div>
</template>