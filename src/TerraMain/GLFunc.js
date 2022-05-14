
mapboxgl.accessToken = 'pk.eyJ1IjoiaHl1bmJpbjczMDMiLCJhIjoiY2pxbjRxemh3MWhqajQ4bTAwc3MxYTJveCJ9.IAd0fIhJkk0dGgIuEHaREw';
const map = new mapboxgl.Map
({
      container: 'map',
      style: 'mapbox://styles/mapbox/dark-v9',
      center: [-81.338709,44.316708], // Remeber, this is longtitude and latitude.
      zoom: 12.0

});
var hoveredStatedID = null;
var toggleableLayerIds = ['terra-layer', 'places','Soil Moisuture', 'layerTesting1', 'layerTesting2'];




var dataFarm;
var data = $.getJSON( "farmfield.geojson", function( json ) {
  dataFarm = data.responseJSON;
});

  map.on('load', function() {

    map.addSource('terra-farm',{
        'type': 'geojson',
        'data': dataFarm
    });
    map.addLayer({
        'id': 'terra-layer',
        'type': 'fill',
        'source': 'terra-farm',
        'layout': {},
        'paint': {
            'fill-color': '#088',
            'fill-opacity': ["case",
                ["boolean", ["feature-state", "hover"], false],
                1,
                0.5
                ]
        }
    });
    // Initialize the farmland information on the side bar.
    // Create Farmland location. create_FarmLandList(farmLoc);
    create_FarmLandList(dataFarm);
  });


/*
var draw = new MapboxDraw({
  displayControlsDefault: false,
  controls:{
    polygon: true,
    trash: true
  }
});

map.addControl(draw);
map.on('draw.create', updateArea);
map.on('draw.delete', updateArea);
map.on('draw.update', updateArea);
*/
function updateArea(e){
  var data = draw.getAll();
  var ans = document.getElementById('calculated-area');
  if(data.features.length >0){
    var area = turf.area(data);
    var rounded_area = Math.round(area*100) / 100;
    ans.innerHTML = '<p><strong>' + rounded_area + '</strong></p><p>Square meters.</p>';
  }
  else{
    ans.innerHTML = '';
    if(e.type !== 'draw.delete') {
      alert("Use somethign... whatever");
    }
  }
}
function mouseControlFunc(){

  map.on('mousemove', 'terra-layer', function (e)
  {
    console.log("Entered to hover State ID.");

    if (e.features.length > 0) {
    if (hoveredStatedID) {

        map.setFeatureState({source: 'terra-farm', id: hoveredStatedID}, { hover: false});
      }
      hoveredStatedID = e.features[0].id;
      map.setFeatureState({source: 'terra-farm', id: hoveredStatedID}, { hover: true});
    }
  });
  map.on('mousemove', function (e)
  {
      document.getElementById('info').innerHTML =
              // e.point is the x, y coordinates of the mousemove event relative
              // to the top-left corner of the map
      JSON.stringify(e.point) + '<br />' +
      JSON.stringify(e.lngLat);
  });



  map.on('click', 'terra-layer', function (e) {
      new mapboxgl.Popup()
          .setLngLat(e.lngLat)
          .setHTML("<b>FIELD OWNER ID : </b>" + e.features[0].properties.id + "</br><b>FARMLAND ID : </b>" + e.features[0].properties.farmid + "</br><b>temperature : </b>" + e.features[0].properties.temperature)
          .addTo(map);
  });
  // Change the cursor to a pointer when the mouse is over the states layer.
  map.on('mouseenter', 'terra-layer', function () {
      map.getCanvas().style.cursor = 'pointer';
  });
  // Change it back to a pointer when it leaves.
  map.on('mouseleave', 'terra-layer', function () {
      map.getCanvas().style.cursor = '';
      if (hoveredStatedID) {
      map.setFeatureState({source: 'terra-farm', id: hoveredStatedID}, { hover: false});
      }
      hoveredStatedID =  null;
  });

}
function hardCoded_GeoJson(){
  // Hardcoded GeoJson Data Testing.
  //Displaying Conestoga College location.
  map.on('load', function () {
          map.addLayer({
              "id": "points",
              "type": "symbol",
              "source":
              {
                  "type": "geojson",
                  "data": {
                      "type": "FeatureCollection",
                      "features": [{
                          "type": "Feature",
                          "geometry": {
                              "type": "Point",
                              "coordinates": [-77.03238901390978, 38.913188059745586]
                          },
                          "properties": {
                              "title": "CommuniTech",
                              "icon": "monument"
                          }
                      },
                      {
                          "type": "Feature",
                          "geometry": {
                              "type": "Point",
                              "coordinates": [-80.411570, 43.387970]
                          },
                          "properties": {
                              "title": "Conestoga College - Main Office(TERRA)",
                              "icon": "monument"
                          }
                      }]
                  }
              },
              "layout": {
                  "icon-image": "{icon}-15",
                  "text-field": "{title}",
                  "text-font": ["Open Sans Semibold", "Arial Unicode MS Bold"],
                  "text-offset": [0, 0.6],
                  "text-anchor": "top"
              }
          });
      });
}
function hardCoded_GeoJsonWithDesc(){
  map.on('load', function () {
      // Add a layer showing the places.
      map.addLayer({
      "id": "places",
      "type": "symbol",
      "source": {
      "type": "geojson",
      "data": {
      "type": "FeatureCollection",
      "features":

      [
        {
          "type": "Feature",
          "properties": {
          "description": "<strong>KEVIN!ddd KEVIN!</strong><p><a href=\"www.google.com\" target=\"_blank\" title=\"Opens in a new window\">Google!</a>Desccription</p></b> hey nice job",
          "icon": "bar"
          },
          "geometry": {
          "type": "Point",
          "coordinates": [-81.338709,44.316708]
          }
        },
        {
          "type": "Feature",
          "properties": {
          "description": "<strong>Big Backyard Beach Bash and Wine Fest</strong><p>EatBar (2761 Washington Boulevard Arlington VA) is throwing a <a href=\"http://tallulaeatbar.ticketleap.com/2012beachblanket/\" target=\"_blank\" title=\"Opens in a new window\">Big Backyard Beach Bash and Wine Fest</a> on Saturday, serving up conch fritters, fish tacos and crab sliders, and Red Apron hot dogs. 12:00-3:00 p.m. $25.grill hot dogs.</p>",
          "icon": "bicycle"
          },
          "geometry": {
          "type": "Point",
          "coordinates": [-77.090372, 38.881189]
          }
        }]
      }
      },
      "layout": {
      "icon-image": "{icon}-15",
      "icon-allow-overlap": true
      }
      });

      // When a click event occurs on a feature in the places layer, open a popup at the
      // location of the feature, with description HTML from its properties.
      map.on('click', 'places', function (e) {
      var coordinates = e.features[0].geometry.coordinates.slice();
      var description = e.features[0].properties.description;

      // Ensure that if the map is zoomed out such that multiple
      // copies of the feature are visible, the popup appears
      // over the copy being pointed to.
      while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
      coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
      }

      new mapboxgl.Popup()
      .setLngLat(coordinates)
      .setHTML(description)
      .addTo(map);
      });

      // Change the cursor to a pointer when the mouse is over the places layer.
      map.on('mouseenter', 'places', function () {
      map.getCanvas().style.cursor = 'pointer';
      });

      // Change it back to a pointer when it leaves.
      map.on('mouseleave', 'places', function () {
      map.getCanvas().style.cursor = '';
      });

  });
}
function create_FarmLandList(farminfo){

  console.log("Create FarmLand List method.");
  for(i = 0; i< farminfo.features.length; i++){
    var curFeature = farminfo.features[i];
    var prop = curFeature.properties;

    var listings = document.getElementById('listings');
    var listing = listings.appendChild(document.createElement('div'));
    listing.className = 'item';
    listing.id = "listing-" + i;

    var link = listing.appendChild(document.createElement('a'));
    link.href = '#';
    link.className = 'title';
    link.dataPosition = i;
    link.innerHTML = "Farmland ID : " + prop.farmid;

    var details = listing.appendChild(document.createElement('div'));
    details.innerHTML = "Temperature : " + prop.temperature;


    link.addEventListener('click', function(e){
      var clickedListing = farminfo.features[this.dataPosition];
      // Need to fly to the points
      GoToFarmSelected(clickedListing);

      var activeItem = document.getElementsByClassName('active');
  //    e.stopPropagation();


      if(activeItem[0]){
        activeItem[0].classList.remove('active');

      }
      this.parentNode.classList.add('active');
    });
  }
}
function GoToFarmSelected(curFeature){
  console.log("Clicked Listing value. " + curFeature.geometry.coordinates[0][1]);
  map.flyTo({
    center:curFeature.geometry.coordinates[0][1],
    zoom: 13
  });
}
function toggleFunc(){
  for (var i = 0; i < toggleableLayerIds.length; i++) {
      var id = toggleableLayerIds[i];

      var link = document.createElement('a');
      link.href = '#';
      link.className = 'active';
      link.textContent = id;

      link.onclick = function (e) {
          var clickedLayer = this.textContent;
          e.preventDefault();
          e.stopPropagation();

          var visibility = map.getLayoutProperty(clickedLayer, 'visibility');

          if (visibility === 'visible') {
              map.setLayoutProperty(clickedLayer, 'visibility', 'none');
              this.className = '';
          } else {
              this.className = 'active';
              map.setLayoutProperty(clickedLayer, 'visibility', 'visible');
          }
      };

      var layers = document.getElementById('menu');
      layers.appendChild(link);
  }
}



toggleFunc();
mouseControlFunc();
hardCoded_GeoJson();
//hardCoded_GeoJsonWithDesc();


// Loading Layer Again.
/*
map.on('style.load', function() {
    // Always add the same custom soruces and layers after a style change

    for (var i = 0; i < customLayers.length; i++) {
        var me = customLayers[i]
        map.addSource(me.layer.source, me.source);
        map.addLayer(me.layer, 'waterway-label')
    }
});
*/


var myBasicLayer = 'mapbox://styles/hyunbin7303/cjqtosfzq60gg2qrq0h9kwc01';
var prop = document.getElementById('prop');
prop.addEventListener('change', function() {

    var selectedOne= prop.value;
    if(selectedOne == 'cb_Darkscreen')
    {
      console.log("Dark Screen Selected.");
      map.setStyle('mapbox://styles/mapbox/dark-v9');
    }
    else if(selectedOne == 'cb_Satellite')
    {
      console.log("Satellite selected. ");
      map.setStyle('mapbox://styles/mapbox/satellite-v9');
    }
    else {
      console.log("ELSE??");
    }
});
