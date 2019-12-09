<template>
  <div class="about">
    <link href='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.css' rel='stylesheet' />
    <div id='map'></div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      map: null,
      ggeojson: null,
      origin: null,
      goal: null,
      timebtwstations: null,
      refreshtimer: null,
      latitudestep: null,
      longitudestep: null,
      crd: null    
    }
  },

  mounted: async function() {
    var mapboxgl = require('mapbox-gl/dist/mapbox-gl.js');
    mapboxgl.accessToken = 'pk.eyJ1Ijoic3VwZXJ5YW5uODkiLCJhIjoiY2syNGgwaG1yMjM2NjNobXY0eTZyNDhtdiJ9.PapYOTQWtc2d0HP7VLCbDw';
    this.map = await new mapboxgl.Map({
      container: 'map',
      style: 'mapbox://styles/superyann89/ck37eqs4c08xg1co2lanx11vx/draft',
      //style: 'mapbox://styles/mapbox/streets-v11',
      center: [2.358989, 48.857518], // starting position [lng, lat]
      zoom: 12.49, // starting zoom,
    });

    let me = this;
    await this.newImage('http://localhost:5000/Images/logo.png', 'subway-icon1', me);
    await this.newImage('http://localhost:5000/Images/marker-40.png', 'user-icon', me);
    await this.newImage('http://localhost:5000/Images/train.png', 'train-icon', me);

    /* await this.map.loadImage('http://localhost:5000/Images/marker-40.png', function(error, image) {
      //console.log(image);
      if (error) throw error;
      // Add the loaded image to the style's sprite with the ID.
      me.map.addImage('subway-icon10', image);
    });
    await this.map.loadImage('http://localhost:5000/Images/marker-40.png', function(error, image) {
      if (error) throw error;
      me.map.addImage('user-icon10', image);
    }); */


    await navigator.geolocation.getCurrentPosition(this.success);

    var geojson = await this.JsonGet();
    //console.log(geojson);

    let coordline1 = [];

    geojson["features"].forEach(element => {
      coordline1.push(element["geometry"]["coordinates"]);
    });
    //console.log(coordline1);
    
    
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
    this.map.addLayer({
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
    });

    //user position
    this.map.addLayer({
      "id": "userposition",
      "type": "symbol",
      "source": {
        "type": "geojson",
        "data": {
          "type": "FeatureCollection",
          "features": [{
            "type": "Feature",
            "geometry": {
            "type": "Point",
            "coordinates": [this.crd.longitude, this.crd.latitude]
            },
            "properties": {
            "title": "You",
            "icon": "user-icon"
            }
          }]
          }
      },
      "layout": {
      // get the icon name from the source's "icon" property
      // concatenate the name to get an icon from the style's sprite sheet
      "icon-image": ["concat", ["get", "icon"]],
      "icon-size": 1,
      // get the title name from the source's "title" property
      "text-field": ["get", "title"],
      "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
      "text-offset": [0, 0.6],
      "text-anchor": "top"
      }
    });

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

    


      var timebtwstations = 2000;
      var refreshtimer = 700;
      
      var cpt = -1;
      
      var goal;

      
      var currentposition = geojson.features[0].geometry.coordinates;
      var x = -1;
      var type = '+';

      
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
      

 
      var stationid = 10;
      var tempsrestant = 2;
 /*      
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
  */

    },

  methods: {

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
</style>