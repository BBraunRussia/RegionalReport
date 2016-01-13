using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Position : BaseDictionary
    {
        public Position(DataRow row)
            : base(row)
        { }
    }
}
