<script setup>
import {onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
import {AuthIsUserLoggedIn} from "@/js/auth";
import router from "@/router";
import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
import {Messages, PasswordResetInitiationResult} from "@/js/constants";

    const passwordResetFormData = reactive({
        login: ""
    })

    const passwordResetFormRules = {
        login: {
            $autoDirty: true,
            required
        }
    }

    const passwordResetFormValidator = useVuelidate(passwordResetFormRules, passwordResetFormData)

    const passwordResetInitiationErrorHappened = ref(false)
    const passwordResetInitiationResult = ref(null)

    const adminInfo = ref(null)
    const adminEmailMailto = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        if (await AuthIsUserLoggedIn())
        {
            // Creature already logged in
            await router.push("/")
        }

        await passwordResetFormValidator.value.$validate()

        // Requesting admin info here - inoptimal to some degree, but easy
        adminInfo.value = await (await WebClientSendGetRequest("/api/SiteInfo/Admin")).json()
        adminEmailMailto.value = "mailto:" + adminInfo.value.email
    }

    async function InitiatePasswordReset()
    {
        passwordResetInitiationErrorHappened.value = false

        passwordResetInitiationResult.value = (await (await WebClientSendPostRequest(
            "/api/Users/InitiatePasswordReset",
            {
                "login": passwordResetFormData.login
            })).json()).result

        switch (passwordResetInitiationResult.value)
        {
            case PasswordResetInitiationResult.Initiated:
                alert(Messages.PasswordResetInstructionsSent)
                break

            case PasswordResetInitiationResult.CreatureNotFound:
                passwordResetInitiationErrorHappened.value = true
                break

            case PasswordResetInitiationResult.CreatureHaveNoEmail:
                passwordResetInitiationErrorHappened.value = true
                break

            case PasswordResetInitiationResult.CreatureHaveNoConfirmedEmail:
                passwordResetInitiationErrorHappened.value = true
                break

            case PasswordResetInitiationResult.FailedToSendEmail:
                passwordResetInitiationErrorHappened.value = true
                break

            default:
                throw new Error("Unknown password reset initiation result.")
        }
    }
</script>

<template>
    <div class="password-reset-form-container">
        <div class="password-reset-form-title">
            Сброс пароля
        </div>

        <div>
            <!-- Login -->
            <div
                v-if="passwordResetInitiationErrorHappened"
                class="password-reset-error-message">

                <!-- Creature not found -->
                <div v-if="passwordResetInitiationResult === PasswordResetInitiationResult.CreatureNotFound">
                    Пользователь с таким логином не найден.
                </div>

                <!-- Creature have no email -->
                <div v-if="passwordResetInitiationResult === PasswordResetInitiationResult.CreatureHaveNoEmail">
                    У этого пользователя не указана электронная почта. Автоматический сброс пароля невозможен, обратитесь, пожалуйста, к администратору:
                    <a :href="adminEmailMailto">{{adminInfo.email}}</a>.
                </div>

                <!-- Creature have no confirmed email -->
                <div v-if="passwordResetInitiationResult === PasswordResetInitiationResult.CreatureHaveNoConfirmedEmail">
                    Электронная почта пользователя не подтверждена. Из соображений безопасности автоматический сброс пароля невозможен, обратитесь, пожалуйста, к администратору:
                    <a :href="adminEmailMailto">{{adminInfo.email}}</a>.
                </div>

                <!-- Failed to send email -->
                <div v-if="passwordResetInitiationResult === PasswordResetInitiationResult.FailedToSendEmail">
                    Не удалось отправить письмо с инструкциями по сбросу пароля, обратитесь, пожалуйста, к администратору:
                    <a :href="adminEmailMailto">{{adminInfo.email}}</a>.
                </div>

            </div>

            <div>
                Логин:
            </div>
            <input
                :class="(passwordResetFormValidator.login.$error)?'password-reset-form-text-field password-reset-form-text-field-invalid':'password-reset-form-text-field'"
                type="text"
                placeholder="Логин"
                v-model="passwordResetFormData.login"
            />

            <!-- Password reset button -->
            <button
                class="password-reset-button"
                type="button"
                @click="async() => await InitiatePasswordReset()"
                :disabled="passwordResetFormValidator.$errors.length > 0">
                Сбросить пароль
            </button>
        </div>

        <div class="password-reset-form-bottom-links-container">
            <a class="password-reset-form-bottom-link" href="/login" title="Вход">Вспомнили пароль?</a>
        </div>
    </div>
</template>