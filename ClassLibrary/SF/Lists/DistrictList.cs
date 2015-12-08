using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class DistrictList : BaseList
    {
        private static DistrictList _uniqueInstance;

        private DistrictList(string tableName)
            : base(tableName)
        { }

        public static DistrictList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new DistrictList("SF_District");

            return _uniqueInstance;
        }
    }
}
