<!-- Root component for reading texts -->

<script setup>
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import {DetectTextType, FilterCategoryTags, FilterOrdinaryTags} from "@/js/libArkumida";
    import CategoryTag from "@/components/Shared/CategoryTag.vue";
    import TagHashed from "@/components/ReadText/Tags/TagHashed.vue";
    import SectionComponent from "@/components/ReadText/SectionComponent.vue";
    import TextIllustrationsContainer from "@/components/ReadText/Illustrations/TextIllustrationsContainer.vue";
    import {TextType} from "@/js/constants";
    import ReadTextPagination from "@/components/Shared/Pagination/PaginationComponent.vue";
    import router from "@/router";
    import CreaturesInfoComponent from "@/components/ReadText/Creatures/CreaturesInfoComponent.vue";
    import ReadTextDownloadComponent from "@/components/ReadText/Download/ReadTextDownloadComponent.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const props = defineProps({
        id: Guid,
        page: Number
    })

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
        textData.value = await (await WebClientSendGetRequest("/api/Texts/GetReadData/" + props.id)).json()

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

        if (pageNumber < 1 || pageNumber > textData.value.textData.pagesCount)
        {
            return;
        }

        if (pageNumber !== currentPageNumber.value)
        {
            currentPageNumber.value = pageNumber
            isPageLoading.value = true
            textPage.value = await (await WebClientSendGetRequest("/api/Texts/GetPage/" + props.id + "/Page/" + currentPageNumber.value)).json()
            isPageLoading.value = false

            // Updating URL in browser address bar without page reload
            await router.replace({ path: "/texts/" + props.id + "/page/" + currentPageNumber.value })
        }
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>
    <div v-else class="body-container">

        <!-- Download -->
        <div class="read-text-download-container">
            <ReadTextDownloadComponent :plaintextFileId="textData.textData.plainTextFile.id" />
        </div>

        <div class="read-text-author-publisher-translator-container">

            <!-- Publisher -->
            <CreaturesInfoComponent
                :creaturesIds="[textData.textData.publisher.entityId]"
                creaturesRole="Загрузил" />

            <!-- Authors -->
            <CreaturesInfoComponent
                :creaturesIds="textData.textData.authors.map(a => a.entityId)"
                creaturesRole="Автор(ы)"/>

            <!-- Translators -->
            <CreaturesInfoComponent
                :creaturesIds="textData.textData.translators.map(t => t.entityId)"
                creaturesRole="Переводчик(и)"/>
        </div>

        <div class="read-text-title">{{ textData.textData.title }}</div>

        <!-- Categorises -->
        <div class="horizontal-flex flex-center read-text-categories-container">
            <div v-for="tag in categoryTags" :key="tag.entityId">
                <CategoryTag :tag="tag" />
            </div>
        </div>

        <!-- Tags -->
        <div class="horizontal-flex flex-center read-text-tags-container" v-if="ordinaryTags.length > 0">
            <div v-for="tag in ordinaryTags" :key="tag.entityId">
                <TagHashed :tag="tag" />
            </div>
        </div>

        <!-- Pagination (top) -->
        <ReadTextPagination :key="currentPageNumber" :currentPage="currentPageNumber" :pagesCount="textData.textData.pagesCount" @goToPage="async (pn) => await LoadTextPage(pn)" />

        <!-- Sections -->
        <LoadingSymbol v-if="isPageLoading"/>
        <div v-else :key="currentPageNumber">
            <div v-for="section in textPage.pageData.sections" :key="section.entityId">
                <SectionComponent :originalText="section.originalText" :variants="section.variants" @goToNextPage="async() => await LoadTextPage(currentPageNumber + 1)"/>
            </div>
        </div>

        <!-- Illustrations (for comics we don't need to show them) -->
        <TextIllustrationsContainer v-if="textType !== TextType.Comics" :illustrations="textData.textData.illustrations" />

        <!-- Pagination (bottom) -->
        <ReadTextPagination :key="currentPageNumber" :currentPage="currentPageNumber" :pagesCount="textData.textData.pagesCount" @goToPage="async (pn) => await LoadTextPage(pn)" />
    </div>
</template>
