import $http from '../http'

export function ajaxGet(url, params, config = {}) {
  return $http.get(url, {
    ...config,
    params
  })
}

export function ajaxPost(url, data, config = {}) {
  return $http.post(url, data, {
    ...config
  })
}

export function ajaxPut(url, data, config = {}) {
  return $http.put(url, data, {
    ...config
  })
}

export function ajaxDelete(url, params, config = {}) {
  return $http.delete(url, {
    ...config,
    params
  })
}
