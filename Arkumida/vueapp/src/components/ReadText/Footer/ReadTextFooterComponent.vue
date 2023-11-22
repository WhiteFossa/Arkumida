<script setup>
    import ReadTextLikeComponent from "@/components/ReadText/Footer/Votes/ReadTextLikeComponent.vue";
    import ReadTextDislikeComponent from "@/components/ReadText/Footer/Votes/ReadTextDislikeComponent.vue";
    import {defineProps, ref} from "vue";

    const props = defineProps({
        textId: String
    })

    const likeComponent = ref(null);
    const dislikeComponent = ref(null);

    async function UpdateDislikeStateAsync()
    {
        dislikeComponent.value.ReloadState()
    }

    async function UpdateLikeStateAsync()
    {
        likeComponent.value.ReloadState()
    }
</script>

<template>
    <div class="read-text-footer">

        <!-- Like -->
        <ReadTextLikeComponent
            ref="likeComponent"
            :textId="props.textId"
            @likeStateAboutToChange="async() => await UpdateDislikeStateAsync()"
            @likeStateChanged="async() => await UpdateDislikeStateAsync()"/>

        <!-- Dislike -->
        <ReadTextDislikeComponent
            ref="dislikeComponent"
            :textId="props.textId"
            @dislikeStateAboutToChange="async() => await UpdateLikeStateAsync()"
            @dislikeStateChanged="async() => await UpdateLikeStateAsync()"/>
    </div>
</template>