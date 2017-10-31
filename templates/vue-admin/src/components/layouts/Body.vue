<template>
  <el-container class="layout" :class="{ 'mini-layout': mini }">
    <layout-header @toggle="onToggle"></layout-header>
    <el-container class="layout-main">
      <layout-sidebar :collapse="mini"></layout-sidebar>
      <el-main>
        <layout-tabs ref="tabs"></layout-tabs>
        <router-view class="layout-content"></router-view>
      </el-main>
    </el-container>
  </el-container>
</template>

<script>
import Sidebar from './Sidebar'
import Tabs from './Tabs'
import Header from './Header'

export default {
  components: {
    [Header.name]: Header,
    [Sidebar.name]: Sidebar,
    [Tabs.name]: Tabs
  },
  data() {
    return {
      mini: false
    }
  },
  methods: {
    onToggle() {
      this.mini = !this.mini
    }
  }
}
</script>

<style lang="stylus">
.layout {
  height: 100vh;
  display: flex;
  flex-direction: column;

  .el-header {
    padding: 0;
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

  .el-main {
    padding: 0;
  }
}
</style>
