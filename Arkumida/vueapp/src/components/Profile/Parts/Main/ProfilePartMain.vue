<script setup>

    import {defineEmits, onMounted, reactive, ref} from "vue";
import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";
import {required} from "@vuelidate/validators";
import useVuelidate from "@vuelidate/core";
import {PostprocessCreatureProfile, RenderTextElement} from "@/js/libArkumida";

    const emit = defineEmits(['reloadProfile'])

    const isLoading = ref(true)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

    const isRenaming = ref(false)

    const renameFormData = reactive({
        name: ""
    })

    const renameRules = {
        name: {
            $autoDirty: true,
            required
        }
    }

    const renameValidator = useVuelidate(renameRules, renameFormData)

    const aboutInfoAsElements = ref(null)
    const renderedAbout = ref("")

    const isAboutEditing = ref(false)

    // We don't need validator for about, because even empty string is allowed
    const aboutEditingFormData = reactive({
        about: ""
    })

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId
        await LoadProfile()

        await renameValidator.value.$validate()

        isLoading.value = false
    }

    async function LoadProfile()
    {
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile

        PostprocessCreatureProfile(creatureProfile)

        // Getting and rendering About information
        aboutInfoAsElements.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/About/GetAsElements")).json()).elements
        renderedAbout.value = ""
        aboutInfoAsElements.value.forEach(e => renderedAbout.value += RenderTextElement(e))
    }

    async function StartToRename()
    {
        renameFormData.name = creatureProfile.value.name

        isRenaming.value = true
    }

    async function CancelRename()
    {
        isRenaming.value = false
    }

    async function CompleteRename()
    {
        isRenaming.value = false

        await WebClientSendPostRequest("/api/Users/" + creatureId.value + "/Rename", { "newName": renameFormData.name })

        // Reloading profile to be sure that server accepted changes
        await LoadProfile()

        emit("reloadProfile")
    }

    async function StartToEditAbout()
    {
        aboutEditingFormData.about = creatureProfile.value.about

        isAboutEditing.value = true
    }

    async function CancelAboutEditing()
    {
        isAboutEditing.value = false
    }

    async function CompleteAboutEditing()
    {
        isAboutEditing.value = false

        await WebClientSendPostRequest("/api/Users/" + creatureId.value + "/About/Update", { "newAbout": aboutEditingFormData.about })

        await LoadProfile()
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />
    <div v-else>

        <!-- Creature name (show mode) -->
        <div
            v-if="!isRenaming"
            class="profile-main-part-creature-name-show">
            {{ creatureProfile.name }}

            <button
                class="button-with-image"
                type="button"
                title="Изменить имя"
                @click="async () => await StartToRename()">
                <img class="small-icon" src="/images/icons/icon_edit.webp" alt="Изменить имя" />
            </button>
        </div>

        <!-- Creaute name (edit mode) -->
        <div
            v-if="isRenaming"
            class="profile-main-part-creature-name-edit">

            <input
                :class="(renameValidator.name.$error)?'profile-main-part-creature-rename-input profile-main-part-creature-rename-input-invalid':'profile-main-part-creature-rename-input'"
                type="text"
                v-model="renameFormData.name" />

            <button
                class="button-with-image"
                type="button"
                title="Подтвердить смену имени"
                :disabled="renameValidator.$errors.length > 0"
                @click="async () => await CompleteRename()">
                <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Подтвердить смену имени" />
            </button>

            <button
                class="button-with-image"
                type="button"
                title="Не менять имя"
                @click="async () => await CancelRename()">
                <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Не менять имя" />
            </button>
        </div>

        <!-- About info -->
        <div class="profile-main-part-about-outer-container">
            <div class="profile-main-part-about-inner-container">

                <div class="profile-main-part-about-caption">
                    <button
                        v-if="!isAboutEditing"
                        class="button-with-image"
                        type="button"
                        title="Редактировать информацию о себе"
                        @click="async () => await StartToEditAbout()">
                        <img class="small-icon" src="/images/icons/icon_edit.webp" alt="Редактировать информацию о себе" />
                    </button>

                    О себе
                </div>

                <!-- About info (show) -->
                <div v-if="!isAboutEditing">
                    <!-- Avatar (big) -->
                    <div class="profile-main-part-avatar-container-show-about">
                        <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Big" />
                    </div>

                    <div
                        v-html="renderedAbout">
                    </div>
                </div>

                <!-- About info (edit) -->
                <div
                    v-if="isAboutEditing"
                    class="profile-main-part-edit-about-container">

                    <div class="profile-main-part-avatar-container-edit-about">
                        <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Big" />
                    </div>

                    <textarea
                        class="profile-main-part-edit-about-textarea"
                        v-model="aboutEditingFormData.about">
                    </textarea>

                </div>
            </div>

            <!-- About editing buttons -->
            <div
                v-if="isAboutEditing"
                class="profile-main-part-about-edit-buttons-outer-container">
                <div class="profile-main-part-about-edit-buttons-bordered-inner-container">

                    <button
                        class="button-with-image"
                        type="button"
                        title="Сохранить изменения"
                        @click="async () => await CompleteAboutEditing()">
                        <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Сохранить изменения" />
                    </button>

                    <button
                        class="button-with-image"
                        type="button"
                        title="Отменить изменения"
                        @click="async () => await CancelAboutEditing()">
                        <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Отменить изменения" />
                    </button>

                </div>
            </div>

        </div>
    </div>
</template>