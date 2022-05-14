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

       



        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public override void Fill(NpgsqlDataReader reader)
        {
            data_name = reader["data_name"].ToString();
            dataset_handler = reader["dataset_handler"].ToString();
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
        // Function   : 
        // Description: 
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
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        //public void SetSelectVariables(string searchType = null)
        //{
        //    if (command != null)
        //    {
                

        //    }
        //}
    }
}
