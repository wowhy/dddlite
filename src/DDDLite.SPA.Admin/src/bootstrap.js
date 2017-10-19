// === DEFAULT / CUSTOM STYLE ===
// WARNING! always comment out ONE of the two require() calls below.
// 1. use next line to activate CUSTOM STYLE (./src/themes)
// require(`./themes/app.${__THEME}.styl`)
// 2. or, use next line to activate DEFAULT QUASAR STYLE
require(`quasar/dist/quasar.${__THEME}.css`)

// Uncomment the following lines if you need IE11/Edge support
// require(`quasar/dist/quasar.ie`)
// require(`quasar/dist/quasar.ie.${__THEME}.css`)

import 'vue-easytable/libs/themes-base/index.css'

import Vue from 'vue'

import Quasar, * as All from 'quasar'
import { VTable, VPagination } from 'vue-easytable'

if (__THEME === 'mat') {
  require('quasar-extras/roboto-font')
}
import 'quasar-extras/material-icons'
import 'quasar-extras/ionicons'
// import 'quasar-extras/fontawesome'
import 'quasar-extras/animate'

import $http from './http'
import authService from './services/auth'

Vue.config.productionTip = false
Vue.use(Quasar, {
  components: All,
  directives: All
}) // Install Quasar Framework

Vue.component(VTable.name, VTable)
Vue.component(VPagination.name, VPagination)

Vue.$http = Vue.prototype.$http = $http

export default function() {
  return new Promise((resolve, reject) => {
    Quasar.start(async () => {
      await authService.sync()
      resolve()
    })
  })
}
