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
            Console.WriteLine("Start loading organization");
            ReadFileOrganization();
            Console.WriteLine("Finish loading organization " + (DateTime.Now - datetime));

            datetime = DateTime.Now;
            Console.WriteLine("Start loading person");
            ReadFilePerson();
            Console.WriteLine("Finish loading person " + (DateTime.Now - datetime));

            datetime = DateTime.Now;
            Console.WriteLine("Start loading relationship");
            ReadFileRelationship();
            Console.WriteLine("Finish loading relationship " + (DateTime.Now - datetime));
        }

        private static void Init()
        {
            DataBase.InitDataBase();
            Provider.InitSQLProvider();
        }

        private static void ReadFileOrganization()
        {
            Logger.Write("Organization start loading");
            ReadFileOrganization readFileOrganization = new ReadFileOrganization();
            readFileOrganization.Start();
            Logger.Write("Organization loading is done");
        }

        private static void ReadFilePerson()
        {
            Logger.Write("Person start loading");
            ReadFilePerson readFilePerson = new ReadFilePerson();
            readFilePerson.Start();
            Logger.Write("Person loading is done");
        }

        private static void ReadFileRelationship()
        {
            Logger.Write("Relationship start loading");
            ReadFileRelationship readFileRelationship = new ReadFileRelationship();
            readFileRelationship.Start();
            Logger.Write("Relationship loading is done");
        }
    }
}
