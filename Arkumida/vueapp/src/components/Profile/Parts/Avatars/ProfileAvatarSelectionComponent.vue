<script setup>
    import {AvatarClass} from "@/js/constants";
    import {defineEmits, defineProps, reactive, ref} from "vue";
    import {Guid} from "guid-typescript";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {UndefinedOrNullToNull} from "@/js/libArkumida";

    const props = defineProps({
        avatar: Object,
        selectedAvatarId: Guid
    })

    const emit = defineEmits(["setAsCurrentAvatar", "renameAvatar", "deleteAvatar"])

    const isEditingName = ref(false)

    const renameAvatarFormData = reactive({
        name: ""
    })

    const renameAvatarRules = {
        name: {
            $autoDirty: true,
            required
        }
    }

    const renameAvatarValidator = useVuelidate(renameAvatarRules, renameAvatarFormData)

    async function SetAsCurrentAvatar()
    {
        emit("setAsCurrentAvatar", UndefinedOrNullToNull(props.avatar?.id))
    }

    async function StartNameEditing()
    {
        isEditingName.value = true
        renameAvatarFormData.name = props.avatar.name
    }

    async function CancelNameEditing()
    {
        isEditingName.value = false
    }

    async function CompleteNameEditing()
    {
        isEditingName.value = false

        emit("renameAvatar", props.avatar.id, renameAvatarFormData.name)
    }

    async function DeleteAvatar()
    {
        emit("deleteAvatar", UndefinedOrNullToNull(props.avatar?.id))
    }
</script>

<template>
    <div :class="(UndefinedOrNullToNull(props.avatar?.id) === props.selectedAvatarId)?'profile-avatars-part-avatar-container profile-avatars-part-avatar-container-selected':'profile-avatars-part-avatar-container'">
        <AvatarComponent :avatar="props.avatar" :avatarClass="AvatarClass.Big" />

        <!-- Avatar name -->
        <div v-if="!isEditingName">

            <div v-if="props.avatar !== null">
                {{ props.avatar.name }}
            </div>
            <div v-else>
                Нет аватарки
            </div>

        </div>

        <!-- Rename avatar -->
        <div
            class="avatar-selection-component-rename-avatar-container"
            v-if="isEditingName">

            <input
                :class="(renameAvatarValidator.name.$error)?'avatar-selection-component-rename-input avatar-selection-component-rename-input-invalid':'avatar-selection-component-rename-input'"
                type="text"
                v-model="renameAvatarFormData.name" />

            <button
                class="button-with-image"
                type="button"
                title="Подтвердить переименование"
                @click="async () => await CompleteNameEditing()"
                :disabled="renameAvatarValidator.$errors.length > 0">
                <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Подтвердить переименование" />
            </button>

            <button
                class="button-with-image"
                type="button"
                title="Отменить переименование"
                @click="async () => await CancelNameEditing()">
                <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Отменить переименование" />
            </button>

        </div>

        <div
            v-if="(props.avatar !== null) || (props.avatar === null && props.selectedAvatarId !== null)"
            class="avatar-selection-component-icons-container">

            <!-- Select as current -->
            <button
                v-if="props.avatar?.id !== props.selectedAvatarId"
                class="button-with-image"
                type="button"
                title="Выбрать как аватарку по-умолчанию"
                @click="async () => await SetAsCurrentAvatar()">
                <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Выбрать как аватарку по-умолчанию" />
            </button>

            <!-- Rename -->
            <button
                v-if="props.avatar !== null"
                class="button-with-image"
                type="button"
                title="Переименовать аватарку"
                @click="async () => await StartNameEditing()"
                :disabled="isEditingName">
                <img class="small-icon" src="/images/icons/icon_edit.webp" alt="Переименовать аватарку" />
            </button>

            <!-- Delete -->
            <button
                v-if="props.avatar !== null"
                class="button-with-image"
                type="button"
                title="Удалить аватарку"
                @click="async () => await DeleteAvatar()">
                <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Удалить аватарку" />
            </button>

        </div>

    </div>
</template>