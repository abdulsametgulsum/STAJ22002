<template>
  <div>
    <v-form @submit.prevent="login">
      <v-text-field
        v-model="model.eposta"
        label="E-posta"
        required
      ></v-text-field>

      <v-text-field
        v-model="model.sifre"
        label="Şifre"
        type="password"
        required
      ></v-text-field>

      <v-btn type="submit" color="success">Giriş Yap</v-btn>
    </v-form>

    <v-row class="mt-4" justify="center">
      <router-link to="/register">
        <v-btn>Üye Olun</v-btn>
      </router-link>
      <router-link to="/forgot-password">
        <v-btn>Şifremi Unuttum</v-btn>
      </router-link>
    </v-row>

    <v-alert v-if="error" type="error">{{ error }}</v-alert>
  </div>
</template>

<script setup>
import { ref, registerRuntimeCompiler } from 'vue'
import { useRouter } from 'vue-router'

import userStore from "@/store/userStore"

const router = useRouter()
const showLoginForm = ref(true)
const model = ref({
  eposta: '',
  sifre: ''
})
const error = ref('')

const emit = defineEmits(['close'])

const login = async () => {
  try {
    await userStore.dispatch("login", model.value);
   
    if (userStore.getters.isAuthenticated){
      if (userStore.getters.isAdmin) {
     
     router.push('/admin')
   } else {
     // Normal kullanıcı giriş yaptıysa, kullanıcıyı ana sayfaya yönlendir
     alert('Giriş başarılı!')
     emit("close") // Formu kapatma fonksiyonu
   }  
  }
  } catch (err) {
    console.error(err)
    error.value = 'Bir hata oluştu!'
  }
}
</script>
