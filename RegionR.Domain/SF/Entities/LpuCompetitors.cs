using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;

namespace ClassLibrary.SF.Entities
{
    public class LpuCompetitors : BaseDictionary
    {
        private string _inn;
        private RealRegion _realRegion;
        private string _kpp;

        public LpuCompetitors(DataRow row)
            : base(row)
        {
            _inn = row[2].ToString();

            int idRealRegion;
            int.TryParse(row[3].ToString(), out idRealRegion);

            RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
            _realRegion = realRegionList.GetItem(idRealRegion) as RealRegion;

            _kpp = row[4].ToString();
        }

        public string INN { get { return _inn; } }
        public string KPP { get { return _kpp; } }
        public RealRegion RealRegion { get { return _realRegion; } }

        public override object[] GetRow()
        {
            return new object[] { ID, Name, RealRegion.Name, _inn };
        }
    }
}
