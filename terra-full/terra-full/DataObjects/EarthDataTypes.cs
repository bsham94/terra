/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace terra
{
    public class EarthDataTypes : IDatabase
    {
        public string data_name;
        public string dataset_handler;
        // Function   : Fill
        // Description: Fills the object with data from the sql reader.
        // Paramaters : NpgsqlDataReader: The sql reader.
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            data_name = reader["data_name"].ToString();
            dataset_handler = reader["dataset_handler"].ToString();
        }

        // Function   : Init
        // Description: Sets the sql command.
        // Paramaters : CommandType: The type of sql command.
        // Returns    : void
        public override void Init(CommandType type)
        {
            switch (type)
            {
                case CommandType.Create:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "createearthdatatype";
                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getearthdatatypes";
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "updateearthdatatype";
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "deleteearthdatatype";
                    break;
                default:
                    break;
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
                command.Parameters.Add(new NpgsqlParameter("dataValue", data_name));
                command.Parameters.Add(new NpgsqlParameter("coordinates", dataset_handler));
            }
        }
    }
}
