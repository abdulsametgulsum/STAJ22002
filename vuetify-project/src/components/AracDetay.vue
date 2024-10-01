<template>
  <v-container>
    <v-card v-if="arac">
      <v-card-title style="text-align: center;">{{ arac.Marka }} - {{ arac.Model }}</v-card-title>
      <v-card-subtitle></v-card-subtitle>
      <v-card-text>
        <v-img
            :src="getImageUrl(arac.FotoUrl)"
            class="d-flex align-center justify-center"
            height="400px"
            width="100%"
            contain
          ></v-img>
        <p>Renk: {{ arac.Renk }}</p>
        <p>Yıl: {{ arac.Yil }}</p>
        <p>Fiyat: {{ arac.GunlukFiyat }} / Günlük</p>
        <p>Motor Gücü: {{ arac.MotorGucu }} kw</p>
        <p>Yakıt Türü: {{ arac.YakitTuru }}</p>
        <p>Yolcu Kapasitesi: {{ arac.YolcuKapasitesi }}</p>
        <p>Günlük km: {{ arac.GunlukKM }} km</p>
        <p>Araç Sınıfı: {{ arac.AracSinifi }}</p>
        <p>Ehliyet Yaşı: {{ arac.EhliyetYasi }} ve üzeri</p><br>
        <hr>
      </v-card-text>
    </v-card>
    <v-alert v-else type="info">Yükleniyor...</v-alert>

    <v-window v-if="benzerAraclar.length" show-arrows>
      <template v-slot:prev="{ props }">
        <v-btn color="success" @click="props.onClick"><strong><</strong></v-btn>
      </template>
      <template v-slot:next="{ props }">
        <v-btn color="info" @click="props.onClick"><strong>></strong></v-btn>
      </template>
      <h1>Benzer Araçlar</h1>
      <v-window-item
        v-for="arac in benzerAraclar"
        :key="arac.AracId"
      >


        <v-card>
          <v-card-title style="text-align: center;">{{ arac.Marka }} - {{ arac.Model }}</v-card-title>
          <v-img
            :src="getImageUrl(arac.FotoUrl)"
            class="d-flex align-center justify-center"
            height="300px"
            width="100%"
            contain
          ></v-img>
          
          <v-card-text>
            <p>Fiyat: {{ arac.GunlukFiyat }} / Günlük</p>
            <v-btn @click="goToAracDetay(arac.AracId)">Detayları Gör</v-btn>
          </v-card-text>
        </v-card>
      </v-window-item>
    </v-window>
  </v-container>
  
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';

const arac = ref(null);
const benzerAraclar = ref([]);
const route = useRoute();
const router = useRouter();

const getImageUrl = (fotoUrl) => {
  return new URL(`../assets/${fotoUrl}`, import.meta.url).href;
};

const fetchAracDetails = async (id) => {
  try {
    const response = await axios.get(`http://localhost:5225/api/Arac/${id}`);
    arac.value = response.data;

    // Benzer araçları çek
    const benzerResponse = await axios.get(`http://localhost:5225/api/Arac/BenzerAraclar/${id}`);
    benzerAraclar.value = benzerResponse.data;
  } catch (error) {
    console.error('Araç detayları alınırken bir hata oluştu:', error);
  }
};

onMounted(() => {
  fetchAracDetails(route.params.id);
});

watch(() => route.params.id, (newId) => {
  if (newId) {
    fetchAracDetails(newId);
  }
});

const goToAracDetay = (id) => {
  router.push(`/arac-detay/${id}`);
};
</script>
<style>
h1 {
  text-align: center;
  margin-bottom: 20px;
}

.arac-detay {
  display: flex;
  flex-wrap: wrap;
  gap: 20px; /* Aralarındaki boşluk */
}

.arac-detay p {
  flex: 1 1 calc(50% - 20px); /* Her öğe yatayda %50 genişliğe sahip olacak ve aralardaki boşluk göz önünde bulundurulacak */
  font-size: 18px; /* Yazı boyutu, ihtiyaca göre ayarlanabilir */
  margin: 0; /* Varsayılan marjı kaldır */
}
</style>
