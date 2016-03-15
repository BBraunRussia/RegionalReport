using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public class Role : BaseDictionary
    {
        public Role(DataRow row)
            : base(row)
        { }
    }
}
