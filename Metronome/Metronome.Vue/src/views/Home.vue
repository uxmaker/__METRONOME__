<template>
  <div class="home">
    <navigation v-bind:isAuth="true" v-bind:alwaysLinks="links.anonyme" v-bind:authLinks="links.auth"></navigation>
    <img alt="Vue logo" src="../assets/logo.png">
  </div>
</template>

<script>
import navigation from '@/components/navigation.vue'

export default {
  name: 'home',
  components: {
    navigation
  },
  data()
  {
    return {
      links : {
        anonyme : [ 
          [ "Accueil", "/", require("../assets/template/essential/png/home-2.png"), "default" ],
          [ "Info Traffic", "/", null, "default" ]
        ],
        auth : [
          [ "Plan", "/about", require("../assets/template/essential/png/map-2.png"), "default" ]
        ]
      }
    }
  }
}
</script>
