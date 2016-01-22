using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class Position : BaseDictionary
    {
        private bool _unique;

        public Position(DataRow row)
            : base(row)
        {
            int idUnique;
            int.TryParse(row[2].ToString(), out idUnique);

            _unique = (idUnique == 1);
        }

        public bool Unique { get { return _unique; } }
    }
}
