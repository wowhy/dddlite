export default {
  getMenus() {
    return new Promise((resolve) => {
      resolve([
        {
          code: '1',
          text: '基础信息',
          children: [
            { code: '1-1', text: '用户管理', url: '/admin/users' }
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
