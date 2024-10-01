<template>
  <div>
    <v-window show-arrows>
      <template v-slot:prev="{ props }">
        <v-btn color="success" @click="props.onClick">Previous slide</v-btn>
      </template>
      <template v-slot:next="{ props }">
        <v-btn color="info" @click="props.onClick">Next slide</v-btn>
      </template>

      <!-- Resimleri döngü ile ekliyoruz -->
      <v-window-item v-for="(image, index) in images" :key="`card-${index}`">
        <v-img
          :src="image"
          class="d-flex align-center justify-center"
          height="1000px"
          width="100%"
          contain
        ></v-img>
      </v-window-item>
    </v-window>

    <form @submit.prevent="searchCars">
      <v-select
        v-model="pickupOffice"
        :items="offices"
        label="Teslim Alma Ofisi"
      ></v-select>

      <v-select
        v-model="dropoffOffice"
        :items="offices"
        label="Teslim Etme Ofisi"
      ></v-select>

      <v-text-field
        v-model="pickupDate"
        label="Teslim Alma Tarihi"
        type="date"
      ></v-text-field>

      <v-text-field
        v-model="dropoffDate"
        label="Teslim Etme Tarihi"
        type="date"
      ></v-text-field>

      <v-btn class="me-4" @click="searchCars">Araç Ara</v-btn>
      <v-btn @click="handleReset">Temizle</v-btn>
    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios';
import { useRouter } from 'vue-router';

const router = useRouter();

// Resim yollarını import.meta kullanarak tanımlıyoruz
const images = ref([
  new URL('@/assets/anasayfa1.jpg', import.meta.url).href,
  new URL('@/assets/anasayfa2.jpg', import.meta.url).href,
  new URL('@/assets/anasayfa3.jpg', import.meta.url).href
]);

const pickupOffice = ref('');
const dropoffOffice = ref('');
const pickupDate = ref('');
const dropoffDate = ref('');

const offices = ref(['Ofis 1', 'Ofis 2', 'Ofis 3', 'Ofis 4']);

const searchCars = async () => {
  if (!pickupDate.value || !dropoffDate.value) {
    console.error('Tarih alanları eksik veya boş.');
    return;
  }

  try {
    const response = await axios.get('http://localhost:5225/api/Arac/searchCars', {
      params: {
        pickupDate: pickupDate.value,
        dropoffDate: dropoffDate.value
      }
    });
    console.log('Araçlar başarıyla getirildi:', response.data);

    // Yönlendirmeyi verileri aldıktan sonra yap
    router.push({
      name: 'CarResults', // Yönlendirilecek sayfa ismi
      query: {
        pickupDate: pickupDate.value,
        dropoffDate: dropoffDate.value
      }
    });
  } catch (error) {
    console.error('Araçlar getirilemedi:', error);
  }
};

const handleReset = () => {
  pickupOffice.value = '';
  dropoffOffice.value = '';
  pickupDate.value = '';
  dropoffDate.value = '';
};
</script>

<style scoped>
/* Basit stil eklemeleri */
</style>
