export const actions = {
  nuxtServerInit({ dispatch, ommit }, { req }) {
    if (req.session && req.session.user) {
      commit('admin/SET_USER', req.session.user)
    }

    // for debug
    return dispatch('admin/login', { username: '', password: '' })
  }
}
