<script setup>
import {onMounted, ref} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import PasswordResetComponent from "@/components/PasswordReset/PasswordResetComponent.vue";
import {useRoute} from "vue-router";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    const route = useRoute()

    const isLoading = ref(true)

    const creatureId = ref(null)
    const token = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await OnPageLoad()

        creatureId.value = route.query.creature
        token.value = route.query.token

        isLoading.value = false
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <PasswordResetComponent
        v-if="!isLoading"
        :creatureId="creatureId"
        :token="token" />
</template>
