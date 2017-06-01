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
          let sampleProgram = `
function loop () {
  // hello world!
}
          `.trim()
          resolve(rs.data.source || sampleProgram)
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
