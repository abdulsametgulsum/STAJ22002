<template>
    <v-container>
        <v-btn @click="goBack" color="secondary">Geri</v-btn>
      <h1 style="text-align: center;">Araç Listesi</h1>
  
      <!-- Araçlar Tablosu -->
      <v-data-table
        :headers="headers"
        :items="araclar"
        item-key="AracId"
        class="elevation-1"
      >
        <template v-slot:item.Aksiyon="{ item }">
          <v-btn @click="openUpdateModal(item)" color="primary" class="mr-2">Güncelle</v-btn>
          <v-btn @click="deleteArac(item.AracId)" color="error">Sil</v-btn>
        </template>
      </v-data-table>
  
      <!-- Araç Güncelleme Modal -->
      <v-dialog v-model="updateDialog" max-width="600px">
        <v-card>
          <v-card-title>
            <span class="headline">Araç Güncelle</span>
          </v-card-title>
          <v-card-subtitle>
            <v-form ref="form" v-model="formIsValid">
              <v-text-field
                v-model="selectedArac.Marka"
                label="Marka"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.Model"
                label="Model"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.Renk"
                label="Renk"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.Yil"
                label="Yıl"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.GunlukFiyat"
                label="Fiyat"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.MotorGucu"
                label="Motor Gücü"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.YakitTuru"
                label="Yakıt Türü"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.YolcuKapasitesi"
                label="Yolcu Kapasitesi"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.GunlukKM"
                label="Günlük KM"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.AracSinifi"
                label="Araç Sınıfı"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.EhliyetYasi"
                label="Ehliyet Yaşı"
                required
              ></v-text-field>
              <v-text-field
                v-model="selectedArac.FotoUrl"
                label="Foto Url"
                required
              ></v-text-field>
            </v-form>
          </v-card-subtitle>
          <v-card-actions>
            <v-btn color="blue darken-1" text @click="updateDialog = false">Kapat</v-btn>
            <v-btn color="blue darken-1" text @click="updateArac" :disabled="!formIsValid">Güncelle</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
  
   
  </v-container>


  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter } from 'vue-router';
  
  const araclar = ref([]);
  const headers = [
    { text: 'Marka', value: 'Marka' },
    { text: 'Model', value: 'Model' },
    { text: 'Renk', value: 'Renk' },
    { text: 'Yıl', value: 'Yil' },
    { text: 'Fiyat', value: 'GunlukFiyat' },
    { text: 'Motor Gücü', value: 'MotorGucu' },
    { text: 'Yakıt Türü', value: 'YakitTuru' },
    { text: 'Yolcu Kapasitesi', value: 'YolcuKapasitesi' },
    { text: 'Günlük KM', value: 'GunlukKM' },
    { text: 'Araç Sınıfı', value: 'AracSinifi' },
    { text: 'Ehliyet Yaşı', value: 'EhliyetYasi' },
    
    { text: 'Aksiyon', value: 'Aksiyon' }
  ];
  const loading = ref(true);
  const error = ref(null);
  const updateDialog = ref(false);
  const selectedArac = ref({});
  const formIsValid = ref(false);
  const router = useRouter();
  const goBack = () => {
    router.push('/admin');
  };
  const fetchAraclar = async () => {
    try {
      const response = await axios.get('http://localhost:5225/api/Arac/GetAllAraclar');
      araclar.value = response.data;
    } catch (err) {
      error.value = 'Araçları alırken bir hata oluştu: ' + err.message;
    } finally {
      loading.value = false;
    }
  };
  
  onMounted(() => {
    fetchAraclar();
  });
  
  const openUpdateModal = (arac) => {
    selectedArac.value = { ...arac };
    updateDialog.value = true;
  };
  
  const updateArac = async () => {
    try {
      await axios.put(`http://localhost:5225/api/Arac/UpdateArac/${selectedArac.value.AracId}`, selectedArac.value);
      fetchAraclar();
      updateDialog.value = false;
    } catch (err) {
      error.value = 'Araç güncellenirken bir hata oluştu: ' + err.message;
    }
  };
  
  const deleteArac = async (id) => {
    try {
      await axios.delete(`http://localhost:5225/api/Arac/${id}`);
      fetchAraclar();
    } catch (err) {
      error.value = 'Araç silinirken bir hata oluştu: ' + err.message;
    }
  };
  </script>
  
  <style>
  .elevation-1 {
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }
  
  h1 {
    margin-bottom: 20px;
  }
  
  .v-btn {
    margin-right: 8px;
  }
  </style>
  