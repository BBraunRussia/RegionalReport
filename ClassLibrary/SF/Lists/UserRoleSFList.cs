using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF.Models;

namespace ClassLibrary.SF.Lists
{
    public class UserRoleSFList : BaseList
    {
        private static UserRoleSFList _uniqueInstance;

        private UserRoleSFList(string tableName)
            : base(tableName)
        { }

        public static UserRoleSFList GetUniqueInstance()
        {
            if (_uniqueInstance == null)
                _uniqueInstance = new UserRoleSFList("SF_UserRoleSF");

            return _uniqueInstance;
        }

        public override DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Пользователь");
            dt.Columns.Add("Роль");

            var list = List.OrderBy(item => (item as UserRoleSF).User.Name);

            foreach (var item in list)
                dt.Rows.Add(item.GetRow());

            return dt;
        }
    }
}
