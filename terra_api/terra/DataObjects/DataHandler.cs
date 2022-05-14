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
        string DataType;
        public DateTime Date;

        public DataHandler() { }
        [JsonConstructor]
        public DataHandler(string DataType)
        {
            this.DataType = DataType;
        }


        public override void Fill(NpgsqlDataReader reader)
        {
            DataSetName = reader[0].ToString();
            Date = DateTime.Parse(reader[1].ToString());
        }

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

        public override void SetInsertVariables()
        {
            throw new NotImplementedException();
        }

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
