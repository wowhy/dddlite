export default function ({ store, error }) {
  if (!store.getters.admin.user) {
    error({
      message: '',
      statusCode: 401
    })
  }
}
