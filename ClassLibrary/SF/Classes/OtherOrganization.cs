using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class OtherOrganization : Organization, IHaveRegion
    {
        public OtherOrganization(DataRow row)
            : base(row)
        {
            KPP = row[13].ToString();
            PostIndex = row[14].ToString();

            int idCity;
            int.TryParse(row[15].ToString(), out idCity);
            City = CityList.GetUniqueInstance().GetItem(idCity) as City;

            District = row[16].ToString();
            Street = row[17].ToString();
            INN = row[18].ToString();
            Pharmacy = row[31].ToString();
        }

        public OtherOrganization(TypeOrg typeOrg)
            : base(typeOrg)
        { }

        public string KPP { get; set; }
        public string PostIndex { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public City City { get; set; }
        public string INN { get; set; }
        public string Pharmacy { get; set; }

        public RealRegion RealRegion
        {
            get { return (City == null) ? null : City.RealRegion; }
            set { }
        }

        public override void Save()
        {
            int id;
            
            int.TryParse(_provider.Insert("SF_OtherOrganization", ID, NumberSF, TypeOrg, Name, ShortName, Email, Website, Phone,
                INN, KPP, PostIndex, (City == null) ? 0 : ID, District, Street, Pharmacy, Deleted.ToString()), out id);

            ID = id;

            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Add(this);
        }

        public object[] GetRow()
        {
            string typeOrgName = TypeOrg.ToString();

            return new object[] { ID, NumberSF, ShortName, typeOrgName, INN, (RealRegion == null) ? string.Empty : RealRegion.Name,
                (City == null) ? string.Empty : City.Name, string.Empty, string.Empty, string.Empty };
        }
    }
}
