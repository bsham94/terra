/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  navsignout.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains the navigation bar allowing the user to sign out and its routes                 |
*************************************************************************************************/
import React, { Component } from 'react';
//import the Link module
import {Link} from "react-router-dom";
import {Button} from "reactstrap";
import logo from './../assets/images/Logo.png';
import { confirmAlert } from 'react-confirm-alert';

import {
  Navbar,
  NavbarBrand,
  Nav,
  NavItem,} from 'reactstrap';
import fire from '../config/fire';

class TopNavBar extends Component {
  constructor(props) {
    super(props);
    this.logout = this.logout.bind(this);

  }

  logout() {
    confirmAlert({
      title: "Sign Out",
      message: "Are you Sure you want to Sign Out?",
      buttons: [
        {
          label: 'Yes',
          onClick: () => {
            fire.auth().signOut();
          }
        },
        {
          label: 'No',
        }
      ]
    })
    
  }

  render() {
    return (
      <Navbar style={{backgroundColor: "#222"}} light expand="md" sticky={'top'} className="shadow-sm py-0 border-dark border-bottom">
          <ul>
          <li>
            <NavbarBrand href="/"><img src={logo} alt="Logo" /></NavbarBrand>
          </li>
          <div id="nav-icon">
            <span></span>
            <span></span>
            <span></span>
          </div>

          </ul>
          <Nav className="ml-auto" navbar>
              <NavItem>
                <Button outline color="secondary" tag={Link} to="/">Home</Button>
              </NavItem>
              <NavItem>
                <Button outline color="secondary" tag={Link} to="/about">About</Button>
              </NavItem>
              <NavItem>
                <Button outline color="secondary" tag={Link} to="/Dashboard">Dashboard</Button>
              </NavItem>
              <NavItem>
                <Button outline color="secondary" tag={Link} to="/AccountSettings">Account Settings</Button>
              </NavItem>
              <NavItem>
                <Button outline color="danger" onClick={this.logout}>Sign Out</Button>
              </NavItem>
          </Nav>
            
        </Navbar>
    );
  }
}

export default TopNavBar;
