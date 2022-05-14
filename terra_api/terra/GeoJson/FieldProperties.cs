/************************************************************************************************|
|   Project:                                                                             |
|   File:  .cs                                                                    |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description:                   |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace terra
{
    class FieldProperties
    {
        public enum OwnerShipType{
            NotOwned = 0,
            OwnedByOther,
            OwnedByUser
        }
        public string id { get; set; }
		public string field_name { get; set; }
		//public float temperature { get; set; }

        public OwnerShipType field_ownership { get; set; }
	}
}
