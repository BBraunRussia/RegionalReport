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
            //try
            //{
                ReadFileOrganization readFileOrganization = new ReadFileOrganization();
                readFileOrganization.Start();
                Logger.Write("Organization loading done");
            //}
            //catch (ArgumentException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    Logger.Write("Organization loading with error");
            //}
        }

        private static void ReadFilePerson()
        {
            //try
            //{
                ReadFilePerson readFilePerson = new ReadFilePerson();
                readFilePerson.Start();
                Logger.Write("Person loading done");
            //}
            //catch (ArgumentException ex)
           // {
             //   Console.WriteLine(ex.Message);
            //    Logger.Write("Person loading with error");
            //}
        }

        private static void ReadFileRelationship()
        {
            try
            {
                ReadFileRelationship readFileRelationship = new ReadFileRelationship();
                readFileRelationship.Start();
                Logger.Write("Relationship loading done");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Write("Relationship loading with error");
            }
        }
    }
}
