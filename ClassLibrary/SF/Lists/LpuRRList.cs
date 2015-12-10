using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class LpuRRList : BaseList
    {
        private static LpuRRList _uniqueInstance;

        private LpuRRList(string tableName)
            : base(tableName)
        { }

        public static LpuRRList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new LpuRRList("SF_LpuRR");

            return _uniqueInstance;
        }
    }
}
