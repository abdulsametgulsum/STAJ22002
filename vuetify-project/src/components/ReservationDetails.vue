<template>
    <div>
      <v-card class="mt-4">
        <v-card-text v-if="carDetails">
          <v-card-title>{{ carDetails.Marka }} - {{ carDetails.Model }}</v-card-title>
          <v-card-subtitle>Aracın Günlük Fiyatı: {{ carDetails.GunlukFiyat }} TL</v-card-subtitle>
          <v-card-subtitle>Teslim Alma Tarihi: {{ formattedPickupDate }}</v-card-subtitle>
          <v-card-subtitle>Teslim Etme Tarihi: {{ formatteddropOffDate }}</v-card-subtitle>
          <v-card-subtitle>Kiralama Süresi: {{ dayDifference }} gün</v-card-subtitle>
          <v-card-subtitle>Toplam Tutar (Araç): {{ totalPrice }} TL</v-card-subtitle>
        </v-card-text>
  
        <v-card class="mt-4">
          <v-card-title>Güvence Paketleri</v-card-title>
          <v-container>
            <v-row>
              <v-col v-for="(pkg, index) in insurancePackages" :key="pkg.GuvencePaketiId" cols="12" md="3">
                <v-card>
                  <v-card-title>{{ pkg.PaketAdi }}</v-card-title>
                  <v-card-text>{{ pkg.Aciklama }}</v-card-text>
                  <v-card-subtitle>{{ pkg.Fiyat }} TL</v-card-subtitle>
                  <v-card-actions>
                    <v-checkbox
                      v-model="selectedPackage"
                      :value="pkg"
                      :label="pkg.PaketAdi"
                      color="primary"
                      :true-value="pkg"
                      :false-value="null"
                    />
                  </v-card-actions>
                </v-card>
              </v-col>
            </v-row>
          </v-container>
        </v-card>
  
        <v-card class="mt-4">
          <v-card-title>Ek Hizmetler</v-card-title>
          <v-container>
            <v-row>
              <v-col v-for="(svc, index) in additionalServices" :key="svc.Id" cols="12" md="3">
                <v-card>
                  <v-card-title>{{ svc.HizmetAdi }}</v-card-title>
                  <v-card-text>{{ svc.Aciklama }}</v-card-text>
                  <v-card-subtitle>{{ svc.Fiyat }} TL</v-card-subtitle>
                  <v-card-actions>
                    <v-checkbox
                      v-model="selectedServices"
                      :value="svc"
                      :label="svc.HizmetAdi"
                      color="primary"
                    />
                  </v-card-actions>
                </v-card>
              </v-col>
            </v-row>
          </v-container>
        </v-card>
  
        <v-card class="mt-4">
          <v-card-text>
            <strong><p>Toplam Fiyat: {{ finalPrice }} TL</p></strong>
          </v-card-text>
          <v-btn color="success" @click="goToPayment">Ödemeye Geç</v-btn>
        </v-card>
      </v-card>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import axios from 'axios';
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  
  const carId = ref(route.params.carId);
  const pickupDate = ref(new Date(route.query.pickupDate));
  const dropoffDate = ref(new Date(route.query.dropoffDate));
  
  const dayDifference = computed(() => {
    const timeDifference = dropoffDate.value - pickupDate.value;
    return Math.ceil(timeDifference / (1000 * 3600 * 24));
  });
  
  const carDetails = ref(null);
  
  const insurancePackages = ref([]);
  const additionalServices = ref([]);
  
  const selectedPackage = ref(null); // Tek bir paket seçili
  const selectedServices = ref([]); // Birden fazla hizmet seçili
  
  const formattedPickupDate = computed(() => {
    return pickupDate.value.toISOString().split('T')[0];
  });
  const formatteddropOffDate = computed(() => {
    return dropoffDate.value.toISOString().split('T')[0];
  });
  
  const totalPrice = computed(() => {
    return carDetails.value ? carDetails.value.GunlukFiyat * dayDifference.value : 0;
  });
  
  const finalPrice = computed(() => {
    const insuranceTotal = selectedPackage.value ? selectedPackage.value.Fiyat : 0;
    const servicesTotal = selectedServices.value.reduce((sum, svc) => sum + svc.Fiyat, 0);
    return totalPrice.value + insuranceTotal + servicesTotal;
  });
  
  const fetchCarDetails = async () => {
    try {
      const response = await axios.get(`http://localhost:5225/api/Arac/${carId.value}`);
      carDetails.value = response.data;
    } catch (error) {
      console.error('Araç detayları çekilemedi:', error);
    }
  };
  
  const fetchInsurancePackages = async () => {
    try {
      const response = await axios.get('http://localhost:5225/api/GuvencePaketleri');
      insurancePackages.value = response.data;
      // Varsayılan olarak standart paketi seçili yap
      selectedPackage.value = insurancePackages.value.find(pkg => pkg.PaketAdi === 'Standart');
    } catch (error) {
      console.error('Güvence paketleri çekilemedi:', error);
    }
  };
  
  const fetchAdditionalServices = async () => {
    try {
      const response = await axios.get('http://localhost:5225/api/EkHizmetler');
      additionalServices.value = response.data;
    } catch (error) {
      console.error('Ek hizmetler çekilemedi:', error);
    }
  };
  
  const goToPayment = () => {
  router.push({
    path: '/payment',
    query: {
      totalPrice: finalPrice.value, // Toplam fiyat
      carId: carId.value,
      pickupDate: pickupDate.value.toISOString().split('T')[0],
      dropoffDate: dropoffDate.value.toISOString().split('T')[0],
      selectedPackage: JSON.stringify(selectedPackage.value), // Güvence paketi
      selectedServices: JSON.stringify(selectedServices.value) // Ek hizmetler
    }
  });
};

  
  onMounted(() => {
    fetchCarDetails();
    fetchInsurancePackages();
    fetchAdditionalServices();
  });
  </script>
  