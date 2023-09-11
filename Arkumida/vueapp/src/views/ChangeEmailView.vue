<script setup>
    import {onMounted, ref} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import {useRoute} from "vue-router";
    import {WebClientSendPostRequest} from "@/js/libWebClient";
    import {Messages} from "@/js/constants";
    import router from "@/router";

    const route = useRoute()

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await OnPageLoad()

        const creatureId = ref(route.query.creature)
        const emailAsBase64 = ref(route.query.email)
        const token = ref(route.query.token)

        const isChanged = (await (await WebClientSendPostRequest(
            "/api/Users/" + creatureId.value + "/Email/Change",
            {
                "email": emailAsBase64.value,
                "token": token.value,
            })).json()).isSuccessful

        if (isChanged)
        {
            alert(Messages.EmailAddressChanged)
        }
        else
        {
            alert(Messages.EmailAddressFailedToChange)
        }

        await router.push("/profile/security")
    }
</script>

<template>
    Изменение адреса электронной почты...
</template>