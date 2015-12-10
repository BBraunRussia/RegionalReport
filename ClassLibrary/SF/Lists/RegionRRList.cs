using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class RegionRRList : BaseList
    {
        private static RegionRRList _uniqueInstance;

        private RegionRRList(string tableName)
            : base(tableName)
        { }

        public static RegionRRList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new RegionRRList("SF_RegionRR");

            return _uniqueInstance;
        }
    }
}
