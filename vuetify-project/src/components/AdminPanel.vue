<template>
    <v-container>
      <v-row>
        <v-col>
          <v-card>
            <v-card-title>
              <span class="headline">Admin Paneli</span>
            </v-card-title>
            <v-card-subtitle>
              <v-btn @click="navigateTo('arac-listele')" color="primary">Araçları Listele</v-btn>
              <v-btn @click="navigateTo('arac-ekle')" color="primary">Araç Ekle</v-btn>
              <v-btn @click="navigateTo('kullanici-listele')" color="primary">Kullanıcıları Listele</v-btn>
              <v-btn @click="navigateTo('rezervasyon-listele')" color="primary">Rezervasyonları Listele</v-btn>
              
            </v-card-subtitle>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script setup>
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  
  const navigateTo = (route) => {
    router.push(`${route}`);
  };
  </script>
  
  <style scoped>
  .headline {
    font-weight: bold;
  }
  </style>
  