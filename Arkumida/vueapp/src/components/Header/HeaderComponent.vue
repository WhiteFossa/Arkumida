<!-- Site header -->
<script setup>
import {defineExpose, ref} from "vue";
    import HeaderSiteInfo from './HeaderSiteInfo.vue'
    import CentralMenu from "@/components/Header/CentralMenu.vue";
    import HeaderUserInfo from "@/components/Header/HeaderUserInfo.vue";

    defineExpose({
        ReloadProfile,
        OnPrivateMessageMarkedAsRead
    })

    const headerUserInfo = ref(null)

    // Call it to reload profile
    async function ReloadProfile()
    {
        headerUserInfo.value.ReloadProfile()
    }

    // Call it when private message marked as read
    async function OnPrivateMessageMarkedAsRead(messageId)
    {
        headerUserInfo.value.OnPrivateMessageMarkedAsRead(messageId)
    }
</script>

<template>
    <header class="header">
        <div class="header-container">
            <div>
                <HeaderSiteInfo />
            </div>
            <div>
                <CentralMenu />
            </div>
            <div>
                <HeaderUserInfo ref="headerUserInfo" />
            </div>
        </div>
    </header>
</template>