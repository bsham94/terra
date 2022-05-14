/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  UserController .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality for the user controller               |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Net.Http;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace terra.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // POST api/Users/id
        // Function   : Get
        // Description: Gets a specific user based on user id
        // Paramaters : User: the user  object
        // Returns    : Actionresult: Returns the user data.
        [HttpPost("id")]
        public ActionResult Get([FromBody] User u)
        {
            Logging.LogActivity(Activity.activites[12]);
            if (!String.IsNullOrEmpty(u.id))
            {
                DAL dal = new DAL();
                User temp = new User(u.id);
                temp.Init(IDatabase.CommandType.Read);
                temp.SetSelectVariables();
                if (dal.Init())
                {
                    User user = null;
                    try
                    {
                        user = (User)dal.Read(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (user == null)
                    {
                        return NotFound();
                    }
                    return Ok(new SimpleUser(user));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }

        // POST api/Users/id
        // Function   : Get
        // Description: Gets a specific user based on user id
        // Paramaters : User: the user  object
        // Returns    : Actionresult: Returns the user data.
        // {id:"RyeHW0fo8kU3hSzGYbZDv5l33352",user_name:"sam"}
        [HttpPost("ShareField")]
        public ActionResult CreateSharedFields([FromBody] User u)
        {
            Logging.LogActivity(Activity.activites[13]);
            if (!String.IsNullOrEmpty(u.user_name) && !String.IsNullOrEmpty(u.id))
            {
                DAL dal = new DAL();
                User temp = new User();
                temp.user_name = u.user_name;
                temp.Init(IDatabase.CommandType.ReadWithParameters);
                temp.SetSelectVariables("user_name");
                if (dal.Init())
                {
                    User user = null;
                    try
                    {
                        user = (User)dal.Read(temp);
                        if (user != null)
                        {
                            temp.Init(IDatabase.CommandType.ShareFields);
                            temp.id = u.id;
                            temp.shared_field_id = user.id;
                            temp.SetSharedFieldVariables();
                            dal.NonQuery(temp);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (user == null)
                    {
                        return NotFound();
                    }
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }

        // POST api/Users
        // Function   : Post
        // Description: Creates a user
        // Paramaters : User: The user to create
        // Returns    : ActionResult: the result
        [HttpPost]
        public ActionResult Post([FromBody] User u)
        {
            Logging.LogActivity(Activity.activites[14]);
            DAL dal = new DAL();
            if (u != null)
            {
                u.Init(IDatabase.CommandType.Create);
                u.SetInsertVariables();
                if (dal.Init() && u.command != null)
                {
                    bool result = false;
                    try
                    {
                        result = dal.NonQuery(u);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (!result)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[12]));
                    }
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest();
        }

        // PUT api/users
        // Function   : Put
        // Description: Updates the user
        // Paramaters : User: the user object
        // Returns    : Actionresult: the result
        [HttpPut]
        public ActionResult Put ([FromBody] User u)
        {
            Logging.LogActivity(Activity.activites[15]);
            if (u != null && !string.IsNullOrEmpty(u.id))
            {
                DAL dal = new DAL();                
                if (dal.Init())
                {
                    IDatabase returnValue = null;
                    User temp = new User(u.id);
                    temp.Init(IDatabase.CommandType.Read);
                    temp.SetSelectVariables();
                    try
                    {
                        returnValue = dal.Read(temp);
                    }
                    catch (Exception e)
                    {
                        Logging.LogError(e.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                    }
                    if (returnValue != null)
                    {
                        u.Init(IDatabase.CommandType.Update);
                        if (u.SetUpdateVariables())
                        {
                            bool result = false;
                            try
                            {
                                result = dal.NonQuery(u);
                            }
                            catch (Exception e)
                            {
                                Logging.LogError(e.Message);
                                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[19]));
                            }
                            if (!result)
                            {
                                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[13]));
                            }
                            return Ok();
                        }
                        return BadRequest();
                    }
                    return NotFound();                  
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }

        // DELETE api/users
        // Function   : Delete
        // Description: Deletes a user.
        // Paramaters : User: the user object
        // Returns    : actionresult: the result
        [HttpDelete]
        public ActionResult Delete([FromBody]User u)
        {
            Logging.LogActivity(Activity.activites[16]);
            if (!String.IsNullOrEmpty(u.id))
            {
                DAL dal = new DAL();
                User temp = new User(u.id);
                temp.Init(IDatabase.CommandType.Delete);
                temp.SetDeleteVariables();
                if (dal.Init())
                {
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
                        return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[14]));
                    }
                    return Ok();
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new TerraExcpetion(TerraExcpetion.messages[1]));
            }
            return BadRequest(new TerraExcpetion(TerraExcpetion.messages[3]));
        }
    }
}
