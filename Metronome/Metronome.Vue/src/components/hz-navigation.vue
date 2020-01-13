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
                <router-link to="/" class="profile mbms"><i class="icon"></i><div class="content">PROFILE</div></router-link>
                <router-link to="/about" class="map mbms"><i class="icon"></i><div class="content">LA CARTE</div></router-link>
            </div>
        </div>
        <div id="ve-navigation" class="">
            <div id="ve-navigation-left" class="ve-navigation-left">
                <div id="ve-navigation-left-header">
                    <button v-on:click="toggleMenu" class="burger-icon"><i class="icon"></i></button>
                    <router-link to="/" class="title">metronome</router-link>
                </div>
                <div v-for='link in alwaysLinks' v-bind:class="link[3]">
                    <router-link v-bind:to="link[1]"><img v-if="link[2] != null" v-bind:src="link[2]" class="icon"><div class="content">{{ link[0] }}</div></router-link>
                </div>
                <div v-for='link in unAuthLinks' v-bind:class="link[3]">
                    <router-link v-bind:to="link[1]">{{ link[0] }}</router-link>
                </div>
                <div v-for='link in authLinks' v-bind:class="link[3]">
                    <router-link v-bind:to="link[1]">{{ link[0] }}</router-link>
                </div>

            </div>
            <button v-on:click="toggleMenu" id="ve-navigation-right"></button>
        </div>
    </div>
</template>

<style scoped>

    /* NAVIGATION */

    /* HORIZONTAL NAVIGATION */

    #hz-navigation
    {
        position:fixed;
        top:0px;
        left:0px;
        width:100%;
        display:grid;
        grid-template-columns:auto 1fr;
        background-color:#FFFFFF;
    }
    
    #hz-navigation-left
    {
        display:grid;
        grid-template-columns: 40px 1fr;
        background-position: right;
        background-repeat: no-repeat;
        background-size:auto 100%;
        padding:8px 16px;
    }

    #hz-navigation-left .burger-icon
    {
        width:40px;
        height:40px;
        padding:8px 8px;
        cursor: pointer;
        border-radius:3px 0 0 3px;
        background:transparent;
    }

    #hz-navigation-left .burger-icon .icon
    {
        display:block;
        width:80%;
        height:80%;
        margin:auto;
        background-image:url("../assets/template/burger-icon.png");
        background-size:cover;
    }

    #hz-navigation-left .title
    {
        color:black;
        font-family:sans-serif;
        text-transform: uppercase;
        font-size:15px;
        line-height:24px;
        letter-spacing: 2px;
        padding: 8px 8px;
        text-decoration: none;
        transition:300ms;
        margin-left:2px;
        border-radius:0 3px 3px 0;
    }

    #hz-navigation-left .title:hover { color:#001025AA; }

    #hz-navigation-right
    {
        display:grid;
        padding: 8px 16px;
        grid-template-columns: 1fr auto auto;
    }

    #hz-navigation-right button
    {
        display:grid;
        grid-template-columns: 40px auto 40px;
        transition: all 0.3s ease-in-out;
        background:transparent;
        cursor: pointer;
    }

    #hz-navigation-right button .content
    {
        color: #FFF;
        border: none;
        font-size:11px;
        line-height:40px;
        padding:0px 8px;
        text-transform: uppercase;
        text-align: center;
        width:100px;
    }

    #hz-navigation-right button:hover
    {
        opacity:0.75;
    }

    #hz-navigation-right button .before { background-size:cover; background-color:transparent; }
    #hz-navigation-right button .after { background-size:cover; background-color:transparent; z-index:1; }

    #hz-navigation-right .signin { margin-right:-37px; grid-template-columns: 8px auto 40px; }
    #hz-navigation-right .signin .before { background-color:#007bff;  border-radius: 3px 0 0 3px; }
    #hz-navigation-right .signin .content { background-color:#007bff; }
    #hz-navigation-right .signin .after { background-image:url("../assets/template/square-primary-right.png"); }

    #hz-navigation-right .signup { grid-template-columns: 40px auto 8px; }
    #hz-navigation-right .signup .before { background-image: url("../assets/template/square-success-left.png"); }
    #hz-navigation-right .signup .content { background-color: #28a745; }
    #hz-navigation-right .signup .after { background-color: #28a745; border-radius:0 3px 3px 0; }


    #hz-navigation-right .map
    {
        border-radius:3px;
        background-color:#17a2b8;
        grid-template-columns:40px 1fr;
        cursor: pointer;
        line-height:40px;
        padding:0 8px;
        border-radius:3px;
    }

    #hz-navigation-right .map .icon
    {
        background-image:url("../assets/template/essential/png/map-2.png");
        display:block;
        width:50%;
        height:50%;
        margin:auto;
        background-size:cover;
        background-position:bottom;
    }

    #hz-navigation-right .profile
    {
        grid-template-columns:40px 1fr;
        cursor: pointer;
        line-height:40px;
        padding:0 8px;
        background-color:#6c757d;
        border-radius:3px;
    }

    #hz-navigation-right .profile .icon
    {
        display:block;
        width:50%;
        height:50%;
        margin:auto;
        background-image:url("../assets/template/essential/png/user-3.png");
        background-size:cover;
        background-position:bottom;
    }
    
    #hz-navigation-right .mbms
    {
        margin-left:8px;
        display:grid;
        transition: all 0.3s ease-in-out;
        text-decoration:none;
    }

    #hz-navigation-right .mbms .content
    {
        width:auto;
        color: #FFF;
        border: none;
        font-size:11px;
        line-height:40px;
        text-transform: uppercase;
        text-align: center;
        padding-right:8px;
    }

    /* HORIZONTAL NAVIGATION RESPONSIVE */

    @media screen and (max-width: 720px)
    {
        #navigation #hz-navigation-right button { display:none; }
    }
    
    /* VERTICAL NAVIGATION */

    #ve-navigation
    {
        display:grid;
        grid-template-columns:auto 1fr;
        grid-template-rows: 1fr;
        position:fixed;
        left: 0px;
        top: 0px;
        width:100%;
        height:100vh;
        z-index:1;
    }

    #ve-navigation div
    {
        -webkit-transition: all 0.3s ease;  
        -moz-transition: all 0.3s ease;  
        -o-transition: all 0.3s ease;  
        -ms-transition: all 0.3s ease;  
        transition: all 0.3s ease;
    }


    #ve-navigation-left
    {
        background-color:#dbdbdb;
    }

    #ve-navigation .ve-navigation-left
    {
        width:300px;
        margin-left:-300px;
    }

    #ve-navigation .isActive
    {
        margin-left:0px;
    }

    #ve-navigation-right
    {
        animation: none;
        transition: 300ms;
        background-color:#00000000;
    }

    #ve-navigation .ActiveRightToo
    {
        background-color:#00000066;
    }

    /* HEADER */
    #ve-navigation-left-header
    {
        display:grid;
        grid-template-columns: 40px 1fr;
        background-position: right;
        background-repeat: no-repeat;
        background-size:auto 100%;
        padding:8px 16px;
        background-color:#FFF;

    }
    #ve-navigation-left-header .burger-icon
    {
        width:40px;
        height:40px;
        padding:8px 8px;
        cursor: pointer;
        border-radius:3px 0 0 3px;
        background:transparent;
    }

    #ve-navigation-left-header .burger-icon .icon
    {
        display:block;
        width:80%;
        height:80%;
        margin:auto;
        background-image:url("../assets/template/burger-icon.png");
        background-size:cover;
    }

    #ve-navigation-left-header  .title
    {
        color:black;
        font-family:sans-serif;
        text-transform: uppercase;
        font-size:15px;
        line-height:24px;
        letter-spacing: 2px;
        padding: 8px 8px;
        text-decoration: none;
        transition:300ms;
        margin-left:2px;
    }

    /* VERTICAL NAVIGATION DEFAULT LINKS */

    #ve-navigation-left .default a
    {
        display:grid;
        grid-template-columns: 40px 1fr;
        padding:8px 16px;
        text-decoration: none;
        transition:300ms;
    }

    #ve-navigation-left .default a:hover
    {
        background-color:#00000020;
    }

    #ve-navigation-left .default .icon
    {
        width:24px;
        padding:8px;
    }

    #ve-navigation-left .default .content{
        color:black;
        font-size:15px;
        line-height:24px;
        letter-spacing: 1px;
        padding: 8px 8px;
        text-decoration: none;
        transition:300ms;
        margin-left:2px;
    }

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
                setTimeout(() => { veNavLeft.classList.add("isActive"); veNavRight.classList.add("ActiveRightToo"); }, 15, veNavLeft, veNavRight)
            }
            else
            {
                veNavLeft.classList.remove("isActive");
                veNavRight.classList.remove("ActiveRightToo");
                setTimeout(() => { veNav.style.display = "none" }, 315, veNav)
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