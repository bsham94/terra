import React, { Component } from 'react';

import Footer from "./components/footer";
import Routes from "./routes";
import TopNavBarSignIn from './components/navSignIn';
import TopNavBarSignOut from './components/navSignOut';
import fire from './config/fire';

class App extends Component {
  constructor(props){
    super(props);
    this.state = {
      user:{},
    }
  }

  componentDidMount() {
    this.authListener();
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
    //console.log(this.state.user);
    return (
      <div className="main">
        {this.state.user ? (<TopNavBarSignOut />) : (<TopNavBarSignIn />)}
        <main>
          <Routes />
        </main>  
        <Footer />
      </div>
    );
  }
}

export default App;
