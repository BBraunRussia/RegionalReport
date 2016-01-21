using System;
using NUnit.Framework;
using ClassLibrary;

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

        [TestCase]
        public void TestMethod1()
        {
        }
    }
}
