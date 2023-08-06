<template>
    <HeaderComponent />

    <MainPage />

    <FooterComponent />

</template>

<script>

import HeaderComponent from "@/components/Header/HeaderComponent.vue";
import FooterComponent from "@/components/Footer/FooterComponent.vue";
import MainPage from "@/components/MainPage/MainPage.vue";

export default {
  name: 'HomeView',
  components: {
      MainPage,
      FooterComponent,
      HeaderComponent
  }
}
</script>
