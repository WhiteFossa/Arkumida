<script setup>
import {defineEmits, onMounted, reactive, ref} from "vue";
import {
    WebClientPostForm,
    WebClientSendGetRequest,
    WebClientSendPostRequest,
} from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import {PostprocessCreatureProfile, UndefinedOrNullToNull} from "@/js/libArkumida";
import ProfileAvatarSelectionComponent from "@/components/Profile/Parts/Avatars/ProfileAvatarSelectionComponent.vue";
import {required} from "@vuelidate/validators";
import useVuelidate from "@vuelidate/core";

    const emit = defineEmits(['reloadProfile'])

    const isLoading = ref(true)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

    const newAvatarUploadInput = ref(null)

    const newAvatarUploadFormData = reactive({
        name: ""
    })

    const newAvatarUploadRules = {
        name: {
            $autoDirty: true,
            required
        }
    }

    const newAvatarUploadValidator = useVuelidate(newAvatarUploadRules, newAvatarUploadFormData)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId
        await LoadProfile()

        await newAvatarUploadValidator.value.$validate()

        isLoading.value = false
    }

    async function SetAsCurrentAvatar(avatarId)
    {
        const response = await WebClientSendPostRequest("/api/Users/" + creatureId.value + "/SetCurrentAvatar", { "avatarId": avatarId })
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

    async function UploadNewAvatar()
    {
        // Do we have files?
        if (newAvatarUploadInput.value.files.length !== 1)
        {
            alert("Выберите ровно один файл для загрузки в качестве аварки.")
            return
        }

        const fileToUpload = newAvatarUploadInput.value.files[0]

        let formData = new FormData();
        formData.append("file", fileToUpload);

        // Uploading file
        const fileUploadResult = (await (await WebClientPostForm("/api/Files/Upload", formData)).json()).fileInfo

        // Creating an avatar from file
        await WebClientSendPostRequest("/api/Users/" + creatureId.value + "/CreateAvatar",{
            "avatar": {
                "name": newAvatarUploadFormData.name,
                "fileId": fileUploadResult.id
            }
        })

        // Reloading profile
        await LoadProfile()

        // Clearing avatar name and files
        newAvatarUploadFormData.name = ""
        newAvatarUploadInput.value.value = ""
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
                    :selectedAvatarId="UndefinedOrNullToNull(creatureProfile.currentAvatar?.id)"
                    @setAsCurrentAvatar="async (aid) => await SetAsCurrentAvatar(aid)"
                    @renameAvatar="async (aid, nn) => await RenameAvatar(aid, nn)" />

            </div>

            <!-- Inserting "no avatar" selection component -->
            <ProfileAvatarSelectionComponent
                :avatar="null"
                :selectedAvatarId="UndefinedOrNullToNull(creatureProfile.currentAvatar?.id)"
                @setAsCurrentAvatar="async (aid) => await SetAsCurrentAvatar(aid)" />

        </div>

        <!-- Upload new avatar -->
        <div class="profile-avatars-part-upload-new-avatar-container">
            <div class="profile-avatars-part-upload-new-avatar">
                <div class="profile-avatars-part-upload-new-avatar-caption">
                    Загрузить новую
                </div>

                <div class="profile-avatars-part-upload-new-avatar-name-form-group">
                    <label>
                        Название:
                    </label>

                    <input
                        class="profile-avatars-part-upload-new-avatar-name"
                        type="text"
                        v-model="newAvatarUploadFormData.name" />
                </div>

                <div class="profile-avatars-part-upload-new-avatar-file-form-group">
                    <label>
                        Файл:
                    </label>

                    <input
                        class="profile-avatars-part-upload-new-avatar-file"
                        ref="newAvatarUploadInput"
                        type="file"
                        accept="image/png, image/jpeg, image/gif" />
                </div>

                <div class="profile-avatars-part-upload-new-avatar-upload-form-group">
                    <button
                        type="button"
                        :disabled="newAvatarUploadValidator.$errors.length > 0"
                        @click="async () => await UploadNewAvatar()">
                        Загрузить
                    </button>
                </div>
            </div>
        </div>

    </div>
</template>