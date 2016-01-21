using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class SubRegion : BaseDictionary
    {
        private RealRegion _realRegion;

        public SubRegion(DataRow row)
            : base(row)
        {
            int idRealRegion;
            int.TryParse(row[2].ToString(), out idRealRegion);
            RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
            _realRegion = realRegionList.GetItem(idRealRegion) as RealRegion;
        }

        public RealRegion RealRegion
        {
            get { return _realRegion; }
        }
    }
}
