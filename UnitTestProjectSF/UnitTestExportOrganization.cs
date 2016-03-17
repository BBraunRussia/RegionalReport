using System;
using NUnit.Framework;
using ClassLibrary;
using ClassLibrary.SF;

namespace UnitTestProjectSF
{
    [TestFixture]
    public class UnitTestExportOrganization
    {
        private ExportOrganization _exportOrganization;

        public UnitTestExportOrganization()
        {
            DataBase.InitDataBase();
            Provider.InitSQLProvider();

            _exportOrganization = new ExportOrganization();
        }
        
        [TestCase(TypeOrg.ЛПУ, false, Result = "RU_Institution")]
        [TestCase(TypeOrg.ЛПУ, true, Result = "RU_Institution")]
        [TestCase(TypeOrg.Отделение, true, Result = "RU_Department")]
        [TestCase(TypeOrg.Аптека, true, Result = "RU_Department")]
        [TestCase(TypeOrg.Отдел, true, Result = "RU_Other")]
        [TestCase(TypeOrg.Аптека, false, Result = "RU_Pharmacy")]
        [TestCase(TypeOrg.Административное_Учреждение, false, Result = "RU_Other")]
        public string TestGetOrgType(TypeOrg typeOrg, bool haveParent)
        {
            Organization organization = GetOrganization(typeOrg, haveParent);

            return _exportOrganization.GetRecordType(organization);
        }

        [TestCase(TypeOrg.ЛПУ, false, Result = "ЛПУ")]
        [TestCase(TypeOrg.ЛПУ, true, Result = "Филиал ЛПУ")]
        [TestCase(TypeOrg.Отделение, true, Result = "Отделение ЛПУ")]
        [TestCase(TypeOrg.Аптека, true, Result = "Аптека ЛПУ")]
        [TestCase(TypeOrg.Отдел, true, Result = "Отдел ЛПУ")]
        [TestCase(TypeOrg.Аптека, false, Result = "Аптека коммерческая")]
        [TestCase(TypeOrg.Административное_Учреждение, false, Result = "Административное учреждение")]
        public string TestGetFormatTypeOrgRus(TypeOrg typeOrg, bool haveParent)
        {
            Organization organization = GetOrganization(typeOrg, haveParent);

            return _exportOrganization.GetFormatTypeOrgRus(organization);
        }

        [TestCase(TypeOrg.ЛПУ, false, Result = "Hospital")]
        [TestCase(TypeOrg.ЛПУ, true, Result = "Hospital branch")]
        [TestCase(TypeOrg.Отделение, true, Result = "Hospital department (medical)")]
        [TestCase(TypeOrg.Аптека, true, Result = "Hospital pharmacy")]
        [TestCase(TypeOrg.Отдел, true, Result = "Hospital department (non-medical)")]
        [TestCase(TypeOrg.Аптека, false, Result = "Pharmacy")]
        [TestCase(TypeOrg.Административное_Учреждение, false, Result = "Governmental-administrative establishment")]
        public string TestGetFormatTypeOrgEng(TypeOrg typeOrg, bool haveParent)
        {
            Organization organization = GetOrganization(typeOrg, haveParent);

            return _exportOrganization.GetFormatTypeOrgEng(organization);
        }

        private Organization GetOrganization(TypeOrg typeOrg, bool haveParent)
        {
            Organization organization = new Organization(typeOrg);

            if (haveParent)
            {
                OrganizationList organizationList = new OrganizationList();
                organization.ParentOrganization = organizationList.GetFirst();
            }

            return organization;
        }
    }
}
