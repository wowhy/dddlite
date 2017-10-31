import Vue from 'vue'
import NProgress from 'vue-nprogress'

import router from './router'
import bootstrap from './bootstrap'

bootstrap().then(() => {
  const nprogress = new NProgress({ parent: '.nprogress-container' })

  /* eslint-disable no-new */
  new Vue({
    el: '#app',
    router,
    nprogress,
    render: h => h(require('./App').default)
  })
})
