using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class MainSpecPerson : BaseDictionary
    {
        public MainSpecPerson(DataRow row)
            : base(row)
        { }
    }
}
