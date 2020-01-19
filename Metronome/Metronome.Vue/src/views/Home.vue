<template>
  <div class="home">
    <hzNav v-bind:isAuth="false" v-bind:alwaysLinks="links.anonyme"></hzNav>
    <img alt="Vue logo" src="../assets/logo.png">
  </div>
</template>

<script>
import hzNav from '@/components/hz-navigation.vue'

export default {
  name: 'home',
  components: {
    hzNav
  },
  data()
  {
    return {
      links : {
        auth : {},
        unauth : {},
        anonyme : [ 
          [ "Accueil", "/", require("../assets/template/essential/png/home-2.png"), "default" ],
          [ "Plan", "/about", require("../assets/template/essential/png/map-2.png"), "default" ],
          [ "Alerts", "/", null, "default" ],
        ]
      }
    }
  }
}
</script>
