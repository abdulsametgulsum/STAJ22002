<template>
  <v-container>
    
    
    <v-card class="mt-4 mx-auto" max-width="500">
      <v-card-title>Ödeme Sayfası</v-card-title>

      <v-card-text>
        <v-form>
          <v-text-field
            v-model="cardNumber"
            label="Kart Numarası"
            outlined
            type="text"
            :rules="[rules.required, rules.cardNumber]"
          ></v-text-field>

          <v-text-field
            v-model="cardHolder"
            label="Kart Sahibinin Adı"
            outlined
            type="text"
            :rules="[rules.required]"
          ></v-text-field>

          <v-text-field
            v-model="expiryDate"
            label="Son Kullanma Tarihi (MM/YY)"
            outlined
            type="text"
            :rules="[rules.required, rules.expiryDate]"
          ></v-text-field>

          <v-text-field
            v-model="cvv"
            label="CVV"
            outlined
            type="text"
            :rules="[rules.required, rules.cvv]"
          ></v-text-field>

          <v-card-text class="mt-4">
            <p><strong>Toplam Tutar:</strong> {{ totalPrice }} TL</p>
          </v-card-text>

          <v-btn color="success" @click="submitPayment">Ödemeyi Tamamla</v-btn>
        </v-form>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';
import { useStore } from 'vuex';

const store = useStore();
const isAuthenticated = computed(() => store.getters.isAuthenticated);
const user = computed(() => store.getters.getUser);

const route = useRoute();

const cardNumber = ref('');
const cardHolder = ref('');
const expiryDate = ref('');
const cvv = ref('');

const rules = {
  required: v => !!v || 'Bu alan gerekli.',
  cardNumber: v => /^\d{16}$/.test(v) || 'Geçersiz kart numarası.',
  expiryDate: v => /^(0[1-9]|1[0-2])\/\d{2}$/.test(v) || 'Geçersiz son kullanma tarihi. Format: MM/YY',
  cvv: v => /^\d{3}$/.test(v) || 'Geçersiz CVV.'
};

const totalPrice = ref(route.query.totalPrice);
const selectedPackage = ref(null);
const selectedServices = ref([]);
const formattedPickupDate = new Date(route.query.pickupDate);
const formattedDropoffDate = new Date(route.query.dropoffDate);

if (isNaN(formattedPickupDate.getTime()) || isNaN(formattedDropoffDate.getTime())) {
  console.error('Geçersiz tarih formatı.');
}

const minDate = new Date('1753-01-01T00:00:00Z');
const maxDate = new Date('9999-12-31T23:59:59Z');

if (formattedPickupDate < minDate || formattedPickupDate > maxDate ||
    formattedDropoffDate < minDate || formattedDropoffDate > maxDate) {
  console.error('Tarih aralığı geçersiz.');
}

const submitPayment = async () => {
  if (!isAuthenticated.value) {
    console.error('Kullanıcı giriş yapmamış.');
    return;
  }

  const currentUser = user.value; // Kullanıcı bilgilerini al

  if (!currentUser || !currentUser.id) {
    console.error('Kullanıcı ID bulunamadı.');
    return;
  }

  try {
    const response = await axios.post('http://localhost:5225/api/Reservation/CompleteReservation', {
      KullaniciId: currentUser.id,
      AracId: route.query.carID,
      KiralamaTarihi: formattedPickupDate.toISOString(),
      DonusTarihi: formattedDropoffDate.toISOString(),
      GuvencePaketiId: selectedPackage.value,
      EkHizmetler: JSON.stringify(selectedServices.value),
      ToplamFiyat: totalPrice.value
    });
    console.log('Ödeme başarılı:', response.data);
  } catch (error) {
    console.error('Ödeme yapılamadı:', error);
  }
};
</script>



<style scoped>
/* Stilinizi burada ekleyin */
</style>
