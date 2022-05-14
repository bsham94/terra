using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace terra.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoilCompactionController : ControllerBase
    {
        // POST: api/Field
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("SoilCompaction")]
        public ActionResult SoilCompaction([FromBody] SoilCompaction soilCompaction)
        {
            if (soilCompaction != null)
            {
                DAL dal = new DAL();
                soilCompaction.Init(IDatabase.CommandType.Create);
                foreach (var item in soilCompaction.soilCompaction)
                {
                    soilCompaction.SetInsertVariables();
                    if (dal.Init() && soilCompaction.command != null)
                    {
                        bool result = dal.NonQuery(soilCompaction);
                        if (!result)
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                        //return Ok();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    soilCompaction.currentCoordinate++;
                    soilCompaction.currentSoilCompaction++;
                }
                return Ok();
                //return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }
        // POST: api/Field
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [HttpPost("SoilCompaction/Get")]
        public ActionResult GetSoilCompaction([FromBody] SoilCompaction soilCompaction)
        {
            if (soilCompaction != null)
            {
                DAL dal = new DAL();
                soilCompaction.Init(IDatabase.CommandType.ReadAll);
                soilCompaction.SetSelectVariables("field_id");
                if (dal.Init() && soilCompaction.command != null)
                {
                    List<IDatabase> list = dal.ReadMore(soilCompaction);
                    if (list == null)
                    {
                        return NotFound();
                    }
                    //Generate the geojson for the fields
                    string geoJson = GeoJSONGeneration.SoilCompactionGeneration((SoilCompaction)list[0]);
                    //Return the geojson string.
                    return Ok(geoJson);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return BadRequest();
        }
    }
}