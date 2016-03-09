using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary
{
    public interface IProvider
    {
        DataTable Select(string tableName);
        string Insert(string tableName, params object[] Params);
        DataTable DoOther(string sql, params object[] Params);
        string SelectOne(string tableName);
        string Delete(string tableName, int id);
        string Update(string tableName, params object[] Params);
    }
}
