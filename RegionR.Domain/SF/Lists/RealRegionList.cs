using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Entities;

namespace ClassLibrary.SF.Lists
{
    public class RealRegionList : BaseList
    {
        private static RealRegionList _uniqueInstance;

        private RealRegionList(string tableName)
            : base(tableName)
        { }

        public static RealRegionList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new RealRegionList("SF_RealRegion");

            return _uniqueInstance;
        }
        
        public List<RealRegion> ToList(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();

            return List.Where(item => userRightList.IsInList(user, (item as RealRegion).RegionRR)).Select(item => item as RealRegion).ToList();
        }

        public DataTable ToDataTable(User user)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Название");

            var list = ToList(user).OrderBy(item => item.Name);

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }
    }
}
