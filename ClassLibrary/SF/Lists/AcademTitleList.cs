using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Lists
{
    public class AcademTitleList : BaseList
    {
        private static AcademTitleList _uniqueInstance;

        private AcademTitleList(string tableName)
            : base(tableName)
        { }

        public static AcademTitleList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new AcademTitleList("SF_AcademTitle");

            return _uniqueInstance;
        }
    }
}
