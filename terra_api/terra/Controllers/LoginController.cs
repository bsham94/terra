using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace terra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        // GET: api/Test
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Test/5
        [AllowAnonymous]
        [HttpPost("Login")]       
        public ActionResult Login(/*[FromBody] User u*/)
        {
            
            //if (!String.IsNullOrEmpty(u.user_id))
            //{
            //DAL dal = new DAL();
            //User temp = new User(u.user_id);
            //temp.Init(IDatabase.CommandType.Read);
            //temp.SetSelectVariables();
            //if (dal.Init() && temp.command != null)
            //{
            //    User user = (User)dal.Read(temp);
            //    if (user == null)
            //    {
            //        return Unauthorized();
            //    }
            
                    return Ok(Authorization.GetToken());
            //    }                
            //}
            //return BadRequest();
        }

        //firebase credentials
        //terraAnalysts @gmail.com
        //n0VVuR7tvHvFUnUNQGQF

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult Register(/*[FromBody] User u*/)
        {
            //if (!String.IsNullOrEmpty(u.user_id))
            //{
            //Replace with firebase stuff?
            //DAL dal = new DAL();
            //User temp = new User(u.user_id);
            //temp.Init(IDatabase.CommandType.Read);
            //temp.SetSelectVariables();
            //if (dal.Init() && temp.command != null)
            //{
            //    User user = (User)dal.Read(temp);
            //    if (user == null)
            //    {
            //        return Unauthorized();
            //    }


            return Ok(Authorization.GetToken());
            //    }                
            //}
            //return BadRequest();
        }




        //// POST: api/Test
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
