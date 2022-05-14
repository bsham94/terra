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
    public class SimpleEarthDataType
    {
        public string data_name;
        public string dataset_handler;

        public SimpleEarthDataType(EarthDataTypes earthDataTypes)
        {
            data_name = earthDataTypes.data_name;
            dataset_handler = earthDataTypes.dataset_handler;
        }

    }
}
