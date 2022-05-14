/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  EarthData.cs                                                                          |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:  This file contains all the functionality for earthdata.                        |
*************************************************************************************************/
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class EarthData : IDatabase
    {
        public int data_id;
        public float dataValue;
        public string type;
        public NetTopologySuite.Geometries.Point coordinates;

        public EarthData() { }

        public EarthData(EarthDataSearch ed)
        {
            data_id = ed.Id;
            type = ed.Type;
        }

        // Function   : Fill
        // Description: Fills the object with data from the sql reader.
        // Paramaters : NpgsqlDataReader: The sql reader.
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            float.TryParse(reader["dataValue"].ToString(),out dataValue);
            coordinates = (NetTopologySuite.Geometries.Point)reader["cords"];
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
                    command.CommandText = "createearthdata";
                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getfielddata";
                    break;
                case CommandType.ReadWithParameters:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getmoisturebycoordinates";
                    break;
                case CommandType.ReadAll:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getearthdatabydate";
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "updateearthdata";
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "deleteearthdata";
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
                command.Parameters.Add(new NpgsqlParameter("dataValue", dataValue));
                command.Parameters.Add(new NpgsqlParameter("coordinates", coordinates));
            }
        }
        // Function   : SetSelecttVariable
        // Description: Sets the variables for reading from the database.
        // Paramaters : none
        // Returns    : void
        public void SetSelectVariables(string searchType = null)
        {
            if (command != null)
            {
                switch (searchType)
                {
                    case ("coordinates"):
                        command.Parameters.Add(new NpgsqlParameter("coordinates", coordinates));
                        break;
                    case ("field_id"):
                        command.Parameters.Add(new NpgsqlParameter("fieldid", data_id));
                        command.Parameters.Add(new NpgsqlParameter("type", type));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("data_id", data_id));
                        break;
                }

            }
        }


    }
}
