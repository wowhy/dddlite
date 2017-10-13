import { RestfulService } from './ajax'

class ProductService extends RestfulService {
  constructor() {
    super('/api/v1/products')
  }
}

export default new ProductService()
