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
justifyContent: center;
align-items: center;
`;


class MapAlert extends React.Component<Props, State> {
    constructor(props) {
        super(props);
        this.inputRef = React.createRef();
        this.state ={

        }
    }

    
    /*
    https://www.styled-components.com/docs/advanced
    Since styled-components allows you to use arbitrary input as interpolations, you must be careful to sanitize that input. 
    Using user input as styles can lead to any CSS being evaluated in the user's browser that an attacker can place in your application.

    */
    render(){
        return (

                <div className='popup_alert'>
                    <h3 ><font color={this.props.message[2]}>{this.props.message[0]}</font></h3>

                    <h5>{this.props.message[1]}</h5>

                    <BottomBar>
                        <Button onClick={this.props.callBack}>Acknowledge</Button>
                    </BottomBar>
                
                </div>
        );
    }

}

export default MapAlert;