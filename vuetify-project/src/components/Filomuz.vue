<template>
  <v-container>
    

    <h1>Filomuz</h1>
    <v-row>
      <v-col
        v-for="arac in araclar"
        :key="arac.AracId"
        cols="12"
        md="4"
      >
        <v-card>
          <v-img
  :src="getImageUrl(arac.FotoUrl)"
  height="200px"
  contain
></v-img>

          <v-card-title>{{ arac.Marka }} - {{ arac.Model }}</v-card-title>
          <v-card-subtitle>Yıl: {{ arac.Yil }}</v-card-subtitle>
          <v-card-actions>
            <v-btn @click="goToAracDetay(arac.AracId)" color="primary">
              Detayları Gör
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <v-alert v-if="loading" type="info">Yükleniyor...</v-alert>
    <p v-if="!loading && araclar.length === 0">Veri bulunamadı</p>
  </v-container>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'




const araclar = ref([])
const loading = ref(true)
const router = useRouter()


const getImageUrl = (fotoUrl) => {
  return new URL(`../assets/${fotoUrl}`, import.meta.url).href;
};




const fetchAraclar = async () => {
  try {
    console.log('Veri çekme işlemi başlatıldı...')
    const response = await axios.get('http://localhost:5225/api/Arac/GetAllAraclar')
    console.log('Veri alındı:', response.data)
    araclar.value = response.data
  } catch (error) {
    console.error('Araçlar getirilemedi:', error)
  } finally {
    loading.value = false
  }
}

const goToAracDetay = (id) => {
  router.push(`/arac-detay/${id}`)
}

onMounted(() => {
  fetchAraclar()
})
</script>

<style scoped>
/* Basit stil ekleyelim */
h1 {
  text-align: center;
  margin-bottom: 20px;
}
</style>
