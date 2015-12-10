using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class District : BaseDictionary
    {
        private int _idRealRegion;

        public District(DataRow row)
            : base(row)
        {
            int.TryParse(row[2].ToString(), out _idRealRegion);
        }

        public RealRegion RealRegion
        {
            get
            {
                RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
                return realRegionList.GetItem(_idRealRegion) as RealRegion;
            }
        }
    }
}
