import { createApp } from 'vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import * as icons from "@element-plus/icons-vue"
import router from './router'
import vuex from './vuex'
import axios from './axios/index'
import VueAxios from 'vue-axios'
import App from './App.vue'

const app = createApp(App)

app.use(ElementPlus, { size: 'small', zIndex: 3000})

Object.keys(icons).forEach(key => {
    app.component(key, icons[key])
})
app.use(router)
app.use(VueAxios, axios)
app.use(vuex)

app.mount('#app')
