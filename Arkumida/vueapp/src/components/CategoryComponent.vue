<script setup>

    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";
    import {CategoryTagType} from "@/js/constants";

    const props = defineProps({
        id: Guid
    })

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const categoryInfo = ref(null)

    const tagLinkHref = ref(null)

    const colorMarkerClasses = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        categoryInfo.value = (await (await fetch(apiBaseUrl + `/api/Categories/` + props.id)).json()).categoryTag

        tagLinkHref.value = "/texts/byTag/" + categoryInfo.value.entityId

        colorMarkerClasses.value = "category-color-marker";

        switch (categoryInfo.value.type)
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

        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <div class="category-block">
            <a class="category-link" :href="tagLinkHref" :title="categoryInfo.tag">{{ categoryInfo.tag }}</a> ({{ categoryInfo.textsCount }})

            <span v-if="categoryInfo.type === CategoryTagType.Sandbox" class="sandbox-category-text-description">(рассказы, требующие доработки)</span>

            <div v-if="categoryInfo.type !== CategoryTagType.Normal" class="special-category-marker">
                <div :class="colorMarkerClasses"></div>
            </div>
        </div>
    </div>

</template>