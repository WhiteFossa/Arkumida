<script setup>
    import {defineProps} from "vue";
    import moment from "moment/moment";
    import {AvatarClass, Messages} from "@/js/constants";
    import AvatarComponent from "@/components/Shared/AvatarComponent.vue";

    const props = defineProps({
        vote: Object
    })
</script>

<template>
    <div class="votes-history-item-container">

        <div class="votes-history-item-timestamp">{{ moment(props.vote.timestamp).format('HH:mm DD.MM.YYYY') }}</div>

        <!-- Creature -->
        <div class="votes-history-item-creature">

            <div v-if="props.vote.isCreatureHidden">
                <AvatarComponent :avatar="null" :avatarClass="AvatarClass.Small" />
                <span class="creatures-info-link">Скрыто</span>
            </div>

            <div v-if="!props.vote.isCreatureHidden">
                <a :href="'/users/' + props.vote.creature.entityId" :title="Messages.CreatureUser + props.vote.creature.title" target="_blank">
                    <AvatarComponent :avatar="props.vote.creature.currentAvatar" :avatarClass="AvatarClass.Small" />
                </a>
                <a class="creatures-info-link" :href="'/users/' + props.vote.creature.entityId" :title="Messages.CreatureUser + props.vote.creature.title" target="_blank">{{ props.vote.creature.name }}</a>
            </div>

        </div>

        <!-- Vote -->
        <div v-if="props.vote.type === 1">Голосует: "Понравилось"</div>
        <div v-if="props.vote.type === 2">Голосует: "Не понравилось"</div>
        <div v-if="props.vote.type === 4">Отзывает голос: "Понравилось"</div>
        <div v-if="props.vote.type === 5">Отзывает голос: "Не понравилось"</div>
    </div>
</template>
