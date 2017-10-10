import { ajaxGet, ajaxPost } from './ajax'
import Vue from 'vue'

const defaultSession = {
  expriesIn: 3480,
  loginAt: 0,
  tokenType: null,
  accessToken: null,
  refreshToken: null,
  user: null
}

class AuthService {
  constructor() {
    this.refreshTokenTimer = 0
    this.session = {
      ...defaultSession
    }
  }

  // 同步登录状态
  sync() {
    return new Promise((resolve, reject) => {
      this.session = getSession()
      if (this.session.refreshToken) {
        this.refreshToken().then(resolve, resolve)
      } else {
        resolve()
      }
    })
  }

  enableAutoRefreshToken() {
    this.refreshTokenTimer = setTimeout(() => {
      this.refreshToken()
    }, this.session.expriesIn * 1000)
  }

  disableAutoRefreshToken() {
    if (this.refreshTokenTimer) {
      clearTimeout(this.refreshTokenTimer)
    }
  }

  // 登录
  login(username, password, longSave = true) {
    let params = {
      grant_type: 'password',
      username,
      password,
      scope: 'phone email roles' + (longSave ? ' offline_access' : '')
    }
    return getToken(params).then(this.processLoginRequest.bind(this)).then(() => {
      if (this.session.refresh_token) {
        this.enableAutoRefreshToken()
      }
    })
  }

  refreshUserInfo() {
    return ajaxGet('/api/v1/userinfo').then(data => {
      this.session.user = data.value
      saveSession(this.session)
      return this.session.user
    })
  }

  refreshToken() {
    let params = {
      grant_type: 'refresh_token',
      refresh_token: this.session.refreshToken
    }
    this.disableAutoRefreshToken()
    return getToken(params).then(data => {
      return this.processLoginRequest(data)
    }).then(() => {
      this.enableAutoRefreshToken()
    })
  }

  // 退出
  logout() {
    this.session = { ...defaultSession }
    clearSession()
  }

  isAuthed() {
    return !this.isExpired()
  }

  getUserInfo() {
    return this.session.user
  }

  isExpired() {
    let now = Date.now()
    if (now > this.session.loginAt + this.session.expiresIn * 1000) {
      return true
    }

    return false
  }

  processLoginRequest(loginData) {
    let now = Date.now()
    let Authorization = `${loginData.token_type} ${loginData.access_token}`
    return ajaxGet('/api/v1/userinfo', {}, {
      headers: {
        Authorization
      }
    }).then(userData => {
      Vue.$http.defaults.headers.common['Authorization'] = Authorization

      this.session.loginAt = now
      this.session.expiresIn = loginData.expires_in - 120
      this.session.tokenType = loginData.token_type
      this.session.accessToken = loginData.access_token

      if (loginData.refresh_token) {
        this.session.refreshToken = loginData.refresh_token
      }

      this.session.user = userData.value

      saveSession(this.session)
    })
  }
}

export default new AuthService()

function getToken(model) {
  let url = '/api/v1/connect/token'
  let headers = {
    'Content-Type': 'application/x-www-form-urlencoded'
  }

  return ajaxPost(url, model, {
    headers
  })
}

function getSession() {
  try {
    let value = localStorage.getItem('user_session')
    if (!value) return { ...defaultSession }

    let session = JSON.parse(value)
    return session
  } catch (ex) {
    return { ...defaultSession }
  }
}

function clearSession() {
  localStorage.removeItem('user_session')
}

function saveSession(session) {
  localStorage.setItem('user_session', JSON.stringify(session))
}
