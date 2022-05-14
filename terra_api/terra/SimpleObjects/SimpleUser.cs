/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class SimpleUser
    {
        public string id;
        public int account_type_id;
        public int field_count;
        public SimpleUser(string id, int account_type_id, int field_count)
        {
            this.id = id;
            this.account_type_id = account_type_id;
            this.field_count = field_count;
        }
        public SimpleUser(User user)
        {
            id = user.id;
            account_type_id = user.account_type_id;
            field_count = user.field_count;
        }
        
    }
}
