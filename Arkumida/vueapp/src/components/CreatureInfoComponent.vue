<script setup>

import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";

    const props = defineProps({
        id: Guid,
        furryReadableId: String,
        name: String,
        creatureRole: { type: String, default: "" }
    })

    const creatureLinkHref = ref(null)
    const creatureLinkTitle = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureLinkHref.value = "/users/" + props.id
        creatureLinkTitle.value = "Пользователь " + props.name
    }

</script>

<template>
    <div class="creature-info-container">
        <div v-if="creatureRole !== ''" class="creature-info-creature-role">{{ props.creatureRole }}</div>

        <a :href="creatureLinkHref" :title="creatureLinkTitle"><img class="creature-info-avatar" src="/images/fossa_avatar.jpg" :alt="creatureLinkTitle" /></a>
        <a class="creature-info-link" :href="creatureLinkHref" :title="creatureLinkTitle">{{ props.name }}</a>
    </div>
</template>