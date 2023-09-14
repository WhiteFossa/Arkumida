<script setup>
    import {onMounted, reactive} from "vue";
    import {AuthIsUserLoggedIn} from "@/js/auth";
    import router from "@/router";
    import useVuelidate from "@vuelidate/core";
    import {required} from "@vuelidate/validators";
    import {WebClientSendPostRequest} from "@/js/libWebClient";
    import {UserRegistrationResult} from "@/js/constants";

    const registrationFormData = reactive({
        login: "",
        password: "",
        passwordConfirmation: ""
    })

    const registrationFormRules = {
        login: {
            $autoDirty: true,
            required
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

    async function Register()
    {
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
                alert("Регистрация успешна, сейчас вы будете перенаправлены на страницу входа на сайт.")

                await router.push("/login")
                break;

            case UserRegistrationResult.LoginIsTaken:
                alert("Этот логин уже занят.")
                break;

            case UserRegistrationResult.WeakPassword:
                alert("Вы выбрали слишком простой пароль, усложните его.")
                break;

            case UserRegistrationResult.GenericError:
                alert("Неизвестная ошибка в процессе регистрации.")
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
            <div>
                Логин:
            </div>
            <input
                :class="(registrationFormValidator.login.$error)?'registration-form-text-field registration-form-text-field-invalid':'registration-form-text-field'"
                type="text"
                placeholder="Логин"
                v-model="registrationFormData.login" />

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
</template>