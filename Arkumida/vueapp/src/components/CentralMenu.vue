<!-- Shows central menu -->
<script setup>

import AtomLink from "@/components/AtomLink.vue";
import TelegramGroup from "@/components/TelegramGroup.vue";
</script>

<template>
    <div>
        <nav>
            <a class="central-menu-link" href="/item1" title="Item 1">Item 1</a>
            <a class="central-menu-link" href="/item2" title="Item 2">Item 2</a>
            <a class="central-menu-link" href="/item3" title="Item 3">Item 3</a>
            
            <div class="central-menu-link-nohover">
                <AtomLink />
            </div>

            <div class="central-menu-link-nohover">
                <TelegramGroup />
            </div>
        </nav>
    </div>
</template>