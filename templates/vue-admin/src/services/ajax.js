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

export function ajaxPatch(url, data, config = {}) {
  return $http.patch(url, data, {
    ...config
  })
}

export function ajaxDelete(url, params, config = {}) {
  return $http.delete(url, {
    ...config,
    params
  })
}

export class RestfulService {
  constructor(url) {
    this.baseUrl = url
  }

  create(resource) {
    return ajaxPost(this.baseUrl, resource)
  }

  update(id, resource, version) {
    let data = []
    generatePatch(resource, data, '/')
    return ajaxPatch(this.baseUrl + '/' + id, data, {
      headers: {
        'If-Match': version
      }
    })
  }

  remove(id, version) {
    return ajaxDelete(this.baseUrl + '/' + id, {}, {
      headers: {
        'If-Match': version
      }
    })
  }

  get(id, includes) {
    return ajaxGet(this.baseUrl + '/' + id, {
      $includes: includes
    })
  }

  find(page, limit, filter, order, includes) {
    return ajaxGet(this.baseUrl, {
      $top: limit,
      $skip: (page - 1) * limit,
      $filter: filter,
      $orderBy: order,
      $count: true,
      $includes: includes
    })
  }
}

function generatePatch(obj, op, parent) {
  Object.keys(obj).forEach(key => {
    if (typeof obj[key] === 'object') {
      generatePatch(obj[key], op, parent + key + '/')
    } else {
      op.push({
        op: 'replace',
        path: parent + key,
        value: obj[key]
      })
    }
  })
}
