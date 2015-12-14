using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class UserRightList : BaseList
    {
        private static UserRightList _uniqueInstance;

        private UserRightList(string tableName)
            : base(tableName)
        { }

        public static UserRightList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new UserRightList("SF_UserRight");

            return _uniqueInstance;
        }

        public List<RegionRR> ToList(User user)
        {
            return List.Where(item => (item as UserRight).User == user).Select(item => (item as UserRight).RegionRR).ToList();
        }
    }
}
