export const actions = {
  nuxtServerInit({ dispatch, ommit }, { req }) {
    if (req.session && req.session.authUser) {
      commit('admin/SET_USER', req.session.authUser)
    }

    return dispatch('admin/login', { username: '', password: '' })
  }
}
