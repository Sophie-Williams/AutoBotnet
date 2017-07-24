const state = {
}

const actions = {
  get_map_room ({commit, state}, args) {
    return args.api.getMapRoom(args.x, args.y)
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
