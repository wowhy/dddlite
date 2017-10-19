import Vue from 'vue'
import VueAxios from 'vue-axios'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'

import router from './router'
import $http from './http'

import authService from './services/auth'

Vue.$http = Vue.prototype.$http = $http
Vue.$router = router

Vue.use(VueAxios, $http)
Vue.use(ElementUI)

export default function() {
  return authService.sync()
}
