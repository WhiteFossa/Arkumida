<!-- Site header -->
<script setup>
    import HeaderSiteInfo from './HeaderSiteInfo.vue'
    import CentralMenu from "@/components/CentralMenu.vue";
</script>

<template>
    <header class="header">
        <div class="horizontal-flex flex-space-between">
            <div>
                <HeaderSiteInfo />
            </div>
            <div>
                <CentralMenu />
            </div>
            <div>
                Right menu will be here
            </div>
        </div>
    </header>
</template>