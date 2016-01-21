using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class AcademTitle : BaseDictionary
    {
        public AcademTitle(DataRow row)
            : base(row)
        { }
    }
}
