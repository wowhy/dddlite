export default {
  getMenus() {
    return new Promise((resolve) => {
      resolve([
        {
          code: '1',
          text: '菜单1',
          children: [
            { code: '1-1', text: '菜单2', url: '/demo' }
          ]
        }
      ])
    })
  },

  login({ username, password }) {
    return new Promise((resolve) => {
      resolve({})
    })
  },

  logout() {
    return new Promise((resolve) => {
      resolve({})
    })
  }
}
