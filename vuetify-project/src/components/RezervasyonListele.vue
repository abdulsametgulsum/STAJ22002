<template>
    <v-container>
        <v-btn @click="goBack" color="secondary">Geri</v-btn>
      <v-row>
        
        <v-col>
            
          <v-card>
            
            <v-card-title>
              <span class="headline">Kiralamaları Listele</span>
            </v-card-title>
            <v-data-table :headers="headers" :items="kiralamalar" item-key="KiralamaId">
              <template v-slot:top>
                <v-toolbar flat>
                  <v-toolbar-title>Kiralama Listesi</v-toolbar-title>
                </v-toolbar>
              </template>
              
            </v-data-table>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  const headers = [
    { text: 'ID', value: 'KiralamaId' },
    { text: 'Kullanıcı ID', value: 'KullaniciId' },
    { text: 'Araç ID', value: 'AracId' },
    { text: 'Başlangıç Tarihi', value: 'KiralamaTarihi' },
    { text: 'Bitiş Tarihi', value: 'DonusTarihi' },
    { text: 'Toplam Fiyat', value: 'ToplamFiyat' },
    { text: 'Aksiyon', value: 'action', sortable: false }
  ];
  
  const kiralamalar = ref([]);
  const goBack = () => {
    router.push('/admin');
  };
  const fetchKiralamalar = async () => {
    try {
      const response = await axios.get('http://localhost:5225/api/Reservation/GetAllKiralamalar');
      kiralamalar.value = response.data;
    } catch (error) {
      console.error('Kiralamaları alırken bir hata oluştu:', error);
    }
  };
  
 
  
  onMounted(fetchKiralamalar);
  </script>
  
  <style scoped>
  .headline {
    font-weight: bold;
  }
  </style>
  