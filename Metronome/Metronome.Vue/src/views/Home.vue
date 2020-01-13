<template>
  <div class="home">
    <hzNav v-bind:isAuth="false"></hzNav>
    <img alt="Vue logo" src="../assets/logo.png">
  </div>
</template>

<script>
import hzNav from '@/components/hz-navigation.vue'

export default {
  name: 'home',
  components: {
    hzNav
  }
}
</script>
