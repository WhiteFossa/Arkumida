<!-- Text short info -->
<script setup>

    import { defineProps } from 'vue'
    import { Guid } from 'guid-typescript'
    import { ref, onMounted } from 'vue'
    import moment from 'moment'
    import LoadingSymbol from './LoadingSymbol.vue'
    import TagSmall from "@/components/TagSmall.vue";
    import SmallTextIcon from "@/components/SmallTextIcon.vue";
    
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
    
    const textTypeName = ref(null)
    const textTypeHref = ref(null)
    const textTypeTitle = ref(null)
    
    const textInfoClasses = ref(null)
    
    const leftIcons = ref([])
    const rightIcons = ref([])
    
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
        authorLinkTitle.value = "Все произведения автора " + textInfo.value.textInfo.author.name
        
        textLinkHref.value = "/texts/" + textInfo.value.textInfo.entityId

        addTime.value = moment(textInfo.value.textInfo.addTime).format('HH:mm DD.MM.YYYY')

        commentsHref.value = "/texts/discuss/" + textInfo.value.textInfo.entityId
        
        textTypeHref.value = "/texts/byType/" + textInfo.value.textInfo.type
        switch (textInfo.value.textInfo.type)
        {
            // Story
            case 0:
                textTypeName.value = "Рассказы"
                textTypeTitle.value = "Все рассказы"
                break

            // Novel
            case 1:
                textTypeName.value = "Повести и Романы"
                textTypeTitle.value = "Все повести и романы"
                break

            // Poetry
            case 2:
                textTypeName.value = "Стихи"
                textTypeTitle.value = "Все стихи"
                break

            // Comics
            case 3:
                textTypeName.value = "Комиксы"
                textTypeTitle.value = "Все комиксы"
                break
                
            default:
                new Error("Unknown text type.")
        }
        
        textInfoClasses.value = "text-short-info-block"
        switch (textInfo.value.textInfo.specialType)
        {
            // Normal text
            case 0:
                break

            // Contest
            case 1:
                textInfoClasses.value += " text-short-info-block-contest"
                leftIcons.value.push({ "type": 0, "url": "" });
                break

            // Sandbox
            case 2:
                textInfoClasses.value += " text-short-info-block-sandbox"
                leftIcons.value.push({ "type": 1, "url": "" });
                break

            // Snuff
            case 3:
                textInfoClasses.value += " text-short-info-block-snuff"
                leftIcons.value.push({ "type": 2, "url": "" });
                break
            
            default:
                new Error("Unknown text type.")
        }
        
        // Debuggin'
        leftIcons.value.push({ "type": 5, "url": "" });
        
        rightIcons.value.push({ "type": 3, "url": "" });
        rightIcons.value.push({ "type": 4, "url": "" });
        rightIcons.value.push({ "type": 6, "url": "http://fchan.us" });
        
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <div :class="textInfoClasses">
            
            <!-- Author and title line -->
            <div class="text-short-info-block-title-line">
                
                <!-- Left icons -->
                <span v-for="leftIcon in leftIcons" :key="leftIcon.type">
                    <SmallTextIcon :type="leftIcon.type" :url="leftIcon.url" />
                </span>
                
                <a class="text-short-info-author-link" :href="authorLinkHref" :title="authorLinkTitle">{{ textInfo.textInfo.author.name }}</a>&nbsp;<a class="text-short-info-text-link" :href="textLinkHref">«{{ textInfo.textInfo.title }}»</a>

                <!-- Right icons -->
                <span v-for="rightIcon in rightIcons" :key="rightIcon.type">
                    <SmallTextIcon :type="rightIcon.type" :url="rightIcon.url" />
                </span>
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

                &nbsp;<img class="text-short-info-block-statistics-line-images" src="/images/vote.png" alt="Голоса за рассказ" title="Голоса за рассказ" />&nbsp;<span class="text-short-info-block-votes-for" v-if="textInfo.textInfo.votesFor > 0">{{ textInfo.textInfo.votesFor }}</span><span v-else>Нет</span>
            </div>
            
            <!-- Type and tags line -->
            <div class="text-short-info-block-type-and-tags-line">
                
                <!-- Type -->
                <a class="text-short-info-block-text-type-link" :href="textTypeHref" :title="textTypeTitle">
                    <strong>
                        {{ textTypeName }}
                    </strong>
                </a>
                
                <!-- Tags -->
                #:
                
                <span v-for="tag in textInfo.textInfo.tags" :key="tag.entityId">
                    <TagSmall :id="tag.entityId" :humanReadableId="tag.humanReadableId" :text="tag.tag" /><span v-if="tag.entityId !== textInfo.textInfo.tags[textInfo.textInfo.tags.length - 1].entityId">, </span>
                </span>
            </div>
        </div>
    </div>
</template>