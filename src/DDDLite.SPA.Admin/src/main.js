import Vue from 'vue'
import router from './router'
import bootstrap from './bootstrap'

bootstrap().then(() => {
  /* eslint-disable no-new */
  new Vue({
    el: '#q-app',
    router,
    render: h => h(require('./App').default)
  })
})
