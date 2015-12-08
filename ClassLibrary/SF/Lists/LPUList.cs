using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class LPUList : BaseList
    {
        private static LPUList _uniqueInstance;

        private LPUList(string tableName)
            : base(tableName)
        { }

        public static LPUList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new LPUList("SF_LPU");

            return _uniqueInstance;
        }
    }
}
