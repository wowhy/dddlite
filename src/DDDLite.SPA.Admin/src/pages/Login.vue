<template>
  <div>
    用户名: <input type="text" v-model="username">
    密码: <input type="password" v-model="password">

    <button @click="login">登录</button>
  </div>
</template>

<script>
import authService from '../services/auth'
export default {
  data() {
    return {
      username: '',
      password: ''
    }
  },
  methods: {
    login() {
      return authService.login(this.username, this.password)
    }
  }
}
</script>

<style lang="stylus" scoped>
</style>
