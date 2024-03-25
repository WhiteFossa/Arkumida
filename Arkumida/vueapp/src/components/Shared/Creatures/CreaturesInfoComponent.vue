<!-- Like CreaturesInfoComponent, but supports the case when creature(s) doesn't exist yet -->
<script setup>
import {defineProps, onMounted, ref} from "vue";
import {WebClientSendGetRequest} from "@/js/libWebClient";
import {AvatarClass, Messages} from "@/js/constants";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import AvatarComponent from "@/components/Shared/AvatarComponent.vue";

    const props = defineProps({
        creaturesRole: { type: String, default: "" }, // Just a text description of this creatures role, like "Authors" or "Translators"
        creatures: Object // Creatures list. May be empty, in this case we will display "no creatures". If not empty, then each creature have the next fileds:
        // id: creature ID (null means "non-existing creature")
        // name: name for non-existing creatures
    })

    const isLoading = ref(true)

    const creaturesList = ref([])

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        console.log(props.creatures)

        props.creatures.forEach(async (creature) =>
        {
            if (creature.id !== null)
            {
                let creatureProfile = (await (await WebClientSendGetRequest("/api/Users/" + creature.id + "/Profile")).json()).creatureWithProfile

                let existingCreature =
                {
                    id: creature.Id,
                    profile: creatureProfile,
                    href: "/users/" + creature.Id,
                    name: creatureProfile.name,
                    title: Messages.CreatureUser + creatureProfile.name
                }

                creaturesList.value.push(existingCreature)
            }
            else
            {
                let nonexistingCreature =
                {
                    id: null,
                    profile: null,
                    href: null,
                    name: creature.name,
                    title: null
                }

                creaturesList.value.push(nonexistingCreature)
            }
        });

        isLoading.value = false
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />

    <div class="creatures-info-container" v-if="!isLoading">
        <div v-if="creaturesRole !== ''" class="creatures-info-creatures-role">{{ props.creaturesRole }}</div>

        <div class="creatures-info-creatures-list">

            <div v-if="creaturesList.length === 0">
                <!-- No creatures -->
                <AvatarComponent :avatar="null" :avatarClass="AvatarClass.Small" />
                <span class="creatures-info-link">Нет</span>
            </div>

            <div v-for="creature in creaturesList" :key="creature">

                <div v-if="creature.id !== null">

                    <!-- Existing creature -->
                    <a :href="creature.href" :title="creature.title">
                        <AvatarComponent :avatar="creature.profile.currentAvatar" :avatarClass="AvatarClass.Small" />
                    </a>
                    <a class="creatures-info-link" :href="creature.href" :title="creature.title">{{ creature.name }}</a>

                </div>

                <div v-if="creature.id === null">

                    <!-- Nonexisting creature -->
                    <AvatarComponent :avatar="null" :avatarClass="AvatarClass.Small" />
                    <span class="creatures-info-link">{{ creature.name }}</span>

                </div>

            </div>

        </div>

    </div>
</template>
