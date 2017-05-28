<template>
  <div>
    <div class="center intro">
      <h3>Code Editor</h3>
    </div>
    <div>
      <v-layout row>
        <v-flex xs12 lg8>
          <div class="editor">
            <codemirror v-model="code" :options="editorOptions"></codemirror>
          </div>
        </v-flex>
        <v-flex xs12 lg4>
          <div class="actions">
            <v-btn primary raised ripple>Deploy Code</v-btn>
          </div>
        </v-flex>
      </v-layout>
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
      }
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