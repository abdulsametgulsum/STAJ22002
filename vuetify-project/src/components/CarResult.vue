<template>
  <div>
    <v-row class="mb-4 mr-4 filter-row">
      <v-col cols="12" md="3">
        <v-select v-model="filters.yakitTuru" :items="yakitTuruOptions" label="Yakıt Türü"></v-select>
      </v-col>
      <v-col cols="12" md="3">
        <v-select v-model="filters.vitesTuru" :items="vitesTuruOptions" label="Vites Türü"></v-select>
      </v-col>
      <v-col cols="12" md="3">
        <v-select v-model="filters.aracSinifi" :items="aracSinifiOptions" label="Sınıf"></v-select>
      </v-col>
    </v-row>
    <v-row>
      <v-col v-for="(car, index) in sortedCars" :key="car.AracId" cols="12" md="12">
        <v-card class="d-flex justify-space-between align-center py-4 px-4">
          <v-img :src="getImageUrl(car.FotoUrl)" height="200px" width="250px" contain></v-img>

          <div class="car-details px-4">
            <v-card-title>{{ car.Marka }} {{ car.Model }}</v-card-title>
            <v-card-subtitle>Motor Gücü: {{ car.MotorGucu }} hp</v-card-subtitle>
            <v-card-subtitle>Yakıt Türü: {{ car.YakitTuru }}</v-card-subtitle>
            <v-card-subtitle>Vites Türü: {{ car.VitesTuru }}</v-card-subtitle>
            <v-card-subtitle>Sınıf: {{ car.AracSinifi }}</v-card-subtitle>
            <v-card-subtitle>Kapasite: {{ car.YolcuKapasitesi }} kişi</v-card-subtitle>
          </div>

          <div class="price-details d-flex flex-column justify-end align-center text-right">
            <h3 class="text--primary mb-3">{{ car.GunlukFiyat }} TL/Günlük</h3>
            <div class="button-group d-flex">
              <v-btn color="primary" @click="toggleDetails(index)" class="mr-2">Tüm Özellikler</v-btn>
              <v-btn color="success" @click="goToReservationDetails(car.AracId)">Kirala</v-btn>
            </div>
          </div>
        </v-card>

        <v-expand-transition>
          <div v-if="carDetailsExpanded[index]" class="car-extra-details px-4 mt-3">
            <p>Yıl: {{ car.Yil }}</p>
            <p>Renk: {{ car.Renk }}</p>
            <p>Motor Gücü: {{ car.MotorGucu }} kw</p>
            <p>Yakıt Türü: {{ car.YakitTuru }}</p>
            <p>Yolcu Kapasitesi: {{ car.YolcuKapasitesi }}</p>
            <p>Günlük km: {{ car.GunlukKM }} km</p>
            <p>Araç Sınıfı: {{ car.AracSinifi }}</p>
            <p>Ehliyet Yaşı: {{ car.EhliyetYasi }} ve üzeri</p>
          </div>
        </v-expand-transition>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'
import { useRouter, useRoute } from 'vue-router'

const router = useRouter()
const route = useRoute()

const cars = ref([])
const carDetailsExpanded = ref([])

const getImageUrl = (fotoUrl) => {
  return new URL(`../assets/${fotoUrl}`, import.meta.url).href;
};
const toggleDetails = (index) => {
  carDetailsExpanded.value[index] = !carDetailsExpanded.value[index]
}
// Filtre seçenekleri
const filters = ref({
  yakitTuru: 'Hepsi',
  vitesTuru: 'Hepsi',
  aracSinifi: 'Hepsi'
})

const yakitTuruOptions = ref(['Hepsi', 'Benzin', 'Dizel', 'Elektrik'])
const vitesTuruOptions = ref(['Hepsi', 'Otomatik', 'Manuel'])
const aracSinifiOptions = ref(['Hepsi', 'Ekonomik', 'Lüks', 'Orta'])

const fetchCars = async () => {
  const pickupDate = route.query.pickupDate
  const dropoffDate = route.query.dropoffDate

  try {
    const response = await axios.get('http://localhost:5225/api/Arac/searchCars', {
      params: {
        pickupDate,
        dropoffDate
      }
    })
    cars.value = response.data
    carDetailsExpanded.value = Array(cars.value.length).fill(false)
  } catch (error) {
    console.error('Araç verileri çekilemedi:', error)
  }
}


const goToReservationDetails = (carId) => {
  router.push({
    name: 'ReservationDetails',
    params: { carId },
    query: {
      pickupDate: route.query.pickupDate,
      dropoffDate: route.query.dropoffDate
    }
  })
}


const sortedCars = computed(() => {
  return cars.value.filter(car => {
    const matchYakitTuru = filters.value.yakitTuru === 'Hepsi' || car.YakitTuru === filters.value.yakitTuru
    const matchVitesTuru = filters.value.vitesTuru === 'Hepsi' || car.VitesTuru === filters.value.vitesTuru
    const matchAracSinifi = filters.value.aracSinifi === 'Hepsi' || car.AracSinifi === filters.value.aracSinifi

    return matchYakitTuru && matchVitesTuru && matchAracSinifi
  })
})

onMounted(fetchCars)
</script>

<style scoped>
.price-details {
  text-align: right;
}

.button-group {
  display: flex;
  justify-content: flex-end;
  margin-top: 10px;
}

.v-btn {
  min-width: 100px;
  margin-left: 10px;
}

/* Araç genişletilmiş detaylarının görünümü */
.car-extra-details {
  background-color: #221f1f;
  padding: 10px;
  border-radius: 5px;
  border: 1px solid #ddd;
  margin-top: 10px;
}

/* Filtreleme kısmı için margin-top ekledim */
.filter-row {
  margin: 20px;
}
</style>
