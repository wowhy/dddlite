import authService from '../services/auth'

export const state = () => ({
  user: null,
  menus: [],
  showMenu: true
})

export const getters = {
  menus: state => state.menus,
  showMenu: state => state.showMenu,
  user: state => state.user
}

export const mutations = {
  SET_USER(state, user) {
    state.user = user
  },

  SET_MENUS(state, menus) {
      state.menus = menus
  },

  SET_SHOWMENU(state, showMenu) {
    state.showMenu = showMenu
  }
}

export const actions = {
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
  },

  toggleMenu({ commit, getters }) {
    commit('SET_SHOWMENU', !getters.showMenu)
  }
}
