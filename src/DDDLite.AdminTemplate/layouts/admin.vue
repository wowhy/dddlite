<template>
  <div class="layout">
    <Row type="flex" style="height: 100%;">
      <Col class="layout-menu-left">
      <Menu active-name="1-2" theme="dark" width="auto" :open-names="['1']">
        <div class="layout-logo-left">
          <div class="layout-logo"></div>
        </div>
        <Submenu v-for="menu in $store.state.admin.menus" :key="menu.code">
          <template slot="title">
            <Icon type="ios-navigate"></Icon>
            {{menu.text}}
          </template>
        </Submenu>
        <!-- <Submenu name="1">
          <template slot="title">
            <Icon type="ios-navigate"></Icon>
            导航一
          </template>
          <MenuItem name="1-1">选项 1</MenuItem>
          <MenuItem name="1-2">选项 2</MenuItem>
          <MenuItem name="1-3">选项 3</MenuItem>
        </Submenu> -->
        <Submenu name="2">
          <template slot="title">
            <Icon type="ios-keypad"></Icon>
            导航二
          </template>
          <MenuItem name="2-1">选项 1</MenuItem>
          <MenuItem name="2-2">选项 2</MenuItem>
        </Submenu>
        <Submenu name="3">
          <template slot="title">
            <Icon type="ios-analytics"></Icon>
            导航三
          </template>
          <MenuItem name="3-1">选项 1</MenuItem>
          <MenuItem name="3-2">选项 2</MenuItem>
        </Submenu>
      </Menu>
      </Col>
      <Col class="layout-main">
      <div class="layout-header">
        <div class="layout-header-menus">
          <a href="#">注册登录</a> |
          <a href="#">帮助中心</a> |
          <a href="#">安全中心</a> |
          <a href="#">服务大厅</a>
        </div>
      </div>
      <div class="layout-breadcrumb">
        <Breadcrumb>
          <BreadcrumbItem href="#">首页</BreadcrumbItem>
          <BreadcrumbItem href="#">应用中心</BreadcrumbItem>
          <BreadcrumbItem>某应用</BreadcrumbItem>
        </Breadcrumb>
      </div>
      <div class="layout-content">
        <div class="layout-content-main">
          <nuxt />
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
export default {
}
</script>

<style lang="less">
.layout {
  border: 1px solid #d7dde4;
  background: #f5f7f9;
  position: relative;
  height: 100%;
}

.layout-menu-left {
  width: 220px;
  background: #464c5b;
}

.layout-main {
  width: calc(100% -220px);
}

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
  height: 40px;
  background: #464c5b;
  padding: 10px 0;
  overflow: hidden;

  .layout-header-menus {
    float: right;
    margin-right: 15px;
  }

  .layout-header-menus a {
    color: #9ba7b5;
  }
}

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
</style>