using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LpuRR : BaseDictionary
    {
        private int _idRegionRR;

        public LpuRR(DataRow row)
            : base(row)
        {
            int.TryParse(row[2].ToString(), out _idRegionRR);
        }

        public RegionRR RegionRR
        {
            get
            {
                RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
                return regionRRList.GetItem(_idRegionRR) as RegionRR;
            }
        }

        public bool IsInList
        {
            get
            {
                LpuList lpuList = new LpuList();
                return lpuList.GetItem(ID) != null;
            }
        }
    }
}
