/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    
    public class Field : IDatabase
    {
        public string user_id;
        public int field_id;
        public Polygon field_coordinates;
        public string field_name;

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public Field()
        {
            user_id = "";
            field_id = 0;
            field_coordinates = null;
            field_name = "";
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public Field(int field_id) {

            user_id = "";
            this.field_id = field_id;
			field_coordinates = null;
            field_name = "";

        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public Field(string user_id)
        {

            this.user_id = user_id;
            field_id = 0;
            field_coordinates = null;
            field_name = "";

        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public Field(string user_id, string field_name)
        {
            this.user_id = user_id;
            this.field_name = field_name;        
            field_id = 0;
            field_coordinates = null;

        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public Field(string user_id, Polygon field_cords, string field_name, int field_id = 0)
        {
            this.user_id = user_id;
            this.field_id = field_id;
            this.field_coordinates = field_cords;
            this.field_name = field_name;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        [JsonConstructor]
        public Field(string user_id, GeoJSON.Net.Geometry.Polygon field_cords, string field_name, int field_id = 0)
        {
            this.user_id = user_id;
            this.field_id = field_id;
            if (field_cords != null )
            {
                List<Coordinate> coordinates = new List<Coordinate>();
                foreach (var item in field_cords.Coordinates)
                {
                    foreach (var otherItem in item.Coordinates)
                    {
                        coordinates.Add(new Coordinate(otherItem.Longitude, otherItem.Latitude));
                    }
                }
                this.field_coordinates = new Polygon(new LinearRing(coordinates.ToArray()));
            }
            else
            {
                this.field_coordinates = null;
            }
            this.field_name = field_name;
        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            user_id = reader["user_id"].ToString();
            field_id = (int) reader["field_id"];
            field_coordinates = (Polygon)reader["field_cords"];
            field_name = reader["field_name"].ToString();
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
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "createfield"

                    };
                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getfieldbyname"
                    };
                    break;
                case CommandType.ReadWithParameters:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "GetFieldByUserId"
                    };
                    break;
                case CommandType.ReadAll:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "GetAllFields"
                    };
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "updatefield"
                    };
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "DeleteField"
                    };
                    break;
                case CommandType.CheckOwnership:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "checkownership"
                    };
                    break;
                case CommandType.Count:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getfieldscount"
                    };
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
            ClearParameters();
            if (command != null)
            {
                //command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
                command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void SetSelectVariables(string searchType = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (searchType)
                {
                    case ("user_id"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        break;
                    case ("field_name"):
                        command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                }             
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void CheckOwnershipVariables(string type = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (type)
                {
                    case ("all"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                        break;
                }
                                    
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void SetCountVariables(string type = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (type)
                {
                    default:    
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        break;
                }

            }
        }
        // Function   : 
        // Description: 
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
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public void SetDeleteVariables(string searchType = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (searchType)
                {
                    case ("user_id"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                }
            }
        }

        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public bool SetUpdateVariables()
        {
            ClearParameters();

            //Check for command == null
            command.Parameters.Add(new NpgsqlParameter("field_id", field_id));

            if (!string.IsNullOrEmpty(field_name) && !string.IsNullOrEmpty(user_id) && field_coordinates != null)
            {
                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
                command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
            }
            else
            {
                if (!string.IsNullOrEmpty(field_name) && !string.IsNullOrEmpty(user_id)) 
                {
                    command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                    command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                }
                else if (!string.IsNullOrEmpty(user_id) && field_coordinates != null)
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", "user_id"));
                    command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
                    command.Parameters.Add(new NpgsqlParameter("updatevalue", user_id));
                }
                else if (!string.IsNullOrEmpty(field_name) && field_coordinates != null)
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", "field_name"));
                    command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
                    command.Parameters.Add(new NpgsqlParameter("updatevalue", field_name));
                }
                else if (!string.IsNullOrEmpty(field_name))
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", 2));
                    command.Parameters.Add(new NpgsqlParameter("updatevalue", field_name));
                }
                else if (!string.IsNullOrEmpty(user_id))
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", 1));
                    command.Parameters.Add(new NpgsqlParameter("updatevalue", user_id));
                }
                else if (field_coordinates != null)
                {
                    command.Parameters.Add(new NpgsqlParameter("updateValue", field_coordinates));
                }
            }

            if (command.Parameters.Count > 1)
            {
                return true;
            }
            return false;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public bool Validate()
        {
            bool isValid = true;
            if (String.IsNullOrEmpty(user_id))
            {
                isValid = false;
            }
            if (field_id < 0)
            {
                isValid = false;
            }
            if (String.IsNullOrEmpty(field_name))
            {
                isValid = false;
            }
            if (field_coordinates == null)
            {
                isValid = false;
            }
            return isValid;
        }
    }
}
