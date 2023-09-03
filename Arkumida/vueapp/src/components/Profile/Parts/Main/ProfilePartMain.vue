<script setup>

import {onMounted, reactive, ref} from "vue";
import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {AvatarClass} from "@/js/constants";
import {required} from "@vuelidate/validators";
import useVuelidate from "@vuelidate/core";
import {PostprocessCreatureProfile} from "@/js/libArkumida";

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

        <div>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
        </div>
    </div>
</template>