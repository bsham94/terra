/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class Device : IDatabase
    {
        public string user_id;
        public int device_id;
        public int device_type_id;
        public int field_id;
        public int device_data;
        public float coordinates;
        public Device() { }

        public Device(string user_id, int device_id, int device_type_id, int field_id, float coordinates, int device_data)
        {
            this.user_id = user_id;
            this.device_id = device_id;
            this.device_type_id = device_type_id;
            this.field_id = field_id;
            this.coordinates = coordinates;
            this.device_data = device_data;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {

        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Init(CommandType type)
        {
            switch (type)
            {
                case CommandType.Create:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "createdevice";
                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "getdevice";
                    break;
                case CommandType.ReadWithParameters:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "GetDeviceByUserId";
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "updatedevice";
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "deletedevice";
                    break;
                default:
                    break;
            }
        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void SetInsertVariables()
        {
            if (command != null)
            {
                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                command.Parameters.Add(new NpgsqlParameter("device_id", device_id));
                command.Parameters.Add(new NpgsqlParameter("device_type_id", device_type_id));
                command.Parameters.Add(new NpgsqlParameter("field_id", field_id ));
                command.Parameters.Add(new NpgsqlParameter("coordinates", coordinates));
                command.Parameters.Add(new NpgsqlParameter("device_data", device_data));
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void SetSelectVariables(string searchType = null)
        {
            if (command != null)
            {
                switch (searchType)
                {
                    case ("user_id"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        break;
                    case ("user_field"):
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("device_id", field_id));
                        break;
                }
            }
        }


    }
}
