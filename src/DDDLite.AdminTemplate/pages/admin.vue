<template>
  <div class="layout" :class="{ 'layout-hide-menu': !showMenu }">
    <Row type="flex" style="height: 100%;">
      <Col class="layout-menu-left">
      <Menu ref="mainMenu" theme="dark" width="auto" :active-name="currentMenu.code" @on-select="onMenuSelect">
        <div class="layout-logo-left">
          <div class="layout-logo"></div>
        </div>
        <MenuItem :name="homeMenu.code">
        <Icon type="ios-home" :size="iconSize"></Icon>
        <span class="layout-text">首 页</span>
        </MenuItem>
        <MenuGroup v-for="menu in menus" :key="menu.code" :title="menu.text">
          <MenuItem v-for="subMenu in menu.children" :key="subMenu.code" :name="subMenu.code">
          <Icon type="ios-navigate" :size="iconSize"></Icon>
          <span class="layout-text">{{subMenu.text}}</span>
          </MenuItem>
        </MenuGroup>
      </Menu>
      </Col>
      <Col class="layout-main">
      <div class="layout-header">
        <div class="layout-header-toggle">
          <Button type="text" @click="toggleMenu">
            <Icon type="navicon" size="32"></Icon>
          </Button>
        </div>
        <div class="layout-header-menus">
          <span>欢迎, {{user.fullname}}!</span>
          <a href="#">帮助中心</a> |
          <a href="#">安全中心</a> |
          <a style="color:#ed3f14;" href="#">退出</a>
        </div>
      </div>
      <div class="layout-breadcrumb">
        <Breadcrumb>
          <BreadcrumbItem :href="homeMenu.url">首 页</BreadcrumbItem>
          <template v-if="currentMenu.code !== homeMenu.code">
            <BreadcrumbItem>{{currentGroup.text}}</BreadcrumbItem>
            <BreadcrumbItem :href="currentMenu.url">{{currentMenu.text}}</BreadcrumbItem>
          </template>
        </Breadcrumb>
      </div>
      <div class="layout-content">
        <div class="layout-content-main">
          <nuxt-child />
        </div>
      </div>
      <div class="layout-copy">
        2011-2016 &copy; TalkingData
      </div>
      </Col>
    </Row>
  </div>
</template>
<script>
import { mapGetters, mapActions } from 'vuex'

const findMenu = function(menus, compare) {
  for (let i = 0; i < menus.length; i++) {
    let menu = menus[i]
    if (compare(menu)) {
      return {
        menu,
        parent: null
      }
    }

    let length = menu.children ? menu.children.length : 0
    for (let j = 0; j < length; j++) {
      let subMenu = menu.children[j]
      if (compare(subMenu)) {
        return {
          menu: subMenu,
          parent: menu
        }
      }
    }
  }

  return null
}

export default {
  data() {
    const homeMenu = {
      code: '#admin',
      url: '/admin'
    }
    return {
      currentMenu: homeMenu,
      currentGroup: null,
      homeMenu: homeMenu
    }
  },
  computed: {
    ...mapGetters('admin', ['menus', 'showMenu', 'user']),
    iconSize() {
      return this.showMenu ? 14 : 24
    }
  },
  methods: {
    onMenuSelect(code) {
      if (code === this.homeMenu.code) {
        this.$router.push(this.homeMenu.url)
        this.currentMenu = this.homeMenu
        return
      }

      let find = findMenu(this.menus, k => k.code === code)
      if (!find || find.menu === this.currentMenu)
        return

      if (find.menu && find.menu.url) {
        this.$router.push(find.menu.url)
      }
    },
    refreshMenu() {
      let menus = [this.homeMenu, ...this.menus]
      let find = findMenu(menus, m => m.url === this.$route.path)
      if (find) {
        this.$nextTick(() => {
          this.currentMenu = find.menu
          this.currentGroup = find.parent
          this.$refs.mainMenu.updateActiveName()
        })
      }
    },
    ...mapActions('admin', ['toggleMenu'])
  },
  watch: {
    $route(val) {
      this.refreshMenu()
    }
  },
  created() {
    this.refreshMenu()
  }
}
</script>

<style lang="less">
.layout {
  border: 1px solid #d7dde4;
  background: #f5f7f9;
  position: relative;
  height: 100%;

  .layout-menu-left {
    width: 220px;
    background: #464c5b;
  }

  .layout-main {
    width: calc(100% -220px);
  }
}

.layout-menu-left {
  .layout-logo-left {
    height: 50px;
    padding-top: 10px !important;
  }

  .layout-logo {
    width: 90%;
    height: 30px;
    background: #5b6270;
    border-radius: 3px;
    margin-left: auto;
    margin-right: auto;
  }
}

.layout-main {
  .layout-breadcrumb {
    padding: 10px 15px 0;
  }

  .layout-content {
    min-height: 200px;
    margin: 15px 15px 0 15px;
    overflow: hidden;
    background: #fff;
    border-radius: 4px;
    height: calc(100% -120px);
  }

  .layout-content-main {
    padding: 10px;
  }

  .layout-copy {
    height: 30px;
    line-height: 30px;
    text-align: center;
    color: #9ea7b4;
  }

  .layout-header {
    height: 45px;
    background: #495060;
    padding: 10px 0;
    overflow: hidden;
    display: flex;
    align-items: center;

    .layout-header-menus {
      flex: 1;
      margin-right: 15px;
      text-align: right;
    }

    .layout-header-menus {
      color: #9ba7b5;
      a,
      span {
        margin-left: 5px;
      }
    }

    .layout-header-toggle i {
      color: #fff;
    }
  }
}

.layout-hide-menu {
  .layout-menu-left {
    width: 65px;
  }
  .layout-main {
    width: calc(100% -65px);
  }
  .layout-text,
  .ivu-menu-item-group-title {
    display: none;
  }
}

.ivu-col {
  transition: width .2s ease-in-out;
}
</style>