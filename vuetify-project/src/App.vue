<template>
  <div>
    <NavbarComponent v-if="!hideNavbar" />
    <router-view></router-view>
    <FooterComponent v-if="!hideNavbar" />
  </div>
</template>

<script>
import FooterComponent from './components/FooterComponent.vue';
import NavbarComponent from './components/NavbarComponent.vue';
import { computed } from 'vue'

const hideNavbar = computed(() => route.meta.hideNavbar)

export default {
  components: {
    FooterComponent,
    NavbarComponent,
  },
};
</script>

<style scoped>
/* Stil eklemeleri */
</style>
