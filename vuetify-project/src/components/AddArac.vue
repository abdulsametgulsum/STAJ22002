<template>
    <v-container>
      <v-card>
        <v-btn @click="goBack" color="secondary">Geri</v-btn>
        <v-card-title>
          <span class="headline">Araç Ekle</span>
        </v-card-title>
        <v-card-subtitle>
          <v-form @submit.prevent="submitForm">
            <v-text-field v-model="arac.Marka" label="Marka" required></v-text-field>
            <v-text-field v-model="arac.Model" label="Model" required></v-text-field>
            <v-text-field v-model="arac.Yil" label="Yıl" type="number" required></v-text-field>
            <v-text-field v-model="arac.Renk" label="Renk" required></v-text-field>
            <v-text-field v-model="arac.GunlukFiyat" label="Günlük Fiyat" type="number" step="0.01" required></v-text-field>
            <v-text-field v-model="arac.Durum" label="Durum" required></v-text-field>
            <v-text-field v-model="arac.MotorGucu" label="Motor Gücü" type="number" required></v-text-field>
            <v-text-field v-model="arac.YakitTuru" label="Yakıt Türü" required></v-text-field>
            <v-text-field v-model="arac.YolcuKapasitesi" label="Yolcu Kapasitesi" type="number" required></v-text-field>
            <v-text-field v-model="arac.GunlukKM" label="Günlük KM" type="number" required></v-text-field>
            <v-text-field v-model="arac.AracSinifi" label="Araç Sınıfı" required></v-text-field>
            <v-text-field v-model="arac.EhliyetYasi" label="Ehliyet Yaşı" type="number" required></v-text-field>
            <v-text-field v-model="arac.VitesTuru" label="Vites Türü" required></v-text-field>
            <v-text-field v-model="arac.FotoUrl" label="Fotoğraf URL" required></v-text-field>
            <v-btn type="submit" color="primary">Ekle</v-btn>
            
          </v-form>
        </v-card-subtitle>
      </v-card>
    </v-container>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';
  
  const arac = ref({
    Marka: '',
    Model: '',
    Yil: '',
    Renk: '',
    GunlukFiyat: '',
    Durum: '',
    MotorGucu: '',
    YakitTuru: '',
    YolcuKapasitesi: '',
    GunlukKM: '',
    AracSinifi: '',
    EhliyetYasi: '',
    VitesTuru: '',
    FotoUrl: ''
  });
  
  const router = useRouter();
  
  const submitForm = async () => {
    try {
      await axios.post('http://localhost:5225/api/Arac/AddArac', arac.value);
      alert('Araç başarıyla eklendi.');
      router.push('/admin-arac-listele');
    } catch (error) {
      console.error('Araç eklenirken bir hata oluştu:', error);
    }
  };
  
  const goBack = () => {
    router.push('/admin');
  };
  </script>
  