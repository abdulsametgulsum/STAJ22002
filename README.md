#Zorunlu Yaz Stajı(STAJ22001) Rent A Car Sitesi 


1210505030	Abdulsamet Gülsüm	Yazılım Mühendisliği

#Proje Açıklaması
Bu proje, Vue.js tabanlı bir uygulama olup Axios ile veri çekme, child component (alt bileşen) direktifleri, form doğrulama ve `v-model` gibi Vue.js özelliklerini içerir. Proje, kullanıcı dostu bir arayüzle veri alışverişi sağlayan formlar ve analiz araçları sunar.
##Kurulum ve Kullanım
Bu proje, Vue.js, Vuetify ve Node.js tabanlı bir Rent A Car web uygulamasıdır. Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edin:


1. Node.js Kurulumu
Projeyi çalıştırabilmek için öncelikle bilgisayarınızda Node.js yüklü olmalıdır. Eğer Node.js yüklü değilse, aşağıdaki adımları izleyerek kurabilirsiniz:
Node.js İndir sayfasından işletim sisteminize uygun sürümü indirin ve yükleyin.
Node.js kurulumunu doğrulamak için terminal ya da komut satırında aşağıdaki komutları çalıştırın:
node -v
npm -v
Eğer komutlar başarıyla çalışırsa, Node.js ve npm'in (Node Package Manager) yüklü olduğunu göreceksiniz.

2. Vue CLI Kurulumu
Projeyi yerel ortamınızda başlatmak için Vue CLI yüklü olmalıdır. Vue CLI'yi global olarak yüklemek için aşağıdaki komutu terminalde çalıştırın:
npm install -g @vue/cli
Vue CLI'nin yüklü olduğunu doğrulamak için şu komutu kullanın:

3. Projeyi Klonlayın
Projeyi yerel bilgisayarınıza klonlayın:
git clone https://github.com/abdulsametgulsum/STAJ22001/
Projeyi klonladıktan sonra, proje dizinine gidin:
cd vuetify-project

4. Bağımlılıkların Kurulumu
Proje dizininde aşağıdaki komutları çalıştırarak gerekli bağımlılıkları yükleyin:
npm install
Bu komut, projenin çalışabilmesi için gereken tüm Node.js ve Vue.js paketlerini yükleyecektir.

5. Projeyi Başlatın
Projeyi yerel sunucuda çalıştırmak için şu komutu kullanın:
npm run dev
Bu komut, uygulamayı başlatacak ve tarayıcınızda projeyi açmak için gerekli bağlantıyı sağlayacaktır. Varsayılan olarak, uygulama şu adreste çalışır: http://localhost:8080.

6. Proje Yapısı
Projede kullanılan ana teknolojiler:

Vue.js: Kullanıcı arayüzü geliştirmede kullanılan JavaScript framework'ü.
Vuetify: Material Design prensiplerini kullanan bir Vue.js UI kütüphanesi.
Axios: Backend ile haberleşme ve veri çekmek için kullanılan HTTP istemcisi.

7. API Bağlantısı
Eğer projede bir backend API bağlantısı yapıyorsanız, Axios ile kullanılan baseURL ayarlarını yapılandırmayı unutmayın. Örneğin:
axios.defaults.baseURL = 'http://localhost:5252/api';

8. Kullanım
Proje çalıştırıldıktan sonra, aşağıdaki özellikleri kullanabilirsiniz:


