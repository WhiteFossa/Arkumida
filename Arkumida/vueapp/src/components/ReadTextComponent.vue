<!-- Root component for reading texts -->

<script setup>
    import LoadingSymbol from "@/components/LoadingSymbol.vue";

    const props = defineProps({
        id: Guid
    })

    import {defineProps, onMounted, ref} from "vue";
    import {Guid} from "guid-typescript";
    import CreatureInfoComponent from "@/components/CreatureInfoComponent.vue";
    import NonexistentCreatureComponent from "@/components/NonexistentCreatureComponent.vue";

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const isLoading = ref(true)

    const textData = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        textData.value = await (await fetch(apiBaseUrl + `/api/Texts/GetReadData/` + props.id)).json()

        isLoading.value = false
    }

</script>

<template>
    <LoadingSymbol v-if="isLoading"/>
    <div v-else class="body-container">

        <div class="read-text-author-publisher-translator-container">

            <!-- Publisher -->
            <CreatureInfoComponent
                :id="textData.textData.publisher.entityId"
                :furryReadableId="textData.textData.publisher.furryReadableId"
                :name="textData.textData.publisher.name"
                creatureRole="Загрузил"
            />

            <!-- Author -->
            <CreatureInfoComponent
                :id="textData.textData.author.entityId"
                :furryReadableId="textData.textData.author.furryReadableId"
                :name="textData.textData.author.name"
                creatureRole="Автор"
            />

            <!-- Translator -->
            <CreatureInfoComponent v-if="textData.textData.translator !== null"
                :id="textData.textData.translator.entityId"
                :furryReadableId="textData.textData.translator.furryReadableId"
                :name="textData.textData.translator.name"
                creatureRole="Переводчик"
            />
            <NonexistentCreatureComponent v-else creatureRole="Переводчик" />

        </div>

        <div class="read-text-title">{{ textData.textData.title }}</div>
    </div>
</template>
