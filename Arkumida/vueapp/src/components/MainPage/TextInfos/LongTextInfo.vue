<script setup>

    import {defineEmits, defineProps, onMounted, ref} from 'vue'
    import {Guid} from "guid-typescript";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import SmallTextIcon from "@/components/MainPage/TextInfos/SmallTextIcon.vue";
    import moment from "moment/moment";
    import TagSmall from "@/components/MainPage/TextInfos/TagSmall.vue";
    import {
        AddIconToList,
        BytesToKilobytesFormatted, DetectSpecialTextType,
        DetectTextType,
        FilterCategoryTags,
        FilterOrdinaryTags, GenerateLinkToText, InjectInclompleteIcon, InjectMlpIcon, IsMlpText
    } from "@/js/libArkumida";
    import {Messages, SpecialTextType, TextIconType, TextType} from "@/js/constants";
    import CategoryTag from "@/components/Shared/CategoryTag.vue";
    import {WebClientSendGetRequest} from "@/js/libWebClient";

    const emit = defineEmits(['closePopup'])

    const props = defineProps({
        id: Guid
    })

    // True if loading under way
    const isLoading = ref(true)

    // Text info
    const textInfo = ref(null)

    const leftIcons = ref([])
    const rightIcons = ref([])

    const authorsLinks = ref([])

    const textLinkHref = ref(null)

    const addTime = ref(null)

    const commentsHref = ref(null)

    const publisherLinkHref = ref(null)
    const publisherLinkTitle = ref(null)

    const translatorsLinks = ref([])

    const categoryTags = ref([])
    const ordinaryTags = ref([])

    const textType = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad() {
        textInfo.value = await (await WebClientSendGetRequest("/api/Texts/GetInfo/" + props.id)).json()

        textInfo.value.textInfo.authors.forEach((author) =>
        {
            let authorLink =
                {
                    id: author.entityId,
                    name: author.name,
                    href: "/texts/byAuthor/" + author.entityId,
                    title: Messages.AllTextsByAuthor + author.name
                }

            authorsLinks.value.push(authorLink)
        });

        textLinkHref.value = GenerateLinkToText(textInfo.value.textInfo.entityId, 1)

        addTime.value = moment(textInfo.value.textInfo.addTime).format('HH:mm DD.MM.YYYY')

        commentsHref.value = "/texts/discuss/" + textInfo.value.textInfo.entityId

        publisherLinkHref.value = "/texts/byPublisher/" + textInfo.value.textInfo.publisher.entityId
        publisherLinkTitle.value = Messages.AllTextsByPublisher + textInfo.value.textInfo.publisher.name

        textInfo.value.textInfo.translators.forEach((translator) =>
        {
            let translatorLink =
                {
                    id: translator.entityId,
                    name: translator.name,
                    href: "/texts/byTranslator/" + translator.entityId,
                    title: Messages.AllTextsByTranslator + translator.name
                }

            translatorsLinks.value.push(translatorLink)
        })

        console.log(translatorsLinks)

        categoryTags.value = FilterCategoryTags(textInfo.value.textInfo.tags)
        if (categoryTags.value.length === 0)
        {
            throw new Error("At least one category tag must present.")
        }

        ordinaryTags.value = FilterOrdinaryTags(textInfo.value.textInfo.tags)

        textType.value = DetectTextType(textInfo.value.textInfo.tags)

        // Is MPL?
        let isMlp = IsMlpText(textInfo.value.textInfo.tags)
        if (isMlp)
        {
            InjectMlpIcon(leftIcons, isMlp)
        }

        let specialTextType = DetectSpecialTextType(textInfo.value.textInfo.tags)
        switch (specialTextType)
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
                throw new Error("Unknown text type.")
        }

        // Is incomplete text? (we mark it with special icon)
        if (textInfo.value.textInfo.isIncomplete)
        {
            InjectInclompleteIcon(rightIcons)
        }

        AddIconToList(textInfo.value.textInfo.leftIcons, leftIcons)
        AddIconToList(textInfo.value.textInfo.rightIcons, rightIcons)

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
    <div class="long-text-info-popup-lower-layer">
    </div>

    <!-- Upper layer -->
    <div class="long-text-info-popup-upper-layer" @click="ClosePopup">
        <div class="long-text-info-popup" @click.stop="DoNothing">

            <button class="long-text-info-popup-close-button" @click="ClosePopup">
                <img class="long-text-info-popup-close-button-image" src="/images/close.svg" alt="Close popup" />
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

                    <span v-for="authorLink in authorsLinks" :key="authorLink.id">
                        <a class="black-link-without-underline" :href="authorLink.href" :title="authorLink.title">{{ authorLink.name }}</a>
                        <span v-if="authorLink.id !== authorsLinks[authorsLinks.length - 1].id" class="spacer">,</span>
                    </span>&nbsp;<a class="black-link-without-underline" :href="textLinkHref"><strong>«{{ textInfo.textInfo.title }}»</strong></a>

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
                    Разместил: <a class="black-link-without-underline" :href="publisherLinkHref" :title="publisherLinkTitle"><strong>{{ textInfo.textInfo.publisher.name }}</strong></a>

                    <span v-if="translatorsLinks.length > 0">
                        Переводчик(и):

                        <span v-for="translatorLink in translatorsLinks" :key="translatorLink.id">
                            <a class="black-link-without-underline" :href="translatorLink.href" :title="translatorLink.title"><strong>{{ translatorLink.name }}</strong></a>
                            <span v-if="translatorLink.id !== translatorsLinks[translatorsLinks.length - 1].id" class="spacer">,</span>
                        </span>
                    </span>

                    Размер:
                    <span v-if="textType === TextType.Comics">
                        <strong>{{ textInfo.textInfo.sizeInPages }} стр.</strong>
                    </span>
                    <span v-else>
                        <strong>{{ BytesToKilobytesFormatted(textInfo.textInfo.sizeInBytes) }} Кб</strong>
                    </span>

                </div>

                <!-- Text description -->
                <div class="text-long-info-block-description">
                    {{ textInfo.textInfo.description }}
                </div>

                <!-- Type and tags line -->
                <div class="text-long-info-block-type-and-tags-line">

                    <!-- Categories -->
                    <span v-for="tag in categoryTags" :key="tag.entityId">
                        <CategoryTag :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.tag" />
                        <span v-if="tag.entityId !== categoryTags[categoryTags.length - 1].entityId" class="spacer">,</span>
                    </span>

                    <!-- Tags -->
                    <span v-if="ordinaryTags.length > 0">
                        #:

                        <span v-for="tag in ordinaryTags" :key="tag.entityId">
                            <TagSmall :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.tag" />
                            <span v-if="tag.entityId !== ordinaryTags[ordinaryTags.length - 1].entityId" class="spacer">,</span>
                        </span>
                    </span>
                </div>
            </div>
        </div>
    </div>

</template>