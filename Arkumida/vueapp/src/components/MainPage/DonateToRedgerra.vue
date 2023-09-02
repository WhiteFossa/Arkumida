<script setup>

    import {ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const isDonateInfoShown = ref(false)

    const donateInfo = ref(null)

    const isLoading = ref(true)

    async function ShowDonateInfo()
    {
        isDonateInfoShown.value = true

        donateInfo.value = await (await WebClientSendGetRequest("/api/SiteNotifications/DonateToRedgerra")).json()
        isLoading.value = false
    }

</script>

<template>
    <div class="donate-to-redgerra">
        <img class="donate-to-redgerra-avatar" src="/images/redgerra_avatar.jpg" alt="Аватарка Redgerrы" />
        Помощь создателю сайта "Furtails" Redgerre. Лично.
        <button v-if="!isDonateInfoShown" class="donate-to-redgerra-button" @click="ShowDonateInfo"><strong>Подробнее</strong></button>

        <span v-if="isDonateInfoShown">

            <LoadingSymbol v-if="isLoading"/>
            <span v-else>
                Перевод на карту в Украину по <a :href="donateInfo.donatePageUrl">ссылке</a>, номер карты &mdash; <strong>{{ donateInfo.donateCardNumber }}</strong>.
            </span>
        </span>
    </div>
</template>
