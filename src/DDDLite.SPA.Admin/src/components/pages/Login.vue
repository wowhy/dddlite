<template>
  <div></div>
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
</style>
