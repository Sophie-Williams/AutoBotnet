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
  ensure_api ({commit, state}, endpoint) {
    return new Promise((resolve, reject) => {
      if (state.api === null) {
        commit('create_api', endpoint)
        resolve(true)
      }
      resolve(false)
    })
  },
  authenticate ({commit, state}, auth) {
    return new Promise((resolve, reject) => {
      let resultData = {
        success: false
      }
      state.api.login(auth.un, auth.pw)
      .then(() => {
        resultData.success = true
        resultData.un = auth.un
        resultData.key = state.api.getApiKey()
        commit('login_result', resultData)
        resolve()
      })
      .catch((e) => {
        commit('login_result', resultData)
        console.log(e)
        reject(new Error('login failed'))
      })
    })
  },
  register_account ({commit, state}, auth) {
    return new Promise((resolve, reject) => {
      let resultData = {
        success: false
      }
      state.api.register(auth.un, auth.pw, auth.invite)
      .then(() => {
        resultData.success = true
        resultData.un = auth.un
        resultData.key = state.api.getApiKey()
        commit('login_result', resultData)
      })
      .catch(() => {
        commit('login_result', resultData)
        reject(new Error('register failed'))
      })
    })
  },
  logout ({commit, state}) {
    return new Promise((resolve, reject) => {
      state.api.logout()
      commit('login_result', { success: false })
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
  },
  auth_data (state) {
    return state.authData
  }
}

export default {
  state,
  actions,
  mutations,
  getters
}
