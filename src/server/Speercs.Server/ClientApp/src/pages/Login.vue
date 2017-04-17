<template>
  <div>
    <div class="center">
      <h2>Login</h2>
      <v-row row>
        <v-col xs12 lg6 offset-lg3>
          <login-form></login-form>
        </v-col xs8>
      </v-row>
    </div>
  </div>
</template>

<script>

import loginForm from '../components/loginForm.vue'

export default {
  data () {
    return {
    }
  },
  components: {
    loginForm
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    }
  }
}
</script>