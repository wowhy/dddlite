<template>
  <el-container class="layout" :class="{ 'mini-layout': !sidebar }">
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
    <el-container class="layout-main">
      <el-menu class="sidebar-menu" router
          :default-active="activeMenu"
          :default-openeds="openeds"
          :collapse="!sidebar">
        <el-menu-item index="/index">
          <i class="fa fa-lg fa-tachometer"></i>
          <span slot="title">主页</span>
        </el-menu-item>

        <el-submenu v-for="menu in menus" :key="menu.code" :index="menu.code">
          <template slot="title">
            <i class="fa fa-lg" :class="'fa-' + menu.icon"></i>
            <span slot="title">{{menu.text}}</span>
          </template>
          <el-menu-item v-for="subMenu in menu.children" :key="subMenu.code" :index="subMenu.url">
            {{subMenu.text}}
          </el-menu-item>
        </el-submenu>
      </el-menu>

      <el-main class="layout-content">
        <router-view></router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import auth from '~/services/auth'

export default {
  data() {
    return {
      menus: [],
      user: {},
      sidebar: true,
      activeMenu: this.$route.path,
      openeds: []
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
      }).catch(() => {})
    },
    toggleSidebar() {
      this.sidebar = !this.sidebar
    }
  },
  watch: {
    $route(val) {
      this.activeMenu = val.path
    }
  },
  async created() {
    this.menus = await auth.getMenus()
    this.user = auth.getUserInfo()

    let path = this.$route.path

    this.menus.some((menu) => {
      if (menu.children && menu.children.length) {
        if (menu.children.some(k => k.url === path)) {
          this.openeds.push(menu.code)
          return true
        }
      }

      return false
    })
  }
}
</script>

<style lang="stylus">
@import '~variables';

$header-height = 60px;
$sidebar-width = 230px;
$sidebar-mini-width = 63px;

.layout {
  height: 100vh;
  display: flex;
  flex-direction: column;

  .el-header {
    padding: 0;
  }

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

  &-main {
    display: flex;
    flex-direction: row;
  }

  &-content {
    flex: 1;
    padding: 10px;
    overflow-y: scroll;
  }

  .sidebar-menu {
    height: "calc(100vh - %s)" % $header-height;
  }

  .sidebar-menu:not(.el-menu--collapse) {
    width: $sidebar-width;
  }

  .sidebar-menu.el-menu--collapse {
    color: $nerutal;
  }
}

.mini-layout {
  .layout {
    &-header-icon {
      width: $sidebar-mini-width;

      h2 {
        display: none;
      }
    }
  }
}
</style>
