<template>
  <div>
    <div v-if="ready">
      <v-layout row wrap>
        <v-flex xs12 md6>
          <v-card>
            <v-card-row class="primary">
              <v-card-title>
                <span class="white--text">Server Status</span>
              </v-card-title>
            </v-card-row>
            <v-list two-line>
              <template v-for="(item, ix) in server_status">
                <div v-bind:key="ix">
                  <v-subheader v-if="item.header" v-text="item.header" />
                  <v-divider v-else-if="item.divider" v-bind:inset="item.inset" />
                  <v-list-item v-else v-bind:key="item.name">
                    <v-list-tile>
                      <v-list-tile-content>
                        <v-list-tile-title>
                          {{ item.name }}
                        </v-list-tile-title>
                        <v-list-tile-sub-title>
                          {{ item.value }} {{ item.unit }}
                        </v-list-tile-sub-title>
                      </v-list-tile-content>
                    </v-list-tile>
                  </v-list-item>
                </div>
              </template>
            </v-list>
          </v-card>
        </v-flex>
        <v-flex xs12 md6>
          <v-card>
            <v-card-row class="secondary">
              <v-card-title>
                <span class="white--text">Resource Overview</span>
              </v-card-title>
            </v-card-row>
            <v-list two-line>
              <template v-for="(item, ix) in resources">
                <v-subheader v-if="item.header" v-text="item.header" />
                <v-divider v-else-if="item.divider" v-bind:inset="item.inset" />
                <v-list-item v-else v-bind:key="item.name">
                  <v-list-tile>
                    <v-list-tile-content>
                      <v-list-tile-title>
                        {{ item.name }}
                      </v-list-tile-title>
                      <v-list-tile-sub-title>
                        {{ item.amount }} {{ item.unit }}
                      </v-list-tile-sub-title>
                    </v-list-tile-content>
                  </v-list-tile>
                </v-list-item>
              </template>
            </v-list>
          </v-card>
        </v-flex>
      </v-layout>
    </div>
    <div class="center" v-else>
      <v-progress-circular indeterminate v-bind:size="80" class="primary--text"></v-progress-circular>
      <h5>Loading Dashboard</h5>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      ready: false,
      server_status: [
        { header: 'Server Health' }
      ],
      resources: [
        { header: 'Total Resources' },
        { name: 'NRG', amount: 63574, unit: 'NG' },
        { divider: true, inset: true },
        { name: 'Metal', amount: 4506, unit: 'kg' },
        { divider: true, inset: true },
        { name: 'Oil', amount: 5648, unit: 'L' }
      ]
    }
  },
  mounted: function () {
    // get info
    this.$store.dispatch('get_server_info', {
        api: this.$store.getters.api
    })
      .then((info) => {
        this.server_status.push({
          name: 'Tick Rate',
          value: `${info.tickrate} ms`
        })
        this.server_status.push({ divider: true, inset: true })
        this.server_status.push({
          name: 'Player Count',
          value: `${info.userCount}`
        })
        this.server_status.push({ divider: true, inset: true })
        this.server_status.push({
          name: 'Map Size',
          value: `${info.mapSize} rooms`
        })
        this.ready = true
      })
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    },
    username: function () {
      return this.$store.getters.auth_data.un;
    }
  }
}
</script>