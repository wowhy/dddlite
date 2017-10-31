<template>
  <el-tabs class="layout-tabs" v-model="activeTab" @tab-remove="removeTab" @tab-click="onTabActive">
    <el-tab-pane label="主页" name="index" :closable="false"></el-tab-pane>
    <el-tab-pane
      v-for="(item, index) in tabs"
      :key="item.name"
      :label="item.title"
      :name="item.name"
      :closable="true"
    >
    </el-tab-pane>
  </el-tabs>
</template>

<script>
export default {
  name: 'layout-tabs',
  data() {
    return {
      tabs: [],
      activeTab: 'index'
    }
  },
  methods: {
    addTab(tab) {
      this.tabs.push(tab)
    },
    removeTab(targetName) {
      let tabs = this.tabs
      let activeName = this.activeTab
      if (activeName === targetName) {
        tabs.forEach((tab, index) => {
          if (tab.name === targetName) {
            let nextTab = tabs[index + 1] || tabs[index - 1]
            if (nextTab) {
              activeName = nextTab.name
            } else {
              activeName = 'index'
            }
          }
        })

        this.$router.push(activeName)
      }

      this.activeTab = activeName
      this.tabs = tabs.filter(tab => tab.name !== targetName)
    },
    onTabActive(tab) {
      this.$router.push({
        name: tab.name
      })
    }
  },
  watch: {
    '$route'(to) {
      if (to.name !== 'index') {
        let tab = this.tabs.find(k => k.name === to.name)
        if (!tab) {
          this.addTab({
            name: to.name,
            title: to.meta.title
          })
        }
      }

      this.activeTab = to.name
    }
  },
  created() {
    if (this.$route.name !== 'index') {
      this.tabs.push({
        name: this.$route.name,
        title: this.$route.meta.title
      })
      this.activeTab = this.$route.name
    }
  }
}
</script>

<style lang="stylus">
@import '~variables'
.layout-tabs
  padding: 5px 5px 0 5px
  .el-tabs__content
    display: none
  .el-tabs__header
    margin-bottom: 0
</style>