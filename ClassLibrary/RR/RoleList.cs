using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class RoleList : BaseList
    {
        private static RoleList _uniqueInstance;

        private RoleList(string tableName)
            : base(tableName)
        { }

        public static RoleList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new RoleList("SF_Role");

            return _uniqueInstance;
        }
    }
}
