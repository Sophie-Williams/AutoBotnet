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
            <v-list-group v-if="item.items">
              <v-list-item slot="item">
                <v-list-tile ripple>
                  <v-list-tile-title v-text="item.title" />
                  <v-list-tile-action>
                    <v-icon>keyboard_arrow_down</v-icon>
                  </v-list-tile-action>
                </v-list-tile>
              </v-list-item>
              <v-list-item v-for="(subItem,i) in item.items" :key="i">
                <v-list-tile ripple>
                  <v-list-tile-title v-text="subItem.title" />    
                </v-list-tile>
              </v-list-item>
            </v-list-group>
            <v-subheader v-else-if="item.header" v-text="item.header" />
            <v-divider v-else-if="item.divider" light />
            <v-list-item v-else>
              <v-list-tile ripple>
                <v-list-tile-avatar>
                  <v-icon>{{ item.avatar }}</v-icon>
                </v-list-tile-avatar>
                <v-list-tile-title v-text="item.title" />    
              </v-list-tile>
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
          { title: 'Dashboard', avatar: 'home', authRequired: true },
          { title: 'Account', avatar: 'person', authRequired: true },
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