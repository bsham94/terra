using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra
{
    public class SimpleFieldSpecific
    {
        public int specifics_id;
        public int field_id;
        public float yield;
        public string seedPlanted;
        public string fertilizer_use;
        public string pesticide_use;
        public DateTime entry_date;


        public SimpleFieldSpecific(FieldSpecific fieldSpecific)
        {
            specifics_id = fieldSpecific.specifics_id;
            field_id = fieldSpecific.field_id;
            yield = fieldSpecific.yield;
            seedPlanted = fieldSpecific.seedPlanted;
            fertilizer_use = fieldSpecific.fertilizer_use;
            pesticide_use = fieldSpecific.pesticide_use;
            entry_date = fieldSpecific.entry_date;
        }

    }
}
