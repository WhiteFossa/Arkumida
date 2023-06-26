<template>
    <HeaderComponent />

    <ReadTextComponent />
</template>

<script>

import HeaderComponent from "@/components/HeaderComponent.vue";
import ReadTextComponent from "@/components/ReadTextComponent.vue";

export default {
    name: 'ReadTextView',
    components: {
        ReadTextComponent,
        HeaderComponent
    }
}
</script>