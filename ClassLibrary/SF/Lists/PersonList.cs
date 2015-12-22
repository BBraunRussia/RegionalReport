using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class PersonList : BaseList
    {
        private static PersonList _uniqueInstance;

        private PersonList(string tableName)
            : base(tableName)
        { }

        public static PersonList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new PersonList("SF_Person");

            return _uniqueInstance;
        }

        public override DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Фамилия");
            dt.Columns.Add("Имя");
            dt.Columns.Add("Отчество");
            dt.Columns.Add("Организация");
            dt.Columns.Add("Должность");
            dt.Columns.Add("Город");
            dt.Columns.Add("Регион RR");
            dt.Columns.Add("SF номер");
            
            var list = List.OrderBy(item => item.Name);

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }
    }
}
