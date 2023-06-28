<!-- Component to display text section -->
<script setup>
import {defineProps, ref} from "vue";
import {SplitTextToParagraphs} from "@/js/libArkumida";

    const props = defineProps({
        originalText: String, // Original text, it is used when section contains bilingual text
        variants: [Object] // Text variants, Russian texts are here
    })

    // Ordering variants by time
    const orderedVariants = ref([])
    orderedVariants.value = [...props.variants];
    orderedVariants.value.sort((a, b) => b.creationTime - a.creationTime);

    // Last variant - for now we display only it
    var paragraphs = SplitTextToParagraphs(orderedVariants.value[0].content)

</script>

<template>
    <p class="text-paragraph" v-for="paragraph in paragraphs" :key="paragraph">
        {{ paragraph }}
    </p>
</template>