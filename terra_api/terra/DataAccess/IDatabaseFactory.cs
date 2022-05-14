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
    public static class IDatabaseFactory
    {
        // Function   : 
        // Description: 
        // Paramaters : none
        // Returns    : void
        public static IDatabase GetObject(string type)
        {
            switch (type)
            {
                case ("Field"):
                    return new Field();
                case ("User"):
                    return new User();
                case ("Device"):
                    return new Device();
                //case ("MoistureData"):
                //    return new MoistureData();
                case ("EarthData"):
                    return new EarthData();
                default:
                    return null;                
            }
        }
    }
}
