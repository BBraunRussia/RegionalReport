using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class City : BaseDictionary
    {
        private int _idDistrict;

        public City(DataRow row)
            : base(row)
        {
            int.TryParse(row[2].ToString(), out _idDistrict);
        }

        public District District
        {
            get
            {
                DistrictList districtList = DistrictList.GetUniqueInstance();
                return districtList.GetItem(_idDistrict) as District;
            }
        }
    }
}
