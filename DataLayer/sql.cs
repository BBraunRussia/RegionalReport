using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataLayer
{
    public enum DBNames { RegionalR, Competitors };
    public enum ServerName { bbmru08, bbmru09 };

    public class Sql : IDataBase
    {
        private const int TIMEOUT = 600;

        private string _server;
        private string _database;
        private bool _winAuth = false;
        private string _userID;
        private string _password;

        private SqlConnection _con;

        public Sql()
        {
            _database = DBNames.RegionalR.ToString();
            _server = ServerName.bbmru08.ToString();

            if (_server == ServerName.bbmru09.ToString())
            {
                _userID = "sa";
                _password = "gfdtk";
            }
            else
            {
                _userID = "RegionalR_user";
                _password = "regionalr78";
            }

            Init();
        }

        public void ChangeDataBase(DBNames name)
        {
            _database = name.ToString();
            Init();
        }

        private string Init()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = _server;
            csb.InitialCatalog = _database;
            csb.IntegratedSecurity = _winAuth;
            if (!_winAuth)
            {
                csb.UserID = _userID;
                csb.Password = _password;
            }
            try
            {
                _con = new SqlConnection(csb.ConnectionString);
                _con.Open();
                return String.Empty;
            }
            catch (Exception ex) { return ex.Message; }
        }

        private String Disconnect()
        {
            try
            {
                _con.Close();
                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (_con.State != ConnectionState.Closed) _con.Close();
            }
        }

        public DataTable GetRecords(String SQL, params Object[] Params)
        {
            if (isOpenedConnection())
                return tryToGetRecords(SQL, Params);
            else
                return null;
        }

        public string GetRecordsOne(String SQL, params Object[] Params)
        {
            if (isOpenedConnection())
                return tryGetRecordsOne(SQL, Params);
            else
                return string.Empty;
        }

        private bool isOpenedConnection()
        {
            if ((_con == null) || (_con.State != ConnectionState.Open))
                _con.Open();

            return (_con != null) && (_con.State == ConnectionState.Open);
        }

        private string tryGetRecordsOne(String SQL, params Object[] Params)
        {
            DataTable dt1 = tryToGetRecords(SQL, Params);

            if ((dt1 != null) && (dt1.Rows.Count > 0))
                return dt1.Rows[0].ItemArray[0].ToString();

            return string.Empty;
        }

        private DataTable tryToGetRecords(String SQL, params Object[] Params)
        {
            DataTable Out = new DataTable();

            SqlDataAdapter sqlDataAdapter = CreateSqlDataAdapter(SQL, Params);
            sqlDataAdapter.Fill(Out);
            Disconnect();
            return Out;
        }

        private SqlDataAdapter CreateSqlDataAdapter(String SQL, params Object[] Params)
        {
            SqlCommand sqlCommand = CreateSqlCommand(SQL, Params);
            return new SqlDataAdapter(sqlCommand);
        }

        private SqlCommand CreateSqlCommand(String SQL, params Object[] Params)
        {
            SqlCommand sqlCommand = new SqlCommand(SQL, _con);
            sqlCommand.CommandTimeout = TIMEOUT;

            for (int i = 0; i < Params.Length; i++)
                sqlCommand.Parameters.Add(GetParam(i, Params));

            return sqlCommand;
        }

        private SqlParameter GetParam(int paramIndex, params Object[] Params)
        {
            return new SqlParameter(String.Format("p{0}", (paramIndex + 1).ToString()), Params[paramIndex]);
        }

        public string getServerName()
        {
            if (_server == @"bbmru08")
                return "Lan";
            if (_server == @"bbmru09")
                return "Internet";

            return "Тестовая";
        }

        public string getDbName()
        {
            return _database;
        }

        public int getDbId()
        {
            if (_server == @"bbmru08")
                return 2;
            if (_server == @"bbmru09")
                return 1;

            return 4;
        }
    }
}
