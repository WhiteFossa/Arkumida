<script setup>
import {defineProps, onMounted, reactive} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
import {AuthIsCreatureLoggedIn, AuthLogCreatureOutAndReLogIn} from "@/js/auth";
    import router from "@/router";
import {Guid} from "guid-typescript";
import {WebClientSendPostRequest} from "@/js/libWebClient";
import {Messages} from "@/js/constants";

    const props = defineProps({
        creatureId: Guid,
        token: String
    })

    const passwordResetFormData = reactive({
        password: "",
        passwordConfirmation: ""
    })

    const passwordResetFormRules = {
        password: {
            $autoDirty: true,
            required
        },

        passwordConfirmation: {
            $autoDirty: true,
            required,
            sameAsPassword: CheckPasswordConfirmation
        }
    }

    const passwordResetFormValidator = useVuelidate(passwordResetFormRules, passwordResetFormData)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        if (await AuthIsCreatureLoggedIn())
        {
            // Creature already logged in
            await router.push("/")
        }

        await passwordResetFormValidator.value.$validate()
    }

    // It looks like Vuelidate's SameAs() is bugged
    function CheckPasswordConfirmation(passwordConfirmation)
    {
        return passwordConfirmation === passwordResetFormData.password
    }

    async function SetNewPassword()
    {
        const isPasswordResetSuccessful = (await (await WebClientSendPostRequest(
            "/api/Users/" + props.creatureId + "/ResetPassword",
            {
                "newPassword": passwordResetFormData.password,
                "token": props.token
            })).json())
            .isSuccessful

        if (isPasswordResetSuccessful)
        {
            alert(Messages.PasswordResetSuccessful)

            await AuthLogCreatureOutAndReLogIn()
        }
        else
        {
            alert(Messages.PasswordResetFailed)
        }
    }
</script>

<template>
    <div class="password-reset-confirmation-form-container">
        <div class="password-reset-confirmation-form-title">
            Новый пароль
        </div>

        <div>
            <!-- Password -->
            <div>
                Пароль:
            </div>
            <input
                :class="(passwordResetFormValidator.password.$error)?'password-reset-confirmation-form-text-field password-reset-confirmation-form-text-field-invalid':'password-reset-confirmation-form-text-field'"
                type="password"
                placeholder="Пароль"
                v-model="passwordResetFormData.password"
            />

            <!-- Password confirmation -->
            <div>
                Подтверждение пароля:
            </div>
            <input
                :class="(passwordResetFormValidator.passwordConfirmation.$error)?'password-reset-confirmation-form-text-field password-reset-confirmation-form-text-field-invalid':'password-reset-confirmation-form-text-field'"
                type="password"
                placeholder="Подтверждение пароля"
                v-model="passwordResetFormData.passwordConfirmation"
            />

            <!-- Password reset button -->
            <button
                class="password-reset-confirmation-button"
                type="button"
                @click="async() => await SetNewPassword()"
                :disabled="passwordResetFormValidator.$errors.length > 0">
                Установить пароль
            </button>
        </div>

        <div class="password-reset-confirmation-form-bottom-links-container">
            <a class="password-reset-confirmation-form-bottom-link" href="/login" title="Вход">Вспомнили старый пароль?</a>
        </div>
    </div>
</template>