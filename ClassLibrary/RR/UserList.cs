using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class UserList : BaseList
    {
        private static UserList _uniqueInstance;

        private UserList(string tableName)
            : base(tableName)
        { }

        public static UserList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new UserList("SF_User");

            return _uniqueInstance;
        }
    }
}
