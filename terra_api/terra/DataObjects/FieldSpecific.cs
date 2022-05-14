using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class FieldSpecific : IDatabase
    { 
        public int specifics_id;
        public int field_id;
        public float yield;
        public string seedPlanted;
        public string fertilizer_use;
        public string pesticide_use;
	    public DateTime entry_date;


        public FieldSpecific()
        {
            field_id = 0;
            specifics_id = 0;
            yield = 0;
            seedPlanted = "";
            fertilizer_use = "";
            pesticide_use = "";
            entry_date = new DateTime();
        }

        public FieldSpecific(int field_id)
        {
            this.field_id = field_id;
            specifics_id = 0;
            yield = 0;
            seedPlanted = "";
            fertilizer_use = "";
            pesticide_use = "";
            entry_date = new DateTime();
        }


        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            field_id = int.Parse(reader["field_id"].ToString());
            specifics_id = int.Parse(reader["specifics_id"].ToString());
            yield = float.Parse(reader["yield"].ToString());
            seedPlanted = reader["seedPlanted"].ToString();
            fertilizer_use = reader["fertilizer_use"].ToString();
            pesticide_use = reader["pesticide_use"].ToString();
            entry_date = (DateTime)reader["entry_date"];

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
                        CommandText = "createfieldspecific"

                    };
                    break;
                case CommandType.Read:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "getfieldspecific"
                    };
                    break;
                //case CommandType.ReadWithParameters:
                //    command = new NpgsqlCommand()
                //    {
                //        CommandType = System.Data.CommandType.StoredProcedure,
                //        CommandText = "GetFieldByUserId"
                //    };
                //    break;
                //case CommandType.ReadAll:
                //    command = new NpgsqlCommand()
                //    {
                //        CommandType = System.Data.CommandType.StoredProcedure,
                //        CommandText = "GetAllFields"
                //    };
                //    break;
                case CommandType.Update:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "updatefieldspecific"
                    };
                    break;
                case CommandType.Delete:
                    command = new NpgsqlCommand()
                    {
                        CommandType = System.Data.CommandType.StoredProcedure,
                        CommandText = "deletefieldspecific"
                    };
                    break;
                //case CommandType.CheckOwnership:
                //    command = new NpgsqlCommand()
                //    {
                //        CommandType = System.Data.CommandType.StoredProcedure,
                //        CommandText = "checkownership"
                //    };
                //    break;
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
                command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                //command.Parameters.Add(new NpgsqlParameter("specifics_id", specifics_id));
                command.Parameters.Add(new NpgsqlParameter("yield", yield));
                command.Parameters.Add(new NpgsqlParameter("seedplanted", seedPlanted));
                command.Parameters.Add(new NpgsqlParameter("fertilizer_use", fertilizer_use));
                command.Parameters.Add(new NpgsqlParameter("pesticide_use", pesticide_use));
                //command.Parameters.Add(new NpgsqlParameter("entry_date", entry_date));
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
        //public void CheckOwnershipVariables(string type = null)
        //{
        //    ClearParameters();
        //    if (command != null)
        //    {
        //        switch (type)
        //        {
        //            case ("all"):
        //                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
        //                command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
        //                break;
        //            default:
        //                command.Parameters.Add(new NpgsqlParameter("user_id", user_id));
        //                command.Parameters.Add(new NpgsqlParameter("field_name", field_name));
        //                break;
        //        }

        //    }
        //}
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
            if (field_id != 0 && !string.IsNullOrEmpty(seedPlanted))
            {
                command.Parameters.Add(new NpgsqlParameter("field_id", field_id));
                command.Parameters.Add(new NpgsqlParameter("seedplanted", seedPlanted));
                if (yield != 0 && !string.IsNullOrEmpty(fertilizer_use) && !string.IsNullOrEmpty(pesticide_use))
                {
                    command.Parameters.Add(new NpgsqlParameter("yield", yield));
                    command.Parameters.Add(new NpgsqlParameter("fertilizer_use", fertilizer_use));
                    command.Parameters.Add(new NpgsqlParameter("pesticide_use", pesticide_use));
                    //command.Parameters.Add(new NpgsqlParameter("entry_date", entry_date));
                }
                else if (yield != 0)
                {
                    //command.Parameters.Add(new NpgsqlParameter("columntype", " yield"));
                    command.Parameters.Add(new NpgsqlParameter("columnvalue", yield));
                }
                else if (!string.IsNullOrEmpty(fertilizer_use))
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", "fertilizer_use"));
                    command.Parameters.Add(new NpgsqlParameter("columnvalue", fertilizer_use));
                }
                else if (!string.IsNullOrEmpty(pesticide_use))
                {
                    command.Parameters.Add(new NpgsqlParameter("columntype", "pesticide_use"));
                    command.Parameters.Add(new NpgsqlParameter("columnvalue", pesticide_use));
                }
                //else if (!entry_date.Equals(new DateTime()))
                //{
                //    command.Parameters.Add(new NpgsqlParameter("columntype", "entry_date"));
                //    command.Parameters.Add(new NpgsqlParameter("entry_date", entry_date));
                //}
            }          
            if (command.Parameters.Count > 2)
            {
                return true;
            }
            return false;
        }
    }
}
