/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  TerraException.cs                                                                     |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains all the error messages the api returns to the front end      |
*************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class TerraExcpetion
    {
        public string message;
        public static string[] messages = 
        {
            "Field not owned by current user.",
            "Could not initalize connection to database.",
            "Field name or user id was not provided.",
            "Failed to find User id.",
            "Failed to find Data type.",
            "Data type does not have multiple days.",
            "No Earthdata found",
            "Failed to find Field Id.",
            "Field Invalid.",
            "Could not update Field Specific",
            "Could not delete Field Specific",
            "Could not create Field Specific",
            "Could not create User",
            "Could not delete User",
            "Could not update User",
            "Could not create field",
            "Could not update field",
            "Could not delete field",
            "No fields found",
            "An error occured when processing the database request."
        };


        public TerraExcpetion(string message)
        {
            this.message = message;
        }



    }
}
