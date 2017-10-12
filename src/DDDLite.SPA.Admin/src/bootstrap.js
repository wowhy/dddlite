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
import Quasar, * as All from 'quasar'

import qs from 'qs'

if (__THEME === 'mat') {
  require('quasar-extras/roboto-font')
}
import 'quasar-extras/material-icons'
import 'quasar-extras/ionicons'
// import 'quasar-extras/fontawesome'
import 'quasar-extras/animate'

import $http from './http'
import router from './router'
import authService from './services/auth'

Vue.config.productionTip = false
Vue.use(Quasar, {
  components: All,
  directives: All
}) // Install Quasar Framework

Vue.$http = Vue.prototype.$http = $http

export default function() {
  return new Promise((resolve, reject) => {
    init(resolve, reject)
  })
}

const alert = msg => {
  let instance = All.Alert.create({
    enter: 'bounceInRight',
    leave: 'bounceOutRight',
    html: msg
  })

  setTimeout(() => instance.dismiss(), 5000)
}

function init(resolve, reject) {
  // init $http
  $http.interceptors.request.use((request) => {
    if (request.data && request.headers['Content-Type'] === 'application/x-www-form-urlencoded') {
      request.data = qs.stringify(request.data)
    }
    return request
  }, (error) => Promise.reject(error))

  $http.interceptors.response.use((res) => res.data, ({ response }) => {
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
  router.beforeEach((to, from, next) => {
    if (to.matched.some(k => k.meta.authorize)) {
      try {
        if (!authService.isAuthed()) {
          return next('/login')
        }
      } catch (ex) {
        return next(false)
      }
    }

    next()
  })

  // start quasar
  Quasar.start(async () => {
    await authService.sync()
    resolve(router)
  })
}
