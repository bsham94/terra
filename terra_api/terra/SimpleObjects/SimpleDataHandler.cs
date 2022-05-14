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
