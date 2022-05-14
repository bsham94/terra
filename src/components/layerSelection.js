/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  layerSelection.js                                                                     |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file holds all of the layer selection                                     |
*************************************************************************************************/


import React from 'react';
import API from './api';
import Select from 'react-select';

var myAPI = new API();

class LayerSelection extends React.Component<Props, State> {
    constructor(props) {
        super(props);
        this.state = {
            layers:[],
            radio:false,
            allFields:true,
            myFields:false,
            soilMoisture:true,
            filterState: [],
            filtersDone: false,
            loaded: false,
            soilCompression: false,
            availableDays: false,
            listAvailableDays:[],
            selectedOption: "00/00/0000",
            currentState: "Select Data",
            currentStateStyle: "waitingIndicator",

        }
        // props filters
    }

    handleChange = name => event => {
        if(name === "allFields"){
            this.setState({myFields:false});
            //console.log("allFields ");
            let list = [ event.target.name, name]
            this.props.callBack(list);
        } else if (name === "myFields"){
            this.setState({allFields:false});
            //console.log("myFields");
            let list = [ event.target.name, name]
            this.props.callBack(list);
        }
        if(event.target.name !== "layer"){
            this.setState({ [name]: event.target.checked });
            //console.log("!layer");
        } else if (this.props.isGettingData === false) {
            this.setState({selectedOption: ""});
            var tempfilter = this.state.filterState;
            for (let i = 0; i < this.props.filters.length; i++) {
                if( this.props.filters[i].dataset_handler !== event.target.getAttribute('handler')){
                    tempfilter[this.props.filters[i].data_name] = false
                }
                else {
                    tempfilter[this.props.filters[i].data_name] = true
                    //console.log("Setting this filter to true: ", this.props.filters[i].data_name);
                }
                
            }
            //tempfilter.every(false);
            //tempfilter[event.target.key] = event.target.checked;
            //console.log("tempFilter: ", tempfilter, " target: ", event.target," ", this.props.filters[0].data_name , " event: ", event.target.checked)
            this.setState({ filterState: tempfilter });

            myAPI.getfieldDataDates(event.target.attributes.handler.value).then((availableDays)=>{
                if(availableDays[1] === ""){
                    
                    //console.log(availableDays[0]);
                    let tempDays = [];
                    for (let i = 0; i < availableDays[0].length; i++) {  
                        //"dataSetName":"precipitation3","date":"2019-03-23T00:00:00"}
                        let date = availableDays[0][i].date;
                        
                        let stuff = ""
                        if (date != null){
                            date = date.split('T');
                            stuff = date[0]
                        }
                        else {
                            stuff = availableDays[0][i].date;
                        }

                        let temp = {
                            
                            value: availableDays[0][i].dataSetName,
                            label: stuff
                        }          
                        tempDays.push(temp)
                        //console.log("Current available Days: ", availableDays[0], " count: ", i, " Max length: ", availableDays[0].length)
                        //console.log("temp available days: ", temp);

                    }
                    this.setState({
                        listAvailableDays: tempDays,
                        availableDays: true
                    })

                    //console.log("available days: ", this.state.availableDays);
                }
                else {
                    this.setState({
                        availableDays: false
                    })

                    //console.log("available days else : ", this.state.availableDays);
                }
                
            });
            //console.log("new name: ", this.state.listAvailableDays);

            let list = [ event.target.name, name]
            this.props.callBack(list);
        }
        
        
    };

    createFilters = () => {
        /*

            <li><input type="checkbox" name="layer" value={soilMoisture} checked={soilMoisture} onChange={this.handleChange('soilMoisture')}/>Soil Moisture</li>
            <li><input type="checkbox" name="layer" value={radio} />Precipitation</li>
            <li><input type="checkbox" name="layer" value={radio} />Air Quality</li>

        */
        
        let table = []
        
        //console.log("this is the props filters", this.props.filters);
        for (let i = 0; i < this.props.filters.length; i++) {            
            //console.log("Current Prop: ", this.props.filters[i].data_name, " count: ", i, " Max length: ", this.props.filters.length)
            table.push(<li><input type="checkbox" handler={this.props.filters[i].dataset_handler} key={i} name="layer" value={this.state.filterState[this.props.filters[i].data_name]} checked={this.state.filterState[this.props.filters[i].data_name]} onChange={this.handleChange(this.props.filters[i].data_name)}/>{this.props.filters[i].data_name}</li>)     
        }
        
        return table
    }

    handleDateChange = (currOption) => {
        
        //Splitting the time off label: "2019-03-29T00:00:00"

        /*var date = currOption.label.split('T');
        if(date != null){
            console.log(date, "state: ", this.state.selectedOption);
            this.setState({selectedOption: date[0]});
            
        }*/
        if (!this.props.isGettingData){
            this.state.selectedOption = currOption;
            this.setState({selectedOption: currOption});
            //console.log("state: ", this.state.selectedOption);
            this.props.dateData(currOption.value);
        }
        
    }

    componentWillReceiveProps(nextProps){
        if(nextProps.isGettingData!==this.props.isGettingData){
            this.changeCurrentState();
        }

        
    }


    componentDidMount(){
        var newFilterState = []

        if(this.props.filters != null && !this.state.filtersDone){
            for (let i = 0; i < this.props.filters.length; i++) {
                newFilterState.push(this.props.filters[i].data_name:false);
            }
            this.setState({filterState: newFilterState});
            this.setState({filtersDone: true});
        }


    }

    changeCurrentState(){
        if(!this.props.isGettingData){
            this.setState({
                currentState: "Grabbing Data",
                currentStateStyle:"loadingIndicator"
            });
            
        } else {
            this.setState({
                currentState: "Select Data",
                currentStateStyle: "waitingIndicator"
            });
        }

    }
    /*

        <ul><li><input type="checkbox" name="layer" value={soilCompression} checked={soilCompression} onChange={this.handleChange('soilCompression')}/>Soil Compression</li></ul>
                <p><font color="green">_____________</font></p>

    */

    /*{this.props.filters.map(function(filter){
        return <li key={ features[0].properties.data_name }>{features[0].properties.data_name}</li>;
    })}*/
    //.features[0].properties.data_name
    //style={{ height: 750, width: 175 }}
    render() {
        var { allFields, myFields, selectedOption, listAvailableDays} = this.state;
        return (

            <div className="layerSelection">
                <h5> Field Selection</h5>
                <p><font color="green">_____________</font></p>
                <ul>
                    <li><input type="checkbox" name="fieldQuery" value={allFields} checked={allFields} onChange={this.handleChange('allFields')}/>All Fields</li>
                    <li><input type="checkbox" name="fieldQuery" value={myFields} checked={myFields} onChange={this.handleChange('myFields')}/>My Fields</li>
                </ul>
                <p><font color="green">_____________</font></p>


                <h5 className="padBoi">Layer Selection</h5>
                <p><font color="green">_____________</font></p>

                
                <ul>
                    {(this.state.filtersDone) ?
                        this.createFilters() 
                    : null}
                    
                </ul>
                {(this.state.availableDays) ?
                    <div><h5 className="padBoi">Available Days</h5>

                    <p><font color="green">_____________</font></p></div>
                : null}
                <ul>
                    {(this.state.availableDays) ?
                        <div className="SearchDateOptions">
                            <Select
                            placeholder="Which Day"
                            defaultValue = {selectedOption}
                            value= {this.state.selectedOption}
                            onChange={this.handleDateChange}
                            options={listAvailableDays}
                            />
                        </div>
                    : null}
                        
                </ul>

                <div className={this.state.currentStateStyle}>
                        {this.state.currentState}
                </div>
                
            </div>

        )
    }



}

export default LayerSelection;