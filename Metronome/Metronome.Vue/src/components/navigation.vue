<template>
    <div id="navigation">
        <div id="hz-navigation">
            <div id="hz-navigation-left">
                <button v-on:click="toggleMenu" class="burger-icon"><i class="icon"></i></button>
                <router-link to="/" class="title">metronome</router-link>
            </div>
            <div id="hz-navigation-right" class="navigation-signs">
                <div></div>
                <button class="signin aths"><div class="before"></div><div class="content">se connecter</div><div class="after"></div></button>
                <button class="signup aths"><div class="before"></div><div class="content">s'enregistrer</div><div class="after"></div></button>
                <router-link to="/" class="profile mbms"><i class="icon"></i><div class="content">Profile</div></router-link>
                <router-link to="/about" class="map mbms"><i class="icon"></i><div class="content">Plan</div></router-link>
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
                <div v-if="isAuth==false">
                    <div v-for='link in unAuthLinks' v-bind:class="link[3]">
                        <router-link v-bind:to="link[1]"><img v-if="link[2] != null" v-bind:src="link[2]" class="icon"><div v-else></div><div class="content">{{ link[0] }}</div></router-link>
                    </div>
                </div>
                <div v-else>
                    <div v-for='link in authLinks' v-bind:class="link[3]">
                        <router-link v-bind:to="link[1]"><img v-if="link[2] != null" v-bind:src="link[2]" class="icon"><div v-else></div><div class="content">{{ link[0] }}</div></router-link>
                    </div>
                </div>
                <div class="signs-responsive" v-if="isAuth==false">
                    <button class="signin"><div class="content">se connecter</div></button>
                    <button class="signup"><div class="content">s'enregistrer</div></button>
                </div>
                <div  class="signs-responsive" v-else>
                    <button to="/" class="profile mbms"><i class="icon"></i><div class="content">Profile</div></button>
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
import { timeout } from 'q';
export default {
    name: 'horizontal-navigation',
    props: {
        isAuth : {
            type : Boolean,
            required : true
        },
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
            isActive : false
        }
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
        }
    },
    updated()
    {
    },
    mounted()
    {
        let authelements = document.getElementsByClassName("aths");
        let memberelements = document.getElementsByClassName("mbms");
        document.getElementById("ve-navigation").style.display = "none";
        if(this.isAuth)
        {
            authelements.forEach(element => {
                element.style.display = "none";
            });
        }
        else
        {
            memberelements.forEach(element => {
                element.style.display = "none";
            });
        }
    }
}
</script>