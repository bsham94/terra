
  map.on('load', function() {

    map.addSource('SoilMoistureSource', {
        "type": "geojson",
        "data": "./SoilMoisture.geojson"
    });
      map.addLayer({
          "id": "Soil Moisuture",
          "type": "circle",
          "source": "SoilMoistureSource",
          "minzoom": 1,
          "paint": {
            // increase the radius of the circle as the zoom level and sm value increases
              "circle-radius": {
                  "property": "sm",
                  "type": "exponential",

                  "stops": [
                      [{ zoom: 15, value: 1 }, 30],
                  //    [{ zoom: 15, value: 62 }, 30],
                  //    [{ zoom: 22, value: 1 }, 20],
                //      [{ zoom: 22, value: 62 }, 50],
                  ]

              },
              "circle-color": {
                  "property": "sm",
                  "type": "exponential",
                  "stops": [
                    [0, 'red'],
                    [10, 'orange'],
                    [20, 'yellow'],
                    [30, "green"],
                    [40, "blue"],
                    [50, "darkblue"],
                    [60, "purple"]
                  ]
              },
//              "circle-stroke-color": "yellow",
//              "circle-stroke-width": 10,
//              "circle-stroke-opacity": 0.10,
/*
              "circle-opacity": {
                  "stops": [
                      [5, 0],
                      [5, 1]
                  ]
              }*/
              "circle-opacity": 0.35,
              "circle-blur": 0.5
          }
      }, 'waterway-label');
  });

  //click on tree to view sm in a popup
  map.on('click', 'Soil Moisuture', function (e) {
    new mapboxgl.Popup()
      .setLngLat(e.features[0].geometry.coordinates)
      .setHTML('<b>Soil Moisuture:</b> '+ e.features[0].properties.sm + '<br/><b>Coordinate Data:</b>' + e.features[0].geometry.coordinates)
      .addTo(map);
  });
