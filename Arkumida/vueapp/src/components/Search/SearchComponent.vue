<!-- Search component -->
<script setup>
    import {defineProps, onMounted, reactive, ref} from "vue";
    import router from "@/router";
    import {SearchConstants} from "@/js/constants";
    import {WebClientSendPostRequest} from "@/js/libWebClient";
    import ShortTextInfo from "@/components/MainPage/TextInfos/ShortTextInfo.vue";
    import PaginationComponent from "@/components/Shared/Pagination/PaginationComponent.vue";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    const props = defineProps({
        queryText: String,
        isFromMainPage: Boolean
    })

    const searchFormData = reactive({
        searchText: ""
    })

    const isSearchHelpExpanded = ref(false)

    const searchResult = ref([])

    const currentPageNumber = ref(1)
    const pagesCount = ref(1)

    const isFirstSearchHappehed = ref(false)
    const isSearchInProgress = ref(true)

    const previousQuery = ref("")

    const searchTextInput = ref(null)
    const searchButton = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        // Adding on Enter handler to search text input
        searchTextInput.value.addEventListener("keypress", function(event) {
            if (event.key === "Enter")
            {
                event.preventDefault();
                searchButton.value.click();
            }
        });

        if (!props.isFromMainPage && props.queryText !== "")
        {
            await SetSeachText(props.queryText)

            await MakeSearchQuery(props.queryText)
        }
    }

    async function ExpandSearchHelp()
    {
        isSearchHelpExpanded.value = true
    }

    async function CollapseSearchHelp()
    {
        isSearchHelpExpanded.value = false
    }

    async function SetSeachText(searchText)
    {
        searchFormData.searchText = searchText
    }

    async function MakeSearchQuery(query)
    {
        await router.replace({ path: "/search/" + encodeURIComponent(query) })

        if (props.isFromMainPage)
        {
            return
        }

        await MakePaginatedSearchQuery(query, 0, SearchConstants.PageSize)
    }

    async function MakePaginatedSearchQuery(query, skip, take)
    {
        if (!isFirstSearchHappehed.value)
        {
            isFirstSearchHappehed.value = true
        }

        isSearchInProgress.value = true

        searchResult.value = await (await WebClientSendPostRequest(
            "/api/Search/Texts",
            {
                "query": query,
                "skip": skip,
                "take": take
            })).json()

        pagesCount.value = Math.ceil(searchResult.value.foundTextsTotalCount / SearchConstants.PageSize)

        previousQuery.value = query

        isSearchInProgress.value = false
    }

    async function GoToPage(pageNumber)
    {
        // We can have a situation, when creature clicks a page, but this page doesn't exist for a new query.
        // So in case of query change we are going to the first page
        if (searchFormData.searchText !== previousQuery.value)
        {
            pageNumber = 1
        }

        currentPageNumber.value = pageNumber

        let skip = (currentPageNumber.value - 1) * SearchConstants.PageSize

        await MakePaginatedSearchQuery(searchFormData.searchText, skip, SearchConstants.PageSize)
    }
</script>

<template>
    <div class="search">

        <!-- Search bar -->
        <div class="horizontal-flex search-container">
            <input
                v-model="searchFormData.searchText"
                class="search-text-area"
                name="text"
                type="text"
                placeholder="Введите запрос здесь"
                ref="searchTextInput" />

            <button
                class="search-button"
                type="submit"
                @click="async () => await MakeSearchQuery(searchFormData.searchText)"
                ref="searchButton">
                Искать!
            </button>
        </div>

        <div class="search-help-icon-container">

            <!-- More (search help) -->
            <div
                v-if="!isSearchHelpExpanded"
                class="underlined-pseudolink"
                @click="async () => await ExpandSearchHelp()">
                Помощь по поиску
            </div>

            <!-- Less (search help) -->
            <div
                v-if="isSearchHelpExpanded"
                class="underlined-pseudolink"
                @click="async () => await CollapseSearchHelp()">
                Скрыть помощь по поиску
            </div>
        </div>

        <!-- Search help -->
        <div v-if="isSearchHelpExpanded">
            <div>
                <strong>Подчёркнутые примеры - кликабельны.</strong>
            </div>

            <br>

            <!-- Search by title -->
            <div>
                Поиск по названию:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Название: [краденый мир]')">
                    Название: [краденый мир]
                </strong>
            </div>

            <!-- Search by description -->
            <div>
                Поиск по аннотации:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Аннотация: [чакат]')">
                    Аннотация: [чакат]
                </strong>
            </div>

            <!-- Search by content -->
            <div>
                Поиск по тексту произведения:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Текст: [с хвостами]')">
                    Текст: [с хвостами]
                </strong>
            </div>

            <!-- Search by author -->
            <div>
                Поиск по автору:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Автор: [Redgerra]')">
                    Автор: [Redgerra]
                </strong>
            </div>

            <!-- Include tags -->
            <div>
                Произведения, содержащие <strong>все</strong> указанные теги:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('+Теги: [росомаха, NO YIFF]')">
                    +Теги: [росомаха, NO YIFF]
                </strong>
            </div>

            <!-- Exclude tags -->
            <div>
                Исключить произведения, содержащие <strong>любой</strong> из указанных тегов:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('-Теги: [милитари, война]')">
                    -Теги: [милитари, война]
                </strong>
            </div>

            <!-- Combined -->
            <div>
                Запросы можно комбинировать:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Название: [зверя] +Теги: [фентези] -Теги: [пес]')">
                    Название: [зверя] +Теги: [фентези] -Теги: [пес]
                </strong>
            </div>

            <!-- Unformatted queries -->
            <br>
            <div>
                Если запрос не отформатирован как описано выше, то поиск выполняется <strong>по названию</strong> произведений.
            </div>
        </div>
    </div>

    <!-- Search results -->
    <div v-if="isFirstSearchHappehed">

        <LoadingSymbol v-if="isSearchInProgress" />

        <div v-if="!isSearchInProgress">

            <div v-if="searchResult.foundTexts.length > 0">
                <PaginationComponent :key="currentPageNumber" :currentPage="currentPageNumber" :pagesCount="pagesCount" @goToPage="async (pn) => await GoToPage(pn)" />

                <ShortTextInfo :id="foundText.id" v-for="foundText in searchResult.foundTexts" :key="foundText.id"/>

                <PaginationComponent :key="currentPageNumber" :currentPage="currentPageNumber" :pagesCount="pagesCount" @goToPage="async (pn) => await GoToPage(pn)" />
            </div>

            <div v-if="searchResult.foundTexts.length === 0">
                Ничего не найдено...
            </div>

        </div>
    </div>
</template>