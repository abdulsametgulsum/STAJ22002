<template>
    <div>
      <v-card class="mt-4">
        <v-card-text>
          
        </v-card-text>
        <v-btn color="primary" @click="goToPayment">Devam Et</v-btn>
      </v-card>
    </div>
  </template>
  
  <script setup>
  import { useRouter } from 'vue-router';
  
  const router = useRouter();
  
  const goToPayment = () => {
    router.push('/payment');
  };
  </script>
  