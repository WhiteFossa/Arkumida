<script setup>

    import {defineProps} from "vue";

    const props = defineProps({
        textId: String
    })

    async function OpenVotesHistory()
    {
        const windowFeatures = "left=100,top=100,width=400,height=600"
        window.open("/votesHistory/" + props.textId, "votesHistory", windowFeatures)
    }

</script>

<template>
    <div
        class="read-text-votes-history-button"
        @click="async() => await OpenVotesHistory()">

        <div>
            <img class="read-text-like-votes-history-image" src="/images/icons/icon_list.webp" alt="История голосов за произведение" />
        </div>

        <div>
            История
        </div>
    </div>
</template>
