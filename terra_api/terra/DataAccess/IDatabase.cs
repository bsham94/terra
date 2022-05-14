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
    public abstract class IDatabase
    {
        public NpgsqlCommand command;

        public abstract void Fill(NpgsqlDataReader reader);
        public abstract void Init(CommandType type);
        public abstract void SetInsertVariables();
        protected IDatabase() { }
        public enum CommandType
        {
            Create = 0,
            Read,
            ReadWithParameters,
            ReadAll,
            Update,
            Delete,
            GetEarthData,
            CheckOwnership,
            Count
        }
    }
}
