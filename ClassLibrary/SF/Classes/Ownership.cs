using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Ownership : BaseDictionary
    {
        public Ownership(DataRow row)
            : base(row)
        { }
    }
}
