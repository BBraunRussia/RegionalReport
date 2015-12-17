using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public abstract class MainOrganization : Organization
    {
        private int _idTypeLPU;
        private int _idOwnership;
        private int _idAdmLevel;
        
        private string _kpp;
        private string _postIndex;
        private int _idCity;
        private string _street;

        public MainOrganization(DataRow row)
            : base(row)
        {
            int.TryParse(row[9].ToString(), out _idTypeLPU);
            int.TryParse(row[10].ToString(), out _idOwnership);
            int.TryParse(row[11].ToString(), out _idAdmLevel);
            _kpp = row[12].ToString();
            _postIndex = row[13].ToString();
            int.TryParse(row[14].ToString(), out _idCity);
            _street = row[15].ToString();
        }

        public MainOrganization(TypeOrg typeOrg)
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

        public RegionCompetitors RegionCompetitors
        {
            set
            {
                RealRegionList realRegionList = RealRegionList.GetUniqueInstance();
                RealRegion realRegion = realRegionList.GetItem(value);

                CityList cityList = CityList.GetUniqueInstance();

                City city = cityList.GetItem(realRegion);

                City = city;
            }
        }
        
        public TypeLPU TypeLPU
        {
            get
            {
                if (_idTypeLPU == 0)
                    return null;

                TypeLPUList typeLPUList = TypeLPUList.GetUniqueInstance();
                return typeLPUList.GetItem(_idTypeLPU) as TypeLPU;
            }
            set { _idTypeLPU = value.ID; }
        }

        public AdmLevel AdmLevel
        {
            get
            {
                if (_idAdmLevel == 0)
                    return null;

                AdmLevelList admLevelList = AdmLevelList.GetUniqueInstance();
                return admLevelList.GetItem(_idAdmLevel) as AdmLevel;
            }
            set { _idAdmLevel = value.ID; }
        }

        public Ownership Ownership
        {
            get
            {
                if (_idOwnership == 0)
                    return null;

                OwnershipList ownershipList = OwnershipList.GetUniqueInstance();
                return ownershipList.GetItem(_idOwnership) as Ownership;
            }
            set { _idOwnership = value.ID; }
        }
    }
}
