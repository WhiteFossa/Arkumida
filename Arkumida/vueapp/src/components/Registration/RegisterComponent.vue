<script setup>
import {onMounted, reactive, ref} from "vue";
    import {AuthIsUserLoggedIn} from "@/js/auth";
    import router from "@/router";
    import useVuelidate from "@vuelidate/core";
import {helpers, required} from "@vuelidate/validators";
    import {WebClientSendPostRequest} from "@/js/libWebClient";
    import {Messages, UserRegistrationResult} from "@/js/constants";
    import PopupYesNo from "@/components/Shared/Popups/PopupYesNo.vue";

    const registrationFormData = reactive({
        login: "",
        password: "",
        passwordConfirmation: ""
    })

    const registrationFormRules = {
        login: {
            $autoDirty: true,
            required,
            isLoginTaken: helpers.withAsync(IsLoginTaken)
        },

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

    const registrationFormValidator = useVuelidate(registrationFormRules, registrationFormData)

    const isRegistrationConfirmationPopupShown = ref(false)

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

        await registrationFormValidator.value.$validate()
    }

    // It looks like Vuelidate's SameAs() is bugged
    function CheckPasswordConfirmation(passwordConfirmation)
    {
        return passwordConfirmation === registrationFormData.password
    }

    async function IsLoginTaken(login)
    {
        const isTaken = (await (await WebClientSendPostRequest(
            "/api/Users/IsLoginTaken",
            {
                "checkData": {
                    "login": login
                }
            })).json())
            .checkResult
            .isTaken

        return !isTaken
    }

    async function Register()
    {
        isRegistrationConfirmationPopupShown.value = true
    }

    async function CancelRegistration()
    {
        isRegistrationConfirmationPopupShown.value = false
    }

    async function ConfirmRegistration()
    {
        isRegistrationConfirmationPopupShown.value = false

        const registrationResult = (await (await WebClientSendPostRequest(
            "/api/Users/Register",
            {
                "registrationData": {
                    "login": registrationFormData.login,
                    "email": "",
                    "password": registrationFormData.password
                }
            })).json()).registrationResult

        switch (registrationResult.result)
        {
            case UserRegistrationResult.OK:
                alert(Messages.RegistrationSuccess)

                await router.push("/login")
                break;

            case UserRegistrationResult.LoginIsTaken:
                alert(Messages.LoginIsTaken)
                break;

            case UserRegistrationResult.WeakPassword:
                alert(Messages.PasswordTooWeak)
                break;

            case UserRegistrationResult.GenericError:
                alert(Messages.GenericRegistrationError)
                break;

            default:
                throw new Error("Unknown registration response.")
        }
    }
</script>

<template>
    <div class="registration-form-container">

        <div class="registration-form-title">
            Регистрация
        </div>

        <!-- Registration form -->
        <div>

            <!-- Login -->
            <div class="registration-login-title-container">
                <div>
                    Логин:
                </div>

                <div
                    v-if="registrationFormValidator.login.isLoginTaken.$invalid"
                    class="registration-login-taken-message">
                    Этот логин занят
                </div>
            </div>
            <input
                :class="(registrationFormValidator.login.$error)?'registration-form-text-field registration-form-text-field-invalid':'registration-form-text-field'"
                type="text"
                placeholder="Логин"
                v-model="registrationFormData.login" />

            <div class="registration-small-gray-text">
                (Логин не может быть изменён после регистрации, но отображаемое имя пользователя — может)
            </div>

            <!-- Password -->
            <div>
                Пароль:
            </div>
            <input
                :class="(registrationFormValidator.password.$error)?'registration-form-text-field registration-form-text-field-invalid':'registration-form-text-field'"
                type="password"
                placeholder="Пароль"
                v-model="registrationFormData.password" />

            <!-- Password confirmation -->
            <div>
                Подтверждение пароля:
            </div>
            <input
                :class="(registrationFormValidator.passwordConfirmation.$error)?'registration-form-text-field registration-form-text-field-invalid':'registration-form-text-field'"
                type="password"
                placeholder="Подтверждение пароля"
                v-model="registrationFormData.passwordConfirmation" />

            <!-- Registration button -->
            <button
                class="registration-button"
                type="button"
                @click="async() => await Register()"
                :disabled="registrationFormValidator.$errors.length > 0">
                Зарегистрироваться
            </button>

        </div>

        <!-- Bottom links -->
        <div class="registration-bottom-links-container">
            <a class="registration-bottom-link" href="/login" title="Вход на сайт">Уже зарегистрированы?</a>
        </div>
    </div>

    <!-- Popups -->

    <!-- Registration confirmation popup -->
    <PopupYesNo
        v-if="isRegistrationConfirmationPopupShown"
        :title="Messages.RegistrationConfirmationTitle"
        :text="Messages.RegistrationConfirmationText"
        @noPressed="async() => await CancelRegistration()"
        @yesPressed="async() => await ConfirmRegistration()" />
</template>