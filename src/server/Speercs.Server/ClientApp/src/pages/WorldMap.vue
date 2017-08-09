<template>
  <div>
    <div v-if="ready">
      Hello. Welcome to the world map, {{ username }}!
    </div>
    <div class="center" v-else>
      <v-progress-circular indeterminate v-bind:size="80" class="primary--text"></v-progress-circular>
      <h5>Loading Map</h5>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      ready: false,
      room: null
    }
  },
  mounted: function () {
    this.$store.dispatch('get_map_room', {
      api: this.$store.getters.api,
      x: 0, y: 0 // TODO: make this actually fetch the right room
    })
      .then((roomData) => {
        console.log('fetched room data', roomData)
        this.room = roomData
        this.ready = true
      })
      .catch((e) => console.log('error fetching map room', e))
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