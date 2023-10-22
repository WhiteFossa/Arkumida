<script setup>

    import HeaderComponent from "@/components/Header/HeaderComponent.vue";
    import SearchComponent from "@/components/Search/SearchComponent.vue";
    import {defineProps, onMounted, ref} from "vue";
    import {OnPageLoad} from "@/js/libArkumida";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";

    const props = defineProps({
        encodedQuery: String
    })

    const isLoading = ref(true)

    const decodedQuery = ref("")

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await OnPageLoad()

        if (props.encodedQuery !== "")
        {
            decodedQuery.value = decodeURIComponent(props.encodedQuery)
        }

        isLoading.value = false
    }
</script>

<template>
    <HeaderComponent />

    <LoadingSymbol v-if="isLoading" />

    <div
        v-if="!isLoading"
        class="body-container">

        <SearchComponent isCalledFromMainPage="false" :queryText="decodedQuery" />
    </div>

</template>
