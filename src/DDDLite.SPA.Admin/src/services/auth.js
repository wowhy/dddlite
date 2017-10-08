import { ajaxPost } from './ajax'

let _isAuthed = false

export default {
  login(username, password) {
    return ajaxPost('/connect/token', {
      grant_type: 'password',
      username,
      password,
      scope: 'offline_access roles email'
    }, {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    })
  },

  logout() {},

  isAuthed() {
    return _isAuthed
  }
}
