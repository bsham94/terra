/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  service.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the axios specific calls                                |
*************************************************************************************************/


const axios = require('axios')

// source for crud format
// https://github.com/FrancescoSaverioZuppichini/API-Class/blob/master/source/API.js

// look into dev enviro vs deployment enviro


class Service {
    constructor({url, newService}){
        // one place to change url depending on the environment 
        if (newService != null){
            this.url = newService.concat(url);
            //console.log("my current url:", url);
        }
        else if(!arguments.length) {
            this.url = "http://localhost:59935/api/field";
        }
        else {
            this.url = "http://localhost:59935/api/".concat(url);
        }

        this.endpoints = {}
        let token = localStorage.getItem('token');
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
    }



    // Function   : createEntity
    // Description: creates the entity
    // Paramaters : entity : current calls controller
    // Returns    : NONE
    createEntity(entity) {

        const name = entity.name;
        this.endpoints[name] = this.createCrudEndpoints(entity);
    }



    // Function   : createEntities
    // Description: creates the array of entities
    // Paramaters : entity : current calls controller
    // Returns    : NONE
    createEntities(arrayOfEntity){
        arrayOfEntity.array.forEach(this.createEntity.bind(this));

    }



    // Function   : createCrudEndpoints
    // Description: creates the endpoint call 
    // Paramaters : name : current path for the api call
    // Returns    : NONE
    createCrudEndpoints({name}){
        var endpoints = {}
        var resourceURL = ''
        if(name === 'none'){
            resourceURL = `${this.url}`
        }else {
            resourceURL = `${this.url}/${name}`
        }
        

        endpoints.getAll = ({ query={}}, config={} ) => axios.get(resourceURL, Object.assign({ params: { query }, config }))

        endpoints.getOne = ({ id }, config={}) =>  axios.get(`${resourceURL}/${id}`,{
            'Content-Type': 'application/json'
          })
        
        /*endpoints.yoMommasHouse = ({web}, config={}) => axios.get( `${web}`,{
            'Content-Type': 'application/json'
          })*/
        endpoints.getWeatherAlerts = ({id, weatherPackage}, config={}) => axios.get(`${resourceURL}/${id}`,{params:{product:weatherPackage['product'] , name: weatherPackage['name'], app_id: weatherPackage['app_id'], app_code: weatherPackage['app_code'] }})

        endpoints.getAllFields = ({endPoint, userID}) =>  axios.post(`${resourceURL}${endPoint}`,{user_id: userID, field_cords:"", field_name:""},{
        'Content-Type': 'application/json'
        })

        endpoints.getUserFields = ({ userID}) =>  axios.post(`${resourceURL}`,{user_id: userID, field_cords:"", field_name:""},{
            'Content-Type': 'application/json'
            })

        endpoints.getFieldData = ( {fieldID}, {type}) =>  axios.post(`${resourceURL}`, { id: fieldID, type: type},{
            'Content-Type': 'application/json'
          })

          
        endpoints.testResource = ({ id }, {fieldID}, {type}) =>  console.log(`${resourceURL}`, { id: fieldID, type: type})

        endpoints.testLine = ({ id }, config={}) => console.log(`${resourceURL}/${id}`)

        endpoints.create = (toCreate, config={}) =>  axios.post(resourceURL, toCreate, config)
        
        endpoints.grab = (toCreate, config={}) =>  axios.post(`${resourceURL}`, toCreate, config)

        endpoints.basicUpdate = (toUpdate, config={}) => axios.put(`${resourceURL}`, toUpdate, config)

        endpoints.update = (toUpdate, config={}) => axios.put(`${resourceURL}/${toUpdate.id}`, toUpdate, config)
        
        endpoints.create = ({ userID }, {coordinates}, {fieldName}) =>  axios.post(`${resourceURL}`, {user_id:userID, field_cords:coordinates, field_name:fieldName},{
            'Content-Type': 'application/json'
          })
        //for future consideration
        //endpoints.patch  = ({id}, toPatch, config={}) => axios.patch(`${resourceURL}/${id}`, toPatch, config)

        endpoints.basicDelete = ( toDelete , config={}) => axios.delete(`${resourceURL}`, {data: toDelete})

        endpoints.delete = ({ id }, config={}) => axios.delete(`${resourceURL}/${id}`, config)

        return endpoints

    }

}

export default Service