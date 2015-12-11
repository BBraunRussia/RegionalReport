using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

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

        public DataTable ToDataTable(RealRegion realRegion)
        {
            var list = List.Where(item => (item as District).RealRegion == realRegion);

            return (list.Count() == 0) ? null : list.Select(item => item.GetRow()).CopyToDataTable();
        }
    }
}
