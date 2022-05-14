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


        // Post: api/Field/FieldName
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        // ex data {field_id:37,user_id:"bSznI8kKorVztbyyslrr2PTrevO2",field_cords:"",field_name:"fhgvv"}
        [HttpPost]
        public ActionResult GetFieldSpecifics(Field f)
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

                    FieldSpecific fieldSpecific = new FieldSpecific(f.field_id);

                    //Set temp database command to read.
                    fieldSpecific.Init(IDatabase.CommandType.Read);
                    //Set variables for database command.
                    fieldSpecific.SetSelectVariables();
                    //Read form database.
                    List<FieldSpecific> fieldSpecifics = null;
                    List<IDatabase> list = dal.ReadMore(fieldSpecific);
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
                return NotFound(); //NotAuthorized               
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPost("Create")]
        public ActionResult CreateFieldSpecifics(FieldSpecific fieldSpecific)
        {
            if (fieldSpecific != null)
            {
                DAL dal = new DAL();
                fieldSpecific.Init(IDatabase.CommandType.Create);
                fieldSpecific.SetInsertVariables();
                if (dal.Init() && fieldSpecific.command != null)
                {
                    bool result = dal.NonQuery(fieldSpecific);
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



        [HttpDelete]
        public ActionResult DeleteFieldSpecifics(FieldSpecific fieldSpecific)
        {
            DAL dal = new DAL();
            FieldSpecific temp = new FieldSpecific(fieldSpecific.field_id);
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
    


        [HttpPut]
        public ActionResult UpdateFieldSpecific(FieldSpecific fieldSpecific)
        {

            if (fieldSpecific != null && fieldSpecific.field_id != 0)
            {
                DAL dal = new DAL();
                fieldSpecific.Init(IDatabase.CommandType.Update);
                //Make sure theres atleast on value to update
                if (dal.Init() && fieldSpecific.command != null && fieldSpecific.SetUpdateVariables())
                {

                    bool result = dal.NonQuery(fieldSpecific);
                    if (!result)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                    return Ok();
                }
                return BadRequest("Nothing to update");
            }
            return BadRequest();
        }
    }
}