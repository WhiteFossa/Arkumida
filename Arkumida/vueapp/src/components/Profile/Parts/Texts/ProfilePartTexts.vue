<script setup>
import {onMounted, reactive, ref} from "vue";
import {WebClientSendGetRequest, WebClientSendPostRequest} from "@/js/libWebClient";
import LoadingSymbol from "@/components/Shared/LoadingSymbol.vue";
import {Messages} from "@/js/constants";

    const isLoading = ref(true)

    const creatureId = ref(null)

    const isCriticsEditing = ref(false)

    const criticsSettings = ref(null)

    const criticsSettingsFormData = reactive({
        isShowDislikes: false,
        isShowDislikesAuthors: false
    })

    onMounted(async () =>
    {
        await OnLoad();
    })

    async function OnLoad()
    {
        creatureId.value = (await (await WebClientSendGetRequest("/api/Users/Current")).json()).creature.entityId

        criticsSettings.value = (await (await WebClientSendGetRequest("/api/Users/" + creatureId.value + "/Critics/GetSettings")).json()).criticsSettings

        isLoading.value = false
    }

    async function StartToEditCritics()
    {
        criticsSettingsFormData.isShowDislikes = criticsSettings.value.isShowDislikes
        criticsSettingsFormData.isShowDislikesAuthors = criticsSettings.value.isShowDislikesAuthors

        isCriticsEditing.value = true
    }

    async function CancelCriticsEditing()
    {
        isCriticsEditing.value = false
    }

    async function IsShowDislikesChanged()
    {
        if (!criticsSettingsFormData.isShowDislikes)
        {
            criticsSettingsFormData.isShowDislikesAuthors = false
        }
    }

async function CompleteCriticsEditing()
{
    const newCriticsSettingsResponse = await WebClientSendPostRequest(
        "/api/Users/" + creatureId.value + "/Critics/SaveSettings",
        {
            "newCriticsSettings":
            {
                "isShowDislikes": criticsSettingsFormData.isShowDislikes,
                "isShowDislikesAuthors": criticsSettingsFormData.isShowDislikesAuthors
            }
        }
    )

    if (newCriticsSettingsResponse.status !== 200)
    {
        alert(Messages.CriticsSettingsFailedToUpdate)
        isCriticsEditing.value = false
        return
    }

    const newCriticsSettings = (await newCriticsSettingsResponse.json()).updatedCriticsSettings

    criticsSettings.value.isShowDislikes = newCriticsSettings.isShowDislikes
    criticsSettings.value.isShowDislikesAuthors = newCriticsSettings.isShowDislikesAuthors

    isCriticsEditing.value = false
}
</script>

<template>
    <LoadingSymbol v-if="isLoading" />
    <div v-else>

        <!-- Critics settings -->
        <div class="profile-texts-part-critics-outer-container">
            <div class="profile-texts-part-critics-inner-container">

                <div class="profile-texts-part-critics-caption">
                    <button
                        v-if="!isCriticsEditing"
                        class="button-with-image"
                        type="button"
                        title="Редактировать настройки критики"
                        @click="async () => await StartToEditCritics()">
                        <img class="small-icon" src="/images/icons/icon_edit.webp" alt="Редактировать настройки критики" />
                    </button>

                    Критика
                </div>

                <div v-if="!isCriticsEditing">
                    <div>Отображать дизлайки: <strong><span v-if="criticsSettings.isShowDislikes">Да</span><span v-else>Нет</span></strong></div>
                    <div>Отображать пользователей, оставивших дизлайки: <strong><span v-if="criticsSettings.isShowDislikesAuthors">Да</span><span v-else>Нет</span></strong></div>
                </div>

                <div v-if="isCriticsEditing">
                    <div>
                        Отображать дизлайки:
                        <input
                            type="checkbox"
                            v-model="criticsSettingsFormData.isShowDislikes"
                            @change="async () => await IsShowDislikesChanged()" />
                    </div>

                    <div>
                        Отображать пользователей, оставивших дизлайки:
                        <input
                            type="checkbox"
                            :disabled="!criticsSettingsFormData.isShowDislikes"
                            v-model="criticsSettingsFormData.isShowDislikesAuthors" />
                    </div>
                </div>

            </div>

            <!-- Critics editing buttons -->
            <div
                v-if="isCriticsEditing"
                class="profile-texts-part-critics-edit-buttons-outer-container">
                <div class="profile-texts-part-critics-edit-buttons-bordered-inner-container">

                    <button
                        class="button-with-image"
                        type="button"
                        title="Сохранить изменения"
                        @click="async () => await CompleteCriticsEditing()">
                        <img class="small-icon" src="/images/icons/icon_ok.webp" alt="Сохранить изменения" />
                    </button>

                    <button
                        class="button-with-image"
                        type="button"
                        title="Отменить изменения"
                        @click="async () => await CancelCriticsEditing()">
                        <img class="small-icon" src="/images/icons/icon_cancel.webp" alt="Отменить изменения" />
                    </button>

                </div>
            </div>
        </div>

    </div>
</template>