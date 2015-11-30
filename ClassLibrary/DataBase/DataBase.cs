using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer;

namespace ClassLibrary
{
    public static class DataBase
    {
        private static IDataBase _database;

        public static void InitDataBase()
        {
            _database = new SQL();
        }

        public static void InitMockDataBase()
        {
            throw new NotImplementedException();
        }

        public static IDataBase GetDataBase()
        {
            if (_database == null)
                throw new NullReferenceException("База данных не инициализирована");
            else
                return _database;
        }
    }

    public static class Provider
    {
        private static IProvider _provider;

        public static void InitWebServiceProvider()
        {
            throw new NotImplementedException();
        }

        public static void InitSQLProvider()
        {
            _provider = new ProviderSQL();
        }

        public static void InitMockProvider()
        {
            throw new NotImplementedException();
        }

        public static IProvider GetProvider()
        {
            if (_provider == null)
                throw new NullReferenceException("Провайдер не инициализирован");
            else
                return _provider;
        }
    }
}
