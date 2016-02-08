using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LpuRR : BaseDictionary
    {
        private RegionRR _regionRR;
        private string _fullName;

        public LpuRR(DataRow row)
            : base(row)
        {
            int idRegionRR;
            int.TryParse(row[2].ToString(), out idRegionRR);
            RegionRRList regionRRList = RegionRRList.GetUniqueInstance();
            _regionRR = regionRRList.GetItem(idRegionRR) as RegionRR;

            _fullName = row[3].ToString();
        }

        public RegionRR RegionRR { get { return _regionRR; } }
        public string FullName { get { return _fullName; } }

        public bool IsInList
        {
            get
            {
                LpuList lpuList = new LpuList();
                return lpuList.GetItem(this) != null;
            }
        }
    }
}
