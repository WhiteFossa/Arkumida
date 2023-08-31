<script setup>
    import {AvatarClass} from "@/js/constants";
    import {defineEmits, defineProps, reactive, ref} from "vue";
    import {Guid} from "guid-typescript";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";

    const props = defineProps({
        avatar: Object,
        selectedAvatarId: Guid
    })

    const emit = defineEmits(["setAsCurrentAvatar", "renameAvatar"])

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
        emit("setAsCurrentAvatar", props.avatar.id)
    }

    async function StartNameEditing()
    {
        isEditingName.value = true
        renameAvatarFormData.name = props.avatar.name
    }

    async function CompleteNameEditing()
    {
        isEditingName.value = false

        emit("renameAvatar", props.avatar.id, renameAvatarFormData.name)
    }
</script>

<template>
    <div :class="(props.avatar.id === props.selectedAvatarId)?'profile-avatars-part-avatar-container profile-avatars-part-avatar-container-selected':'profile-avatars-part-avatar-container'">
        <AvatarComponent :avatar="props.avatar" :avatarClass="AvatarClass.Big" />

        <!-- Avatar name -->
        <div v-if="!isEditingName">
            {{ props.avatar.name }}
        </div>

        <!-- Rename avatar -->
        <div
            class="avatar-selection-component-rename-avatar-container"
            v-if="isEditingName">

            <input
                class="avatar-selection-component-rename-input"
                type="text"
                v-model="renameAvatarFormData.name" />

            <button
                type="button"
                @click="async () => await CompleteNameEditing()"
                :disabled="renameAvatarValidator.$error">
                OK
            </button>

        </div>

        <div class="avatar-selection-component-icons-container">

            <!-- Select as current -->
            <div
                v-if="props.avatar.id !== props.selectedAvatarId"
                @click="async () => await SetAsCurrentAvatar()"
                class="avatar-selection-component-select-as-current"
                title="Выбрать как аватарку по-умолчанию">
                <img class="small-icon" src="/images/icons/icon_ok.png" alt="Выбрать как аватарку по-умолчанию" />
            </div>

            <!-- Rename -->
            <div
                @click="async () => await StartNameEditing()"
                class="avatar-selection-component-rename"
                title="Переименовать аватарку">
                <img class="small-icon" src="/images/icons/icon_edit.png" alt="Переименовать аватарку" />
            </div>

            <!-- Delete -->
            <div
                class="avatar-selection-component-delete"
                title="Удалить аватарку">
                <img class="small-icon" src="/images/icons/icon_cancel.png" alt="Удалить аватарку" />
            </div>

        </div>

    </div>
</template>