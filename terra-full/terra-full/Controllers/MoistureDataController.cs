using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;

namespace terra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoistureDataController : ControllerBase
    {
        // GET: api/MoistureData
        [HttpGet]
        public ActionResult Get([FromQuery(Name ="id")]int id)
        {
            if (id != 0)
            {
                DAL dal = new DAL();
                MoistureData temp = new MoistureData(id);
                temp.init(IDatabase.CommandType.GetEarthData);
                temp.SetSelectVariables("field_id");
                if (dal.init() && temp.command != null)
                {
                    List<IDatabase> moistureData = dal.ReadMore(temp);
                    if (moistureData == null)
                    {
                        return NotFound("Field with that id could not be found.");
                    }
                    return Ok(new SimpleMoistureData((MoistureData)moistureData[0]));
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }

        // GET: api/MoistureData/5
        [HttpGet]
        public ActionResult Get([FromQuery(Name ="longitude")]float longitude, [FromQuery(Name = "laditude")] float laditude)
        {
                DAL dal = new DAL();
                MoistureData temp = new MoistureData(new NpgsqlPoint(longitude,laditude));
                temp.init(IDatabase.CommandType.ReadWithParameters);
                temp.SetSelectVariables("coordinates");
                if (dal.init() && temp.command != null)
                {
                    MoistureData moistureData = (MoistureData)dal.Read(temp);
                    if (moistureData == null)
                    {
                        return NotFound();
                    }
                    return Ok(new SimpleMoistureData(moistureData));
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public ActionResult Get([FromQuery(Name = "type")]string type, [FromQuery(Name = "longitude")]float longitude, [FromQuery(Name = "laditude")] float laditude)
        {
            if (!String.IsNullOrEmpty(type))
            {
                DAL dal = new DAL();
                EarthData temp = EarthDataFactory.GetObject(type);
                temp.init(IDatabase.CommandType.ReadWithParameters);
                temp.SetSelectVariables("coordinates");
                if (dal.init() && temp.command != null)
                {
                    MoistureData moistureData = (MoistureData)dal.Read(temp);
                    if (moistureData == null)
                    {
                        return NotFound();
                    }
                    return Ok(new SimpleMoistureData(moistureData));
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }


        [HttpGet]
        public ActionResult Get([FromQuery(Name = "type")]string type, [FromQuery(Name = "id")] int id)
        {
            if (!String.IsNullOrEmpty(type))
            {
                DAL dal = new DAL();
                MoistureData temp = new MoistureData();
                temp.init(IDatabase.CommandType.ReadWithParameters);
                temp.SetSelectVariables("coordinates");
                if (dal.init() && temp.command != null)
                {
                    MoistureData moistureData = (MoistureData)dal.Read(temp);
                    if (moistureData == null)
                    {
                        return NotFound();
                    }
                    return Ok(new SimpleMoistureData(moistureData));
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }



        // POST: api/MoistureData
        [HttpPost]
        public ActionResult Post([FromBody] string value)
        {
            return BadRequest();
        }

        // PUT: api/MoistureData/5
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
