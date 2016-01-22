using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DataLayer
{
    public class sql
    {
        //private String _Server = @"bbmru07";
        private String _Server = @"bbmru08";
        //private String _Server = @"bbmru04\sub";
        private String _Database;
        //private String _Database = "RegionalR_Test2";
        //private String _Database = "RegionalR_10042015";
        private Boolean _WinAuth = false;
        private String _UserID;
        private String _Password;

        private SqlConnection _con;

        public sql()
        {
            _Database = DBNames.RegionalR_TestSF.ToString();

            if (_Server == @"bbmru09")
            {
                _UserID = "sa";
                _Password = "gfdtk";
            }
            else
            {
                _UserID = "RegionalR_user";
                _Password = "regionalr78";
            }

            Init();
        }

        private string Init()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = _Server;
            csb.InitialCatalog = _Database;
            csb.IntegratedSecurity = _WinAuth;
            if (!_WinAuth)
            {
                csb.UserID = _UserID;
                csb.Password = _Password;
            }
            try
            {
                _con = new SqlConnection(csb.ConnectionString);
                return String.Empty;
            }
            catch (Exception ex) { return ex.Message; }
        }

        public string getServerName()
        {
            if (_Server == @"bbmru08")
                return "Lan";
            if (_Server == @"bbmru09")
                return "Internet";

            return "Тестовая";
        }

        public string getDbName()
        {
            return _Database;
        }

        public int getDbId()
        {
            if (_Server == @"bbmru08")
                return 2;
            if (_Server == @"bbmru09")
                return 1;

            return 4;
        }        

        public String Disconnect()
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
            DataTable outDT = new DataTable();
            try
            {
                SqlCommand _com = new SqlCommand(SQL, _con);
                _com.CommandTimeout = 600;
                for (int i = 0; i < Params.Length; i++)
                {
                    int nump = i + 1;
                    SqlParameter _prm = new SqlParameter(String.Format("p{0}", nump.ToString()), Params[i]);
                    _com.Parameters.Add(_prm);
                }
                SqlDataAdapter _da = new SqlDataAdapter(_com);
                _da.Fill(outDT);
                Disconnect();
                return outDT;
            }
            catch
            {
                Disconnect();
                return null;
            }
        }
    }
}
