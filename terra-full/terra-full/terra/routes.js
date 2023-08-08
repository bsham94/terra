import React, {Component} from "react";
import { Route, Switch} from "react-router-dom";
import PrivateRoute from 'react-private-route';
import fire from './config/fire';
import { TransitionGroup, CSSTransition } from "react-transition-group";
import styled from "styled-components";
//import the modules
import Main from "./components/main";
import Login from "./components/login";
import Dashboard from "./components/dashboard";
import AccountSettings from "./components/accountsettings";
import ForgotDetails from "./components/forgotdetails";
import About from "./components/about";

class Routing extends Component {
    constructor(props){
        super(props);
        this.state = {
          user:{},
        }
      }

    componentDidMount() {
        this.authListener();
    }

    componentWillMount(){
      document.title = "T.E.R.R.A."
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
            <Wrapper>
                <Route render={({location}) => (
                    <TransitionGroup className="transition-group">
                        <CSSTransition
                        key={location.key}
                        timeout={{ enter: 300, exit: 300 }}
                        classNames="fade"
                        >
                            <Switch location={location}>
                                <Route exact path="/" component={Main} />
                                <Route exact path="/about" component={About} />
                                <PrivateRoute
                                    exact
                                    path="/login"
                                    component={Login}
                                    isAuthenticated={!this.state.user /* this method returns true or false */}
                                    redirect="/"
                                />
                                <PrivateRoute
                                    exact
                                    path="/forgotdetails"
                                    component={ForgotDetails}
                                    isAuthenticated={!this.state.user /* this method returns true or false */}
                                    redirect="/dashboard"
                                />
                                <PrivateRoute
                                    exact
                                    path="/dashboard"
                                    component={Dashboard}
                                    isAuthenticated={!!this.state.user /* this method returns true or false */}
                                    redirect="/login"
                                />
                                <PrivateRoute
                                    exact
                                    path="/accountsettings"
                                    component={AccountSettings}
                                    isAuthenticated={!!this.state.user /* this method returns true or false */}
                                    redirect="/login"
                                />
                            </Switch>
                        </CSSTransition>
                    </TransitionGroup>
                )} />
                
            </Wrapper>  
        )
    }
}

const Wrapper = styled.div`
  .fade-enter {
    opacity: 0.01;
  }

  .fade-enter.fade-enter-active {
    opacity: 1;
    transition: opacity 300ms ease-in;
  }

  .fade-exit {
    opacity: 1;
  }

  .fade-exit.fade-exit-active {
    opacity: 0.01;
    transition: opacity 300ms ease-in;
  }

  div.transition-group {
    position: relative;
  }

  section.route-section {
    position: absolute;
    width: 100%;
    top: 0;
    left: 0;
  }
`;

export default Routing;