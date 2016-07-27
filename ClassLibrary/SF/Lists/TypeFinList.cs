using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Lists
{
    public class TypeFinList : BaseList
    {
        private static TypeFinList _uniqueInstance;

        private TypeFinList(string tableName)
            : base(tableName)
        { }

        public static TypeFinList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new TypeFinList("SF_TypeFin");

            return _uniqueInstance;
        }
    }
}
