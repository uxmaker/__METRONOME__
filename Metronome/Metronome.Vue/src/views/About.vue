<template>
  <div class="about">
    <link href='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.css' rel='stylesheet' />
    <div id='map'></div>
    <nav id="filter-group" class="filter-group"></nav>
  </div>
</template>

<script>
export default {
  data() {
    return {
      map: null,
      origin: null,
      goal: null,
      refreshtimer: null, 
      crd: null    
    }
  },

  mounted: async function() {

    var mapboxgl = require('mapbox-gl/dist/mapbox-gl.js');
    mapboxgl.accessToken = 'pk.eyJ1Ijoic3VwZXJ5YW5uODkiLCJhIjoiY2syNGgwaG1yMjM2NjNobXY0eTZyNDhtdiJ9.PapYOTQWtc2d0HP7VLCbDw';
    //pas le laisser ici
    this.map = new mapboxgl.Map({
      container: 'map',
      style: 'mapbox://styles/superyann89/ck37eqs4c08xg1co2lanx11vx/draft',
      //style: 'mapbox://styles/mapbox/streets-v11',
      center: [2.358989, 48.857518], // starting position [lng, lat]
      zoom: 12.49, // starting zoom,
      minZoom: 11
    });

    var state = true;
    var me = this;

    this.map.on('styledata', async () => {
      if (state == true) {
        state = false;
        await this.newImage('http://localhost:5000/Images/logo.png', 'subway-icon1', me);
        await this.newImage('http://localhost:5000/Images/marker-40.png', 'user-icon', me);
        await this.newImage('http://localhost:5000/Images/train.png', 'train-icon', me);

        await navigator.geolocation.getCurrentPosition(this.success);

        var geojson = await this.JsonGet();
        console.log(geojson);

        let coordline1 = [];

        geojson["features"].forEach(element => {
          coordline1.push(element["geometry"]["coordinates"]);
        });
        //console.log(coordline1);
        
        this.map.addControl(
          new mapboxgl.GeolocateControl({
          positionOptions: {
            enableHighAccuracy: true
          },
          trackUserLocation: true,
          fitBoundsOptions: {maxZoom:13}
          })
        );
        
        this.map.addLayer({
          "id": "route",
          "type": "line",
          "source": {
            "type": "geojson",
            "data": {
              "type": "Feature",
              "properties": {},
              "geometry": {
                "type": "LineString",
                "coordinates": coordline1
                }
              }
            },
            "layout": {
            "line-join": "round",
            "line-cap": "round"
            },
          "paint": {
          "line-color": "#FFCD00",
          "line-width": 7
          }
        });
        
        this.map.addSource('points', { type: 'geojson', data: geojson });
        /* this.map.addLayer({
          "id": "points",
          "type": "symbol",
          "source": "points",
          "layout": {
          // get the icon name from the source's "icon" property
          // concatenate the name to get an icon from the style's sprite sheet
          "icon-image": "subway-icon1",
          "icon-size": 0.4,
          // get the title name from the source's "title" property
          "text-field": ["get", "title"],
          "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
          "text-offset": [0, 0.6],
          "text-anchor": "top"
          }
        }); */

        this.map.jumpTo({ 'center': [this.crd.longitude, this.crd.latitude], 'zoom': 12 });

        //var testcoord =  geojson.features[0].geometry.coordinates;
        var origin = geojson.features[0].geometry.coordinates;

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
       

        var timebtwstations = 2000;
        var refreshtimer = 700;
        var cpt = -1;
        var goal;
        
        var currentposition = geojson.features[0].geometry.coordinates;
        var x = -1;
        var type = '+';

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
       

        var filterGroup = document.getElementById('filter-group');
        var layersName = [];

        geojson["features"].forEach(function(feature) {
          var symbol = feature.properties['ligne'];
          var layerID = 'poi-' + symbol;
          layersName.push(layerID);

          if (!me.map.getLayer(layerID)) {
            me.map.addLayer({
            'id': layerID,
            'type': 'symbol',
            'source': 'points',
            'layout': {
              'icon-image': 'subway-icon1',
              "icon-size": 0.4,
              "text-field": ["get", "title"],
              "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
              "text-offset": [0, 0.6],
              "text-anchor": "top",
              'icon-allow-overlap': true
            },
            'filter': ['==', 'ligne', symbol]
          });
          
          // Add checkbox and label elements for the layer.
          var input = document.createElement('input');
          input.type = 'checkbox';
          input.id = layerID;
          input.checked = true;
          filterGroup.appendChild(input);

          var label = document.createElement('label');
          label.setAttribute('for', layerID);
          label.textContent = 'ligne ' + symbol;
          filterGroup.appendChild(label);

          // When the checkbox changes, update the visibility of the layer.
          input.addEventListener('change', function(e) {
            me.map.setLayoutProperty(
              layerID,
              'visibility',
              e.target.checked ? 'visible' : 'none'
            );
          });
        }
        });

        layersName.forEach(element => {
          me.addMarker(element, me, mapboxgl);
        });

        var stationid = 10;
        var tempsrestant = 2;
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

            var nbstations = Math.trunc(tempsrestant/1.30);
            //console.log(stationid + nbstations);
            //currentposition = geojson.features[stationid].geometry.coordinates;
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
        

      }
    });
  },



  methods: {

    async addMarker(layername, me, mapboxgl) {
      me.map.on('click', layername , async (e) => {
        var mcoordinates = e.features[0].geometry.coordinates.slice();
        var description = e.features[0].properties.title;

        var timeleft = await me.UpdateSubway(e.features[0].properties.title);
        //GET THE DATA FROM THE STOP|UPDATE|DISPLAY
        //EXPECTED AS id = e.features[0].properties.id

        // Ensure that if the map is zoomed out such that multiple
        // copies of the feature are visible, the popup appears
        // over the copy being pointed to.
        while (Math.abs(e.lngLat.lng - mcoordinates[0]) > 180) {
          mcoordinates[0] += e.lngLat.lng > mcoordinates[0] ? 360 : -360;
        }
        
        new mapboxgl.Popup()
          .setLngLat(mcoordinates)
          .setHTML(description + " prochain train dans : " + timeleft + "min")
          .addTo(me.map);
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

    async UpdateSubway() {
      return(4);
    },

    async JsonGet() {
      let response = await fetch("http://localhost:5000/Maps/pointmap.geojson");
      let json = await response.json();
      //console.log(json);
      
      return json;
    },

    async success(pos) {
      this.crd = pos.coords;
      /* console.log('Your current position is:');
      console.log(`Latitude : ${ this.crd.latitude}`);
      console.log(`Longitude: ${this.crd.longitude}`);
      console.log(`More or less ${this.crd.accuracy} meters.`); */
    },

    async newImage(loc, name, me) {
      me.map.loadImage(loc, await function(error, image) {
      //console.log(image);
      if (error) throw error;
      // Add the loaded image to the style's sprite with the ID.
      me.map.addImage(name, image);
      });
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

    .filter-group {
    font: 14px/20px 'Helvetica Neue', Arial, Helvetica, sans-serif;
    font-weight: 600;
    position: absolute;
    top: 50px;
    right: 10px;
    z-index: 1;
    border-radius: 3px;
    width: 120px;
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
    
    .filter-group input[type='checkbox'] + label {
    background-color: #b0b0b0;
    display: block;
    cursor: pointer;
    padding: 10px;
    border-bottom: 1px solid rgba(0, 0, 0, 0.25);
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
</style>