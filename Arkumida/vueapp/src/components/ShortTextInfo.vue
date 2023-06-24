<!-- Text short info -->
<script setup>

    import { defineProps } from 'vue'
    import { Guid } from 'guid-typescript'
    import { ref, onMounted } from 'vue'
    import moment from 'moment'
    import LoadingSymbol from './LoadingSymbol.vue'
    import TagSmall from "@/components/TagSmall.vue";
    import SmallTextIcon from "@/components/SmallTextIcon.vue";
    import LongTextInfo from "@/components/LongTextInfo.vue";
    import {AddIconToList, DetectSpecialTextType, FilterCategoryTags, FilterOrdinaryTags} from "@/js/libArkumida";
    import {Messages, SpecialTextType, TextIconType } from "@/js/constants";
    import CategoryTag from "@/components/CategoryTag.vue";
    
    const props = defineProps({
        id: Guid
    })
    
    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
    // True if loading under way
    const isLoading = ref(true)
    
    // Text info
    const textInfo = ref(null)

    const authorLinkHref = ref(null)
    const authorLinkTitle = ref(null)
    
    const textLinkHref = ref(null)

    const addTime = ref(null)
    
    const commentsHref = ref(null)
    
    const textInfoClasses = ref(null)
    
    const leftIcons = ref([])
    const rightIcons = ref([])

    // If true, then long info popup is shown
    const isLongInfoShown = ref(false)

    const categoryTags = ref([])
    const ordinaryTags = ref([])

    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        textInfo.value = await (await fetch(apiBaseUrl + `/api/Texts/GetInfo/` + props.id)).json()
        
        authorLinkHref.value = "/texts/byAuthor/" + textInfo.value.textInfo.author.entityId
        authorLinkTitle.value = Messages.AllTextsByAuthor + textInfo.value.textInfo.author.name
        
        textLinkHref.value = "/texts/" + textInfo.value.textInfo.entityId

        addTime.value = moment(textInfo.value.textInfo.addTime).format('HH:mm DD.MM.YYYY')

        commentsHref.value = "/texts/discuss/" + textInfo.value.textInfo.entityId

        categoryTags.value = FilterCategoryTags(textInfo.value.textInfo.tags)
        if (categoryTags.value.length === 0)
        {
            throw new Error("At least one category tag must present.")
        }

        ordinaryTags.value = FilterOrdinaryTags(textInfo.value.textInfo.tags)

        let specialTextType = DetectSpecialTextType(textInfo.value.textInfo.tags)
        textInfoClasses.value = "text-short-info-block"
        switch (specialTextType)
        {
            // Normal text
            case SpecialTextType.Normal:
                break

            // Contest
            case SpecialTextType.Contest:
                textInfoClasses.value += " text-short-info-block-contest"
                leftIcons.value.push({ "type": TextIconType.Contest, "url": "" });
                break

            // Sandbox
            case SpecialTextType.Sandbox:
                textInfoClasses.value += " text-short-info-block-sandbox"
                leftIcons.value.push({ "type": TextIconType.Sandbox, "url": "" });
                break

            // Snuff
            case SpecialTextType.Snuff:
                textInfoClasses.value += " text-short-info-block-snuff"
                leftIcons.value.push({ "type": TextIconType.Snuff, "url": "" });
                break
            
            default:
                throw new Error("Unknown text type.")
        }
        
        // Additional icons
        AddIconToList(textInfo.value.textInfo.leftIcons, leftIcons)
        AddIconToList(textInfo.value.textInfo.rightIcons, rightIcons)

        isLoading.value = false
    }

    async function ShowLongTextInfo()
    {
        isLongInfoShown.value = true
    }

    async function HideLongTextInfo()
    {
        isLongInfoShown.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>

        <LongTextInfo v-if="isLongInfoShown === true" @closePopup="HideLongTextInfo" :id="props.id"/>

        <div :class="textInfoClasses">
            
            <!-- Author and title line -->
            <div class="horizontal-flex text-short-info-block-title-line">
                <div>
                    <!-- Left icons -->
                    <SmallTextIcon v-for="leftIcon in leftIcons" :key="leftIcon.type" :type="leftIcon.type" :url="leftIcon.url" />

                    <a class="text-short-info-author-link" :href="authorLinkHref" :title="authorLinkTitle">{{ textInfo.textInfo.author.name }}</a>&nbsp;<a class="text-short-info-text-link" :href="textLinkHref">«{{ textInfo.textInfo.title }}»</a>

                    <!-- Right icons -->
                    <SmallTextIcon v-for="rightIcon in rightIcons" :key="rightIcon.type" :type="rightIcon.type" :url="rightIcon.url" />
                </div>

                <div>
                    <!-- Full info button -->
                    <button class="text-long-info-button" @click="ShowLongTextInfo">Подробнее</button>
                </div>

            </div>
            
            <!-- Statistics line -->
            <div class="text-short-info-block-statistics-line">
                {{ addTime }}&nbsp;
                <img class="text-short-info-block-statistics-line-images" src="/images/glazz.png" alt="Количество просмотров" title="Количество просмотров" />&nbsp;{{ textInfo.textInfo.viewsCount }}&nbsp;
                
                <span v-if="textInfo.textInfo.commentsCount === 0">
                    <img class="text-short-info-block-statistics-line-images" src="/images/oblako.png" alt="Количество комментариев" title="Количество комментариев" />&nbsp;0
                </span>
                <span v-else>
                    <a class="text-short-info-block-comments-link" :href="commentsHref" title="Количество комментариев">
                        <img class="text-short-info-block-statistics-line-images" src="/images/oblako.png" alt="Количество комментариев" title="Количество комментариев" />&nbsp;{{ textInfo.textInfo.commentsCount }}
                    </a>
                </span>

                &nbsp;<img class="text-short-info-block-statistics-line-images" src="/images/vote.png" alt="Голоса за рассказ" title="Голоса за рассказ" />&nbsp;<span class="text-short-info-block-votes-for" v-if="textInfo.textInfo.votesFor > 0">+<strong>{{ textInfo.textInfo.votesFor }}</strong></span><span v-else>Нет</span>
            </div>
            
            <!-- Type and tags line -->
            <div class="text-short-info-block-type-and-tags-line">

                <!-- Categories -->
                <span v-for="tag in categoryTags" :key="tag.entityId">
                    <CategoryTag :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.tag" /><span v-if="tag.entityId !== categoryTags[categoryTags.length - 1].entityId">, </span>
                </span>
                
                <!-- Tags -->
                <span v-if="ordinaryTags.length > 0">
                    #:

                    <span v-for="tag in ordinaryTags" :key="tag.entityId">
                        <TagSmall :id="tag.entityId" :furryReadableId="tag.furryReadableId" :text="tag.tag" /><span v-if="tag.entityId !== ordinaryTags[ordinaryTags.length - 1].entityId">, </span>
                    </span>
                </span>
            </div>
        </div>
    </div>
</template>