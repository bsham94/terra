using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class SharedField : IDatabase
    {
        public string field_owner_id;
        public string shared_owner_id;

        
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            field_owner_id = reader["field_owner_id"].ToString();
            shared_owner_id = reader["shared_owner_id"].ToString();
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
                        CommandText = "createsharedfield"
                    };

                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getsharedfield"
                    };
                    break;
                case CommandType.ReadWithParameters:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getsharedfieldbysharedfieldid"
                    };
                    break;
                case CommandType.Update:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "updatesharedfield"
                    };
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "deletesharedfield"
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
                command.Parameters.Add(new NpgsqlParameter("field_owner_id", field_owner_id));
                command.Parameters.Add(new NpgsqlParameter("shared_owner_id", shared_owner_id));
            }
        }
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        //public bool SetUpdateVariables()
        //{
        //    ClearParameters();
        //    //Check for command == null
        //    command.Parameters.Add(new NpgsqlParameter("userid", field_owner_id));
        //    if (account_type_id != 0 && field_count >= 0)
        //    {
        //        command.Parameters.Add(new NpgsqlParameter("account_type_id", account_type_id));
        //        command.Parameters.Add(new NpgsqlParameter("field_count", field_count));
        //        return true;
        //    }
        //    else
        //    {
        //        if (field_count >= 0)
        //        {
        //            command.Parameters.Add(new NpgsqlParameter("columntype", "field_count"));
        //            command.Parameters.Add(new NpgsqlParameter("updateValue", field_count));
        //        }
        //        else if (account_type_id != 0)
        //        {
        //            command.Parameters.Add(new NpgsqlParameter("columntype", "account_type_id"));
        //            command.Parameters.Add(new NpgsqlParameter("updatevalue", account_type_id));
        //        }
        //    }

        //    if (command.Parameters.Count > 1)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        //public void SetDeleteVariables(string searchType = null)
        //{
        //    ClearParameters();
        //    if (command != null)
        //    {
        //        switch (searchType)
        //        {
        //            case ("account_type_id"):
        //                command.Parameters.Add(new NpgsqlParameter("account_type_id", field_owner_id));
        //                break;
        //            default:
        //                command.Parameters.Add(new NpgsqlParameter("userid", field_owner_id));
        //                break;
        //        }
        //    }
        //}
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
                    case ("shared_owner_id"):
                        command.Parameters.Add(new NpgsqlParameter("shared_owner_id", shared_owner_id));
                        break;
                    default:
                        command.Parameters.Add(new NpgsqlParameter("field_owner_id", field_owner_id));
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
    }
}
