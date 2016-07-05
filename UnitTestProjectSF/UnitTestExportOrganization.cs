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
        
        //[TestCase(TypeOrg.ЛПУ, false, Result = "RU_Institution")]
        //[TestCase(TypeOrg.ЛПУ, true, Result = "RU_Institution")]
        [TestCase(TypeOrg.Отделение, true, Result = "RU_Department")]
        //[TestCase(TypeOrg.Аптека, true, Result = "RU_Department")]
        [TestCase(TypeOrg.Отдел, true, Result = "RU_Other")]
        [TestCase(TypeOrg.Аптека, false, Result = "RU_Pharmacy")]
        [TestCase(TypeOrg.Административное_Учреждение, false, Result = "RU_Other")]
        [TestCase(TypeOrg.Ветеренарная_клиника, false, Result = "RU_Other")]
        [TestCase(TypeOrg.Стоматология, false, Result = "RU_Other")]
        public string GetOrgType(TypeOrg typeOrg, bool haveParent)
        {
            Organization organization = GetOrganization(typeOrg, haveParent);

            return _exportOrganization.GetRecordType(organization);
        }
        
        private Organization GetOrganization(TypeOrg typeOrg, bool haveParent)
        {
            Organization organization = new Organization(typeOrg);

            if (haveParent)
            {
                organization.ParentOrganization = OrganizationList.GetUniqueInstance().GetFirst();
            }

            return organization;
        }
    }
}
