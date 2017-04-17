import Vue from 'vue'
import App from './App.vue'
import Vuetify from 'vuetify'

require('./stylus/main.styl')

Vue.use(Vuetify)

let app = new Vue({
  el: '#app',
  render: h => h(App)
})
