<script setup>
import {defineProps} from "vue";
import {Guid} from "guid-typescript";

    const props = defineProps({
        textId: Guid
    })
</script>

<template>
    <div class="votes-history-title">
        История голосов
    </div>

    <div>
        Текст: {{ props.textId }}
    </div>
</template>