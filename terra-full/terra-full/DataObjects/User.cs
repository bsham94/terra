/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  User.cs                                                                               |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:This file contains all functionality for the user object                         |
*************************************************************************************************/
using System;
using Npgsql;

namespace terra
{
    public class User : IDatabase
    {
        public string id;
        public string user_name;
        public string shared_field_id;
        public int account_type_id;
        public int field_count;
        
        // Function   : User
        // Description: User constructor
        public User() {   }
        // Function   : User
        // Description: User constructor
        public User(string id)
        {
            this.id = id;
            account_type_id = 0;
            field_count = 0;

        }
        // Function   : User
        // Description: User constructor
        public User(string id, string user_name)
        {
            this.id = id;
            this.user_name = user_name;
        }
        // Function   : User
        // Description: User constructor
        public User(string id, int account_type_id, int field_count,string user_name)
        {
            this.id = id;
            this.account_type_id = account_type_id;
            this.field_count = field_count;
            this.user_name = user_name.ToLower();
        }
        // Function   : Fill
        // Description: Fills the object with data from the sql reader.
        // Paramaters : NpgsqlDataReader: The sql reader.
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            id = reader["user_id"].ToString();
            field_count = (int)reader["field_count"];
            account_type_id = (int)reader["account_type_id"];
            user_name = reader["user_name"].ToString();
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
                        CommandText = "CreateUser"
                    };

                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "GetUser"
                    };
                    break;
                case CommandType.ReadWithParameters:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getuserbyusername"
                    };
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "updateuser"
                    };
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "DeleteUser"
                    };
                    break;
                case CommandType.ShareFields:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "createsharedfield"
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
                command.Parameters.Add(new NpgsqlParameter("userid", id));
                command.Parameters.Add(new NpgsqlParameter("user_name", user_name));
                command.Parameters.Add(new NpgsqlParameter("account_type_id", account_type_id));
                command.Parameters.Add(new NpgsqlParameter("field_count", field_count));
            }
        }
        // Function   : SetInsertVariable
        // Description: Sets the variables for inserting to the database.
        // Paramaters : none
        // Returns    : void
        public void SetSharedFieldVariables()
        {
            ClearParameters();
            if (command != null)
            {
                command.Parameters.Add(new NpgsqlParameter("field_owner_id", id));
                command.Parameters.Add(new NpgsqlParameter("shared_owner_id", shared_field_id));
            }
        }
        // Function   : SetUpdateVariables
        // Description: Sets the variables for updating a user.
        // Paramaters : none
        // Returns    : void
        public bool SetUpdateVariables()
        {
            ClearParameters();
            //Check for command == null
            command.Parameters.Add(new NpgsqlParameter("userid", id));
            if (account_type_id != 0 && field_count >= 0)
            {
                command.Parameters.Add(new NpgsqlParameter("account_type_id", account_type_id));
                command.Parameters.Add(new NpgsqlParameter("field_count", field_count));
                return true;
            }
            else
            {
                if (field_count >= 0)
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", "field_count"));
                    command.Parameters.Add(new NpgsqlParameter("updatevalue", field_count));
                }
                else if (account_type_id != 0)
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", "account_type_id"));
                    command.Parameters.Add(new NpgsqlParameter("updatevalue", account_type_id));
                }
            }

            if (command.Parameters.Count > 1)
            {              
                return true;
            }
            return false;
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
                    case ("account_type_id"):
                        command.Parameters.Add(new NpgsqlParameter("account_type_id", id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("userid", id));
                        break;
                }
            }
        }
        // Function   : SetSelecttVariable
        // Description: Sets the variables for reading from the database.
        // Paramaters : string: change what to select by.
        // Returns    : void
        public void SetSelectVariables(string searchType = null)
        {
            ClearParameters();
            if (command != null)
            {
                switch (searchType)
                {

                    case ("user_name"):
                        command.Parameters.Add(new NpgsqlParameter("user_name", user_name));
                        break;                    
                    case ("account_type_id"):
                        command.Parameters.Add(new NpgsqlParameter("account_type_id", id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("userid", id));
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
        // Function   : Validate
        // Description: Validates the user.
        // Paramaters : none
        // Returns    : bool: whether the user is valid or not.
        public bool Validate()
        {
            bool isValid = true;
            if (String.IsNullOrEmpty(id))
            {
                isValid = false;
            }
            if (account_type_id < 1 || account_type_id > 3)
            {
                isValid = false;
            }
            if (field_count < 0 || field_count > 5)
            {
                isValid = false;
            }
            return isValid;
        }

    }
}
