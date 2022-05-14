/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  header.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains the header portion connecting to main.js in order to
                  display the homepage               |
*************************************************************************************************/
import React, { Component } from 'react';
import {Link} from "react-router-dom";
import {Button} from "reactstrap";
import fire from "./../config/fire";

class Header extends Component {
  constructor(props){
    super(props);
    this.state = {
      user:{},
    }
  }

  componentDidMount() {
    this.authListener();
    
    window.scrollTo(0, 0);
  }

  authListener() {
    fire.auth().onAuthStateChanged((user) => {
      if (user) {
        this.setState({ user });
        //localStorage.setItem('user', user.uid);
      } else {
        this.setState({ user: null });
        //localStorage.removeItem('user');
      }
    });
  }

  render() {
    return (
      <header>
        <slider>
          <slide></slide>
          <slide></slide>
          <slide></slide>
          <slide></slide>
          <slide></slide>
        </slider>
          <div>
            <h1>T.E.R.R.A</h1> <h2>The Earth Research Results Application</h2>
            <p>A Farmer's Tool</p>
            {this.state.user ? <Button tag={Link} to="/dashboard">Dashboard  <i className="fas fa-chevron-circle-right"></i></Button> : <Button tag={Link} to="/login">Sign In / Register  <i className="fas fa-chevron-circle-right"></i></Button>}
          </div>
        
      </header>
    );
  }
}


export default Header;
