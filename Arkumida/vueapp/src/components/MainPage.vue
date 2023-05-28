<script setup>

import TopButton from "@/components/TopButton.vue";
import SearchComponent from "@/components/SearchComponent.vue";
import TextsStatisticsComponent from "@/components/TextsStatisticsComponent.vue";

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
        
        <h1>Furtails.pw</h1>
    </div>
</template>
