/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  about.js                                                                            |
|   Date: April 2, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the information about the application and the specific
        tools used to create it             |
*************************************************************************************************/
import React, { Component } from 'react';

class About extends Component {
    componentDidMount() {
        window.scrollTo(0, 0);
    }
    render() {
        return (
            <React.Fragment>
                <section className="how">
                    <i className="fas fa-map-marker-alt"></i>
                    <center><h2>HOW DO WE DO IT</h2></center>
                    <div>
                        <p>With TERRA our main objective is to give farmers one place to get all their 
                        accumulated information about their fields and easily visualize this information 
                        to allow for quick understanding. Another Objective of this application will be 
                        to create a community around this information for the benefit of everyone involved, 
                        we aim to give the tools to aid in improving soil conditions to allow for higher yields 
                        and over all better production.</p>
                    <br />
                        <div>
                            <div>
                                <h2>REACT.JS</h2>
                                <h4>FRONT-END</h4>
                                <p>Within this application we are using ReactJS to fill in the front End of the Website. 
                                    ReactJS allows to use various different packages including the React Framework in order to create and design a modern feeling website.</p> 
                            </div>
                            <div>
                                <h2>.NET CORE</h2>
                                <h4>BACK-END</h4>
                                <p>For our api we are using .Net Core’s web api. 
                                    The web api will be used by our front end to communicate with our database. 
                                    We chose .net because we have experience in developing in the .Net framework.</p> 
                            </div>
                            <div>
                                <h2>POSTGRESQL</h2>
                                <h4>DATABASE</h4>
                                <p>For our database we will be using PostgresSql and PostGIS. 
                                    The PostgresSql database will be used to store our user information,
                                     spatial data, and the earth data we retrieve from the NASA api.
                                      We chose PostgresSQL as our database because of its excellent spatial data extension PosGIS.</p> 
                            </div>
                        </div>
                    </div>
                </section>
                <section className="how">
                    <i className="fas fa-angle-down"></i>
                    <center><h2>Features</h2></center>
                    <div>
                    <br />
                        <div class="Row">
                            <div class="Column">
                                <h2>Field Data Visualization</h2>
                            </div>
                            <div class="Column">
                                <h2>Sharing Field Data (User's Choice)</h2>
                            </div>
                            <div class="Column">
                                <h2>Storing Field Data</h2>
                            </div>
                        </div>
                        <div class="Row">
                            <div class="Column">
                                <h2>Third Party Data Integration</h2>
                            </div>
                            <div class="Column">
                                <h2>Multi-Day Data Retrieval</h2>
                            </div>
                            <div class="Column">
                                <h2>Data Powered Field Suggestions</h2>
                            </div>
                        </div>
                    </div>
                </section>
            </React.Fragment>
        );
    }
}

export default About;
