<script setup>
    import {defineProps} from "vue";
    import moment from "moment/moment";

    const props = defineProps({
        vote: Object
    })
</script>

<template>
    <div class="votes-history-item-container">

        <div>{{ moment(props.vote.timestamp).format('HH:mm DD.MM.YYYY') }}</div>

        <!-- Creature -->
        <div v-if="props.vote.isCreatureHidden">Скрыто</div>
        <div v-if="!props.vote.isCreatureHidden">{{ props.vote.creature.name }}</div>

        <!-- Vote -->
        <div v-if="props.vote.type === 1">Голосует: "Произведение понравилось"</div>
        <div v-if="props.vote.type === 2">Голосует: "Произведение не понравилось"</div>
        <div v-if="props.vote.type === 4">Отзывает голос: "Произведение понравилось"</div>
        <div v-if="props.vote.type === 5">Отзывает голос: "Произведение не понравилось"</div>
    </div>
</template>
