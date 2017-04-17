import { SpeercsApi } from '../api/speercs'

const state = {
  loggedIn: false,
  authData: {
    un: null,
    key: null
  },
  api: null
}

const actions = {
  authenticate ({commit, state}, auth) {
    return new Promise((resolve, reject) => {
      let resultData = {
        success: false
      }
      console.log(state)
      state.api.login(auth.un, auth.pw)
      .then(() => {
        resultData.un = auth.un
        resultData.key = state.api.apiKey
        commit('login_result', resultData)
        resolve()
      })
      .catch(() => {
        commit('login_result', resultData)
        reject(new Error('login failed'))
      })
    })
  },
  register_account ({commit, state}, auth) {
    let resultData = {
      success: false
    }
    commit('create_api')
    console.log('aa', state.api_available)
    state.api.register(auth.un, auth.pw, auth.invite)
      .then(() => {
        resultData.un = auth.un
        resultData.key = state.api.apiKey
        commit('login_result', resultData)
      })
      .catch(() => {
        commit('login_result', resultData)
      })
  }
}

const mutations = {
  login_result (state, data) {
    if (data.success) {
      state.authData.un = data.un
      state.authData.key = data.key
      state.loggedIn = true
    } else {
      state.authData.un = null
      state.authData.key = null
      state.loggedIn = false
    }
  },
  create_api (state, endpoint) {
    state.api = new SpeercsApi(endpoint)
  }
}

const getters = {
  api_available (state) {
    return state.api !== null
  }
}

export default {
  state,
  actions,
  mutations,
  getters
}
