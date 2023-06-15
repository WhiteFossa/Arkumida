<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";
    import {CategoryTagType} from "@/js/constants";

    const props = defineProps({
        id: Guid,
        furryReadableId: String,
        type: Number,
        textsCount: Number,
        tag: String
    })

    const tagLinkHref = ref(null)

    const colorMarkerClasses = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        tagLinkHref.value = "/texts/byTag/" + props.id

        colorMarkerClasses.value = "category-color-marker";

        switch (props.type)
        {
            case CategoryTagType.Normal:
                break;

            case CategoryTagType.Snuff:
                colorMarkerClasses.value += " category-color-marker-snuff";
                break;

            case CategoryTagType.Sandbox:
                colorMarkerClasses.value += " category-color-marker-sandbox";
                break;

            case CategoryTagType.Contest:
                colorMarkerClasses.value += " category-color-marker-contest";
                break;

            default:
                new Error("Unknown category tag type.")
        }
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <div class="category-block">
            <a class="category-link" :href="tagLinkHref" :title="props.tag">{{ props.tag }}</a> ({{ props.textsCount }})

            <span v-if="props.type === CategoryTagType.Sandbox" class="sandbox-category-text-description">(рассказы, требующие доработки)</span>

            <div v-if="props.type !== CategoryTagType.Normal" class="special-category-marker">
                <div :class="colorMarkerClasses"></div>
            </div>
        </div>
    </div>

</template>