/************************************************************************************************|
|   Project:  TERRA                                                                              |
|   File:  UserController .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                                                                                 |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Net.Http;
using System.Runtime.Serialization;
using Json.Net;
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
        // GET api/user?id=asldfkasldkfjalsdfkjaslkdfj
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpGet]
        public ActionResult Get([FromQuery(Name ="id")]string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                DAL dal = new DAL();
                User temp = new User(id);
                temp.Init(IDatabase.CommandType.Read);
                temp.SetSelectVariables();
                if (dal.Init())
                {
                    User user = (User)dal.Read(temp);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    return Ok(new SimpleUser(user));
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }

        // POST api/Users
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost]
        public ActionResult Post([FromBody] User u)
        {
            DAL dal = new DAL();
            if (u != null)
            {
                u.Init(IDatabase.CommandType.Create);
                u.SetInsertVariables();
                if (dal.Init() && u.command != null)
                {
                    bool result = dal.NonQuery(u);
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

        // PUT api/users
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPut]
        public ActionResult Put ([FromBody] User u)
        {
            if (u != null && !string.IsNullOrEmpty(u.id))
            {
                DAL dal = new DAL();
                u.Init(IDatabase.CommandType.Update);

                if (dal.Init() && u.command != null && u.SetUpdateVariables())
                {
                    bool result = dal.NonQuery(u);
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

        // DELETE api/users?id=asldfkasldkfjalsdfkjaslkdfj
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpDelete]
        public ActionResult Delete([FromQuery(Name = "id")]string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                DAL dal = new DAL();
                User temp = new User(id);
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
            return BadRequest();
        }
    }
}
