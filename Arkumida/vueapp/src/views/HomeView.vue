<template>
    <HeaderComponent />

    <MainPage />

    <FooterComponent />

</template>

<script>

import HeaderComponent from "@/components/HeaderComponent.vue";
import FooterComponent from "@/components/FooterComponent.vue";
import MainPage from "@/components/MainPage.vue";

export default {
  name: 'HomeView',
  components: {
      MainPage,
      FooterComponent,
      HeaderComponent
  }
}
</script>
