/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  farmMap.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality for the map to work                   |
|   Example:      https://go.nasa.gov/2UPM2oR                                                    |
*************************************************************************************************/


import React from 'react';
import ReactMapboxGl, { 
  Layer, 
  Feature, 
  GeoJSONLayer, 
  Popup, 
  ScaleControl,
  ZoomControl,
  RotationControl,
} from "react-mapbox-gl";
import styled from 'styled-components';
import API from './api';
import LayerSelection from './layerSelection';
import '@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css';
import NamingPopup from './fieldNamePopup.js';
import MapAlert from './mapAlert.js';
import Select from 'react-select';
import Geocode from "react-geocode";
import BoxIn from "./../assets/defaults/boxLocation.geojson";



/*
Gramps old farm
POLYGON((-81.3397394295207 44.3119328211494,
  -81.3353276684481 44.3101656596193,
  -81.3413938399206 44.302461586382,
  -81.3419453100539 44.3025817021824,
  -81.3408423697873 44.3041603442054,
  -81.3424488262625 44.3048638557847,
  -81.3434798356422 44.3048810145027,
  -81.3447506146512 44.3054644077801,
  -81.3397394295207 44.3119328211494))


*/
/*  grid square/polygon:  
**  NW - 45.700687, -82.453883
**  NE - 45.700687, -78.596814
**  SE - 42.50156423966207, -78.896814
**  SW - 42.50156423966207, -82.67500689573649

**  tempLat = [45.700687, 45.700687, 42.50156423966207, 42.50156423966207]
**  tempLong = [-82.453883, -78.596814, -78.896814, -82.67500689573649]
*/

var tempLat = [45.700687, 45.700687, 42.50156423966207, 42.50156423966207]
var tempLong = [-82.453883, -78.596814, -78.896814, -82.67500689573649]
var lowLat = 100, highLat = -100, lowLon = 100, highLon = -100;


// map, reduse, filter, observables, redux archt( actions, effects, reducers)

//https://github.com/uber/react-map-gl/tree/master/examples
const Map = ReactMapboxGl({
    accessToken: "pk.eyJ1IjoidGVycmFhbmFseXN0cyIsImEiOiJjanJvM3RpMGgweGU5NDlvbzFvY3JvYTVtIn0.JO2aAeuiRa0YqY_sQsDgcg",
});

var myAPI = new API();

const options = [
  { value: 'Place', label: 'Place' },
  { value: 'Field Name', label: 'Field Name' },
  { value: 'Coordinates', label: 'Coordinates' }
];

Geocode.setApiKey("AIzaSyCUvvd6zM3G2kGtphRdwKdb-ilMWkhXP9o");
//Geocode.enableDebug();

// ----- add to style file
//

const Input = styled.input`
  padding: 0.5em;
  margin: 0.5em;
  color: #2837ff;
  background: #f4feff;
  border: none;
  border-radius: 3px;
`;

const Button = styled.button`
border: 1px solid #3770c6;
height: 100%;
background-color: rgb(84, 152, 255);
color: white;
font-size: 13px;
padding: 6px 12px;
margin: 10px 10px;
border-radius: 6px;
cursor: pointer;
outline: none;
:hover {
  background-color: #3770c6;
}
`;


const DeleteButton = styled.button`
border: 1px solid #3770c6;
height: 100%;
background-color: rgb(84, 152, 255);
color: white;
font-size: 13px;
padding: 6px 12px;
margin: 10px 10px;
border-radius: 6px;
cursor: pointer;
outline: none;
:hover {
  background-color: rgb(255, 0, 0);
}
`;


styled.div`
position: absolute;
Top: 20px;
left: 20px;
right: 20px;
height: 40px;
display: flex;
justify-content: space-between;
align-items: center;
`;

styled.div`
position: absolute;
Bottom: 20px;
left: 20px;
right: 20px;
height: 40px;
display: flex;
justify-content: space-between;
align-items: center;
`;

let boxLinePaint = {
  'line-color': '#ed0707',
  'line-width': 5
};

//0 not owned(green) #00fc2a, 1 owned by someone else(red) ef0000, 2 owned by you(blue) 0300ef

// 0 not owned(green), 1 owned by someone else, but shared (blue), 2 owned by you (green)
let polygonPaint = Map.FillPaint = {
    'fill-color': {
      property: 'field_ownership',
      stops: [
        [0, '#00d118'],
        [1, '#05fffa'],
        [2, '#00d118'],
      ]
     },
    'fill-opacity': 0.5,
    'fill-outline-color': {
      property: 'field_ownership',
      stops: [
        [0, '#00d118'],
        [1, '#0300ef'],
        [2, '#ea05ff'],
      ]
     }
  };
  
  //currently 1km
  let moisture = Map.circlePaint = {
    'circle-color': {
      property: 'sm',
      stops: [
        [0.05, '#ff0000'],
        [0.10, '#ff4200'],
        [0.15, '#ff8000'],
        [0.20, '#ffc900'],
        [0.25, '#ffff00'],
        [0.30, '#e0ff00'],
        [0.35, '#93ff00'],
        [0.40, '#08ff00'],
        [0.45, '#00ff83'],
        [0.50, '#00ffd1'],
        [0.55, '#00d8ff'],
        [0.60, '#00aeff'],
        [0.65, '#0087ff'],
        [0.70, '#0059ff'],
        [0.75, '#0000ff'],
        [0.80, '#3a00ff'],
        [0.85, '#7000ff'],
        [0.90, '#8300ff'],
        [0.95, '#4a008f'],
        [0.99, '#31005f'],
      ]
     },
    'circle-opacity': 1,
    'circle-radius': [
      "interpolate", ["linear"], ["zoom"],
      // zoom is 5 (or less) -> circle radius will be 1px
      5, 40,
      // zoom is 10 (or greater) -> circle radius will be 5px
      10, 50,
      15, 150
  ],
    'circle-blur':1
  };

  // currently 5km ??
  let precipitation = Map.circlePaint = {
    'circle-color': {
      property: 'sm',
      stops: [
        [0.0, '#ffffff'],
        [0.105, '#910076'],
        [0.700, '#800091'],
        [1.5, '#650091'],
        [4.075, '#460091'],
        [5.5, '#1a0091'],
        [6.05, '#000991'],
        [7.5, '#002191'],
        [8.3, '#004391'],
        [8.9, '#005e91'],
        [9.80, '#007891'],
        [10.9, '#009191'],
        [11.40, '#009176'],
        [12.45, '#009152'],
        [13.50, '#00912b'],
        [14.25, '#009104'],
        [15.20, '#219100'],
        [17.8, '#f3f700'],
        [19.80, '#f7a800'],
        [20.85, '#f76600'],
        [22.0, '#f70c00'],
        [25.0, '#ffffff'],
      ]
     },
    'circle-opacity': 1,
    'circle-radius': [
      "interpolate", ["linear"], ["zoom"],
      // zoom is 5 (or less) -> circle radius will be 1px
      5, 20,
      // zoom is 10 (or greater) -> circle radius will be 5px
      10, 150,
      14, 1000
  ],
    'circle-blur':1
  };

  // currently 250m
  let vegetation = Map.circlePaint = {
    'circle-color': {
      property: 'sm',
      stops: [
        [0.0, '#ffffff'],
        [0.300, '#59351b'],
        [0.315, '#96ba96'],
        [1.0, '#003800'],
      ]
     },
    'circle-opacity': 1,
    'circle-radius': [
      "interpolate", ["linear"], ["zoom"],
      // zoom is 5 (or less) -> circle radius will be 1px
      5, 5,
      // zoom is 10 (or greater) -> circle radius will be 5px
      10, 13,
      15, 150,
      20, 200
  ],
    'circle-blur':1
  };

  //currently 1km
  let temp = Map.circlePaint = {
    'circle-color': {
      property: 'sm',
      stops: [
        [200.0, '#a501c1'],
        [230.0, '#a501c1'],
        [250.0, '#00d8ff'],
        [270.0, '#04db19'],
        [298.0, '#e5ed07'],
        [340.0, '#ed0707'],
      ]
     },
    'circle-opacity': 1,
    'circle-radius': [
      "interpolate", ["linear"], ["zoom"],
      // zoom is 5 (or less) -> circle radius will be 1px
      5, 30,
      // zoom is 10 (or greater) -> circle radius will be 5px
      10, 50,
      15, 150,
      20, 200
  ],
    'circle-blur':1
  };

  /*
  9km distance between data points
  'circle-radius': [
      "interpolate", ["linear"], ["zoom"],
      // zoom is 5 (or less) -> circle radius will be 1px
      5, 40,
      // zoom is 10 (or greater) -> circle radius will be 5px
      10, 50
  ]

  */


  const StyledPopup = styled.div`
  background: white;
  color: #3f618c;
  font-weight: 400;
  padding: 5px;
  border-radius: 2px;
`;

const FirstCirclePaint = {
  'circle-stroke-width': 2,
  'circle-radius': [
    "interpolate", ["linear"], ["zoom"],
    // zoom is 5 (or less) -> circle radius will be 1px
    5, 40,
    // zoom is 10 (or greater) -> circle radius will be 5px
    10, 10
  ],
  'circle-opacity': 1,
  //'circle-blur': 0.15,
  'circle-color': '#ff1702',
  'circle-stroke-color': 'green'
};

let DrawPolyPaint = Map.FillPaint = {
  'fill-color': '#ff7505',//'#00fc2a',
  'fill-opacity': 0.7
};

// style file ^^^^^^

  var mapStyles = ['satellite', 'dark', 'basic'];
  var toggleableLayerIds = ['terra-layer','Soil Moisuture', 'layerTesting1', 'layerTesting2'];
  const InitialUserPostion = [-81.338709, 44.316708];

  // will is before did is after - obvious 
class FarmMap extends React.Component<Props, State> {
  constructor(props) {
    super(props);
    this.inputRef = React.createRef();
    this.state = {
      fields: '',
      fieldData: '',
      fieldLoadType: "allFields",
      isLoaded: false,
      loadFieldData: false,
      clicked: false,
      mapCenter: InitialUserPostion,
      featurePos: InitialUserPostion,
      userZoom: [11.5],
      dataName:toggleableLayerIds[1],
      dataValue: "",
      drawState: false,
      drawStateName: "Draw Field",
      needFieldName: false,
      polygon: [],
      geoJsonpoly: [[]],
      btnVariant: "primary",
      userFieldID: 0,
      fieldOwnership: 2,
      listDataFromLayerSelection:null,
      isUserAuthorized:false,
      gotData:false,
      bgColor:'rgb(84, 152, 255)',
      isAlert:false,
      whatAlert:[],
      mapStyle:'mapbox://styles/mapbox/satellite-v9',
      mapStyleID: 1,
      filters: [],
      currentFilter:"",
      circlePaint: moisture,
      selectedOption: { value: 'Place', label: 'Place' },
      optionHint: "Example: Kitchener Ontario",
      isSoilCompacted: false,
      soilCompactedData:"",
      filtersAvailable:false,
      highlightedField: false,
      fieldName: "",
      currentlyGettingFieldData: false,
      fieldsExist: false,


    };
  }


      // Function   : setupBoundaries
      // Description: sets up the boundry for the user creating a new field
      // Paramaters : NULL
      // Returns    : NONE
      setupBoundaries() {
        for(let count = 0; count < tempLat.length; count++){
          if(lowLat > tempLat[count]){
            lowLat = tempLat[count]
            //console.log("are we assigning lowLat?");
          }
          if(highLat < tempLat[count]){
            highLat = tempLat[count]
          }
          if(lowLon > tempLong[count]){
            lowLon = tempLong[count]
          }
          if(highLon < tempLong[count]){
            highLon = tempLong[count]
          }
        }
      }

      componentWillMount() {
        this.setupBoundaries();
        navigator.geolocation.getCurrentPosition(
          ({ coords }: any) => {
            const { latitude, longitude } = coords;


            // this is causing errors
            this.setState({
              mapCenter: [longitude, latitude]
            });
          },
          err => {
            console.error('Cannot retrieve your current position', err);
          }
        );

        myAPI.GetEarthDataTypes().then((dataTypes) =>{
          if(dataTypes != null){
          //console.log("what is the property: ", dataTypes[0].data_name);
          this.setState({
            filters: dataTypes})
          //console.log("Current Datatypes: ", dataTypes, "length: ", dataTypes.length);
          }
        }); 

      }




      // Function   : getFieldInfo
      // Description: this functions sets the current data style after
      //              getting the data and updating the data
      // Paramaters : layerOption : gets the current field data
      // Returns    : NONE
      getFieldInfo(layerOption){
        
        //console.log("Current DataType: ", this.state.currentFilter);
        
        if(this.state.currentFilter !== ""){
          let filter = this.state.currentFilter;
          if(layerOption != null){
            if (layerOption[1] !== this.state.currentFilter){
              filter = layerOption[1];
            }
          }
          this.setState({currentlyGettingFieldData: true});
          myAPI.getFieldData(this.state.userFieldID, filter).then((fieldData) => {
            this.setState({fieldData: fieldData[0]})
            this.setState({
              isLoaded: true, 
              });
            //console.log("getFieldInfo: ", this.state.fieldData)
            if(fieldData[1] !== ""){
              if(this.state.loadFieldData){
                this.setState({
                  isAlert:true,
                  whatAlert:["Error", "Getting field data resulted in: " + fieldData[1],"red"]
                })
              }
              
              
              this.setState({loadFieldData: false})
            }
            else {
              //if(layerOption != null){
                if(this.state.currentFilter.includes('Moisture')){
                  //console.log("else current filter: ",this.state.currentFilter, " WHY IS THIS CURRENT FILTER 'Moisture'??? ");
                  this.setState({
                    circlePaint: moisture,
                    //fieldData: fieldData[0]
                  });
                  this.props.layerTypeCallBack(this.state.currentFilter);
                  
                }
                else if (this.state.currentFilter.includes('Precipitation')){
                  //console.log("else current filter: ",this.state.currentFilter, " WHY IS THIS CURRENT FILTER 'Precipitation'??? ");
                  this.setState({
                    circlePaint: precipitation,
                    //fieldData: fieldData[0]
                  });
                  this.props.layerTypeCallBack(this.state.currentFilter);
                 
                }
                else if (this.state.currentFilter.includes('Temperature')){
                  //console.log("else current filter: ",this.state.currentFilter, " WHY IS THIS CURRENT FILTER 'Temperature'??? ");
                  this.setState({circlePaint: temp})
                  this.props.layerTypeCallBack(this.state.currentFilter);
                  
                }
                else if (this.state.currentFilter.includes('Vegetation')){
                  //console.log("else current filter: ",this.state.currentFilter, " WHY IS THIS CURRENT FILTER 'Vegetation'??? ");
                  this.setState({
                    circlePaint: vegetation,
                    //fieldData: fieldData[0] 
                  })
                  this.props.layerTypeCallBack(this.state.currentFilter);
                  
                }
                else if (this.state.currentFilter.includes('soilCompression') && fieldData[0] === this.state.fieldData){
                  myAPI.getSoilCompaction().then((soilData)=> {
                    if(soilData[1] !== ""){
                      this.setState({
                        isAlert:true,
                        whatAlert:["Error", "Getting soil compaction resulted in:" + soilData[1],"red"]
                      })
                      
                    } else {
      
                      this.setState({
                        soilCompactedData: soilData[0],
                        isSoilCompacted: true
                      });
                      
                    }
                    
                  });
      
                }
                else {
                  this.setState({circlePaint: moisture})
                  //console.log("else current filter: ",this.state.currentFilter, " WHY IS THIS CURRENT FILTER??? ");

                }
      
      
              //}
              if (this.state.fieldsExist === true){
                this.setState({loadFieldData: true})
              }
              
            }
            //console.log("what is this?:  ", this.state.fieldData);

            this.setState({currentlyGettingFieldData: false});
           // console.log("Did we get the error?:  ", fieldData[1]);
          });
          //console.log("curr userFieldID: ", this.state.userFieldID);
        } else {
          this.setState({
            isAlert:true,
            whatAlert:["Tip", "Please select a filter in order to see specific data pertaining to that field", "blue"]
          })

        }
        
        
      }



      // Function   : getFields
      // Description: gets the current fields based on the users choice
      // Paramaters : temp : see if we are loading all fields or my fields
      // Returns    : NONE
      getFields(temp){
        //
        //console.log("temp: ", temp)
        if(temp != null){
          if (temp === "allFields"){
            myAPI.getFields().then((allfields) => {
              //update the state aka field object with the data once it has been returned
                this.setState({
                  fields: allfields[0],
                  isLoaded: true})
                //console.log("what is fields?:  ", allfields);
                if(allfields[1] !== ""){
                  /*this.setState({
                    isAlert:true,
                    whatAlert:["Error", "Getting field data resulted in:" + allfields[1],"red"]
                  })*/
                  this.setState({fieldsExist: false});
                }else {
                  this.setState({fieldsExist: true});
                }
              });
            //console.log("allFields temp: ", temp)
            //console.log("Loading All fields");
          } else if (temp === "myFields"){
            myAPI.getMyFields().then((allfields) => {
              //update the state aka field object with the data once it has been returned
              this.setState({
                fields: allfields[0],
                isLoaded: true})
              if(allfields[1] !== ""){
                /*this.setState({
                  isAlert:true,
                  whatAlert:["Error", "Getting field data resulted in:" + allfields[1],"red"]
                })*/
                
                this.setState({fieldsExist: false});
              }else {
                this.setState({fieldsExist: true});
              }
              //console.log("what is fields?:  ", allfields);
            });
            //console.log("myFields temp: ", temp)
            //console.log("Loading myFields")
          }

        }else if( this.state.fieldLoadType === "allFields" ){
          
          // wait for the data to be returned on the promise from the api call
          myAPI.getFields().then((allfields) => {
          //update the state aka field object with the data once it has been returned
            this.setState({
              fields: allfields[0],
              isLoaded: true})
            if(allfields[1] !== ""){
              /*this.setState({
                isAlert:true,
                whatAlert:["Error", "Getting field data resulted in:" + allfields[1],"red"]
              })*/
              
              this.setState({fieldsExist: false});
            }else {
              this.setState({fieldsExist: true});
            }
          });
          //console.log("Loading All fields");
        } else if (this.state.fieldLoadType === "myFields" ){
          
          myAPI.getMyFields().then((allfields) => {
            //update the state aka field object with the data once it has been returned
            this.setState({
              fields: allfields[0],
              isLoaded: true})
            if(allfields[1] !== ""){
              /*this.setState({
                isAlert:true,
                whatAlert:["Error", "Getting field data resulted in:" + allfields[1],"red"]
              })*/
              
              this.setState({fieldsExist: false});
            }else {
              this.setState({fieldsExist: true});
            }
          });
          //console.log("Loading myFields")
        }

        //console.log("what is fields?:  ", this.state.fields);

      }



      // Function   : checkLoginStatusReactSide
      // Description: checks the login status on the react side then gets the fields
      // Paramaters : NONE
      // Returns    : NONE
      checkLoginStatusReactSide = async () => {

        try {
            let user = await myAPI.checkLoginStatus();
            if (user ) {
                //console.log('user is found: ', user.email);
                //this.getFieldInfo();
                this.getFields();
            }
        } catch(err) {
            console.log('catch | error: ', err);   
        }
    }



      componentDidMount(){
        this.checkLoginStatusReactSide();
        //this.setState({isUser: myAPI.authorizeUser()}, this.fillData())
        if(this.state.filters !== [] && this.state.filtersAvailable ===false ){

          this.setState({filtersAvailable: true});
        }
      }

      componentDidUpdate = (prevState) =>{
        //console.log("whos user: ", this.state.isUser);
        if(this.state.isUser === true && this.state.isLoaded === false && this.state.gotData === false){
          //this.getFieldInfo(); // don't load field data on start, require user to click on a field to easily check if they own it / lazy for now
          this.getFields();
          this.setState({gotData:true});
          //console.log("did update");
        }
      }

      /*onDrawCreate = ({ features }) => {
        //console.log(features);
      };

      onDrawUpdate = ({ features }) => {
        //console.log({ features });
      };
      // doesn't work due to map undefined bug in the wrapper - look into it later
      //You need to install "react-mapbox-gl": "3.9.1". V 4 is not supported.
      /*<DrawControl
                  position="top-left"
                  onDrawCreate={this.onDrawCreate}
                  onDrawUpdate={this.onDrawUpdate}
                />*/

      
      // Function   : onPopup
      // Description: this function deals with the pop on specific data points
      // Paramaters : map, evt : current map and the event that triggered this call
      // Returns    : NONE
      // for current cords - https://github.com/alex3165/react-mapbox-gl/issues/285
      onPopup(map, evt){
        var dataMean = "" 
        if(this.state.currentFilter.includes("Temperature")){
          //T(°C) = 300K - 273.15 = 26.85 °C
          let degrees = 300 - evt.features[0].properties.sm
          dataMean = ' Kelvin || ' + degrees + 'c'
        }
        else if (this.state.currentFilter.includes("Moisture")){
          //https://nsidc.org/data/SPL2SMAP_S
          dataMean = ' cm3/cm3'
        }
        else if (this.state.currentFilter.includes("Precipitation")){
          //https://ghrc.nsstc.nasa.gov/hydro/?q=AMSR2NRT#/details?ds=A2_RainOcn_NRT&_k=41ke4s
          dataMean = ' mm/hr'
        }
        else if (this.state.currentFilter.includes("Vegetation")){
          dataMean = ' Presence Of Chlorophyll'
          //,
          // are depicted in dark green colors, areas with some green leaf growth are in light greens, 
          //and areas with little to no vegetation growth are depicted in tan colors.
        }
        this.setState({
          clicked: true,
          featurePos: evt.lngLat,
          dataValue: evt.features[0].properties.sm + dataMean,
        });
      };
      closePopup(){ 
        this.setState({clicked: false});
      }



      // Function   : toggleDraw
      // Description: this function handles the drawing field button
      // Paramaters : NONE
      // Returns    : NONE
      toggleDraw(){
        if (this.state.geoJsonpoly[0].length < 2 && this.state.drawState){
          this.setState({drawState: false,
            bgColor: "rgb(84, 152, 255)",
            drawStateName: "Draw Field"
          });
        }
       else if(this.state.drawState){
         
          this.setState({needFieldName:true});
          this.setState({drawState: false,
            bgColor: "rgb(84, 152, 255)",
            drawStateName: "Draw Field"
          });

        }
         else {
          this.setState({drawState: true,
            bgColor: "rgb(244, 66, 66)",
            drawStateName: "Confirm Field"
          });

        }
        
      }



      // Function   : placeMarker
      // Description: this function handes placing a new point when drawing a field
      // Paramaters : NONE
      // Returns    : NONE
      placeMarker(map, evt){
        if(this.state.drawState){
          //this.state.arrayvar.concat([newelement]
          let tempCord = this.state.geoJsonpoly.slice();
          let tempLngLat = [evt.lngLat['lng'], evt.lngLat['lat']]
          tempCord[0].push(tempLngLat)
          this.setState({
            feat: this.state.feat + 1,
            polygon: this.state.polygon.concat([evt.lngLat]),
            geoJsonpoly: tempCord,
          });
          //console.log(this.state.polygon);
          //console.log(this.state.geoJsonpoly);

        }

      }

      

      // Function   : handleKeyPress
      // Description: this function handles the search box when a key is pressed looking for the enter key
      // Paramaters : e : the event
      // Returns    : NONE
      handleKeyPress = (e) => {
        if (e.key === 'Enter') {
          //console.log("currentSelection: ", this.state.selectedOption, this.state.selectedOption.value );
          //console.log("what is the e value: ", this.inputRef.current.value);
          // Get address from latidude & longitude.

          if(this.state.selectedOption.value === "Place" || this.state.selectedOption === "Place"){
            // Get latidude & longitude from address.
            Geocode.fromAddress(this.inputRef.current.value).then(
              response => {
                const { lat, lng } = response.results[0].geometry.location;
                this.setState({
                  mapCenter: [lng, lat],
                  userZoom:[14]
                  
                });
                //console.log(lat, lng);
              },
              error => {
                console.error(error);
              }
            );

          }
          else if (this.state.selectedOption.value === "Field Name"){

            //console.log("fieldName current fields: ", this.state.fields)

            for (var tempCount = 0; tempCount < this.state.fields.features.length; tempCount++){
              //&& this.state.fields.features[tempCount].properties.field_ownership === 2 we only see our fields or our shared fields now
              if(this.state.fields.features[tempCount].properties.field_name === this.inputRef.current.value ){
                //console.log("Trying to Changing cords");
                var currCords = this.state.fields.features[tempCount].geometry.coordinates[0][0];
                this.setState({
                  mapCenter: currCords,
                  userZoom:[14]
                });
                break;

              }
              else {
                //console.log("Can't to Changing cords: ", this.state.fields.features[tempCount].properties.field_name);
              }

            }


          }
          else if (this.state.selectedOption.value === "Coordinates"){
            let letCords = this.inputRef.current.value;
            var tempCords = letCords.split(',');

            // lat, lng
            // 40.12312 , -80.3114132
            Geocode.fromLatLng(tempCords[0], tempCords[1]).then(
              response => {
                const address = response.results[0].formatted_address;
                //console.log(address);
                this.setState({
                  mapCenter: [tempCords[1], tempCords[0]],
                  userZoom:[14]
                });

              },
              error => {
                console.error(error);
              }
            );

          }
          else {
            console.error("error, Coordinates");
          }
         
        }
      }


      // Function   : dateSpecificData
      // Description: this function handles getting date specific data
      // Paramaters : layerDateData : current layer data
      // Returns    : NONE
      dateSpecificData = (layerDateData) => {

        //console.log(layerDateData);
        this.setState({currentlyGettingFieldData: true});
        myAPI.getFieldDataByDate(layerDateData, this.state.userFieldID).then((fieldDataDate)=>{
          
            if(fieldDataDate[1] !== ""){
              if(this.state.loadFieldData){
                this.setState({
                  isAlert:true,
                  whatAlert:["Error", "Getting field data resulted in: " + fieldDataDate[1],"red"]
                })
              }
              
              
              this.setState({loadFieldData: false})
              
            } else {
              this.setState({fieldData: fieldDataDate[0]})
              this.setState({isLoaded: true});
            }

            this.setState({currentlyGettingFieldData: false});
        });

      }

      layerSelectionCallback = (dataFromLayerSelec) => {
        if(dataFromLayerSelec[0] === "fieldQuery" && dataFromLayerSelec[1] !== this.state.fieldLoadType){
          
          this.setState({
            fieldLoadType: dataFromLayerSelec[1],
            
            gotData:false,
          })


          this.getFields(dataFromLayerSelec[1]);
          

          
          //console.log("loading fields");
        } else if (dataFromLayerSelec[0] === "layer"){

          if( this.state.currentFilter !== dataFromLayerSelec[1]){
            this.setState({currentFilter: dataFromLayerSelec[1]});
            
            if(this.state.loadFieldData){
              this.getFieldInfo(dataFromLayerSelec);
            }


          }
          // we need to check if the user has currently clicked on a field - loadFieldData
          //console.log("data layer selection: ", dataFromLayerSelec[1]);
          


          
          //this.getFieldInfo(dataFromLayerSelec[1]);
          

        }
        //this.setState({listDataFromLayerSelection:dataFromLayerSelec});

        

        //console.log("layer selection: ", dataFromLayerSelec, this.state.fieldLoadType);
      }


      // Function   : fillData
      // Description: fills the fields
      // Paramaters : NONE
      // Returns    : NONE
      fillData() {
        //console.log("callback worked");
        if(this.state.isUser === true){
          //this.getFieldInfo();
          this.getFields();
        }

      }




      // Function   : fieldNameCallBack
      // Description: once the new field has been named it goes through this function to ensure it
      //              meets the requirements
      // Paramaters : fieldNameData
      // Returns    : NONE
      fieldNameCallBack = (fieldNameData) =>{
        this.setState({needFieldName:false});
        //console.log("fieldNameCallBack results: ", fieldNameData, this.state.polygon.length);
        //the user wishes to confirm the field with the name they've given
      
        if (fieldNameData[0] === true && this.state.geoJsonpoly[0].length > 2){
          var insideBox = true
          //console.log("checking each poly: ", this.state.geoJsonpoly[0]);
          //console.log("lowLat , highLat, lowLon, highLon: ", lowLat, highLat, lowLon, highLon)
          // need to check if the field is within the boundries
          //for (let i = 0; i < this.props.filters.length; i++) {
          for (let polyCount = 0; polyCount < this.state.polygon.length; polyCount++ ){
            //lowLat, highLat, lowLon, highLon = 0;
            if (this.state.polygon[polyCount].lat > lowLat && this.state.polygon[polyCount].lat < highLat &&this.state.polygon[polyCount].lng > lowLon && this.state.polygon[polyCount].lng < highLon){

            }
            else {
              insideBox = false;
            }
            //console.log("checking each poly point: ", this.state.polygon[polyCount]);
            
          }

          if(!insideBox){
            // error user is attempting to draw outside the boundries
            this.setState({
              polygon:[],
              geoJsonpoly:[[]],
              isAlert:true,
              whatAlert:["Error","Fields are required to be inside the red outline!", "red"]
            })
          }
          else {

            //console.log("newFieldName: ", fieldNameData[1]);
            let data = {fieldID: this.state.userFieldID, coordinates: this.state.polygon, fieldName: fieldNameData[1]};

            // we need to now make the api call to insert the field
            myAPI.insertField(data).then((response) => {
              if(response != null){

                this.setState({drawState: false,
                  bgColor: "rgb(84, 152, 255)",
                  drawStateName: "Draw Field",
                  isAlert:true,
                  whatAlert:["Success","You have created a new field!","green"]
                });
                this.getFields();

              }
              else {
                this.setState({drawState: false,
                  bgColor: "rgb(84, 152, 255)",
                  drawStateName: "Draw Field",
                  isAlert:true,
                  whatAlert:["Error","Failed to create field! Field name is already in use by you.","red"]
                });
              }
              
              
            }
            );

            this.setState({
              polygon:[],
              geoJsonpoly:[[]],
            })

          }

        }
        else if (fieldNameData[0] === true){

          
          // the polygon isn't big enough so we need to alert the user that their 
          // field needs to be more then 1 point to be able to create one
          this.setState({
            polygon:[],
            geoJsonpoly:[[]],
            isAlert:true,
            whatAlert:["Error","Fields requires more then 1 point!", "red"]
          })



        }
        else if (fieldNameData[0] === false){

          this.setState({
            polygon:[],
            geoJsonpoly:[[]],
          });
        }



      }


      // Function   : MapAlertCallBack
      // Description: fills the fields
      // Paramaters : MapAlertData
      // Returns    : NONE
      MapAlertCallBack = (MapAlertData) => {
        //console.log("mapalertCallBack");
        this.setState({
          isAlert:false,
          whatAlert:[]
        })
      }



      // Function   : SelectField
      // Description: this function handles selecting the field based on if it was clicked on
      //              the map
      // Paramaters : MapAlertData
      // Returns    : NONE
      SelectField(map, evt){
        //if(evt.features[0].properties.field_ownership === 2){
        if(evt.features[0].properties.field_name === this.state.fieldName && this.state.loadFieldData === true){

        } else {
          this.setState({
            fieldName: evt.features[0].properties.field_name,
            userFieldID: evt.features[0].properties.id,
            highlightedField: true
          });
          this.getFieldInfo();
          var fieldInfo = {}
          fieldInfo.id = evt.features[0].properties.id
          fieldInfo.name = evt.features[0].properties.field_name
          fieldInfo.fieldOwnership = evt.features[0].properties.field_ownership;
          fieldInfo.currentFilter = this.state.currentFilter;
          this.setState({fieldOwnership: evt.features[0].properties.field_ownership})
          this.props.mapCallBack(fieldInfo);
        }
        
        /*} else {
          this.setState({
            isAlert:true,
            whatAlert:["Error","To see field data you are required to own it", "red"],
            highlightedField: false
          })
        }*/
      }



      // Function   : nextStyle
      // Description: this function handles changing the style of the map
      // Paramaters : NONE
      // Returns    : NONE
      nextStyle(){
        //console.log("current style:", this.state.mapStyleID)
        var nextMap = "mapbox://styles/mapbox/" + mapStyles[this.state.mapStyleID] +"-v9";
        let count = this.state.mapStyleID;
        count = count + 1;
        if (count > 2){
          count = 0;
        }
        this.setState({
          mapStyle:nextMap,
          mapStyleID: count,
        })
        //console.log(this.state.mapStyle)
      }


      // Function   : handleChange
      // Description: this function handles changing of the search options
      // Paramaters : NONE
      // Returns    : NONE
      handleChange = (currOption) => {
        

        if(currOption.value === "Place"){
          
          this.setState({optionHint: "Example: Kitchener Ontario"});

        }
        else if (currOption.value === "Coordinates"){
          this.setState({optionHint: "Coordinates Format: latidude, longitude"});
          
        }
        else {
          this.setState({optionHint: ""});

        }
        //console.log(`Option selected:`, currOption);

        this.setState({ selectedOption: currOption });
      }


      // Function   : deleteField
      // Description: this function handles deleting a field
      // Paramaters : NONE
      // Returns    : NONE
      deleteField() {
        if (this.state.fieldOwnership === 2){
          myAPI.DeleteField(this.state.fieldName).then((result)=> {

            if(result[1] !== ""){
              this.setState({
                isAlert:true,
                whatAlert:["Error", "Deleting the field resulted in:" + result[1],"red"]
              })
              
            } else {
              this.getFields();
              this.setState({loadFieldData:false,
                highlightedField: false
              });
            let fieldInfo = {};
            fieldInfo.id = "No Field"
            fieldInfo.name = ""
            fieldInfo.fieldOwnership = "";
            fieldInfo.currentFilter = this.state.currentFilter;
            this.props.mapCallBack(fieldInfo);

            }

          });
        } else {
          this.setState({
            isAlert:true,
            whatAlert:["Error", "You cannot delete another user's field","red"]
          })
        }
      }

      

      /*<div>
              <font color="white">temp input: field ID</font>
              <input onKeyPress={event => {
                if (event.key === 'Enter') {
                  this.setState({
                    userFieldID: event.target.value
                  });
                  this.getFieldInfo();
                }
              }}/>
               
                
            </div>*/

            // search bar https://github.com/JedWatson/react-select
            //https://www.npmjs.com/package/react-geocode
    render() {
      var {isLoaded, fieldData, fields, mapCenter, featurePos, userZoom, dataValue, polygon, drawStateName,
         mapStyle, circlePaint, selectedOption, optionHint} = this.state;

        if(!isLoaded) {
          return<div ><font color="white">Loading...</font></div>;

        } else {

          return (
            <div>
              <div className="RowD">

                <div className="ToolBarSpacing">
                  <Button style={{backgroundColor:this.state.bgColor}} onClick={this.toggleDraw.bind(this)}> {drawStateName}</Button>
                </div>
                <div className="ToolBarSpacing">
                  <Button  onClick={this.nextStyle.bind(this)}>Change style</Button>
                </div>
                <h7> <font color="white" >Search:</font></h7>  
                <div className="SearchOptions">
                
                  <Select
                    placeholder="Search Options"
                    defaultValue = {selectedOption}
                    value={selectedOption}
                    onChange={this.handleChange}
                    options={options}
                  />
                </div> 
                <div className="ToolBarSpacing">
                  <Input ref={this.inputRef} onKeyPress={this.handleKeyPress} placeholder="Start your search..."  ></Input>
                   <h7><font color="white" >{optionHint}</font></h7>
                </div>
                {(this.state.highlightedField) ?
                <div className="ToolBarSpacing">
                  <DeleteButton Color="danger" onClick={this.deleteField.bind(this)}>Delete Field</DeleteButton>
                </div>
                : null }
                
                

               </div>
            <div className="RowC" style={{ height: 750, width: 1330 }}>
            
            <div className="mapSpacing" style={{ height: 750, width: 1095 }}>
              {(this.state.isAlert) ?
                <MapAlert message={this.state.whatAlert} callBack={this.MapAlertCallBack}></MapAlert> 
                : null
              }
              
              {(this.state.needFieldName) ?
                <NamingPopup callBack={this.fieldNameCallBack}></NamingPopup> 
                : null
              }

              
             
              <Map
                style={mapStyle}
                center= {mapCenter}
                zoom={userZoom}
                containerStyle={{
                  height: "77vh",
                  width: "56vw"
                }}
                onClick={this.placeMarker.bind(this)}>
              >
                
                <ScaleControl />
                <ZoomControl />
                <RotationControl style={{ top: 90 }} />

                <GeoJSONLayer
                  data={BoxIn}
                  linePaint={boxLinePaint}
                  sourceOptions = {{
                  'type': 'geojson'
                  }}
                />

                {(this.state.fieldsExist) ?
                  <GeoJSONLayer
                  type="fill"
                  data={this.state.fields}
                  fillOnClick={this.SelectField.bind(this, fields.getCurrentPosition)}
                  fillPaint={polygonPaint}
                  sourceOptions = {{
                  'type': 'geojson'
                  }}
                />
                
                : null}
                
                {(this.state.loadFieldData) ? 
                  <GeoJSONLayer
                  type="circle"
                  data={this.state.fieldData}
                  circlePaint={circlePaint}
                  circleOnClick={this.onPopup.bind(this, fieldData.getCurrentPosition)}
                  sourceOptions = {{
                  'type': 'geojson'
                  }}
                  />
                :null}
                

                {(this.state.isSoilCompacted) ? 
                  <GeoJSONLayer
                  type="circle"
                  data={this.state.soilCompactedData}
                  circlePaint={circlePaint}
                  circleOnClick={this.onPopup.bind(this, fieldData.getCurrentPosition)}
                  sourceOptions = {{
                  'type': 'geojson'
                  }}
                  />
                :null}
                

                {(this.state.drawState && polygon.length >= 1) ?
                  
                  <Layer type="circle" paint={FirstCirclePaint}><Feature coordinates={this.state.geoJsonpoly[0][0]}/></Layer>
                  
                  :null }

                {(this.state.drawState && polygon.length > 1) ?       
                  //https://stackoverflow.com/questions/52121763/react-mapbox-polygon-layer
                  <Layer type="fill" paint={DrawPolyPaint}>
                    <Feature coordinates={this.state.geoJsonpoly} />
                  </Layer>

                : null}


                {this.state.clicked && (
                  <Popup
                    coordinates={featurePos}
                    offset={{
                    'bottom-left': [0, 0],  'bottom': [0, 0], 'bottom-right': [0, 0]
                    }}
                    onClick={this.closePopup.bind(this)}
                  >
                    {console.log("popup")}
                      <StyledPopup>
                        <div >Data: {this.state.currentFilter}
                        </div>
                        <div>
                          Value: {dataValue} 
                        </div>
                      </StyledPopup>
                  </Popup>
                )}
                

              </Map>
              
            </div>
            {(this.state.filtersAvailable) ? 
               <div style={{ height: 750, width: 175 }}>
              <LayerSelection filters={this.state.filters} callBack={this.layerSelectionCallback} dateData={this.dateSpecificData} isGettingData={this.state.currentlyGettingFieldData}></LayerSelection>
              </div>
            : null}
            
            </div>
            </div>
      );


        }
      
    }
  }

  
  export default FarmMap;