/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using NetTopologySuite.Geometries;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class SimpleField
    {
        public string user_id;
        public int field_id;
        public string field_name;
        public SimpleField(string user_id, int field_id, string field_name)
        {
            this.user_id = user_id;
            this.field_id = field_id;
            this.field_name = field_name;
        }
        public SimpleField(Field field)
        {
            user_id = "";//field.user_id;
            field_id = field.field_id;
            field_name = field.field_name;
        }
    }
}
