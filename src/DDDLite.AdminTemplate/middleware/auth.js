export default function ({ store, error }) {
  if (!store.state.authUser) {
    error({
      message: '',
      statusCode: 401
    })
  }
}
