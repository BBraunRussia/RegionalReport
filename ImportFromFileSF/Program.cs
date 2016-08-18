using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary.SF.Import;
using ClassLibrary;
using ClassLibrary.Common;

namespace ImportFromFileSF
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();

            DateTime datetime = DateTime.Now;
            LogManager.Logger.Debug("Start loading organization");
            ReadFileOrganization();
            LogManager.Logger.Debug("Finish loading organization ", (DateTime.Now - datetime));

            datetime = DateTime.Now;
            LogManager.Logger.Debug("Start loading person");
            ReadFilePerson();
            LogManager.Logger.Debug("Finish loading person ", (DateTime.Now - datetime));

            datetime = DateTime.Now;
            LogManager.Logger.Debug("Start loading relationship");
            ReadFileRelationship();
            LogManager.Logger.Debug("Finish loading relationship ", (DateTime.Now - datetime));
        }

        private static void Init()
        {
            DataBase.InitDataBase();
            Provider.InitSQLProvider();
        }

        private static void ReadFileOrganization()
        {
            ReadFileOrganization readFileOrganization = new ReadFileOrganization();
            readFileOrganization.Start();
        }

        private static void ReadFilePerson()
        {
            ReadFilePerson readFilePerson = new ReadFilePerson();
            readFilePerson.Start();
        }

        private static void ReadFileRelationship()
        {
            ReadFileRelationship readFileRelationship = new ReadFileRelationship();
            readFileRelationship.Start();
        }
    }
}
