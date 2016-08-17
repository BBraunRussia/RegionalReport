using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;

namespace ClassLibrary.SF.Entities
{
    public class SubRegion : BaseDictionary
    {
        private RealRegion _realRegion;

        public SubRegion(DataRow row)
            : base(row)
        {
            int idRealRegion;
            int.TryParse(row[2].ToString(), out idRealRegion);
            _realRegion = RealRegionList.GetUniqueInstance().GetItem(idRealRegion) as RealRegion;
        }

        public RealRegion RealRegion
        {
            get { return _realRegion; }
        }
    }
}
