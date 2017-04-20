<template>
  <div>
    Hello. Welcome the code editor, {{ username }}!
    <codemirror v-model="code" :options="editorOptions"></codemirror>
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
        theme: 'paraiso-dark',
        lineNumbers: true,
        line: true,
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