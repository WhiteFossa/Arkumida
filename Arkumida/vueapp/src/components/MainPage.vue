<script setup>

import TopButton from "@/components/TopButton.vue";

</script>

<template>
    <body>
        <!-- Main page code -->
        
        <!-- Top buttons -->
        <div class="horizontal-flex top-buttons-container">
            <TopButton href="/addText" text="Добавить"/>
            <TopButton href="/collectiveTranslation" text="Совместный перевод" smallText="Сейчас переводятся: 2"/>
            <TopButton href="/contests" text="Конкурсы" />
        </div>
        
        <h1>Furtails.pw</h1>
    </body>
</template>
