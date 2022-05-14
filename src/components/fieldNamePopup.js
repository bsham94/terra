/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  fieldNamePopup.js                                                                     |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file holds the component for the user to name their field                 |
*************************************************************************************************/


import React from 'react';
import styled from 'styled-components';

const Button = styled.button`
border: 1px solid #3770c6;
height: 100%;
background-color: rgb(84, 152, 255);
color: white;
font-size: 13px;
padding: 6px 12px;
border-radius: 6px;
cursor: pointer;
outline: none;
:hover {
  background-color: #3770c6;
}
`;


const BottomBar = styled.div`
position: absolute;
Bottom: 5px;
left: 5px;
right: 5px;
height: 40px;
display: flex;
justify-content: space-between;
align-items: center;
`;

const Input = styled.input`
  padding: 0.5em;
  margin: 0.5em;
  color: #2837ff;
  background: #f4feff;
  border: none;
  border-radius: 3px;
`;

class NamingPopup extends React.Component<Props, State> {
    constructor(props) {
        super(props);
        this.inputRef = React.createRef();
        this.state ={
            error:false,

        }
    }


    handleClick = name => event => {
        let list = [ name, this.inputRef.current.value]
        if(this.inputRef.current.value.length > 2 ){
            this.props.callBack(list);
        }
        else if (!name) {
            this.props.callBack(list);
        }
        else {
            this.setState({holder:true});
        }
        
    };
    
    /*
    https://www.styled-components.com/docs/advanced
    Since styled-components allows you to use arbitrary input as interpolations, you must be careful to sanitize that input. 
    Using user input as styles can lead to any CSS being evaluated in the user's browser that an attacker can place in your application.

    */
    render(){
        return (
            <div className='popup'>
                <div className='popup_inner'>
                    <h3 >Please name your field:</h3>

                    <Input ref={this.inputRef} placeholder="Enter field name..."  ></Input>

                    {
                        (this.state.holder) ?
                        <h5><font color="red">*Name needs to be at least 3 letters</font></h5> 
                    : null }


                    <BottomBar>
                        <Button onClick={this.handleClick(true)}>Confirm Name</Button>
                        <Button onClick={this.handleClick(false)}>Cancel new Field</Button>
                    </BottomBar>
                
                </div>
            </div>
        );
    }

}

export default NamingPopup;