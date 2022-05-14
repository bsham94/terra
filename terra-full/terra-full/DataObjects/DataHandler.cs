 /************************************************************************************************|
|   Project: Terra                                                                                |
|   File:  Datahandler.cs                                                                         |
|   Date: March 8, 2019                                                                           |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                           |
|   Description: This file contains all the functinality for the data handler.                    |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;

namespace terra
{
    public class DataHandler : IDatabase
    {
        //public int FieldId;
        public string DataSetName;
        public string DataType;
        public DateTime Date;

        public DataHandler() { }
        [JsonConstructor]
        public DataHandler(string DataType)
        {
            this.DataType = DataType;
        }

        // Function   : Fill
        // Description: Fills the object with data from the sql reader.
        // Paramaters : NpgsqlDataReader: The sql reader.
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            DataSetName = reader[0].ToString();
            Date = DateTime.Parse(reader[1].ToString());
        }
        // Function   : Init
        // Description: Sets the sql command.
        // Paramaters : CommandType: The type of sql command.
        // Returns    : void
        public override void Init(CommandType type)
        {
            switch (type)
            {               
                case CommandType.Read:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "gethandler"
                    };
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
            throw new NotImplementedException();
        }
        // Function   : SetSelecttVariable
        // Description: Sets the variables for reading from the database.
        // Paramaters : none
        // Returns    : void
        public void SetSelectVariables(string searchType = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (searchType)
                {
                        default:
                            command.Parameters.Add(new NpgsqlParameter("datatype", DataType));
                        break;
                }
            }
        }

        // Function   : ClearParameters
        // Description: Clears the any set parameters.
        // Paramaters : none
        // Returns    : void
       private void ClearParameters()
        {
            if (command != null)
            {
                if (command.Parameters.Count != 0)
                {
                    command.Parameters.Clear();
                }
            }
        }
    }
}
