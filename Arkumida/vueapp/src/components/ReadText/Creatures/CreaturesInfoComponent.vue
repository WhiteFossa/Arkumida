<!-- Like CreatureInfoComponent, but for from 1 to N creatures -->
<script setup>
import {defineProps, onMounted, ref} from "vue";
import {Messages} from "@/js/constants";

    const props = defineProps({
        creaturesRole: { type: String, default: "" }, // Just a text description of this creatures role, like "Authors" or "Translators"
        creatures: Object
    })

    const creaturesLinks = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        props.creatures.forEach((creature) =>
        {
            let creatureLink =
                {
                    id: creature.entityId,
                    name: creature.name,
                    href: "/users/" + creature.entityId,
                    title: Messages.CreatureUser + creature.name
                }

            creaturesLinks.value.push(creatureLink)
        });
    }
</script>

<template>
    <div class="creatures-info-container">
        <div v-if="creaturesRole !== ''" class="creatures-info-creatures-role">{{ props.creaturesRole }}</div>

        <div class="creatures-info-creatures-list">
            <div v-for="creatureLink in creaturesLinks" :key="creatureLink.entityId">
                <a :href="creatureLink.href" :title="creatureLink.title"><img class="creatures-info-avatar" src="/images/fossa_avatar.jpg" :alt="creatureLink.title" /></a>
                <a class="creatures-info-link" :href="creatureLink.href" :title="creatureLink.title">{{ creatureLink.name }}</a>
            </div>
        </div>

    </div>
</template>