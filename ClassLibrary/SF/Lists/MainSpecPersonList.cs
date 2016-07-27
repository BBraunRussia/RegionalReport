using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Lists
{
    public class MainSpecPersonList : BaseList
    {
        private static MainSpecPersonList _uniqueInstance;

        private MainSpecPersonList(string tableName)
            : base(tableName)
        { }

        public static MainSpecPersonList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new MainSpecPersonList("SF_MainSpecPerson");

            return _uniqueInstance;
        }
    }
}
