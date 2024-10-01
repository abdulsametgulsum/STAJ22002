<template>
  <v-footer class="bg-grey-lighten-1">
    <v-row justify="center" no-gutters>
      <!-- Menü bağlantıları -->
      <v-btn
        v-for="link in links"
        :key="link"
        class="mx-2"
        color="white"
        rounded="xl"
        variant="text"
        :to="getLinkDestination(link)"
        @click="link === 'Giriş Yap' && openLoginDialog()"
        v-if="link !== 'Giriş Yap' || !isAuthenticated"
      >
        {{ link }}
      </v-btn>

      <!-- Kullanıcı bilgileri ve çıkış yap butonu -->
      <div v-if="isAuthenticated">
        <p>Kullanıcı: {{ user.Ad }} {{ user.Soyad }}</p>
        <v-btn @click="logout">Çıkış Yap</v-btn>
      </div>
    </v-row>

    <!-- Giriş Yap Formu Modal -->
    <v-dialog v-model="loginDialog" max-width="400" class="login-dialog">
      <v-card>
        <v-card-title class="headline">Giriş Yap</v-card-title>
        <v-card-text>
          <LoginForm @close="loginDialog = false" />
        </v-card-text>
      </v-card>
    </v-dialog>
  </v-footer>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import LoginForm from './LoginForm.vue'
import userStore from "@/store/userStore"

const router = useRouter()
const isAuthenticated = computed(() => userStore.getters.isAuthenticated)
const user = computed(() => userStore.getters.getUser)

const logout = () => {
  store.dispatch('logout')
  router.push('/') // Çıkış yaptıktan sonra ana sayfaya yönlendir
}

const links = [
  'Anasayfa',
  'Filomuz',
  'Bize Ulaşın',
  'Hakkımızda',
  'Giriş Yap',
]

const loginDialog = ref(false)

const openLoginDialog = () => {
  loginDialog.value = true
}

const closeLoginDialog = () => {
  loginDialog.value = false
}

const getLinkDestination = (link) => {
  switch (link) {
    case 'Anasayfa':
      return '/'
    case 'Filomuz':
      return '/filomuz'
    case 'Bize Ulaşın':
      return '/bize-ulasin'
    case 'Hakkımızda':
      return '/hakkimizda'
    default:
      return '#'
  }
}
</script>

<style>
/* Stil ayarları */
</style>
