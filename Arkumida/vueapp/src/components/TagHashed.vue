<!-- Like TagSmall, but with # and different style -->
<script setup>

import { ref, defineProps } from 'vue'
import { Guid } from 'guid-typescript'

const props = defineProps({
    id: Guid,
    furryReadableId: String,
    text: String
})

const tagHref = ref(null)
const tagTitle = ref(null)

tagHref.value = "/texts/byTag/" + props.id
tagTitle.value = "Все произведения с тегом " + props.text;

</script>

<template>
    #<a class="hashed-tag-link" :href="tagHref" :title="tagTitle">{{ props.text }}</a>
</template>