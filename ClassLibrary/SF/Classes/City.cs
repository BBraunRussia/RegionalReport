using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class City : BaseDictionary
    {
        private int _idRealRegion;
        private string _phoneCode;

        public City(DataRow row)
            : base(row)
        {
            int.TryParse(row[2].ToString(), out _idRealRegion);
            _phoneCode = row[3].ToString();
        }

        public City(int idRealRegion)
        {
            _idRealRegion = idRealRegion;
        }

        public RealRegion RealRegion
        {
            get
            {
                RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
                return realRegionList.GetItem(_idRealRegion) as RealRegion;
            }
        }

        public string PhoneCode
        {
            get { return _phoneCode; }
            set { _phoneCode = value; }
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
