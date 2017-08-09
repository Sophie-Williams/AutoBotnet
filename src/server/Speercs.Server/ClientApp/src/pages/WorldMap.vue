<template>
  <div>
    Hello. Welcome to the world map, {{ username }}!
  </div>
</template>

<script>
export default {
  data() {
    return {
    }
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