using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class OtherOrganization : Organization
    {
        private string _kpp;
        private string _inn;
        private string _postIndex;
        private int _idCity;
        private string _district;
        private string _street;
        private string _pharmacy;

        public OtherOrganization(DataRow row)
            : base(row)
        {
            _kpp = row[13].ToString();
            _postIndex = row[14].ToString();
            int.TryParse(row[15].ToString(), out _idCity);
            _district = row[16].ToString();
            _street = row[17].ToString();
            
            _inn = row[18].ToString();

            _pharmacy = row[31].ToString();
        }

        public OtherOrganization(TypeOrg typeOrg)
            : base(typeOrg)
        { }

        public string KPP
        {
            get { return _kpp; }
            set { _kpp = value; }
        }

        public string PostIndex
        {
            get { return _postIndex; }
            set { _postIndex = value; }
        }

        public string District
        {
            get { return _district; }
            set { _district = value; }
        }

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        public City City
        {
            get
            {
                if (_idCity == 0)
                    return null;

                CityList cityList = CityList.GetUniqueInstance();
                return cityList.GetItem(_idCity) as City;
            }
            set { _idCity = value.ID; }
        }

        public RealRegion RealRegion { get { return (City == null) ? null : City.RealRegion; } }

        public string INN
        {
            get { return _inn; }
            set { _inn = value; }
        }

        public string Pharmacy
        {
            get { return _pharmacy; }
            set { _pharmacy = value; }
        }
        
        public override void Save()
        {
            int id;
            
            int.TryParse(_provider.Insert("SF_OtherOrganization", ID, NumberSF, TypeOrg, Name, ShortName, Email, WebSite, Phone,
                INN, KPP, PostIndex, City.ID, District, Street, Pharmacy), out id);

            SetID(id);

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }

        public object[] GetRow()
        {
            string typeOrgName = TypeOrg.ToString();

            return new object[] { ID, NumberSF, ShortName, typeOrgName, INN, RealRegion.Name, City.Name, string.Empty, string.Empty };
        }
    }
}
