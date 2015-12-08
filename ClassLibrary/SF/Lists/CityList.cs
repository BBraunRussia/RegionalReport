using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class CityList : BaseList
    {
        private static CityList _uniqueInstance;

        private CityList(string tableName)
            : base(tableName)
        { }

        public static CityList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new CityList("SF_City");

            return _uniqueInstance;
        }
    }
}
