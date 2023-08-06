<script setup>
    import {defineEmits, defineProps, reactive } from "vue";
    import {maxValue, minValue, numeric, required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";

    const props = defineProps({
        currentPage: Number,
        pagesCount: Number
    })

    const formData = reactive({
        targetPage: props.currentPage
    })

    const rules = {
        targetPage: {
            $autoDirty: true,
            required,
            numeric,
            minValueValue: minValue(1),
            maxValueRef: maxValue(props.pagesCount)
        }
    }

    const validator = useVuelidate(rules, formData)

    const emit = defineEmits(['goToPage'])

    async function SubmitForm()
    {
        await validator.value.$validate();

        if (!validator.value.$error)
        {
            await GoToPage(Number(formData.targetPage))
        }
    }

    async function GoToPage(pageNumber)
    {
        if (pageNumber < 1 || pageNumber > props.pagesCount)
        {
            return;
        }

        emit('goToPage', pageNumber)
    }
</script>

<template>
    <form class="read-text-pagination-go-to-page-container" v-on:submit.prevent="SubmitForm">

        <input
            :class="(validator.targetPage.$error)?'read-text-pagination-go-to-page-input read-text-pagination-go-to-page-input-invalid':'read-text-pagination-go-to-page-input'"
            type="text"
            v-model="formData.targetPage" />
        <button
            class="read-text-pagination-go-to-page-button"
            :disabled="validator.targetPage.$error">
            Перейти
        </button>
    </form>
</template>