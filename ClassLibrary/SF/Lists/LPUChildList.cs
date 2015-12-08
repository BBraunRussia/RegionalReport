using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class LPUChildList : BaseList
    {
        private static LPUChildList _uniqueInstance;

        private LPUChildList(string tableName)
            : base(tableName)
        { }

        public static LPUChildList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new LPUChildList("SF_LPUChild");

            return _uniqueInstance;
        }
    }
}
