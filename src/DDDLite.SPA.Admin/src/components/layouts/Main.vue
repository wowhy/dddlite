<template>
  <div class="layout" :class="{ 'mini-layout': !sidebar }">
    <header class="layout-header">
      <div class="layout-header-icon">
        <h2>VUEADMIN</h2>
      </div>
      <nav class="layout-header-navbar">
        <div class="pull-left layout-header-toggle" @click="toggleSidebar">
          <i class="fa fa-align-justify"></i>
        </div>
        <div class="pull-right" style="padding: 0 10px;">
          <span>你好，{{user.name}} !</span>
          <el-button type="danger" size="small">退出</el-button>
        </div>
      </nav>
    </header>
    <div class="layout-main">
      <el-menu class="sidebar-menu" router
           :default-active="activeMenu"
           :collapse="!sidebar">
          <el-menu-item index="index">
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

      <section class="layout-content">
        <router-view></router-view>
      </section>
    </div>
  </div>
</template>

<script>
import auth from '~/services/auth'

export default {
  data() {
    return {
      menus: [],
      user: {},
      sidebar: true,
      activeMenu: 'index'
    }
  },
  computed: {
    title() {
      return this.$route.meta.title
    }
  },
  methods: {
    logout() {
    },
    toggleSidebar() {
      this.sidebar = !this.sidebar
    }
  },
  async created() {
    this.menus = await auth.getMenus()
    this.user = auth.getUserInfo()
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

  // &-sidebar {
  //   width: $sidebar-width;
  //   height: 100vh;
  //   background-color: $dark;
  // }

  &-content {
    flex: 1;
    padding: 10px;
    overflow-y: scroll;
  }

  .sidebar-menu {
    height: 100vh;
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
      transition: 0.3s width;

      h2 {
        display: none;
      }
    }
  }
}
</style>
