<template>
  <div class="bg-dark fullscreen row flex-center">
    <q-card class="bg-white login-form">
      <q-card-title>
        登录
      </q-card-title>
      <q-card-main>
        <q-field icon="person">
          <q-input v-model="username" placeholder="请输入用户名" class="full-width" />
        </q-field>
        <q-field icon="vpn_key">
          <q-input v-model="password" type="password" placeholder="请输入密码" class="full-width" />
        </q-field>
        <q-field>
          <q-checkbox v-model="longsave" label="自动登录" />
        </q-field>
      </q-card-main>
      <q-card-actions>
        <q-btn color="primary" class="full-width" loader @click="login">
          登录
          <span slot="loading">
            <q-spinner-hourglass class="on-left" />
            正在登录...
          </span>
        </q-btn>
      </q-card-actions>
    </q-card>
  </div>
</template>

<script>
import authService from '~/services/auth'
export default {
  data() {
    return {
      username: '',
      password: '',
      longsave: false
    }
  },
  methods: {
    async login(e, done) {
      try {
        await authService.login(this.username, this.password, this.longsave).then(() => {
          this.$router.replace('/')
        })
      } catch (ex) {
        // nothing
      }

      done()
    }
  }
}
</script>

<style lang="stylus" scoped>
.login-form {
  width: 440px;
}
</style>
