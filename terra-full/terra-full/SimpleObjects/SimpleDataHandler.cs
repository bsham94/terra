/************************************************************************************************|
|   Project: Terra                                                                               |
|   File:  SimpleDataHandler.cs                                                                  |
|   Date: March 8, 2019                                                                          |
|   Author: Ben Shamas, Sam Guta, Kevin Park, Zaid Omar                                          |
|   Description: This class is used for easy serialization                                       |
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class SimpleDataHandler
    {
        public int FieldId;
        public string DataSetName;
        public DateTime Date;
        public SimpleDataHandler(DataHandler da)
        {
            DataSetName = da.DataSetName;
            Date = da.Date;
        }
    }
}
