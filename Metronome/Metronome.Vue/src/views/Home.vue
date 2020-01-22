<template>
  <div class="home">
    <navigation v-bind:alwaysLinks="links.anonyme" v-bind:authLinks="links.auth"></navigation>
    <img alt="Vue logo" src="../assets/logo.png">
  </div>
</template>

<script>
import AuthService from '../services/AuthService'
import navigation from '@/components/navigation.vue'

export default {
  name: 'home',
  components: {
    navigation
  },
  computed: {
    auth: () => AuthService,
  },
  methods:{
  },
  data()
  {
    return {
      links : {
        anonyme : [ 
          [ "Accueil", "/", require("../assets/template/essential/png/home-2.png"), "default" ]
        ],
        auth : [
          [ "Plan", "/about", require("../assets/template/essential/png/map-2.png"), "default" ]
        ]
      }
    }
  }
}
</script>
