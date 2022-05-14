/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  dashboard.js                                                                                |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file the main component for the dashboard which holds the                 |
|                 map and simulation                                                             |
*************************************************************************************************/

import * as React from 'react';
import Farmmap from './farmMap'
import FarmSimulation from './farmSimulation';
import fire from './../config/fire';
import styled from 'styled-components';
import {Rectangle} from 'react-shapes';
import API from './api';

var myAPI = new API();

const TopBar = styled.div`
position: absolute;
width: 100%;
Top: 5px;
right: 30px;
height: 40px;
display: flex;
justify-content: center;
align-items: center;
`;

const Button = styled.button`
border: 1px solid #3770c6;
height: 100%;
background-color: rgb(0, 142, 45);
color: white;
font-size: 13px;
margin-right:40px;
padding: 6px 12px;
border-radius: 6px;
cursor: pointer;
outline: none;
:hover {
  background-color: #00ad36;
}
`;

const legendSizeWidth = 10;
const legendSizeHeight = 20;

class Footer extends React.Component{
  constructor(props){
    super(props);
    this.state = {
      name:{},
      isMap:true,
      weatherCondition:'',
      currentFarm:{},
      currentFieldName:"",
      fieldSize: 0,
      fieldSpecifics: {},
      currentFarmHaveInfo: false,
      soilMoisture: false,
      precipitation: false,
      vegetation: false,
      temperature: false,
      ownField: false,


    }
  }

  componentDidMount() {
    this.forceUpdate();
    window.scrollTo(0, 0);
    fetch(`https://api.apixu.com/v1/current.json?key=7d520ec9f642480b8fd13651192403&q=Kitchener-Waterloo`)
        .then((response) => {
            // Examine the text in the response
            response.json().then((data) => {
              this.setState({
                weatherCondition: data.current.condition.text
              })
            });
          }
        ).catch(function(err) {
          console.log('Fetch Error :-S', err);
        });
  }

  componentWillMount() {
    fire.auth().onAuthStateChanged((user) => {
      if (user) {
        this.state.name = user.displayName
        //console.log(this.state.name);
      }
      
    });
  }

  ComponentChange = name => event => {
    /*this.setState(prevState => ({
      isMap: !prevState.isMap
    }));*/
    if(name === "map"){
      this.setState({isMap:true});
    } else if (name === "simulation"){
      this.setState({isMap:false});
    }

  };

  currentLayerType = (currentLayerType) => {
    //console.log("currentLayerType: ", currentLayerType);
    if(currentLayerType.includes('Moisture') ){
      //console.log("Current Field Type Dashboard: ", currentLayerType);
        this.setState({
          soilMoisture: true,
          precipitation: false,
          temperature: false,
          vegetation: false
        })
    }
    else if (currentLayerType.includes('Precipitation')){
      //console.log("Current Field Type Dashboard: ", currentLayerType);
      this.setState({          
        soilMoisture: false,
        precipitation: true,
        temperature: false,
        vegetation: false
      })
    }
    else if (currentLayerType.includes('Temperature')){
     // console.log("Current Field Type Dashboard: ", currentLayerType);
      this.setState({
        soilMoisture: false,
        precipitation: false,
        temperature: true,
        vegetation: false
      })
    }
    else if (currentLayerType.includes('Vegetation')){
      //console.log("Current Field Type Dashboard: ", currentLayerType);
      this.setState({
        soilMoisture: false,
        precipitation: false,
        temperature: false,
        vegetation: true
      })
    }

  }

  currentfield = (currentfield) => {
    //console.log("current dashboard: ", currentfield);
    //https://www.farmfoodcareon.org/wp-content/uploads/2017/05/Fact-Sheet-Field-Crop-2016.pdf
    if(currentfield[0] == null && currentfield["id"] == null){
      //nope
    }
    else if ( currentfield["id"] == "No Field"){
      this.setState({
        soilMoisture: false,
        precipitation: false,
        temperature: false,
        vegetation: false,
        currentFarmHaveInfo: false,
        ownField: false,

      })
    }

    if (currentfield[0] != null){
      
    }
    else {
      if ( currentfield.currentFilter != null){
        if(currentfield.currentFilter.includes('Moisture') ){
          //console.log("Current Field Type Dashboard: ", currentfield.currentFilter);
            this.setState({
              soilMoisture: true,
              precipitation: false,
              temperature: false,
              vegetation: false
            })
        }
        else if (currentfield.currentFilter.includes('Precipitation')){
          //console.log("Current Field Type Dashboard: ", currentfield.currentFilter);
          this.setState({          
            soilMoisture: false,
            precipitation: true,
            temperature: false,
            vegetation: false
          })
        }
        else if (currentfield.currentFilter.includes('Temperature')){
          //console.log("Current Field Type Dashboard: ", currentfield.currentFilter);
          this.setState({
            soilMoisture: false,
            precipitation: false,
            temperature: true,
            vegetation: false
          })
        }
        else if (currentfield.currentFilter.includes('Vegetation')){
          //console.log("Current Field Type Dashboard: ", currentfield.currentFilter);
          this.setState({
            soilMoisture: false,
            precipitation: false,
            temperature: false,
            vegetation: true
          })
        }
      }
      

      //console.log("Dashboard current field: ", currentfield);
      this.setState({currentFieldName: currentfield.name})
      


      /*myAPI.getFieldArea(currentfield.name).then((area)=> {
        if(area[1] === ""){
          this.setState({fieldSize: area[0]});
        }
        
      });*/
      if(currentfield.fieldOwnership != null){
        if(currentfield.fieldOwnership === 2){
          myAPI.getFieldSpecifics(currentfield).then((currFieldSpecifics) => {
            if(currFieldSpecifics[1] === ""){
              this.setState({
                currentFarm: currFieldSpecifics[0],
                currentFarmHaveInfo: true
              })
              /*
                entry_date: "2019-03-25T00:00:00"
                fertilizer_use: "human poop"
                field_id: 37
                pesticide_use: "no"
                seedPlanted: "Corn"
                specifics_id: 9
                yield: 10
              */
            }
            else {
              this.setState({
                currentFarmHaveInfo: false
              })
            } 
            
          });

          myAPI.getFieldArea(currentfield.name).then((area)=> {
            if(area[1] === ""){
              this.setState({fieldSize: area[0]});
            }
            
          });

          this.setState({ownField:true});

        }
        else {
          this.setState({
            ownField:false,
            currentFarmHaveInfo: false});
        }
      }
      
      //console.log("Current field info: ", this.state.currentFarm, ", ", this.state.fieldSize, ", ", currentfield.fieldOwnership); 
      
    }
    
   
  }

  render() {
    let weatherCondition;
    let userName;
    weatherCondition = this.state.weatherCondition
    userName = this.state.name + "'s";
    //console.log(weatherCondition)
    return (
      <React.Fragment>

      {(this.state.isMap) ? 

        <div style={{backgroundColor: "#222"}} className="dashboard">
            <div className="cards">
              <h2>{ userName } <br></br>FarmLand Information</h2>
              <p><font color="grey">_____________</font></p>
              {this.state.weatherCondition ? <font color="white"><h5>Weather Condition: { weatherCondition }</h5> </font>: <font color="white"><h5>Weather Condition: Please Enter a Valid City</h5></font>  }
              <br></br>
              <h8><font color="white">Field Name: {this.state.currentFieldName}</font></h8><br></br>
              {(this.state.ownField) ? 
                <h8><font color="white">
                  <ul className="fieldArea">
                  Field Size:
                    <li>{this.state.fieldSize} sqm</li> 
                    <li>{(this.state.fieldSize * 0.00024710538146717).toFixed(2)} Acres</li>
                  </ul> 
                  </font>
                </h8> 
              : null }
              {(this.state.currentFarmHaveInfo) ?
                <h8><font color="white">
                  
                  <ul className="fieldTable">
                    Last Field Entry:
                      <li>Yield: {this.state.currentFarm[0].yield}</li>
                      <li>Seed: {this.state.currentFarm[0].seedPlanted}</li>
                      <li>Fertilizer: {this.state.currentFarm[0].fertilizer_use}</li>
                      <li>Pesticides: {this.state.currentFarm[0].pesticide_use}</li>
                    </ul>
                </font></h8>
              : null }
            </div>
        </div>
        : null }

        {(this.state.isMap) ? 
          <div style={{backgroundColor: "#222"}} className="dashboardLegend">
            <div className="legend">
              <div className="cards">
                <h2>Map Legend</h2>
                <Rectangle width={10} height={10} fill={{color:'#05fffa'}} />
                &nbsp;
                <h7 style={{color:"#fff", textAlign:"center"}}>Cyan Field Layer - Field Owned by Another User</h7>
                <br></br>
                <Rectangle width={10} height={10} fill={{color:'#00d118'}} />
                &nbsp;
                <h7 style={{color:"#fff", textAlign:"center"}}>Green Field Layer - User Owned Field</h7>
                <br/><br/>
                { (this.state.soilMoisture) ? 
                <div1>
                    <div1>
                      <h2>Soil Mosture Gradient</h2>
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ff4200'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ff8000'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ffc900'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ffff00'}} />            
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#e0ff00'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#93ff00'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#08ff00'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#00ff83'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#00ffd1'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#00d8ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#00aeff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#0087ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#0059ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#0000ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#3a00ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#7000ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#8300ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#4a008f'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#31005f'}} /> 
                    </div1>
                    <div><h7 style={{color:"#fff", textAlign:"center"}}>0.05 cm3/cm3 - 0.99 cm3/cm3 </h7 ></div>
                  </div1>
                  
                  :
                  null }
                  { this.state.precipitation ? 
                  <div1>
                    <div1>
                      <h2>Precipitation Gradient</h2>
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#910076'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#800091'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#650091'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#460091'}} />            
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#1a0091'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#000991'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#002191'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#004391'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#005e91'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#007891'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#009191'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#009176'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#009152'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#00912b'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#009104'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#219100'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#f3f700'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#f7a800'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#f76600'}} /> 
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#f70c00'}} /> 
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ffffff'}} /> 
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ffffff'}} /> 

                    </div1>
                    <div><h7 style={{color:"#fff", textAlign:"center"}}>0.105 mm/hr - 25.0 mm/hr</h7 ></div>
                  </div1>
                  :
                  <div1>              
                  </div1> }
                  { this.state.vegetation ? 
                  <div1>
                    <div1>
                      <h2>Vegetation Gradient</h2>
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ffffff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#59351b'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#96ba96'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#003800'}} />
                    </div1>
                    <div><h7 style={{color:"#fff", textAlign:"center"}}>0.00 POC - 1.0 POC </h7 ></div>
                    <div><h7 style={{color:"#fff", textAlign:"center"}}>Presence Of Chlorophyll</h7 ></div>
                    
                    <br></br>
                    <h7 style={{color:"#fff", textAlign:"center"}}> Areas with a lot of green leaf growth, indicates the presence of chlorophyll which reflects more infrared light and less visible light</h7>
                    
                  </div1>
                  :
                  <div1>              
                  </div1> }
                  { this.state.temperature ? 
                  <div1>
                    <div1>
                      <h2>Temperature Gradient</h2>
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#a501c1'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#a501c1'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#00d8ff'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#04db19'}} />            
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#e5ed07'}} />
                      <Rectangle width={legendSizeWidth} height={legendSizeHeight} fill={{color:'#ed0707'}} />
                    </div1>
                    <div><h7 style={{color:"#fff", textAlign:"center"}}>-73.15 c - 66.85 c </h7></div>
                    <h5 style={{color:"#fff", textAlign:"center"}}> </h5>
                  </div1>
                  :
                  <div1>              
                  </div1> }
                  <br/> 
              </div>
            </div>
          </div>
      
        : null }


        <section className="map">
        <div className="RowC" style={{ width: 1330 }}>
          <div className="mapSpacing" style={{ width: 1095 }}>
            <TopBar>
              <Button onClick={this.ComponentChange("map")} name="map">MapBox</Button>
              <Button onClick={this.ComponentChange("simulation")} name="simulation">Simulation</Button>
            </TopBar>
          </div>
          <div style={{width: 150}}></div>
        </div>
          
          
          {(this.state.isMap) ? 
              <Farmmap mapCallBack = {this.currentfield} layerTypeCallBack = {this.currentLayerType}>

              </Farmmap>
            : null}
          {(!this.state.isMap) ? 
              <FarmSimulation>

              </FarmSimulation>
            : null}
          
          


        </section>
        
        
      </React.Fragment>
    );
  }
}

export default Footer;

