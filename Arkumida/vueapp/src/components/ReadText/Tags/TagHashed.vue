<!-- Like TagSmall, but with # and different style -->
<script setup>
    import { ref, defineProps } from 'vue'
    import {GenerateOneTagSearchQuery} from "@/js/libArkumida";

    const props = defineProps({
        tag: Object
    })

    const tagHref = ref(null)
    const tagTitle = ref(null)

    tagHref.value = GenerateOneTagSearchQuery(props.tag.name)
    tagTitle.value = "Все произведения с тегом " + props.tag.name
</script>

<template>
    #<a class="hashed-tag-link" :href="tagHref" :title="tagTitle">{{ props.tag.name }}</a>
</template>