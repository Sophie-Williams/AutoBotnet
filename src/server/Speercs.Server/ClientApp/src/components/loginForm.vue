<template>
  <v-card class="grey lighten-4 elevation-0">
    <v-card-text>
      <v-container fluid>
        <v-layout row>
          <v-flex xs10 offset-xs1 lg6 offset-lg3>
            <template v-if="mode === 'login'">
              <v-text-field name="username-input" label="Username" v-model="un"></v-text-field>
              <v-text-field name="password-input" label="Password" type="password" v-model="pw"></v-text-field>
              <v-subheader class="red--text" v-if="err !== null">{{ err }}</v-subheader>
              <div class="center">
                <v-btn @click.native="proceed_login" :loading="pending" :disabled="!canProceed" primary raised ripple>Login</v-btn>
              </div>
            </template>
            <template v-else-if="mode === 'register'">
              <v-text-field name="username-input" label="Username" v-model="un"></v-text-field>
              <v-text-field name="password-input" label="Password" type="password" v-model="pw"></v-text-field>
              <v-text-field v-if="meta.invite" name="ikey-input" label="Invite Key" type="password" v-model="ikey"></v-text-field>
              <v-subheader class="red--text" v-if="err !== null">{{ err }}</v-subheader>
              <div class="center">
                <v-btn @click.native="proceed_register" :loading="pending" :disabled="!canProceed" primary raised ripple>Register</v-btn>
              </div>
            </template>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  name: 'loginForm',
  props: ['mode'],
  data() {
    return {
      un: null,
      pw: null,
      ikey: null,
      err: null,
      pending: false,
      meta: {
        invite: false
      }
    }
  },
  computed: {
    canProceed() { return !this.pending }
  },
  methods: {
    ensure_api() {
      return this.$store.dispatch('ensure_api', window.location.origin)
    },
    get_meta() {
      this.pending = true
      this.ensure_api()
        .then(() => {
          this.$store.dispatch('get_meta')
            .then((meta) => {
              console.log('meta', meta)
              // update meta stuff
              this.meta.invite = meta.inviteRequired
              this.pending = false
            })
            .catch(() => this.pending = false)
        })
    },
    attempt_relogin() {
      this.pending = true
      this.ensure_api()
        .then(() => {
          this.$store.dispatch('attempt_reauthenticate')
            .then(() => {
              console.log('reauthenticated successfully')
              // proceed
              this.onProceed()
            })
            .catch(() => this.pending = false)
        })
    },
    proceed_login() {
      this.pending = true
      let b = {
        un: this.un,
        pw: this.pw
      }
      this.ensure_api()
        .then(() => {
          this.$store.dispatch('authenticate', b)
            .then(() => {
              console.log('login successful')
              // proceed
              this.onProceed()
            })
            .catch((e) => {
              this.pending = false
              this.err = 'invalid credentials'
              console.log('login failure', e)
            })
        })
    },
    proceed_register() {
      this.pending = true
      let b = {
        un: this.un,
        pw: this.pw,
        invite: this.ikey
      }
      this.ensure_api()
        .then((rs) => {
          this.$store.dispatch('register_account', b)
            .then(() => {
              console.log('registration successful')
              // proceed
              this.onProceed()
            })
            .catch((e) => {
              this.pending = false
              this.err = 'registration failed'
              if (e.response) {
                this.err += `: ${e.response.data}`
              }
              console.log('registration failure', e)
            })
        })
    },
    onProceed() {
      if (this.$route.query.r) {
        this.$router.push(this.$route.query.r)
      } else {
        this.$router.push('/d')
      }
    }
  },
  mounted() {
    this.attempt_relogin()
    if (this.mode == 'register') {
      // get meta
      this.get_meta()
    }
  }
}
</script>