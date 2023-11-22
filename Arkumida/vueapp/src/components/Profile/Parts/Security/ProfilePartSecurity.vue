<script setup>
import {defineProps, onMounted, reactive, ref} from "vue";
    import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
    import {PostprocessCreatureProfile} from "@/js/libArkumida";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import {email, required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {AuthLogCreatureOutAndReLogIn} from "@/js/auth";
import {Messages, ProfileConsts} from "@/js/constants";

    const props = defineProps({
        action: String
    })

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

    const isEmailConfirmed = ref(false)
    const isEmailConfirmationMessageBeingSent = ref(false)

    const isEmailEmpty = ref(false)

    const isEmailBeingEdited = ref(false)

    const emailChangeFormData = reactive({
        email: ""
    })

    const emailChangeRules = {
        email: {
            $autoDirty: true,
            email
        }
    }

    const emailChangeValidator = useVuelidate(emailChangeRules, emailChangeFormData)

    const isEmailChangeMessageBeingSent = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId
        await LoadProfile()

        if (props.action === ProfileConsts.ForcePasswordChangeActionName)
        {
            await StartToChangePassword()
        }

        isLoading.value = false
    }

    async function LoadProfile()
    {
        creatureProfile.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Profile")).json()).creatureWithProfile

        PostprocessCreatureProfile(creatureProfile)

        isEmailConfirmed.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Email/IsConfirmed")).json()).isConfirmed

        isEmailEmpty.value = creatureProfile.value.email.length === 0
    }

    async function StartToChangePassword()
    {
        await ClearChangePasswordForm()

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

            await AuthLogCreatureOutAndReLogIn()

            return
        }

        // Clearing the form to force creature to re-input data
        await ClearChangePasswordForm()
        isPasswordChangeFailed.value = true
    }

    // It looks like Vuelidate's SameAs() is bugged
    function CheckNewPasswordConfirmation(newPasswordConfirmation)
    {
        return newPasswordConfirmation === passwordChangeFormData.newPassword
    }

    async function ClearChangePasswordForm()
    {
        passwordChangeFormData.oldPassword = ""
        passwordChangeFormData.newPassword = ""
        passwordChangeFormData.newPasswordConfirmation = ""
    }

    async function BeginEmailConfirmation()
    {
        isEmailConfirmationMessageBeingSent.value = true

        const isConfirmationMessageSent = (await (await WebClientSendPostRequest(
            "/api/Users/" + creatureId.value + "/Email/InitiateConfirmation",
            {})).json()).isSuccessful

        isEmailConfirmationMessageBeingSent.value = false

        if (isConfirmationMessageSent)
        {
            alert(Messages.EmailAddressConfirmationEmailSent)
        }
        else
        {
            alert(Messages.EmailAddressConfirmationEmailNotSent)
        }
    }

    async function BeginEmailChange()
    {
        emailChangeFormData.email = creatureProfile.value.email

        emailChangeValidator.value.$validate()

        isEmailBeingEdited.value = true
    }

    async function CancelEmailChange()
    {
        isEmailBeingEdited.value = false
    }

    async function CompleteEmailChange()
    {
        isEmailBeingEdited.value = false

        isEmailChangeMessageBeingSent.value = true

        const emailChangeInitiationResult = await (await WebClientSendPostRequest(
            "/api/Users/" + creatureId.value + "/Email/InitiateChange",
            {
                "newEmail": emailChangeFormData.email
            })).json()

        isEmailChangeMessageBeingSent.value = false

        if (!emailChangeInitiationResult.isSuccessful)
        {
            alert(Messages.EmailAddressChangeRequestFailed)

            await LoadProfile()

            return
        }

        if (emailChangeInitiationResult.isEmailSent)
        {
            alert(Messages.EmailAddressChangeEmailSent)
        }
        else
        {
            // Email changed without sending a message, reloading
            await LoadProfile()
        }
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
                class="profile-security-part-change-password-outer-container">
                <div
                    class="profile-security-part-change-password-inner-container">

                    <div class="profile-security-part-change-password-caption">
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

                <!-- Password change OK / Cancel buttons -->
                <div class="profile-security-part-change-password-buttons-outer-container">
                    <div class="profile-security-part-change-password-buttons-inner-container">
                        <button
                            class="button-with-image"
                            type="button"
                            title="Изменить пароль"
                            :disabled="passwordChangeValidator.$errors.length > 0"
                            @click="async () => await ConfirmPasswordChange()">
                            <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Изменить пароль" />
                        </button>

                        <button
                            class="button-with-image"
                            type="button"
                            title="Отказаться от изменения пароля"
                            @click="async () => await CancelPasswordChange()">
                            <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Отказаться от изменения пароля" />
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Email -->
        <div class="profile-security-part-email-container">
            E-mail:

            <!-- Show email form -->
            <div
                v-if="!isEmailBeingEdited"
                class="profile-security-part-email-show-container">

                <div v-if="!isEmailEmpty">
                    <!-- Non-empty email -->
                    {{creatureProfile.email}}
                </div>
                <div v-else>
                    <!-- Empty email -->
                    Не указан
                </div>

                <div v-if="!isEmailChangeMessageBeingSent">
                    <!-- Email change button -->
                    <button
                        class="button-with-image"
                        type="button"
                        title="Изменить адрес электронной почты"
                        @click="async () => await BeginEmailChange()">
                        <img class="small-icon" src="/images/icons/icon_edit.webp" alt="Изменить адрес электронной почты" />
                    </button>
                </div>
                <div v-else>
                    <LoadingSymbol />
                </div>


                <!-- Email not confirmed message -->
                <div
                    v-if="!isEmailConfirmed && !isEmailEmpty"
                    class="profile-security-part-email-not-confirmed-warning-container">

                    <div class="profile-security-part-email-not-confirmed-warning">
                        <div class="vertical-align-flex">
                            <img class="small-icon" src="/images/icons/icon_warning.webp" alt="Адрес не подтверждён!" />
                        </div>

                        <div>
                            Адрес не подтверждён, вы не сможете получать уведомления на почту!
                        </div>
                    </div>

                    <div
                        v-if="!isEmailConfirmationMessageBeingSent"
                        class="underlined-pseudolink"
                        @click="async () => await BeginEmailConfirmation()">
                        Подтвердить
                    </div>
                    <div v-else>
                        <LoadingSymbol />
                    </div>

                </div>
            </div>

            <!-- Email editing form -->
            <div
                v-if="isEmailBeingEdited"
                class="profile-security-part-email-edit-container">

                <input
                    :class="(emailChangeValidator.email.$error)?'profile-security-part-email-edit-input profile-security-part-email-edit-input-invalid':'profile-security-part-email-edit-input'"
                    type="text"
                    v-model="emailChangeFormData.email"/>

                <button
                    class="button-with-image"
                    type="button"
                    title="Подтвердить изменение адреса электронной почты"
                    :disabled="emailChangeValidator.$errors.length > 0"
                    @click="async () => await CompleteEmailChange()">
                    <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Подтвердить изменение адреса электронной почты" />
                </button>

                <button
                    class="button-with-image"
                    type="button"
                    title="Отменить изменение адреса электронной почты"
                    @click="async () => await CancelEmailChange()">
                    <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Отменить изменение адреса электронной почты" />
                </button>

            </div>

        </div>
    </div>
</template>