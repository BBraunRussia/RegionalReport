using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class SubRegionList : BaseList
    {
        private static SubRegionList _uniqueInstance;

        private SubRegionList(string tableName)
            : base(tableName)
        { }

        public static SubRegionList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new SubRegionList("SF_SubRegion");

            return _uniqueInstance;
        }

        public SubRegion GetItem(RegionRR regionRR)
        {
            return List.Where(item => (item as SubRegion).RegionRR.ID == regionRR.ID).Select(item => item as SubRegion).First();
        }
    }
}
