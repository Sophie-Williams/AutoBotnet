<template>
  <v-app class="elevation-1" top-toolbar left-sidebar>
    <v-toolbar>
      <v-toolbar-side-icon class="hidden-lg-and-up" @click.native.stop="sidebar_v = !sidebar_vsss" />
      <v-toolbar-title>{{ appName }}</v-toolbar-title>
    </v-toolbar>
    <main>
      <v-sidebar v-model="sidebar_v" height="auto">
        <v-list dense>
          <template v-for="(item, i) in sidebar">
            <v-subheader v-if="item.header" v-text="item.header" />
            <v-divider v-else-if="item.divider" light />
            <v-list-item v-else>
              <template v-if="!item.autoHide || (item.unauthRequired && !loggedIn) || (item.authRequired && loggedIn)">
                <v-list-tile :router="true" :href="item.router" ripple>
                  <v-list-tile-avatar>
                    <v-icon>{{ item.avatar }}</v-icon>
                  </v-list-tile-avatar>
                  <v-list-tile-title v-text="item.title" />    
                </v-list-tile>
              </template>
            </v-list-item>
          </template>
        </v-list>
      </v-sidebar>
      <v-content>
        <v-container fluid>
          <div class="content-container">
            <router-view></router-view>
          </div>
        </v-container>
      </v-content>
    </main>
  </v-app>
</template>

<script>
  export default {
    data () {
      return {
        sidebar_v: null,
        sidebar: [
          { header: 'Quick Links' },
          // {
          //   title: 'Parent',
          //   items: [
          //     { title: 'Child' },
          //     { title: 'Child' },
          //     { title: 'Child' }
          //   ]
          // },
          { 
            title: 'Home',
            avatar: 'home',
            router: '/'
          },
          { 
            title: 'Dashboard',
            avatar: 'dashboard',
            router: '/dashboard',
            authRequired: true,
            autoHide: true
          },
          {
            title: 'Account',
            avatar: 'person',
            authRequired: true,
            autoHide: true
          },
          {
            title: 'Login',
            avatar: 'person',
            router: '/login',
            unauthRequired: true,
            autoHide: true
          },
          {
            title: 'Register',
            avatar: 'create',
            router: '/register',
            unauthRequired: true,
            autoHide: true
          },
          {
            title: 'Logout',
            avatar: 'exit_to_app',
            router: '/logout',
            authRequired: true,
            autoHide: true
          },
          { divider: true },
          { header: 'Support' },
          { title: 'Report Bugs', avatar: 'error' }
        ]
      }
    },
    computed: {
      appName: function() {
        return this.$store.state.data.appName
      },
      loggedIn: function() {
        return this.$store.state.auth.loggedIn
      }
    },
    methods: {}
  }
</script>

<style lang="stylus">
  @import '../node_modules/vuetify/src/stylus/main'
  @import './css/main.css'
</style>