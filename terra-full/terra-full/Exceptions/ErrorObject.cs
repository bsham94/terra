/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  ErrorObject.cs                                                                        |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains all the functionality for logging api errors                 |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace terra
{
    public class ErrorObject : IDatabase
    {
        public string methodName;
        public string description;
        public string className;
        public ErrorObject(string methodName, string description,string className)
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
                    CommandText = "logerror"
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
