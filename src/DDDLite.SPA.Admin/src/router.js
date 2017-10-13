import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

function load(component) {
  // '@' is aliased to src/components
  return () => System.import(`@/${component}.vue`)
}

export default new VueRouter({
  /*
   * NOTE! VueRouter "history" mode DOESN'T works for Cordova builds,
   * it is only to be used only for websites.
   *
   * If you decide to go with "history" mode, please also open /config/index.js
   * and set "build.publicPath" to something other than an empty string.
   * Example: '/' instead of current ''
   *
   * If switching back to default "hash" mode, don't forget to set the
   * build publicPath back to '' so Cordova builds work again.
   */

  mode: 'history',

  routes: [
    { path: '/login', component: load('pages/Login') },
    {
      path: '/',
      component: load('layouts/Main'),
      redirect: '/index',
      meta: {
        authorize: true
      },
      children: [
        { path: '/index', component: load('pages/Dashboard'), meta: { title: '主页' } }
      ]
    },

    // Always leave this last one
    { path: '*', component: load('pages/Error404') } // Not found
  ]
})
