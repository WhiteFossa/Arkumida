<!-- Small tag (color is fixed) -->
<script setup>

    import { ref, defineProps } from 'vue'
    
    const props = defineProps({
        id: String,
        furryReadableId: String,
        text: String
    })
    
    const tagHref = ref(null)
    const tagTitle = ref(null)
    
    tagHref.value = "/texts/byTag/" + props.id
    tagTitle.value = "Все произведения с тегом " + props.text;

</script>

<template>
    <a class="small-tag-link" :href="tagHref" :title="tagTitle">
        {{ props.text }}
    </a>
</template>