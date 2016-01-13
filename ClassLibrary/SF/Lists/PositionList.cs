using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class PositionList : BaseList
    {
        private static PositionList _uniqueInstance;

        private PositionList(string tableName)
            : base(tableName)
        { }

        public static PositionList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new PositionList("SF_Position");

            return _uniqueInstance;
        }
    }
}
