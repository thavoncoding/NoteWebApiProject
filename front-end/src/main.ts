// import { createApp } from 'vue'
// import App from './App.vue'
// import './style.css'
// import router from './router'


// createApp(App).use(router).mount('#app');

import { createApp } from 'vue'
import App from './App.vue'
import './style.css'
import router from './router'
import VueToast from 'vue-toast-notification'
import 'vue-toast-notification/dist/theme-sugar.css'


const app = createApp(App)

app.use(router)
app.use(VueToast)
app.mount('#app')
