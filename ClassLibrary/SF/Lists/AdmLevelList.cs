using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Lists
{
    public class AdmLevelList : BaseList
    {
        private static AdmLevelList _uniqueInstance;

        private AdmLevelList(string tableName)
            : base(tableName)
        { }

        public static AdmLevelList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new AdmLevelList("SF_AdmLevel");

            return _uniqueInstance;
        }
    }
}
