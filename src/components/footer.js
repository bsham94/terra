/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  footer.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains the footer               |
*************************************************************************************************/
import React, { Component } from 'react';

class Footer extends Component {
  render() {
    return (
            
        <footer>
        <p style={{color:"#fff"}}></p>
        <br />
        <p style={{color:"#fff"}}>Follow Us!</p>
        <br />
        <ul>
        <li><a href="https://www.facebook.com/TERRA-Analysts-604508393353679/"><i className="fab fa-facebook-f"></i></a></li>
        <li><a href="https://twitter.com/terraanalysts"><i className="fab fa-twitter"></i></a></li>
        <li><a href="mailto:terraanalysts@gmail.com"><i className="far fa-envelope"></i></a></li>

        </ul>
        <br />
        <p style={{color:"#fff"}}>&copy; T.E.R.R.A 2019</p>
        </footer>
    );
  }
}

export default Footer;

