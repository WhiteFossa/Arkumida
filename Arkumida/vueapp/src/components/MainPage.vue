<script setup>
    import TopButton from "@/components/TopButton.vue";
    import SearchComponent from "@/components/SearchComponent.vue";
    import TextsStatisticsComponent from "@/components/TextsStatisticsComponent.vue";
    import MainLogo from "@/components/MainLogo.vue";
    import ShortTextInfosContainer from "@/components/ShortTextInfosContainer.vue";
    import {ref} from "vue";
    import DonateToRedgerra from "@/components/DonateToRedgerra.vue";
    import CategoriesContainer from "@/components/CategoriesContainer.vue";
    import TagsContainer from "@/components/TagsContainer.vue";
    import {TagSubtype} from "@/js/constants";

    const shortTextsInfosDisplayMode = ref("")
    const newestButtonClass = ref("")
    const mostPopularButtonClass = ref("")

    // By default we are showing the newest texts
    ToggleNewestShortTextsInfosMode()

    // Switch to "newest" mode
    async function ToggleNewestShortTextsInfosMode()
    {
        if (shortTextsInfosDisplayMode.value !== "newest")
        {
            shortTextsInfosDisplayMode.value = "newest"
            newestButtonClass.value = "texts-short-infos-button-active"
            mostPopularButtonClass.value = "texts-short-infos-button"
        }
    }

    // Switch to "most popular" mode
    async function ToggleMostPopularShortTextsInfosMode()
    {
        if (shortTextsInfosDisplayMode.value !== "mostPopular")
        {
            shortTextsInfosDisplayMode.value = "mostPopular"
            newestButtonClass.value = "texts-short-infos-button"
            mostPopularButtonClass.value = "texts-short-infos-button-active"
        }
    }

</script>

<template>
    <div class="body-container">
        <!-- Main page code -->
        
        <!-- Top buttons -->
        <div class="horizontal-flex top-buttons-container">
            <TopButton href="/addText" text="Добавить"/>
            <TopButton href="/collectiveTranslation" text="Совместный перевод" smallText="Сейчас переводятся: 2"/>
            <TopButton href="/contests" text="Конкурсы" />
        </div>
        
        <!-- Search -->
        <SearchComponent />
        
        <!-- Texts statistics -->
        <TextsStatisticsComponent />
        
        <!-- Logo and texts list -->
        <div class="horizontal-flex logo-and-texts-container">
            <div class="main-logo-container">
                <div>
                    <div class="horizontal-flex texts-short-infos-buttons-container">
                        <!-- Texts list control buttons -->
                        <button :class="newestButtonClass" @click="ToggleNewestShortTextsInfosMode">Новые</button>
                        <button :class="mostPopularButtonClass" @click="ToggleMostPopularShortTextsInfosMode">Популярные</button>
                    </div>

                    <MainLogo />
                </div>
            </div>
            
            <!-- Texts container -->
            <ShortTextInfosContainer v-if="shortTextsInfosDisplayMode === 'newest'" dataSource="/api/Texts/Latest" />
            <ShortTextInfosContainer v-if="shortTextsInfosDisplayMode === 'mostPopular'" dataSource="/api/Texts/Popular" />
        </div>

        <!-- Donate to Redgerra -->
        <DonateToRedgerra />

        <!-- Categories and tags -->
        <div class="horizontal-flex categories-and-tags-contaner">
            <div class="categories-container">
                <div class="categories-and-tags-title">
                    РАЗДЕЛЫ
                </div>

                <div>
                    <CategoriesContainer />
                </div>
            </div>

            <div class="tags-container">
                <div class="categories-and-tags-title">
                    ТЕГИ
                </div>

                <div>
                    <TagsContainer :subtype="TagSubtype.Participants" />
                    <TagsContainer :subtype="TagSubtype.Species" />
                    <TagsContainer :subtype="TagSubtype.Setting" />
                    <TagsContainer :subtype="TagSubtype.Actions" />
                </div>
            </div>
        </div>
    </div>
</template>
