<template>
  <div>
    <div class="center intro">
      <h3>Code Editor</h3>
    </div>
    <div v-if="ready">
      <v-layout row>
        <v-flex xs12 lg8>
          <div class="editor">
            <codemirror v-model="code" :options="editorOptions"></codemirror>
          </div>
        </v-flex>
        <v-flex xs12 lg4>
          <div class="actions">
            <v-btn primary raised ripple
              @click.native="deployCode"
              :loading="deploying"
              :disabled="deploying"
            >
              Deploy Code
              <v-icon right light>send</v-icon>
            </v-btn>
          </div>
          <v-btn info raised ripple
              @click.native="reloadCode"
              :loading="reloading"
              :disabled="reloading"
            >
              Request Reload
              <v-icon right light>refresh</v-icon>
            </v-btn>
        </v-flex>
      </v-layout>
    </div>
    <div class="center" v-else>
      <v-progress-circular indeterminate v-bind:size="60" class="primary--text"></v-progress-circular>
      <h5>Loading Data</h5>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
      code: 'function loop() {\n}',
      editorOptions: {
        // codemirror options
        tabSize: 2,
        mode: 'text/javascript',
        theme: 'material',
        lineNumbers: true,
        line: true,
        height: '100%',
        // viewportMargin: Infinity,
        // keyMap: "sublime",
        // extraKeys: { "Ctrl": "autocomplete" },
        // foldGutter: true,
        // gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
        styleSelectedText: true,
        highlightSelectionMatches: { showToken: /\w/, annotateScrollbar: true },
      },
      ready: false,
      loadError: false,
      deploying: false,
      reloading: false
    }
  },
  computed: {
    appName: function () {
      return this.$store.state.data.appName
    },
    username: function () {
      return this.$store.getters.auth_data.un;
    }
  },
  methods: {
    deployCode () {
      this.deploying = true
      this.$store.dispatch('deploy_user_code', {
        api: this.$store.getters.api,
        source: this.code
      })
        .then(() => {
          this.deploying = false
        })
        .catch(() => {
          this.deploying = false
        })
    },
    reloadCode () {
      this.reloading = true
      this.$store.dispatch('reload_user_code', {
        api: this.$store.getters.api
      })
        .then(() => {
          this.reloading = false
        })
        .catch(() => {
          this.reloading = false
        })
    }
  },
  mounted () {
    // fetch code
    this.$store.dispatch('get_user_code', {
      api: this.$store.getters.api
    })
      .then((usrc) => {
        this.code = usrc
        this.ready = true
      })

  }
}
</script>

<style>
  .editor {
    height: 60%;
    padding-bottom: 40%;
    position: relative;
  }
  .CodeMirror-scroll {
    overflow-y: hidden;
    overflow-x: auto;
    min-height: 100%;
  }
</style>