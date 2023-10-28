<!-- Category tag -->
<script setup>
    import { ref, defineProps } from 'vue'
    import {GenerateOneTagSearchQuery} from "@/js/libArkumida";

    const props = defineProps({
        tag: Object
    })

    const tagHref = ref(null)
    const tagTitle = ref(null)

    tagHref.value = GenerateOneTagSearchQuery(props.tag.tag)
    tagTitle.value = "Все произведения из категории " + props.tag.tag;
</script>

<template>
    <strong>
        <a class="category-tag-link" :href="tagHref" :title="tagTitle">
            {{ props.tag.tag }}
        </a>
    </strong>
</template>