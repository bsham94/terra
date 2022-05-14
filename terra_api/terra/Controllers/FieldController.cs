/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  FieldController.cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                                                                                 |
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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FieldController : ControllerBase
    {

        // Post: api/Field/FieldName
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("FieldName")]
        public ActionResult GetByName(Field f)
        {
            DAL dal = new DAL();
            //Create a temporary field object
            Field temp = new Field(f.user_id, f.field_name);
            //set database command to check ownership
            temp.Init(IDatabase.CommandType.CheckOwnership);
            //Set variables for database command.
            temp.CheckOwnershipVariables();
            //Check if dal initializes properly
            if (dal.Init())
            {
                if (dal.CheckOwnership(temp))
                {
                    //Set temp database command to read.
                    temp.Init(IDatabase.CommandType.Read);
                    //Set variables for database command.
                    temp.SetSelectVariables("field_name");
                    //Read form database.
                    Field field = (Field)dal.Read(temp);
                    //Generate geojson for field.
                    string geoJson = GeoJSONGeneration.FieldGeneration(new List<Field> { field });
                    //Retrun geogson string.
                    return Ok(geoJson);

                }
                //Field not found or does not belong to user.
                return NotFound(); //NotAuthorized               
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        // Post: api/Field/GetArea
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        //{user_id:"bSznI8kKorVztbyyslrr2PTrevO2",field_cords:"",field_name:"fhgvv"}
        [HttpPost("FieldArea")]
        public ActionResult GetArea(Field f)
        {
            DAL dal = new DAL();
            //Create a temporary field object
            Field temp = new Field(f.user_id, f.field_name);
            //set database command to check ownership
            temp.Init(IDatabase.CommandType.CheckOwnership);
            //Set variables for database command.
            temp.CheckOwnershipVariables();
            //Check if dal initializes properly
            if (dal.Init())
            {
                if (dal.CheckOwnership(temp))
                {
                    //Set temp database command to read.
                    temp.Init(IDatabase.CommandType.Read);
                    //Set variables for database command.
                    temp.SetSelectVariables("field_name");
                    //Read form database.
                    Field field = (Field)dal.Read(temp);
                    if (field == null)
                    {
                        return NotFound();
                    }
                    double fieldArea = field.field_coordinates.Area;
                    return Ok(JsonConvert.SerializeObject(fieldArea));
                }
                //Field not found or does not belong to user.
                return NotFound(); //NotAuthorized               
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        // Post: api/Field/FieldPosition/All
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("FieldCount")]
        public ActionResult GetFieldCount(Field f)
        {
            DAL dal = new DAL();
            //Create a temporary field object
            Field temp = new Field(f.user_id);
            //set database command to read from database.
            //Read all fields from database.
            temp.Init(IDatabase.CommandType.Count);
            temp.SetCountVariables("user_id");
            //Check if dal initializes properly
            if (dal.Init() && temp.command != null)
            {
                //Read all rows return form database.
                int count = dal.Count(temp);
                //If fields list is null, no field were found.
                if (count == 0)
                {
                    return NotFound();
                }
                return Ok(JsonConvert.SerializeObject(count));
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        // Post: api/Field/UsersFields
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("UsersFields")]
        public ActionResult GetByUserId(Field f)
        {
            if (!String.IsNullOrEmpty(f.user_id))
            {
                DAL dal = new DAL();
                //Create a temporary field object
                Field temp = new Field(f.user_id);
                //set database command to read from database.
                //Read with params means it will read from database using the specified column
                temp.Init(IDatabase.CommandType.ReadWithParameters);
                //Set variables for database command.
                temp.SetSelectVariables("user_id");
                //Check if dal initializes properly
                if (dal.Init())
                {
                    //Read all rows return form database.
                    List<IDatabase> fields = dal.ReadMore(temp);
                    //If fields list is null, no field were found.
                    if (fields == null)
                    {
                        return NotFound();
                    }
                    //Generate the geojson for the fields
                    string geoJson = GeoJSONGeneration.FieldGeneration(fields.Cast<Field>().ToList(), dal, f);
                    //Return the geojson string.
                    return Ok(geoJson);

                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }

        // Post: api/Field/FieldPosition/All
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("FieldPosition/All")]
        public ActionResult GetFieldPosition(Field f)
        {
            DAL dal = new DAL();
            //Create a temporary field object
            Field temp = new Field(f.user_id);
            //set database command to read from database.
            //Read all fields from database.
            temp.Init(IDatabase.CommandType.ReadAll);
            temp.SetSelectVariables("user_id");
            //Check if dal initializes properly
            if (dal.Init())
            {
                //Read all rows return form database.
                List<IDatabase> fields = dal.ReadMore(temp);
                //If fields list is null, no field were found.
                if (fields == null)
                {
                    return NotFound();
                }
                //Generate the geojson for the fields
                string geoJson = GeoJSONGeneration.FieldGeneration(fields.Cast<Field>().ToList(), dal, f);
                //Return the geojson string.
                return Ok(geoJson);
            }
            return BadRequest("Couldnt find connectionstring.");
        }
        // Post: api/Field/EarthData
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpGet("EarthData/All")]
        public ActionResult GetEarthDataSets()
        {
            DAL dal = new DAL();
            //Check if dal initializes properly
            if (dal.Init())
            {
                EarthDataTypes earthDataTypes = new EarthDataTypes();
                earthDataTypes.Init(IDatabase.CommandType.Read);
                List<EarthDataTypes> types = dal.ReadMore(earthDataTypes).Cast<EarthDataTypes>().ToList();
                List<SimpleEarthDataType> simpleTypes = new List<SimpleEarthDataType>();
                foreach (var item in types)
                {
                    simpleTypes.Add(new SimpleEarthDataType(item));
                }
                return Ok(simpleTypes);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // Post: api/Field/EarthData
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("EarthData/Dates/All")]
        public ActionResult GetEarthDataDates(DataHandler da)
        {
            DAL dal = new DAL();
            //Check if dal initializes properly
            if (dal.Init())
            {
                da.Init(IDatabase.CommandType.Read);
                da.SetSelectVariables();
                List<DataHandler> das = dal.ReadMore(da).Cast<DataHandler>().ToList();
                List<SimpleDataHandler> simpleDataHandlers = new List<SimpleDataHandler>();
                foreach (var item in das)
                {
                    simpleDataHandlers.Add(new SimpleDataHandler(item));
                }
                return Ok(simpleDataHandlers);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        // Post: api/Field/EarthData
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("EarthData/Dates")]
        public ActionResult GetEarthDataByDate(EarthDataSearch earthData)
        {
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
                    List<EarthData> earthDatas = dal.ReadMore(ed).Cast<EarthData>().ToList();
                    string geoJson = "";
                    if (earthDatas != null)
                    {
                        geoJson = GeoJSONGeneration.FieldDataGeneration(earthDatas.Cast<EarthData>().ToList(), earthData.Type);
                    }
                    if (earthDatas == null)
                    {
                        return NotFound();
                    }
                    return Ok(geoJson);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }


        // Post: api/Field/EarthData
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("EarthData")]
        public ActionResult GetFieldProperties([FromBody] EarthDataSearch earthData)
        {
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
                        //Put database rows into list.
                        datas = dal.ReadMore(ed);

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
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost]
        public ActionResult Post([FromBody] Field field)
        {
            if (field != null)
            {
                DAL dal = new DAL();
                field.Init(IDatabase.CommandType.Create);
                field.SetInsertVariables();
                if (dal.Init() && field.command != null)
                {
                    bool result = dal.NonQuery(field);
                    if (!result)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }

        // PUT: api/Field
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPut]
        public ActionResult Put([FromBody] Field f)
        {

            if (f != null && f.field_id != 0)
            {
                DAL dal = new DAL();
                f.Init(IDatabase.CommandType.Update);
                //Make sure theres atleast on value to update
                if (dal.Init() && f.command != null && f.SetUpdateVariables())
                {

                    bool result = dal.NonQuery(f);
                    if (!result)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }

        // DELETE api/Field/5
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpDelete]
        public ActionResult Delete([FromBody] Field f)
        {
            DAL dal = new DAL();
            Field temp = new Field(f.field_id);
            temp.Init(IDatabase.CommandType.Delete);
            temp.SetDeleteVariables();
            if (dal.Init())
            {
                bool result = dal.NonQuery(temp);
                if (!result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                return Ok();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
       
    }
}

