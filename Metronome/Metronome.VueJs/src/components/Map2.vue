<template>
    <div id="map-container">
        <div id="map" style="width:100%; height:100vh;"></div>
    </div>
</template>

<script>

export default {

    data() {
        return {
            map: null,
            dotmapbox: null
        }
    },
    mounted: function ()
    {
        mapboxgl.baseApiUrl = 'https://api.mapbox.com';
        mapboxgl.accessToken = 'pk.eyJ1IjoibWV0cm9ub21leGFwcCIsImEiOiJjazJhZWQ0cXEyc2h5M2Nucm0yaGp0dnQ3In0.TWAnNTliW7AzEFekxjuVIg';
        
        //mapboxgl.workerCount = 2; // Nombre de Cores (CPU) utilisé.
        //mapboxgl.maxParallelImageRequests = 8;


        // CREATE MAP
        map = new mapboxgl.Map({
            container: 'map',
                    style: 'mapbox://styles/metronomexapp/ck2agx8dt25un1cs4mwtuoas6',
                    center:[2.3488,48.8534],
                    antialias: true,
                    container: 'map',
                    zoom:15.5,
                    pitch: 45,
                    bearing: -17.6
                    });

        function rotateCamera(timestamp)
        {
            map.rotateTo((timestamp / 350) % 360, {duration: 0});
            requestAnimationFrame(rotateCamera);
        }

        this.dotmapbox = {
                size : 100,
                width: this.size,
                height: this.size,
                data: new Uint8Array(this.size * this.size * 4),

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
            }

        // MAP => LOAD

        map.on('load', function( )
            {             
                // ACTIVE ROTATION
                rotateCamera(0);

                var layers = map.getStyle().layers;
                var labelLayerId;
                for (var i = 0; i < layers.length; i++)
                {
                    if (layers[i].type === 'symbol' && layers[i].layout['text-field'])
                    {
                        labelLayerId = layers[i].id;
                        break;
                    }
                }

            map.addLayer(
            {
                'id': '3d-buildings',
                'source': 'composite',
                'source-layer': 'building',
                'filter': ['==', 'extrude', 'true'],
                'type': 'fill-extrusion',
                'minzoom': 15,
                'paint': {
                    'fill-extrusion-color': '#fff',
                    'fill-extrusion-height': [
                    "interpolate", ["linear"], ["zoom"],
                    15, 0,
                    15.05, ["get", "height"]
                    ],
                    'fill-extrusion-base': [
                    "interpolate", ["linear"], ["zoom"],
                    15, 0,
                    15.05, ["get", "min_height"]
                    ],
                    'fill-extrusion-opacity': .8
                }
            }, labelLayerId);
        });

        
    }
}

</script>

<style scoped>

@import url("https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.css");

*{ margin:0px; padding:0px; }

</style>