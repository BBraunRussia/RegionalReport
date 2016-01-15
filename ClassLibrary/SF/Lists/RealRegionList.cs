using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
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
    }
}
