import { ajaxPost } from './ajax'
import jwtDecode from 'jwt-decode'

let authed = false
let session = {
  expriesIn: 3480,
  loginAt: 0,
  accessToken: '',
  refreshToken: '',
  profile: null
}

export default {
  sync() {
    return new Promise((resolve, reject) => {
      syncSession()

      if (authed) {
        let now = Date.now()
        if (now > session.loginAt + session.expiresIn * 1000) {
          authed = false
          clearSession()
        }
      }

      resolve(authed)
    })
  },

  login(username, password) {
    return ajaxPost('/api/v1/connect/token', {
      grant_type: 'password',
      username,
      password,
      scope: 'offline_access name email roles'
    }, {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }).then((data) => {
      authed = true

      session.loginAt = Date.now()
      session.expiresIn = data.expires_in - 120
      session.accessToken = data.access_token
      session.refreshToken = data.refresh_token
      session.profile = jwtDecode(data.access_token)

      saveSession()
    })
  },

  logout() {},

  isAuthed() {
    return authed
  }
}

function syncSession() {
  try {
    if (authed) return false

    let value = localStorage.getItem('user_session')
    if (!value) return false

    session = JSON.parse(value)

    if (Date.now() > session.loginAt + session.expiresIn * 1000) {
      authed = false
    } else {
      authed = true
    }

    return true
  } catch (ex) {
    return false
  }
}

function clearSession() {
  localStorage.removeItem('user_session')
}

function saveSession() {
  localStorage.setItem('user_session', JSON.stringify(session))
}
