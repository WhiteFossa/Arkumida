<!-- Shows texts statistics -->
<script setup>
    import moment from 'moment'
    import LoadingSymbol from '../Shared/LoadingSymbol.vue'

    import { ref, onMounted } from 'vue'

    // API base URL
    const apiBaseUrl = process.env.VUE_APP_API_URL
    
    // True if loading under way
    const isLoading = ref(true)
    
    // Version string
    const totalTexts = ref(0)

    // Sources URL
    const readToday = ref(0)
    
    // Last add time
    const lastAdd = ref(null)
    
    // OnMounted hook
    onMounted(async () =>
    {
        await OnLoad();
    })
    
    // Called when page is loaded
    async function OnLoad()
    {
        const textsStatistics = await (await fetch(apiBaseUrl + `/api/Statistics/Texts`)).json()

        totalTexts.value = textsStatistics.totalTexts
        readToday.value = textsStatistics.readToday
        lastAdd.value = moment(textsStatistics.lastAddTime).format('HH:mm DD.MM.YYYY')
        
        isLoading.value = false
    }

</script>

<template>
    <div v-if="isLoading">
        <LoadingSymbol />
    </div>
    <div v-else>
        <!-- Shown after load -->
        <div class="texts-statistics">
            Сегодня на сайте <strong class="texts-statistics-total-texts">{{ totalTexts }}</strong> текстов, прочитано {{ readToday }}. Последнее добавление <strong>{{ lastAdd }}</strong>
        </div>
    </div>
</template>