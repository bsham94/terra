import React, { Component } from 'react';
import {Redirect} from "react-router-dom";
import {Button} from "reactstrap";
import { AvForm, AvField } from 'availity-reactstrap-validation';
import fire from '../config/fire';
import { confirmAlert } from 'react-confirm-alert';

class ForgotDetails extends Component {
    constructor(props) {
        super(props);
        this.change = this.change.bind(this);
        this.passwordReset = this.passwordReset.bind(this);
        
        this.state = {
            email: ''
        }
    }

    change = (e) => {
        this.setState({
            [e.target.name]: e.target.value
        });
    };

    passwordReset = e => {
        var auth = fire.auth();

        auth.sendPasswordResetEmail(this.state.email).then(() => {
            confirmAlert({
                title: "Password Reset Set",
                message: "The Password Reset Link has been sent to " + this.state.email,
                buttons: [
                  {
                    label: 'Close',
                  }
                ]
              })
        }).catch(function(error) {
            confirmAlert({
                title: "Error",
                message: error.message,
                buttons: [
                  {
                    label: 'Try Again',
                  }
                ]
              })
        });

    }

    render() {
        if (this.state.toHome === true)
        {
            return <Redirect to='/' />
        }
        return (
            <section className="packagesContainer">
                <div>
                    <h2 style={{color:"#fff", textAlign:"center"}}>Forgot Your Password?</h2>
                    <i className="fas fa-chevron-circle-down"></i>
                    <div className="packages">
                        <div style={{paddingLeft:"410px"}}>
                            <h4 style={{color:"#fff", marginBottom:"10"}}>Password Reset</h4>
                            <br />
                            <div>
                                <AvForm style={{width: "500px"}}>
                                    <AvField placeholder="E-Mail Address" type="email" name="email" value={this.state.email} onChange={e => this.change(e)} validate={{email: true}} />
                                    <br/>
                                    <Button type="submit" color="secondary" size="lg" onClick={e => this.passwordReset(e)}>Send Password Reset</Button>
                                </AvForm>
                            </div>
                        </div>
                    </div>
            </div>
            </section>
        );
    }
}

export default ForgotDetails;
