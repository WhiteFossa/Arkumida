<script setup>
    import TagComponent from "@/components/MainPage/Tags/TagComponent.vue";
    import {defineProps, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {Messages, TagSubtype} from "@/js/constants";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const props = defineProps({
        subtype: Number
    })

    const isLoading = ref(true)

    const tags = ref([])

    const tagsSpacer = ref(" ")

    const subtypeName = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        const response = await (await WebClientSendGetRequest("/api/Tags/List?subtype=" + props.subtype)).json()
        tags.value = response.tags

        switch(props.subtype)
        {
            case TagSubtype.Participants:
                subtypeName.value = Messages.Participants
                break;

            case TagSubtype.Species:
                subtypeName.value = Messages.Species
                break;

            case TagSubtype.Setting:
                subtypeName.value = Messages.Setting
                break;

            case TagSubtype.Actions:
                subtypeName.value = Messages.Actions
                break;

            default:
                throw new Error("Unknown tag subtype.")
        }

        isLoading.value = false
    }
</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>

    <div v-else class="tags-subtype-container">
        <strong>{{ subtypeName }}</strong>:

        <span v-for="tag in tags" :key="tag.entityId">
            <TagComponent :tag="tag" />
            <span v-if="tag.entityId !== tags[tags.length - 1].entityId">{{ tagsSpacer }}</span>
        </span>
    </div>
</template>
