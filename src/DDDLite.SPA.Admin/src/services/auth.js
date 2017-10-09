import { ajaxGet, ajaxPost } from './ajax'
import Vue from 'vue'

const defaultSession = {
  expriesIn: 3480,
  loginAt: 0,
  tokenType: '',
  accessToken: '',
  refreshToken: '',
  user: null
}

class AuthService {
  constructor() {
    this.authed = false
    this.session = {
      ...defaultSession
    }
  }

  sync() {
    return new Promise((resolve, reject) => {
      this.syncSession()

      if (this.authed) {
        if (this.isExpired()) {
          this.clearSession()
        }
      }

      resolve(this.authed)
    })
  }

  login(username, password) {
    return ajaxPost('/api/v1/connect/token', {
      grant_type: 'password',
      username,
      password,
      scope: 'offline_access phone email roles'
    }, {
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }).then((data) => {
      this.authed = true

      this.session.loginAt = Date.now()
      this.session.expiresIn = data.expires_in - 120
      this.session.tokenType = data.token_type
      this.session.accessToken = data.access_token
      this.session.refreshToken = data.refresh_token

      Vue.$http.defaults.headers.common['Authorization'] = `${this.session.tokenType} ${this.session.accessToken}`

      return this.refreshUserInfo()
    })
  }

  logout() {
    this.authed = false
    this.clearSession()
  }

  isAuthed() {
    return this.authed
  }

  getUserInfo() {
    return this.session.user
  }

  refreshUserInfo() {
    return ajaxGet('/api/v1/userinfo').then(data => {
      this.session.user = data.value
      this.saveSession()
      return this.session.user
    })
  }

  isExpired() {
    let now = Date.now()
    if (now > this.session.loginAt + this.session.expiresIn * 1000) {
      return true
    }

    return false
  }

  syncSession() {
    try {
      if (this.authed) return false

      let value = localStorage.getItem('user_session')
      if (!value) return false

      this.session = JSON.parse(value)

      if (this.isExpired()) {
        this.authed = false
      } else {
        this.authed = true
        Vue.$http.defaults.common.headers['Authorization'] = `${this.session.tokenType} ${this.session.accessToken}`
      }

      return true
    } catch (ex) {
      return false
    }
  }

  clearSession() {
    this.session = { ...defaultSession }
    localStorage.removeItem('user_session')
  }

  saveSession() {
    localStorage.setItem('user_session', JSON.stringify(this.session))
  }
}

export default new AuthService()
