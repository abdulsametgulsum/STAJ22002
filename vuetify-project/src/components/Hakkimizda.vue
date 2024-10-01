<template>
    <v-container fluid>
      <v-row>
        <v-col cols="12">
          <v-card class="h-100">
            <v-card-title class="headline">Hakkımızda</v-card-title>
            <v-card-text>
              <p>Biz, araç kiralama sektöründe mükemmel hizmet sunmayı kendimize misyon edindik. Müşterilerimize güvenli, konforlu ve kaliteli araçlar sunarak onların seyahat deneyimlerini en üst seviyeye çıkarmayı hedefliyoruz. Müşterilerimizin memnuniyeti bizim için ön plandadır ve her müşterimizin ihtiyaçlarını anlamak için sürekli olarak çaba gösteriyoruz.</p>
  
              <h2>Vizyonumuz</h2>
              <p>Vizyonumuz, araç kiralama sektöründe lider bir şirket olmak ve müşterilerimize en iyi hizmeti sunarak sektördeki en güvenilir marka olmaktır. Yenilikçi hizmet anlayışımız ve müşteri odaklı yaklaşımımız ile her geçen gün daha da büyümeyi amaçlıyoruz. Ayrıca, çevre dostu araç seçeneklerimizle sürdürülebilir bir gelecek için de katkıda bulunmayı hedefliyoruz.</p>
  
              <h2>Misyonumuz</h2>
              <p>Misyonumuz, müşterilerimizin ihtiyaçlarına en uygun araçları sunarak onların seyahat deneyimlerini mükemmel hale getirmektir. Kaliteli araçlar, güvenilir hizmet ve müşteri memnuniyeti bizim için ön plandadır. Her müşterimizin ihtiyaçlarını anlamak ve onlara kişiye özel çözümler sunmak için çalışıyoruz. Bu doğrultuda, her geçen gün hizmet kalitemizi artırmayı hedefliyoruz.</p>
  
              <h2>Değerlerimiz</h2>
              <ul>
                <li><strong>Güvenilirlik:</strong> Müşterilerimize güvenilir araçlar ve hizmetler sunarız.</li>
                <li><strong>Kalite:</strong> Yüksek kaliteli araçlar ve profesyonel hizmet anlayışını benimseriz.</li>
                <li><strong>Yenilikçilik:</strong> Sektördeki yenilikleri takip eder ve hizmetlerimizi sürekli geliştiririz.</li>
                <li><strong>Müşteri Memnuniyeti:</strong> Müşterilerimizin memnuniyetini ön planda tutarız.</li>
                <li><strong>Sürdürülebilirlik:</strong> Çevre dostu araçlar ve uygulamalarla sürdürülebilir bir gelecek için çalışırız.</li>
              </ul>
  
              <p>Bizimle iletişime geçmek veya hizmetlerimiz hakkında daha fazla bilgi almak için <router-link to="/bize-ulasin">Bize Ulaşın</router-link> sayfasını ziyaret edebilirsiniz. Her türlü soru ve geri bildiriminiz bizim için değerlidir.</p>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </v-container>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  </script>
  
  <style scoped>
  .v-card {
    padding: 2rem;
  }
  
  .v-card-title {
    font-size: 2rem;
    margin-bottom: 1rem;
  }
  
  .v-card-text {
    font-size: 1rem;
    line-height: 1.6;
  }
  
  ul {
    padding-left: 1.5rem;
  }
  
  p {
    margin-bottom: 1rem;
  }
  </style>
  