<template>
  <div class="page-login">
    <div class="login-box">
      <div class="login-title">
        <h3>Vue ADMIN</h3>
      </div>
      <div class="login-form">
        <el-form :model="loginForm" :rules="loginRule" ref="loginForm">
          <el-form-item prop="username">
            <el-input placeholder="用户名" type="text" v-model="loginForm.username" auto-complete="off"></el-input>
          </el-form-item>
          <el-form-item prop="pass">
            <el-input placeholder="密码" type="password" v-model="loginForm.pass" auto-complete="off"></el-input>
          </el-form-item>
          <el-form-item prop="remember" class="remember">
            <el-checkbox v-model="loginForm.remember">记住密码</el-checkbox>
          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="handleSubmit">登录</el-button>
          </el-form-item>
        </el-form>
      </div>
    </div>
  </div>
</template>

<script>
import auth from '~/services/auth'

export default {
  name: 'login',
  data () {
    return {
      loginRule: {
        username: [
          {
            require: true,
            message: '请填写用户名'
          }
        ],
        password: [
          { require: true, message: '请填写密码' }
        ]
      },
      loginForm: {
        username: '',
        pass: '',
        remember: true
      }
    }
  },
  methods: {
    handleSubmit () {
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          auth.login(this.loginForm.username, this.loginForm.pass).then(() => {
            this.$router.push('index')
          })
        }
      })
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style lang="stylus" scoped>
@import '~variables'

html, body, .page-login {
  min-height: 100vh;
}
.page-login {
  background-color: $dark;
  padding-top: 50px;
}
.login-title {
  color: #2a323c;
  text-align: center;
  padding: 20px 0 0;
}
.login-box {
  margin: 0 auto;
  max-width: 440px;
  border-radius: 5px;
  background-color: #fff;
}
.login-form {
  padding: 20px 30px;
  border: 0;

  button {
    display: block;
    width: 100%;
  }

  .remember {
    // margin: 0;
  }
}
</style>