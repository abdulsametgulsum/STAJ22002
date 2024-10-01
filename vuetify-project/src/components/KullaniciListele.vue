<template>
    <v-container>
        <v-btn @click="goBack" color="secondary">Geri</v-btn>
      <v-row>
       
        <v-col>
            
          <v-card>
            <v-card-title>
              <span class="headline">Kullanıcıları Listele</span>
            </v-card-title>
            <v-data-table :headers="headers" :items="kullanicilar" item-key="KullaniciId">
              <template v-slot:top>
                <v-toolbar flat>
                  <v-toolbar-title>Kullanıcı Listesi</v-toolbar-title>
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
    { text: 'ID', value: 'KullaniciId' },
    { text: 'Ad', value: 'Ad' },
    { text: 'Soyad', value: 'Soyad' },
    { text: 'E-posta', value: 'Eposta' },
    { text: 'TC Kimlik No', value: 'TcKimlikNo' },
    { text: 'Doğum Tarihi', value: 'DogumTarihi' },
    { text: 'Aksiyon', value: 'action', sortable: false }
  ];
  
  const kullanicilar = ref([]);
  
  const fetchKullanicilar = async () => {
    try {
      const response = await axios.get('http://localhost:5225/api/Kullanıcı/GetKullanicilar');
      kullanicilar.value = response.data;
    } catch (error) {
      console.error('Kullanıcıları alırken bir hata oluştu:', error);
    }
  };
  

  
 
  const goBack = () => {
    router.push('/admin');
  };
  onMounted(fetchKullanicilar);
  </script>
  
  <style scoped>
  .headline {
    font-weight: bold;
  }
  </style>
  