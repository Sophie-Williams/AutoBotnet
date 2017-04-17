<template>
  <div>
    <div class="center">
      <h2>Login</h2>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    }
  }
}
</script>