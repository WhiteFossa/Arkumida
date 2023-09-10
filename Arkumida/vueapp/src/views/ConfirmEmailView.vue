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
        const token = ref(route.query.token)

        const isConfirmed = (await (await WebClientSendPostRequest(
            "/api/Users/" + creatureId.value + "/Email/Confirm",
            {
                "token": token.value,
            })).json()).isSuccessful

        if (isConfirmed)
        {
            alert(Messages.EmailAddressConfirmed)
        }
        else
        {
            alert(Messages.EmailAddressFailedToConfirm)
        }

        await router.push("/profile/security")
    }
</script>

<template>
    Подтверждение адреса электронной почты...
</template>