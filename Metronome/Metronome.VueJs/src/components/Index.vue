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
// import('../core/Candy/candy.js'); ( #TD-549287364 )

export default {
    data() {
        return {
          endpoint: null,
          window: {
            width: 0,
            height: 0
          },
          elelines: null,
          colors: ["#B6BD00", "#003CA6", "#FFCD00", "#E19BDF", "#704B1C", "#6ECA97", "#C9910D", "#FF7E2E", "#CF009E", "#6EC4E8", "#FA9ABA", "#007852"], 
         
          line: []
        }
    },

    mounted() {
      
        AuthService.registerAuthenticatedCallback(() => this.onAuthenticated());
        window.addEventListener('resize', this.handleResize)

      this.handleResize();
      //recupere l'élement qui contient les lignes
      this.elelines = document.getElementById("svg-dynamic-background");

      // on stock le this car setinterval est appelé par le systeme
      let me = this;
      
      // ce qui s'effectue à chaque tour
      // VVVVV
      window.setInterval(function(){
        
        if (Math.random() >= 0.96 && me.line.length < 11) {
          me.createline();
        }

        //vide les lignes
        me.elelines.innerHTML = null;

        //recreer les lignes
        me.line.forEach   ( element =>
        {
          if (element.axis == "x") 
          {
            if (element.position >= 0 && element.position <= me.window.width + 10) 
            {
              element.position = element.position + element.pas;
            }
            else
            {
              element.coloropacity = (parseInt(element.coloropacity, 16) - 16).toString(16);
            }
            if (element.side == true) 
            {
              me.elelines.innerHTML = me.elelines.innerHTML + '<line id="" x1="'+ element.position+'" y1="'+ element.otherbase +'" x2="0" y2="'+ element.otherbase +'" style="stroke:' + element.color + element.coloropacity + ';stroke-width:12" /><circle cx="'+ element.position +'" cy="'+ element.otherbase +'" r="10" stroke="black" stroke-width="5" fill="white" />';
            } 
            else 
            {
              me.elelines.innerHTML = me.elelines.innerHTML + '<line id="" x1="'+ (me.window.width - element.position) +'" y1="'+ element.otherbase +'" x2="'+ me.window.width +'" y2="'+ element.otherbase +'" style="stroke:' + element.color + element.coloropacity + ';stroke-width:12" /><circle cx="'+ (me.window.width - element.position) +'" cy="'+ element.otherbase +'" r="10" stroke="black" stroke-width="5" fill="white" />';
            }
  
          }
          if (element.axis == "y") 
          {
            if (element.position >= 0 && element.position <= me.window.height + 10) 
            {
              element.position = element.position + element.pas;
            }
            else {
              element.coloropacity = (parseInt(element.coloropacity, 16) - 16).toString(16);
            }
            if (element.side == true) {
              me.elelines.innerHTML = me.elelines.innerHTML + '<line id="" x1="'+ element.otherbase +'" y1="'+ element.position +'" x2="'+ element.otherbase +'" y2="0" style="stroke:' + element.color + element.coloropacity +';stroke-width:12" /><circle cy="'+ element.position +'" cx="'+ element.otherbase +'" r="10" stroke="black" stroke-width="5" fill="white" />';
            }
            else {
              me.elelines.innerHTML = me.elelines.innerHTML + '<line id="" x1="'+ element.otherbase +'" y1="'+ (me.window.height - element.position) +'" x2="'+ element.otherbase +'" y2="'+ me.window.height +'" style="stroke:' + element.color + element.coloropacity + ';stroke-width:12" /><circle cy="'+ (me.window.height - element.position) +'" cx="'+ element.otherbase +'" r="10" stroke="black" stroke-width="5" fill="white" />';
            }
          }
        });
        
        //supprime la ligne si elle n'est plus visible
        for (let index = 0; index < me.line.length -1 ; index++) {
          const element = me.line[index];
          if (element.coloropacity <= "0" ) {
            me.line.splice(index, 1);
          }
        }
      }, 50);
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
        this.createline();
    },

    methods: {
        login(provider) {
            AuthService.login(provider);
        },

        onAuthenticated() {
            //this.$router.replace('/');
        },

        //s'occupe de recupérer la taille de la fenetre
        handleResize() 
        {
         this.window.width = window.innerWidth;
         this.window.height = window.innerHeight;
        },
        //crée une ligne aléatoirement
        createline() {

        if (Math.random() >= 0.5) {
            var axisv = "x"; 
            var basev = Math.floor(Math.random() * Math.floor(window.innerHeight - 50));
        }
        else {
            var axisv = "y";
            var basev = Math.floor(Math.random() * Math.floor(window.innerWidth- 50));
        }
        
        if (Math.random() >= 0.5) {
            var sidev = true;
        } else {
            var sidev = false;
        }

        // Line :
        // axis  = sur x ou y 
        // otherbase = la valeur de x si la ligne avance sur y || y si la ligne avance sur x
        // position = extremité de la ligne qui avance
        // pas = nb de pixels avancé par la ligne à chaque tour 
        // color = la couleur de la ligne select dans le array 
        // coloropacity = cest dans le nom quoi

        this.line.push({ axis : axisv, otherbase: basev, position: 0, pas: (Math.floor(Math.random() * Math.floor(10 - 2)) + 2) , color: this.colors[Math.floor(Math.random() * Math.floor(this.colors.length))] , coloropacity : "FF", side: sidev});
        }
    }
}
</script>

<style scoped>@import '../css/views/index.css';

.treat {
    --scale-x: 0;
    --scale-y: 0;
    pointer-events: none;
    display: block;
    position: absolute;
    top: 0;
    left: calc(50% - .5rem);
    border-radius: 50%;
    width: 1em;
    height: 1em;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 5vmin;
    -webkit-transform: translate(calc( var(--x) * 1px ), calc( var(--y) * 1px )) translate(-50%, -50%);
            transform: translate(calc( var(--x) * 1px ), calc( var(--y) * 1px )) translate(-50%, -50%);
    pointer-events: none;
    -webkit-animation: treat-enter 0.1s ease-in backwards, treat-exit 300ms linear calc( (var(--lifetime, 3000) * 1ms) - 300ms) forwards;
            animation: treat-enter 0.1s ease-in backwards, treat-exit 300ms linear calc( (var(--lifetime, 3000) * 1ms) - 300ms) forwards;
  }

  @-webkit-keyframes treat-enter {
    from {
      opacity: 0;
    }
  }
  @keyframes treat-enter {
    from {
      opacity: 0;
    }
  }
  @-webkit-keyframes treat-exit {
    to {
      opacity: 0;
    }
  }
  @keyframes treat-exit {
    to {
      opacity: 0;
    }
  }

  .treat .inner {
    -webkit-animation: inner-rotate .6s linear infinite;
            animation: inner-rotate .6s linear infinite;
    -webkit-transform: rotate(calc(-1turn * var(--direction) ));
            transform: rotate(calc(-1turn * var(--direction) ));
  }

  @-webkit-keyframes inner-rotate {
    to {
      -webkit-transform: none;
              transform: none;
    }
  }
  @keyframes inner-rotate {
    to {
      -webkit-transform: none;
              transform: none;
    }
  }
</style>