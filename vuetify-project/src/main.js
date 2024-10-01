import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router'
import { defineRule } from 'vee-validate'
import { required, email, min, max } from '@vee-validate/rules'
import { loadFonts } from './plugins/webfontloader'
import store from './store';
const app = createApp(App)
app.use(store); 
app.use(router)
app.use(vuetify)
loadFonts()
defineRule('required', required)
defineRule('email', email)
defineRule('min', min)
defineRule('max', max)
app.mount('#app')
