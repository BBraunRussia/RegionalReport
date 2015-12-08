using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public class TypeLPUList : BaseList
    {
        private static TypeLPUList _uniqueInstance;

        private TypeLPUList(string tableName)
            : base(tableName)
        { }

        public static TypeLPUList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new TypeLPUList("SF_TypeLPU");

            return _uniqueInstance;
        }
    }
}
