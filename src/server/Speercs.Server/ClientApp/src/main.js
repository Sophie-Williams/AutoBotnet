import Vue from 'vue'
import Vuetify from 'vuetify'
import VueRouter from 'vue-router'

// app
import App from './App.vue'

// routes
import routes from './routes'

// store
import store from './store/index'

// ui styles

import './stylus/main.styl'

// register plugins
Vue.use(VueRouter)
Vue.use(Vuetify)

// router setup
let router = new VueRouter({
  routes
})

const app = new Vue({
  el: '#app',
  render: h => h(App),
  router,
  store
})
