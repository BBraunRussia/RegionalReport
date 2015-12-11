using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class RegionRR : BaseDictionary
    {
        private string _salesDistrict;

        public RegionRR(DataRow row)
            : base(row)
        {
            _salesDistrict = row[2].ToString();
        }

        public string SalesDistrict { get { return _salesDistrict; } }
    }
}
