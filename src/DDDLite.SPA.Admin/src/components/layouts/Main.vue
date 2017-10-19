<template>
  <div>
    <router-view></router-view>
  </div>
</template>

<script>
import auth from '~/services/auth'

export default {
  data() {
    return {
      menus: [],
      user: {}
    }
  },
  computed: {
    title() {
      return this.$route.meta.title
    }
  },
  methods: {
    logout() {
    }
  },
  async created() {
    this.menus = await auth.getMenus()
    this.user = auth.getUserInfo()
  }
}
</script>

<style lang="stylus">
</style>
