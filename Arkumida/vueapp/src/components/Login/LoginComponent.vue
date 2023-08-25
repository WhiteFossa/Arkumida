<script setup>

    import {reactive} from "vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {AuthLogUserIn} from "@/js/auth";
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

    async function LogIn()
    {
        await validator.value.$validate()

        if (!validator.value.$error)
        {
            const loginResult = await AuthLogUserIn(logInFormData.login, logInFormData.password, logInFormData.isRememberMe)

            // On successfull login redirecting to main page
            if (loginResult === LoginResult.OK)
            {
                await router.push("/");
            }
        }
    }
</script>

<template>
    <div class="login-form-container">
        <div class="login-form-title">
            Вход на сайт
        </div>

        <form>
            <!-- Login -->
            <div>
                Имя пользователя:
            </div>
            <input
                :class="(validator.login.$error)?'login-form-text-field login-form-text-field-invalid':'login-form-text-field'"
                type="text"
                placeholder="Имя пользователя"
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
            <div>
                <button
                    class="login-button"
                    type="button"
                    @click="LogIn"
                    :disabled="validator.$error">
                    Войти
                </button>
            </div>
        </form>

        <div class="login-reset-password-container">
            <a class="login-reset-password-link" href="/resetPassword" title="Восстановить пароль">Забыли пароль?</a>
        </div>
    </div>
</template>
