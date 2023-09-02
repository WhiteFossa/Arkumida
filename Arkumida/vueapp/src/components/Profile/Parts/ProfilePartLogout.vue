<script setup>

    import {ref} from "vue";
    import PopupYesNo from "@/components/Shared/Popups/PopupYesNo.vue";
    import {AuthLogUserOut} from "@/js/auth";
    import {Messages} from "@/js/constants";

    const isLogOutPopupShown = ref(false)

    async function RequestLogOut()
    {
        isLogOutPopupShown.value = true
    }

    async function LogOutCancelled()
    {
        isLogOutPopupShown.value = false
    }

    async function LogOutConfirmed()
    {
        await AuthLogUserOut()

        isLogOutPopupShown.value = false
    }

</script>

<template>

    <!-- Logging out (from this device) -->
    <div class="underlined-pseudolink" @click="async () => await RequestLogOut()">Выйти с сайта</div>

    <!-- Popups -->
    <PopupYesNo
        v-if="isLogOutPopupShown"
        :title="Messages.LogOutTitle"
        :text="Messages.LogOutText"
        @noPressed="async() => await LogOutCancelled()"
        @yesPressed="async() => await LogOutConfirmed()" />
</template>