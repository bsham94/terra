/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  Fields.cs                                                                             |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This file contains all functionality for the field object                       |
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
        public string new_field_name;
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
            this.field_name = field_name.ToLower();        
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
            this.field_name = field_name.ToLower();
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
                field_coordinates = new Polygon(new LinearRing(coordinates.ToArray()));
            }
            else
            {
                field_coordinates = null;
            }
            if (!string.IsNullOrEmpty(field_name))
            {
                this.field_name = field_name.ToLower();
            }
            else
            {
                this.field_name = "";
            }                    
        }

        public Field(string user_id, string field_name, string new_field_name)
        {
            this.user_id = user_id;
            this.field_name = field_name;
            this.new_field_name = new_field_name;
        }


        // Function   : Fill
        // Description: Fills the object with data from the sql reader.
        // Paramaters : NpgsqlDataReader: The sql reader.
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            user_id = reader["user_id"].ToString();
            field_id = (int) reader["field_id"];
            field_coordinates = (Polygon)reader["field_cords"];
            field_name = reader["field_name"].ToString();
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
                        CommandText = "getfieldbyuserid"
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
                case CommandType.GetArea:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getfieldarea"
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
            ClearParameters();
            if (command != null)
            {
                //command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
                command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
            }
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
                    case ("user_id"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        break;
                    case ("field_name"):
                        command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                        break;
                    case ("name_and_id"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("fieldid", field_id));
                        break;
                }             
            }
        }
        // Function   : CheckOwnershipVariables
        // Description: Sets the variables to check field ownership
        // Paramaters : string: pick what to checkownership by.
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
        // Function   : SetCountVariables
        // Description: Sets the variables to get field count
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
        // Function   : SetDeleteVariables
        // Description: Sets the delete variables
        // Paramaters : string: set what to delete by.
        // Returns    : void
        public void SetDeleteVariables(string searchType = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (searchType)
                {
                    case ("field_name"):
                        command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                    case ("user_id"):
                        command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                        break;
                }
            }
        }
        // Function   : SetUpdateVariables
        // Description: Sets the variables for updating a field.
        // Paramaters : none
        // Returns    : void
        public bool SetUpdateVariables()
        {
            ClearParameters();

            if (command != null)
            {
                if (!string.IsNullOrWhiteSpace(user_id) && !string.IsNullOrWhiteSpace(field_name) && !string.IsNullOrWhiteSpace(new_field_name))
                {
                    command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
                    command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
                    command.Parameters.Add(new NpgsqlParameter("new_field_name", new_field_name));

                }
                if (command.Parameters.Count == 3)
                {
                    return true;
                }
            }
            return false;
        }
        // Function   : SetUpdateVariables
        // Description: Sets the variables for updating a field.
        // Paramaters : none
        // Returns    : void
        //public bool SetUpdateVariables()
        //{
        //    ClearParameters();

        //    if (command != null)
        //    {
        //        command.Parameters.Add(new NpgsqlParameter("field_id", field_id));

        //        if (!string.IsNullOrEmpty(field_name) && !string.IsNullOrEmpty(user_id) && field_coordinates != null)
        //        {
        //            command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
        //            command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
        //            command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(field_name) && !string.IsNullOrEmpty(user_id))
        //            {
        //                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
        //                command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
        //            }
        //            else if (!string.IsNullOrEmpty(user_id) && field_coordinates != null)
        //            {
        //                command.Parameters.Add(new NpgsqlParameter("columntype", "user_id"));
        //                command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
        //                command.Parameters.Add(new NpgsqlParameter("updatevalue", user_id));
        //            }
        //            else if (!string.IsNullOrEmpty(field_name) && field_coordinates != null)
        //            {
        //                command.Parameters.Add(new NpgsqlParameter("columntype", "field_name"));
        //                command.Parameters.Add(new NpgsqlParameter("field_cords", field_coordinates));
        //                command.Parameters.Add(new NpgsqlParameter("updatevalue", field_name));
        //            }
        //            else if (!string.IsNullOrEmpty(field_name))
        //            {
        //                command.Parameters.Add(new NpgsqlParameter("columntype", 2));
        //                command.Parameters.Add(new NpgsqlParameter("updatevalue", field_name));
        //            }
        //            else if (!string.IsNullOrEmpty(user_id))
        //            {
        //                command.Parameters.Add(new NpgsqlParameter("columntype", 1));
        //                command.Parameters.Add(new NpgsqlParameter("updatevalue", user_id));
        //            }
        //            else if (field_coordinates != null)
        //            {
        //                command.Parameters.Add(new NpgsqlParameter("updateValue", field_coordinates));
        //            }
        //        }

        //        if (command.Parameters.Count > 1)
        //        {
        //            return true;
        //        } 
        //    }
        //    return false;
        //}

        // Function   : Validate
        // Description: Validates the field.
        // Paramaters : none
        // Returns    : bool: whether the field is valid or not.
        public bool Validate()
        {
            bool result = true;

            if (!ValidateName(field_name))
            {
                result = false;
            }
            if (!ValidateUserId(user_id))
            {
                result = false;
            }
            if (!ValidateCords(field_coordinates))
            {
                result = false;
            }

            return result;
        }
        // Function   : ValidateName
        // Description: Validates the field name.
        // Paramaters : none
        // Returns    : bool: whether the field name is valid or not.
        public bool ValidateName(string field_name)
        {
            bool result = true;
            if (string.IsNullOrEmpty(field_name) || string.IsNullOrWhiteSpace(field_name))
            {
                result = false;
            }
            return result;
        }
        // Function   : ValidateUserId
        // Description: Validates the user id.
        // Paramaters : none
        // Returns    : bool: whether the field name is valid or not.
        public bool ValidateUserId(string user_id)
        {
            bool result = true;

            if (string.IsNullOrEmpty(user_id) || string.IsNullOrWhiteSpace(user_id))
            {
                result = false;
            }

            return result;
        }
        // Function   : ValidateCords
        // Description: Validates the field coordinates
        // Paramaters : none
        // Returns    : bool: whether the field coordintaes are valid or not.
        public bool ValidateCords(Polygon polygon)
        {
            bool result = true;

            if (polygon == null)
            {
                result = false;
            }
            return result;
        }
    }
}
