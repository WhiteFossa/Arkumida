<script setup>
import {defineEmits, onMounted, ref} from "vue";
import {
    WebClientPostForm,
    WebClientSendGetRequest,
    WebClientSendPostRequest,
    } from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {PostprocessCreatureProfile} from "@/js/libArkumida";
import ProfileAvatarSelectionComponent from "@/components/Profile/Parts/Avatars/ProfileAvatarSelectionComponent.vue";

    const emit = defineEmits(['reloadProfile'])

    const isLoading = ref(true)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId
        await LoadProfile()

        isLoading.value = false
    }

    async function SetAsCurrentAvatar(avatarId)
    {
        const response = await WebClientSendPostRequest("/api/Users/Current/SetCurrentAvatar", { "avatarId": avatarId })
        if (!response.ok)
        {
            // Something went wrong
            return;
        }

        // Reloading profile
        await LoadProfile()

        // We need to reload external profile because of default avatar change
        emit("reloadProfile")
    }

    async function UploadNewAvatar(ev)
    {
        const uploadControl = ev.target
        const fileToUpload = uploadControl.files[0]

        let formData = new FormData();
        formData.append("file", fileToUpload);

        // Uploading file
        const fileUploadResult = (await (await WebClientPostForm("/api/Files/Upload", formData)).json()).fileInfo

        // Creating an avatar from file
        await WebClientSendPostRequest("/api/Users/Current/CreateAvatar",{
            "avatar": {
                "name": fileUploadResult.name,
                "fileId": fileUploadResult.id
            }
        })

        // Reloading profile
        await LoadProfile()
    }

    async function LoadProfile()
    {
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile

        PostprocessCreatureProfile(creatureProfile)
    }

    async function RenameAvatar(avatarId, newName)
    {
        await WebClientSendPostRequest("/api/Users/" + creatureId.value + "/RenameAvatar",{
            "avatarId": avatarId,
            "newName": newName
        })

        await LoadProfile()
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />
    <div v-else>

        <!-- Container with existing avatars -->
        <div class="profile-avatars-part-avatars-container" :key="creatureProfile.currentAvatar">

            <div v-for="avatar in creatureProfile.avatars" :key="avatar.id">

                <ProfileAvatarSelectionComponent
                    :avatar="avatar"
                    :selectedAvatarId="creatureProfile.currentAvatar?.id"
                    @setAsCurrentAvatar="async (aid) => await SetAsCurrentAvatar(aid)"
                    @renameAvatar="async (aid, nn) => await RenameAvatar(aid, nn)" />

            </div>

        </div>

        <!-- Upload new avatar -->
        <div class="profile-avatars-part-upload-new-avatar">
            Загрузить новую:

            <input
                type="file"
                accept="image/png, image/jpeg, image/gif"
                @change="async ($event) => await UploadNewAvatar($event)" />

        </div>

    </div>
</template>