import Vue from 'vue'
import VueRouter from 'vue-router'

import authService from './services/auth'

Vue.use(VueRouter)

function load(component) {
  // '@' is aliased to src/components
  return () => System.import(`@/${component}.vue`)
}

const router = new VueRouter({
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
    { name: 'login', path: '/login', component: load('pages/Login') },
    {
      path: '/',
      component: load('layouts/Main'),
      redirect: '/index',
      meta: {
        authorize: true
      },
      children: [
        { name: 'index', path: '/index', component: load('pages/Dashboard'), meta: { title: '主页' } },
        { name: 'demo', path: '/demo', component: load('pages/Demo'), meta: { title: '测试' } },
        { name: 'products', path: '/products', component: load('pages/Products'), meta: { title: '产品管理' } }
      ]
    },

    // Always leave this last one
    { path: '*', component: load('pages/Error404') } // Not found
  ]
})

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

export default router
