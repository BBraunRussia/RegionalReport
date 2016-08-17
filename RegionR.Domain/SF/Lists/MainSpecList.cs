using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Lists
{
    public class MainSpecList : BaseList
    {
        private static MainSpecList _uniqueInstance;

        private MainSpecList(string tableName)
            : base(tableName)
        { }

        public static MainSpecList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new MainSpecList("SF_MainSpec");

            return _uniqueInstance;
        }
    }
}
