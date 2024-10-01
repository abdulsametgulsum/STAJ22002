<template>
  <v-container>
    
      <v-form @submit.prevent="register" v-model="valid">
        <v-text-field
          v-model="firstName"
          label="Ad"
          :rules="[rules.required]"
          required
        ></v-text-field>
  
        <v-text-field
          v-model="lastName"
          label="Soyad"
          :rules="[rules.required]"
          required
        ></v-text-field>
  
        <v-text-field
          v-model="tcKimlikNo"
          label="TC Kimlik No"
          :rules="[rules.required, rules.tcLength]"
          required
          maxlength="11"
        ></v-text-field>
  
        <v-text-field
          v-model="phone"
          label="Telefon"
          :rules="[rules.required, rules.phone]"
          required
        ></v-text-field>
  
        <v-text-field
          v-model="birthDate"
          label="Doğum Tarihi"
          :rules="[rules.required]"
          required
          type="date"
        ></v-text-field>
  
        <v-text-field
          v-model="email"
          label="E-posta"
          :rules="[rules.required, rules.email]"
          required
        ></v-text-field>
  
        <v-text-field
          v-model="confirmEmail"
          label="E-posta Onayla"
          :rules="[rules.required]"
          required
        ></v-text-field>
  
        <v-text-field
          v-model="password"
          label="Şifre"
          :rules="[rules.required, rules.minLength(6)]"
          required
          type="password"
        ></v-text-field>
  
        <v-text-field
          v-model="confirmPassword"
          label="Şifre Onayla"
          :rules="[rules.required]"
          required
          type="password"
        ></v-text-field>
  
        <v-checkbox
          v-model="termsAccepted"
          label="Kullanım şartlarını kabul ediyorum"
          :rules="[rules.required]"
          required
        ></v-checkbox>
  
        <v-checkbox
          v-model="newsletter"
          label="Bültene abone olmak istiyorum"
        ></v-checkbox>
  
        <v-checkbox
          v-model="agreePrivacyPolicy"
          label="Gizlilik politikasını kabul ediyorum"
          :rules="[rules.required]"
          required
        ></v-checkbox>
  
       
      <v-btn
        :disabled="!valid"
        color="success"
        type="submit"
      >
        Üye Ol
      </v-btn>

      <v-alert v-if="error" type="error">{{ error }}</v-alert>
      <v-alert v-if="success" type="success">{{ success }}</v-alert>
    </v-form>
  </v-container>
</template>

<script setup>
import { ref, defineEmits } from 'vue'
import axios from 'axios'

const emit = defineEmits(['close'])

const firstName = ref('')
const lastName = ref('')
const tcKimlikNo = ref('')
const phone = ref('')
const birthDate = ref('')
const email = ref('')
const confirmEmail = ref('')
const password = ref('')
const confirmPassword = ref('')
const termsAccepted = ref(false)
const newsletter = ref(false)
const agreePrivacyPolicy = ref(false)
const error = ref('')
const success = ref('')
const valid = ref(false)

const rules = {
  required: v => !!v || 'Bu alan gereklidir.',
  email: v => /.+@.+\..+/.test(v) || 'Geçerli bir e-posta adresi girin.',
  minLength: length => v => v.length >= length || `${length} karakter veya daha fazla olmalıdır.`,
  tcLength: v => v.length === 11 || 'TC kimlik numarası 11 haneli olmalıdır.',
  phone: v => /^\d{10,15}$/.test(v) || 'Geçerli bir telefon numarası girin (10-15 haneli).',
}

const register = async () => {
  try {
    const payload = {
      Ad: firstName.value,
      Soyad: lastName.value,
      TcKimlikNo: tcKimlikNo.value,
      Telefon: phone.value,
      DogumTarihi: birthDate.value,
      Eposta: email.value,
      Sifre: password.value,
    }

    console.log('Gönderilen veri:', payload)

    const response = await axios.post('http://localhost:5225/api/Kullanıcı/AddKullanıcı', payload)

    console.log('API yanıtı:', response.data)

    if (response.data.success) {
      success.value = 'Kayıt başarılı!'
      error.value = ''
      emit('close') // Dialog'u kapat
    } else {
      error.value = `Kayıt sırasında bir hata oluştu: ${response.data.message || 'Bilinmeyen hata'}`
      success.value = ''
    }
  } catch (err) {
    console.error('API isteğinde hata:', err.response ? err.response.data : err.message)
    error.value = `Bir hata oluştu: ${err.response ? err.response.data.message : err.message}`
    success.value = ''
  }
}
</script>
