using System;
using NUnit.Framework;
using ClassLibrary;
using ClassLibrary.SF;

namespace UnitTestProjectSF
{
    [TestFixture]
    public class UnitTestExportOrganization
    {
        public UnitTestExportOrganization()
        {
            DataBase.InitDataBase();
            Provider.InitSQLProvider();
        }

        [TestCase("ЛПУ", Result="ЛПУ")]
        public void TestGetOrgType()
        {
            ExportOrganization ex = new ExportOrganization();
            ex.GetRecordType();
        }
    }
}
