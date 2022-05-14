/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  Activity.cs                                                                           |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains all the functionality for logging api activity               |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace terra
{
    public class Activity : IDatabase
    {
        public string methodName;
        public string description;
        public string className;
        public static string[] activites =
        {
            "Searching for field by name",
            "Getting field area",
            "Getting users field count",
            "Searching for fields by user id.",
            "Searching for all fields.",
            "Retrieving all available data sets.",
            "Retrieving all available days for data type.",
            "Retriveing all data for data type by date. ",
            "Retrieving all earth data for field.",
            "Creating field",
            "Updating field",
            "Deleteing field",
            "Searching for user by id.",
            "Sharing field with user",
            "Creating user.",
            "Updating user.",
            "Deleting user.",
            "Getting field specific by user name and field name.",
            "Creating a field specific",
            "Deleting a field specific",
            "Updating a field specific"
        };


        public Activity(string methodName, string description, string className)
        {
            this.description = description;
            this.methodName = methodName;
            this.className = className;
        }

        // Function   : Fill
        // Description: Fills the object with data from the sql reader.
        // Paramaters : NpgsqlDataReader: The sql reader.
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            throw new NotImplementedException();
        }
        // Function   : Init
        // Description: Sets the sql command.
        // Paramaters : CommandType: The type of sql command.
        // Returns    : void
        public override void Init(CommandType type)
        {
            if (type.Equals(CommandType.Log))
            {
                command = new NpgsqlCommand()
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "logactivity"
                };
            }
        }
        // Function   : SetInsertVariable
        // Description: Sets the variables for inserting to the database.
        // Paramaters : none
        // Returns    : void
        public override void SetInsertVariables()
        {
            if (command != null)
            {
                command.Parameters.Add(new NpgsqlParameter("function_name", methodName));
                command.Parameters.Add(new NpgsqlParameter("class_name", className));
                command.Parameters.Add(new NpgsqlParameter("description", description));
            }
        }
    }
}

