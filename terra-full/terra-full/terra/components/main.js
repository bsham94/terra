/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  main.js                                                                            |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality for the homepage in connection with
                  header.js                |
*************************************************************************************************/
import React, { Component } from 'react';
import { Card, CardImg, CardText, CardGroup, CardBody,
  CardTitle, CardSubtitle, Button, CardFooter } from 'reactstrap';
import Header from "./header";
import sam from "./../assets/images/0.jpg";
import ben from "./../assets/images/1.jpg";
import zaid from "./../assets/images/2.jpg";
import hyunbin from "./../assets/images/3.jpg";

class Main extends Component {
  componentDidMount() {
    window.scrollTo(0, 0);
  }
  render() {
    return (
      <React.Fragment>
        <Header />  
        <section className="intro">
        
            <div className="cards">  
              <h2><i className="fas fa-compass"></i>< br/>Accurate Information</h2>
              <p style={{color:"#fff"}}>With T.E.R.R.A, you are able to effectively view and monitor your fields crops with ease.</p> 
 
            </div>
            <div className="cards">  
              <h2><i className="fas fa-kiwi-bird"></i>< br/>A Lot To See</h2>
              <p style={{color:"#fff"}}>Various different settings & options that allow users to see everything they need in order to effectively monitor their fields</p> 

            </div>
            <div className="cards">  
              <h2><i className="fas fa-id-card-alt"></i>< br/>Certified Staff</h2>
              <p style={{color:"#fff"}}>Endless Support from Staff that are trained to support with all issues</p> 

            </div>
          
        
         
        </section>
        
        <section className="authors">
        <i className="fas fa-frog"></i>
          <center><h2>Who are We?</h2></center>
          <CardGroup>
            <Card body inverse style={{ backgroundColor: '#222', borderColor: '#222' }} outline color="secondary">
              <CardImg src={sam} alt="sam"/>
              <CardBody>
                <CardTitle><h1>Samuel Guta</h1></CardTitle>
                <CardSubtitle><h4>NASA API Functionality Implementation</h4></CardSubtitle>
                <CardText>As a software developer, I’m always trying to be better and learn more. 
             My primary focus is on the back-end although I am a full stack developer for mobile and desktop applications. 
             Skilled in C#(.net/.net core), Python, C/C++, SQL (PostgreSQL & SQL Server), and Java. 
             I’m aimed to be a new grad from Conestoga College’s Software Engineer Technology with an advanced diploma.</CardText>
                <CardFooter>
                  <Button href="https://github.com/samdabullet">GitHub</Button>
                </CardFooter>
              </CardBody>
            </Card>
            <Card body inverse style={{ backgroundColor: '#222', borderColor: '#222' }} outline color="secondary">
              <CardImg src={ben} alt="ben"/>
              <CardBody>
                <CardTitle><h1>Ben Shamas</h1></CardTitle>
                <CardSubtitle><h4>General Application Functionality</h4></CardSubtitle>
                <CardText>I am currently a third year Software Engineering Technology student at Conestoga College. As an aspiring software developer, 
                  I like to solve difficult problems and am always looking for something to challenge my programming skills.
                   I entered the field of software development because programming has been an interest of mine since high school. 
                   I have experience in C#(.net/.net core), C/C++, SQL (PostgreSQL & SQL Server), and Java.</CardText>
                <CardFooter>
                  <Button href="https://github.com/bsham94">GitHub</Button>
                </CardFooter>
              </CardBody>
            </Card>
            <Card body inverse style={{ backgroundColor: '#222', borderColor: '#222' }} outline color="secondary">
              <CardImg src={zaid} alt="zaid"/>
              <CardBody>
                <CardTitle><h1>Zaid Omar</h1></CardTitle>
                <CardSubtitle><h4>UI Design Interface</h4></CardSubtitle>
                <CardText>Experienced Software Developer with a demonstrated history of completing projects in multiple industries. 
             Skilled in C#, C/C++, HTML, Node.js, ReactJS and React Native. Strong software professional with an Ontario College Advanced Diploma
              focused in Software Engineering Technology from Conestoga College.</CardText>
                <CardFooter>
                  <Button href="https://github.com/ZaidOm">GitHub</Button>
                </CardFooter>
              </CardBody>
            </Card>
            <Card body inverse style={{ backgroundColor: '#222', borderColor: '#222' }} outline color="secondary">
              <CardImg src={hyunbin} alt="kevin"/>
              <CardBody>
                <CardTitle><h1>Hyunbin Park</h1></CardTitle>
                <CardSubtitle><h4>Data Visualization</h4></CardSubtitle>
                <CardText>Software Engineering Technology student with an avid interest in the Objective oriented pattern programming and the game development, who brings potent communication skills and a professional, collaborative manner to the workplace. 
A self-learner who enjoys working in a team to deliver consistent, well-designed solutions that meet or exceed customer requirements. </CardText>
                <CardFooter>
                  <Button href="https://github.com/hyunbin7303">GitHub</Button>
                </CardFooter>
              </CardBody>
            </Card>
        </CardGroup>
        </section>

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
           <div class="Row">
            <div class="Column">
              <h2>NASA EARTH DATA</h2>
              <h4>OPEN DATA</h4>
              <p>We are utilizing Nasa's earth data as our back bone to the data gathering and displaying for the farm fields. 
                The reason for using this is they have vast amounts of data free to use how we see fit. 
                As NASA says on their website "NASA's data policy ensures that all NASA data are available fully, openly, and without restrictions."</p> 
              </div>
              <div class="Column">
                <h2>MAPGL</h2>
                <h4>VISUALIZATION</h4>
                <p>To visualize data on the map, this application uses Map Gl api which is a javascript library.
                   It enables program to render interactive maps with detail information so that user can get data about their specific farmlands.</p> 
              </div>
            </div>
        </div>
        </section>
        <section className="standalone-img">
          <div className="image2"></div>
        </section>

       
        </React.Fragment>
    );
  }
}

export default Main;
