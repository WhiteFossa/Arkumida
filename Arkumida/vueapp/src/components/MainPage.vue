<script setup>

import TopButton from "@/components/TopButton.vue";
import SearchComponent from "@/components/SearchComponent.vue";
import TextsStatisticsComponent from "@/components/TextsStatisticsComponent.vue";
import MainLogo from "@/components/MainLogo.vue";
import ShortTextInfosContainer from "@/components/ShortTextInfosContainer.vue";

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
        <div class="horizontal-flex logo-and-texts-container">
            <div class="main-logo-container">
                <MainLogo />
            </div>
            
            <!-- Texts container -->
            <ShortTextInfosContainer dataSource="/api/Texts/Popular" />
        </div>
    </div>
</template>
