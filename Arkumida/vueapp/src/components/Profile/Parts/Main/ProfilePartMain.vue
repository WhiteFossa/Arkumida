<script setup>

import {onMounted, reactive, ref} from "vue";
import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";
import {required} from "@vuelidate/validators";
import useVuelidate from "@vuelidate/core";
import {PostprocessCreatureProfile, RenderTextElement} from "@/js/libArkumida";

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
        await WebClientSendPostRequest("/api/Users/" + creatureId.value + "/Rename", { "newName": renameFormData.name })

        // Reloading profile to be sure that server accepted changes
        await LoadProfile()

        isRenaming.value = false
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
                <img class="small-icon" src="/images/icons/icon_edit.png" alt="Изменить имя" />
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
                <img class="small-icon" src="/images/icons/icon_ok.png" alt="Подтвердить смену имени" />
            </button>

            <button
                class="button-with-image"
                type="button"
                title="Не менять имя"
                @click="async () => await CancelRename()">
                <img class="small-icon" src="/images/icons/icon_cancel.png" alt="Не менять имя" />
            </button>
        </div>

        <div class="profile-main-part-avatar-container">
            <AvatarComponent :avatar="creatureProfile.currentAvatar" :avatarClass="AvatarClass.Big" />
        </div>

        <!-- About info -->
        <div
            v-html="renderedAbout">
        </div>
    </div>
</template>