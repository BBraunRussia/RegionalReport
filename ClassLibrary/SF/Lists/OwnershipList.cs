using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class OwnershipList : BaseList
    {
        private static OwnershipList _uniqueInstance;

        private OwnershipList(string tableName)
            : base(tableName)
        { }

        public static OwnershipList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new OwnershipList("SF_Ownership");

            return _uniqueInstance;
        }
    }
}
