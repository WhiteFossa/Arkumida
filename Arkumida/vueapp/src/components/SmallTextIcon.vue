<!-- Small text icon -->
<script setup>

    import { ref, defineProps } from 'vue'
    
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
        case 0:
            iconUrl.value += "icon_contest.png"
            iconText.value = "Конкурсное произведение"
            break
    
        // Sandbox
        case 1:
            iconUrl.value += "icon_sandbox.png"
            iconText.value = "Песочница"
            break
    
        // Snuff
        case 2:
            iconUrl.value += "icon_snuff.png"
            iconText.value = "Снафф"
            break

        // Have illustrations
        case 3:
            iconUrl.value += "icon_illustrations.png"
            iconText.value = "Есть иллюстрации"
            break

        // Incomplete
        case 4:
            iconUrl.value += "icon_incomplete.png"
            iconText.value = "Не закончено"
            break

        // MLP
        case 5:
            iconUrl.value += "icon_mlp.png"
            iconText.value = "MLP"
            break

        // Series
        case 6:
            iconUrl.value += "icon_series.png"
            iconText.value = "Часть серии"
            break
    
        default:
            new Error("Unknown text type.")
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