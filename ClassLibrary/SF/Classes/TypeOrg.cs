using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class TypeOrg : BaseDictionary
    {
        public TypeOrg(DataRow row)
            : base(row)
        { }
    }
}
