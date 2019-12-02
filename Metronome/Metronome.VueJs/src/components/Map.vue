<template>
    <div id="map" style="width:100%; height:100vh;"></div>
</template>

<script>
export default {

    data() {
        return {
        }
    },
    
    mounted: function ()
    { 
mapboxgl.accessToken = 'pk.eyJ1IjoibWV0cm9ub21leGFwcCIsImEiOiJjazJhZWQ0cXEyc2h5M2Nucm0yaGp0dnQ3In0.TWAnNTliW7AzEFekxjuVIg';



        // CREATE MAP
        var map = new mapboxgl.Map({
            container: 'map',
                    style: 'mapbox://styles/metronomexapp/ck2agx8dt25un1cs4mwtuoas6',
                    center:[2.3488,48.8534],
                    zoom:17,
                    antialias: true,
                    container: 'map'
                    });

var size = 100;

var pulsingDot = {
    width: size,
    height: size,
    data: new Uint8Array(size * size * 4),

    onAdd: function() {
        var canvas = document.createElement('canvas');
        canvas.width = this.width;
        canvas.height = this.height;
        this.context = canvas.getContext('2d');
    },

    render: function() {
        var duration = 3000;
        var t = (performance.now() % duration) / duration;

        var radius = size / 2 * 0.3;
        var outerRadius = size / 2 * 0.7 * t + radius;
        var context = this.context;

        // draw inner circle
        context.beginPath();
        context.arc(this.width / 2, this.height / 2, radius, 0, Math.PI * 2);
        context.fillStyle = 'rgba(255, 100, 100, 1)';
        context.fill();
        context.stroke();

        // update this image's data with data from the canvas
        this.data = context.getImageData(0, 0, this.width, this.height).data;

        // keep the map repainting
        map.triggerRepaint();

        // return `true` to let the map know that the image was updated
        return true;
    }
};

map.on('load', function () {

    map.addImage('pulsing-dot', pulsingDot, { pixelRatio: 2 });

    map.addLayer(
        {
            "id": "points",
            "type": "symbol",
            "source": {
                "type": "geojson",
                "data": {
                    "type": "FeatureCollection",
                    "features": [{
                        "type": "Feature",
                        "geometry": {
                            "type": "Point",
                            "coordinates": [2.3488,48.8534]
                        }
                    }]
                }
            },
            "layout": {
                "icon-image": "pulsing-dot"
            }
        });
    });
},

    method:{

        generateStationMetro( lng, lat, name, type ){

            let x = { "type": "Faeture", "geometry" : { "type": "Point", "coordinates" : [ lng, lat ] } };
            return x;

        }

    }

}
</script>