<template>
  <div>
    Hello. Welcome to {{ appName }}!
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
      return this.$root.appName
    }
  },
  mounted: function () {
    console.log(this.$root.appName)
  }
}
</script>