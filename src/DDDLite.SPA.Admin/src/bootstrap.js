// === DEFAULT / CUSTOM STYLE ===
// WARNING! always comment out ONE of the two require() calls below.
// 1. use next line to activate CUSTOM STYLE (./src/themes)
// require(`./themes/app.${__THEME}.styl`)
// 2. or, use next line to activate DEFAULT QUASAR STYLE
require(`quasar/dist/quasar.${__THEME}.css`)

// Uncomment the following lines if you need IE11/Edge support
// require(`quasar/dist/quasar.ie`)
// require(`quasar/dist/quasar.ie.${__THEME}.css`)

import Vue from 'vue'
import Quasar, { Alert } from 'quasar'
import router from './router'

if (__THEME === 'mat') {
  require('quasar-extras/roboto-font')
}
import 'quasar-extras/material-icons'
import 'quasar-extras/ionicons'
// import 'quasar-extras/fontawesome'
import 'quasar-extras/animate'

import axios from 'axios'
import qs from 'qs'

export default function() {
  return new Promise((resolve, reject) => {
    init(resolve, reject)
  })
}

const alert = msg => {
  let instance = Alert.create({
    enter: 'bounceInRight',
    leave: 'bounceOutRight',
    html: msg
  })

  setTimeout(() => instance.dismiss(), 5000)
}

function init(resolve, reject) {
  Vue.config.productionTip = false
  Vue.use(Quasar) // Install Quasar Framework

  // init $http
  let $http = axios.create()

  Vue.prototype.$http = $http
  Vue.$http = $http

  $http.interceptors.request.use((request) => {
    if (request.data && request.data.headers && request.data.headers['Content-Type'] === 'application/x-www-form-urlencoded') {
      request.headers['Content-Type'] = 'application/x-www-form-urlencoded'
      request.data = qs.stringify(request.data.data)
    }
    return request
  }, (error) => Promise.reject(error))

  $http.interceptors.response.use((res) => {
    return res
  }, ({ response }) => {
    let data = response.data
    if (typeof response.data !== 'object') {
      data = {
        error: {
          code: 'Unknown',
          message: '服务器发生未知错误，请联系管理员！'
        }
      }
    }

    if (response.status === 401) {
      data = {
        error: {
          message: '登录超时，请重新登录！'
        }
      }
      router.push('/')
    }

    alert(data.error.message)

    return Promise.reject(data)
  })

  // init router
  const authService = require('./services/auth').default
  router.beforeEach((to, from, next) => {
    if (to.matched.some(k => k.meta.authorize)) {
      if (!authService.isAuthed()) {
        next('/login')
        return
      }
    }

    next()
  })

  // start quasar
  Quasar.start(() => {
    resolve(router)
  })
}
