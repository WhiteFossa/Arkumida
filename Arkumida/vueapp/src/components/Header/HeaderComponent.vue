<!-- Site header -->
<script setup>
    import HeaderSiteInfo from './HeaderSiteInfo.vue'
    import CentralMenu from "@/components/Header/CentralMenu.vue";
    import RightMenu from "@/components/Header/RightMenu.vue";
</script>

<template>
    <header class="header">
        <div class="horizontal-flex flex-cross-center header-container">
            <div>
                <HeaderSiteInfo />
            </div>
            <div>
                <CentralMenu />
            </div>
            <div>
                <RightMenu />
            </div>
        </div>
    </header>
</template>