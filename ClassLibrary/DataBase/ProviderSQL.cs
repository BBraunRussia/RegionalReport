﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataLayer;

namespace ClassLibrary
{
    public class ProviderSQL : IProvider
    {
        private static IDataBase _db;

        public ProviderSQL()
        {
            _db = DataBase.GetDataBase();
        }

        public DataTable Select(string tableName)
        {
            return _db.GetRecords("exec " + tableName + "_Select");
        }

        public string SelectOne(string tableName)
        {
            DataTable dt = Select(tableName);

            if (dt.Rows.Count > 0)
                return dt.Rows[0].ItemArray[0].ToString();
            else
                throw new Exception("Пустое значение");
        }

        public string Insert(string tableName, params object[] Params)
        {
            return _db.GetRecordsOne("exec " + tableName + "_Insert " + CreateParamsString(Params), Params);
        }
                
        public string Update(string tableName, params object[] Params)
        {
            return _db.GetRecordsOne("exec " + tableName + "_Update " + CreateParamsString(Params), Params);
        }

        private string CreateParamsString(params object[] Params)
        {
            StringBuilder paramList = new StringBuilder();

            for (int i = 1; i <= Params.Count(); i++)
            {
                if (paramList.ToString() != string.Empty)
                    paramList.Append(", ");
                paramList.Append("@p" + i);
            }

            return paramList.ToString();
        }

        public string Delete(string tableName, int id)
        {
            return _db.GetRecordsOne("exec " + tableName + "_Delete @p1", id);
        }

        public DataTable DoOther(string sql, params object[] Params)
        {
            return _db.GetRecords(sql, Params);
        }
    }
}
