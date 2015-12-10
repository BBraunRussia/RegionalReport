using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class TypeOrgList : BaseList
    {
        private static TypeOrgList _uniqueInstance;

        private TypeOrgList(string tableName)
            : base(tableName)
        { }

        public static TypeOrgList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new TypeOrgList("SF_TypeOrg");

            return _uniqueInstance;
        }
    }
}
