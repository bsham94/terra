/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  farmSimulation.js                                                                     |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file holds farm simulation component to run it with the unity wrapper     |
*************************************************************************************************/

import React from 'react';
import Unity, { UnityContent } from "react-unity-webgl";


class FarmSimulation extends React.Component<Props, State> {
    constructor(props) {
        super(props);

        //https://github.com/jeffreylanters/react-unity-webgl-test/
        //https://github.com/elraccoone/react-unity-webgl/issues/31
        this.unityContent = new UnityContent(
            "Build/test.json",
            "Build/UnityLoader.js"
        );

    }
    

    render() {
        
        return <Unity unityContent = {this.unityContent}/>;
    }



}

export default FarmSimulation;