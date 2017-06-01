const state = {
}

const actions = {
  deploy_user_code ({commit, state}, args) {
    return args.api.deployUserCode(args.source)
  },
  get_user_code ({commit, state}, args) {
    return new Promise((resolve, reject) => {
      args.api.getUserCode()
        .then((rs) => {
          resolve(rs.data)
        })
        .catch((e) => reject(e))
    })
  },
  reload_user_code ({commit, state}, args) {
    return args.api.reloadUserCode()
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
