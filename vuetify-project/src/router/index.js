import { createRouter, createWebHistory } from 'vue-router';
import { useStore } from 'vuex';
import RegisterForm from '../components/RegisterForm.vue';
import Filomuz from '../components/Filomuz.vue';
import AracDetay from '../components/AracDetay.vue';
import BizeUlasın from '../components/BizeUlasın.vue';
import Home from '../components/Home.vue';
import CarResult from '../components/CarResult.vue';
import Payment from '../components/Payment.vue';
import ReservationDetails from '../components/ReservationDetails.vue';
import InsuranceAndServices from '../components/InsuranceAndServices.vue';
import KullaniciListele from '../components/KullaniciListele.vue';
import RezervasyonListele from '../components/RezervasyonListele.vue';

import AdminPanel from '../components/AdminPanel.vue';
import Hakkimizda from '../components/Hakkimizda.vue';
import AddArac from '../components/AddArac.vue';
import AdminArac from '../components/AdminArac.vue';

// Rota tanımları
const routes = [
  {path : '/arac-listele', component:AdminArac},
  { path: '/arac-ekle', component: AddArac },
  { path: '/', name: 'Home', component: Home },
  { path: '/register', name: 'Register', component: RegisterForm },
  { path: '/filomuz', name: 'Filomuz', component: Filomuz },
  { path: '/arac-detay/:id', component: AracDetay, props: true },
  { path: '/bize-ulasin', name: 'BizeUlasın', component: BizeUlasın },
  { path: '/results', name: 'CarResults', component: CarResult },
  { path: '/payment', name: 'Payment', component: Payment },
  { path: '/', redirect: '/reservation-details' },
  { path: '/hakkimizda', name: 'Hakkimizda', component: Hakkimizda },
  { path: '/reservation-details/:carId', name: 'ReservationDetails', component: ReservationDetails, props: route => ({
      pickupDate: route.query.pickupDate,
      dropoffDate: route.query.dropoffDate
    })
  },
  { path: '/insurance-and-services', name: 'InsuranceAndServices', component: InsuranceAndServices },
  { path: '/admin', component: AdminPanel, meta: { hideNavbar: true } },
  {
    path: '/kullanici-listele',
    name: 'KullaniciListele',
    component: KullaniciListele
  },
  {
    path: '/rezervasyon-listele',
    name: 'RezervasyonListele',
    component: RezervasyonListele
  },

];

// Router'ı oluştur
const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

// Router navigasyon koruma
router.beforeEach((to, from, next) => {
  // Vuex store'ı al
  const store = useStore();
  if (to.meta.requiresAuth && !store.getters.isAuthenticated) {
    next('/login');
  } else {
    next();
  }
});

export default router;
