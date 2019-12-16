<template>
    <div id="V-INDEX">
        
        <!-- HEADER -->

        <div id="V-INDEX-HEADER">
            <div id="V-INDEX-HEADER-LOGO"></div>
            <button  @click="login('Base')" id="V-INDEX-HEADER-LOGIN" class="button">LOGIN</button>
            <router-link to="/Logout" id="V-INDEX-HEADER-REGISTER" class="button">LOGOUT</router-link>
            <!--<router-link to="/register" id="V-INDEX-HEADER-REGISTER" class="button">REGISTER</router-link>
            <router-link to="/register" id="V-INDEX-HEADER-CONTACT" class="button">CONTACT</router-link>-->
            <router-link to="/Map" id="V-INDEX-HEADER-SUPPORT" class="button">MAP</router-link>
        </div>

        <!-- SECTION 1 -->

        <div id="V-INDEX-SECTION1">
          <svg id="svg-dynamic-background"></svg>
          <div id="V-INDEX-SECTION1-WLC">
            <h1>WELCOME BOYS !</h1>
            <div class="treat-wrapper">
            <!--<button class="treat-button">#FreeSpi !</button>-->
          </div>
          </div>
        </div>

        <!-- SECTION 2 -->

        <div id="V-INDEX-SECTION2">
        </div>

        <!-- SECTION 3 -->

        <div id="V-INDEX-SECTION3">

        </div>

        <div id="V-INDEX-FOOTER">
            <!--<img src="../../assets/logo-baw.png"/>-->
            <div class="copy">COPYRIGHT</div>
        </div>
    </div>
</template>

<script>
import AuthService from '../services/AuthService'
import Vue from 'vue'

export default {
    data() {
        return {
        }
    },

    mounted() {
      AuthService.registerAuthenticatedCallback(() => this.onAuthenticated());
    },

    beforeDestroy() {
        AuthService.removeAuthenticatedCallback(() => this.onAuthenticated());
    },

    destroyed()
    {
        window.removeEventListener('resize', this.handleResize)
    },

    created()
    {
    },

    methods: {
        login(provider) {
            AuthService.login(provider);
        },
        onAuthenticated()
        {
            this.$router.replace('/');
        }
    }
}
</script>

<style scoped>@import '../css/views/index.css';

</style>