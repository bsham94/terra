/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  FieldController.cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains all the functionality for the field controller               |
*************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//https://github.com/NetTopologySuite/NetTopologySuite/tree/develop/NetTopologySuite.Samples.Console/Geometries


namespace terra.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {

        // Post: api/Field/FieldName
        // Function   : GetByName
        // Description: Gets a field by name
        // Paramaters : field: A field object that should contain the user_id and field_name
        // Returns    : ActionResult: Returns the field data.
        [HttpPost("FieldName")]
        public ActionResult GetByName(Field f)
        {
            Logging.LogActivity(Activity.activites[0]);
            DAL dal = new DAL();
            if (!string.IsNullOrEmpty(f.user_id) && !string.IsNullOrEmpty(f.field_name))
            {
                //Create a temporary field object
                Field temp = new Field(f.user_id, f.field_name);
                //set database command to check ownership
                temp.Init(IDatabase.CommandType.CheckOwnership);
                //Set variables for database command.
                temp.CheckOwnershipVariables();
                //Check if dal initializes properly
                if (dal.Init())
                {
                    bool ownershipResult = false;
                    try
                    {
                        ownershipResult = dal.CheckOwnership(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (ownershipResult)
                    {
                        //Set temp database command to read.
                        temp.Init(IDatabase.CommandType.Read);
                        //Set variables for database command.
                        temp.SetSelectVariables("field_name");
                        Field field = null;
                        //Read form database.
                        try
                        {
                            field = (Field)dal.Read(temp);
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }
                        if (field == null)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                        //Generate geojson for field.
                        string geoJson = GeoJSONGeneration.FieldGeneration(new List<Field> { field });
                        //Retrun geogson string.
                        return Ok(geoJson);
                    }
                    //Field not found or does not belong to user.
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[0])); //NotAuthorized               
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[2]));
        }
        // Post: api/Field/GetArea
        // Function   : GetArea
        // Description: Gets the area of a given field.
        // Paramaters : f: The field object containing the field name and user id
        // Returns    : ActionResult: The field area.
        //{user_id:"bSznI8kKorVztbyyslrr2PTrevO2",field_cords:"",field_name:"fhgvv"}
        [HttpPost("FieldArea")]
        public ActionResult GetArea(Field f)
        {
            Logging.LogActivity(Activity.activites[1]);
            DAL dal = new DAL();
            if (!string.IsNullOrEmpty(f.user_id) && !string.IsNullOrEmpty(f.field_name))
            {
                //Create a temporary field object
                Field temp = new Field(f.user_id, f.field_name);
                //set database command to check ownership
                temp.Init(IDatabase.CommandType.CheckOwnership);
                //Set variables for database command.
                temp.CheckOwnershipVariables();
                //Check if dal initializes properly
                if (dal.Init())
                {
                    bool ownershipResult = false;
                    try
                    {
                        ownershipResult = dal.CheckOwnership(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (ownershipResult)
                    {
                        //Set temp database command to read.
                        temp.Init(IDatabase.CommandType.Read);
                        //Set variables for database command.
                        temp.SetSelectVariables("field_name");
                        Field field = null;
                        float fieldArea = 0;
                        try
                        {
                            //Read form database.
                            field = (Field)dal.Read(temp);
                            if (field == null)
                            {
                                return NotFound();
                            }
                            field.Init(IDatabase.CommandType.GetArea);
                            field.SetSelectVariables();
                            fieldArea = fieldArea = dal.GetArea(field);
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }
                        return Ok(JsonConvert.SerializeObject(fieldArea));
                    }
                    //Field not found or does not belong to user.
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[0])); //NotAuthorized               
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[2]));
        }
        // Post: api/Field/FieldPosition/All
        // Function   : GetFieldCount
        // Description: Gets the count of fields for a specific user.
        // Paramaters : Field: field object containing the user_id
        // Returns    : Actionresult: The field count.
        [HttpPost("FieldCount")]
        public ActionResult GetFieldCount(Field field)
        {
            Logging.LogActivity(Activity.activites[2]);
            DAL dal = new DAL();
            if (!string.IsNullOrEmpty(field.user_id))
            {
                //Create a temporary field object
                Field temp = new Field(field.user_id);
                //set database command to read from database.
                //Read all fields from database.
                temp.Init(IDatabase.CommandType.Count);
                temp.SetCountVariables("user_id");
                //Check if dal initializes properly
                if (dal.Init() && temp.command != null)
                {
                    int count = 0;
                    try
                    {
                        //Read all rows return form database.
                        count = dal.Count(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    return Ok(JsonConvert.SerializeObject(count));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }
        // Post: api/Field/UsersFields
        // Function   : GetByUserId
        // Description: Gets all field info for a specific user id.
        // Paramaters : Field: field object with a user id.
        // Returns    : ActionResult: All the field data for a specific user.
        [HttpPost("UsersFields")]
        public ActionResult GetByUserId(Field field)
        {
            Logging.LogActivity(Activity.activites[3]);
            if (!string.IsNullOrEmpty(field.user_id))
            {
                DAL dal = new DAL();
                //Create a temporary field object
                Field temp = new Field(field.user_id);
                //set database command to read from database.
                //Read with params means it will read from database using the specified column
                
                //changed from read with params, test to make sure it still works
                temp.Init(IDatabase.CommandType.ReadWithParameters);
                //Set variables for database command.
                temp.SetSelectVariables("user_id");
                //Check if dal initializes properly
                if (dal.Init())
                {
                    List<IDatabase> fields = null;
                    try
                    {
                        //Read all rows return form database.
                        fields = dal.ReadMore(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    //If fields list is null, no field were found.
                    if (fields == null)
                    {
                        return NotFound(new TerraExcpetion(TerraExcpetion.messages[18]));
                    }
                    //Generate the geojson for the fields
                    string geoJson = GeoJSONGeneration.FieldGeneration(fields.Cast<Field>().ToList(), dal, field);
                    //Return the geojson string.
                    return Ok(geoJson);

                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }

        // Post: api/Field/FieldPosition/All
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("FieldPosition/All")]
        public ActionResult GetFieldPosition(Field field)
        {
            Logging.LogActivity(Activity.activites[4]);
            DAL dal = new DAL();
            if (!string.IsNullOrEmpty(field.user_id))
            {
                //Create a temporary field object
                Field temp = new Field(field.user_id);
                //set database command to read from database.
                //Read all fields from database.
                temp.Init(IDatabase.CommandType.ReadAll);
                temp.SetSelectVariables("user_id");
                //Check if dal initializes properly
                if (dal.Init())
                {
                    List<IDatabase> fields = null;
                    try
                    {
                        //Read all rows return form database.
                        fields = dal.ReadMore(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    //If fields list is null, no field were found.
                    if (fields == null)
                    {
                        return NotFound(new TerraExcpetion(TerraExcpetion.messages[6]));
                    }
                    //Generate the geojson for the fields
                    string geoJson = GeoJSONGeneration.FieldGeneration(fields.Cast<Field>().ToList(), dal, field);
                    //Return the geojson string.
                    return Ok(geoJson);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }
        // Post: api/Field/EarthData
        // Function   : GetEarthDataSets
        // Description: Gets all available data sets.
        // Paramaters : none
        // Returns    : ActionResult: A list of all possible data sets.
        [HttpGet("EarthData/All")]
        public ActionResult GetEarthDataSets()
        {
            Logging.LogActivity(Activity.activites[5]);
            DAL dal = new DAL();
            //Check if dal initializes properly
            if (dal.Init())
            {
                EarthDataTypes earthDataTypes = new EarthDataTypes();
                earthDataTypes.Init(IDatabase.CommandType.Read);
                List<EarthDataTypes> types = null;
                try
                {
                    List<IDatabase> temp = dal.ReadMore(earthDataTypes);
                    if (temp != null)
                    {
                        types = temp.Cast<EarthDataTypes>().ToList();
                    }
                }
                catch (Exception e)
                {
                    Logging.LogError(e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                }
                if (types != null)
                {
                    List<SimpleEarthDataType> simpleTypes = new List<SimpleEarthDataType>();
                    foreach (var item in types)
                    {
                        simpleTypes.Add(new SimpleEarthDataType(item));
                    }
                    return Ok(simpleTypes);
                }
                return NotFound();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
        }

        // Post: api/Field/EarthData/Dates/All
        // Function   : GetEarthDataDates
        // Description: Gets all available days for a specific data type.
        // Paramaters : Datahandler: Datahandler object with the data type
        // Returns    : ActionResult: List of days.
        [HttpPost("EarthData/Dates/All")]
        public ActionResult GetEarthDataDates(DataHandler dataHandler)
        {
            Logging.LogActivity(Activity.activites[6]);
            DAL dal = new DAL();
            //Check if dal initializes properly
            if (!string.IsNullOrEmpty(dataHandler.DataType))
            {
                if (dal.Init())
                {
                    dataHandler.Init(IDatabase.CommandType.Read);
                    dataHandler.SetSelectVariables();
                    List<DataHandler> dataHandlers = null;
                    try
                    {
                        List<IDatabase> temp = dal.ReadMore(dataHandler);
                        if (temp != null)
                        {
                            dataHandlers = temp.Cast<DataHandler>().ToList();
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (dataHandlers != null)
                    {
                        List<SimpleDataHandler> simpleDataHandlers = new List<SimpleDataHandler>();
                        foreach (var item in dataHandlers)
                        {
                            simpleDataHandlers.Add(new SimpleDataHandler(item));
                        }
                        return Ok(simpleDataHandlers);
                    }
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[5]));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[4]));
        }

        // Post: api/Field/EarthData/Dates
        // Function   : GetEarthDataByDate
        // Description: Gets earth data for a specific date.
        // Paramaters : EarthData: Earthdata object with field_id and earthdata type
        // Returns    : ActionResult: List of earth data.
        [HttpPost("EarthData/Dates")]
        public ActionResult GetEarthDataByDate(EarthDataSearch earthData)
        {
            Logging.LogActivity(Activity.activites[7]);
            if (earthData.Id != 0)
            {
                DAL dal = new DAL();
                //Check if dal initializes properly
                if (dal.Init())
                {
                    //check that earthdata.type is a correct table name.
                    EarthData ed = new EarthData();
                    ed.data_id = earthData.Id;
                    ed.type = earthData.Type;
                    ed.Init(IDatabase.CommandType.ReadAll);
                    ed.SetSelectVariables("field_id");
                    List<IDatabase> earthDatas = null;
                    try
                    {
                        earthDatas = dal.ReadMore(ed);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    string geoJson = "";
                    if (earthDatas != null)
                    {
                        geoJson = GeoJSONGeneration.FieldDataGeneration(earthDatas.Cast<EarthData>().ToList(), earthData.Type);
                    }
                    if (earthDatas == null)
                    {
                        return NotFound(new TerraExcpetion(TerraExcpetion.messages[6]));
                    }
                    return Ok(geoJson);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[7]));
        }


        // Post: api/Field/EarthData
        // Function   : GetFieldProperties
        // Description: Gets all data for a specific type and field.
        // Paramaters : Earthdata: Earthdata object with a field_id and data type.
        // Returns    : All earthdata for a specific field.
        [HttpPost("EarthData")]
        public ActionResult GetFieldProperties([FromBody] EarthDataSearch earthData)
        {
            Logging.LogActivity(Activity.activites[8]);
            if (earthData.Id != 0)
            {
                DAL dal = new DAL();
                //Check if dal initializes properly
                if (dal.Init())
                {
                    List<IDatabase> datas = null;
                    //If eather type equals earthdata, query database
                    if (earthData.Type == "Soil Moisture" || earthData.Type == "Surface Temperature" || earthData.Type == "Precipitation" || earthData.Type == "Vegetation Indices")
                    {
                        EarthData ed = new EarthData(earthData);
                        //set database command to read from database.
                        ed.Init(IDatabase.CommandType.Read);
                        //Set to select by field_id
                        ed.SetSelectVariables("field_id");
                        try
                        {
                            //Put database rows into list.
                            datas = dal.ReadMore(ed);
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }

                        string geoJson = "";
                        if (datas != null)
                        {
                            //Cast data to Earthdata
                            //Returns the Earthdata objects in geojson format.
                            geoJson = GeoJSONGeneration.FieldDataGeneration(datas.Cast<EarthData>().ToList(), "MoistureData");
                        }
                        if (datas == null)
                        {
                            return NotFound();
                        }
                        return Ok(geoJson);
                    }
                    return BadRequest();
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }

        // POST: api/Field
        // Function   : Post
        // Description: Creates a field.
        // Paramaters : Field: The field object to insert into the database.
        // Returns    : ActionResult: Result of the field creation
        [HttpPost]
        public ActionResult Post([FromBody] Field field)
        {
            Logging.LogActivity(Activity.activites[9]);
            if (field.Validate())
            {
                DAL dal = new DAL();
                field.Init(IDatabase.CommandType.Create);
                field.SetInsertVariables();
                if (dal.Init() && field.command != null)
                {
                    bool result = false;
                    try
                    {
                        result = dal.NonQuery(field);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (!result)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[15]));
                    }
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[8]));
        }

        // PUT: api/Field
        // Function   : Put
        // Description: Updates a field.
        // Paramaters : Field: The field object to update.
        // Returns    : The result of the update.
        // {user_id:"",field_name:"",new_field_name:""}
        [HttpPut]
        public ActionResult Put([FromBody] Field field)
        {
            Logging.LogActivity(Activity.activites[10]);
            if (field.ValidateUserId(field.user_id) && field.ValidateName(field.field_name)&&field.ValidateName(field.new_field_name))
            {
                DAL dal = new DAL();
                field.Init(IDatabase.CommandType.CheckOwnership);
                field.CheckOwnershipVariables();               
                //Make sure theres atleast on value to update
                if (dal.Init())
                {
                    bool ownershipResult = false;
                    try
                    {
                        ownershipResult = dal.CheckOwnership(field);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (ownershipResult)
                    {
                        field.Init(IDatabase.CommandType.Update);
                        if (field.SetUpdateVariables())
                        {
                            bool result = false;
                            try
                            {
                                result = dal.NonQuery(field);
                            }
                            catch (Exception e)
                            {
                                Logging.LogError(e.Message);
                                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                            }
                            if (!result)
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[16]));
                            }
                            return Ok();
                        }
                        return BadRequest();
                    }
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[0]));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[2]));
        }

        // DELETE api/Field
        // Function   : Delete
        // Description: Deletes a field from the database.
        // Paramaters : Field: field object containing a user_id and field_name
        // Returns    : the result of the delete.
        // {user_id:"",field_name:""}
        [HttpDelete]
        public ActionResult Delete([FromBody] Field field)
        {
            Logging.LogActivity(Activity.activites[11]);
            DAL dal = new DAL();
            if (!string.IsNullOrEmpty(field.user_id) && !string.IsNullOrEmpty(field.field_name))
            {
                field.Init(IDatabase.CommandType.CheckOwnership);
                field.CheckOwnershipVariables();
                if (dal.Init())
                {
                    bool ownershipResult = false;
                    try
                    {
                        ownershipResult = dal.CheckOwnership(field);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (ownershipResult)
                    {
                        bool result = false;
                        try
                        {
                            Field temp = new Field(field.user_id, field.field_name);
                            temp.Init(IDatabase.CommandType.ReadWithParameters);
                            temp.SetSelectVariables("name_and_id");
                            Field newField = (Field)dal.Read(temp);
                            if (newField!=null)
                            {
                                temp.field_id = newField.field_id;
                                temp.Init(IDatabase.CommandType.Delete);
                                temp.SetDeleteVariables("field_name");
                                result = dal.NonQuery(temp);
                            }  
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }
                        if (!result)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[17]));
                        }
                        return Ok();
                    }
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[0]));
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[2]));
        }

    }
}

