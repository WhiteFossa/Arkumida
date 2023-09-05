<script setup>
    import {onMounted, reactive, ref} from "vue";
    import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import {PostprocessCreatureProfile} from "@/js/libArkumida";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {AuthLogUserOutAndReLogIn} from "@/js/auth";
    import {Messages} from "@/js/constants";

    const isLoading = ref(true)

    const creatureId = ref(null)
    const creatureProfile = ref(null)

    const isPasswordChanging = ref(false)

    const passwordChangeFormData = reactive({
        oldPassword: "",
        newPassword: "",
        newPasswordConfirmation: ""
    })

    const passwordChangeRules = {
        oldPassword: {
            $autoDirty: true,
            required
        },

        newPassword: {
            $autoDirty: true,
            required
        },

        newPasswordConfirmation: {
            $autoDirty: true,
            required,
            sameAsPassword: CheckNewPasswordConfirmation
        }
    }

    const passwordChangeValidator = useVuelidate(passwordChangeRules, passwordChangeFormData)

    const isPasswordChangeFailed = ref(false)

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

    async function LoadProfile()
    {
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile

        PostprocessCreatureProfile(creatureProfile)
    }

    async function StartToChangePassword()
    {
        ClearChangePasswordForm()

        passwordChangeValidator.value.$validate()

        isPasswordChanging.value = true
    }

    async function CancelPasswordChange()
    {
        isPasswordChanging.value = false
    }

    async function ConfirmPasswordChange() {
        const passwordChangeResult = (await (await WebClientSendPostRequest(
            "/api/Users/" + creatureId.value + "/ChangePassword",
            {
                "oldPassword": passwordChangeFormData.oldPassword,
                "newPassword": passwordChangeFormData.newPassword
            })).json()).isSuccessful

        if (passwordChangeResult)
        {
            isPasswordChanging.value = false

            alert(Messages.PasswordChangedMessage)

            await AuthLogUserOutAndReLogIn()

            return
        }

        // Clearing the form to force creature to re-input data
        ClearChangePasswordForm()
        isPasswordChangeFailed.value = true
    }

    // It looks like Vuelidate's SameAs() is bugged
    function CheckNewPasswordConfirmation(newPasswordConfirmation)
    {
        return newPasswordConfirmation === passwordChangeFormData.newPassword
    }

    function ClearChangePasswordForm()
    {
        passwordChangeFormData.oldPassword = ""
        passwordChangeFormData.newPassword = ""
        passwordChangeFormData.newPasswordConfirmation = ""
    }
</script>

<template>
    <LoadingSymbol v-if="isLoading" />
    <div v-else>

        <!-- Login -->
        <div class="profile-security-part-login-container">
            Логин:
            {{creatureProfile.login}}
            <span class="profile-security-part-login-comment">(Логин не может быть изменён)</span>
        </div>

        <!-- Password -->
        <div class="profile-security-part-password-container">

            <!-- Show password -->
            <div
                v-if="!isPasswordChanging">
                Пароль:
                <span
                    class="underlined-pseudolink"
                    @click="async () => await StartToChangePassword()">
                    изменить
                </span>
            </div>

            <!-- Edit password -->
            <div
                v-if="isPasswordChanging"
                class="profile-security-part-change-password-container">

                <div class="profile-security-part-change-password-caption">

                    <button
                        class="button-with-image"
                        type="button"
                        title="Изменить пароль"
                        :disabled="passwordChangeValidator.$errors.length > 0"
                        @click="async () => await ConfirmPasswordChange()">
                        <img class="small-icon" src="/images/icons/icon_ok.png" alt="Изменить пароль" />
                    </button>

                    <button
                        class="button-with-image"
                        type="button"
                        title="Отказаться от изменения пароля"
                        @click="async () => await CancelPasswordChange()">
                        <img class="small-icon" src="/images/icons/icon_cancel.png" alt="Отказаться от изменения пароля" />
                    </button>

                    Изменить пароль
                </div>

                <div
                    v-if="isPasswordChangeFailed"
                    class="profile-security-part-change-password-change-failed-info">

                    <div>
                        Не удалось изменить пароль!
                    </div>

                    <div>
                        Обычно это случается если:
                    </div>

                    <ul class="ul-with-better-margins">
                        <li>Старый пароль неверен</li>
                        <li>Новый пароль слишком простой</li>
                    </ul>

                </div>

                <table>
                    <!-- Old password -->
                    <tr>
                        <td>
                            <label>
                                Старый пароль:
                            </label>
                        </td>

                        <td class="profile-security-part-change-password-old-password-table-cell">
                            <input
                                :class="(passwordChangeValidator.oldPassword.$error)?'profile-security-part-change-password-old-password-input profile-security-part-change-password-old-password-input-invalid':'profile-security-part-change-password-old-password-input'"
                                type="password"
                                placeholder="Старый пароль"
                                v-model="passwordChangeFormData.oldPassword" />
                        </td>
                    </tr>

                    <!-- New password -->
                    <tr>
                        <td>
                            <label>
                                Новый пароль:
                            </label>
                        </td>

                        <td class="profile-security-part-change-password-new-password-table-cell">
                            <input
                                :class="(passwordChangeValidator.newPassword.$error)?'profile-security-part-change-password-new-password-input profile-security-part-change-password-new-password-input-invalid':'profile-security-part-change-password-new-password-input'"
                                type="password"
                                placeholder="Новый пароль"
                                v-model="passwordChangeFormData.newPassword" />
                        </td>
                    </tr>

                    <!-- Password confirmation -->
                    <tr>
                        <td>
                            <label>
                                Подтверждение:
                            </label>
                        </td>

                        <td class="profile-security-part-change-password-new-password-confirmation-table-cell">
                            <input
                                :class="(passwordChangeValidator.newPasswordConfirmation.$error)?'profile-security-part-change-password-new-password-confirmation-input profile-security-part-change-password-new-password-confirmation-input-invalid':'profile-security-part-change-password-new-password-confirmation-input'"
                                type="password"
                                placeholder="Повторите новый пароль"
                                v-model="passwordChangeFormData.newPasswordConfirmation" />
                        </td>
                    </tr>
                </table>
            </div>

        </div>

        <!-- Email -->
        <div class="profile-security-part-email-container">
            E-mail:
            {{creatureProfile.email}}
        </div>
    </div>
</template>