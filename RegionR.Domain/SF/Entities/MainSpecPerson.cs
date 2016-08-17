using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF.Entities
{
    public class MainSpecPerson : BaseDictionary
    {
        public MainSpecPerson(DataRow row)
            : base(row)
        { }
    }
}
