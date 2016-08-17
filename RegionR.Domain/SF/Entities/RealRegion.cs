using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Entities
{
    public class RealRegion : BaseDictionary
    {
        private RegionRR _regionRR;

        public RealRegion(DataRow row)
            : base(row)
        {
            int idRegionRR;
            int.TryParse(row[2].ToString(), out idRegionRR);
            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;
        }

        public RegionRR RegionRR { get { return _regionRR; } }
    }
}
