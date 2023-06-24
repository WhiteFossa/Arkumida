<!-- Small text icon -->
<script setup>

    import { ref, defineProps } from 'vue'
    import {TextIconType} from "@/js/constants";
    
    const props = defineProps({
        type: Number,
        url: { type: String, default: "" }
    })
    
    const iconUrl = ref(null)
    const iconText = ref(null)

    iconUrl.value = "/images/text-icons/"
    switch (props.type)
    {
        // Contest
        case TextIconType.Contest:
            iconUrl.value += "icon_contest.png"
            iconText.value = "Конкурсное произведение"
            break
    
        // Sandbox
        case TextIconType.Sandbox:
            iconUrl.value += "icon_sandbox.png"
            iconText.value = "Песочница"
            break
    
        // Snuff
        case TextIconType.Snuff:
            iconUrl.value += "icon_snuff.png"
            iconText.value = "Снафф"
            break

        // Have illustrations
        case TextIconType.Illustrations:
            iconUrl.value += "icon_illustrations.png"
            iconText.value = "Есть иллюстрации"
            break

        // Incomplete
        case TextIconType.Incomplete:
            iconUrl.value += "icon_incomplete.png"
            iconText.value = "Не закончено"
            break

        // MLP
        case TextIconType.Mlp:
            iconUrl.value += "icon_mlp.png"
            iconText.value = "MLP"
            break

        // Series
        case TextIconType.Series:
            iconUrl.value += "icon_series.png"
            iconText.value = "Часть серии"
            break
    
        default:
            throw new Error("Unknown text type.")
    }

</script>

<template>
    <span class="small-text-icon" v-if="url === ''">
        <img :src="iconUrl" :alt="iconText" :title="iconText" />
    </span>
    <span class="small-text-icon" v-else>
        <a :href="url" :title="iconText"><img :src="iconUrl" :alt="iconText" :title="iconText" /></a>
    </span>
    
</template>