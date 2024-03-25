<script setup>
import {onMounted, reactive, ref} from "vue";
    import {AuthRedirectToLoginPageIfNotLoggedIn} from "@/js/auth";
    import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
    import {required} from "@vuelidate/validators";
    import useVuelidate from "@vuelidate/core";
    import {WebClientSendGetRequest} from "@/js/libWebClient";
    import CreaturesInfoWithNonexistentCreaturesComponent
    from "@/components/Shared/Creatures/CreaturesInfoComponent.vue";

    const isLoading = ref(true)

    const newTextFormData = reactive({
        title: "",
        description: ""
    })

    const newTextRules = {
        title: {
            $autoDirty: true,
            required
        },
        description: {
            $autoDirty: true,
            required
        },
    }

    const newTextValidator = useVuelidate(newTextRules, newTextFormData)

    const currentCreature = ref(null)

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        await AuthRedirectToLoginPageIfNotLoggedIn()

        currentCreature.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature

        await newTextValidator.value.$validate()

        isLoading.value = false
    }

</script>

<template>

    <LoadingSymbol v-if="isLoading"/>

    <div
        v-if="!isLoading"
        class="body-container">

        <div class="add-new-text-title">
            Добавление нового произведения
        </div>

        <div class="add-new-text-form-container">

            <!-- Title and description -->
            <div class="add-new-text-title-and-description-outer-container">

                <div class="add-new-text-title-and-description-inner-container">

                    <div class="add-new-text-title-and-description-caption">
                        Название и описание
                    </div>

                    <!-- Title -->
                    <div class="add-new-text-title-hint-container">
                        <div>Название</div>
                    </div>

                    <div class="add-new-text-title-container">

                        <input
                            :class="(newTextValidator.title.$error)?'add-new-text-title-field-invalid':'add-new-text-title-field-valid'"
                            type="text"
                            v-model="newTextFormData.title"
                            placeholder="Введите название..."/>
                    </div>

                    <!-- Description -->
                    <div class="add-new-text-description-hint-container">
                        <div>Описание</div>
                    </div>

                    <div class="add-new-text-description-container">
                    <textarea
                        :class="(newTextValidator.description.$error)?'add-new-text-description-textarea-invalid':'add-new-text-description-textarea-valid'"
                        v-model="newTextFormData.description"
                        placeholder="Введите описание..."></textarea>
                    </div>

                </div>

            </div>

            <!-- Creatures (publisher, authors, translators) -->
            <div class="add-new-text-creatures-outer-container">

                <div class="add-new-text-creatures-inner-container">

                    <div class="add-new-text-creatures-caption">
                        Автор(ы) и переводчик(и)
                    </div>

                    <div class="add-new-text-creatures-subcontainer">

                        <!-- Publisher -->
                        <CreaturesInfoWithNonexistentCreaturesComponent
                            :creatures="[ { id: currentCreature.entityId} ]"
                            creaturesRole="Загрузил" />

                        <!-- Author(s) -->
                        <CreaturesInfoWithNonexistentCreaturesComponent
                            :creatures="[ { id: null, name: 'Юффер'}, { id: null, name: 'Йиффер'}, { id: null, name: 'Йерфер'} ]"
                            creaturesRole="Автор(ы)" />

                        <!-- Translators -->
                        <CreaturesInfoWithNonexistentCreaturesComponent
                            :creatures="[ { id: null, name: 'Яппер'}, { id: null, name: 'Гурглер'} ]"
                            creaturesRole="Переводчик(и)"/>

                    </div>

                </div>

            </div>

        </div>

    </div>

</template>
