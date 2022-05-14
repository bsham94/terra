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
    public class SimpleDevice
    {
        public string user_id;
        public int device_id;
        public int device_type_id;
        public int field_id;
        public int device_data;
        public float coordinates;
        public SimpleDevice(string user_id, int device_id, int device_type_id, int field_id, float coordinates, int device_data)
        {
            this.user_id = user_id;
            this.device_id = device_id;
            this.device_type_id = device_type_id;
            this.field_id = field_id;
            this.coordinates = coordinates;
            this.device_data = device_data;
        }
    }
}
