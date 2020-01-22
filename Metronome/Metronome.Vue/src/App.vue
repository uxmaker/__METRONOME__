<template>
  <div id="app"><router-view/></div>
</template>

<script scoped>
import AuthService from './services/AuthService'
import {state} from './state'

export default {
  name: 'app',
    data() {
        return {
            state
        }
    },

    computed: {
        auth: () => AuthService,
        
        isLoading() {
            return this.state.isLoading;
        }
    }
}
</script>

<style>
@import './css/default.css';
</style>
