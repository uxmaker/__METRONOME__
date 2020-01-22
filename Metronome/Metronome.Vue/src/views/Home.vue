<template>
  <div class="home">
    <navigation v-bind:alwaysLinks="links.anonyme" v-bind:authLinks="links.auth"></navigation>
    <img alt="Vue logo" src="../assets/logo.png">
    <div class="sections">
      <div class="home-section home-section-1">
        <h3>(Le parisien) Pour les boutiques des gares et métros, la grève à la RATP et la SNCF a été «catastrophique»</h3>
        <img src="http://www.leparisien.fr/resizer/5Kb1htzVh6AZy2aI-AGFcgkj1pg=/932x582/arc-anglerfish-eu-central-1-prod-leparisien.s3.amazonaws.com/public/QFOENPVY4NZAA6YZVUQOYQQBBA.jpg"/>
        
        <p><small>Le 21 janvier 2020 à 14h29</small>Sous terre, dans les gares et stations de métro d’Ile-de-France, des boutiques ont subi de plein fouet les conséquences de 45 jours de grève dans les transports. Avec une baisse importante de leur chiffre d’affaires.</p>
        <a href="http://www.leparisien.fr/info-paris-ile-de-france-oise/transports/pour-les-boutiques-des-gares-et-metros-la-greve-a-la-ratp-et-la-sncf-a-ete-catastrophique-21-01-2020-8240799.php" target="_blank">Lire la suite ...</a>
      </div>
      <div class="home-section">
        <h3>Grève RATP et SNCF : les prévisions de trafic pour ce dimanche 19 janvier en Île-de-France</h3>
        <img src="http://www.leparisien.fr/resizer/mtigEDN1G5TsR0H5ED2SD38VGWE=/932x582/arc-anglerfish-eu-central-1-prod-leparisien.s3.amazonaws.com/public/YN66S6CJU5UHDB7QX4UJBUWRKE.jpg"/>
        
        <p><small>Le 18 janvier 2020 à 17h33</small>Un trafic proche de la normale. L'amélioration, « très nette » d'après les prévisions de la RATP, se poursuit sur le réseau de transports en commun francilien. Ce dimanche, comme la veille, la circulation sera ainsi bien plus fluide que ce qui a été observé le week-end précédent à la RATP et à la SNCF.</p>
        <a href="http://www.leparisien.fr/info-paris-ile-de-france-oise/transports/greve-ratp-et-sncf-les-previsions-de-trafic-pour-ce-dimanche-19-janvier-en-ile-de-france-18-01-2020-8238972.php" target="_blank">Lire la suite ...</a>
      </div>
      <div class="home-section">
        <h3>Grève RATP et SNCF : les prévisions de trafic pour ce lundi 20 janvier en Île-de-France</h3>
        <img src="http://www.leparisien.fr/resizer/sIZInxf5XVMlx6BoNpnoyfhre2c=/932x582/arc-anglerfish-eu-central-1-prod-leparisien.s3.amazonaws.com/public/AYZOBK5LIDMNDRGQNSKF5ITJYQ.jpg"/>

        <p><small>Le 19 janvier 2020 à 17h41</small> Comme un lundi (presque) normal. Après plusieurs jours où une amélioration « très nette » du trafic a été constatée, l'annonce faite samedi par l'Unsa-RATP, première organisation professionnelle dans l'entreprise, appelant à suspendre dès lundi la grève illimitée, vient évidemment confirmer la tendance.</p>
        <a href="http://www.leparisien.fr/info-paris-ile-de-france-oise/transports/greve-ratp-et-sncf-les-previsions-de-trafic-pour-ce-lundi-20-janvier-en-ile-de-france-19-01-2020-8239443.php" target="_blank">Lire la suite ...</a>
      </div>
    </div>
  </div>
</template>

<style>

.sections
{
  display:grid;
  grid-template-columns: 1fr 1fr;
  padding:16px;
  grid-gap:16px;
}

.home-section
{
  background-color:white;
  padding:16px;
  border-radius:3px;
  display:grid;
  grid-template-areas:  "title title"
                        "image para"
                        "image lirelasuite ";
  grid-template-columns: 200px 1fr;
  grid-gap:8px;
                       
}

.home-section small
{
  font-style: italic;
  display:block;
}

.home-section h3
{
  margin-bottom:8px;
  text-decoration: underline;
  grid-area: title;
}

.home-section img
{
  grid-area: image;
  width:100%;
  border-radius:3px;
}

.home-section
{
  text-align:justify;
  font-size:12px;
}

.home-section a
{
  grid-area: lirelasuite;
  display:block;
  color:orange;
  font-weight:bold;
  background-color:#101010;
  margin-top:6px;
  padding:4px 8px;
  border-radius:3px;
  height:24px;
  line-height:24px;
  margin-left:auto;
}

    /* RESPONSIVE */

    @media screen and (max-width: 720px)
    {
      
    .sections
    {
      display:grid;
      grid-template-columns: 1fr;
    }
.home-section
{
  background-color:white;
  padding:16px;
  border-radius:3px;
  display:grid;
  grid-template-areas:  "title"
                        "image"
                        "para"
                        "lirelasuite";
  grid-template-columns: 1fr;
  grid-gap:8px;
                       
}

    }
</style>
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
