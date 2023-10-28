<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {CategoryTagType} from "@/js/constants";
    import {GenerateOneTagSearchQuery} from "@/js/libArkumida";

    const props = defineProps({
        category: Object
        /*type: Number,
        textsCount: Number,
        tag: String*/
    })

    const isLoading = ref(true)

    const tagLinkHref = ref(null)

    const colorMarkerClasses = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        tagLinkHref.value = GenerateOneTagSearchQuery(props.category.tag)

        colorMarkerClasses.value = "category-color-marker";

        switch (props.category.type)
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
                throw new Error("Unknown category tag type.")
        }

        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <div class="category-block">
            <a class="category-link" :href="tagLinkHref" :title="props.category.tag">{{ props.category.tag }}</a> ({{ props.category.textsCount }})

            <span v-if="props.category.type === CategoryTagType.Sandbox" class="sandbox-category-text-description">(рассказы, требующие доработки)</span>

            <div v-if="props.category.type !== CategoryTagType.Normal" class="special-category-marker">
                <div :class="colorMarkerClasses"></div>
            </div>
        </div>
    </div>

</template>