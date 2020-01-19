<template>
    <div id="navigation">
        <div id="hz-navigation">
            <div id="hz-navigation-left">
                <button v-on:click="toggleMenu" class="burger-icon"><i class="icon"></i></button>
                <router-link to="/" class="title">metronome</router-link>
            </div>
            <div id="hz-navigation-right" class="navigation-signs">
                <div></div>
                <button class="signin aths" @click="login('Base')"><div class="before"></div><div class="content">se connecter</div><div class="after"></div></button>
                <button class="signup aths" @click="register('Base-Register')"><div class="before"></div><div class="content">s'enregistrer</div><div class="after"></div></button>
                <router-link to="/" class="profile mbms"><i class="icon"></i><div class="content">Profile</div></router-link>
                <router-link to="/about" class="map mbms"><i class="icon"></i><div class="content">Plan</div></router-link>
                <router-link to="/Logout" class="profile mbms"><i class="icon"></i><div class="content">Logout</div></router-link>
            </div>
        </div>
        <div id="ve-navigation" class="">
            <div id="ve-navigation-left" class="ve-navigation-left">
                <div id="ve-navigation-left-header">
                    <button v-on:click="toggleMenu" class="burger-icon"><i class="icon"></i></button>
                    <router-link to="/" class="title">metronome</router-link>
                </div>
                    <div v-for='link in alwaysLinks' v-bind:class="link[3]">
                        <router-link v-bind:to="link[1]"><img v-if="link[2] != null" v-bind:src="link[2]" class="icon"><div v-else></div><div class="content">{{ link[0] }}</div></router-link>
                    </div>
                <div v-if="this.authenticated==false">
                    <div v-for='link in unAuthLinks' v-bind:class="link[3]">
                        <router-link v-bind:to="link[1]"><img v-if="link[2] != null" v-bind:src="link[2]" class="icon"><div v-else></div><div class="content">{{ link[0] }}</div></router-link>
                    </div>
                </div>
                <div v-else>
                    <div v-for='link in authLinks' v-bind:class="link[3]">
                        <router-link v-bind:to="link[1]"><img v-if="link[2] != null" v-bind:src="link[2]" class="icon"><div v-else></div><div class="content">{{ link[0] }}</div></router-link>
                    </div>
                </div>
                <div class="signs-responsive" v-if="this.authenticated==false">
                    <button class="signin"><div class="content">se connecter</div></button>
                    <button class="signup"><div class="content">s'enregistrer</div></button>
                </div>
                <div  class="signs-responsive" v-else>
                    <button to="/" class="profile mbms"><i class="icon"></i><div class="content">Profile</div></button>
                    <button to="/Logout" class="profile mbms"><i class="icon"></i><div class="content">Logout</div></button>
                </div>
            </div>
            <button v-on:click="toggleMenu" id="ve-navigation-right"></button>
        </div>
    </div>
</template>

<style scoped>
@import url('../css/navigation.css');
</style>

<script>
import AuthService from '../services/AuthService'
import { timeout } from 'q';

export default {
    name: 'horizontal-navigation',
    props: {
        authLinks : {
            type : Array,
            required : false
        },
        unAuthLinks : {
            type : Array,
            required : false
        },
        alwaysLinks : {
            type : Array,
            required : false
        }
    },
    data(){
        return {
            authenticated : false,
            endpoint: null
        }
    },
    computed: {
        auth: () => AuthService,
    },
    methods :{
        toggleMenu(){
            let veNav = document.getElementById("ve-navigation");
            let veNavLeft = document.getElementById("ve-navigation-left");
            let veNavRight = document.getElementById("ve-navigation-right");

            this.isActive = (this.isActive ? false : true);

            if(this.isActive)
            {
                veNav.style.display = "grid";
                setTimeout(() => { veNavLeft.classList.add("isActive"); veNavRight.classList.add("ActiveRightToo"); }, 16, veNavLeft, veNavRight)
            }
            else
            {
                veNavLeft.classList.remove("isActive");
                veNavRight.classList.remove("ActiveRightToo");
                setTimeout(() => { veNav.style.display = "none" }, 300, veNav)
            }
        },
        login(provider) {
            AuthService.login(provider);
        },
        register(provider) {
            AuthService.register(provider)
        },
        toggleAuth() {
            
            let authelements = document.getElementsByClassName("aths");
            let memberelements = document.getElementsByClassName("mbms");

            document.getElementById("ve-navigation").style.display = "none";
            if(this.authenticated)
            {
                authelements.forEach(element => {
                    element.style.display = "none";
                });
                memberelements.forEach(element => {
                    element.style.display = "grid";
                });
            }
            else
            {
                memberelements.forEach(element => {
                    element.style.display = "none";
                });
                authelements.forEach(element => {
                    element.style.display = "grid";
                });
            }
        },
        onAuthenticated() {
            this.authenticated = true;
            this.toggleAuth();
            console.log("AUTHENTICATED")
        }
    },
    mounted()
    {
        this.auth.registerAuthenticatedCallback(() => this.onAuthenticated());
        this.auth.registerAuthenticatedCallback(() => this.toggleAuth());
        console.log("IS CONNECTED " + this.auth.isConnected)
        if(this.auth.isConnected)
        {
            this.authenticated = true;
        }
        this.toggleAuth();
    },
    beforeDestroy() {
        this.auth.removeAuthenticatedCallback(() => this.onAuthenticated());
    },
}
</script>