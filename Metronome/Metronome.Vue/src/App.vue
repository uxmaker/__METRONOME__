<template>
  <div id="app">
    <AppNavigation v-bind:items="this.test"/>
    <router-view/>
  </div>
</template>

<script scoped>
import AppNavigation from '@/components/Navigation.vue'

export default {
  name: 'app',
  data(){
    return {
      test : [ ['Accueil','/'] ]
    }
  },
  components: {
    AppNavigation
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
