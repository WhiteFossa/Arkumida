<!-- Search component -->
<script setup>
import {defineProps, onMounted, reactive, ref} from "vue";
import router from "@/router";

    const props = defineProps({
        queryText: String,
        isCalledFromMainPage: Boolean
    })

    const searchFormData = reactive({
        searchText: ""
    })

    const isSearchHelpExpanded = ref(false)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        if (props.queryText !== "")
        {
            await SetSeachText(props.queryText)
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
        if (props.isCalledFromMainPage)
        {
            // If we are called from main page - we are going to special search page
            await router.replace({ path: "/search/" + encodeURIComponent(query) })
            return
        }

        // Making query
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
                type="text" />

            <button
                class="search-button"
                type="submit"
                @click="async () => await MakeSearchQuery(searchFormData.searchText)">
                Искать!
            </button>
        </div>

        <div class="search-help-icon-container">

            <!-- More (search help) button -->
            <button
                v-if="!isSearchHelpExpanded"
                class="button-with-image"
                type="button"
                title="Показать помощь по поиску"
                @click="async () => await ExpandSearchHelp()">
                <img class="search-help-icon" src="/images/icons/icon_double_arrow_down.webp" alt="Показать помощь по поиску" title="Показать помощь по поиску" />
            </button>

            <!-- Less (search help) button -->
            <button
                v-if="isSearchHelpExpanded"
                class="button-with-image"
                type="button"
                title="Скрыть помощь по поиску"
                @click="async () => await CollapseSearchHelp()">
                <img class="search-help-icon" src="/images/icons/icon_double_arrow_up.webp" alt="Скрыть помощь по поиску" title="Скрыть помощь по поиску" />
            </button>
        </div>

        <!-- Search help -->
        <div v-if="isSearchHelpExpanded">
            <!-- Search by title -->
            <div>
                Поиск по названию: <em>Название: [текст для поиска]</em>, пример:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Название: [краденый мир]')">
                    Название: [краденый мир]
                </strong>
            </div>

            <!-- Search by description -->
            <div>
                Поиск по аннотации: <em>Аннотация: [текст для поиска]</em>, пример:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Аннотация: [чакат]')">
                    Аннотация: [чакат]
                </strong>
            </div>

            <!-- Search by content -->
            <div>
                Поиск по тексту произведения: <em>Текст: [текст для поиска]</em>, пример:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Текст: [с хвостами]')">
                    Текст: [с хвостами]
                </strong>
            </div>

            <!-- Search by author -->
            <div>
                Поиск по автору: <em>Автор: [имя автора]</em>, пример:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Автор: [Redgerra]')">
                    Автор: [Redgerra]
                </strong>
            </div>

            <!-- Include tags -->
            <div>
                Произведения, содержащие все указанные теги: <em>+Теги: [тег1, тег2]</em>, пример:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('+Теги: [росомаха, NO YIFF]')">
                    +Теги: [росомаха, NO YIFF]
                </strong>
            </div>

            <!-- Exclude tags -->
            <div>
                Исключить произведения, содержащие любой из указанных тегов: <em>-Теги: [тег1, тег2]</em>, пример:
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('-Теги: [милитари, война]')">
                    -Теги: [милитари, война]
                </strong>
            </div>

            <!-- Combined -->
            <div>
                Запросы можно комбинировать, например
                <strong
                    class="underlined-pseudolink"
                    @click="async () => await SetSeachText('Название: [зверя] +Теги: [фентези] -Теги: [пес]')">
                    Название: [зверя] +Теги: [фентези] -Теги: [пес]
                </strong>
            </div>

            <!-- Unformatted queries -->
            <div>
                Если запрос не отформатирован как описано выше, то поиск выполняется по названию произведений.
            </div>
        </div>
    </div>
</template>