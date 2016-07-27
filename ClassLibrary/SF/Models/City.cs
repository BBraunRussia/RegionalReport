using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Lists;

namespace ClassLibrary.SF.Models
{
    public class City : BaseDictionary
    {
        public RealRegion RealRegion
        {
            get;
            private set;
        }

        public string PhoneCode { get; set; }

        public City(DataRow row)
            : base(row)
        {
            int idRealRegion;
            RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
            int.TryParse(row[2].ToString(), out idRealRegion);
            RealRegion = realRegionList.GetItem(idRealRegion) as RealRegion;

            PhoneCode = row[3].ToString();
        }

        public City(RealRegion realRegion)
        {
            RealRegion = realRegion;
        }

        public new object[] GetRow()
        {
            return new object[] { ID, Name, PhoneCode };
        }

        public void Save()
        {
            int idCity;
            int.TryParse(_provider.Insert("SF_City", ID, Name, RealRegion.ID, PhoneCode), out idCity);
            ID = idCity;

            CityList cityList = CityList.GetUniqueInstance();
            cityList.Add(this);
        }
    }
}
