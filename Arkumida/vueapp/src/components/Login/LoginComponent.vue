<script setup>

import {onMounted, reactive, ref} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {AuthLogCreatureIn} from "@/js/auth";
    import router from "@/router";
    import {LoginResult} from "@/js/constants";

    const logInFormData = reactive({
        login: "",
        password: "",
        isRememberMe: false
    })

    const rules = {
        login: {
            $autoDirty: true,
            required
        },
        password: {
            $autoDirty: true,
            required
        }
    }

    const validator = useVuelidate(rules, logInFormData)

    const isIncorrectCredentialsMessageShown = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await validator.value.$validate()
    }

    async function LogIn()
    {
        isIncorrectCredentialsMessageShown.value = false

        const loginResult = await AuthLogCreatureIn(logInFormData.login, logInFormData.password, logInFormData.isRememberMe)

        // On successfull login redirecting to main page
        if (loginResult === LoginResult.OK)
        {
            await router.push("/");
        }
        else
        {
            isIncorrectCredentialsMessageShown.value = true
        }
    }
</script>

<template>
    <div class="login-form-container">
        <div class="login-form-title">
            Вход на сайт
        </div>

        <div
            v-if="isIncorrectCredentialsMessageShown"
            class="login-form-incorrect-credentials">
            Неверный логин или пароль!
        </div>

        <div>
            <!-- Login -->
            <div>
                Логин:
            </div>
            <input
                :class="(validator.login.$error)?'login-form-text-field login-form-text-field-invalid':'login-form-text-field'"
                type="text"
                placeholder="Логин"
                v-model="logInFormData.login" />

            <!-- Password -->
            <div>
                Пароль:
            </div>
            <input
                :class="(validator.password.$error)?'login-form-text-field login-form-text-field-invalid':'login-form-text-field'"
                type="password"
                placeholder="Пароль"
                v-model="logInFormData.password" />

            <!-- Remember me? -->
            <div class="login-remember-me-container">
                <input
                    type="checkbox"
                    v-model="logInFormData.isRememberMe" />
                Запомнить меня
            </div>

            <!-- Log in button -->
            <button
                class="login-button"
                type="button"
                @click="async() => await LogIn()"
                :disabled="validator.$errors.length > 0">
                Войти
            </button>
        </div>

        <div class="login-bottom-links-container">
            <a class="login-bottom-link" href="/initiatePasswordReset" title="Восстановить пароль">Забыли пароль?</a>
            <a class="login-bottom-link" href="/register" title="Регистрация">Хотите зарегистрироваться?</a>
        </div>
    </div>
</template>
