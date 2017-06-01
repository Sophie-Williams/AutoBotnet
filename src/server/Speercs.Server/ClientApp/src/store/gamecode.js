const state = {
}

const actions = {
  deploy_user_code ({commit, state}, args) {
    return args.api.deployUserCode(args.source)
  },
  get_user_code ({commit, state}, args) {
    return args.api.getUserCode()
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
