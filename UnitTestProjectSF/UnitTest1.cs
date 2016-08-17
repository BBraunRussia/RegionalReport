using System;
using NUnit.Framework;
using ClassLibrary;
using ClassLibrary.SF;
using ClassLibrary.SF.Import;
using ClassLibrary.SF.Export;
using RegionReport.Domain;

namespace UnitTestProjectSF
{
    [TestFixture]
    public class UnitTest1
    {
        public UnitTest1()
        {
            DataBase.InitDataBase();
            Provider.InitSQLProvider();
        }

        [TestCase(RecordType.RU_Hospital, "", "", Result = TypeOrg.ЛПУ)]
        [TestCase(RecordType.RU_Department, "", "Pharmaciuticals", Result = TypeOrg.Аптека)]
        [TestCase(RecordType.RU_Pharmacy, "", "", Result = TypeOrg.Аптека)]
        [TestCase(RecordType.RU_Department, "", "", Result = TypeOrg.Отделение)]
        [TestCase(RecordType.RU_Institution_Buying, "", "", Result = TypeOrg.Дистрибьютор)]
        [TestCase(RecordType.RU_Other, "Department (purchasing dep., etc)", "", Result = TypeOrg.Отдел)]
        [TestCase(RecordType.RU_Other, "Governmental-administrative establishment", "", Result = TypeOrg.Административное_Учреждение)]
        [TestCase(RecordType.RU_Other, "Dealer", "", Result = TypeOrg.Дистрибьютор)]
        [TestCase(RecordType.RU_Other, "Veterinary clinic", "", Result = TypeOrg.Ветеренарная_клиника)]
        [TestCase(RecordType.RU_Other, "Dentistry", "", Result = TypeOrg.Стоматология)]
        public TypeOrg ImportOrganization(RecordType recordType, string clientType, string mainSpec)
        {
            OrganizationModel organizationModel = new OrganizationModel { RecordType = recordType.ToString(), ClientType = clientType, MainSpec = mainSpec };

            return new ReadFileOrganization().GetTypeOrg(organizationModel);
        }
    }
}
