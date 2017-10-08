import Vue from 'vue'

export function ajaxGet(url, params, config = {}) {
  return Vue.$http.get(url, {
    ...config,
    params
  })
}

export function ajaxPost(url, data, config = {}) {
  return Vue.$http.post(url, {
    ...config,
    data
  })
}

export function ajaxPut(url, data, config = {}) {
  return Vue.$http.put(url, {
    ...config,
    data
  })
}

export function ajaxDelete(url, config = {}) {
  return Vue.$http.delete(url, {
    ...config
  })
}
