<template>
    <v-container>
      <v-row>
        <v-col cols="12" md="6">
          <h2>İletişim Bilgileri</h2>
          <p>Adres: ********</p>
          <p>Telefon: ********</p>
          <p>E-Posta: ********</p>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="6">
          <h2>Bize Ulaşın</h2>
          <v-form ref="form" v-model="valid" lazy-validation>
            <v-text-field
              v-model="ad"
              label="Adınız"
              :rules="[v => !!v || 'Ad gerekli']"
              required
            ></v-text-field>
            <v-text-field
              v-model="soyad"
              label="Soyadınız"
              :rules="[v => !!v || 'Soyad gerekli']"
              required
            ></v-text-field>
            <v-text-field
              v-model="telefon"
              label="Telefon Numaranız"
              :rules="[v => !!v || 'Telefon gerekli']"
              required
            ></v-text-field>
            <v-text-field
              v-model="konu"
              label="Konu Hakkında Bilgi Veriniz"
              :rules="[v => !!v || 'Konu gerekli']"
              required
            ></v-text-field>
            <v-textarea
              v-model="mesaj"
              label="Mesajınız"
              :rules="[v => !!v || 'Mesaj gerekli']"
              required
            ></v-textarea>
            <v-checkbox
              v-model="kvkk"
              :rules="[v => !!v || 'KVKK metnini kabul etmelisiniz.']"
              label="KVKK Aydınlatma Metnini okudum, kabul ediyorum."
            ></v-checkbox>
            <v-btn :disabled="!valid" color="primary" @click="submitForm">
              Mesaj Gönder
            </v-btn>
          </v-form>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  
  const ad = ref('');
  const soyad = ref('');
  const telefon = ref('');
  const konu = ref('');
  const mesaj = ref('');
  const kvkk = ref(false);
  const valid = ref(false);
  
  const submitForm = () => {
    const formData = {
      GonderenAd: ad.value,
      GonderenSoyad: soyad.value,
      GonderenTelefon: telefon.value,
      Konu: konu.value,
      MesajMetni: mesaj.value,
      Kvkk: kvkk.value
    };
  
    axios
      .post('http://localhost:5225/api/Iletisim', formData)
      .then(response => {
        alert('Mesajınız başarıyla gönderildi.');
      })
      .catch(error => {
        console.error('Mesaj gönderimi sırasında hata oluştu:', error);
      });
  };
  </script>
  
  <style scoped>
  h2 {
    margin-bottom: 20px;
  }
  </style>
  