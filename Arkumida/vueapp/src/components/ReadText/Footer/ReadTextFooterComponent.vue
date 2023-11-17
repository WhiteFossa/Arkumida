<script setup>

    import ReadTextLikeComponent from "@/components/ReadText/Footer/Votes/ReadTextLikeComponent.vue";
    import ReadTextDislikeComponent from "@/components/ReadText/Footer/Votes/ReadTextDislikeComponent.vue";
</script>

<template>
    <div class="read-text-footer">

        <!-- Like -->
        <ReadTextLikeComponent />

        <!-- Dislike -->
        <ReadTextDislikeComponent />
    </div>
</template>