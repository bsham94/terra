/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using Npgsql;


namespace terra
{
    public class User : IDatabase
    {
        public string id;
        public int account_type_id;
        public int field_count;

        public User() {   }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public User(string id)
        {
            this.id = id;
            account_type_id = 0;
            field_count = 0;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public User(string id, int account_type_id, int field_count)
        {
            this.id = id;
            this.account_type_id = account_type_id;
            this.field_count = field_count;
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            id = reader["user_id"].ToString();
            field_count = (int)reader["field_count"];
            account_type_id = (int)reader["account_type_id"];
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
                        CommandText = "GetUserByTypeId"
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
                command.Parameters.Add(new NpgsqlParameter("userid", id));
                command.Parameters.Add(new NpgsqlParameter("account_type_id", account_type_id));
                command.Parameters.Add(new NpgsqlParameter("field_count", field_count));
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
                    command.Parameters.Add(new NpgsqlParameter("updateValue", field_count));
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
                    case ("account_type_id"):
                        command.Parameters.Add(new NpgsqlParameter("account_type_id", id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("userid", id));
                        break;
                }
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
                    case ("account_type_id"):
                        command.Parameters.Add(new NpgsqlParameter("account_type_id", id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("userid", id));
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
