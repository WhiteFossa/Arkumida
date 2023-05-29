<script setup>

import TopButton from "@/components/TopButton.vue";
import SearchComponent from "@/components/SearchComponent.vue";
import TextsStatisticsComponent from "@/components/TextsStatisticsComponent.vue";
import MainLogo from "@/components/MainLogo.vue";
import ShortTextInfo from "@/components/ShortTextInfo.vue";

</script>

<template>
    <div class="body-container">
        <!-- Main page code -->
        
        <!-- Top buttons -->
        <div class="horizontal-flex top-buttons-container">
            <TopButton href="/addText" text="Добавить"/>
            <TopButton href="/collectiveTranslation" text="Совместный перевод" smallText="Сейчас переводятся: 2"/>
            <TopButton href="/contests" text="Конкурсы" />
        </div>
        
        <!-- Search -->
        <SearchComponent />
        
        <!-- Texts statistics -->
        <TextsStatisticsComponent />
        
        <!-- Logo and texts list -->
        <div class="horizontal-flex logo-and-texts-containero">
            <div>
                <MainLogo />
            </div>
            <div class="texts-short-info-blocks-container">
                <ShortTextInfo id="0e9fc697-31d6-4776-bf7e-05950936b4de" />
                <ShortTextInfo id="185d7cc9-83e7-4de3-b1d8-5f8d50af6a88" />
            </div>
        </div>
    </div>
</template>
