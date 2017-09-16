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
      resolve({
        token: '',
        user: {
          id: '',
          username: username,
          fullname: '管理员'
        }
      })
    })
  },

  logout() {
    return new Promise((resolve) => {
      resolve({})
    })
  }
}
