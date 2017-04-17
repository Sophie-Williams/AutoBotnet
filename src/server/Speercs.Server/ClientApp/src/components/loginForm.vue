<template>
  <v-card class="grey lighten-4 elevation-0">
    <v-card-text>
      <v-container fluid>
        <v-row row>
        <v-col xs10 offset-xs1 lg6 offset-lg3>
          <v-text-field
            name="username-input"
            label="Username"
          ></v-text-field>
          <v-text-field
            name="password-input"
            label="Password"
            type="password"
          ></v-text-field>
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

    }
  }
}
</script>