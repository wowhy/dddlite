<template>
  <q-layout ref="layout" view="lHh Lpr fff" :left-class="{'bg-grey-2': true}" reveal>
    <q-toolbar slot="header">
      <q-btn flat @click="$refs.layout.toggleLeft()">
        <q-icon name="menu" />
      </q-btn>

      <q-toolbar-title>
        {{title}}
      </q-toolbar-title>

      <span>您好, {{user.name}} </span>
      <q-btn flat @click="logout">
        <q-icon name="power_settings_new" />
      </q-btn>
    </q-toolbar>

    <q-scroll-area class="fit" slot="left">
      <div class="row flex-center bg-white full-width" style="height: 100px;">
        <img src="~/assets/quasar-logo-full.svg" style="height: 75px; width 75px;" />
        <div style="margin-left: 15px">
          Quasar v{{ $q.version }}
        </div>
      </div>
      <q-list no-border>
        <q-side-link item to="/index" exact replace>
          <q-item-side icon="home" />
          <q-item-main label="主页" />
          <q-item-side right icon="chevron_right" />
        </q-side-link>
        <q-item-separator />

        <template v-for="menu in menus">
          <q-list-header :key="menu.code">
            {{ menu.text }}
          </q-list-header>
          <q-side-link v-for="item in menu.children" :key="item.code" item :to="item.url" exact>
            <q-item-side :icon="item.icon" />
            <q-item-main :label="item.text" />
            <q-item-side right icon="chevron_right" />
          </q-side-link>
        </template>

        <q-list-header>Essential Links</q-list-header>
        <q-item @click="launch('http://quasar-framework.org')">
          <q-item-side icon="school" />
          <q-item-main label="Docs" sublabel="quasar-framework.org" />
        </q-item>
        <q-item @click="launch('http://forum.quasar-framework.org')">
          <q-item-side icon="record_voice_over" />
          <q-item-main label="Forum" sublabel="forum.quasar-framework.org" />
        </q-item>
        <q-item @click="launch('https://gitter.im/quasarframework/Lobby')">
          <q-item-side icon="chat" />
          <q-item-main label="Gitter Channel" sublabel="Quasar Lobby" />
        </q-item>
        <q-item @click="launch('https://twitter.com/quasarframework')">
          <q-item-side icon="rss feed" />
          <q-item-main label="Twitter" sublabel="@quasarframework" />
        </q-item>
      </q-list>
    </q-scroll-area>

    <router-view />
  </q-layout>
</template>

<script>
import {
  openURL,
  Dialog
} from 'quasar'
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
    launch(url) {
      openURL(url)
    },
    logout() {
      let that = this
      Dialog.create({
        title: '登出',
        message: '您是否确定登出？',
        buttons: [
          {
            label: '否'
          },
          {
            label: '是',
            handler() {
              auth.logout()
              that.$router.replace('/login')
            }
          }
        ]
      })
    }
  },
  async created() {
    this.menus = await auth.getMenus()
    this.user = auth.getUserInfo()
  }
}
</script>

<style lang="stylus">
.logo-container {
  width: 255px;
  height: 242px;
  perspective: 800px;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translateX(-50%) translateY(-50%);
}

.logo {
  position: absolute;
  transform-style: preserve-3d;
}
</style>
