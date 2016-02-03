using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace RegionR
{
    class globalData
    {
        public static String Region { get; set; }
        public static String Div { get; set; }
        public static String input { get; set; }
        public static DataTable dt { get; set; }
        public static String Login { get; set; }
        public static String UserName { get; set; }
        public static int role { get; set; } // для рент
        public static bool tabFlagRD { get; set; } // для рент
        public static bool tabFlagSvod { get; set; } // для рент
        public static int UserID { get; set; }
        public static int UserID2 { get; set; }
        public static int UserAccess { get; set; }
        public static bool load { get; set; }
        public static int UserEdit1 { get; set; }
        public static bool admin { get; set; }
        public static String RD { get; set; }
        public static int db_id { get; set; }
        public static DateTime CurDate { get; set; }
        public static DateTime CurDateFull { get; set; }
        public static int fp { get; set; }
        public static int form { get; set; }
        public static string year { get; set; }
        public static bool update { get; set; }
        public static string serv { get; set; }
        public static string db { get; set; }
        public static int indexRow { get; set; }
        public static int indexCol { get; set; }
        public static int indexRow2 { get; set; }
        public static int indexCol2 { get; set; }
        //public static bool editFact { get; set; }
        //public static bool editPlan { get; set; }
        public static string text { get; set; }
        public static DataTable dtSBA { get; set; }
        /* Отчёт дистрибьютора */
        public static string colReg { get; set; }
        public static string colMatCode { get; set; }
        public static string colMatName { get; set; }
        public static string colCount { get; set; }
        public static string colLPU { get; set; }
        public static string colLast { get; set; }
        
        public static string folderName { get; set; }
        public static string filePass { get; set; }
        public static int rowBegin { get; set; }

        /* Маркетинг - ссылка на конференцию из справочника */
        public static int maplan { get; set; }
        public static string mayear { get; set; }
        public static string matype { get; set; }

        /* Ассортиментный план с историей по ЛПУ или нет */
        public static bool acc_history { get; set; }
    }
}
