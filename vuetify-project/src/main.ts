import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import { defineRule } from 'vee-validate'
import { required, email, min, max } from '@vee-validate/rules'
import store from './store'

const app = createApp(App)

app.use(store); 
app.use(router)
app.use(vuetify)

// VeeValidate kurallarını tanımlayın
defineRule('required', required)
defineRule('email', email)
defineRule('min', min)
defineRule('max', max)

app.mount('#app')


