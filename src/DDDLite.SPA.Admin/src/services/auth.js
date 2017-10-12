import { LocalStorage } from 'quasar'
import { ajaxGet, ajaxPost } from './ajax'

const defaultSession = {
  expiresIn: 3480,
  loginAt: 0,
  tokenType: null,
  accessToken: null,
  refreshToken: null
}

class AuthService {
  constructor() {
    this.refreshTokenTimer = 0
  }

  // 同步登录状态
  async sync() {
    try {
      this.session = LocalStorage.get.item('session') || { ...defaultSession }
      this.user = LocalStorage.get.item('user') || {}

      if (this.session.refreshToken) {
        await this.refreshToken()
      }
    } catch (ex) {
      this.session = { ...defaultSession }
      this.user = {}
    }
  }

  enableAutoRefreshToken() {
    if (this.session.expiresIn && this.session.expiresIn > 0) {
      this.refreshTokenTimer = setTimeout(() => {
        this.refreshToken()
      }, this.session.expiresIn * 1000)
    }
  }

  disableAutoRefreshToken() {
    if (this.refreshTokenTimer) {
      clearTimeout(this.refreshTokenTimer)
    }
  }

  // 登录
  async login(username, password, longSave = true) {
    let params = {
      grant_type: 'password',
      username,
      password,
      scope: 'phone email roles' + (longSave ? ' offline_access' : '')
    }

    await this.$$login(params)
  }

  async refreshUserInfo() {
    let data = await ajaxGet('/api/v1/userinfo')
    this.user = data.value

    LocalStorage.set('user', this.user)

    return this.user
  }

  async refreshToken() {
    let params = {
      grant_type: 'refresh_token',
      refresh_token: this.session.refreshToken
    }

    await this.$$login(params)
  }

  // 退出
  logout() {
    this.session = { ...defaultSession }
    this.user = {}
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

  async $$login(params) {
    let now = Date.now()

    this.disableAutoRefreshToken()

    let loginData = await getToken(params)
    let userData = await getUser(loginData.token_type, loginData.access_token)

    this.session = createSession(loginData)
    this.session.loginAt = now
    this.user = userData.value

    if (params.refresh_token) {
      this.session.refreshToken = params.refresh_token
    }

    LocalStorage.set('session', this.session)
    LocalStorage.set('user', this.user)

    if (this.session.refreshToken) {
      this.enableAutoRefreshToken()
    }
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

async function getUser(tokenType, accessToken) {
  let Authorization = `${tokenType} ${accessToken}`
  return ajaxGet('/api/v1/userinfo', {}, {
    headers: {
      Authorization
    }
  })
}

function createSession(data) {
  return {
    expiresIn: data.expires_in - 120,
    tokenType: data.token_type,
    accessToken: data.access_token,
    refreshToken: data.refresh_token
  }
}

function clearSession() {
  LocalStorage.remove('session')
  LocalStorage.remove('user')
}
