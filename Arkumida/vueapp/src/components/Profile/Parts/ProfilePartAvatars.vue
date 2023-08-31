<script setup>
import {onMounted, ref} from "vue";
import {
    WebClientPostForm,
    WebClientSendGetRequest,
    WebClientSendPostRequest,
    } from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";
    import router from "@/router";

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
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile

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

        // Reloading the page
        await router.go()
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
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />
    <div v-else>

        <!-- Container with existing avatars -->
        <div class="profile-avatars-part-avatars-container">

            <div v-for="avatar in creatureProfile.avatars" :key="avatar.entityId">

                <div class="profile-avatars-part-avatar-container">
                    <AvatarComponent :avatar="avatar" :avatarClass="AvatarClass.Big" />

                    <div class="underlined-pseudolink" @click="async () => await SetAsCurrentAvatar(avatar.id)">
                        Выбрать
                    </div>
                </div>

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