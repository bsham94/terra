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
    public class EarthDataSearch
    {
        [Newtonsoft.Json.JsonConstructor]
        EarthDataSearch(int Id, string Type)
        {
            this.Id = Id;
            this.Type = Type;
        }

		public int Id { get; set; }
        public string Type { get; set; }
    }
}
