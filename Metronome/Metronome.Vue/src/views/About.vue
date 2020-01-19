<template>
  <div class="about">
    <link href='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.css' rel='stylesheet' />
    <div id='map'></div>
    <nav id="clear-filter" class="clear-filter"></nav>
    <nav id="filter-group" class="filter-group"></nav>
    <div style="z-index: 1; position: absolute; top: 5px; right: 47px">
      <img src="/panel.png" v-on:click='alertevent' class="alertbutton" />
    </div>
     <autocomplete
      class='searchbar'
      :search="search"
      placeholder="Cherche une station"
      aria-label="Cherche une station"
      @submit="searchsubmit"
      auto-select
    ></autocomplete>
  </div>
</template>

<script>
import { getTimeStopAsync } from '../api/stationsApi'

export default {
  data() {
    return {
      map: null,
      mapboxgldata: null,
      popupinfo: null,
      geojsondata: null,
      origin: null,
      goal: null,
      stationsname: null,
      refreshtimer: null, 
      crd: null    
    }
  },

  mounted: async function() {
    const config = require('../appsettings.json');
    let mapboxgl = require('mapbox-gl/dist/mapbox-gl.js');
    mapboxgl.accessToken = config.token;
    this.mapboxgldata = mapboxgl;
    //pas le laisser ici
    this.map = new mapboxgl.Map({
      container: 'map',
      style: 'mapbox://styles/superyann89/ck37eqs4c08xg1co2lanx11vx/draft',
      //style: 'mapbox://styles/mapbox/streets-v11',
      center: [2.358989, 48.857518], // starting position [lng, lat]
      zoom: 12.49, // starting zoom,
      minZoom: 11
    });

    let state = true;
    let me = this;

    this.map.on('styledata', async () => {
      if (state == true) {
        state = false;
        await this.newImage('http://localhost:5000/Images/logo.png', 'subway-icon1', me);
        await this.newImage('http://localhost:5000/Images/marker-40.png', 'user-icon', me);
        await this.newImage('http://localhost:5000/Images/train.png', 'train-icon', me);

        let geojson = await this.JsonGet("http://localhost:5000/StopArea/GetStopAreas");
        let orderstop = await this.JsonGet("http://localhost:5000/Maps/ListeStations.json");
        //console.log(orderstop);

        let names = [];        
        let stopscoord = {};
        let colors = {};

        colors["1"] = "#ffcd00";
        colors["2"] = "#003ca6";
        colors["3"] = "#837902";
        colors["3B"] = "#6ec4e8";
        colors["4"] = "#be418d";
        colors["5"] = "#ff7e2e";
        colors["6"] = "#6eca97";
        colors["7"] = "#fa9aba";
        colors["7B"] = "#6eca97";
        colors["8"] = "#e19bdf";
        colors["9"] = "#b6bd00";
        colors["10"] = "#c9910d";
        colors["11"] = "#704b1c";
        colors["12"] = "#007852";
        colors["13"] = "#6ec4e8";
        colors["14"] = "#62259d";
        colors["15"] = "#a81032";

        geojson["features"].forEach(element => {
          names.push(element["properties"]["title"]);
          stopscoord[element["properties"]["title"]]= element["geometry"]["coordinates"];
        });
        
        //.map((element, index) => {})
        let t = orderstop.Ligne.map((l, i) => {
          return {
            id: i,
            number: l.id,
            stops: l.Stations.map( s => stopscoord[s])
          };
        });

        t.forEach(element => {
          
          this.map.addLayer({
            "id": element.id.toString(),
            "type": "line",
            "source": {
              "type": "geojson",
              "data": {
                "type": "Feature",
                "properties": {},
                "geometry": {
                  "type": "LineString",
                  "coordinates": element.stops
                  }
                }
              },
              "layout": {
              "line-join": "round",
              "line-cap": "round"
              },
            "paint": {
            "line-color": colors[element.number],
            "line-width": 7
            }
          });
        });

       
        geojson["features"].forEach(function(feature) {
          let ligne = feature.properties['ligne'];
          feature.properties.ligne0 = (ligne.length > 0) ? ligne[0] : "";
          feature.properties.ligne1 = (ligne.length > 1) ? ligne[1] : "";
          feature.properties.ligne2 = (ligne.length > 2) ? ligne[2] : "";
          feature.properties.ligne3 = (ligne.length > 3) ? ligne[3] : "";
          feature.properties.ligne4 = (ligne.length > 4) ? ligne[4] : "";
          feature.properties.ligne5 = (ligne.length > 5) ? ligne[5] : "";
        });
        
        this.geojsondata = geojson; 
        this.stationsname = names;
        
        this.map.addControl(
          new mapboxgl.GeolocateControl({
          positionOptions: {
            enableHighAccuracy: true
          },
          trackUserLocation: true,
          fitBoundsOptions: {maxZoom:13}
          })
        );
    
        this.map.addSource('points', { type: 'geojson', data: geojson });
        let origin = geojson.features[0].geometry.coordinates;

        this.map.addSource('test', {
          type: 'geojson',
          data: {
              "type": "FeatureCollection",
              "features": [{
                  "type": "Feature",
                  "properties": {},
                  "geometry": {
                      "type": "Point",
                      "coordinates": origin
                  }
              }]
          }
        });
       

       

        /* 
        window.setInterval(function() {

          //console.log(x % (timebtwstations / refreshtimer) == 0 && x !== 0);
          //console.log('x : ' + x);
            
          origin = currentposition;
          goal = geojson.features[(cpt + 1)].geometry.coordinates; 
          

          //console.log(geojson.features.length);
          if(type == '+') { x++; } else { x--;}

          currentposition = geojson.features[x].geometry.coordinates;
              
          if (x == (geojson.features.length - 1 ) && type == '+') { type = '-'; }
          else if(x == 0 && type == '-' ) { type = '+'; }
      
          //console.log('origin + ' + origin);
          //console.log('goal : ' + goal);
          //console.log(geojson.features[0].geometry.coordinates[0]);

          me.map.getSource('test').setData({
            "type": "FeatureCollection",
            "features": [{
                "type": "Feature",
                "properties": { 
                  "title": "Ton metro",
                "icon": "user-icon"  },
                "geometry": {
                    "type": "Point",
                    "coordinates": currentposition
                }
            }]
          }); 
          


        }, refreshtimer);
        */
       

        let filterGroup = document.getElementById('filter-group');
        let layersName = [];

        geojson["features"].forEach(function(feature) {
          let arrayline = feature.properties['ligne'];
          let layerID = 'poi-' + arrayline[0];
          //console.log(arrayline);

          if (!me.map.getLayer(layerID)) {
            //console.log(arrayline[0]);
            layersName.push(layerID);

            me.map.addLayer({
              'id': layerID,
              'type': 'symbol',
              'source': 'points',
              'layout': {
                'icon-image': 'subway-icon1',
                "icon-size": ["interpolate", ["linear"], ["zoom"], 10, 0.1, 12, 0.2, 13, 0.4],
                "text-field": ["step", ["zoom"], "", 14,["get", "title"]],
                "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
                "text-offset": [0, 0.7],
                "text-anchor": "top",
                'icon-allow-overlap': true
              },
              'filter': [
                  "any",
                  ["==",'ligne0', arrayline[0]],
                  ["==",'ligne1', arrayline[0]],
                  ["==",'ligne2', arrayline[0]],
                  ["==",'ligne3', arrayline[0]],
                  ["==",'ligne4', arrayline[0]],
                  ["==",'ligne5', arrayline[0]]
              
              ]
            });
            
            // Add checkbox and label elements for the layer.
            let input = document.createElement('input');
            input.type = 'checkbox';
            input.id = layerID;
            input.checked = true;
            filterGroup.appendChild(input);

            let label = document.createElement('label');
            label.setAttribute('for', layerID);
            //label.innerHTML = `Metro <img src="@/assets/logometro/m${arrayline[0]}genrvb.svg" height="23" width="23";>`;
            label.innerHTML = 'Metro <img src="/logometro/m'+arrayline[0]+'genrvb.svg" height="23" width="23";>';
            filterGroup.appendChild(label);

            // When the checkbox changes, update the visibility of the layer.
            input.addEventListener('change', function(e) {
              me.map.setLayoutProperty(
                layerID,
                'visibility',
                e.target.checked ? 'visible' : 'none'
              );
              t.forEach(element => {
                if(arrayline[0] == element.number) {
                  me.map.setLayoutProperty(
                    element.id,
                    'visibility',
                    e.target.checked ? 'visible' : 'none'
                  );
                }
              });
            });
          }
        });

        let clearfilter = document.getElementById('clear-filter');
        let input = document.createElement('input');
        input.type = 'checkbox';
        input.id = "clear";
        input.checked = true;
        clearfilter.appendChild(input);
        let label = document.createElement('label');
        label.setAttribute('for', "clear");
        label.id = "cleartext";
        label.textContent = 'Tout ❌';
        clearfilter.appendChild(label);

        input.addEventListener('change', function(e) {
          document.getElementById("cleartext").innerHTML = e.target.checked ? 'Tout ❌' : 'Tout ✔️';
          layersName.forEach(element => {
            document.getElementById(element).checked = e.target.checked ? true : false
            me.map.setLayoutProperty(
              element,
              'visibility',
              e.target.checked ? 'visible' : 'none'
            );
          });
          t.forEach(element2 => {
            me.map.setLayoutProperty(
              element2.id,
              'visibility',
              e.target.checked ? 'visible' : 'none'
            );
          });
        });
        //console.log(layersName);
        layersName.forEach(element => {
          me.addMarker(element, me, mapboxgl);
        });
/* 
        let stationid = 10;
        let tempsrestant = 2;
        this.map.addLayer({
          "id": "test",
          "type": "symbol",
          "source": "test",
          "layout": {
          // get the icon name from the source's "icon" property
          // concatenate the name to get an icon from the style's sprite sheet
          "icon-image": "train-icon",
          "icon-size": 1.5,
          // get the title name from the source's "title" property
          "text-field": ["get", "title"],
          "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
          "text-offset": [0, 0.6],
          "text-anchor": "top"
          }
        }); 
        window.setInterval( function() {
            let nbstations = Math.trunc(tempsrestant/1.30);
            if (type == '+') {currentposition = geojson.features[stationid - nbstations ].geometry.coordinates;} 
            else { currentposition = geojson.features[stationid + nbstations].geometry.coordinates }

            me.map.getSource('test').setData({
              "type": "FeatureCollection",
              "features": [{
                  "type": "Feature",
                  "properties": { 
                    "title": "Ton metro",
                  "icon": "user-icon"  },
                  "geometry": {
                      "type": "Point",
                      "coordinates": currentposition
                  }
              }]
            }); 

        }, refreshtimer);

 */



   
        
      }
    });
  },



  methods: {

    alertevent: function (event) {
      navigator.geolocation.getCurrentPosition(this.success);
    },

    async UpdateData(lignes, popup, arret, description, me) {
      let texte = "";
      let timeleft = await me.UpdateSubway(arret);
      console.log(timeleft);
      lignes.forEach(nb => {
        texte += '<img src="/logometro/m'+nb+'genrvb.svg" height="25" width="25" display="block" margin="0 auto";> prochain dans <b>'+timeleft['line'+nb]+'min</b><br>';
      });
      
      popup.setHTML("<strong>"+description+"</strong><p>" + texte + "</p>");

      
      let interval = setInterval(async () => {
        if (!popup.isOpen()) clearInterval(interval);
        let texte = "";
        let timeleft = await me.UpdateSubway(arret);
        //console.log(timeleft);
        lignes.forEach(nb => {
          texte += '<img src="/logometro/m'+nb+'genrvb.svg" height="25" width="25" display="block" margin="0 auto";> prochain dans <b>'+timeleft['line'+nb]+'min</b><br>';
        });
        
        popup.setHTML("<strong>"+description+"</strong><p>" + texte + "</p>");

        
      },20000, lignes, popup, arret, description, me );

    },

    async addMarker(layername, me, mapboxgl) {
      me.map.on('click', layername , async (e) => {
        if(me.popupinfo !== null) this.popupinfo.remove(); 
        let mcoordinates = e.features[0].geometry.coordinates.slice();
        let description = e.features[0].properties.title;
        let lignes = e.features[0].properties.ligne;
        lignes = JSON.parse(lignes);

        me.popupinfo = new mapboxgl.Popup()
          .setLngLat(mcoordinates)
          .addTo(me.map);
        
        me.UpdateData(lignes, me.popupinfo, e.features[0].properties.title, description, me);
       
      });
      // Change the cursor to a ? when the mouse is over the subway stations.
      me.map.on('mouseenter', layername , function() {
        me.map.getCanvas().style.cursor = 'help';
      });      
      // Change it back to a pointer when it leaves.
      me.map.on('mouseleave', layername , function() {
        me.map.getCanvas().style.cursor = '';
      });

    },

    async UpdateSubway(arret) {
      return (await getTimeStopAsync(arret));
    },

    CheckboxDisplayEvent(e,t) {
      me.map.setLayoutProperty(
        layerID,
        'visibility',
        e.target.checked ? 'visible' : 'none'
      );
      t.forEach(element => {
        if(arrayline[0] == element.number) {
          me.map.setLayoutProperty(
            element.id,
            'visibility',
            e.target.checked ? 'visible' : 'none'
          );
        }
      });
    },

    async JsonGet(url) {
      let response = await fetch(url);
      let json = await response.json();
      //console.log(json);
      return json;
    },

    async success(pos) {
      this.crd = pos.coords;
      console.log('Your current position is:');
      console.log(`Latitude : ${ this.crd.latitude}`);
      console.log(`Longitude: ${this.crd.longitude}`);
      console.log(`More or less ${this.crd.accuracy} meters.`); 
      console.log(`Le code pour l'event `);
    },

    async newImage(loc, name, me) {
      me.map.loadImage(loc, await function(error, image) {
      //console.log(image);
      if (error) throw error;
      // Add the loaded image to the style's sprite with the ID.
      me.map.addImage(name, image);
      });
    },

    search(input) {
      if (input.length < 1) { return [] }
      return this.stationsname.filter(name => {
        return name.toLowerCase()
          .startsWith(input.toLowerCase())
      })
    }, 

    async searchsubmit(result) {
      let id = this.stationsname.indexOf(result);
      //console.log(id);
      if(id > -1) {
        //console.log(id);
        if(this.popupinfo !== null) this.popupinfo.remove(); 
        let mcoordinates = this.geojsondata.features[id].geometry.coordinates.slice();
        let description = this.geojsondata.features[id].properties.title;
        this.popupinfo = new this.mapboxgldata.Popup()
          .setLngLat(mcoordinates)
          .addTo(this.map);

        this.map.jumpTo({ 'center': this.geojsondata.features[id].geometry.coordinates, 'zoom': 14 });
        this.UpdateData(this.geojsondata.features[id].properties.ligne, this.popupinfo, this.geojsondata.features[id].properties.title, description, this);
      }
    }
  }
}
</script>

<style>
    body { margin:0; padding:0; }
    #map { position:absolute; top:0; bottom:0; width:100%; }
    #marker {
    
    background-size: cover;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    cursor: help;
    }
    
    .mapboxgl-popup {
    max-width: 200px;
    }

    .alertbutton {
      z-index: 1;
      width: 40px;
      height: 40px;
    }
    .filter-group {
    font: 12px/20px 'Helvetica Neue', Arial, Helvetica, sans-serif;
    font-weight: 600;
    position: absolute;
    top: 150px;
    right: 10px;
    z-index: 1;
    border-radius: 3px;
    width: 100px;
    color: #fff;
    }

    .clear-filter {
    font: 12px/20px 'Helvetica Neue', Arial, Helvetica, sans-serif;
    font-weight: 600;
    position: absolute;
    top: 120px;
    right: 10px;
    z-index: 1;
    border-radius: 3px;
    width: 100px;
    color: #fff;
    }
    
    .filter-group input[type='checkbox']:first-child + label {
    border-radius: 5px 5px 0 0;
    }
    
    .filter-group label:last-child {
    border-radius: 0 0 5px 5px;
    border: none;
    }
    
    .filter-group input[type='checkbox'] {
    display: none;
    }
    .clear-filter input[type='checkbox'] {
    display: none;
    }

    .filter-group input[type='checkbox'] + label {
    background-color: #b0b0b0;
    display: block;
    cursor: pointer;
    padding: 4px;
    border-bottom: 1px solid rgba(0, 0, 0, 0.25);
    }

    .clear-filter input[type='checkbox'] + label {
    background-color: #d3d3d3;
    display: block;
    cursor: pointer;
    padding: 4px;
    border-bottom: 1px solid rgba(0, 0, 0, 0.25);
    border-radius: 25px;
    }
    
    .filter-group input[type='checkbox'] + label {
    background-color: #b0b0b0;
    text-transform: capitalize;
    }
    
    .filter-group input[type='checkbox'] + label:hover,
    .filter-group input[type='checkbox']:checked + label {
    background-color: #b0b0b0;
    }
    
    .filter-group input[type='checkbox']:checked + label:after {
    content: '✔️';
    margin-left: 5px;
    }

    .filter-ctrl {
    position: absolute;
    top: 50px;
    right: 30px;
    z-index: 1;
    width: 180px;
    }
    
    .filter-ctrl input[type='text'] {
    font: 12px/20px 'Helvetica Neue', Arial, Helvetica, sans-serif;
    width: 100%;
    border: 0;
    background-color: #fff;
    height: 40px;
    margin: 0;
    color: rgba(0, 0, 0, 0.5);
    padding: 10px;
    box-shadow: 0 0 0 2px rgba(0, 0, 0, 0.1);
    border-radius: 3px;
    }
    .searchbar {
    position: absolute;
    top: 50px;
    right: 10px;
    z-index: 1;
    width: 220px;
    }
    .mapboxgl-popup-content {
    font: 15px/20px 'Helvetica Neue', Arial, Helvetica, sans-serif;
    }
    
</style>