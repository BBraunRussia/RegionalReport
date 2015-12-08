using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LPUChild : BaseDictionary
    {
        private int _idLPUParent;

        public LPUChild(DataRow row)
            : base (row)
        {
            int.TryParse(row[1].ToString(), out _idLPUParent);
        }
    }
}
