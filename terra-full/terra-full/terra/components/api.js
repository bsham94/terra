/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  api.js                                                                                |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality for the api calls to function         |
*************************************************************************************************/


import Service from './service.js';
import React from 'react';
import fire from './../config/fire';
import Auth from './auth.js'
import tempFieldData from "./../assets/defaults/SoilMoisture.geojson";
import tempFields from "./../assets/defaults/farmfield.geojson";



var fieldData = tempFieldData;//require('./../assets/defaults/SoilMoisture.geojson');
var fields = tempFields; //require('./../assets/defaults/farmfield.geojson');
let massiveError = "Massive error, contact the developer for assistants.";

class API extends React.Component{
    constructor(props) {
        super(props);
        this.state = {
          userID:{},
          userToken:"",

        }
        

        // Function   : fire.auth().onAuthStateChanged
        // Description: Gets a current users id and sets it's state
        // Paramaters : NULL
        // Returns    : user: current user object.
        fire.auth().onAuthStateChanged((user) => {
            if (user) {
                this.state.userID = user.uid;
                //this.setState({userID: user.uid});

                //DEBUG
                //console.log("user id is set");
            } else {
                this.setState({ user: null });
                //localStorage.removeItem('user');

                //DEBUG
                //console.log("Error getting userID");
            }
        });
      }

    // Function   : getWeatherAlerts
    // Description: Gets a current weather status
    // Paramaters : NULL
    // Returns    : user: current user object.
    // Resources  : https://weather.cit.api.here.com/weather/1.0/report.json?product=alerts&name=Inukjuak&app_id=DemoAppId01082013GAL&app_code=AJKnXv84fjrb0KIHawS0Tg
    getWeatherAlerts() {
       fetch('http://api.apixu.com/v1/current.json?key=7d520ec9f642480b8fd13651192403&q=Kitchener')
        .then((response) => {
            // Examine the text in the response
            response.json().then((data) => {
              return data;
            });
          }
        ).catch(function(err) {
          console.log('Fetch Error :-S', err);
        });

    }


    // Function   : GetFieldSpecifics
    // Description: Gets a current weather status
    // Paramaters : fieldInterest: current selected fieldID
    // Returns    : data: the field specfic data
    //              eMsg: error messaged if error occured
    GetFieldSpecifics(fieldInterest){

        var apiService = new Service({url:'field'});

        apiService.createEntity({name: 'EarthData'});
        var data = {}

        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.EarthData.getFieldSpecifics( {fieldID: fieldInterest}, {type: ""})
    
        .then(function (response){
            // stringify then re-pack as json works
            data = response.data//structure.repack(response.data)

            //debug
            //console.log("fieldData: ", fieldData);
            return [data,""]
            
        })
        .catch(function (error){
            //debug
            data = {};
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Field specifics: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            return [data,eMsg]
        });



    }



    // Function   : updateFieldName
    // Description: Updates the field name
    // Paramaters : currFieldID, newFieldName: field info required to update field name
    // Returns    : data: the field specfic data
    //              eMsg: error messaged if error occured
    //examples
    //http://localhost:54357/api/field
    //{user_id:"",field_name:"",new_field_name:""}
    updateFieldName(currFieldID, newFieldName){
        var localConfig = {}

        localConfig.user_id = this.state.userID;
        localConfig.field_name = currFieldID;
        localConfig.new_field_name = newFieldName; 

        var structure = new StructureData();
        var apiService = new Service({url:'field'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.basicUpdate(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data.status,""]
            
        })
        .catch(function (error){

            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error updating field name: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });

    }



    // Function   : getSoilCompaction
    // Description: Updates the field name
    // Paramaters : NONE
    // Returns    : response.data: soil compaction info
    //              eMsg: error messaged if error occured
    //http://localhost:54357/api/field/SoilCompaction/Get
    //{field_id:4,soilCompaction:"",linestring:""}
    getSoilCompaction(){
        var localConfig = {}

        localConfig.field_id = 4;
        localConfig.soilCompaction = "";
        localConfig.linestring = ""; 

        var structure = new StructureData();
        var apiService = new Service({url:'SoilCompaction/Get'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){

            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting soil compaction data: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });


    }



    // Function   : getfieldDataDates
    // Description: gets all the available days with current dataset
    // Paramaters : handlerType: the current filter type
    // Returns    : response.data: soil compaction info
    //              eMsg: error messaged if error occured
    //http://localhost:59935/api/field/EarthData/Dates/All
    //{"DataType":"precipitation_handler"}
    getfieldDataDates(handlerType){
        var localConfig = {}

        localConfig.dataType = handlerType;

        var structure = new StructureData();
        var apiService = new Service({url:'field/EarthData/Dates/All'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting availabe data by dates: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData, eMsg]
        });


    }



    // Function   : getFieldDataByDate
    // Description: gets field data by date
    // Paramaters : dataName, field_id: current field name and field id
    // Returns    : response.data: soil compaction info
    //              eMsg: error messaged if error occured
    //EarthData/Dates params: {"id":"37",type:"soil_moisture"}
    getFieldDataByDate(dataName, field_id){
        var localConfig = {}

        localConfig.id= field_id;
        localConfig.type = dataName;

        var structure = new StructureData();
        var apiService = new Service({url:'field/EarthData/Dates'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Field Data by date: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,"No Data Found Within This Field..."]
        });


    }


    // Function   : getFieldSpecifics
    // Description: gets field specifics
    // Paramaters : dataName, field_id: current field name and field id
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    //ex data {field_id:37,user_id:"bSznI8kKorVztbyyslrr2PTrevO2",field_cords:"",field_name:"fhgvv"}
    getFieldSpecifics(currField){

        var localConfig = {}

        localConfig.field_id = currField.id;
        localConfig.user_id = this.state.userID;
        localConfig.field_cords = "";
        localConfig.field_name = currField.name; 

        var structure = new StructureData();
        var apiService = new Service({url:'FieldSpecifics'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Field Data specifics #2?: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });


    }



    // Function   : updateFieldSpecifics
    // Description: updates field specifics
    // Paramaters : currField, fieldSpecifics: current field name and field specifics to update
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    //FieldSpecifics
    //ex data {specifics_id:0,field_id:1,yield:0.0,seedPlanted:"",fertilizer_use:"",pesticide_use:""}
    //field_id, yield, seedPlanted, fertilizer_use, pesticide_use
    // currField = field_id
    /*fieldSpecifics { 
        fieldSpecifics.yield
        fieldSpecifics.seedPlanted
        fieldSpecifics.pesticide_use
    }*/
    updateFieldSpecifics(currField, fieldSpecifics){
        //console.log("chicken shit: ", fieldSpecifics);
        var localConfig = {}
        localConfig.user_id = this.state.userID;
        localConfig.field_name = currField;
        localConfig.yield = fieldSpecifics.yield;
        localConfig.seedPlanted = fieldSpecifics.seedsPlanted;
        localConfig.fertilizer_use = fieldSpecifics.fertilizer_use;
        localConfig.pesticide_use = fieldSpecifics.pesticide_use;


        var structure = new StructureData();
        var apiService = new Service({url:'FieldSpecifics'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.basicUpdate(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Updating Field Specifics: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });


    }



    // Function   : deleteFieldSpecifics
    // Description: deletes field specifics
    // Paramaters : currField, fieldSpecifics: current field name and field specifics to delete
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    //FieldSpecifics
    //ex data {specifics_id:0,field_id:1,yield:0.0,seedPlanted:"",fertilizer_use:"",pesticide_use:""}
    //field_id, yield, seedPlanted, fertilizer_use, pesticide_use
    //
    deleteFieldSpecifics(currField, fieldSpecifics){

        var localConfig = {}
        localConfig.user_id = this.state.userID;
        localConfig.field_name = currField;
        localConfig.yield = fieldSpecifics.yield;
        localConfig.seedPlanted = fieldSpecifics.seedPlanted;
        localConfig.fertilizer_use = fieldSpecifics.fertilizer;
        localConfig.pesticide_use = fieldSpecifics.pesticide_use;


        var structure = new StructureData();
        var apiService = new Service({url:'FieldSpecifics'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.basicDelete(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){

            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Deleting Field Specifics: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });


    }



    // Function   : insertFieldSpecifics
    // Description: creates a new field specifics
    // Paramaters : currField, fieldSpecifics: current field name and field specifics to create
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    //FieldSpecifics
    //ex data {specifics_id:0,field_id:1,yield:0.0,seedPlanted:"",fertilizer_use:"",pesticide_use:""}
    //field_id, yield, seedPlanted, fertilizer_use, pesticide_use
    //
    insertFieldSpecifics(currField, fieldSpecifics){

        //console.log("InsertFieldSpecifics Params:", currField," ", fieldSpecifics);
        var localConfig = {}
        localConfig.user_id = this.state.userID;
        localConfig.field_name = currField;
        localConfig.yield = fieldSpecifics.yield;
        localConfig.seedPlanted = fieldSpecifics.seedsPlanted;
        localConfig.fertilizer_use = fieldSpecifics.fertilizer_use;
        localConfig.pesticide_use = fieldSpecifics.pesticide_use;


        var structure = new StructureData();
        var apiService = new Service({url:'FieldSpecifics'});

        apiService.createEntity({name: 'Create'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.Create.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Inserting Field Specifics: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });


    }




    // Function   : getFieldArea
    // Description: gets current field area in sqm
    // Paramaters : currFieldName: field name that the user is currently looking at
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    //http://localhost:54357/api/Field/FieldArea
    //ex data {"user_id":"bSznI8kKorVztbyyslrr2PTrevO2","field_cords":"","field_name":"name"}
    getFieldArea(currFieldName){

        var localConfig = {}

        localConfig.user_id = this.state.userID;
        localConfig.field_cords = "";
        localConfig.field_name = currFieldName; 

        var structure = new StructureData();
        var apiService = new Service({url:'Field'});

        apiService.createEntity({name: 'FieldArea'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.FieldArea.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Field Area: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,eMsg]
        });


    }

    
    // Function   : checkLoginStatus
    // Description: checks current login status
    // Paramaters : NONE
    // Returns    : NONE
    checkLoginStatus() {
        return new Promise((resolve, reject) => {
            fire.auth().onAuthStateChanged((user) => {
                if (user) {
                    this.setState({userID:user.uid});
                    //console.log("userID updated in constructor: ", this.state.userID)
                    resolve(user);

                } else {
                    //debug
                    //console.log("Error user is currently null");

                    this.setState({ user: null });
                    //localStorage.removeItem('user');
                }
            });
        });
    }



    // Function   : DeleteField
    // Description: deletes the field
    // Paramaters : fieldName: field name that the user is currently looking at
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    DeleteField(fieldName){
        var localConfig = {}
        //localConfig.field_id = fieldID;
        localConfig.user_id = this.state.userID;
        //localConfig.field_cords = "";
        localConfig.field_name = fieldName;

        var apiService = new Service({url:'field'});

        apiService.createEntity({name: 'none'});

        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.basicDelete(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works

            //debug
            //console.log("fieldData: ", fieldData);
            return [response,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Field Data: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            
            return ["",eMsg]
        });
        
    }



    // Function   : getFieldData
    // Description: gets the field data like soil moisture
    // Paramaters : fieldID, dataType: field id, datatype
    // Returns    : response.data: field specific info
    //              eMsg: error messaged if error occured
    getFieldData(fieldID, dataType) {

       var localConfig = {}
        localConfig.id = fieldID; 
        localConfig.type = dataType;//"EarthData";

        var structure = new StructureData();
        var apiService = new Service({url:'field'});

        apiService.createEntity({name: 'EarthData'});

        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.EarthData.getFieldData( {fieldID: localConfig.id}, {type: localConfig.type})
    
        .then(function (response){
            // stringify then re-pack as json works
            fieldData = structure.repack(response.data)

            //debug
            //console.log("fieldData: ", fieldData);
            return [fieldData,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Field Data: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }
            
            fieldData = structure.repack(fieldData)
            return [fieldData,"No Data Found Within This Field..."]
        });
    }



    // Function   : getFields
    // Description: gets all the fields connected to the current user
    // Paramaters : NONE
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    getFields(){

        var apiService = new Service({url:'field'});
        apiService.createEntity({name: 'FieldPosition'});
        var temp = '/All'
        var structure = new StructureData();
        // even though it is technically all everything is packed into one geojson file
        // return here also so that we return the promise to the caller and allow for the call back

        // **note** id needs to be from the firbase stuff to verify ownership of the field on the 
        // apis end
        //console.log("Curr user: ", this.state.userID);
        return apiService.endpoints.FieldPosition.getAllFields({endPoint: temp, userID:this.state.userID})
        
        //apiService.endpoints.FieldPosition.testLine({id:temp})

    
        .then(function (response){
            // stringify then re-pack as json works
            fields = structure.repack(response.data);

            //debug
            //console.log("fieldData: ", fields);
            return [fields,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                //console.log("Error Getting Fields: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
                if(error.response.status === 401){
                    fire.auth().onAuthStateChanged((user) => {
                        if (user) {
                            console.log("refresh")
                            //this.setState({userID: user.uid})
                            //this.state.userID = user.uid;
                            
                            //console.log("user refresh: ", user);
                            user.getIdToken(true).then(response =>{
                            Auth.authenticateUser(response);       
                            });         
            
                            //console.log("user id is set");
                        } else {
                            this.setState({ user: null });
                            //localStorage.removeItem('user');
                            console.log("Error getting userID");
                        }
                    });
                }
            }

            return [fields,eMsg]
        });
    }




    // Function   : getMyFields
    // Description: gets my fields only
    // Paramaters : localUid: user id to ensure the correct id
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    getMyFields(localUid){

        //let tempUserID = this.state.userID;
        var apiService = new Service({url:'field'});
        apiService.createEntity({name: 'usersFields'});

        // /All breaks the call
        var structure = new StructureData();
        let user = ""
        if (localUid != null){
            user = localUid;
        }
        else {
            user = this.state.userID;
        }


        return apiService.endpoints.usersFields.getUserFields({userID:user})
        
        //apiService.endpoints.FieldPosition.testLine({id:temp})

    
        .then(function (response){
            // stringify then re-pack as json works
            fields = structure.repack(response.data);

            //debug
            //console.log("fieldData: ", fields);
            return [fields,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                console.log("Error Getting My Fields: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }

                if(error.response.status === 401){
                    fire.auth().onAuthStateChanged((user) => {
                        if (user) {
                            console.log("refresh")
                            //this.state.userID = user.uid;
                            
                            
                            user.getIdToken(true).then(response =>{
                                Auth.authenticateUser(response);               
                                       
                            }); 
            
                            //console.log("user id is set");
                        } else {
                            this.setState({ user: null });
                            //localStorage.removeItem('user');
                            console.log("Error getting userID");
                        }
                    });
    
                }

            }
            

            return [fields,eMsg]
        });

        


        //getUserFields

    }




    // Function   : getMyFieldCount
    // Description: gets my field count
    // Paramaters : userID: user id to ensure the correct id
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    //{user_id:"ASDFJFWEIOJSD8238GFSDF289YTSDLJ",field_name:"",field_cords:""}
    //http://localhost:54357/api/Field/FieldCount
    getMyFieldCount(userID){
        
        var localConfig = {}
        //console.log("stored user: ", this.state.userID);
        localConfig.user_id = userID;
        localConfig.field_name = "";
        localConfig.field_cords = ""; 

        var apiService = new Service({url:'Field/FieldCount'});

        apiService.createEntity({name: 'none'});


        //apiService.endpoints.EarthData.testResource( {id: temp }, {fieldID: localConfig.id}, {type: localConfig.type});
        // return here also so that we return the promise to the caller and allow for the call back
        return apiService.endpoints.none.grab(localConfig)
    
        .then(function (response){
            // stringify then re-pack as json works
            
            // use status to let the user know if they did update the name of the field
            return [response.data,""]
            
        })
        .catch(function (error){
            let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                console.log("Error Getting My Field Count ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }

            if(error.response.status === 401){
                fire.auth().onAuthStateChanged((user) => {
                    if (user) {
                        console.log("refresh")
                        //this.setState({userID: user.uid})
                        //this.state.userID = user.uid;
                        
                        //console.log("user refresh: ", user);
                        user.getIdToken(true).then(response =>{
                        Auth.authenticateUser(response);       
                        });         
        
                        //console.log("user id is set");
                    } else {
                        this.setState({ user: null });
                        //localStorage.removeItem('user');
                        console.log("Error getting userID");
                    }
                });
            }
            
            
            return ["NA",eMsg]
        });

    }




    // Function   : insertField
    // Description: gets my field count
    // Paramaters : data: the field poly to insert
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    insertField(data){


        var apiService = new Service({url:'field'});
        apiService.createEntity({name: 'none'});
        var temp = data["coordinates"];
        //console.log(temp);
        var cords = '{"type": "Polygon","coordinates":[[';
        let first = true;
        temp.forEach(element => {
            if (!first){
                cords += ",";
            }
            else {
                first = false;
            }
            //cords += "["+ element['lat'] +"," + element['lng']+ "]";
            cords += "["+ element['lng'] +"," + element['lat']+ "]";

        });
        // close the poly 
        //cords += ",["+ temp[0]['lat'] +"," + temp[0]['lng']+ "]]]}";
        cords += ",["+ temp[0]['lng'] +"," + temp[0]['lat']+ "]]]}";
        // 


        var parsedCords = JSON.parse(cords);
        //console.log("fieldID: ", this.state.userID);
        return apiService.endpoints.none.create({ userID:this.state.userID }, {coordinates:parsedCords}, {fieldName:data['fieldName']})
    
        .then(function (response){
            //debug
            //console.log("Insert Results: ", response.data);
            return "Success"
            
        })
        .catch(function (error){
            /*let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                console.log("Error Inserting New Field: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }*/


            return null
        });
        
    }



    // Function   : GetEarthDataTypes
    // Description: gets all the earth data types available
    // Paramaters : NONE
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    //http://localhost:54357/api/field/EarthData/All
    GetEarthDataTypes(){
        var apiService = new Service({url:'field'});
        apiService.createEntity({name: 'EarthData'});
        //console.log(temp);

        let location = "All"
        //console.log("fieldID: ", this.state.userID);
        //console.log("Trying to get earth types");

        return apiService.endpoints.EarthData.getOne({id:location})
        .then(function (response){
            //debug
            //console.log("Insert Results: ", response.data);
            return response.data
            
        })
        .catch(function (error){
            /*let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                console.log("Error Getting Field Data Types: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }*/

            return null
        });


    }




    // Function   : RegisterUser
    // Description: register a new user
    // Paramaters : localUser: current users info
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    /*http://localhost:54357/api/users
        {
            id:"asdt234gaewdfvasdfq324tqa3wefasdfasdf",
            user_name:"fake ben", 
            account_type_id:1,
            field_count:0}
    */
   RegisterUser(localUser){

        var localParams = {}

        localParams.id = localUser.uid;
        localParams.account_type_id = 1;
        localParams.field_count = 0;
        localParams.user_name = localUser.username;

        var apiService = new Service({url:'users'});
        apiService.createEntity({name: 'none'});

        return apiService.endpoints.none.grab(localParams)
        .then(function (response){
            //debug
            //console.log("Insert Results: ", response.data);
            return response.data
            
        })
        .catch(function (error){
            /*let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                console.log("Error Registering new user: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }*/

            return null
        });

    }




    // Function   : CreateRelationship
    // Description: allow a user to share their fields with someone else
    // Paramaters : email: the user they wish to share their fields with
    // Returns    : response.data: all the fields as a geojson object
    //              eMsg: error messaged if error occured
    //api/users/SharedField
    //{id:"RyeHW0fo8kU3hSzGYbZDv5l33352",user_name:"sam"}
    // userId is sharing their fields with username
    CreateRelationship(email){
        var localParams = {}

        localParams.id = this.state.userID;
        localParams.user_name = email;

        var apiService = new Service({url:'users'});
        apiService.createEntity({name: 'ShareField'});

        return apiService.endpoints.ShareField.grab(localParams)
        .then(function (response){
            //debug
            //console.log("Insert Results: ", response.data);
            return response.data
            
        })
        .catch(function (error){
            /*let eMsg = "";
            if(error === undefined){
                eMsg = massiveError;
            }
            else {
                console.log("Error Registering new user: ",error);
                eMsg = error.response.message;
                if(error.response.message === undefined){
                    eMsg = error.response.status;
                }
            }*/

            return null
        });

    }


}

class StructureData {
    repack(geoJson){
        var tempData = geoJson;
        var geoSrting = JSON.stringify(tempData);
  
        //debug
        //console.log("geoString: ", geoSrting);
        const data = JSON.parse(geoSrting);

        //debug
        //console.log("data: ", data);
        return data
    }
  }

export default API