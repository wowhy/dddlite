<template>
  <el-menu class="sidebar-menu" router
      :default-active="activeMenu"
      :default-openeds="openeds"
      :collapse="collapse">
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
</template>

<script>
import auth from '~/services/auth'

export default {
  name: 'layout-sidebar',
  props: {
    collapse: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      menus: [],
      user: {},
      activeMenu: this.$route.path,
      openeds: []
    }
  },
  watch: {
    $route(val) {
      this.activeMenu = val.path
    }
  },
  async created() {
    this.menus = await auth.getMenus()

    let path = this.$route.path

    this.menus.some(menu => {
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

.sidebar-menu {
  height: 'calc(100vh - %s)' % $header-height;
}

.sidebar-menu:not(.el-menu--collapse) {
  width: $sidebar-width;
}

.sidebar-menu.el-menu--collapse {
  color: $nerutal;
}
</style>