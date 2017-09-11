import authService from '../services/auth'

export const state = () => ({
  authUser: null,
  menus: []
})

export const mutations = {
  SET_USER(state, user) {
    state.authUser = user
  },

  SET_MENUS(state, menus) {
      state.menus = menus
  }
}

export const actions = {
  nuxtServerInit({ commit }, { req }) {
    if (req.session && req.session.authUser) {
      commit('SET_USER', req.session.authUser)
    }
  },
  async login({ commit }, { username, password }) {
    try {
      const data = await authService.login({ username, password })
      const menus = await authService.getMenus(data.token)
      commit('SET_USER', data.user)
      commit('SET_MENUS', menus)
    } catch (error) {
      if (error.response && error.response.status === 401) {
        throw new Error('Bad credentials')
      }
      throw error
    }
  },

  async logout({ commit }) {
    await authService.logout()
    commit('SET_USER', null)
  }
}
