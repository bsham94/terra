/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  login.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality in order to successfully login
                  and register a user                |
*************************************************************************************************/
import React, { Component } from 'react';
import {Link, Redirect} from "react-router-dom";
import {Button} from "reactstrap";
import { AvForm, AvField } from 'availity-reactstrap-validation';
import fire from '../config/fire';
import axios from 'axios';
import Auth from './auth.js'
import { confirmAlert } from 'react-confirm-alert';
import API from './api';

var myAPI = new API();

class Login extends Component {
    constructor(props) {
        super(props);
        this.onLogin = this.onLogin.bind(this);
        this.onRegister = this.onRegister.bind(this);
        this.change = this.change.bind(this);
        
        this.state = {
            firstName: '',
            lastName: '',
            email: '',
            password: '',
            emailLogin: '',
            passwordLogin: '',
            toHome: false,
        }
    }
    componentDidMount() {
        window.scrollTo(0, 0);
    }
    change = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        });
    };
    
    onLogin = e => {
        e.preventDefault();
        fire.auth().signInWithEmailAndPassword(this.state.emailLogin, this.state.passwordLogin).then((u) => 
        fire.auth().onAuthStateChanged((user) => {
            if (user) {
                    user.getIdToken(true).then(response =>{
                    Auth.authenticateUser(response);
                    axios.defaults.headers.common['Authorization'] = `Bearer ${response}`                
                });               
            }
            
          this.setState({  
            toHome: true
        })
        })).catch(function(error) {
            var errorCode;
            switch(error.code) {
                case 'auth/wrong-password':
                   errorCode = "Wrong Password";
                   break;
                case 'auth/user-not-found':
                    errorCode = "User Not Found";
                    break;
                case 'auth/auth/user-disabled':
                   errorCode = "User is Disabled";
                   break;
                case 'auth/invalid-email':
                    errorCode = "Invalid Email";
                    break;
                default:
                    errorCode = "Invalid Details Please Try Again";
                    break;
            }
            var errorMessage = error.message;
            confirmAlert({
                title: errorCode,
                message: errorMessage,
                buttons: [
                  {
                    label: 'Try Again',
                  }
                ]
              })
          })
    }

    onRegister = e => {
        e.preventDefault();        
        fire.auth().createUserWithEmailAndPassword(this.state.email, this.state.password).then(() => ( 
            fire.auth().onAuthStateChanged((user) => {
                if (user) {
                    user.updateProfile({
                        displayName: this.state.firstName + " " + this.state.lastName
                    })
                    user.getIdToken(true).then(response =>{
                        Auth.authenticateUser(response);
                        axios.defaults.headers.common['Authorization'] = `Bearer ${response}`                
                    });  
                    user.sendEmailVerification()
                    this.setState({userid:user.uid})
                    //console.log(this.state.userid);
                    
                    //axios.post('https://terra-capstone-api.herokuapp.com/api/users',
                    let localUser = {
                        uid: user.uid,
                        username: this.state.email
                    }
                    myAPI.RegisterUser(localUser).then((response)=>{
                        if(response != null){
                            this.setState({
                                toHome: true
                            })
                        }
                        
                    });  
              }
            }))
        ).catch(function(error) {
            var errorCode
            switch(error.code) {
                case 'auth/email-already-in-use':
                   errorCode = "Email Already in Use";
                   break;
                case 'auth/invalid-email':
                    errorCode = "Invalid Email";
                    break;
                case 'auth/operation-not-allowed':
                   errorCode = "Operation Not Allowed";
                   break;
                case 'auth/weak-password':
                    errorCode = "Password is too Weak";
                    break; 
                default:
                    errorCode = "Invalid Details Please Try Again";
                    break;
                    
            }
            var errorMessage = error.message;
            confirmAlert({
                title: errorCode,
                message: errorMessage,
                buttons: [
                  {
                    label: 'Try Again',
                  }
                ]
              })
          })
    }

    render() {
        if (this.state.toHome === true)
        {
            return <Redirect to='/' />
        }
        return (
            <section className="packagesContainer">
                <div>
                    <h2 style={{color:"#fff", textAlign:"center"}}>Sign In or Register.</h2>
                    <i className="fas fa-chevron-circle-down"></i>
                    <div className="packages">
                    <div>
                        <h4 style={{color:"#fff"}}>Register</h4>
                        <br />
                        <div>
                            <AvForm style={{width: "500px"}}>
                                <AvField placeholder="First Name" type="name" name="firstName" value={this.state.firstName} onChange={e => this.change(e)} validate={{
                                    required: {value: true, errorMessage: 'Please enter a first name'},
                                    pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your first name must be composed only with letter and numbers'},
                                    minLength: {value: 1, errorMessage: 'Your first name must be between 6 and 16 characters'},
                                    maxLength: {value: 16, errorMessage: 'Your first name must be between 6 and 16 characters'}
                                }} />
                                <br/>
                                <AvField placeholder="Last Name" type="name" name="lastName" value={this.state.lastName} onChange={e => this.change(e)} validate={{
                                    required: {value: true, errorMessage: 'Please enter a last name'},
                                    pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your last name must be composed only with letter and numbers'},
                                    minLength: {value: 1, errorMessage: 'Your last name must be between 6 and 16 characters'},
                                    maxLength: {value: 16, errorMessage: 'Your last name must be between 6 and 16 characters'}
                                }} />
                                <br/>
                                <AvField placeholder="E-Mail Address" type="email" name="email" value={this.state.email} onChange={e => this.change(e)} validate={{email: true}} />
                                <br/>
                                <AvField placeholder="Password" type="password" name="password" value={this.state.password} onChange={e => this.change(e)} validate={{
                                    required: {value: true, errorMessage: 'Please enter a valid password'},
                                    pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your password must be composed only with letter and numbers'},
                                    minLength: {value: 6, errorMessage: 'Your password must be between 6 and 16 characters'},
                                    maxLength: {value: 16, errorMessage: 'Your password must be between 6 and 16 characters'}
                                }} />
                                <br/>
                                    <Button type="submit" color="secondary" size="lg" onClick={e => this.onRegister(e)} >Register</Button>
                            </AvForm>
                        </div>
                    </div>
                    <h2 style={{color:"#fff"}}>Or</h2>
                    <div>
                        <h4 style={{color:"#fff", marginBottom:"10"}}>Sign In</h4>
                        <br />
                        <div>
                            <AvForm style={{width: "500px"}}>
                                <AvField placeholder="E-Mail Address" type="email" name="emailLogin" value={this.state.emailLogin} onChange={e => this.change(e)} validate={{email: true}} />
                                <br/>
                                <AvField placeholder="Password" type="password" name="passwordLogin" value={this.state.passwordLogin} onChange={e => this.change(e)} validate={{
                                    required: {value: true, errorMessage: 'Please enter a valid password'},
                                    pattern: {value: '^[A-Za-z0-9]+$', errorMessage: 'Your password must be composed only with letter and numbers'},
                                    minLength: {value: 6, errorMessage: 'Your password must be between 6 and 16 characters'},
                                    maxLength: {value: 16, errorMessage: 'Your password must be between 6 and 16 characters'}
                                }} />
                                <br/>
                                    <Button type="submit" color="secondary" size="lg" onClick={e => this.onLogin(e)}>Sign In</Button>
                            </AvForm>
                            <br></br>
                            <br></br>
                            <Link to="/forgotdetails">
                                <Button color="secondary">Forgot Your E-Mail/Password?</Button>                            
                            </Link>
                        </div>
                    </div>
                    
                    </div>
            </div>
            </section>
        );
    }
}

export default Login;
