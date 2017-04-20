<template>
  <div>
    <div class="center">
      <h2>Register</h2>
      <v-row row>
        <v-col xs12 lg6 offset-lg3>
          <login-form mode="register"></login-form>
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
  }
}
</script>