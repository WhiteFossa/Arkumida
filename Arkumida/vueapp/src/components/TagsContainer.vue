<script setup>
    import TagComponent from "@/components/TagComponent.vue";
    import {defineProps, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";
    import {Messages, TagSubtype} from "@/js/constants";

    const props = defineProps({
        subtype: TagSubtype
    })

    const apiBaseUrl = process.env.VUE_APP_API_URL

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
        const response = await (await fetch(apiBaseUrl + '/api/Tags/List?subtype=' + props.subtype)).json()
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
                new Error("Unknown tag subtype.")
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
            <TagComponent :id="tag.entityId" :furryReadableId="tag.furryReadableId" :sizeCategory="tag.sizeCategory" :tag="tag.tag" />
            <span v-if="tag.entityId !== tags[tags.length - 1].entityId">{{ tagsSpacer }}</span>
        </span>
    </div>
</template>
