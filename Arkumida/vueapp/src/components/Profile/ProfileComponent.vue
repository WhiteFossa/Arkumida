<script setup>
import {defineProps, onMounted, ref} from "vue";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import router from "@/router";
import ProfilePartButton from "@/components/Profile/ProfilePartButton.vue";

    const props = defineProps({
        part: String
    })

   class ProfilePartsIds
    {
        static Main = 0
        static Avatars = 1
        static Security = 2
        static Logout = 3
    }

    // Profile parts
    const profileParts =
    [
        // Main
        {
            "id": ProfilePartsIds.Main,
            "part": "main",
            "name": "Главное"
        },

        // Avatars
        {
            "id": ProfilePartsIds.Avatars,
            "part": "avatars",
            "name": "Аватарки"
        },

        // Security
        {
            "id": ProfilePartsIds.Security,
            "part": "security",
            "name": "Безопасность"
        },

        // Logout
        {
            "id": ProfilePartsIds.Logout,
            "part": "logout",
            "name": "Выход"
        }
    ]

    const isLoading = ref(true)

    const currentPart = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        // If we are came directly to profile going to main part
        if (props.part === "")
        {
            await GoToPartById(ProfilePartsIds.Main)
        }
        else
        {
            currentPart.value = await GetPartByPart(props.part)
        }

        isLoading.value = false
    }

    async function GoToPartById(partId)
    {
        let part = profileParts.filter(function (pp) { return pp.id === partId })[0]

        currentPart.value = part
        router.replace({ path: "/profile/" + part.part })
    }

    async function GetPartByPart(part)
    {
        return profileParts.filter(function (pp) { return pp.part === part })[0]
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>

    <div v-if="!isLoading" class="body-container">
        <div class="profile-container">
            <div class="profile-buttons-container">

                <div v-for="part in profileParts" :key="part.id">
                    <ProfilePartButton :part="part" :currentPartId="currentPart.id" @goToPart="async (pid) => await GoToPartById(pid)"/>
                </div>

            </div>

            <div class="profile-content-container">
                <h1>{{ currentPart.name }}</h1>
            </div>
        </div>
    </div>
</template>