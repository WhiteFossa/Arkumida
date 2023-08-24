<!-- List of creatures -->
<script setup>
    import {defineProps, onMounted, ref} from "vue";
    import {AvatarClass, Messages} from "@/js/constants";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    const props = defineProps({
        creaturesRole: { type: String, default: "" }, // Just a text description of this creatures role, like "Authors" or "Translators"
        creaturesIds: Object // May be empty, we display "no creature" in this case
    })

    const isLoading = ref(true)

    const creatures = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        props.creaturesIds.forEach(async (creatureId) =>
        {
            if (creatureId === null)
            {
                throw new Error("Creature ID must not be null!");
            }

            let creatureProfile = (await (await WebClientSendGetRequest("/api/Users/" + creatureId + "/Profile")).json()).creatureWithProfile

            let creature =
                {
                    id: creatureId,
                    profile: creatureProfile,
                    href: "/users/" + creatureId,
                    title: Messages.CreatureUser + creatureProfile.name
                }

            creatures.value.push(creature)
        });

        isLoading.value = false
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />

    <div class="creatures-info-container" v-if="!isLoading">
        <div v-if="creaturesRole !== ''" class="creatures-info-creatures-role">{{ props.creaturesRole }}</div>

        <div class="creatures-info-creatures-list">

            <div v-if="creatures.length === 0">
                <!-- No creatures -->
                <AvatarComponent :avatar="null" :avatarClass="AvatarClass.Small" />
                <span class="creature-info-link">Нет</span>
            </div>

            <div v-for="creature in creatures" :key="creature">
                <a :href="creature.href" :title="creature.title">
                    <AvatarComponent :avatar="creature.profile.currentAvatar" :avatarClass="AvatarClass.Small" />
                </a>
                <a class="creatures-info-link" :href="creature.href" :title="creature.title">{{ creature.profile.name }}</a>
            </div>

        </div>

    </div>
</template>