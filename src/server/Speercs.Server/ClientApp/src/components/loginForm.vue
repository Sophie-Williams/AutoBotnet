<template>
  <v-card class="grey lighten-4 elevation-0">
    <v-card-text>
      <v-container fluid>
        <v-row row>
          <v-col xs10 offset-xs1 lg6 offset-lg3>
            <v-text-field
              name="username-input"
              label="Username"
              v-model="un"
            ></v-text-field>
            <v-text-field
              name="password-input"
              label="Password"
              type="password"
              v-model="pw"
            ></v-text-field>
            <v-subheader v-if="err !== null">{{ err }}</v-subheader>
            <div class="center">
              <v-btn @click.native="proceed" :disabled="!canProceed" primary raised ripple>Login</v-btn>
            </div>
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  name: 'loginForm',
  data () {
    return {
      un: null,
      pw: null,
      err: null,
      canProceed: true
    }
  },
  methods: {
    proceed: function () {
      this.canProceed = false
      let b = {
        un: this.un,
        pw: this.pw
      }
      this.$store.dispatch('authenticate', b)
        .then(() => {
          // todo
          console.log('login successful')
        })
        .catch(() => {
          this.canProceed = true
        })
    }
  }
}
</script>