<!-- Like creature component, but for non-existing creature -->
<script setup>

import {defineProps, onMounted } from "vue";
import {AvatarClass} from "@/js/constants";
import AvatarComponent from "@/components/Shared/AvatarComponent.vue";

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

        <AvatarComponent :avatar="null" :avatarClass="AvatarClass.Small" />
        <span class="creature-info-link">Нет</span>
    </div>
</template>