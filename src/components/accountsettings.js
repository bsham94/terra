/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  accountsettings.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality for account settings to work and
                    every api calls that pertains to each setting                 |
*************************************************************************************************/
import React, { Component } from 'react';
import { AvForm, AvField } from 'availity-reactstrap-validation';
import fire from '../config/fire';
import {Collapse, Button, Dropdown, DropdownToggle, DropdownMenu, DropdownItem} from "reactstrap";
import { confirmAlert } from 'react-confirm-alert'; // Import
import 'react-confirm-alert/src/react-confirm-alert.css' // Import css
import Clock from 'react-live-clock';
import API from './api';

var myAPI = new API();

class Settings extends Component {
    constructor(props) {
        super(props);
        this.onUpdateEmail = this.onUpdateEmail.bind(this);
        this.change = this.change.bind(this);
        this.toggleEmailUpdate = this.toggleEmailUpdate.bind(this);
        this.togglePasswordUpdate = this.togglePasswordUpdate.bind(this); 
        this.toggleFieldUpdate = this.toggleFieldUpdate.bind(this);
        this.toggleCollapseRelationShip = this.toggleCollapseRelationShip.bind(this);
        this.toggleFieldSpecificsUpdate = this.toggleFieldSpecificsUpdate.bind(this); 
        this.toggleFieldSpecificsInsert= this.toggleFieldSpecificsInsert.bind(this); 
        this.toggleFieldSpecificsInsertPesticide= this.toggleFieldSpecificsInsertPesticide.bind(this); 
        this.toggleFieldSpecificsInsertYield= this.toggleFieldSpecificsInsertYield.bind(this);   
        this.toggleFieldSpecificsInsertFertilizer= this.toggleFieldSpecificsInsertFertilizer.bind(this);   
        this.toggleAll= this.toggleAll.bind(this);                                                      
        this.state = {
            email: '',
            password: '',
            currentPassword: '',
            newPassword: '',
            confirmNewPassword: '',
            fieldName: '',
            newFieldName: '',
            emailVerified: '',
            fieldNameInsert: '',
            pesticideInsert: '',
            yieldInsert: '',
            fertilizerInsert: '',
            seedsPlantedInsert: '',
            fieldNameUpdate: '',
            pesticideUpdate: '',
            yieldUpdate: 0,
            fertilizerUpdate: '',
            seedsPlantedUpdate: '',
            name:{},
            locationName: '',
            fieldCount: '',
            friendsEmail: '',
            collapseEmailUpdate: false,
            collapsePasswordUpdate: false,
            collapseFieldUpdate: false,
            collapseFieldSpecificsInsert: false, 
            collapseFieldSpecificsInsertPesticide: false, 
            collapseFieldSpecificsInsertYield: false, 
            collapseFieldSpecificsInsertFertilizer: false, 
            collapseFieldSpecificsUpdate: false,  
            allDropdownOpen: false,
            collapseRelationShip: false,
            myFields: {},  
            fieldsExist: false,
            
        }
        
    }

    componentDidMount() {
        window.scrollTo(0, 0);
        fire.auth().onAuthStateChanged((user) => {
            if (user) {
                this.myFieldList(user.uid);
                myAPI.getMyFieldCount(user.uid).then((fcount) => {
                    this.setState({fieldCount: fcount})
                    //this.state.fieldCount = fcount;
                })
            } else {
                console.log("Error gettign userID");
            }
        });
        
    }
    
    change = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        });
    };

    toggleAll() {
        this.setState(prevState => ({
          allDropdownOpen: !prevState.allDropdownOpen
        }));
      }

    toggleEmailUpdate() {
        this.setState(state => ({ collapseEmailUpdate: !state.collapseEmailUpdate }));
    } 

    togglePasswordUpdate() {
        this.setState(state => ({ collapsePasswordUpdate: !state.collapsePasswordUpdate }));
    }

    toggleFieldUpdate() {
        this.setState(state => ({ collapseFieldUpdate: !state.collapseFieldUpdate }));
    }

    toggleCollapseRelationShip() {
        this.setState(state => ({ collapseRelationShip: !state.collapseRelationShip }));
    }

    toggleFieldSpecificsUpdate() {
        this.setState(state => ({ collapseFieldSpecificsUpdate: !state.collapseFieldSpecificsUpdate }));
    }

    toggleFieldSpecificsInsert() {
        this.setState(state => ({ 
            collapseFieldSpecificsInsert: !state.collapseFieldSpecificsInsert ,
            collapseFieldSpecificsInsertPesticide: false,
            collapseFieldSpecificsInsertYield: false, 
            collapseFieldSpecificsInsertFertilizer: false, 

        }));
    }

    toggleFieldSpecificsInsertPesticide() {
        this.setState(state => ({ 
            collapseFieldSpecificsInsertPesticide: !state.collapseFieldSpecificsInsertPesticide,
            collapseFieldSpecificsInsert: false, 
            collapseFieldSpecificsInsertYield: false, 
            collapseFieldSpecificsInsertFertilizer: false, 
         }));
    }

    toggleFieldSpecificsInsertYield() {
        this.setState(state => ({ 
            collapseFieldSpecificsInsertYield: !state.collapseFieldSpecificsInsertYield,
            collapseFieldSpecificsInsert: false, 
            collapseFieldSpecificsInsertPesticide: false, 
            collapseFieldSpecificsInsertFertilizer: false, 
         }));
    }

    toggleFieldSpecificsInsertFertilizer() {
        this.setState(state => ({ 
            collapseFieldSpecificsInsertFertilizer: !state.collapseFieldSpecificsInsertFertilizer,
            collapseFieldSpecificsInsert: false, 
            collapseFieldSpecificsInsertPesticide: false, 
            collapseFieldSpecificsInsertYield: false, 
         }));
    }

    onUpdateEmail() {
        var user = fire.auth().currentUser;
        const firebase = require('firebase');
        var cred = firebase.auth.EmailAuthProvider.credential(user.email, this.state.password);

        user.reauthenticateAndRetrieveDataWithCredential(cred).then(() => {
            user.updateEmail(this.state.email).then(() => {
                confirmAlert({
                    title: 'Updated Email',
                    message: 'Sucessfully Updated Email!',
                    buttons: [
                      {
                        label: 'Close',
                      }
                    ]
                  })
              })
          }).catch(function(error) {
            confirmAlert({
                title: 'Invalid Password',
                message: 'You have entered an incorrect password when attempting to update your E-Mail address!',
                buttons: [
                  {
                    label: 'Try Again',
                  }
                ]
              })
          });
    }

    onUpdatePassword() {
        if (this.state.newPassword === this.state.confirmNewPassword)
        {
            var user = fire.auth().currentUser;
            const firebase = require('firebase');
            var cred = firebase.auth.EmailAuthProvider.credential(user.email, this.state.currentPassword);

            user.reauthenticateAndRetrieveDataWithCredential(cred).then(() => {
                user.updatePassword(this.state.newPassword).then(() => {
                    confirmAlert({
                        title: 'Updated Password',
                        message: 'Sucessfully Updated PassWord!',
                        buttons: [
                          {
                            label: 'Close',
                          }
                        ]
                      })
                  })
              }).catch(function(error) {
                confirmAlert({
                    title: 'Invalid Password',
                    message: 'You have entered an incorrect password when attempting to update your password!',
                    buttons: [
                      {
                        label: 'Try Again',
                      }
                    ]
                  })
              });
            
        }
        else
        {
            confirmAlert({
                title: 'Unmatched Passwords',
                message: 'Your Password inputs do not match! Please try again',
                buttons: [
                  {
                    label: 'Try Again',
                  }
                ]
              })
        }
    }

    onUpdateFieldName() {
        myAPI.updateFieldName(this.state.fieldName,this.state.newFieldName).then((fieldData) => {
            if(fieldData[1] === "")
            {
                confirmAlert({
                    title: "Success",
                    message: "Successfully Updated Field Name",
                    buttons: [
                      {
                        label: 'Close',
                      }
                    ]
                  })

                fire.auth().onAuthStateChanged((user) => {
                    if (user) {
                        this.myFieldList(user.uid);
                    } else {
                        console.log("Error gettign userID");
                    }
                });
            }
            else
            {
                confirmAlert({
                    title: "Error",
                    message: "Error Updating Field Name",
                    buttons: [
                      {
                        label: 'Try Again'
                      }
                    ]
                  })
            }
            
        })
    }

    onUpdateFieldSpecifics() {

        var fieldSpecifics = {
            yield: this.state.yieldUpdate,
            seedsPlanted: this.state.seedsPlantedUpdate,
            fertilizer_use: this.state.fertilizerUpdate,
            pesticide_use: this.state.pesticideUpdate
        }
        myAPI.updateFieldSpecifics(this.state.fieldNameUpdate,fieldSpecifics).then((fieldData) => {
            if(fieldData[1] === "")
            {
                confirmAlert({
                    title: "Success",
                    message: "Successfully Updated",
                    buttons: [
                      {
                        label: 'Close',
                      }
                    ]
                  })
            }
            else
            {
                confirmAlert({
                    title: "Error",
                    message: "Error Updating Field Specifics",
                    buttons: [
                      {
                        label: 'Try Again'
                      }
                    ]
                  })
            }

            // reset to defaults and ensure that following 
            // calls have success
            this.setState({
                yieldUpdate: 0,
                pesticideUpdate: '',
                fertilizerUpdate:''
            });
        })
    }

    onInsertFieldSpecifics() {
        var fieldSpecifics = {
            yield: this.state.yieldInsert,
            seedsPlanted: this.state.seedsPlantedInsert,
            fertilizer_use: this.state.fertilizerInsert,
            pesticide_use: this.state.pesticideInsert
        }
        myAPI.insertFieldSpecifics(this.state.fieldNameInsert,fieldSpecifics).then((fieldData) => {
            if(fieldData[1] === "")
            {
                confirmAlert({
                    title: "Success",
                    message: "Successfully Inserted",
                    buttons: [
                      {
                        label: 'Close',
                      }
                    ]
                  })
            }
            else
            {
                confirmAlert({
                    title: "Error",
                    message: "Error Inserting Field Specifics",
                    buttons: [
                      {
                        label: 'Try Again'
                      }
                    ]
                  })
            }
            
        })
    }

    resendVerification() {
        var user = fire.auth().currentUser;

        user.sendEmailVerification().then(function() {
        // Email sent.
        }).catch(function(error) {
        // An error happened.
        });
    }

    myFieldList(userId) {
        myAPI.getMyFields(userId).then((myFields) => {
            //update the state aka field object with the data once it has been returned
            let tempMyFields = [];
            //features[tempCount].properties.field_name
            if(myFields[1] === ""){
                //console.log("current field names: ", myFields[0].features);
                for(let myFieldCount = 0; myFieldCount < myFields[0].features.length; myFieldCount++){
                    tempMyFields.push(myFields[0].features[myFieldCount].properties.field_name);
                    //console.log("current field names: ", tempMyFields);
                }
                this.setState({
                    myFields: tempMyFields,
                    fieldsExist: true
                });
            }
            
            if(myFields[1] !== ""){
              // no fields?
              this.setState({
                fieldsExist: false
                });
            }
            //console.log("what my fields?:  ", this.state.myFields);
          });


    }

    fieldListCreate() {
        let table = []

        for(let myFieldCount = 0; myFieldCount < this.state.myFields.length; myFieldCount++){
            table.push(<li style={{fontSize:"22px"}} className="fieldList">{this.state.myFields[myFieldCount]}</li>)
            //console.log("pushing that field: ", this.state.myFields[myFieldCount]);
        }

        return table
    }


    createRelationship(){
        //console.log("friendEmail: ", this.state.friendsEmail);
        myAPI.CreateRelationship(this.state.friendsEmail).then((result)=>{
            if(result !== null)
            {
                confirmAlert({
                    title: "Success",
                    message: "Successfully Created",
                    buttons: [
                      {
                        label: 'Close',
                      }
                    ]
                  })
            }
            else
            {
                confirmAlert({
                    title: "Error",
                    message: "Error Creating Relationship",
                    buttons: [
                      {
                        label: 'Try Again'
                      }
                    ]
                  })
            }


        });
        fire.auth().onAuthStateChanged((user) => {
            if (user) {
                
                
            } else {
                console.log("Error gettign userID");
            }
        });

    }

    render() {
        var user = fire.auth().currentUser;
        let userName;
        let locationName;
        let fieldCount;
        if (user) {
          this.state.name = user.displayName
          userName = this.state.name + "'s";
        }
        if (user != null) {
            this.state.emailVerified = user.emailVerified
        }
        locationName = this.state.locationName
        fieldCount = this.state.fieldCount
        //console.log(locationName)
        return (
            <React.Fragment>
            <section className="packagesContainer">
                <div>
                    <h2 style={{color:"#fff", textAlign:"center"}}>{ userName } Account Settings</h2>
                    <i className="fas fa-chevron-circle-down"></i>
                    <div className="packages">
                        <h5 style={{color:"#fff"}}>Current User Time: <Clock
                        format={'dddd, MMMM Mo, YYYY, h:mm:ss A'}
                        ticking={true}
                        timezone={'US/Eastern'} /></h5>
                        { this.state.locationName ? <h5 style={{color:"#fff"}}>Geographic Location: { locationName }</h5> : <h5 style={{color:"#fff"}}>Geographic Location: Ontario, Canada</h5> }
                        <h5 style={{color:"#fff"}}>Total Owned Fields: { this.state.fieldCount }</h5>
                    </div>
                    <div className="packages">
                        <AvForm style={{width: "500px"}}>
                            <h3 style={{color:"#fff"}}>General Settings</h3>
                            <br />
                            <Button color="primary" onClick={this.toggleEmailUpdate} style={{ marginBottom: '1rem' }}><h5 style={{color:"#fff"}}>Update Email</h5></Button>
                            <Collapse isOpen={this.state.collapseEmailUpdate}>
                                <div>
                                    <AvField placeholder="Password" type="password" name="password" value={this.state.password} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid password'},
                                        pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your password must be composed only with letter and numbers'},
                                        minLength: {value: 6, errorMessage: 'Your password must be between 6 and 16 characters'},
                                        maxLength: {value: 16, errorMessage: 'Your password must be between 6 and 16 characters'}
                                    }} />
                                    <AvField placeholder="New E-Mail Address" type="email" name="email" value={this.state.email} onChange={e => this.change(e)} validate={{email: true}} />
                                    <Button size="lg" onClick={e => { this.onUpdateEmail()}}>Submit</Button>
                                </div>
                            </Collapse>
                            <br />
                            <hr />
                            <Button color="primary" onClick={this.togglePasswordUpdate} style={{ marginBottom: '1rem' }}><h5 style={{color:"#fff"}}>Update Password</h5></Button>
                            <Collapse isOpen={this.state.collapsePasswordUpdate}>
                                <div>
                                    <AvField placeholder="Current Password" type="password" name="currentPassword" value={this.state.currentPassword} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid password'},
                                        pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your password must be composed only with letter and numbers'},
                                        minLength: {value: 6, errorMessage: 'Your password must be between 6 and 16 characters'},
                                        maxLength: {value: 16, errorMessage: 'Your password must be between 6 and 16 characters'}
                                    }} />
                                    <AvField placeholder="New Password" type="password" name="newPassword" value={this.state.newPassword} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid password'},
                                        pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your password must be composed only with letter and numbers'},
                                        minLength: {value: 6, errorMessage: 'Your password must be between 6 and 16 characters'},
                                        maxLength: {value: 16, errorMessage: 'Your password must be between 6 and 16 characters'}
                                    }} />
                                    <AvField placeholder="Confirm New Password" type="password" name="confirmNewPassword" value={this.state.confirmNewPassword} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid password'},
                                        pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your password must be composed only with letter and numbers'},
                                        minLength: {value: 6, errorMessage: 'Your password must be between 6 and 16 characters'},
                                        maxLength: {value: 16, errorMessage: 'Your password must be between 6 and 16 characters'}
                                    }} />
                                    <Button size="lg" onClick={e => this.onUpdatePassword()} >Submit</Button>
                                </div>
                            </Collapse>
                            <br />
                            <hr />
                            <h5 style={{color:"#fff"}}>Verified?</h5>
                                {this.state.emailVerified ? <h6 style={{color:"#32CD32"}}>Yes</h6> : 
                                <div>
                                <h6 style={{color:"#FF0000"}}>No</h6>
                                <br />
                                <Button size="lg" onClick={e => this.resendVerification()} >Resend Verification Email</Button>
                                </div>
                                }
                        </AvForm>
                        <br></br>  
                    <AvForm style={{width: "500px"}}>
                        <h3 style={{color:"#fff", marginBottom:"10"}}>Update Field Names</h3>
                        <br />
                        <Button color="primary" onClick={this.toggleFieldUpdate} style={{ marginBottom: '1rem' }}><h5 style={{color:"#fff"}}>Update Field</h5></Button>
                        <Collapse isOpen={this.state.collapseFieldUpdate}>
                            <div>
                                <AvField placeholder="Field Name" name="fieldName" value={this.state.fieldName} onChange={e => this.change(e)}/>
                                <br/>
                                <AvField placeholder="New Field Name" name="newFieldName" value={this.state.newFieldName} onChange={e => this.change(e)}/>
                                <br/>
                                <Button color="secondary" size="lg" onClick={e => this.onUpdateFieldName()}>Submit</Button>
                            </div>
                        </Collapse>
                        <br/>
                            <br />
                            <hr />
                            <h5 style={{color:"#fff"}}>List of my Fields</h5>
                            <div style={{width:"80%"}}> 
                                <ul style={{color:"#fff"}} className="fieldArea">
                                    {(this.state.fieldsExist) ? this.fieldListCreate() : <p>No Field's Exist</p>}
                                </ul>
                            </div>
                        </AvForm>
                        <AvForm style={{width: "500px"}}>
                        <h3 style={{color:"#fff", marginBottom:"10"}}>Field Specifics</h3>
                        <br/>
                            <h10 style={{color:"#fff", marginBottom:"10"}}>Use this in order to add or update any specifics about your field. Such as, pesticide used, yield, fertilizer used & seeds planted.</h10>
                            <br />
                            <br />
                            <Button color="primary" onClick={this.toggleFieldSpecificsUpdate} style={{ marginBottom: '1rem' }}><h5 style={{color:"#fff"}}>Insert Specific Field Information</h5></Button>
                            <Collapse isOpen={this.state.collapseFieldSpecificsUpdate}>
                                    <br />
                                    <div>
                                        <AvField placeholder="Field Name"  name="fieldNameInsert" value={this.state.fieldNameInsert} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid field name'}
                                    }}/>
                                        <AvField placeholder="Pesticide Being Used"  name="pesticideInsert" value={this.state.pesticideInsert} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid pesticide'}
                                    }}/>
                                        <AvField placeholder="Yield"  name="yieldInsert" value={this.state.yieldInsert} onChange={e => this.change(e)}  validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid yield'}
                                    }}/>
                                        <AvField placeholder="Fertilizer Being Used"  name="fertilizerInsert" value={this.state.fertilizerInsert} onChange={e => this.change(e)}  validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid fertilizer'}
                                    }}/>
                                        <AvField placeholder="Seeds Planted"  name="seedsPlantedInsert" value={this.state.seedsPlantedInsert} onChange={e => this.change(e)}  validate={{
                                        required: {value: true, errorMessage: 'Please enter seed(s) planted'},
                                    }}/>
                                        <Button size="lg" onClick={e => this.onInsertFieldSpecifics()} >Insert</Button>
                                    </div>
                            </Collapse>
                            <br />
                            <Dropdown isOpen={this.state.allDropdownOpen} toggle={this.toggleAll}>
                                        <DropdownToggle size="lg" color="primary" style={{ marginBottom: '1rem' }}>
                                            Update Specific Field Information
                                        </DropdownToggle>
                                        <DropdownMenu>
                                            <DropdownItem onClick={this.toggleFieldSpecificsInsert}>Update All</DropdownItem>
                                            <DropdownItem divider />
                                            <DropdownItem onClick={this.toggleFieldSpecificsInsertPesticide}>Update Pesticide</DropdownItem>
                                            <DropdownItem divider />
                                            <DropdownItem onClick={this.toggleFieldSpecificsInsertYield}>Update Yield</DropdownItem>
                                            <DropdownItem divider />
                                            <DropdownItem onClick={this.toggleFieldSpecificsInsertFertilizer}>Update Fertilizer</DropdownItem>
                                        </DropdownMenu>
                                        </Dropdown>
                            <Collapse isOpen={this.state.collapseFieldSpecificsInsert}>
                                    <br />
                                    <div>
                                        <AvField placeholder="Field Name"  name="fieldNameUpdate" value={this.state.fieldNameUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid field name'}
                                    }}/>
                                    <AvField placeholder="Seeds Planted"  name="seedsPlantedUpdate" value={this.state.seedsPlantedUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter seed(s) planted'}
                                    }}/>
                                        <AvField placeholder="Pesticide Being Used"  name="pesticideUpdate" value={this.state.pesticideUpdate} onChange={e => this.change(e)} />
                                        <AvField placeholder="Yield"  name="yieldUpdate" value={this.state.yieldUpdate} onChange={e => this.change(e)} />
                                        <AvField placeholder="Fertilizer Being Used"  name="fertilizerUpdate" value={this.state.fertilizerUpdate} onChange={e => this.change(e)} />
                                        <Button size="lg" onClick={e => this.onUpdateFieldSpecifics()} >Update</Button>
                                    </div>
                            </Collapse>
                            <Collapse isOpen={this.state.collapseFieldSpecificsInsertPesticide}>
                                    <br />
                                    <div>
                                        <AvField placeholder="Field Name"  name="fieldNameUpdate" value={this.state.fieldNameUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid field name'}
                                    }} />
                                    <AvField placeholder="Seeds Planted"  name="seedsPlantedUpdate" value={this.state.seedsPlantedUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter seed(s) planted'}
                                    }}/>
                                        <AvField placeholder="Pesticide Being Used"  name="pesticideUpdate" value={this.state.pesticideUpdate} onChange={e => this.change(e)} />
                                        <Button size="lg" onClick={e => this.onUpdateFieldSpecifics()} >Update</Button>
                                    </div>
                            </Collapse>
                            <Collapse isOpen={this.state.collapseFieldSpecificsInsertYield}>
                                    <br />
                                    <div>
                                        <AvField placeholder="Field Name"  name="fieldNameUpdate" value={this.state.fieldNameUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid field name'}
                                    }}/>
                                    <AvField placeholder="Seeds Planted"  name="seedsPlantedUpdate" value={this.state.seedsPlantedUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter seed(s) planted'}
                                    }} />
                                        <AvField placeholder="Yield"  name="yieldUpdate" value={this.state.yieldUpdate} onChange={e => this.change(e)} />
                                        <Button size="lg" onClick={e => this.onUpdateFieldSpecifics()} >Update</Button>
                                    </div>
                            </Collapse>
                            <Collapse isOpen={this.state.collapseFieldSpecificsInsertFertilizer}>
                                    <br />
                                    <div>
                                        <AvField placeholder="Field Name"  name="fieldNameUpdate" value={this.state.fieldNameUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter a valid field name'}
                                    }}/>
                                     <AvField placeholder="Seeds Planted"  name="seedsPlantedUpdate" value={this.state.seedsPlantedUpdate} onChange={e => this.change(e)} validate={{
                                        required: {value: true, errorMessage: 'Please enter seed(s) planted'}
                                    }} />
                                        <AvField placeholder="Fertilizer Being Used"  name="fertilizerUpdate" value={this.state.fertilizerUpdate} onChange={e => this.change(e)} />
                                        <Button size="lg" onClick={e => this.onUpdateFieldSpecifics()} >Update</Button>
                                    </div>
                            </Collapse>
                        </AvForm>
                        
                        <br></br>
                        <AvForm style={{width: "500px"}}>
                        <h3 style={{color:"#fff", marginBottom:"10"}}>Share Your Fields</h3>
                        <br />
                        <Button color="primary" onClick={this.toggleCollapseRelationShip} style={{ marginBottom: '1rem' }}><h5 style={{color:"#fff"}}>Create Relationship</h5></Button>
                        <Collapse isOpen={this.state.collapseRelationShip}>
                            <div>
                                <AvField placeholder="Friends Email" name="friendsEmail" value={this.state.friendsEmail} onChange={e => this.change(e)}/>
                                <br/>
                                <Button color="secondary" size="lg" onClick={e => this.createRelationship()}>Submit</Button>
                            </div>
                        </Collapse>
                        </AvForm>
                    
                    </div>
                    
            </div>
            </section>
            </React.Fragment>
        );
    }
}

export default Settings;
