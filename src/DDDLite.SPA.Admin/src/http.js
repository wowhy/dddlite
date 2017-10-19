import axios from 'axios'
import qs from 'qs'
import { Alert } from 'quasar'

const $http = axios.create()

$http.interceptors.request.use((request) => {
  if (request.data && request.headers['Content-Type'] === 'application/x-www-form-urlencoded') {
    request.data = qs.stringify(request.data)
  }
  return request
}, (error) => Promise.reject(error))

$http.interceptors.response.use((res) => res.data, ({ response }) => {
  let data = response.data
  let message = null

  if (typeof data === 'object') {
    if (data.error && data.error.message) {
      message = data.error.message
    } else if (data.error_description) {
      message = data.error_description
    }
  }

  if (response.status === 401) {
    message = '登录超时，请重新登录！'
    require('./router').default.push({ name: 'login' })
  } else if (response.status === 504) {
    message = '无法连接到服务器！'
  } else if (!message || response.status / 100 === 5) {
    message = '服务器发生未知错误！'
    console.log(data)
  }

  notify(message)

  return Promise.reject(data)
})

export default $http

function notify (msg) {
  let instance = Alert.create({
    enter: 'bounceInRight',
    leave: 'bounceOutRight',
    html: msg
  })

  setTimeout(() => instance.dismiss(), 5000)
}
