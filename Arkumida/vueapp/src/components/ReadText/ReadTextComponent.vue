<!-- Root component for reading texts -->

<script setup>
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    const props = defineProps({
        id: Guid,
        page: Number
    })

    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import CreatureInfoComponent from "@/components/CreatureInfoComponent.vue";
    import NonexistentCreatureComponent from "@/components/NonexistentCreatureComponent.vue";
    import {DetectTextType, FilterCategoryTags, FilterOrdinaryTags} from "@/js/libArkumida";
    import CategoryTag from "@/components/CategoryTag.vue";
    import TagHashed from "@/components/TagHashed.vue";
    import SectionComponent from "@/components/SectionComponent.vue";
    import TextIllustrationsContainer from "@/components/TextIllustrationsContainer.vue";
    import {TextType} from "@/js/constants";
    import ReadTextPagination from "@/components/ReadText/Pagination/ReadTextPagination.vue";
    import router from "@/router";

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const textData = ref(null)

    const categoryTags = ref([])
    const ordinaryTags = ref([])

    const textType = ref(null)

    const textPage = ref(null)
    const currentPageNumber = ref(0)
    const isPageLoading = ref(true)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        // Loading text metadata
        textData.value = await (await fetch(apiBaseUrl + `/api/Texts/GetReadData/` + props.id)).json()

        categoryTags.value = FilterCategoryTags(textData.value.textData.tags)
        if (categoryTags.value.length === 0)
        {
            throw new Error("At least one category tag must present.")
        }

        ordinaryTags.value = FilterOrdinaryTags(textData.value.textData.tags)

        textType.value = DetectTextType(textData.value.textData.tags)

        // Inital page (from URL)
        isLoading.value = false

        await LoadTextPage(props.page)
    }

    // Load text page. Text id comes from props.id
    async function LoadTextPage(pageNumber)
    {
        // Page number can come from URL, in this case it will be a string, and we need a number for correct
        // comparison
        pageNumber = Number(pageNumber)

        if (pageNumber !== currentPageNumber.value)
        {
            currentPageNumber.value = pageNumber
            isPageLoading.value = true
            textPage.value = await (await fetch(apiBaseUrl + `/api/Texts/GetPage/` + props.id + `/Page/` + currentPageNumber.value)).json()
            isPageLoading.value = false

            // Updating URL in browser address bar without page reload
            router.replace({ path: "/texts/" + props.id + "/page/" + currentPageNumber.value })
        }
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>
    <div v-else class="body-container">

        <div class="read-text-author-publisher-translator-container">

            <!-- Publisher -->
            <CreatureInfoComponent
                :id="textData.textData.publisher.entityId"
                :furryReadableId="textData.textData.publisher.furryReadableId"
                :name="textData.textData.publisher.name"
                creatureRole="Загрузил"
            />

            <!-- Author -->
            <CreatureInfoComponent
                :id="textData.textData.author.entityId"
                :furryReadableId="textData.textData.author.furryReadableId"
                :name="textData.textData.author.name"
                creatureRole="Автор"
            />

            <!-- Translator -->
            <CreatureInfoComponent v-if="textData.textData.translator !== null"
                :id="textData.textData.translator.entityId"
                :furryReadableId="textData.textData.translator.furryReadableId"
                :name="textData.textData.translator.name"
                creatureRole="Переводчик"
            />
            <NonexistentCreatureComponent v-else creatureRole="Переводчик" />

        </div>

        <div class="read-text-title">{{ textData.textData.title }}</div>

        <!-- Categorises -->
        <div class="horizontal-flex flex-center read-text-categories-container">
            <div v-for="tag in categoryTags" :key="tag.entityId">
                <CategoryTag :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.name" />
            </div>
        </div>

        <!-- Tags -->
        <div class="horizontal-flex flex-center read-text-tags-container" v-if="ordinaryTags.length > 0">
            <div v-for="tag in ordinaryTags" :key="tag.entityId">
                <TagHashed :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.name" />
            </div>
        </div>

        <!-- Pagination (top) -->
        <ReadTextPagination :key="currentPageNumber" :currentPage="currentPageNumber" :pagesCount="textData.textData.pagesCount" @goToPage="async (pn) => await LoadTextPage(pn)" />

        <!-- Sections -->
        <LoadingSymbol v-if="isPageLoading"/>
        <div v-else :key="currentPageNumber">
            <div v-for="section in textPage.pageData.sections" :key="section.entityId">
                <SectionComponent :originalText="section.originalText" :variants="section.variants" />
            </div>
        </div>

        <!-- Illustrations (for comics we don't need to show them) -->
        <TextIllustrationsContainer v-if="textType !== TextType.Comics" :illustrations="textData.textData.illustrations" />

        <!-- Pagination (bottom) -->
        <ReadTextPagination :key="currentPageNumber" :currentPage="currentPageNumber" :pagesCount="textData.textData.pagesCount" @goToPage="async (pn) => await LoadTextPage(pn)" />
    </div>
</template>
