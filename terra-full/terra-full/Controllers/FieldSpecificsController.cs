/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  FieldSpecificsController .cs                                                          |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains all the functionality for the fieldspecifics controller      |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace terra.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FieldSpecificsController : ControllerBase
    {


        // Post: api/Field/FieldSpecific
        // Function   : GetFieldSpecifics
        // Description: Gets the field specifics
        // Paramaters : Field: the field object
        // Returns    : Actionresult: the field specifics for the field.
        // ex data {user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2",field_name:"old farm"}
        [HttpPost]
        public ActionResult GetFieldSpecifics(Field f)
        {
            Logging.LogActivity(Activity.activites[17]);
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
                    List<FieldSpecific> fieldSpecifics = null;
                    List<IDatabase> list = null;
                    try
                    {
                        //Get field_id using the name and user id
                        temp.Init(IDatabase.CommandType.ReadWithParameters);
                        temp.SetSelectVariables("name_and_id");
                        Field newField = (Field)dal.Read(temp);
                        FieldSpecific fieldSpecific = null;
                        if (newField != null)
                        {
                            fieldSpecific = new FieldSpecific(newField.field_id);
                            //Set temp database command to read.
                            fieldSpecific.Init(IDatabase.CommandType.Read);
                            //Set variables for database command.
                            fieldSpecific.SetSelectVariables();
                            //Read form database.
                            list = dal.ReadMore(fieldSpecific);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (list != null)
                    {
                        fieldSpecifics = list.Cast<FieldSpecific>().ToList();
                    }
                    if (fieldSpecifics == null)
                    {
                        return NotFound();
                    }
                    List<SimpleFieldSpecific> simpleTypes = new List<SimpleFieldSpecific>();
                    foreach (var item in fieldSpecifics)
                    {
                        simpleTypes.Add(new SimpleFieldSpecific(item));
                    }
                    return Ok(simpleTypes);

                }
                //Field not found or does not belong to user.
                return NotFound(new TerraExcpetion(TerraExcpetion.messages[0])); //NotAuthorized               
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
        }
        // Post: api/FieldSpecific/Create
        // Function   : GetFieldSpecifics
        // Description: Gets the field specifics
        // Paramaters : Field: the field object
        // Returns    : Actionresult: the field specifics for the field.
        //{user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2",field_name:"old farm",yield:10.0,seedPlanted:"beans",fertilizer_use:"None",pesticide_use:"none"}
        [HttpPost("Create")]
        public ActionResult CreateFieldSpecifics(FieldSpecific fieldSpecific)
        {
            Logging.LogActivity(Activity.activites[18]);
            if (fieldSpecific != null)
            {
                DAL dal = new DAL();
                //Create a temporary field object
                Field temp = new Field(fieldSpecific.user_id, fieldSpecific.field_name);
                //set database command to check ownership
                temp.Init(IDatabase.CommandType.CheckOwnership);
                //Set variables for database command.
                temp.CheckOwnershipVariables();
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
 
                        bool result = false;
                        try
                        {
                            //Get field_id using the name and user id
                            temp.Init(IDatabase.CommandType.ReadWithParameters);
                            temp.SetSelectVariables("name_and_id");
                            Field newField = (Field)dal.Read(temp);                         
                            if (newField!=null)
                            {
                                fieldSpecific.field_id = newField.field_id;
                                fieldSpecific.Init(IDatabase.CommandType.Create);
                                fieldSpecific.SetInsertVariables();
                                result = dal.NonQuery(fieldSpecific);
                            }
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }
                        if (!result)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[11]));
                        }
                        return Ok();
                    }
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[0]));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest();

        }


        // Delete: api/FieldSpecific
        // Function   : DeleteFieldSpecifics
        // Description: Deletes the field specifics
        // Paramaters : FieldSpecific: the fieldspecific object
        // Returns    : Actionresult: the delete result
        //{user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2", field_name:"old farm", field_id:4} //will delete all for field_id
        [HttpDelete]
        public ActionResult DeleteFieldSpecifics(FieldSpecific fieldSpecific)
        {
            Logging.LogActivity(Activity.activites[19]);
            DAL dal = new DAL();
            if (dal.Init())
            {
                Field tempField = new Field(fieldSpecific.user_id, fieldSpecific.field_name);
                tempField.Init(IDatabase.CommandType.CheckOwnership);
                tempField.CheckOwnershipVariables();
                bool ownershipResult = false;
                try
                {
                    ownershipResult = dal.CheckOwnership(tempField);
                }
                catch (Exception e)
                {
                    Logging.LogError(e.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                }
                if (ownershipResult)
                {
                    //Get field_id using the name and user id
                    tempField.Init(IDatabase.CommandType.ReadWithParameters);
                    tempField.SetSelectVariables("name_and_id");
                    Field newField = (Field)dal.Read(tempField);
                    if (newField != null)
                    {
                        FieldSpecific temp = new FieldSpecific(newField.field_id);
                        temp.Init(IDatabase.CommandType.Delete);
                        temp.SetDeleteVariables();
                        bool result = false;
                        try
                        {
                            result = dal.NonQuery(temp);
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }
                        if (!result)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[10]));
                        }
                        return Ok();
                    }
                    return NotFound();
                }
                return NotFound(new TerraExcpetion(TerraExcpetion.messages[0]));

            }
            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
        }


        // Put: api/FieldSpecific
        // Function   : UpdateFieldSpecifics
        // Description: Updates the field specifics
        // Paramaters : FieldSpecific: the fieldspecific object
        // Returns    : Actionresult: the Update result
        //{user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2",field_name:"old farm",yield:50.0,seedPlanted:"beans",fertilizer_use:"",pesticide_use:""}
        //{user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2",field_name:"old farm",yield:0,seedPlanted:"beans",fertilizer_use:"",pesticide_use:"asdfasdf"}
        //{user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2",field_name:"old farm",yield:0,seedPlanted:"beans",fertilizer_use:"asdfasdf",pesticide_use:""}
        //{user_id:"N4rmYMsx5QM0U6b9gd3IWPgIw0X2",field_name:"old farm",yield:110,seedPlanted:"beans",fertilizer_use:"asdfasdf",pesticide_use:"asdfasdf"}
        [HttpPut]
        public ActionResult UpdateFieldSpecific(FieldSpecific fieldSpecific)
        {
            Logging.LogActivity(Activity.activites[20]);
            if (fieldSpecific != null)
            {
                DAL dal = new DAL();
                fieldSpecific.Init(IDatabase.CommandType.Update);
                //Make sure theres atleast on value to update
                if (dal.Init() && fieldSpecific.command != null)
                {
                    Field tempField = new Field(fieldSpecific.user_id, fieldSpecific.field_name);
                    tempField.Init(IDatabase.CommandType.CheckOwnership);
                    tempField.CheckOwnershipVariables();

                    bool ownershipResult = false;
                    try
                    {
                        ownershipResult = dal.CheckOwnership(tempField);
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
                            //Get field_id using the name and user id
                            tempField.Init(IDatabase.CommandType.ReadWithParameters);
                            tempField.SetSelectVariables("name_and_id");
                            Field newField = (Field)dal.Read(tempField);
                            if (newField!=null)
                            {
                                fieldSpecific.field_id = newField.field_id;
                                if (fieldSpecific.SetUpdateVariables())
                                {
                                    result = dal.NonQuery(fieldSpecific);
                                }
                            }                          
                        }
                        catch (Exception e)
                        {
                            Logging.LogError(e.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                        }
                        if (!result)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[9]));
                        }
                        return Ok();
                    }
                    return NotFound(new TerraExcpetion(TerraExcpetion.messages[0]));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest();
        }
    }
}