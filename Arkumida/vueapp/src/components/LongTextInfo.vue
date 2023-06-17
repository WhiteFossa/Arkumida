<script setup>

    import {defineEmits, defineProps, onMounted, ref} from 'vue'
    import {Guid} from "guid-typescript";
    import LoadingSymbol from "@/components/LoadingSymbol.vue";
    import SmallTextIcon from "@/components/SmallTextIcon.vue";
    import moment from "moment/moment";
    import TagSmall from "@/components/TagSmall.vue";
    import {AddIconToList, BytesToKilobytesFormatted} from "@/js/libArkumida";
    import {Messages, SpecialTextType, TextIconType, TextType} from "@/js/constants";

    const emit = defineEmits(['closePopup'])

    const props = defineProps({
        id: Guid
    })

    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL

    // True if loading under way
    const isLoading = ref(true)

    // Text info
    const textInfo = ref(null)

    const leftIcons = ref([])
    const rightIcons = ref([])

    const authorLinkHref = ref(null)
    const authorLinkTitle = ref(null)

    const textLinkHref = ref(null)

    const addTime = ref(null)

    const commentsHref = ref(null)

    const publisherLinkHref = ref(null)
    const publisherLinkTitle = ref(null)

    const translatorLinkHref = ref(null)
    const translatorLinkTitle = ref(null)

    const textTypeName = ref(null)
    const textTypeHref = ref(null)
    const textTypeTitle = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad() {
        textInfo.value = await (await fetch(apiBaseUrl + `/api/Texts/GetInfo/` + props.id)).json()

        authorLinkHref.value = "/texts/byAuthor/" + textInfo.value.textInfo.author.entityId
        authorLinkTitle.value = Messages.AllTextsByAuthor + textInfo.value.textInfo.author.name

        textLinkHref.value = "/texts/" + textInfo.value.textInfo.entityId

        addTime.value = moment(textInfo.value.textInfo.addTime).format('HH:mm DD.MM.YYYY')

        commentsHref.value = "/texts/discuss/" + textInfo.value.textInfo.entityId

        publisherLinkHref.value = "/texts/byPublisher/" + textInfo.value.textInfo.publisher.entityId
        publisherLinkTitle.value = Messages.AllTextsByPublisher + textInfo.value.textInfo.publisher.name

        if (textInfo.value.textInfo.translator !== null)
        {
            translatorLinkHref.value = "/texts/byTranslator/" + textInfo.value.textInfo.translator.entityId
            translatorLinkTitle.value = Messages.AllTextsByTranslator + textInfo.value.textInfo.translator.name
        }

        switch (textInfo.value.textInfo.specialType)
        {
            // Normal text
            case SpecialTextType.Normal:
                break

            // Contest
            case SpecialTextType.Contest:
                leftIcons.value.push({ "type": TextIconType.Contest, "url": "" });
                break

            // Sandbox
            case SpecialTextType.Sandbox:
                leftIcons.value.push({ "type": TextIconType.Sandbox, "url": "" });
                break

            // Snuff
            case SpecialTextType.Snuff:
                leftIcons.value.push({ "type": TextIconType.Snuff, "url": "" });
                break

            default:
                new Error("Unknown text type.")
        }

        AddIconToList(textInfo.value.textInfo.leftIcons, leftIcons)
        AddIconToList(textInfo.value.textInfo.rightIcons, rightIcons)

        textTypeHref.value = "/texts/byType/" + textInfo.value.textInfo.type
        switch (textInfo.value.textInfo.type)
        {
            // Story
            case TextType.Story:
                textTypeName.value = Messages.Stories
                textTypeTitle.value = Messages.AllStories
                break

            // Novel
            case TextType.Novel:
                textTypeName.value = Messages.Novels
                textTypeTitle.value = Messages.AllNovels
                break

            // Poetry
            case TextType.Poetry:
                textTypeName.value = Messages.Poetry
                textTypeTitle.value = Messages.AllPoetry
                break

            // Comics
            case TextType.Comics:
                textTypeName.value = Messages.Comics
                textTypeTitle.value = Messages.AllComics
                break

            default:
                new Error("Unknown text type.")
        }

        isLoading.value = false
    }

    async function ClosePopup()
    {
        emit('closePopup')
    }

    // Empty click handler for popup
    async function DoNothing()
    {

    }

</script>

<template>
    <!-- Lower layer -->
    <div class="popup-lower-layer">
    </div>

    <!-- Upper layer -->
    <div class="popup-upper-layer" @click="ClosePopup">
        <div class="popup" @click.stop="DoNothing">

            <button class="popup-close-button" @click="ClosePopup">
                <img class="popup-close-button-image" src="/images/close.svg" alt="Close" />
            </button>

            <div v-if="isLoading">
                <LoadingSymbol />
            </div>
            <div v-else class="text-long-info-block">
                <!-- Long text info container -->

                <!-- Author and title line -->
                <div class="horizontal-flex text-long-info-block-title-line">
                    <!-- Left icons -->
                    <SmallTextIcon v-for="leftIcon in leftIcons" :key="leftIcon.type" :type="leftIcon.type" :url="leftIcon.url" />

                    <a class="text-long-info-author-link" :href="authorLinkHref" :title="authorLinkTitle"><strong>{{ textInfo.textInfo.author.name }}</strong></a>&nbsp;<a class="text-long-info-text-link" :href="textLinkHref"><strong>«{{ textInfo.textInfo.title }}»</strong></a>

                    <!-- Right icons -->
                    <SmallTextIcon v-for="rightIcon in rightIcons" :key="rightIcon.type" :type="rightIcon.type" :url="rightIcon.url" />
                </div>

                <!-- Statistics line -->
                <div class="text-long-info-block-statistics-line">
                    {{ addTime }}&nbsp;
                    <img class="text-long-info-block-statistics-line-images" src="/images/glazz.png" alt="Количество просмотров" title="Количество просмотров" />&nbsp;{{ textInfo.textInfo.viewsCount }}&nbsp;

                    <span v-if="textInfo.textInfo.commentsCount === 0">
                        <img class="text-long-info-block-statistics-line-images" src="/images/oblako.png" alt="Количество комментариев" title="Количество комментариев" />&nbsp;0
                    </span>
                        <span v-else>
                        <a class="text-long-info-block-comments-link" :href="commentsHref" title="Количество комментариев">
                            <img class="text-long-info-block-statistics-line-images" src="/images/oblako.png" alt="Количество комментариев" title="Количество комментариев" />&nbsp;{{ textInfo.textInfo.commentsCount }}
                        </a>
                    </span>

                    &nbsp;<img class="text-long-info-block-statistics-line-images" src="/images/vote.png" alt="Голоса за рассказ" title="Голоса за рассказ" />&nbsp;<span class="text-long-info-block-votes-for" v-if="textInfo.textInfo.votesFor > 0">+<strong>{{ textInfo.textInfo.votesFor }}</strong></span><span v-else>Нет</span>
                </div>

                <!-- Publisher, translator and size -->
                <div class="text-long-info-block-publisher-line">
                    Разместил: <a class="text-long-info-publisher-link" :href="publisherLinkHref" :title="publisherLinkTitle"><strong>{{ textInfo.textInfo.publisher.name }}</strong></a>
                    <span v-if="textInfo.textInfo.translator !== null">
                        Переводчик: <a class="text-long-info-translator-link" :href="translatorLinkHref" :title="translatorLinkTitle"><strong>{{ textInfo.textInfo.translator.name }}</strong></a>
                    </span>
                    Размер:
                    <span v-if="textInfo.textInfo.type === TextType.Comics"><strong>{{ textInfo.textInfo.sizeInPages }} стр.</strong></span>
                    <span v-else><strong>{{ BytesToKilobytesFormatted(textInfo.textInfo.sizeInBytes) }} Кб</strong></span>

                </div>

                <!-- Text description -->
                <div class="text-long-info-block-description">
                    {{ textInfo.textInfo.description }}
                </div>

                <!-- Type and tags line -->
                <div class="text-long-info-block-type-and-tags-line">

                    <!-- Type -->
                    <a class="text-long-info-block-text-type-link" :href="textTypeHref" :title="textTypeTitle">
                        <strong>
                            {{ textTypeName }}
                        </strong>
                    </a>

                    <!-- Tags -->
                    #:

                    <span v-for="tag in textInfo.textInfo.tags" :key="tag.entityId">
                        <TagSmall :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.tag" /><span v-if="tag.entityId !== textInfo.textInfo.tags[textInfo.textInfo.tags.length - 1].entityId">, </span>
                    </span>
                </div>
            </div>
        </div>
    </div>

</template>