<template>
  <el-header class="layout-header">
      <div class="layout-header-icon">
        <h2>VUEADMIN</h2>
      </div>
      <nav class="layout-header-navbar">
        <div class="pull-left layout-header-toggle" @click="toggleSidebar">
          <i class="fa fa-align-justify"></i>
        </div>
        <div class="pull-right" style="padding: 0 10px;">
          <span>你好，{{user.name}} !</span>
          <el-button type="text" size="normal" @click="logout">登出系统</el-button>
        </div>
      </nav>
    </el-header>
</template>

<script>
import auth from '~/services/auth'
export default {
  name: 'layout-header',
  data() {
    return {
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
      this.$confirm('是否登出系统?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        auth.logout()
        this.$router.push('login')
      }).catch(() => { })
    },
    toggleSidebar() {
      this.$emit('toggle')
    }
  },
  created() {
    this.user = auth.getUserInfo()
  }
}
</script>

<style lang="stylus">
@import '~variables';

.layout {
  &-header {
    height: $header-height;
    overflow: hidden;
    background-color: $dark;
    display: flex;
    flex: column;
    color: $neutral;
    line-height: $header-height;

    &-icon {
      width: $sidebar-width;
      text-align: center;
      overflow: hidden;
      transition: 0.3s width;

      h2 {
        margin: 0;
      }
    }

    &-navbar {
      flex: 1;
    }

    &-toggle {
      line-height: $header-height;
      cursor: pointer;
    }
  }
}

.mini-layout {
  .layout {
    &-header-icon {
      width: 25px;

      h2 {
        display: none;
      }
    }
  }
}
</style>


