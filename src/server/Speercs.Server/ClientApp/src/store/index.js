import Vue from 'vue'
import Vuex from 'vuex'

// store modules
import auth from './auth'
import gamecode from './gamecode'
import data from './data'
import serverinfo from './serverinfo'
import game from './game'

Vue.use(Vuex)

export default new Vuex.Store({
  modules: {
    auth,
    data,
    gamecode,
    serverinfo,
    game
  }
})
