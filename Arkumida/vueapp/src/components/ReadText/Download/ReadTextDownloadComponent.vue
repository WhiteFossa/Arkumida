<script setup>

import {defineProps, ref} from "vue";
    import {Guid} from "guid-typescript";

    const props = defineProps({
        plaintextFileId: Guid
    })

    const apiBaseUrl = process.env.VUE_APP_API_URL

    const plaintextDownloadUrl = ref(null)
    plaintextDownloadUrl.value = apiBaseUrl + "/api/Files/Download/" + props.plaintextFileId

</script>

<template>
    <div class="read-text-download">
        <div class="read-text-download-caption">Скачать</div>

        <ul class="read-text-download-list">
            <li><a class="read-text-download-file-link" :href="plaintextDownloadUrl" title="Скачать в формате TXT">TXT</a></li>
            <li>PDF</li>
        </ul>
    </div>
</template>