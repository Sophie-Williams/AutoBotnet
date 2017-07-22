const state = {
}

const actions = {
  get_server_info ({commit, state}, args) {
    return new Promise((resolve, reject) => {
      args.api.getInfo()
        .then((rs) => {
          resolve(rs.data)
        })
        .catch((e) => reject(e))
    })
  }
}

const mutations = {
}

const getters = {
}

export default {
  state,
  actions,
  mutations,
  getters
}
