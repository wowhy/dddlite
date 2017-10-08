import Vue from 'vue'
import bootstrap from './bootstrap'

bootstrap().then((router) => {
  /* eslint-disable no-new */
  new Vue({
    el: '#q-app',
    router,
    render: h => h(require('./App').default)
  })
})
