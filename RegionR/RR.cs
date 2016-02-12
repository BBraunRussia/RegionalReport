using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using RegionR.Directories;
using Excel = Microsoft.Office.Interop.Excel;
using RegionR.other;
using System.Security.Principal;
using RegionR.help;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using DataLayer;
using ClassLibrary;
using ClassLibraryBBAuto;

namespace RegionR
{
    public partial class RR : Form
    {
        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgreen1 = Color.FromArgb(0, 180, 130);
        Color bbgray4 = Color.FromArgb(150, 150, 150);
        Color bbgray5 = Color.FromArgb(230, 230, 230);

        int ma_old = 0;
        int ma_new = 0;

        String[] f = new String[6];
        int rb = 5;
        int begval = 0;
        Double begval2 = 0;
        string proc1 = "";
        string proc2 = "";
        bool load = false;
        TreeNode temptn;
        
        int subsum = 0, subsumKS = 0;
        bool loadusers = true;
        bool filter = false;
        int curbtn = 0;
        bool fillFilter = true;
        Point point = new Point();
        static object locker = new object();
        BackgroundWorker[] bgw;
        string button = "";
        bool KosReg = false; // флаг проверки косвенных продаж по регионам

        VisitList visitList;

        public RR()
        {
            InitializeComponent();

            DataBase.InitDataBase();
            Provider.InitSQLProvider();

            /* Закрыть доступ ОPM на косвенные продажи * /
            btnAdd.Enabled =  false;
            btnEdit.Enabled = false;
            btnDel.Enabled =  false;
            /**/
            setCurDateForGlobalDataAndForComponents();

            getSetting();

            button25.BackColor = bbgreen3;
            button31.BackColor = bbgreen3;

            this.Text = Application.ProductName + " " + Application.ProductVersion + ", версия " + globalData.serv + ", база данных " + globalData.db;
            Connect();
        }

        private void setCurDateForGlobalDataAndForComponents()
        {
            String date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-01";

            Sql sql1 = new Sql();

            String sqldate = sql1.GetRecordsOne("exec GetDateCur @p1", date);
            String serverdate = sql1.GetRecordsOne("exec GetDateServer");

            DateTime date2 = Convert.ToDateTime(serverdate);
            String date3 = date2.Year + "-" + date2.Month + "-" + date2.Day;

            globalData.CurDateFull = date2.Date;

            if (sqldate == null)
                globalData.CurDate = Convert.ToDateTime(date);
            else
                globalData.CurDate = Convert.ToDateTime(sqldate);

            setCurDateForDateTimePickers();
        }

        private void setCurDateForDateTimePickers()
        {
            dateTimePicker1.Value = globalData.CurDate;
            dateTimePicker2.Value = globalData.CurDate;
            dateTimePicker3.Value = globalData.CurDate;
            dateTimePicker4.Value = globalData.CurDate;
            dateTimePicker5.Value = globalData.CurDate;
            dateTimePicker6.Value = globalData.CurDate;
            dateTimePicker7.Value = globalData.CurDate;
            dateTimePicker8.Value = globalData.CurDate;
            dateTimePicker9.Value = globalData.CurDate;
            dateTimePicker15.Value = globalData.CurDate;
            dateTimePicker17.Value = globalData.CurDate;
            dateTimePicker18.Value = globalData.CurDate;
            dateTimePicker19.Value = globalData.CurDate;
            dateTimePicker20.Value = globalData.CurDate;

            fillComboBoxYear(cbYearMA);
            fillComboBoxYear(cbYearAcc);
            fillComboBoxYear(cbYearAccAll);
            fillComboBoxYear(cbYearDyn);
            fillComboBoxYear(cbYearAllPrivSales);
            fillComboBoxYear(cbYearCheck);
            fillComboBoxYear(cbYearEvo);
            fillComboBoxYear(cbYearEvoRP);
            fillComboBoxYear(comboBox9);
        }

        private void fillComboBoxYear(ComboBox combo)
        {
            combo.Items.Add(globalData.CurDate.Year - 4);
            combo.Items.Add(globalData.CurDate.Year - 3);
            combo.Items.Add(globalData.CurDate.Year - 2);
            combo.Items.Add(globalData.CurDate.Year - 1);
            combo.Items.Add(globalData.CurDate.Year);
            combo.Items.Add(globalData.CurDate.Year + 1);
            combo.SelectedIndex = combo.Items.Count - 2;
        }

        private void getSetting()
        {
            Sql sql1 = new Sql();

            DataTable dtNY = new DataTable();
            dtNY = sql1.GetRecords("exec GetSettings");

            globalData.year = dtNY.Rows[0].ItemArray[3].ToString();

            globalData.serv = sql1.getServerName();
            globalData.db = sql1.getDbName();
            globalData.db_id = sql1.getDbId();
        }

        private int Connect()
        {            
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
            String str = "Пользователь: " + globalData.UserName;
           
            if ((globalData.UserAccess == 1) && (globalData.UserID == 1))
            {
                str = "Рад видеть тебя, мой Создатель.";
            }

            str += "      Последний раз вы были в системе: " + sql1.GetRecordsOne("exec LastEvent @p1", globalData.UserID);

            lbUserName.Text = str;

            setVisibleElementMainMenu();

            CreateTree();
            ClearFilter();
            treeView1.Focus();

            if (globalData.fp == 1)
            {
                tabControl1.SelectedIndex = 10;
                tabControl1.Visible = true;                
            }
            
            return 0;
        }

        private void setVisibleElementMainMenu()
        {
            actionToolStripMenuItem.Visible = false;
            importToolStripMenuItem.Visible = false;
            logoutToolStripMenuItem.Visible = false;
            repDistToolStripMenuItem.Visible = false;
            comparisonMatToolStripMenuItem.Visible = false;
            settingsToolStripMenuItem.Visible = false;
            toolStripMenuItem1.Visible = false;
            toolStripMenuItem2.Visible = false;
            btnEditReport.Visible = false;
            toolStripMenuItem6.Visible = false;
            divideToolStripMenuItem.Visible = false;
            usersToolStripMenuItem1.Visible = false;

            регионыToolStripMenuItem.Visible = false;
            отчётыToolStripMenuItem.Visible = false;
            косвенныеToolStripMenuItem.Visible = false;
            MatForBtnToolStripMenuItem.Visible = false;

            nomForAccToolStripMenuItem.Visible = false;
            RepDistRightToolStripMenuItem.Visible = false;
            trackersToolStripMenuItem.Visible = false;
            userManagerToolStripMenuItem.Visible = false;
            userBCToolStripMenuItem.Visible = false;
            userRoleSFToolStripMenuItem.Visible = false;

            if (globalData.UserAccess == 1)
            {
                actionToolStripMenuItem.Visible = true;
                importToolStripMenuItem.Visible = true;
                logoutToolStripMenuItem.Visible = true;
                repDistToolStripMenuItem.Visible = true;
                comparisonMatToolStripMenuItem.Visible = true;
                settingsToolStripMenuItem.Visible = true;
                toolStripMenuItem1.Visible = true;
                toolStripMenuItem2.Visible = true;
                btnEditReport.Visible = true;
                toolStripMenuItem6.Visible = true;
                divideToolStripMenuItem.Visible = true;
                usersToolStripMenuItem1.Visible = true;

                регионыToolStripMenuItem.Visible = true;
                отчётыToolStripMenuItem.Visible = true;
                косвенныеToolStripMenuItem.Visible = true;
                MatForBtnToolStripMenuItem.Visible = true;

                nomForAccToolStripMenuItem.Visible = true;
                RepDistRightToolStripMenuItem.Visible = true;
                trackersToolStripMenuItem.Visible = true;
                userManagerToolStripMenuItem.Visible = true;
                userBCToolStripMenuItem.Visible = true;
                userRoleSFToolStripMenuItem.Visible = true;
            }
            if ((globalData.UserAccess == 2) || (globalData.UserAccess == 3) || (globalData.UserAccess == 4))
            {
                importToolStripMenuItem.Visible = true;
                importReportToolStripMenuItem.Visible = false;
                loadLPUToolStripMenuItem.Visible = false;
                customersToolStripMenuItem.Visible = false;
                materialsToolStripMenuItem.Visible = false;
                userBCToolStripMenuItem.Visible = true;
            }

            if (globalData.UserAccess == 11)
                trackersToolStripMenuItem.Visible = true;
        }

        private void CreateTree()
        {
            try
            {
                TreeNode tn = new TreeNode();
                TreeNode tn2 = new TreeNode();
                TreeNode tn3 = new TreeNode();
                TreeNode tn4 = new TreeNode();

                if ((globalData.UserAccess == 1) || (globalData.UserAccess == 3) || (globalData.UserAccess == 9))
                {
                    treeView1.Nodes.Add("Все регионы");
                    treeView1.Nodes.Add("Отчёты по компаниям");
                    treeView1.Nodes.Add("Отчёты для руководства");
                    treeView1.Nodes.Add("Отчёты по региональным директорам");
                    treeView1.Nodes.Add("Отчёты дистрибьюторов");
                }
              
                if ((globalData.UserAccess == 2) || (globalData.UserAccess == 4))
                {
                    treeView1.Nodes.Add("Мои регионы");
                    treeView1.Nodes.Add("Все регионы");
                    treeView1.Nodes.Add("Отчёты для руководства");
                    treeView1.Nodes.Add("Отчёты по региональным директорам");
                    treeView1.Nodes.Add("Отчёты дистрибьюторов");
                }
                if ((globalData.UserAccess == 5) || (globalData.UserAccess == 6) || (globalData.UserAccess == 7))
                {
                    treeView1.Nodes.Add("Мои регионы");
                    treeView1.Nodes.Add("Все регионы");
                    treeView1.Nodes.Add("Отчёты дистрибьюторов");
                    treeView1.Nodes.Add("Выполнение плана");
                }
                if (globalData.UserAccess == 8)
                {
                    treeView1.Nodes.Add("Отчёты для руководства");
                }
                if (globalData.UserAccess == 1 || (globalData.UserAccess == 4) || (globalData.UserAccess == 6) || (globalData.UserAccess == 2) || (globalData.UserAccess == 5))
                {
                    if ((globalData.UserAccess == 4) || (globalData.UserAccess == 6) || (globalData.UserAccess == 2) || (globalData.UserAccess == 5))
                    {
                        treeView1.Nodes.Add("Проверка");
                        if (globalData.UserAccess == 4 || globalData.UserAccess == 2)
                            tn = treeView1.Nodes[5];
                        else
                            tn = treeView1.Nodes[4];
                        tn.Nodes.Add("Сравнение продаж");
                        tn.Nodes.Add("Проверка косвенных по регионам");
                        if (globalData.UserAccess != 5)
                            tn.Nodes.Add("Проверка косвенных по номенклатуре");
                                                
                        tn2 = tn.Nodes[0];
                        tn2.Nodes.Add("HC");
                        tn2.Nodes.Add("AE");
                    }
                    else
                    {
                        treeView1.Nodes.Add("Проверка");
                        tn = treeView1.Nodes[5];
                        tn.Nodes.Add("Ассортиментные планы");
                        tn.Nodes.Add("Косвенные продажи");
                        tn.Nodes.Add("Отчёт из SAP");
                        tn.Nodes.Add("Сравнение продаж");
                        tn.Nodes.Add("Проверка косвенных продаж");
                        tn.Nodes.Add("По отчётам дистрибьюторов");
                        tn.Nodes.Add("Проверка косвенных по регионам");
                        tn.Nodes.Add("Ассортиментные планы на год");                        

                        tn2 = tn.Nodes[0];
                        tn2.Nodes.Add("HC");
                        tn2.Nodes.Add("AE");

                        tn2 = tn.Nodes[1];
                        tn2.Nodes.Add("HC");
                        tn2.Nodes.Add("AE");
                        tn2.Nodes.Add("OM");

                        tn2 = tn.Nodes[3];
                        tn2.Nodes.Add("HC");
                        tn2.Nodes.Add("AE");
                    }
                }
                treeView1.Nodes.Add("Visits", "Визиты");
                tn = treeView1.Nodes["Visits"];
                tn.Nodes.Add("Планировщик");
                tn.Nodes.Add("Отчёт");


                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                string acs = string.Empty;

                if ((globalData.UserAccess == 5) || (globalData.UserAccess == 6) || (globalData.UserAccess == 2))
                    acs = sql1.GetRecordsOne("exec GetUserRentAccess @p1", globalData.UserID);

                if ((globalData.UserAccess == 1) || (globalData.UserID == 78) || (globalData.UserID == 7) || (globalData.UserAccess == 2 && acs == "1") || (globalData.UserAccess == 5 && acs == "1") || (globalData.UserAccess == 13) || (globalData.UserAccess == 6 && acs == "1") || (globalData.UserAccess == 4) || (globalData.UserAccess == 3))
                {
                    treeView1.Nodes.Add("Rent", "Отчётность");
                    tn = treeView1.Nodes["Rent"];
                    if (globalData.UserAccess != 3)
                    {
                        tn.Nodes.Add("Маркетинг 1");
                        tn2 = tn.Nodes[0];
                        if ((globalData.UserAccess == 1) || (globalData.UserAccess == 5) || (globalData.UserAccess == 2) || (globalData.UserID == 7) || (globalData.UserAccess == 6) || (globalData.UserAccess == 4) || (globalData.UserID == 78) || (globalData.UserAccess == 13))
                            tn2.Nodes.Add("Заполнение");
                        if ((globalData.UserAccess == 1) || (globalData.UserAccess == 4) || (globalData.UserID == 78) || (globalData.UserID == 7) || (globalData.UserAccess == 13))
                            tn2.Nodes.Add("Отчёт для РД");
                        if ((globalData.UserAccess == 1) || (globalData.UserID == 78) || (globalData.UserID == 7) || (globalData.UserID == 6) || (globalData.UserAccess == 13))
                            tn2.Nodes.Add("Итоговый отчёт");
                        if ((globalData.UserAccess == 1) || (globalData.UserAccess == 13))
                            tn2.Nodes.Add("Сводный отчёт");
                        if ((globalData.UserAccess == 1) || (globalData.UserAccess == 13))
                            tn2.Nodes.Add("Отчёт для контроллинга");
                        if ((globalData.UserAccess == 1) || (globalData.UserAccess == 4) || (globalData.UserID == 78) || (globalData.UserID == 7) || (globalData.UserID == 6) || (globalData.UserAccess == 13))
                            tn2.Nodes.Add("Сторно");
                    }
                    if ((globalData.UserAccess == 3) || (globalData.UserAccess == 1) || (globalData.UserAccess == 4))
                        tn.Nodes.Add("Маркетинг 2");

                }


                treeView1.Nodes.Add("Справочник пользователей");


                tn = treeView1.Nodes[0];
               

                DataTable dtNY = new DataTable();
                dtNY = sql1.GetRecords("exec GetSettings");

                string year = dtNY.Rows[0].ItemArray[3].ToString();

                int i = 0;

                if ((globalData.UserAccess == 1) || (globalData.UserAccess == 3) || (globalData.UserAccess == 9))
                {
                    dt1 = sql1.GetRecords("exec Region_Select");

                    foreach (DataRow row in dt1.Rows)
                    {
                        tn.Nodes.Add(row[1].ToString());
                        tn2 = tn.Nodes[i];

                        tn2.Nodes.Add("Динамика продаж");
                        tn2.Nodes.Add("Ассортиментный план");
                        tn2.Nodes.Add("Косвенные продажи");
                        tn2.Nodes.Add("Маркетинговые мероприятия");
                        tn2.Nodes.Add("Ассортиментный план " + year);
                        tn3 = tn2.Nodes[0];
                        tn3.Nodes.Add("Анализ продаж");

                        int t = 1;

                        if (globalData.UserAccess != 9)
                        {
                            tn3.Nodes.Add("Динамика продаж");
                            t = 2;
                        }

                        for (int j = 0; j < t; j++)
                        {
                            tn4 = tn3.Nodes[j];
                            if ((globalData.UserAccess == 1) || (globalData.UserAccess == 2))
                            {
                                tn4.Nodes.Add("HC");
                                tn4.Nodes.Add("AE");
                                tn4.Nodes.Add("OM");
                            }
                            else if ((globalData.UserAccess == 3) || (globalData.UserAccess == 9))
                            {
                                dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, 0", globalData.UserID);
                                foreach (DataRow row3 in dt3.Rows)
                                    tn4.Nodes.Add(row3[1].ToString());
                            }
                        }
                        
                        t = 1;

                        if ((globalData.UserAccess == 1) || (globalData.UserAccess == 2))
                        {
                            tn3 = tn2.Nodes[t];
                            tn3.Nodes.Add("HC");
                            tn3.Nodes.Add("AE");

                            tn3 = tn2.Nodes[t + 1];
                            tn3.Nodes.Add("HC");
                            tn3.Nodes.Add("AE");
                            tn3.Nodes.Add("OM");

                            tn3 = tn2.Nodes[t + 2];
                            tn3.Nodes.Add("HC");
                            tn3.Nodes.Add("AE");

                            tn3 = tn2.Nodes[t + 3];
                            tn3.Nodes.Add("HC");
                            tn3.Nodes.Add("AE");
                        }
                        else
                        {
                            dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, 0", globalData.UserID);
                            tn3 = tn2.Nodes[t];
                            tn4 = tn2.Nodes[t + 2];
                            foreach (DataRow row3 in dt3.Rows)
                            {
                                if ((row3[1].ToString() == "HC") || (row3[1].ToString() == "AE"))
                                {
                                    tn3.Nodes.Add(row3[1].ToString());
                                    tn4.Nodes.Add(row3[1].ToString());
                                }
                            }
                            tn3 = tn2.Nodes[t + 1];
                            tn4 = tn2.Nodes[t + 3];

                            foreach (DataRow row3 in dt3.Rows)
                            {
                                tn3.Nodes.Add(row3[1].ToString());
                                tn4.Nodes.Add(row3[1].ToString());
                            }
                        }

                        i++;
                    }
                }
                else if ((globalData.UserAccess == 2) || (globalData.UserAccess == 4) || (globalData.UserAccess == 5) || (globalData.UserAccess == 6) || (globalData.UserAccess == 7))
                {
                    dt1 = sql1.GetRecords("exec SelRegionByUserID @p1", globalData.UserID);

                    foreach (DataRow row in dt1.Rows)
                    {
                        tn.Nodes.Add(row[1].ToString());
                        tn2 = tn.Nodes[i];

                        tn2.Nodes.Add("Динамика продаж");
                        tn2.Nodes.Add("Ассортиментный план");
                        tn2.Nodes.Add("Косвенные продажи");
                        tn2.Nodes.Add("Маркетинговые мероприятия");
                        tn2.Nodes.Add("Ассортиментный план " + year);
                        //tn2.Nodes.Add("Асс. план " + year + " (c историей по ЛПУ)");
                        tn3 = tn2.Nodes[0];
                        tn3.Nodes.Add("Анализ продаж");

                        int t = 1;

                        tn3.Nodes.Add("Динамика продаж");
                        t = 2;

                        for (int j = 0; j < t; j++)
                        {
                            tn4 = tn3.Nodes[j];
                            
                            dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, @p2", globalData.UserID, row[0]);
                            foreach (DataRow row3 in dt3.Rows)
                                tn4.Nodes.Add(row3[1].ToString());
                        }

                        t = 1;

                        
                        dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, 0", globalData.UserID);
                        tn3 = tn2.Nodes[t];
                        tn4 = tn2.Nodes[t + 2];
                        foreach (DataRow row3 in dt3.Rows)
                        {
                            if ((row3[1].ToString() == "HC") || (row3[1].ToString() == "AE"))
                            {
                                tn3.Nodes.Add(row3[1].ToString());
                                tn4.Nodes.Add(row3[1].ToString());
                            }
                        }
                        tn3 = tn2.Nodes[t + 1];
                        tn4 = tn2.Nodes[t + 3];
                        foreach (DataRow row3 in dt3.Rows)
                        {
                            tn3.Nodes.Add(row3[1].ToString());
                            tn4.Nodes.Add(row3[1].ToString());
                        }

                        i++;
                    }
                    tn.Nodes.Add("Российская федерация");
                    tn2 = tn.Nodes[i];

                    tn2.Nodes.Add("Динамика продаж");
                    tn3 = tn2.Nodes[0];
                    tn3.Nodes.Add("Анализ продаж");
                    tn4 = tn3.Nodes[0];
                    foreach (DataRow row3 in dt3.Rows)
                        tn4.Nodes.Add(row3[1].ToString());


                    i = 0;

                    dt1 = sql1.GetRecords("exec Region_Select");

                    foreach (DataRow row in dt1.Rows)
                    {
                        tn = treeView1.Nodes[1];
                        tn.Nodes.Add(row[1].ToString());
                        tn2 = tn.Nodes[i];

                        tn2.Nodes.Add("Динамика продаж");
                        tn2.Nodes.Add("Ассортиментный план");
                        tn2.Nodes.Add("Косвенные продажи");
                        tn2.Nodes.Add("Маркетинговые мероприятия");
                        tn2.Nodes.Add("Ассортиментный план " + year);
                        tn3 = tn2.Nodes[0];
                        tn3.Nodes.Add("Анализ продаж");

                        int t = 1;

                        tn3.Nodes.Add("Динамика продаж");
                        t = 2;

                        for (int j = 0; j < t; j++)
                        {
                            tn4 = tn3.Nodes[j];

                            foreach (DataRow row3 in dt3.Rows)
                                tn4.Nodes.Add(row3[1].ToString());
                        }

                        t = 1;

                        tn3 = tn2.Nodes[t];
                        tn4 = tn2.Nodes[t + 2];
                        foreach (DataRow row3 in dt3.Rows)
                        {
                            if ((row3[1].ToString() == "HC") || (row3[1].ToString() == "AE"))
                            {
                                tn3.Nodes.Add(row3[1].ToString());
                                tn4.Nodes.Add(row3[1].ToString());
                            }
                        }
                        tn3 = tn2.Nodes[t + 1];
                        tn4 = tn2.Nodes[t + 3];
                        foreach (DataRow row3 in dt3.Rows)
                        {
                            tn3.Nodes.Add(row3[1].ToString());
                            tn4.Nodes.Add(row3[1].ToString());
                        }

                        i++;
                    }
                }


                if ((globalData.UserAccess == 1) || (globalData.UserAccess == 2) || (globalData.UserAccess == 4))
                {
                    if (globalData.UserAccess == 1)
                    {
                        tn = treeView1.Nodes[1];
                        tn.Nodes.Add("Динамика продаж по Кардиологии");
                        tn.Nodes.Add("Динамика продаж по Шовным материалам");
                        tn = treeView1.Nodes[2];
                     
                    }
                    else
                    {
                        tn = treeView1.Nodes[2];
                    }
                    tn.Nodes.Add("HC", "HC");
                    tn.Nodes.Add("AE", "AE");
                    tn.Nodes.Add("OM", "OM");

                    tn2 = tn.Nodes["HC"];
                    tn2.Nodes.Add("Общая динамика продаж");
                    tn2.Nodes.Add("Общий ассортиментный план");
                    tn2.Nodes.Add("Личные продажи (список)");
                    tn2.Nodes.Add("Общий маркетинговый план");
                    tn2.Nodes.Add("Общий ассортиментный план " + year);
                    tn2.Nodes.Add("Выполнение плана по России");
                    tn2.Nodes.Add("VisitsRegDir", "Визиты");
                    tn3 = tn2.Nodes["VisitsRegDir"];
                    tn3.Nodes.Add("Планировщик");
                    tn3.Nodes.Add("Отчёт");

                    tn2 = tn.Nodes["AE"];
                    tn2.Nodes.Add("Общая динамика продаж");
                    tn2.Nodes.Add("Общий ассортиментный план");
                    tn2.Nodes.Add("Личные продажи (список)");
                    tn2.Nodes.Add("Общий маркетинговый план");
                    tn2.Nodes.Add("Общий ассортиментный план " + year);
                    tn2.Nodes.Add("VisitsRegDir", "Визиты");
                    tn3 = tn2.Nodes["VisitsRegDir"];
                    tn3.Nodes.Add("Планировщик");
                    tn3.Nodes.Add("Отчёт");

                    tn2 = tn.Nodes["OM"];
                    tn2.Nodes.Add("Общая динамика продаж");
                    tn2.Nodes.Add("Личные продажи (список)");
                    tn2.Nodes.Add("VisitsRegDir", "Визиты");
                    tn3 = tn2.Nodes["VisitsRegDir"];
                    tn3.Nodes.Add("Планировщик");
                    tn3.Nodes.Add("Отчёт");

                    if (globalData.UserAccess == 1)
                    {
                        tn = treeView1.Nodes[3];
                    }
                    else
                    {
                        tn = treeView1.Nodes[3];
                    }

                    dt1 = sql1.GetRecords("exec SelRegDir");
                    foreach (DataRow row in dt1.Rows)
                    {
                        tn.Nodes.Add(row[1].ToString());
                    }
                    foreach (TreeNode node in tn.Nodes)
                    {
                        node.Nodes.Add("Общая динамика продаж");
                        tn2 = node.Nodes[0];
                        tn2.Nodes.Add("HC");
                        tn2.Nodes.Add("AE");
                        tn2.Nodes.Add("OM");

                        node.Nodes.Add("Общий ассортиментный план");
                        node.Nodes.Add("Выполнение плана");
                        node.Nodes.Add("Маркетинговые мероприятия");
                        node.Nodes.Add("Общий ассортиментный план " + year);
                        node.Nodes.Add("Общий асс. план " + year + "(с историей по ЛПУ)");
                        node.Nodes.Add("Visits", "Визиты");
                        tn = node.Nodes["Visits"];
                        tn.Nodes.Add("Планировщик");
                        tn.Nodes.Add("Отчёт");

                        for (int j = 1; j < 6; j++)
                        {
                            tn2 = node.Nodes[j];
                            tn2.Nodes.Add("HC");
                            tn2.Nodes.Add("AE");
                        }
                    }
                }
                else if ((globalData.UserAccess == 2) || (globalData.UserAccess == 4) || (globalData.UserAccess == 5) || (globalData.UserAccess == 6) || (globalData.UserAccess == 7))
                {
                    if (i == 1)
                    {
                        treeView1.Nodes[0].Expand();
                        tn = treeView1.Nodes[0];
                        tn.Nodes[0].Expand();
                    }
                    else
                        treeView1.Nodes[0].Expand();

                }
                else if (globalData.UserAccess == 3)
                {
                    tn = treeView1.Nodes[1];
                    tn.Nodes.Add("Динамика продаж по Кардиологии");
                    tn.Nodes.Add("Динамика продаж по Шовным материалам");
                    tn = treeView1.Nodes[2];

                    int temp = 0;
                    dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, 0", globalData.UserID);
                    foreach (DataRow row in dt3.Rows)
                    {
                        tn.Nodes.Add(row[1].ToString());
                        tn2 = tn.Nodes[temp];
                        tn2.Nodes.Add("Общая динамика продаж");
                        if (row[1].ToString() != "OM")
                            tn2.Nodes.Add("Общий ассортиментный план");
                        tn2.Nodes.Add("Личные продажи (список)");
                        tn2.Nodes.Add("Общий ассортиментный план " + year);
                        tn2.Nodes.Add("VisitsRegDir", "Визиты");
                        tn3 = tn2.Nodes["VisitsRegDir"];
                        tn3.Nodes.Add("Планировщик");
                        tn3.Nodes.Add("Отчёт");
                        temp++;
                    }
                    
                    tn = treeView1.Nodes[3];

                    foreach (DataRow row3 in dt3.Rows)
                    {
                        dt1 = sql1.GetRecords("exec SelRegDir @p1", row3[0]);
                        foreach (DataRow row in dt1.Rows)
                        {
                            tn.Nodes.Add(row[1].ToString());
                        }
                        foreach (TreeNode node in tn.Nodes)
                        {
                            node.Nodes.Add("Общая динамика продаж");
                            tn2 = node.Nodes[0];
                            tn2.Nodes.Add(row3[1].ToString());

                            node.Nodes.Add("Общий ассортиментный план");
                            node.Nodes.Add("Выполнение плана");
                            node.Nodes.Add("Маркетинговые мероприятия");
                            node.Nodes.Add("Общий ассортиментный план " + year);
                            node.Nodes.Add("Visits", "Визиты");
                            tn = node.Nodes["Visits"];
                            tn.Nodes.Add("Планировщик");
                            tn.Nodes.Add("Отчёт");

                            for (int j = 1; j < 5; j++)
                            {
                                tn2 = node.Nodes[j];
                                tn2.Nodes.Add(row3[1].ToString());
                            }
                        }
                    }
                }
                else if (globalData.UserAccess == 8)
                {
                    tn.Nodes.Add("HC");
                    tn.Nodes.Add("AE");

                    tn2 = tn.Nodes[0];
                    tn2.Nodes.Add("Общий ассортиментный план");

                    tn2 = tn.Nodes[1];
                    tn2.Nodes.Add("Общий ассортиментный план");
                }
                else if (globalData.UserAccess == 9)
                {
                    tn = treeView1.Nodes[1];
                    tn.Nodes.Add("Динамика продаж по Кардиологии");
                    tn.Nodes.Add("Динамика продаж по Шовным материалам");
                    tn = treeView1.Nodes[2];

                    dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, 0", globalData.UserID);
                    foreach (DataRow row in dt3.Rows)
                    {
                        tn.Nodes.Add(row[1].ToString());
                        tn2 = tn.Nodes[0];
                        if (row[1].ToString() != "OM")
                            tn2.Nodes.Add("Общий ассортиментный план");
                    }

                    tn = treeView1.Nodes[3];

                    dt3 = sql1.GetRecords("exec SelSDivByUserID @p1, 0", globalData.UserID);
                    foreach (DataRow row3 in dt3.Rows)
                    {
                        dt1 = sql1.GetRecords("exec SelRegDir @p1", row3[0]);
                        foreach (DataRow row in dt1.Rows)
                        {
                            tn.Nodes.Add(row[1].ToString());
                        }
                        foreach (TreeNode node in tn.Nodes)
                        {
                            node.Nodes.Add("Общий ассортиментный план");
                            node.Nodes.Add("Выполнение плана");
                            node.Nodes.Add("Маркетинговые мероприятия");
                            node.Nodes.Add("Общий ассортиментный план " + year);

                            for (int j = 0; j < 3; j++)
                            {
                                tn2 = node.Nodes[j];
                                tn2.Nodes.Add(row3[1].ToString());
                            }
                        }
                    }
                }

                tn = treeView1.Nodes[treeView1.Nodes.Count - 1];
                dt1 = sql1.GetRecords("exec SelRegDir ''");
                int k = 0;
                foreach (DataRow row in dt1.Rows)
                {
                    tn.Nodes.Add(row[1].ToString());
                    dt2 = sql1.GetRecords("exec SelRegByRD @p1", row[0].ToString());
                    tn2 = tn.Nodes[k];
                    foreach (DataRow row2 in dt2.Rows)
                    {
                        tn2.Nodes.Add(row2[1].ToString());
                    }
                    k++;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void регионыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegionForm regForm = new RegionForm();
            regForm.ShowDialog();
        }

        private void отчётыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Name cn = new Name("exec SelTypeReport", "exec InsTypeReport", "exec UpdTypeReport");
            cn.ShowDialog();
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;
                int i = 2;
                try
                {
                    Sql sql1 = new Sql();
                    while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                    {
                        String s1 = xlSh.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString();//number
                        String s2 = xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString();//name
                        String s3 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//stp
                        String s4 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//payer
                        String s5 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString(); //plant
                        String s6 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//dc

                        sql1.GetRecords("exec InsCustomer @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В строке " + i.ToString());
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.");
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.");
                        xlApp.Visible = true;
                    }
                }
            }
        }        

        private void materialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;
                int i = 2;
                try
                {
                    Sql sql1 = new Sql();
                    while (xlSh.get_Range("K" + i.ToString(), "K" + i.ToString()).Value2 != null)
                    {
                        String s1 = xlSh.get_Range("K" + i.ToString(), "K" + i.ToString()).Value2.ToString();//number
                        String s2 = xlSh.get_Range("L" + i.ToString(), "L" + i.ToString()).Value2.ToString();//name
                        String s3 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//sdiv
                        String s4 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//pdiv
                        String s5 = xlSh.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString(); //sba
                        if (s5.Count() == 1)
                            s5 = "0" + s5;
                        String s6 = xlSh.get_Range("G" + i.ToString(), "G" + i.ToString()).Value2.ToString();//mmg
                        if (s6.Count() == 1)
                            s6 = "0" + s6;
                        String s7 = xlSh.get_Range("I" + i.ToString(), "I" + i.ToString()).Value2.ToString();//msg
                        String s8 = xlSh.get_Range("M" + i.ToString(), "M" + i.ToString()).Value2.ToString();//nom_id
                        if (s8 == null)
                            s8 = "0";

                        sql1.GetRecords("exec InsMaterial @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8", s1, s2, s3, s4, s5, s6, s7, s8);

                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В строке " + i.ToString());
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.");
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.");
                        xlApp.Visible = true;
                    }
                }
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Не удалось удалить объект. " + ex.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            globalData.indexRow = 0;
            globalData.indexCol = 4;
            globalData.indexRow2 = 0;
            globalData.indexCol2 = 4;

            proc1 = "";
            proc2 = "";
            loadData();

            if (temptn != null)
                temptn.BackColor = Color.White;
            e.Node.BackColor = bbgreen3;
            temptn = e.Node;
        }

        private void loadData()
        {
            Cursor = Cursors.WaitCursor;
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            if (tn1 == null)
            {
                Cursor = Cursors.Default;
                return;
            }
            if (tn1.Text == "Справочник пользователей")
            {
                tabControl1.SelectedIndex = 11;
                tabControl1.Visible = true;
                globalData.Region = "";
                globalData.RD = "";
                loadUsersForIT();
                Cursor = Cursors.Default;
                return;
            }
            if (tn1.Text == "Отчёты дистрибьюторов")
            {
                dt1 = sql1.GetRecords("exec SelCustRepDist");

                globalData.load = false;
                cbCustRepDist.DataSource = dt1;
                cbCustRepDist.DisplayMember = "cust_name";
                cbCustRepDist.ValueMember = "cust_id";
                globalData.load = true;

                dt2 = sql1.GetRecords("exec SelSubRegRepDist");

                globalData.load = false;
                cbSubRegRepDist.DataSource = dt2;
                cbSubRegRepDist.DisplayMember = "subreg_nameRus";
                cbSubRegRepDist.ValueMember = "subreg_id";
                globalData.load = true;

                
                SelRepDist();
                Cursor = Cursors.Default;
                return;
            }
            if (tn1.Text == "Выполнение плана")
            {
                tabControl1.SelectedIndex = 22;
                tabControl1.Visible = true;

                cbSDivEvoRP.SelectedIndex = 0;
                fillRD(cbRDEvoRP);
                fillReg(cbRegEvoRP);
                fillUsersEvoRP(cbUsersEvoRP);

                Cursor = Cursors.Default;
                return;
            }
            if (tn1.Text == "Планировщик")
            {
                if (tn1.Parent.Parent != null)
                    globalData.RD = tn1.Parent.Parent.Text;
                else
                    globalData.RD = String.Empty;

                tabControl1.SelectedIndex = 25;
                tabControl1.Visible = true;

                if ((!globalData.update) || (cbUsersVisitPlanDays.Items.Count == 0))
                    loadUsersVisitPlan(cbUsersVisitPlanDays);

                globalData.load = true;

                loadVisitPlanDays();

                Cursor = Cursors.Default;
                return;
            }
            if (tn1.Text == "Отчёт")
            {
                if (tn1.Parent.Parent != null)
                    globalData.RD = tn1.Parent.Parent.Text;
                else
                    globalData.RD = String.Empty;

                tabControl1.SelectedIndex = 24;
                tabControl1.Visible = true;

                if ((!globalData.update) || (cbUsersVisitPlanReport.Items.Count == 0))
                    loadUsersVisitPlan(cbUsersVisitPlanReport);

                globalData.load = true;

                loadVisitPlanReport();

                Cursor = Cursors.Default;
                return;
            }

            if (tn1.Parent == null)
            {
                Cursor = Cursors.Default;
                return;
            }

            if (tn1.Parent.Text == "Маркетинг 1")
            {
                if (tn1.Text == "Заполнение")
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage23"];
                    tabControl1.Visible = true;
                    _dgvRent.Columns.Clear();
                    _dgvRent.Rows.Clear();
                    loadRent(false);
                }
                if (tn1.Text == "Отчёт для РД")
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage23"];
                    tabControl1.Visible = true;
                    _dgvRent.Columns.Clear();
                    _dgvRent.Rows.Clear();
                    loadRent(true);
                }
                if (tn1.Text == "Итоговый отчёт")
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage24"];
                    tabControl1.Visible = true;
                    _dgvRentRD.Columns.Clear();
                    _dgvRentRD.Rows.Clear();
                    loadSvodRent(false);
                }
                if (tn1.Text == "Сводный отчёт")
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage24"];
                    tabControl1.Visible = true;
                    _dgvRentRD.Columns.Clear();
                    _dgvRentRD.Rows.Clear();
                    loadSvodRent(true);
                }
                if (tn1.Text == "Отчёт для контроллинга")
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage26"];
                    tabControl1.Visible = true;
                    _dgvStorno.Columns.Clear();
                    _dgvStorno.Rows.Clear();
                    loadStorno(false);
                }
                if (tn1.Text == "Сторно")
                {
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage26"];
                    tabControl1.Visible = true;
                    _dgvStorno.Columns.Clear();
                    _dgvStorno.Rows.Clear();
                    loadStorno(true);
                }
                                  
            }

            if (tn1.Text == "Маркетинг 2")
            {
                tabControl1.SelectedTab = tabControl1.TabPages["tabPage25"];
                tabControl1.Visible = true;

                globalData.Div = "HC";
                globalData.Region = String.Empty;
                globalData.RD = String.Empty;

                loadAccessPM();
                loadPM();
                loadRegPM();
                loadMAType(cbTypeMAPM);

                SelMAPM();
            }

            if (tn1.Parent.Text == "Справочник пользователей")
            {
                tabControl1.SelectedIndex = 11;
                tabControl1.Visible = true;
                globalData.RD = tn1.Text;
                globalData.Region = "";
                loadUsersForIT();
                Cursor = Cursors.Default;
                return;
            }
            if (tn1.Parent.Text == "Проверка")
            {
                if (tn1.Text == "Отчёт из SAP")
                {
                    SelAllReport();
                    tabControl1.SelectedIndex = 13;
                    tabControl1.Visible = true;
                }
                if (tn1.Text == "Проверка косвенных продаж" || tn1.Text == "Проверка косвенных по номенклатуре" || tn1.Text == "Проверка косвенных по регионам")
                {
                    if (tn1.Text == "Проверка косвенных по регионам")
                        KosReg = true;
                    else
                        KosReg = false;

                    tabControl1.SelectedIndex = 20;
                    tabControl1.Visible = true;

                    checkKosReport();
                }
                if (tn1.Text == "Ассортиментные планы на год")
                {
                    tabControl1.SelectedTab = tabPage27;
                    tabControl1.Visible = true;
                    cbYearPlan.SelectedItem = globalData.year;
                    SelComparePlan();
                }
                if (tn1.Text == "По отчётам дистрибьюторов")
                {
                    tabControl1.SelectedIndex = 21;
                    tabControl1.Visible = true;

                    checkDistReport();
                }
            }

            switch (tn1.Parent.Text)
            {
                case "Отчёты по компаниям":
                    {
                        if (curbtn == 0)
                            curbtn = 6;


                        if (tn1.Text == "Динамика продаж по Кардиологии")
                            dt1 = sql1.GetRecords("exec SelDynByComp @p1, @p2, @p3", dateTimePicker15.Value.Year, curbtn, "kard");
                        else
                        {
                            dt1 = sql1.GetRecords("exec SelDynByComp @p1, @p2, @p3", dateTimePicker15.Value.Year, curbtn, "shov");
                            if (curbtn == 5)
                                curbtn--;
                        }

                        SetButtonColorDyn(curbtn);

                        if (dt1 == null)
                        {
                            MessageBox.Show("Не удалось получить данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        _dgv2.DataSource = dt1;
                        tabControl1.Visible = true;
                        tabControl1.SelectedIndex = 1;

                        for (int i = 1; i < 14; i++)
                        {
                            _dgv2.Columns[i].DefaultCellStyle.Format = "N2";
                            _dgv2.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }

                        break;
                    }
            }

            if (tn1.Parent.Parent == null)
            {
                Cursor = Cursors.Default;
                return;
            }
            else if (tn1.Parent.Parent.Text == "Справочник пользователей")
            {
                tabControl1.SelectedIndex = 11;
                tabControl1.Visible = true;
                globalData.Region = tn1.Text;
                globalData.RD = tn1.Parent.Text;
                loadUsersForIT();
                Cursor = Cursors.Default;
                return;
            }
            else if (tn1.Text == "Выполнение плана по России")
            {
                tabControl1.SelectedIndex = 9;
                tabControl1.Visible = true;

                fillRegions("RF", cbRegEvo);
                selEvo(_dgv10, "RF", dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), "0"/*cbRegEvo.SelectedValue.ToString()*/, cbRegEvo.SelectedValue.ToString(), "");               
                
                Cursor = Cursors.Default;
                return;
            }
            else if (tn1.Parent.Parent.Text == "Проверка")
            {
                globalData.Div = tn1.Text;

                if (tn1.Parent.Text == "Ассортиментные планы")
                {
                    tabControl1.SelectedIndex = 12;
                    tabControl1.Visible = true;
                    selPSAcc();
                }
                else if (tn1.Parent.Text == "Косвенные продажи")
                {
                    tabControl1.SelectedIndex = 15;
                    tabControl1.Visible = true;
                    fillRegions("", cbRegAllKos);
                    fillUsersAllKos();
                }
                else if (tn1.Parent.Text == "Сравнение продаж")
                {
                    tabControl1.SelectedIndex = 17;
                    tabControl1.Visible = true;
                    fillRegions("", cbRegCheck);
                }
                Cursor = Cursors.Default;
                return;
            }

            DataTable dtNY = new DataTable();
            dtNY = sql1.GetRecords("exec GetSettings");

            string year = dtNY.Rows[0].ItemArray[3].ToString();

            switch (tn1.Text)
            {
                case "HC":
                    {
                        fillUsers();

                        #region Анализ продаж HC

                        if (tn1.Parent.Text == "Анализ продаж")
                        {
                            SelectAP();
                        }
                        #endregion

                        #region Динамика продаж HC
                        if (tn1.Parent.Text == "Динамика продаж")
                        {
                            button20.Visible = false;
                            label14.Visible = false;
                            cbRegions.Visible = false;
                            SelDyn();
                        }
                        #endregion

                        #region Ассортиментный план HC

                        if (tn1.Parent.Text == "Ассортиментный план")
                        {
                            TreeNode tn2 = new TreeNode();
                            tn2 = treeView1.SelectedNode.Parent.Parent.Parent;

                            if (tn2.Text == "Мои регионы")
                                btnSaveAcc.Enabled = true;
                            else
                                btnSaveAcc.Enabled = false;

                            rbEuro.Visible = true;
                            rbRub.Visible = true;

                            fillLPU(cbLPU, cbUsers2.SelectedValue.ToString(), globalData.Region, cbYearAcc.SelectedItem.ToString());
                                        
                            SelRegAcc();
                        }
                        #endregion

                        #region Косвенные продажи HC
                        if (tn1.Parent.Text == "Косвенные продажи")
                        {
                            SelKosReport();
                        }
                        #endregion

                        #region Маркетинговые мероприятия HC
                        if (tn1.Parent.Text == "Маркетинговые мероприятия")
                        {
                            ma_old = ma_new;

                            if (tn1.Parent.Parent.Text != globalData.Region)
                            {
                                globalData.RD = tn1.Parent.Parent.Text;
                                globalData.Region = String.Empty;
                                fillRegions(globalData.RD, cbRegMA);
                                fillUsersMA();
                                fillLPUMA();
                                ma_new = 1;
                            }
                            else
                            {
                                globalData.RD = String.Empty;
                                ma_new = 2;
                            }
                            SetVisMA();
                        }
                        #endregion

                        #region Ассортиментный план HC Новый год
                        if (tn1.Parent.Text == "Ассортиментный план " + globalData.year)
                        {
                            SelAccNY();
                        }
                        #endregion

                        #region Динамика продаж HC
                        if (tn1.Parent.Text == "Общая динамика продаж")
                        {
                            button20.Visible = true;
                            button20.Text = "Общая";
                            label14.Visible = true;
                            cbRegions.Visible = true;
                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegions);
                            if (cbRegions.SelectedValue != null)
                                loadDyn(cbRegions.SelectedValue.ToString());
                            else
                                MessageBox.Show("не найдены регионы для построения динамики продаж", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion

                        #region Общий ассортиментный план HC
                        if (tn1.Parent.Text == "Общий ассортиментный план")
                        {
                            tabControl1.SelectedIndex = 6;
                            tabControl1.Visible = true;

                            radioButton11.Visible = true;
                            radioButton10.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegAcc);
                                                        
                            string reg = "0";
                            if (cbRegAcc.Visible)
                                reg = cbRegAcc.SelectedValue.ToString();
                            fillUsersAcc(cbUsersAcc, reg, cbYearAccAll.SelectedItem.ToString());

                            string user = "0";
                            if (cbUsersAcc.Visible)
                                user = cbUsersAcc.SelectedValue.ToString();
                            
                            if (cbUsersAcc.Items.Count > 0)
                                fillLPU(cbLPUAcc, user, reg, cbYearAccAll.SelectedItem.ToString());
                        }
                        #endregion

                        #region Выполнение плана HC
                        if (tn1.Parent.Text == "Выполнение плана")
                        {
                            tabControl1.SelectedIndex = 9;
                            tabControl1.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegEvo);
                            fillUsersAcc(cbUsersEvo, cbRegEvo.SelectedValue.ToString(), cbYearEvo.SelectedItem.ToString());

                            string user = "0";
                            if (cbUsersEvo.Visible)
                                user = cbUsersEvo.SelectedValue.ToString();
                            selEvo(_dgv10, globalData.Div, dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), cbRegEvo.SelectedValue.ToString(), globalData.RD, user);
                        }
                        #endregion
                        
                        #region Ассортиментный план HC Новый год
                        if (tn1.Parent.Text == "Общий ассортиментный план " + globalData.year)
                        {
                            globalData.acc_history = false;

                            tabControl1.SelectedIndex = 16;
                            tabControl1.Visible = true;
                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegAllAccNY);

                            string reg = "0";
                            
                            reg = cbRegAllAccNY.SelectedValue.ToString();

                            fillUsersAcc(cbUsersAllAccNY, reg, year);

                            string user = "0";
                            if (cbUsersAllAccNY.Visible)
                                user = cbUsersAllAccNY.SelectedValue.ToString();
                            
                            fillLPU(cbLPUAllAccNY, user, reg, globalData.year);
                        }
                        #endregion

                        #region Ассортиментный план HC Новый год с историей по ЛПУ
                        if (tn1.Parent.Text == "Общий асс. план " + globalData.year + "(с историей по ЛПУ)")
                        {
                            globalData.acc_history = true;

                            tabControl1.SelectedIndex = 16;
                            tabControl1.Visible = true;
                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegAllAccNY);

                            string reg = "0";

                            reg = cbRegAllAccNY.SelectedValue.ToString();

                            fillUsersAcc(cbUsersAllAccNY, reg, year);

                            string user = "0";
                            if (cbUsersAllAccNY.Visible)
                                user = cbUsersAllAccNY.SelectedValue.ToString();

                            fillLPU(cbLPUAllAccNY, user, reg, globalData.year);
                        }
                        #endregion

                        break;
                    }
                case "AE":
                    {
                        fillUsers();

                        #region Анализ продаж AE

                        if (tn1.Parent.Text == "Анализ продаж")
                        {
                            SelectAP();
                        }
                        #endregion

                        #region Ассортиментный план AE

                        if (tn1.Parent.Text == "Ассортиментный план")
                        {
                            fillLPU(cbLPU, cbUsers2.SelectedValue.ToString(), globalData.Region, cbYearAcc.SelectedItem.ToString());

                            rbEuro.Visible = false;
                            rbRub.Visible  = false;


                            SelRegAcc();
                        }

                        #endregion

                        #region Динамика продаж AE
                        if (tn1.Parent.Text == "Динамика продаж")
                        {
                            button20.Visible = false;
                            label14.Visible = false;
                            cbRegions.Visible = false;
                            SelDyn();
                        }
                        #endregion

                        #region Косвенные продажи AE
                        if (tn1.Parent.Text == "Косвенные продажи")
                        {
                            SelKosReport();
                        }
                        #endregion

                        #region Маркетинговые мероприятия AE
                        if (tn1.Parent.Text == "Маркетинговые мероприятия")
                        {
                            ma_old = ma_new;

                            if (tn1.Parent.Parent.Text != globalData.Region)
                            {
                                globalData.RD = tn1.Parent.Parent.Text;
                                globalData.Region = String.Empty;
                                fillRegions(globalData.RD, cbRegMA);
                                fillUsersMA();
                                ma_new = 1;
                            }
                            else
                            {
                                globalData.RD = String.Empty;
                                ma_new = 2;
                            }

                            SetVisMA();
                        }
                        #endregion

                        #region Ассортиментный план AE Новый год
                        if (tn1.Parent.Text == "Ассортиментный план " + globalData.year)
                        {
                            SelAccNY();
                        }
                        #endregion

                        #region Динамика продаж AE
                        if (tn1.Parent.Text == "Общая динамика продаж")
                        {
                            button20.Text = "Общая";
                            button20.Visible = true;
                            label14.Visible = true;
                            cbRegions.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegions);
                            if (cbRegions.SelectedValue != null)
                                loadDyn(cbRegions.SelectedValue.ToString());
                            else
                                MessageBox.Show("не найдены регионы для построения динамики продаж", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion

                        #region Общий ассортиментный план AE
                        if (tn1.Parent.Text == "Общий ассортиментный план")
                        {
                            tabControl1.SelectedIndex = 6;
                            tabControl1.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;

                            radioButton11.Visible = false;
                            radioButton10.Visible = false;


                            fillRegions(globalData.RD, cbRegAcc);

                            string reg = "0";
                            if (cbRegAcc.Visible)
                                reg = cbRegAcc.SelectedValue.ToString();

                            fillUsersAcc(cbUsersAcc, reg, cbYearAccAll.SelectedItem.ToString());
                            string user = "0";
                            if (cbUsersAcc.Visible)
                                user = cbUsersAcc.SelectedValue.ToString();
                                                        
                            if (cbUsersAcc.Items.Count > 0)
                                fillLPU(cbLPUAcc, user, reg, cbYearAccAll.SelectedItem.ToString());
                        }
                        #endregion
                        
                        #region Выполнение плана AE
                        if (tn1.Parent.Text == "Выполнение плана")
                        {
                            tabControl1.SelectedIndex = 9;
                            tabControl1.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;

                            string user = "0";
                            if (cbUsersEvo.Visible)
                                user = cbUsersEvo.SelectedValue.ToString();
                            selEvo(_dgv10, globalData.Div, dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), "0", globalData.RD, user);
                        }
                        #endregion

                        #region Ассортиментный план АЕ Новый год

                        if (tn1.Parent.Text == "Общий ассортиментный план " + globalData.year)
                        {
                            globalData.acc_history = false;

                            tabControl1.SelectedIndex = 16;
                            tabControl1.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegAllAccNY);

                            string user = "0", reg = "0";
                            if (cbRegAllAccNY.Visible)
                                reg = cbRegAllAccNY.SelectedValue.ToString();
                            
                            fillUsersAcc(cbUsersAllAccNY, reg, year);
                            if (cbUsersAllAccNY.Visible)
                                user = cbUsersAllAccNY.SelectedValue.ToString();                            
                            
                            fillLPU(cbLPUAllAccNY, user, reg, globalData.year);
                        }
                        #endregion

                        #region Ассортиментный план АЕ Новый год

                        if (tn1.Parent.Text == "Общий асс. план " + globalData.year + "(с историей по ЛПУ)")
                        {
                            globalData.acc_history = true;

                            tabControl1.SelectedIndex = 16;
                            tabControl1.Visible = true;

                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegAllAccNY);

                            string user = "0", reg = "0";
                            if (cbRegAllAccNY.Visible)
                                reg = cbRegAllAccNY.SelectedValue.ToString();

                            fillUsersAcc(cbUsersAllAccNY, reg, year);
                            if (cbUsersAllAccNY.Visible)
                                user = cbUsersAllAccNY.SelectedValue.ToString();

                            fillLPU(cbLPUAllAccNY, user, reg, globalData.year);
                        }
                        #endregion

                        break;
                    }
                case "OM":
                    {
                        fillUsers();

                        #region Анализ продаж OM

                        if (tn1.Parent.Text == "Анализ продаж")
                        {
                            SelectAP();
                        }
                        #endregion

                        #region Динамика продаж OM
                        if (tn1.Parent.Text == "Динамика продаж")
                        {
                            button20.Visible = false;
                            label14.Visible = false;
                            cbRegions.Visible = false;
                            SelDyn();
                        }
                        #endregion

                        #region Косвенные продажи OM

                        if (tn1.Parent.Text == "Косвенные продажи")
                        {
                            SelKosReport();
                        }
                        #endregion

                        #region Динамика продаж OM
                        if (tn1.Parent.Text == "Общая динамика продаж")
                        {
                            button20.Text = "Общая";
                            button20.Visible = true;
                            label14.Visible = true;
                            cbRegions.Visible = true;
                            globalData.RD = tn1.Parent.Parent.Text;
                            fillRegions(globalData.RD, cbRegions);
                            if (cbRegions.SelectedValue != null)
                                loadDyn(cbRegions.SelectedValue.ToString());
                            else
                                MessageBox.Show("не найдены регионы для построения динамики продаж", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion

                        break;
                    }
                case "Общая динамика продаж":
                    {
                        switch (tn1.Parent.Text)
                        {
                            case "HC":
                                {
                                    button20.Visible = true;

                                    globalData.RD = String.Empty;
                                    globalData.Div = tn1.Parent.Text;
                                    tabControl1.SelectedIndex = 4;
                                    tabControl1.Visible = true;

                                    treeView1.Focus();

                                    label14.Visible = true;
                                    cbRegions.Visible = true;

                                    fillRegions(String.Empty, cbRegions);

                                    loadDyn(cbRegions.SelectedValue.ToString());

                                    break;
                                }
                            case "AE":
                                {
                                    button20.Visible = true;

                                    globalData.RD = String.Empty;
                                    globalData.Div = tn1.Parent.Text;
                                    tabControl1.SelectedIndex = 4;
                                    tabControl1.Visible = true;

                                    treeView1.Focus();

                                    label14.Visible = true;
                                    cbRegions.Visible = true;

                                    dt1 = sql1.GetRecords("exec Region_Select");
                                    load = false;
                                    cbRegions.DataSource = dt1;
                                    cbRegions.DisplayMember = "reg_nameRus";
                                    cbRegions.ValueMember = "reg_id";
                                    load = true;

                                    loadDyn(cbRegions.SelectedValue.ToString());

                                    break;
                                }
                            case "OM":
                                {
                                    button20.Visible = true;

                                    globalData.RD = String.Empty;
                                    globalData.Div = tn1.Parent.Text;
                                    tabControl1.SelectedIndex = 4;
                                    tabControl1.Visible = true;

                                    treeView1.Focus();

                                    label14.Visible = true;
                                    cbRegions.Visible = true;

                                    dt1 = sql1.GetRecords("exec Region_Select");
                                    load = false;
                                    cbRegions.DataSource = dt1;
                                    cbRegions.DisplayMember = "reg_nameRus";
                                    cbRegions.ValueMember = "reg_id";
                                    load = true;

                                    loadDyn(cbRegions.SelectedValue.ToString());

                                    break;
                                }

                        }

                        break;
                    }

                case "Общий ассортиментный план":
                    {
                        globalData.Div = tn1.Parent.Text;

                        if ((globalData.Div == "HC") || (globalData.Div == "AE"))
                        {
                            tabControl1.SelectedIndex = 6;
                            tabControl1.Visible = true;


                            globalData.RD = String.Empty;

                            fillRegions(String.Empty, cbRegAcc);

                            string reg = "0";
                            if (cbRegAcc.Visible)
                                reg = cbRegAcc.SelectedValue.ToString();

                            fillUsersAcc(cbUsersAcc, reg, cbYearAccAll.SelectedItem.ToString());
                            globalData.RD = String.Empty;

                            string user = "0";
                            if ((cbUsersAcc.Visible) && (cbUsersAcc.Items.Count > 0))
                                user = cbUsersAcc.SelectedValue.ToString();

                            switch (tn1.Parent.Text)
                            {
                                case "HC":
                                    {
                                        radioButton11.Visible = true;
                                        radioButton10.Visible = true;

                                        fillLPU(cbLPUAcc, user, reg, globalData.CurDate.Year.ToString());
                                        break;
                                    }
                                case "AE":
                                    {
                                        radioButton11.Visible = false;
                                        radioButton10.Visible = false;

                                        fillLPU(cbLPUAcc, user, reg, globalData.CurDate.Year.ToString());
                                        break;
                                    }
                            }
                        }
                        break;
                    }
                case "Личные продажи (список)":
                    {
                        switch (tn1.Parent.Text)
                        {
                            case "HC":
                                {
                                    globalData.Div = "HC";
                                    btnBasic.BackColor = Button.DefaultBackColor;
                                    btnBasic.Visible = true;
                                    fillRegions("", cbRegionAllPrivSales);
                                    selAllPrivSales();
                                    break;
                                }
                            case "AE":
                                {
                                    globalData.Div = "AE";
                                    btnBasic.Visible = false;
                                    fillRegions("", cbRegionAllPrivSales);
                                    selAllPrivSales();
                                    break;
                                }
                            case "OM":
                                {
                                    globalData.Div = "OM";
                                    btnBasic.Visible = false;
                                    fillRegions("", cbRegionAllPrivSales);
                                    selAllPrivSales();
                                    break;
                                }
                        }
                        break;
                    }
                case "Общий маркетинговый план":
                    {
                        globalData.Div = tn1.Parent.Text;
                        globalData.Region = String.Empty;
                        globalData.RD = String.Empty;

                        fillRegions(String.Empty, cbRegMA);
                        fillUsersMA();
                        fillLPUMA();

                        ma_old = ma_new;
                        ma_new = 3;

                        SetVisMA();
                        break;
                    }
                default:
                    {
                        if ((tn1.Text == "Общий ассортиментный план " + year) && ((tn1.Parent.Text == "HC") || (tn1.Parent.Text == "AE")))
                        {
                            tabControl1.SelectedIndex = 16;
                            tabControl1.Visible = true;

                            globalData.Div = tn1.Parent.Text;
                            globalData.Region = String.Empty;
                            globalData.RD = String.Empty;

                            fillRegions(String.Empty, cbRegAllAccNY);

                            string reg = "0";
                            if (cbRegAllAccNY.Visible)
                                reg = cbRegAllAccNY.SelectedValue.ToString();

                            fillUsersAcc(cbUsersAllAccNY, cbRegAllAccNY.SelectedValue.ToString(), year);

                            string user = "0";
                            if (cbUsersAllAccNY.Visible)
                                user = cbUsersAllAccNY.SelectedValue.ToString();

                            fillLPU(cbLPUAllAccNY, user, reg, globalData.year);
                        }
                        break;
                    }
            }
            Cursor = Cursors.Default;
        }

        private void fillTableHeader(String type, DataGridView dgv)
        {
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            switch (type)
            {
                case "AP":
                    {
                        tabControl1.SelectedIndex = 0;
                        tabControl1.Visible = true;

                        if (dgv.ColumnCount == 0)
                        {
                            dgv.Columns.Clear();
                            dgv.Columns.Add("rep_id", "report");
                            dgv.Columns.Add("db_id_rep", "db_id_rep");
                            dgv.Columns.Add("db_id_ps", "db_id_ps");
                            dgv.Columns.Add("ps_id", "ps_id");
                            if (globalData.UserID == 1 || globalData.UserID == 258)
                            {
                                dgv.Columns["rep_id"].Visible = true;
                                dgv.Columns["db_id_rep"].Visible = true;
                                dgv.Columns["db_id_ps"].Visible = true;
                                dgv.Columns["ps_id"].Visible = true;
                            }
                            else
                            {
                                dgv.Columns["rep_id"].Visible = false;
                                dgv.Columns["db_id_rep"].Visible = false;
                                dgv.Columns["db_id_ps"].Visible = false;
                                dgv.Columns["ps_id"].Visible = false;
                            }
                            dgv.Columns.Add("date", "Дата");
                            dgv.Columns["date"].DefaultCellStyle.Format = "MM - yyyy";
                            dgv.Columns["date"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["date"].ReadOnly = true;
                            dgv.Columns["date"].Width = 60;
                            dgv.Columns.Add("comp", "Компания");
                            dgv.Columns["comp"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["comp"].ReadOnly = true;
                            dgv.Columns.Add("reg", "Регион");
                            dgv.Columns["reg"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["reg"].ReadOnly = true;
                            dgv.Columns["reg"].Width = 150;

                            if (radioButton4.Checked)
                                dgv.Columns["reg"].HeaderText = "Пришло из региона";
                            else
                                dgv.Columns["reg"].HeaderText = "Регион продаж";

                            dgv.Columns.Add("subreg", "Субрегион");
                            dgv.Columns["subreg"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["subreg"].ReadOnly = true;
                            dgv.Columns["subreg"].Width = 150;

                            dgv.Columns.Add("refDoc", "Счёт-фактура");
                            dgv.Columns["refDoc"].ReadOnly = true;
                            dgv.Columns["refDoc"].Visible = true;
                            dgv.Columns["refDoc"].SortMode = DataGridViewColumnSortMode.Programmatic;

                            dgv.Columns.Add("cust_code", "Покупатель");
                            dgv.Columns["cust_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["cust_code"].ReadOnly = true;
                            dgv.Columns.Add("cust_name", "Покупатель (наименование)");
                            dgv.Columns["cust_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["cust_name"].ReadOnly = true;
                            dgv.Columns["cust_name"].Width = 200;
                            dgv.Columns.Add("sba_code", "SBA");
                            dgv.Columns["sba_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["sba_code"].ReadOnly = true;
                            dgv.Columns["sba_code"].Width = 40;
                            dgv.Columns.Add("sba_name", "SBA (наименование)");
                            dgv.Columns["sba_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["sba_name"].ReadOnly = true;
                            dgv.Columns["sba_name"].Width = 140;
                            dgv.Columns.Add("mmg_code", "MMG (код)");
                            dgv.Columns["mmg_code"].Visible = false;
                            dgv.Columns.Add("mmg_name", "MMG (наименование)");
                            dgv.Columns["mmg_name"].Visible = false;
                            dgv.Columns.Add("msg_code", "MSG (код)");
                            dgv.Columns["msg_code"].Visible = false;
                            dgv.Columns.Add("msg_name", "MSG (наименование)");
                            dgv.Columns["msg_name"].Visible = false;
                            dgv.Columns.Add("mat_code", "Артикул товара");
                            dgv.Columns["mat_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["mat_code"].ReadOnly = true;
                            dgv.Columns["mat_code"].Width = 120;
                            dgv.Columns.Add("mat_name", "Наименование товара");
                            dgv.Columns["mat_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["mat_name"].ReadOnly = true;
                            dgv.Columns["mat_name"].Width = 150;
                            dgv.Columns.Add("sr", "Продажи без НДС (евро)");
                            dgv.Columns["sr"].DefaultCellStyle.Format = "N2";
                            dgv.Columns["sr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns["sr"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["sr"].ReadOnly = true;
                            dgv.Columns["sr"].Width = 130;
                            dgv.Columns.Add("srRub", "Продажи без НДС (руб.)");
                            dgv.Columns["srRub"].DefaultCellStyle.Format = "N2";
                            dgv.Columns["srRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns["srRub"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["srRub"].ReadOnly = true;
                            dgv.Columns["srRub"].Width = 130;
                            dgv.Columns.Add("bum", "Кол-во (шт)");
                            //dgv.Columns["bum"].DefaultCellStyle.Format = "N0";
                            dgv.Columns["bum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns["bum"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["bum"].ReadOnly = true;
                            dgv.Columns["bum"].Width = 90;

                            if (radioButton6.Checked)
                            {
                                dgv.Columns.Add("user_name", "Пользователь");
                                dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                                dgv.Columns.Add("move_date", "Дата и время перемещения");
                                dgv.Columns["move_date"].Width = 130;
                                dgv.Columns["move_date"].SortMode = DataGridViewColumnSortMode.Programmatic;

                                break;
                            }

                            dgv.Columns.Add("UnitPrice", "Цена за единицу (евро)");
                            dgv.Columns["UnitPrice"].Visible = false;
                            dgv.Columns.Add("UnitPriceRub", "Цена за единицу (руб.)");
                            dgv.Columns["UnitPriceRub"].Visible = false;
                            dgv.Columns.Add("fact", "Факт. продажи (шт)");
                            dgv.Columns["fact"].Width = 110;
                            dgv.Columns["fact"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns["fact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns.Add("sumSale", "Сумма продаж (евро)");
                            dgv.Columns["sumSale"].ReadOnly = true;
                            dgv.Columns["sumSale"].DefaultCellStyle.Format = "N2";
                            dgv.Columns["sumSale"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns["sumSale"].Width = 110;
                            dgv.Columns["sumSale"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            dgv.Columns.Add("sumSaleRub", "Сумма продаж (руб.)");
                            dgv.Columns["sumSaleRub"].ReadOnly = true;
                            dgv.Columns["sumSaleRub"].DefaultCellStyle.Format = "N2";
                            dgv.Columns["sumSaleRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgv.Columns["sumSaleRub"].Width = 120;
                            dgv.Columns["sumSaleRub"].SortMode = DataGridViewColumnSortMode.Programmatic;

                            DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();

                            Sql sql1 = new Sql();
                            DataTable dt1 = new DataTable();
                            
                            dt1 = sql1.GetRecords("exec selLPU @p1, @p2, @p3, @p4", cbUsers.SelectedValue, globalData.Region, globalData.Div, globalData.CurDate.Year);

                            if (dt1 != null)
                            {
                                foreach (DataRow row in dt1.Rows)
                                    comboCol.Items.Add(row.ItemArray[1]);

                                comboCol.Name = "lpu";


                                dgv.Columns.Insert(27, comboCol);
                                dgv.Columns[27].Width = 100;
                                dgv.Columns[27].HeaderText = "ЛПУ";
                                dgv.Columns["lpu"].SortMode = DataGridViewColumnSortMode.Programmatic;

                                dgv.Columns.Add("count", "Количество");
                                dgv.Columns["count"].Visible = false;

                                DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
                                checkCol.DataPropertyName = "colcheck";
                                checkCol.Name = "check";
                                dgv.Columns.Insert(29, checkCol);
                                dgv.Columns["check"].HeaderText = "Ушло";
                                dgv.Columns["check"].Width = 50;
                                dgv.Columns["check"].SortMode = DataGridViewColumnSortMode.Programmatic;
                                dgv.Columns["check"].Visible = false;
                                dgv.Columns.Add("upd", "upd");
                                if (globalData.UserID == 1)
                                    dgv.Columns["upd"].Visible = true;
                                else
                                    dgv.Columns["upd"].Visible = false;

                                dgv.Columns.Add("user_name", "Пользователь");
                                dgv.Columns["user_name"].Visible = false;
                                dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
                            }
                            else
                            {
                                MessageBox.Show("Не найдены ЛПУ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            if (radioButton6.Checked)
                                break;

                            if (radioButton4.Checked)
                                dgv.Columns["reg"].HeaderText = "Пришло из региона";
                            else
                                dgv.Columns["reg"].HeaderText = "Регион продаж";

                            DataGridViewComboBoxColumn comboCol = new DataGridViewComboBoxColumn();

                            if (rb == 5)
                            {
                                Sql sql1 = new Sql();
                                DataTable dt1 = new DataTable();
                                if (cbUsers.Visible == false)
                                    dt1 = sql1.GetRecords("exec selLPU 0, @p1, @p2, @p3", globalData.Region, globalData.Div, globalData.CurDate.Year);
                                else
                                    dt1 = sql1.GetRecords("exec selLPU @p1, @p2, @p3, @p4", cbUsers.SelectedValue, globalData.Region, globalData.Div, globalData.CurDate.Year);

                                if (dt1 != null)
                                {
                                    foreach (DataRow row in dt1.Rows)
                                        comboCol.Items.Add(row.ItemArray[1]);

                                    int temp = dgv.Columns[27].Width;

                                    dgv.Columns.RemoveAt(27);

                                    comboCol.Name = "lpu";

                                    dgv.Columns.Insert(27, comboCol);

                                    dgv.Columns["lpu"].Width = temp;
                                    dgv.Columns["lpu"].HeaderText = "ЛПУ";
                                    dgv.Columns["lpu"].SortMode = DataGridViewColumnSortMode.Programmatic;
                                }
                                else
                                {
                                    MessageBox.Show("Не найдены ЛПУ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            if (!cbUsers.Visible)
                                dgv.Columns["user_name"].Visible = true;
                            else
                                dgv.Columns["user_name"].Visible = false;
                        }


                        if (rb == 5)
                        {
                            dgv.Columns["fact"].Visible = true;
                            dgv.Columns["sumSale"].Visible = true;
                            dgv.Columns["sumSaleRub"].Visible = true;
                            if (globalData.Div == "OM")
                                dgv.Columns["lpu"].Visible = false;
                            else
                                dgv.Columns["lpu"].Visible = true;
                            dgv.Columns["check"].Visible = false;//!chbOnlyPS.Checked;

                            dgv.Columns["srRub"].Visible = false;
                        }
                        else if (rb == 3)
                        {
                            dgv.Columns["fact"].Visible = false;
                            dgv.Columns["sumSale"].Visible = false;
                            dgv.Columns["sumSaleRub"].Visible = false;
                            dgv.Columns["lpu"].Visible = false;
                            dgv.Columns["check"].Visible = false; //!chbOnlyPS.Checked;
                        }
                        else
                        {
                            dgv.Columns["fact"].Visible = false;
                            dgv.Columns["sumSale"].Visible = false;
                            dgv.Columns["sumSaleRub"].Visible = false;
                            dgv.Columns["lpu"].Visible = false;
                            dgv.Columns["check"].Visible = false;
                            dgv.Columns["srRub"].Visible = true;
                        }
                        break;
                    }
                case "All":
                    {
                        dgv.Columns.Clear();

                        dgv.Columns.Add("rep_id", "report");
                        dgv.Columns.Add("db_id", "db_id");
                        if (globalData.UserID == 1)
                        {
                            dgv.Columns["rep_id"].Visible = true;
                            dgv.Columns["db_id"].Visible = true;
                        }
                        else
                        {
                            dgv.Columns["rep_id"].Visible = false;
                            dgv.Columns["db_id"].Visible = false;
                        }

                        dgv.Columns.Add("comp_name", "Company");
                        dgv.Columns["comp_name"].ReadOnly = true;
                        dgv.Columns["comp_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgv.Columns["comp_name"].Width = 50;
                        dgv.Columns["comp_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("reg_nameRus", "Регион");
                        dgv.Columns["reg_nameRus"].ReadOnly = true;
                        dgv.Columns["reg_nameRus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgv.Columns["reg_nameRus"].Width = 100;
                        dgv.Columns["reg_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("subreg_nameRus", "Субрегион");
                        dgv.Columns["subreg_nameRus"].ReadOnly = true;
                        dgv.Columns["subreg_nameRus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgv.Columns["subreg_nameRus"].Width = 150;
                        dgv.Columns["subreg_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_SalesOrg", "Sales Organization");
                        dgv.Columns["rep_SalesOrg"].ReadOnly = true;
                        dgv.Columns["rep_SalesOrg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgv.Columns["rep_SalesOrg"].Width = 50;
                        dgv.Columns["rep_SalesOrg"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("cust_DistChan", "Distribution Channel");
                        dgv.Columns["cust_DistChan"].ReadOnly = true;
                        dgv.Columns["cust_DistChan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["cust_DistChan"].Width = 50;
                        dgv.Columns["cust_DistChan"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("cust_payer", "Payer");
                        dgv.Columns["cust_payer"].ReadOnly = true;
                        dgv.Columns["cust_payer"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["cust_payer"].Width = 70;
                        dgv.Columns["cust_payer"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("cust_ShipTo", "Ship-to party");
                        dgv.Columns["cust_ShipTo"].ReadOnly = true;
                        dgv.Columns["cust_ShipTo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["cust_ShipTo"].Width = 70;
                        dgv.Columns["cust_ShipTo"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("cust_plant", "Plant");
                        dgv.Columns["cust_plant"].ReadOnly = true;
                        dgv.Columns["cust_plant"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["cust_plant"].Width = 50;
                        dgv.Columns["cust_plant"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("cust_code", "Customer");
                        dgv.Columns["cust_code"].ReadOnly = true;
                        dgv.Columns["cust_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgv.Columns["cust_code"].Width = 70;
                        dgv.Columns["cust_code"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("cust_name", "Customer Name");
                        dgv.Columns["cust_name"].ReadOnly = true;
                        dgv.Columns["cust_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        dgv.Columns["cust_name"].Width = 70;
                        dgv.Columns["cust_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_RefDoc", "Reference document");
                        dgv.Columns["rep_RefDoc"].ReadOnly = true;
                        dgv.Columns["rep_RefDoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_RefDoc"].Width = 70;
                        dgv.Columns["rep_RefDoc"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_date", "Period/year");
                        dgv.Columns["rep_date"].ReadOnly = true;
                        dgv.Columns["rep_date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_date"].Width = 70;
                        dgv.Columns["rep_date"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_date"].DefaultCellStyle.Format = "MM - yyyy";

                        dgv.Columns.Add("rep_createDate", "Created on");
                        dgv.Columns["rep_createDate"].ReadOnly = true;
                        dgv.Columns["rep_createDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_createDate"].Width = 70;
                        dgv.Columns["rep_createDate"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_createDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

                        dgv.Columns.Add("rep_invoiceDate", "Invoice date");
                        dgv.Columns["rep_invoiceDate"].ReadOnly = true;
                        dgv.Columns["rep_invoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_invoiceDate"].Width = 70;
                        dgv.Columns["rep_invoiceDate"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_invoiceDate"].DefaultCellStyle.Format = "dd/MM/yyyy";

                        dgv.Columns.Add("rep_DocNum", "Document number");
                        dgv.Columns["rep_DocNum"].ReadOnly = true;
                        dgv.Columns["rep_DocNum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_DocNum"].Width = 70;
                        dgv.Columns["rep_DocNum"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_SalesOrder", "Sales Order");
                        dgv.Columns["rep_SalesOrder"].ReadOnly = true;
                        dgv.Columns["rep_SalesOrder"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesOrder"].Width = 70;
                        dgv.Columns["rep_SalesOrder"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_CancelDoc", "Canceled document");
                        dgv.Columns["rep_CancelDoc"].ReadOnly = true;
                        dgv.Columns["rep_CancelDoc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_CancelDoc"].Width = 70;
                        dgv.Columns["rep_CancelDoc"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("pdiv_code", "Division");
                        dgv.Columns["pdiv_code"].ReadOnly = true;
                        dgv.Columns["pdiv_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["pdiv_code"].Width = 50;
                        dgv.Columns["pdiv_code"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("sba_code", "SBA");
                        dgv.Columns["sba_code"].ReadOnly = true;
                        dgv.Columns["sba_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["sba_code"].Width = 50;
                        dgv.Columns["sba_code"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("sba_name", "SBA Name");
                        dgv.Columns["sba_name"].ReadOnly = true;
                        dgv.Columns["sba_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["sba_name"].Width = 50;
                        dgv.Columns["sba_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("msg_code", "MSG");
                        dgv.Columns["msg_code"].ReadOnly = true;
                        dgv.Columns["msg_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["msg_code"].Width = 50;
                        dgv.Columns["msg_code"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("mat_code", "Product");
                        dgv.Columns["mat_code"].ReadOnly = true;
                        dgv.Columns["mat_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["mat_code"].Width = 70;
                        dgv.Columns["mat_code"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_SalesQuan", "Sales quantity");
                        dgv.Columns["rep_SalesQuan"].ReadOnly = true;
                        dgv.Columns["rep_SalesQuan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesQuan"].Width = 50;
                        dgv.Columns["rep_SalesQuan"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_SalesQuanBum", "Invoiced Quantity in BUM");
                        dgv.Columns["rep_SalesQuanBum"].ReadOnly = true;
                        dgv.Columns["rep_SalesQuanBum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesQuanBum"].Width = 50;
                        dgv.Columns["rep_SalesQuanBum"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_Cost", "Cost of goods (euro)");
                        dgv.Columns["rep_Cost"].ReadOnly = true;
                        dgv.Columns["rep_Cost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_Cost"].Width = 70;
                        dgv.Columns["rep_Cost"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_CostRub", "Cost of goods (rub)");
                        dgv.Columns["rep_CostRub"].ReadOnly = true;
                        dgv.Columns["rep_CostRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_CostRub"].Width = 70;
                        dgv.Columns["rep_CostRub"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_SalesRev", "Sales revenue (euro)");
                        dgv.Columns["rep_SalesRev"].ReadOnly = true;
                        dgv.Columns["rep_SalesRev"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesRev"].Width = 70;
                        dgv.Columns["rep_SalesRev"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns.Add("rep_SalesRevRub", "Sales revenue (rub)");
                        dgv.Columns["rep_SalesRevRub"].ReadOnly = true;
                        dgv.Columns["rep_SalesRevRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesRevRub"].Width = 70;
                        dgv.Columns["rep_SalesRevRub"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.EnableHeadersVisualStyles = false;

                        break;
                    }
                case "Kos":
                    {
                        tabControl1.SelectedIndex = 2;
                        tabControl1.Visible = true;

                        dgv.Columns.Clear();
                        dgv.Columns.Add("rep_id", "rep_id");
                        dgv.Columns.Add("db_id", "db_id");
                        dgv.Columns.Add("rep_date", "rep_date");
                        dgv.Columns.Add("comp_nameRus", "comp_nameRus");
                        dgv.Columns.Add("cust_code", "cust_code");
                        dgv.Columns.Add("cust_name", "cust_name");
                        dgv.Columns.Add("rep_dist", "rep_dist");
                        dgv.Columns.Add("subreg_nameRus", "subreg_nameRus");
                        dgv.Columns.Add("lpu_sname", "lpu_sname");
                        dgv.Columns.Add("nom_group", "nom_group");
                        dgv.Columns.Add("nom_name", "nom_name");
                        dgv.Columns.Add("rep_SalesQuanBum", "rep_SalesQuanBum");
                        dgv.Columns.Add("rep_SalesRev", "rep_SalesRev");
                        dgv.Columns.Add("rep_SalesRevRub", "rep_SalesRevRub");
                        dgv.Columns.Add("user_name", "Пользователь");


                        if (globalData.UserID == 1)
                        {
                            dgv.Columns["rep_id"].Visible = true;
                            dgv.Columns["db_id"].Visible = true;
                        }
                        else
                        {
                            dgv.Columns["rep_id"].Visible = false;
                            dgv.Columns["db_id"].Visible = false;
                        }

                        if (!cbUsers3.Visible)
                            dgv.Columns["user_name"].Visible = true;
                        else
                            dgv.Columns["user_name"].Visible = false;


                        dgv.Columns["rep_date"].DefaultCellStyle.Format = "MM - yyyy";

                        dgv.Columns["rep_date"].HeaderText = "Дата";
                        dgv.Columns["comp_nameRus"].HeaderText = "Компания";
                        dgv.Columns["cust_code"].HeaderText = "код дистрибьютора";
                        dgv.Columns["cust_name"].HeaderText = "Дистрибьютор";
                        dgv.Columns["rep_dist"].HeaderText = "Промежуточный дистрибьютор";
                        dgv.Columns["subreg_nameRus"].HeaderText = "Регион";
                        dgv.Columns["lpu_sname"].HeaderText = "ЛПУ";
                        dgv.Columns["nom_group"].HeaderText = "Группа товаров";
                        dgv.Columns["nom_name"].HeaderText = "Номенклатура";
                        dgv.Columns["rep_SalesQuanBum"].HeaderText = "Количество";
                        dgv.Columns["rep_SalesRev"].HeaderText = "Цена в Евро";
                        dgv.Columns["rep_SalesRevRub"].HeaderText = "Цена в рублях";

                        dgv.Columns["rep_date"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["comp_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["cust_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["cust_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_dist"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["subreg_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["lpu_sname"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["nom_group"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["nom_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_SalesQuanBum"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_SalesRev"].SortMode = DataGridViewColumnSortMode.Programmatic;
                        dgv.Columns["rep_SalesRevRub"].SortMode = DataGridViewColumnSortMode.Programmatic;

                        dgv.Columns["rep_SalesQuanBum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesRev"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesRevRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgv.Columns["rep_SalesQuanBum"].DefaultCellStyle.Format = "N0";
                        dgv.Columns["rep_SalesRev"].DefaultCellStyle.Format = "N2";
                        dgv.Columns["rep_SalesRevRub"].DefaultCellStyle.Format = "N2";

                        break;
                    }
                case "DynKard":
                    {
                        tabControl1.SelectedIndex = 1;
                        tabControl1.Visible = true;

                        dgv.Columns.Clear();
                        dgv.Columns.Add("empty", "");
                        dgv.Columns.Add("jan", "Январь");
                        dgv.Columns.Add("feb", "Февраль");
                        dgv.Columns.Add("mart", "Март");
                        dgv.Columns.Add("apr", "Апрель");
                        dgv.Columns.Add("may", "Май");
                        dgv.Columns.Add("june", "Июнь");
                        dgv.Columns.Add("jule", "Июль");
                        dgv.Columns.Add("aug", "Август");
                        dgv.Columns.Add("sept", "Сентябрь");
                        dgv.Columns.Add("okt", "Октябрь");
                        dgv.Columns.Add("nov", "Ноябрь");
                        dgv.Columns.Add("dec", "Декабрь");
                        dgv.Columns.Add("total", "Итого");

                        break;
                    }
                case "Acc":
                    {
                        int year;
                        if (tabControl1.SelectedIndex == 3)
                            year = Convert.ToInt32(cbYearAcc.SelectedItem.ToString());
                        else
                            year = Convert.ToInt32(cbYearAccAll.SelectedItem);

                        if (dgv.ColumnCount == 0)
                        {
                            dgv.Columns.Add("acc_id", "acc_id");
                            dgv.Columns.Add("nom_id", "nom_id");
                            dgv.Columns.Add("nom", "Номенклатура");
                            dgv.Columns.Add("nom_type", "шт/\nевро");

                            dgv.Columns.Add("py3", (year - 3).ToString() + "\nфакт, EUR");
                            dgv.Columns.Add("py2", (year - 1).ToString() + "\nфакт, шт");
                            dgv.Columns.Add("py1", (year - 1).ToString() + "\nфакт, EUR");

                            dgv.Columns.Add("cyplan", year.ToString() + "\nплан, шт");
                            dgv.Columns.Add("dilcost", "Дил. цена\nбез НДС");
                            dgv.Columns.Add("dilcostRub", "Дил. цена\nбез НДС,руб.");
                            dgv.Columns.Add("cyplanEuro", year.ToString() + "\nплан, EUR");
                            dgv.Columns.Add("cyplanRub", year.ToString() + "\nплан, РУБ");
                            dgv.Columns.Add("cyfact", year.ToString() + "\nфакт, шт");
                            dgv.Columns.Add("cyfactEuro", year.ToString() + "\nфакт, EUR");
                            dgv.Columns.Add("cyfactRub", year.ToString() + "\nфакт, РУБ");
                            dgv.Columns.Add("pr", "% плана");
                            dgv.Columns.Add("prEuro", "% плана (Евро)");

                            dgv.Columns.Add("JanFact", "1");
                            dgv.Columns.Add("FebFact", "2");
                            dgv.Columns.Add("MartFact", "");
                            dgv.Columns.Add("AprFact", "");
                            dgv.Columns.Add("MayFact", "");
                            dgv.Columns.Add("JuneFact", "");
                            dgv.Columns.Add("JuleFact", "");
                            dgv.Columns.Add("AugFact", "");
                            dgv.Columns.Add("SepFact", "");
                            dgv.Columns.Add("OktFact", "");
                            dgv.Columns.Add("NovFact", "");
                            dgv.Columns.Add("DecFact", "");

                            dgv.Columns.Add("prRub", "% плана (РУБ)");

                            dgv.Columns.Add("JanFactR", "1");
                            dgv.Columns.Add("FebFactR", "2");
                            dgv.Columns.Add("MartFactR", "");
                            dgv.Columns.Add("AprFactR", "");
                            dgv.Columns.Add("MayFactR", "");
                            dgv.Columns.Add("JuneFactR", "");
                            dgv.Columns.Add("JuleFactR", "");
                            dgv.Columns.Add("AugFactR", "");
                            dgv.Columns.Add("SepFactR", "");
                            dgv.Columns.Add("OktFactR", "");
                            dgv.Columns.Add("NovFactR", "");
                            dgv.Columns.Add("DecFactR", "");


                            dgv.Columns.Add("nom_group", "");
                            dgv.Columns.Add("div", "");

                            dgv.Columns.Add("h1Sales", "I квартал Общий объем закупок ЛПУ");
                            dgv.Columns.Add("vc_id1", "");
                            dgv.Columns.Add("vc_sum1", "I квартал Объем закупок конкурентов");

                            dgv.Columns.Add("h2Sales", "II квартал Общий объем закупок ЛПУ");
                            dgv.Columns.Add("vc_id2", "");
                            dgv.Columns.Add("vc_sum2", "II квартал Объем закупок конкурентов");

                            dgv.Columns.Add("h3Sales", "III квартал Общий объем закупок ЛПУ");
                            dgv.Columns.Add("vc_id3", "");  
                            dgv.Columns.Add("vc_sum3", "III квартал Объем закупок конкурентов");

                            dgv.Columns.Add("h4Sales", "IV квартал Общий объем закупок ЛПУ");
                            dgv.Columns.Add("vc_id4", "");
                            dgv.Columns.Add("vc_sum4", "IV квартал Объем закупок конкурентов");

                            dgv.Columns.Add("upd", "");
                            dgv.Columns.Add("upd2", "");
                            dgv.Columns.Add("upd3", "");
                            dgv.Columns.Add("upd4", "");
                            
                            dgv.Columns.Add("prVC", "Доли конкурентов в объеме продаж " + year.ToString() + " год");
                        }
                        else
                        {
                            dgv.Rows.Clear();
                            dgv.Columns["py3"].HeaderText = (year - 3).ToString() + "\nфакт, EUR";
                            dgv.Columns["py2"].HeaderText = (year - 1).ToString() + "\nфакт, шт";
                            dgv.Columns["py1"].HeaderText = (year - 1).ToString() + "\nфакт, EUR";
                            dgv.Columns["cyplan"].HeaderText = year.ToString() + "\nплан, шт";
                            dgv.Columns["cyplanEuro"].HeaderText = year.ToString() + "\nплан, EUR";
                            dgv.Columns["cyplanRub"].HeaderText = year.ToString() + "\nплан, РУБ";
                            dgv.Columns["cyfact"].HeaderText = year.ToString() + "\nфакт, шт";
                            dgv.Columns["cyfactEuro"].HeaderText = year.ToString() + "\nфакт, EUR";
                            dgv.Columns["cyplanRub"].HeaderText = year.ToString() + "\nплан, РУБ";
                            dgv.Columns["prVC"].HeaderText = "Доли конкурентов в объеме продаж" + year.ToString() + " год";
                        }
                        break;
                    }
                case "AccNY":
                    {
                        if (dgv.ColumnCount == 0)
                        {
                            dgv.Columns.Clear();

                            Sql sql1 = new Sql();
                            DataTable dt1 = new DataTable();

                            dt1 = sql1.GetRecords("exec GetSettings");

                            int year = Convert.ToInt32(dt1.Rows[0].ItemArray[3]);

                            dgv.Columns.Add("acc_id", "acc_id");
                            dgv.Columns.Add("nom_id", "nom_id");
                            dgv.Columns.Add("nom", "Номенклатура");
                            dgv.Columns.Add("nom_type", "шт/\nевро");

                            dgv.Columns.Add("py4", (year - 2).ToString() + "\nплан, шт");
                            dgv.Columns.Add("py3", (year - 2).ToString() + "\nплан, EUR");
                            dgv.Columns.Add("py2", (year - 2).ToString() + "\nфакт, шт");
                            dgv.Columns.Add("py1", (year - 2).ToString() + "\nфакт, EUR");

                            dgv.Columns.Add("lyplan", (year - 1).ToString() + "\nплан, шт");
                            dgv.Columns.Add("lydilcost", (year - 1).ToString() + "\nДил. цена\nбез НДС");
                            dgv.Columns.Add("lyplanEuro", (year - 1).ToString() + "\nплан, EUR");
                            dgv.Columns.Add("lyfact", (year - 1).ToString() + "\nфакт, шт");
                            dgv.Columns.Add("lyfactEuro", (year - 1).ToString() + "\nфакт, EUR");

                            dgv.Columns.Add("pr", "% плана");
                            dgv.Columns.Add("prEuro", "% плана (Евро)");

                            dgv.Columns.Add("cyplan", year.ToString() + "\nплан, шт");
                            dgv.Columns.Add("cydilcost", year.ToString() + "\nДил. цена\nбез НДС");                           
                            dgv.Columns.Add("cyplanEuro", year.ToString() + "\nплан, EUR");                           
                            dgv.Columns.Add("prCyLyEuro", "План " + year.ToString() + "/\nФакт " + (year - 1).ToString() + " (%)");
                            
                            dgv.Columns.Add("nom_group", "");
                            dgv.Columns.Add("nom_seq", "");
                            dgv.Columns.Add("nom_year1", "");
                            dgv.Columns.Add("nom_year2", "");                            
                            dgv.Columns.Add("cydilcostRub", year.ToString() + "\nДил. цена\nРУБ");
                            dgv.Columns.Add("cyplanRub", year.ToString() + "\nплан, РУБ");
                            dgv.Columns.Add("upd", "");
                        }
                        else
                            dgv.Rows.Clear();
                        break;
                    }
            }
        }

        private void fillComboBox(DataTable dt1, String nameCol, ComboBox cb1, int filtnum)
        {
            var query = (from c in dt1.AsEnumerable()
                         orderby c.Field<String>(nameCol + "_name") ascending
                         select new
                         {
                             Number = c.Field<String>(nameCol + "_code"),
                             Name = c.Field<String>(nameCol + "_name")
                         }).Distinct();
            if (query.Count() != 0)
            {
                cb1.Items.Clear();
                cb1.Items.Add("все");
                for (int i = 0; i < query.Count(); i++)
                {
                    cb1.Items.Add(query.ElementAt(i).Number + " - " + query.ElementAt(i).Name);
                }
                if (f[filtnum] == String.Empty)
                    cb1.SelectedIndex = 0;
                cb1.Enabled = true;
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (((e.ColumnIndex >= 3) && (e.ColumnIndex <= 12)) || (e.ColumnIndex == 17) || (e.ColumnIndex == 18) || (e.ColumnIndex == 20) || (e.ColumnIndex == 27) || (e.ColumnIndex == 31))
                {
                    SubSum(e.ColumnIndex);
                }
            }
        }

        private void SubSum(int colnum)
        {
            try
            {
                Double sum = 0, sum2 = 0, sum3 = 0;
                Double msum = 0, msum2 = 0, msum3 = 0;

                for (int j = 0; j < _dgv1.Rows.Count; j++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row = _dgv1.Rows[j];
                    if ((row.DefaultCellStyle.BackColor == bbgreen1) || (row.DefaultCellStyle.BackColor == bbgreen3))
                    {
                        _dgv1.Rows.Remove(row);
                        j--;
                    }
                }

                if (subsum == colnum)
                {
                    subsum = 0;
                    return;
                }

                _dgv1.Sort(_dgv1.Columns[colnum], ListSortDirection.Ascending);
                String s1 = _dgv1.Rows[0].Cells[colnum].Value.ToString();

                int i = 0;
                foreach (DataGridViewRow row in _dgv1.Rows)
                {
                    if ((row.Cells[colnum].Value.ToString() != s1) && (s1 != ""))
                    {
                        s1 = _dgv1.Rows[i].Cells[colnum].Value.ToString();
                        if (chbOnlyPS.Checked)
                            _dgv1.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                        else
                            _dgv1.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                        _dgv1.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                        msum += sum;
                        msum2 += sum2;
                        msum3 += sum3;
                        sum = 0;
                        sum2 = 0;
                        sum3 = 0;
                    }
                    else if (s1 != "")
                    {
                        if (chbOnlyPS.Checked)
                        {
                            sum += Convert.ToDouble(row.Cells["fact"].Value.ToString());
                            sum2 += Convert.ToDouble(row.Cells["sumSale"].Value.ToString());
                            sum3 += Convert.ToDouble(row.Cells["sumSaleRub"].Value.ToString());
                        }
                        else
                        {
                            sum += Convert.ToDouble(row.Cells["sr"].Value.ToString());
                            if(rb != 5)
                                sum2 += Convert.ToDouble(row.Cells["srRub"].Value.ToString());
                            sum3 += Convert.ToDouble(row.Cells["bum"].Value.ToString());
                        }
                    }
                    i++;
                }
                msum += sum;
                msum2 += sum2;
                msum3 += sum3;

                if (chbOnlyPS.Checked)
                {
                    _dgv1.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                    _dgv1.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", msum, msum2, msum3);
                }
                else
                {
                    _dgv1.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                    _dgv1.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", msum, msum2, msum3);
                }
                _dgv1.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                _dgv1.Rows[i + 1].DefaultCellStyle.BackColor = bbgreen1;
                subsum = colnum;
            }
            catch
            {
                subsum = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (comboBox1.SelectedItem.ToString() != "все")
                {
                    String str = comboBox1.SelectedItem.ToString();
                    str = str.Split(' ')[0];
                    f[0] = str;
                    filter = true;
                }
                else
                {
                    f[0] = "";
                }
                loadData();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (comboBox2.SelectedItem.ToString() != "все")
                {
                    String str = comboBox2.SelectedItem.ToString();
                    str = str.Split(' ')[0];
                    f[1] = str;
                    filter = true;
                }
                else
                {
                    f[1] = "";
                }
                loadData();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (comboBox4.SelectedItem.ToString() != "все")
                {
                    String str = comboBox4.SelectedItem.ToString();
                    str = str.Split(' ')[0];
                    f[3] = str;
                    filter = true;
                }
                else
                {
                    f[3] = "";
                }
                loadData();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (comboBox5.SelectedItem.ToString() != "все")
                {
                    f[4] = comboBox5.SelectedItem.ToString();
                    filter = true;
                }
                else
                {
                    f[4] = "";
                }
                loadData();
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (comboBox6.SelectedItem.ToString() != "все")
                {
                    f[5] = comboBox6.SelectedItem.ToString();
                    filter = true;
                }
                else
                {
                    f[5] = "";
                }
                loadData();
            }
        }

        private void ClearFilter()
        {
            for (int i = 0; i < 6; i++)
                f[i] = "";
            filter = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "";
            button = "";
            loadData();
            _dgv1.Columns["sba_code"].Visible = true;
            _dgv1.Columns["sba_name"].Visible = true;
            _dgv1.Columns["mmg_code"].Visible = false;
            _dgv1.Columns["mmg_name"].Visible = false;

            setButtonColor(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button2.Text;
            
            if (button2.Text == "ST")
            {
                _dgv1.Columns["sba_code"].Visible = false;
                _dgv1.Columns["sba_name"].Visible = false;
                _dgv1.Columns["mmg_code"].Visible = true;
                _dgv1.Columns["mmg_name"].Visible = true;
            }

            loadData();
            setButtonColor(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button3.Text;
            
            if (button3.Text == "NE")
            {
                _dgv1.Columns["sba_code"].Visible = false;
                _dgv1.Columns["sba_name"].Visible = false;
                _dgv1.Columns["mmg_code"].Visible = true;
                _dgv1.Columns["mmg_name"].Visible = true;
            }

            loadData();
            setButtonColor(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button4.Text;

            if (button4.Text == "PS")
            {
                _dgv1.Columns["sba_code"].Visible = false;
                _dgv1.Columns["sba_name"].Visible = false;
                _dgv1.Columns["mmg_code"].Visible = true;
                _dgv1.Columns["mmg_name"].Visible = true;
            }

            loadData();
            setButtonColor(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button5.Text;
                        
            if (button5.Text == "OT")
            {
                _dgv1.Columns["sba_code"].Visible = false;
                _dgv1.Columns["sba_name"].Visible = false;
                _dgv1.Columns["mmg_code"].Visible = true;
                _dgv1.Columns["mmg_name"].Visible = true;
            }

            loadData();
            setButtonColor(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button6.Text;
            loadData();
            _dgv1.Columns["sba_code"].Visible = false;
            _dgv1.Columns["sba_name"].Visible = false;
            _dgv1.Columns["mmg_code"].Visible = true;
            _dgv1.Columns["mmg_name"].Visible = true;

            setButtonColor(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button7.Text;
            loadData();
            _dgv1.Columns["sba_code"].Visible = false;
            _dgv1.Columns["sba_name"].Visible = false;
            _dgv1.Columns["mmg_code"].Visible = true;
            _dgv1.Columns["mmg_name"].Visible = true;

            setButtonColor(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button8.Text;
            loadData();
            _dgv1.Columns["sba_code"].Visible = false;
            _dgv1.Columns["sba_name"].Visible = false;
            _dgv1.Columns["mmg_code"].Visible = true;
            _dgv1.Columns["mmg_name"].Visible = true;

            setButtonColor(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ClearFilter();
            proc2 = "Button";
            button = button9.Text;
            loadData();
            _dgv1.Columns["sba_code"].Visible = false;
            _dgv1.Columns["sba_name"].Visible = false;
            _dgv1.Columns["mmg_code"].Visible = true;
            _dgv1.Columns["mmg_name"].Visible = true;

            setButtonColor(9);
        }

        private void setButtonColor(int cur)
        {
            button1.BackColor = Control.DefaultBackColor;
            button2.BackColor = Control.DefaultBackColor;
            button3.BackColor = Control.DefaultBackColor;
            button4.BackColor = Control.DefaultBackColor;
            button5.BackColor = Control.DefaultBackColor;
            button6.BackColor = Control.DefaultBackColor;
            button7.BackColor = Control.DefaultBackColor;
            button8.BackColor = Control.DefaultBackColor;
            button9.BackColor = Control.DefaultBackColor;

            switch (cur)
            {
                case 1:
                    {
                        button1.BackColor = bbgreen3;
                        break;
                    }
                case 2:
                    {
                        button2.BackColor = bbgreen3;
                        break;
                    }
                case 3:
                    {
                        button3.BackColor = bbgreen3;
                        break;
                    }
                case 4:
                    {
                        button4.BackColor = bbgreen3;
                        break;
                    }
                case 5:
                    {
                        button5.BackColor = bbgreen3;
                        break;
                    }
                case 6:
                    {
                        button6.BackColor = bbgreen3;
                        break;
                    }
                case 7:
                    {
                        button7.BackColor = bbgreen3;
                        break;
                    }
                case 8:
                    {
                        button8.BackColor = bbgreen3;
                        break;
                    }
                case 9:
                    {
                        button9.BackColor = bbgreen3;
                        break;
                    }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                rb = 1;
                ClearFilter();
                chbOnlyPS.Checked = false;
                fillFilter = true;
                loadData();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                rb = 2;
                ClearFilter();
                chbOnlyPS.Checked = false;
                fillFilter = true;
                loadData();
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                rb = 3;
                ClearFilter();
                chbOnlyPS.Checked = false;
                fillFilter = true;
                loadData();
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                rb = 4;
                ClearFilter();
                fillFilter = true;
                chbOnlyPS.Checked = false;
                if (globalData.Region == "Российская федерация")
                    _dgv1.Columns.Clear();
                loadData();
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                rb = 5;
                ClearFilter();
                fillFilter = true;
                chbOnlyPS.Checked = false;
                loadData();
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                rb = 6;
                ClearFilter();
                fillFilter = true;
                chbOnlyPS.Checked = false;
                _dgv1.Columns.Clear();
                loadData();
            }
        }

        private void заполнитьДинамикуПродажПоКардиологииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputDialog ind = new InputDialog("Год", "Введите год", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;
            
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec Region_Select");
            for (int i = 1; i < 7; i++)
            {
                foreach (DataRow row in dt1.Rows)
                {
                    sql1.GetRecords("exec InsDynKard @p1, @p2, @p3", i, row[dt1.Columns[0]].ToString(), globalData.input);
                }
            }
            MessageBox.Show("Динамика заполнена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _dgv1EndEdit(e.ColumnIndex, e.RowIndex);

            if ((e.ColumnIndex == 29) && (!chbOnlyPS.Checked) && (subsum != 0))
            {
                if (_dgv1.Rows[e.RowIndex].Cells["check"].Value != null)
                {
                    string val = _dgv1.Rows[e.RowIndex].Cells["check"].Value.ToString();

                    for (int i = e.RowIndex - 1; i >= 0; i--)
                    {
                        if (_dgv1.Rows[i].DefaultCellStyle.BackColor == bbgreen3)
                            break;

                        _dgv1.Rows[i].Cells["check"].Value = val;
                    }
                }
            }
        }

        private void _dgv1EndEdit(int ColumnIndex, int RowIndex)
        {
            try
            {
                if (ColumnIndex == 24)
                {
                    if (!chbOnlyPS.Checked)
                    {
                        var val = _dgv1.Rows[RowIndex];
                        int v1 = Int32.Parse(val.Cells["fact"].Value.ToString());
                        if (Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["fact"].Value) > Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["bum"].Value))
                        {
                            _dgv1.Rows[RowIndex].Cells["fact"].Value = "";
                            _dgv1.Rows[RowIndex].Cells["sumSale"].Value = "";
                            _dgv1.Rows[RowIndex].Cells["sumSaleRub"].Value = "";
                        }
                        else
                        {
                            if (_dgv1.Rows[RowIndex].Cells["UnitPrice"].Value != null)
                                _dgv1.Rows[RowIndex].Cells["sumSale"].Value = Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["UnitPrice"].Value.ToString()) * Convert.ToDouble(_dgv1.CurrentRow.Cells["fact"].Value);
                            else
                                _dgv1.Rows[RowIndex].Cells["sumSale"].Value = (Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["sr"].Value.ToString()) / Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["bum"].Value.ToString())) * Convert.ToDouble(_dgv1.CurrentRow.Cells["fact"].Value);
                            if (_dgv1.Rows[RowIndex].Cells["UnitPriceRub"].Value != null)
                                _dgv1.Rows[RowIndex].Cells["sumSaleRub"].Value = Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["UnitPriceRub"].Value.ToString()) * Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["fact"].Value);
                        }
                    }
                    else
                    {
                        if (_dgv1.Rows[RowIndex].Cells["fact"].Value.ToString() == String.Empty)
                            _dgv1.Rows[RowIndex].Cells["fact"].Value = 0;
                            
                        if (Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["count"].Value) < Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["fact"].Value))
                        {
                            _dgv1.Rows[RowIndex].Cells["fact"].Value = _dgv1.Rows[RowIndex].Cells["count"].Value;
                        }
                        else
                        {
                            if (_dgv1.Rows[RowIndex].Cells["fact"].Value.ToString() == "")
                                _dgv1.Rows[RowIndex].Cells["fact"].Value = 0;
                            _dgv1.Rows[RowIndex].Cells["bum"].Value = begval - Convert.ToInt32(_dgv1.Rows[RowIndex].Cells["fact"].Value.ToString()) + Convert.ToInt32(_dgv1.Rows[RowIndex].Cells["bum"].Value.ToString());
                            _dgv1.Rows[RowIndex].Cells["sumSale"].Value = Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["fact"].Value.ToString()) * Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["UnitPrice"].Value);
                            _dgv1.Rows[RowIndex].Cells["sumSaleRub"].Value = Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["fact"].Value.ToString()) * Convert.ToDouble(_dgv1.Rows[RowIndex].Cells["UnitPriceRub"].Value);
                        }
                    }
                }

                if (ColumnIndex == 28)
                {
                    if (_dgv1.Rows[RowIndex].Cells[0].Value.ToString() == "")
                    {
                        int i = RowIndex - 1;
                        while (_dgv1.Rows[i].Cells[0].Value.ToString() != "")
                        {
                            if (Convert.ToBoolean(_dgv1.Rows[RowIndex].Cells["check"].Value) == false)
                                _dgv1.Rows[i].Cells["check"].Value = false;
                            else
                                _dgv1.Rows[i].Cells["check"].Value = true;
                            i--;
                            if (i == -1)
                                break;
                        }
                    }
                }
                _dgv1.Rows[RowIndex].Cells["upd"].Value = "1";
            }
            catch
            {
                _dgv1.Rows[RowIndex].Cells["fact"].Value = "";
                _dgv1.Rows[RowIndex].Cells["sumSale"].Value = "";
                _dgv1.Rows[RowIndex].Cells["sumSaleRub"].Value = "";
                _dgv1.Rows[RowIndex].Cells["upd"].Value = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dttemp = globalData.CurDate;
                if ((globalData.CurDate != dateTimePicker1.Value) && (globalData.UserAccess == 1))
                    globalData.CurDate = dateTimePicker1.Value;

                if (!chbOnlyPS.Checked)
                {
                    if (cbUsers.SelectedValue.ToString() == "0")
                    {
                        MessageBox.Show("Для сохранения необходимо выбрать пользователя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Sql sql1 = new Sql();
                    DataTable dt1 = new DataTable();
                    bool b = false;
                    int i = 0;
                    foreach (DataGridViewRow row in _dgv1.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["check"].Value) == true)
                        {
                            if ((Convert.ToBoolean(row.Cells["check"].Value) != false) && (row.Cells[0].Value.ToString() != ""))
                            {
                                sql1.GetRecords("exec checkout @p1, @p2, 1", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value);
                                b = true;
                            }
                        }
                        else if ((row.Cells[0].Value.ToString() != "") && (rb == 3))
                        {
                            sql1.GetRecords("exec checkout @p1, @p2, 0", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value);
                            b = true;
                        }
                        else
                        {
                            if (subsum == 0)
                            {
                                if ((row.Cells["fact"].Value != null) && (row.Cells["lpu"].Value != null))
                                {
                                    if ((row.Cells["fact"].Value.ToString() != "") && (row.Cells["lpu"].Value.ToString() != "") && (row.Cells["fact"].Value.ToString() != "0"))
                                    {
                                        string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value, globalData.db_id, row.Cells["lpu"].Value, cbUsers.SelectedValue.ToString(), row.Cells["fact"].Value, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                        if ((res != "1") && (globalData.UserAccess == 1))
                                            MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        b = true;
                                    }

                                    else if ((row.Cells["fact"].Value != null) && (globalData.Div == "OM"))
                                    {
                                        if ((row.Cells["fact"].Value.ToString() != "") && (row.Cells["fact"].Value.ToString() != "0"))
                                        {
                                            string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, 'OM', @p4, @p5, @p6, @p7, @p8, @p9", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value, globalData.db_id, cbUsers.SelectedValue.ToString(), row.Cells["fact"].Value, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                            if ((res != "1") && (globalData.UserAccess == 1))
                                                MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            b = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if ((row.Cells["fact"].Value != null) && (row.Cells["lpu"].Value != null) && (row.Cells[0].Value.ToString() == ""))
                                {
                                    if ((row.Cells["fact"].Value.ToString() != "") && (row.Cells["lpu"].Value.ToString() != "") && (row.Cells["fact"].Value.ToString() != "0"))
                                    {
                                        int index = i;
                                        int count = Convert.ToInt32(row.Cells["fact"].Value.ToString());
                                        while (count > 0)
                                        {
                                            DataGridViewRow row2 = new DataGridViewRow();
                                            row2 = _dgv1.Rows[--index];

                                            if (row2.Cells["bum"].Value.ToString() == "0")
                                                continue;

                                            if (count > Convert.ToInt32(row2.Cells["bum"].Value))
                                            {
                                                string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10", row2.Cells["rep_id"].Value, row2.Cells["db_id_rep"].Value, globalData.db_id, row.Cells["lpu"].Value, cbUsers.SelectedValue.ToString(), row2.Cells["bum"].Value, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                                if ((res != "1") && (globalData.UserAccess == 1))
                                                    MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                count -= Convert.ToInt32(row2.Cells["bum"].Value);
                                            }
                                            else
                                            {
                                                string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10", row2.Cells["rep_id"].Value, row2.Cells["db_id_rep"].Value, globalData.db_id, row.Cells["lpu"].Value, cbUsers.SelectedValue.ToString(), count, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                                if ((res != "1") && (globalData.UserAccess == 1))
                                                    MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                count = 0;
                                            }
                                        }
                                        b = true;
                                    }
                                }
                                else if ((globalData.Div != "OM") && (row.Cells["fact"].Value != null) && (row.Cells["lpu"].Value != null))
                                {
                                    if ((row.Cells["fact"].Value.ToString() != "") && (row.Cells["lpu"].Value.ToString() != "") && (row.Cells["fact"].Value.ToString() != "0"))
                                    {
                                        string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value, globalData.db_id, row.Cells["lpu"].Value, cbUsers.SelectedValue.ToString(), row.Cells["fact"].Value, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                        if ((res != "1") && (globalData.UserAccess == 1))
                                            MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        b = true;
                                    }
                                }

                                else if ((globalData.Div == "OM") && (row.Cells["fact"].Value != null) && (row.Cells[0].Value.ToString() == ""))
                                {
                                    if ((row.Cells["fact"].Value.ToString() != "") && (row.Cells["fact"].Value.ToString() != "0"))
                                    {
                                        int index = i;
                                        int count = Convert.ToInt32(row.Cells["fact"].Value.ToString());
                                        while (count > 0)
                                        {
                                            DataGridViewRow row2 = new DataGridViewRow();
                                            row2 = _dgv1.Rows[--index];

                                            if (row2.Cells["bum"].Value.ToString() == "0")
                                                continue;

                                            if (count > Convert.ToInt32(row2.Cells["bum"].Value))
                                            {
                                                string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, 'OM', @p4, @p5, @p6, @p7, @p8, @p9", row2.Cells["rep_id"].Value, row2.Cells["db_id_rep"].Value, globalData.db_id, cbUsers.SelectedValue.ToString(), row2.Cells["bum"].Value, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                                if ((res != "1") && (globalData.UserAccess == 1))
                                                    MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                count -= Convert.ToInt32(row2.Cells["bum"].Value);
                                            }
                                            else
                                            {
                                                string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, 'OM', @p4, @p5, @p6, @p7, @p8, @p9", row2.Cells["rep_id"].Value, row2.Cells["db_id_rep"].Value, globalData.db_id, cbUsers.SelectedValue.ToString(), count, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                                if ((res != "1") && (globalData.UserAccess == 1))
                                                    MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                count = 0;
                                            }
                                        }
                                        b = true;
                                    }
                                }
                                else if ((globalData.Div == "OM") && (row.Cells["fact"].Value != null))
                                {
                                    if ((row.Cells["fact"].Value.ToString() != "") && (row.Cells["fact"].Value.ToString() != "0"))
                                    {
                                        string res = sql1.GetRecordsOne("exec InsPersSales @p1, 0, @p2, @p3, 'OM', @p4, @p5, @p6, @p7, @p8, @p9", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value, globalData.db_id, cbUsers.SelectedValue.ToString(), row.Cells["fact"].Value, globalData.Region, globalData.UserID, globalData.CurDate, globalData.Div);
                                        if ((res != "1") && (globalData.UserAccess == 1))
                                            MessageBox.Show(res, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        b = true;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                    if (b)
                    {
                        loadData();
                        int temp = subsum;
                        subsum = 0;
                        SubSum(temp);
                    }
                }
                else
                {
                    Sql sql1 = new Sql();
                    DataTable dt1 = new DataTable();
                    bool b = false;
                    int tsubsum = subsum;
                    SubSum(subsum);

                    foreach (DataGridViewRow row in _dgv1.Rows)
                    {
                        DateTime date;
                        if (globalData.UserAccess == 1)
                            date = Convert.ToDateTime(row.Cells["date"].Value);
                        else
                            date = globalData.CurDate;

                        if ((globalData.Div == "OM") && ((row.Cells["fact"].Value.ToString() == "") || (row.Cells["fact"].Value.ToString() == "0")))
                        {
                            sql1.GetRecords("exec DelPersSalesOM @p1, @p2, @p3, @p4, @p5, @p6", row.Cells["rep_id"].Value.ToString(), row.Cells["db_id_rep"].Value.ToString(), row.Cells["db_id_ps"].Value.ToString(), cbUsers.SelectedValue.ToString(), row.Cells["bum"].Value.ToString(), date);
                            b = true;
                        }
                        else if ((globalData.Div == "OM") && (row.Cells["fact"].Value.ToString() != "") && (Convert.ToInt32(row.Cells["fact"].Value.ToString()) < Convert.ToInt32(row.Cells["count"].Value.ToString())))
                        {
                            sql1.GetRecords("exec UpdPersSalesOM @p1, @p2, @p3, @p4, @p5, @p6, @p7", row.Cells["rep_id"].Value.ToString(), row.Cells["db_id_rep"].Value.ToString(), row.Cells["db_id_ps"].Value.ToString(), cbUsers.SelectedValue.ToString(), row.Cells["fact"].Value, globalData.UserID, date);
                            b = true;
                        }

                        if ((globalData.Div != "OM") && ((row.Cells["fact"].Value.ToString() == "") || (row.Cells["lpu"].Value == null) || (row.Cells["fact"].Value.ToString() == "0")))
                        {
                            sql1.GetRecords("exec DelPersSales @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9", row.Cells["rep_id"].Value.ToString(), row.Cells["db_id_rep"].Value.ToString(), row.Cells["db_id_ps"].Value.ToString(), row.Cells["lpu"].Value.ToString(), cbUsers.SelectedValue.ToString(), row.Cells["bum"].Value.ToString(), globalData.UserID, date, globalData.Region);
                            b = true;
                        }
                        else if ((globalData.Div != "OM") && (row.Cells["fact"].Value.ToString() != "") && (row.Cells["lpu"].Value.ToString() != "") && (Convert.ToInt32(row.Cells["fact"].Value.ToString()) <= Convert.ToInt32(row.Cells["count"].Value.ToString())) && (row.Cells["upd"].Value.ToString() == "1"))
                        {
                            sql1.GetRecords("exec UpdPersSales @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9", row.Cells["rep_id"].Value, row.Cells["db_id_rep"].Value, row.Cells["db_id_ps"].Value, row.Cells["lpu"].Value, cbUsers.SelectedValue, row.Cells["fact"].Value, globalData.UserID, date, row.Cells["ps_id"].Value);
                            b = true;
                        }
                    }
                    SubSum(tsubsum);
                    if (b)
                    {
                        loadData();
                        int temp = subsum;
                        subsum = 0;
                        SubSum(temp);
                    }
                }

                if ((globalData.CurDate != dateTimePicker1.Value) && (globalData.UserAccess == 1))
                    globalData.CurDate = dttemp;
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить данные. Ошибка - " + err, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chbOnlyPersSales_CheckedChanged(object sender, EventArgs e)
        {
            if (_dgv1.ColumnCount > 0)
            {
                if (chbOnlyPS.Checked)
                {
                    proc1 = "PersSales";
                    _dgv1.Columns["bum"].HeaderText = "Остаток";
                    btnHideUsersAP.Text = "Скрыть";
                    btnHideUsersAP.Visible = true;
                    cbUsers.Visible = true;
                    if (_dgv1.Rows.Count != 0)
                    {
                        globalData.indexRow = _dgv1.CurrentCell.RowIndex;
                        globalData.indexCol = _dgv1.CurrentCell.ColumnIndex;
                    }
                }
                else
                {
                    proc1 = "";
                    _dgv1.Columns["bum"].HeaderText = "Количество (шт)";
                    btnHideUsersAP.Visible = false;
                    cbUsers.Visible = true;
                    if (_dgv1.Rows.Count != 0)
                    {
                        globalData.indexRow2 = _dgv1.CurrentCell.RowIndex;
                        globalData.indexCol2 = _dgv1.CurrentCell.ColumnIndex;
                    }
                }                

                loadData();

                if (_dgv1.Rows.Count != 0)
                {
                    if (chbOnlyPS.Checked)
                    {
                        if (globalData.indexRow2 >= _dgv1.Rows.Count)
                            _dgv1.CurrentCell = _dgv1[globalData.indexCol2, _dgv1.Rows.Count - 1];
                        else
                            _dgv1.CurrentCell = _dgv1[globalData.indexCol2, globalData.indexRow2];
                    }
                    else
                    {
                        if (globalData.indexRow >= _dgv1.Rows.Count)
                            _dgv1.CurrentCell = _dgv1[globalData.indexCol, _dgv1.Rows.Count - 1];
                        else
                            _dgv1.CurrentCell = _dgv1[globalData.indexCol, globalData.indexRow];
                    }
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 23)
                {
                    begval = Convert.ToInt32(_dgv1.CurrentRow.Cells["fact"].Value.ToString());
                }
            }
            catch
            {
                begval = 0;
            }
        }

        private void SetComponents(String s1, String s2)
        {
            switch (s1)
            {

                case "HC":
                    #region HC
                    {
                        switch (s2)
                        {
                            case "AP":
                                {
                                    button1.Text = "HC";
                                    button2.Text = "BC";
                                    button3.Text = "CC";
                                    button4.Text = "CN";
                                    button5.Text = "ICU";
                                    //TODO: выключены кнопки в АП HC
                                    button1.Visible = true;
                                    button2.Visible = true;
                                    button3.Visible = true;
                                    button4.Visible = true;
                                    button5.Visible = true;
                                    button6.Visible = false;
                                    button7.Visible = false;
                                    button8.Visible = false;
                                    button9.Visible = false;
                                    button10.Visible = true;
                                    btnSave.Visible = true;

                                    label1.Visible = true;
                                    label2.Visible = true;
                                    label3.Visible = true;
                                    label4.Visible = true;
                                    label6.Visible = true;
                                    label7.Visible = true;
                                    label8.Visible = true;

                                    dateTimePicker1.Visible = true;
                                    dateTimePicker2.Visible = true;

                                    comboBox1.Visible = true;
                                    comboBox2.Visible = true;
                                    comboBox4.Visible = true;
                                    comboBox5.Visible = true;
                                    comboBox6.Visible = true;

                                    radioButton1.Visible = true;
                                    radioButton2.Visible = true;
                                    radioButton3.Visible = true;
                                    radioButton4.Visible = true;
                                    radioButton5.Visible = true;

                                    chbOnlyPS.Visible = true;

                                    break;
                                }
                            case "Dyn":
                                {
                                    button1.Visible = false;
                                    button2.Visible = false;
                                    button3.Visible = false;
                                    button4.Visible = false;
                                    button5.Visible = false;
                                    button6.Visible = false;
                                    button7.Visible = false;
                                    button8.Visible = false;
                                    button9.Visible = false;
                                    button10.Visible = false;
                                    btnSave.Visible = false;

                                    label1.Visible = false;
                                    label2.Visible = false;
                                    label3.Visible = false;
                                    label4.Visible = false;
                                    label6.Visible = false;
                                    label7.Visible = false;
                                    label8.Visible = false;

                                    dateTimePicker1.Visible = true;
                                    dateTimePicker2.Visible = false;

                                    comboBox1.Visible = false;
                                    comboBox2.Visible = false;

                                    comboBox4.Visible = false;
                                    comboBox5.Visible = false;
                                    comboBox6.Visible = false;

                                    radioButton1.Visible = false;
                                    radioButton2.Visible = false;
                                    radioButton3.Visible = false;
                                    radioButton4.Visible = false;
                                    radioButton5.Visible = false;

                                    chbOnlyPS.Visible = false;

                                    break;
                                }
                            case "Kos":
                                {
                                    button1.Visible = false;
                                    button2.Visible = false;
                                    button3.Visible = false;
                                    button4.Visible = false;
                                    button5.Visible = false;
                                    button6.Visible = false;
                                    button7.Visible = false;
                                    button8.Visible = false;
                                    button9.Visible = false;
                                    button10.Visible = false;
                                    btnSave.Visible = false;

                                    label1.Visible = false;
                                    label2.Visible = false;
                                    label3.Visible = false;
                                    label4.Visible = false;
                                    label6.Visible = false;
                                    label7.Visible = false;
                                    label8.Visible = false;

                                    dateTimePicker1.Visible = false;
                                    dateTimePicker2.Visible = false;

                                    comboBox1.Visible = false;
                                    comboBox2.Visible = false;
                                    comboBox4.Visible = false;
                                    comboBox5.Visible = false;
                                    comboBox6.Visible = false;
                                    cbUsers.Visible = false;

                                    radioButton1.Visible = false;
                                    radioButton2.Visible = false;
                                    radioButton3.Visible = false;
                                    radioButton4.Visible = false;
                                    radioButton5.Visible = false;

                                    chbOnlyPS.Visible = false;

                                    break;
                                }
                        }
                        break;
                    }
                    #endregion
                case "AE":
                    #region AE
                    {
                        switch (s2)
                        {
                            case "AP":
                                {
                                    button1.Text = "AE";
                                    button2.Text = "ST";
                                    button3.Text = "NE";
                                    button4.Text = "PS";
                                    button5.Text = "OT";
                                    button6.Text = "SP";
                                    button7.Text = "CT";
                                    button8.Text = "VS";
                                    button9.Text = "AM";
                                    //TODO: выключены кнопки в АП АЕ
                                    button1.Visible = true;
                                    button2.Visible = true;
                                    button3.Visible = true;
                                    button4.Visible = true;
                                    button5.Visible = true;
                                    button6.Visible = true;
                                    button7.Visible = true;
                                    button8.Visible = true;
                                    button9.Visible = true;
                                    button10.Visible = true;
                                    btnSave.Visible = true;

                                    label1.Visible = true;
                                    label2.Visible = true;
                                    label3.Visible = true;
                                    label4.Visible = true;
                                    label6.Visible = true;
                                    label7.Visible = true;
                                    label8.Visible = true;

                                    dateTimePicker1.Visible = true;
                                    dateTimePicker2.Visible = true;

                                    comboBox1.Visible = true;
                                    comboBox2.Visible = true;
                                    comboBox4.Visible = true;
                                    comboBox5.Visible = true;
                                    comboBox6.Visible = true;

                                    radioButton1.Visible = true;
                                    radioButton2.Visible = true;
                                    radioButton3.Visible = true;
                                    radioButton4.Visible = true;
                                    radioButton5.Visible = true;

                                    chbOnlyPS.Visible = true;

                                    break;
                                }
                            case "Dyn":
                                {
                                    button1.Visible = false;
                                    button2.Visible = false;
                                    button3.Visible = false;
                                    button4.Visible = false;
                                    button5.Visible = false;
                                    button6.Visible = false;
                                    button7.Visible = false;
                                    button8.Visible = false;
                                    button9.Visible = false;
                                    button10.Visible = false;
                                    btnSave.Visible = false;

                                    label1.Visible = false;
                                    label2.Visible = false;
                                    label3.Visible = false;
                                    label4.Visible = false;
                                    label6.Visible = false;
                                    label7.Visible = false;
                                    label8.Visible = false;

                                    dateTimePicker1.Visible = false;
                                    dateTimePicker2.Visible = false;

                                    comboBox1.Visible = false;
                                    comboBox2.Visible = false;
                                    comboBox4.Visible = false;
                                    comboBox5.Visible = false;
                                    comboBox6.Visible = false;
                                    cbUsers.Visible = false;

                                    radioButton1.Visible = false;
                                    radioButton2.Visible = false;
                                    radioButton3.Visible = false;
                                    radioButton4.Visible = false;
                                    radioButton5.Visible = false;

                                    chbOnlyPS.Visible = false;

                                    break;
                                }
                            case "Kos":
                                {
                                    break;
                                }
                        }
                        break;
                    }
                    #endregion
                case "OM":
                    #region OM
                    {
                        switch (s2)
                        {
                            case "AP":
                                {
                                    button1.Text = "OM";
                                    button2.Text = "Incontinence Care";
                                    button3.Text = "Stoma Care";
                                    button4.Text = "Wound Management";
                                    button5.Text = "Infection Control";
                                    //TODO: выключены кнопки в АП ОМ
                                    button1.Visible = true;
                                    button2.Visible = true;
                                    button3.Visible = true;
                                    button4.Visible = true;
                                    button5.Visible = true;
                                    button6.Visible = false;
                                    button7.Visible = false;
                                    button8.Visible = false;
                                    button9.Visible = false;
                                    button10.Visible = true;
                                    btnSave.Visible = true;
                                    //btnUpdAcc.Visible = false;

                                    label1.Visible = true;
                                    label2.Visible = true;
                                    label3.Visible = true;
                                    label4.Visible = true;
                                    label6.Visible = true;
                                    label7.Visible = true;
                                    label8.Visible = true;

                                    dateTimePicker1.Visible = true;
                                    dateTimePicker2.Visible = true;

                                    comboBox1.Visible = true;
                                    comboBox2.Visible = true;
                                    comboBox4.Visible = true;
                                    comboBox5.Visible = true;
                                    comboBox6.Visible = true;

                                    radioButton1.Visible = true;
                                    radioButton2.Visible = true;
                                    radioButton3.Visible = true;
                                    radioButton4.Visible = true;
                                    radioButton5.Visible = true;

                                    chbOnlyPS.Visible = true;

                                    break;
                                }
                            case "Dyn":
                                {
                                    button1.Visible = false;
                                    button2.Visible = false;
                                    button3.Visible = false;
                                    button4.Visible = false;
                                    button5.Visible = false;
                                    button6.Visible = false;
                                    button7.Visible = false;
                                    button8.Visible = false;
                                    button9.Visible = false;
                                    button10.Visible = false;
                                    btnSave.Visible = false;

                                    label1.Visible = false;
                                    label2.Visible = false;
                                    label3.Visible = false;
                                    label4.Visible = false;
                                    label6.Visible = false;
                                    label7.Visible = false;
                                    label8.Visible = false;

                                    dateTimePicker1.Visible = false;
                                    dateTimePicker2.Visible = false;

                                    comboBox1.Visible = false;
                                    comboBox2.Visible = false;
                                    comboBox4.Visible = false;
                                    comboBox5.Visible = false;
                                    comboBox6.Visible = false;
                                    cbUsers.Visible = false;

                                    radioButton1.Visible = false;
                                    radioButton2.Visible = false;
                                    radioButton3.Visible = false;
                                    radioButton4.Visible = false;
                                    radioButton5.Visible = false;

                                    chbOnlyPS.Visible = false;

                                    break;
                                }
                            case "Kos":
                                {
                                    break;
                                }
                        }
                        break;
                    }
                    #endregion
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ClearFilter();
            loadData();
        }

        private void btnDK1_Click(object sender, EventArgs e)
        {
            curbtn = 1;
            loadData();
        }

        private void btnDK2_Click(object sender, EventArgs e)
        {
            curbtn = 2;
            loadData();
        }

        private void btnDK3_Click(object sender, EventArgs e)
        {
            curbtn = 3;
            loadData();
        }

        private void btnDK4_Click(object sender, EventArgs e)
        {
            curbtn = 4;
            loadData();
        }

        private void btnDK5_Click(object sender, EventArgs e)
        {
            curbtn = 5;
            loadData();
        }
        private void btnDK6_Click(object sender, EventArgs e)
        {
            curbtn = 6;
            loadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            globalData.UserID2 = Convert.ToInt32(cbUsers3.SelectedValue);

            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            AddRowReport arr = new AddRowReport();
            arr.ShowDialog();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            EditRow(_dgv3, cbUsers3);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewCell cell = _dgv3.SelectedCells[0];

                Sql sql1 = new Sql();
                sql1.GetRecords("exec DelReportKos @p1, @p2, @p3", _dgv3.Rows[cell.RowIndex].Cells["rep_id"].Value.ToString(), _dgv3.Rows[cell.RowIndex].Cells["db_id"].Value.ToString(), globalData.UserID);

                loadData();
            }
            catch(Exception err)
            {
                MessageBox.Show("Не удалось удалить продажу. Системная ошибка - " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditRow(DataGridView dgv, ComboBox cbU)
        {
            try
            {
                Sql sql1 = new Sql();

                DataGridViewCell cell = dgv.SelectedCells[0];

                if (cbU != null)
                    globalData.UserID2 = Convert.ToInt32(cbU.SelectedValue);
                else
                {
                    globalData.UserID2 = Convert.ToInt32(sql1.GetRecordsOne("exec GetUserID @p1", dgv.Rows[cell.RowIndex].Cells["user_name"].Value));
                    globalData.Region = sql1.GetRecordsOne("exec GetRegionName @p1", dgv.Rows[cell.RowIndex].Cells["subreg_nameRus"].Value);
                }

                AddRowReport arr = new AddRowReport(dgv.Rows[cell.RowIndex].Cells["rep_id"].Value.ToString(), dgv.Rows[cell.RowIndex].Cells["db_id"].Value.ToString(), cbUsers3.Visible);
                arr.ShowDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось войти в режим редактирования. Системная ошибка - " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbLPU_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                string lpu = "0", user = "0";
                  
                lpu = cbLPU.SelectedValue.ToString();
                if (cbUsers2.Visible)
                    user = cbUsers2.SelectedValue.ToString();

                SelAcc(cbYearAcc.SelectedItem.ToString(), lpu, user, globalData.Region, "", _dgv4);
            }
        }

        private void удалитьСторныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            InputDialog ind = new InputDialog("Год", "Введите год", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;

            String year = globalData.input;

            InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
            id.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;

            string month = globalData.input;

            string date = year + "-" + month + "-01";

            sql1.GetRecords("exec DelStorn @p1", date);
            MessageBox.Show("Сторны удалены.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users us = new Users();
            us.ShowDialog();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (button20.Text == "Общая")
            {
                button20.Text = "По регионам";
                label14.Visible = false;
                cbRegions.Visible = false;
                if ((globalData.RD == String.Empty) || (globalData.RD == null))
                    loadDynAll();
                else
                    selDynRD();
            }
            else
            {
                button20.Text = "Общая";
                label14.Visible = true;
                cbRegions.Visible = true;
                loadDyn(cbRegions.SelectedValue.ToString());
            }
        }

        private void cbRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                loadDyn(cbRegions.SelectedValue.ToString());
            }
        }

        private void loadDynAll()
        {
            if (button20.Text == "Общая")
            {
                label14.Visible = true;
                cbRegions.Visible = true;
            }
            else
            {
                label14.Visible = false;
                cbRegions.Visible = false;
            }

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec SelAllDyn @p1, @p2", globalData.Div, cbYearDyn.SelectedItem.ToString());
            _dgv5.Columns.Clear();
            _dgv5.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Ошибка при подсчёте динамики продаж.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            formatDyn();
        }

        private void loadDyn(String regID)
        {
            tabControl1.SelectedIndex = 4;
            tabControl1.Visible = true;

            Sql sql1 = new Sql();
            _dgv5.DataSource = null;
            _dgv5.Columns.Clear();
            DataTable dt1 = new DataTable();
            if (globalData.Region == null)
                globalData.Region = String.Empty;
            dt1 = sql1.GetRecords("exec SelDyn @p1, @p2, @p3, @p4", globalData.Region, regID, globalData.Div, cbYearDyn.SelectedItem.ToString());
            _dgv5.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные для построения динамики продаж", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            formatDyn();
        }

        private void formatDyn()
        {
            _dgv5.Columns["t_id"].Visible = false;

            /*
            _dgv5.Columns["pm8"].Visible = true;
            _dgv5.Columns["pm9"].Visible = false;
            _dgv5.Columns["pm10"].Visible = false;
            _dgv5.Columns["pm11"].Visible = false;
            _dgv5.Columns["pm12"].Visible = false;
            */

            _dgv5.Columns["pm8"].Visible = false;
            _dgv5.Columns["pm9"].Visible = false;
            _dgv5.Columns["pm10"].Visible = false;
            _dgv5.Columns["pm11"].Visible = false;
            _dgv5.Columns["pm12"].Visible = false;

            _dgv5.Columns["pm8"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm9"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm10"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm11"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm12"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm1"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm2"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm3"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm4"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm5"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm6"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm7"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm8"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm9"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm10"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm11"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm12"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["total"].DefaultCellStyle.Format = "N3";

            _dgv5.Columns["pm8R"].Visible = false;
            _dgv5.Columns["pm9R"].Visible = false;
            _dgv5.Columns["pm10R"].Visible = false;
            _dgv5.Columns["pm11R"].Visible = false;
            _dgv5.Columns["pm12R"].Visible = false;

            _dgv5.Columns["pm8R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm9R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm10R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm11R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["pm12R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm1R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm2R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm3R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm4R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm5R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm6R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm7R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm8R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm9R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm10R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm11R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["cm12R"].DefaultCellStyle.Format = "N3";
            _dgv5.Columns["totalR"].DefaultCellStyle.Format = "N3";

            _dgv5.Columns["pm8"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm9"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm10"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm11"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm4"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm5"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm6"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm7"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm8"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm9"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm10"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm11"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm12"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            _dgv5.Columns["pm8R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm9R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm10R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm11R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["pm12R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm1R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm2R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm3R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm4R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm5R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm6R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm7R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm8R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm9R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm10R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm11R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["cm12R"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            _dgv5.Columns["totalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            _dgv5.Columns["name"].HeaderText = "";
            _dgv5.Columns["pm8"].HeaderText = "08-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm9"].HeaderText = "09-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm10"].HeaderText = "10-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm11"].HeaderText = "11-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm12"].HeaderText = "12-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["cm1"].HeaderText = "01-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm2"].HeaderText = "02-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm3"].HeaderText = "03-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm4"].HeaderText = "04-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm5"].HeaderText = "05-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm6"].HeaderText = "06-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm7"].HeaderText = "07-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm8"].HeaderText = "08-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm9"].HeaderText = "09-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm10"].HeaderText = "10-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm11"].HeaderText = "11-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm12"].HeaderText = "12-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["total"].HeaderText = "Итого";

            _dgv5.Columns["pm8R"].HeaderText = "08-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm9R"].HeaderText = "09-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm10R"].HeaderText = "10-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm11R"].HeaderText = "11-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["pm12R"].HeaderText = "12-" + (Convert.ToInt32(cbYearDyn.SelectedItem) - 1).ToString();
            _dgv5.Columns["cm1R"].HeaderText = "01-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm2R"].HeaderText = "02-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm3R"].HeaderText = "03-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm4R"].HeaderText = "04-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm5R"].HeaderText = "05-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm6R"].HeaderText = "06-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm7R"].HeaderText = "07-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm8R"].HeaderText = "08-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm9R"].HeaderText = "09-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm10R"].HeaderText = "10-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm11R"].HeaderText = "11-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["cm12R"].HeaderText = "12-" + cbYearDyn.SelectedItem.ToString();
            _dgv5.Columns["totalR"].HeaderText = "Итого";


            _dgv5.Columns["name"].Frozen = true;
            _dgv5.Columns["name"].Width = 240;
            _dgv5.Columns["name"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            foreach (DataGridViewColumn col in _dgv5.Columns)
                col.SortMode = DataGridViewColumnSortMode.Programmatic;

            _dgv5.Rows[0].Height = 30;
            _dgv5.Rows[1].Height = 30;
            _dgv5.Rows[3].Height = 30;
            _dgv5.Rows[0].DefaultCellStyle.BackColor = bbgreen3;
            _dgv5.Rows[0].DefaultCellStyle.SelectionBackColor = bbgray4;
            _dgv5.Rows[1].DefaultCellStyle.BackColor = bbgreen3;
            _dgv5.Rows[1].DefaultCellStyle.SelectionBackColor = bbgray4;

            int i = 0;
            bool b = false;
            if (globalData.Div == "HC")
            {
                _dgv5.Rows[8].DefaultCellStyle.BackColor = bbgreen3;

                foreach (DataGridViewRow row in _dgv5.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "11")
                        b = true;
                    if (b)
                        _dgv5.Rows[i].DefaultCellStyle.BackColor = bbgray5;
                    i++;
                }
            }
            if (globalData.Div == "AE")
            {
                _dgv5.Rows[7].DefaultCellStyle.BackColor = bbgreen3;
                /*
                foreach (DataGridViewRow row in _dgv5.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "10")
                        b = true;
                    if (b)
                        _dgv5.Rows[i].DefaultCellStyle.BackColor = bbgray5;
                    i++;
                }
                */
            }
            if (globalData.Div == "OM")
            {
                if (_dgv5.Rows[8].Cells[1].Value.ToString() == "Личные продажи:")
                    _dgv5.Rows[8].DefaultCellStyle.BackColor = bbgreen3;
                else
                    _dgv5.Rows[7].DefaultCellStyle.BackColor = bbgreen3;
                
            }

            visiblePrivSales(_dgv5, radioButton15, radioButton14);
        }

        private void dataGridView6_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > 6)
                {
                    globalData.Region = cbRegions.Text;
                    privateSales pS = new privateSales(_dgv5.SelectedRows[0].Cells[1].Value.ToString(), "AP");
                    pS.Show();
                }
            }
            catch
            {
            }
        }

        /*
        private void loadAccPlanByLPU()
        {
            if (cbLPU.SelectedValue != null)
            {
                clsSql sql1 = new clsSql();
                //_dgv4.Columns.Clear();
                DataTable dt1 = new DataTable();

                dt1 = sql1.GetRecords("exec SelAccNew @p1, @p2, @p3, @p4, 0", clsData.Div, cbYearAcc.SelectedItem.ToString(), cbLPU.SelectedValue, cbUsers2.SelectedValue);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAcc(dt1, _dgv4);
            }
        }
        */

        private void fillAcc(DataTable dt, DataGridView dgv)
        {
            fillTableHeader("Acc", dgv);

            int i = 0;
            Double[] prVC = new Double[8];

            foreach (DataRow row in dt.Rows)
            {
                dgv.Rows.Add(row.ItemArray);

                if (dgv.Rows[i].Cells["vc_sum1"].Value.ToString() != "")
                    prVC[0] = Convert.ToDouble(dgv.Rows[i].Cells["vc_sum1"].Value);
                else
                    prVC[0] = 0;

                if (dgv.Rows[i].Cells["vc_sum2"].Value.ToString() != "")
                    prVC[1] = Convert.ToDouble(dgv.Rows[i].Cells["vc_sum2"].Value);
                else
                    prVC[1] = 0;

                if (dgv.Rows[i].Cells["vc_sum3"].Value.ToString() != "")
                    prVC[2] = Convert.ToDouble(dgv.Rows[i].Cells["vc_sum3"].Value);
                else
                    prVC[2] = 0;

                if (dgv.Rows[i].Cells["vc_sum4"].Value.ToString() != "")
                    prVC[3] = Convert.ToDouble(dgv.Rows[i].Cells["vc_sum4"].Value);
                else
                    prVC[3] = 0;
                /****************************************************************/
                if (dgv.Rows[i].Cells["h1Sales"].Value.ToString() != "")
                    prVC[4] = Convert.ToDouble(dgv.Rows[i].Cells["h1Sales"].Value);
                else
                    prVC[4] = 0;

                if (dgv.Rows[i].Cells["h2Sales"].Value.ToString() != "")
                    prVC[5] = Convert.ToDouble(dgv.Rows[i].Cells["h2Sales"].Value);
                else
                    prVC[5] = 0;

                if (dgv.Rows[i].Cells["h3Sales"].Value.ToString() != "")
                    prVC[6] = Convert.ToDouble(dgv.Rows[i].Cells["h3Sales"].Value);
                else
                    prVC[6] = 0;

                if (dgv.Rows[i].Cells["h4Sales"].Value.ToString() != "")
                    prVC[7] = Convert.ToDouble(dgv.Rows[i].Cells["h4Sales"].Value);
                else
                    prVC[7] = 0;


                if (((prVC[0] + prVC[1] + prVC[2] + prVC[3]) != 0) && ((prVC[4] + prVC[5] + prVC[6] + prVC[7]) != 0))
                {
                    dgv.Rows[i].Cells["prVC"].Value = Math.Round(((prVC[0] + prVC[1] + prVC[2] + prVC[3]) / (prVC[4] + prVC[5] + prVC[6] + prVC[7]) * 100), 2).ToString() + "%";
                }
                else
                {
                    dgv.Rows[i].Cells["prVC"].Value = "0%";
                }
                i++;
            }
            formatAcc(dgv);
            
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage7"])
                visibleAcc(dgv, radioButton11, radioButton10);
            else
                visibleAcc(dgv, rbRub, rbEuro);
        }

        private void formatAcc(DataGridView dgv)
        {
            //-------------------------------------------------------------------
            dgv.Columns["acc_id"].Visible = false;
            dgv.Columns["nom_id"].Visible = false;
            dgv.Columns["nom_group"].Visible = false;
            dgv.Columns["vc_id1"].Visible = false;
            dgv.Columns["vc_id2"].Visible = false;
            dgv.Columns["upd"].Visible = false;
            dgv.Columns["upd2"].Visible = false;
            dgv.Columns["vc_id3"].Visible = false;
            dgv.Columns["vc_id4"].Visible = false;
            dgv.Columns["upd3"].Visible = false;
            dgv.Columns["upd4"].Visible = false;

            if (globalData.UserID == 258)
            {
                //dgv.Columns["nom_id"].Visible = true;
                //dgv.Columns["nom_group"].Visible = true;
                //dgv.Columns["vc_id1"].Visible = true;
                //dgv.Columns["vc_id2"].Visible = true;
                //dgv.Columns["upd"].Visible = true;
                //dgv.Columns["vc_id3"].Visible = true;
                //dgv.Columns["vc_id4"].Visible = true;
                //dgv.Columns["upd2"].Visible = true;
                //dgv.Columns["upd3"].Visible = true;
                //dgv.Columns["upd4"].Visible = true;
            }
            //убрать, если понадобится ассортиментный план за текущий год -2 и -3
            dgv.Columns["py3"].Visible = false;
            
            dgv.Columns["py1"].Visible = true;

            //dgv.Columns["vc_id1"].Visible = false;
            //dgv.Columns["vc_id2"].Visible = false;

            //-------------------------------------------------------------------
            if (globalData.Div == "HC")
            {
                dgv.Columns["py2"].Visible = true;

                dgv.Columns["nom_type"].Visible = true;
                dgv.Columns["dilcost"].Visible = true;
                dgv.Columns["dilcostRub"].Visible = true;
                dgv.Columns["cyplan"].Visible = true;
                dgv.Columns["cyfact"].Visible = true;
                dgv.Columns["pr"].Visible = true;

                dgv.Columns["nom_type"].Width = 40;
                dgv.Columns["nom_type"].Frozen = true;

                dgv.Columns["dilcost"].Width = 65;
                dgv.Columns["dilcostRub"].Width = 65;

                dgv.Columns["cyfactEuro"].DefaultCellStyle.BackColor = bbgray5;
                dgv.Columns["cyfactRub"].DefaultCellStyle.BackColor = bbgray5;

                dgv.Columns["cyplanEuro"].DefaultCellStyle.Format = "N2";
                dgv.Columns["cyfactEuro"].DefaultCellStyle.Format = "N2";

                dgv.Columns["cyplanRub"].DefaultCellStyle.Format = "N2";
                dgv.Columns["cyfactRub"].DefaultCellStyle.Format = "N2";

                dgv.Columns["dilcostRub"].DefaultCellStyle.Format = "N2";

                dgv.Columns["nom_type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["dilcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["dilcostRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgv.Columns["cyfact"].DefaultCellStyle.BackColor = bbgray5;

                dgv.Columns["pr"].DefaultCellStyle.Format = "N2";
                //dgv.Columns["prVC"].DefaultCellStyle.Format = "N2";

                dgv.Columns["JanFact"].HeaderText = "Январь\nфакт, ед.";
                dgv.Columns["FebFact"].HeaderText = "Февраль\nфакт, ед.";
                dgv.Columns["MartFact"].HeaderText = "Март\nфакт, ед.";
                dgv.Columns["AprFact"].HeaderText = "Апрель\nфакт, ед.";
                dgv.Columns["MayFact"].HeaderText = "Май\nфакт, ед.";
                dgv.Columns["JuneFact"].HeaderText = "Июнь\nфакт, ед.";
                dgv.Columns["JuleFact"].HeaderText = "Июль\nфакт, ед.";
                dgv.Columns["AugFact"].HeaderText = "Август\nфакт, ед.";
                dgv.Columns["SepFact"].HeaderText = "Сентябрь\nфакт, ед.";
                dgv.Columns["OktFact"].HeaderText = "Октябрь\nфакт, ед.";
                dgv.Columns["NovFact"].HeaderText = "Ноябрь\nфакт, ед.";
                dgv.Columns["DecFact"].HeaderText = "Декабрь\nфакт, ед.";

                dgv.Columns["JanFactR"].HeaderText = "Январь\nфакт, ед.";
                dgv.Columns["FebFactR"].HeaderText = "Февраль\nфакт, ед.";
                dgv.Columns["MartFactR"].HeaderText = "Март\nфакт, ед.";
                dgv.Columns["AprFactR"].HeaderText = "Апрель\nфакт, ед.";
                dgv.Columns["MayFactR"].HeaderText = "Май\nфакт, ед.";
                dgv.Columns["JuneFactR"].HeaderText = "Июнь\nфакт, ед.";
                dgv.Columns["JuleFactR"].HeaderText = "Июль\nфакт, ед.";
                dgv.Columns["AugFactR"].HeaderText = "Август\nфакт, ед.";
                dgv.Columns["SepFactR"].HeaderText = "Сентябрь\nфакт, ед.";
                dgv.Columns["OktFactR"].HeaderText = "Октябрь\nфакт, ед.";
                dgv.Columns["NovFactR"].HeaderText = "Ноябрь\nфакт, ед.";
                dgv.Columns["DecFactR"].HeaderText = "Декабрь\nфакт, ед.";
            }
            else
            {
                dgv.Columns["py2"].Visible = false;

                dgv.Columns["nom_type"].Visible = false;
                dgv.Columns["dilcost"].Visible = false;
                dgv.Columns["dilcostRub"].Visible = false;
                dgv.Columns["cyplan"].Visible = false;
                dgv.Columns["cyfact"].Visible = false;
                dgv.Columns["pr"].Visible = false;

                dgv.Columns["JanFact"].HeaderText = "Январь\nфакт, EUR";
                dgv.Columns["FebFact"].HeaderText = "Февраль\nфакт, EUR";
                dgv.Columns["MartFact"].HeaderText = "Март\nфакт, EUR";
                dgv.Columns["AprFact"].HeaderText = "Апрель\nфакт, EUR";
                dgv.Columns["MayFact"].HeaderText = "Май\nфакт, EUR";
                dgv.Columns["JuneFact"].HeaderText = "Июнь\nфакт, EUR";
                dgv.Columns["JuleFact"].HeaderText = "Июль\nфакт, EUR";
                dgv.Columns["AugFact"].HeaderText = "Август\nфакт, EUR";
                dgv.Columns["SepFact"].HeaderText = "Сентябрь\nфакт, EUR";
                dgv.Columns["OktFact"].HeaderText = "Октябрь\nфакт, EUR";
                dgv.Columns["NovFact"].HeaderText = "Ноябрь\nфакт, EUR";
                dgv.Columns["DecFact"].HeaderText = "Декабрь\nфакт, EUR";

                dgv.Columns["JanFactR"].HeaderText = "Январь\nфакт, РУБ.";
                dgv.Columns["FebFactR"].HeaderText = "Февраль\nфакт, РУБ.";
                dgv.Columns["MartFactR"].HeaderText = "Март\nфакт, РУБ.";
                dgv.Columns["AprFactR"].HeaderText = "Апрель\nфакт, РУБ.";
                dgv.Columns["MayFactR"].HeaderText = "Май\nфакт, РУБ.";
                dgv.Columns["JuneFactR"].HeaderText = "Июнь\nфакт, РУБ";
                dgv.Columns["JuleFactR"].HeaderText = "Июль\nфакт, РУБ";
                dgv.Columns["AugFactR"].HeaderText = "Август\nфакт, РУБ.";
                dgv.Columns["SepFactR"].HeaderText = "Сентябрь\nфакт, РУБ.";
                dgv.Columns["OktFactR"].HeaderText = "Октябрь\nфакт, РУБ.";
                dgv.Columns["NovFactR"].HeaderText = "Ноябрь\nфакт, РУБ.";
                dgv.Columns["DecFactR"].HeaderText = "Декабрь\nфакт, РУБ.";

                dgv.Columns["cyplanEuro"].DefaultCellStyle.BackColor = bbgray5;
                dgv.Columns["cyfactEuro"].DefaultCellStyle.BackColor = bbgray5;

                dgv.Columns["cyplanRub"].DefaultCellStyle.BackColor = bbgray5;
                dgv.Columns["cyfactRub"].DefaultCellStyle.BackColor = bbgray5;
            }
            
            dgv.Columns["JanFact"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["MartFact"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["MayFact"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["JuleFact"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["SepFact"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["NovFact"].DefaultCellStyle.BackColor = bbgray5;

            dgv.Columns["JanFactR"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["MartFactR"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["MayFactR"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["JuleFactR"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["SepFactR"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["NovFactR"].DefaultCellStyle.BackColor = bbgray5;


            dgv.Columns["vc_sum1"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["vc_sum2"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["vc_sum3"].DefaultCellStyle.BackColor = bbgray5;
            dgv.Columns["vc_sum4"].DefaultCellStyle.BackColor = bbgray5;

            dgv.Columns["py3"].DefaultCellStyle.Format = "N2";
            dgv.Columns["py2"].DefaultCellStyle.Format = "N0";
            dgv.Columns["py1"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cyplan"].DefaultCellStyle.Format = "N0";
            dgv.Columns["cyfact"].DefaultCellStyle.Format = "N0";
            dgv.Columns["cyplanEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cyfactEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cyplanRub"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cyfactRub"].DefaultCellStyle.Format = "N2";

            dgv.Columns["prEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["prRub"].DefaultCellStyle.Format = "N2";

            dgv.Columns["JanFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["FebFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["MartFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["AprFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["MayFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["JuneFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["JuleFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["AugFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["SepFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["OktFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["NovFact"].DefaultCellStyle.Format = "N2";
            dgv.Columns["DecFact"].DefaultCellStyle.Format = "N2";

            dgv.Columns["JanFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["FebFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["MartFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["AprFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["MayFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["JuneFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["JuleFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["AugFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["SepFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["OktFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["NovFactR"].DefaultCellStyle.Format = "N2";
            dgv.Columns["DecFactR"].DefaultCellStyle.Format = "N2";

            dgv.Columns["h1Sales"].DefaultCellStyle.Format = "N2";
            dgv.Columns["h2Sales"].DefaultCellStyle.Format = "N2";
            dgv.Columns["h3Sales"].DefaultCellStyle.Format = "N2";
            dgv.Columns["h4Sales"].DefaultCellStyle.Format = "N2";

            dgv.Columns["vc_sum1"].DefaultCellStyle.Format = "N2";
            dgv.Columns["vc_sum2"].DefaultCellStyle.Format = "N2";
            dgv.Columns["vc_sum3"].DefaultCellStyle.Format = "N2";
            dgv.Columns["vc_sum4"].DefaultCellStyle.Format = "N2";

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            
            dgv.Columns["nom"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns["nom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.Columns["nom"].Width = 250;
            dgv.Columns["nom"].Frozen = true;

            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == 0)
                {
                    dgv.Rows[row.Index].ReadOnly = true;
                    dgv.Rows[row.Index].DefaultCellStyle.BackColor = bbgreen3;
                    dgv.Rows[row.Index].DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == -1)
                {
                    dgv.Rows[row.Index].ReadOnly = true;
                    dgv.Rows[row.Index].DefaultCellStyle.BackColor = bbgreen1;
                    dgv.Rows[row.Index].DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_id"].Value) == 3)
                {
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = bbgreen3;
                }

                if ((Convert.ToInt32(row.Cells["nom_id"].Value) == 11) || (Convert.ToInt32(row.Cells["nom_id"].Value) == 156))
                {
                    dgv.Rows[row.Index].ReadOnly = false;
                }
            }

            dgv.Columns["div"].Width = 2;
            dgv.Columns["div"].DefaultCellStyle.BackColor = bbgray4;
            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells["div"].Style.BackColor = bbgray4;
            }

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.ReadOnly = true;
            }

            if (cbLPU.Visible == false)
            {
                dgv.Columns["vc_sum1"].ReadOnly = true;
                dgv.Columns["vc_sum2"].ReadOnly = true;
                dgv.Columns["vc_sum3"].ReadOnly = true;
                dgv.Columns["vc_sum4"].ReadOnly = true;
            }
            else
            {
                dgv.Columns["vc_sum1"].ReadOnly = false;
                dgv.Columns["vc_sum2"].ReadOnly = false;
                dgv.Columns["vc_sum3"].ReadOnly = false;
                dgv.Columns["vc_sum4"].ReadOnly = false;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (btnHideLPU.Text == "Скрыть ЛПУ")
            {
                btnHideLPU.Text = "Показать ЛПУ";
                cbLPU.Visible = false;
            }
            else
            {
                btnHideLPU.Text = "Скрыть ЛПУ";
                cbLPU.Visible = true;
                
            }
            SelRegAcc();
        }

        /*
        private void hideLPUAcc()
        {
            if (btnHideLPU.Text == "Показать ЛПУ")
            {
                clsSql sql1 = new clsSql();
                DataTable dt1 = new DataTable();

                dt1 = sql1.GetRecords("exec SelAccNew @p1, @p2, 0, @p3, @p4", clsData.Div, cbYearAcc.SelectedItem.ToString(), cbUsers2.SelectedValue, clsData.Region);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                fillAcc(dt1, _dgv4);
            }
            else
            {
                fillLPU(cbLPU, cbUsers2.SelectedValue.ToString(), clsData.Region, Convert.ToInt32(cbYearAcc.SelectedItem.ToString()));
                loadAccPlanByLPU();
            }
        }
        */

        private void dataGridView7_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //privateSales pS = new privateSales(_dgv6.SelectedRows[0].Cells[0].Value.ToString(), "Acc");
            //pS.Show();
        }

        private void btnUpdAcc_Click(object sender, EventArgs e)
        {
            if (prBar1.Visible == true)
            {
                MessageBox.Show("Ассортиментный план обновляется, не мешайте программе работать.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            int userID;

            if (tn1.Text == "Анализ продаж")
                userID = Convert.ToInt32(cbUsers.SelectedValue.ToString());
            else
                userID = Convert.ToInt32(cbUsers2.SelectedValue.ToString());

            if ((globalData.Div == "HC") || (globalData.Div == "AE"))
            {
                if (globalData.UserAccess == 1)
                {
                    InputDialog ind = new InputDialog("Месяц", "Введите номер месяца в формате 1,2,3...", true);
                    ind.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    if ((Convert.ToInt32(globalData.input) < 1) || (Convert.ToInt32(globalData.input) > 12))
                    {
                        MessageBox.Show("Месяц введён неверно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                        globalData.CurDate = Convert.ToDateTime(cbYearAcc.SelectedItem.ToString() + "-" + globalData.input.ToString() + "-01");
                }

                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec SelLPU @p1, @p2, @p3, @p4", userID, globalData.Region, globalData.Div, globalData.CurDate.Year);

                prBar1.Visible = true;
                
                prBar1.Value = 0;
                prBar1.Maximum = dt1.Rows.Count;

                if (globalData.UserID != 1)
                {
                    prBar1.Visible = false;

                    Cursor = Cursors.WaitCursor;

                    foreach (DataRow row in dt1.Rows)
                    {
                        sql1.GetRecords("exec fillFactAcc @p1, @p2, @p3", row[0].ToString(), globalData.Div, globalData.CurDate);
                    }                    

                    loadData();

                    Cursor = Cursors.Default;

                    MessageBox.Show("Ассортиментный план обновлён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
                
                bgw = new BackgroundWorker[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    bgw[i] = new BackgroundWorker();
                    bgw[i].DoWork += backgroundWorker_DoWork;
                    bgw[i].RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                    bgw[i].WorkerSupportsCancellation = true;
                }
                int j = 0;

                foreach (DataRow row in dt1.Rows)
                {
                    /*if (j != 0)
                    {
                        if (bgw[j - 1].IsBusy)
                        {
                     * */
                            bgw[j].RunWorkerAsync(row[0].ToString());
                            j++;
                        //}
                    //}
                }
            }
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings set1 = new Settings();
            set1.ShowDialog();
        }

        private void косвенныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KosCust ks = new KosCust();
            ks.ShowDialog();
        }

        private void _dgv3_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (((e.ColumnIndex > 1) && (e.ColumnIndex < 11)) || (e.ColumnIndex == 14))
                subsumKosSales(e.ColumnIndex, _dgv3, 11);
        }

        private void subsumKosSales(int colnum, DataGridView dgv, int begin)
        {
            Double sum = 0, sum2 = 0, sum3 = 0;
            Double msum = 0, msum2 = 0, msum3 = 0;

            for (int j = 0; j < dgv.Rows.Count; j++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row = dgv.Rows[j];
                if ((row.DefaultCellStyle.BackColor == bbgreen1) || (row.DefaultCellStyle.BackColor == bbgreen3))
                {
                    dgv.Rows.Remove(row);
                    j--;
                }
            }

            if ((subsumKS == colnum) || (dgv.Rows.Count == 0))
            {
                subsumKS = 0;
                return;
            }

            dgv.Sort(dgv.Columns[colnum], ListSortDirection.Ascending);
            String s1 = dgv.Rows[0].Cells[colnum].Value.ToString();

            int i = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if ((row.Cells[colnum].Value.ToString() != s1) && (s1 != ""))
                {
                    s1 = dgv.Rows[i].Cells[colnum].Value.ToString();
                    if (begin == 11)
                        dgv.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                    else
                        dgv.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                    dgv.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                    msum += sum;
                    msum2 += sum2;
                    msum3 += sum3;
                    sum = 0;
                    sum2 = 0;
                    sum3 = 0;
                }
                else if (s1 != "")
                {
                    if (row.Cells[begin].Value.ToString() != String.Empty)
                        sum += Convert.ToDouble(row.Cells[begin].Value.ToString());
                    if (row.Cells[begin + 1].Value.ToString() != String.Empty)
                        sum2 += Convert.ToDouble(row.Cells[begin + 1].Value.ToString());
                    if (row.Cells[begin + 2].Value.ToString() != String.Empty)
                        sum3 += Convert.ToDouble(row.Cells[begin + 2].Value.ToString());
                }
                i++;
            }
            msum += sum;
            msum2 += sum2;
            msum3 += sum3;

            if (begin == 11)
            {
                dgv.Rows.Add("", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                dgv.Rows.Add("", "", "", "", "", "", "", "", "", "", "", msum, msum2, msum3);
            }
            else
            {
                dgv.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3);
                dgv.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", msum, msum2, msum3);
            }

            dgv.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
            dgv.Rows[i + 1].DefaultCellStyle.BackColor = bbgreen1;
            subsumKS = colnum;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                loadData();
                globalData.update = false;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                loadData();
        }

        private void cbYearAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                Sql sql1 = new Sql();

                DataTable dt1 = new DataTable();

                if (globalData.UserAccess == 5)
                {
                    bool all = false;

                    TreeNode tn1 = new TreeNode();
                    tn1 = treeView1.SelectedNode.Parent.Parent.Parent;

                    if (tn1.Text == "Все регионы")
                        all = true;

                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, cbYearAcc.SelectedItem);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, cbYearAcc.SelectedItem);
                cbUsers2.DataSource = dt1;
                cbUsers2.DisplayMember = "user_name";
                cbUsers2.ValueMember = "user_id";
            }
        }

        private void cbUsers2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                loadData();
        }

        private void fillUsers()
        {
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            bool all = false;

            if (globalData.Region == String.Empty)
                globalData.Region = tn1.Parent.Parent.Parent.Text;

            if (tn1.Parent.Parent.Parent == null)
            {
                loadusers = true;
                fillFilter = true;
            }
            else if (tn1.Parent.Parent.Parent.Text == "Мои регионы")
            {
                if (globalData.Region != tn1.Parent.Parent.Text)
                {
                    globalData.Region = tn1.Parent.Parent.Text;
                    loadusers = true;
                    fillFilter = true;
                }
                all = false;
            }
            else if (tn1.Parent.Parent.Parent.Text == "Все регионы")
            {
                if (globalData.Region != tn1.Parent.Parent.Text)
                {
                    globalData.Region = tn1.Parent.Parent.Text;
                    loadusers = true;
                    fillFilter = true;
                }
                all = true;
            }
            else if (globalData.Region != tn1.Parent.Parent.Parent.Text)
            {
                globalData.Region = tn1.Parent.Parent.Parent.Text;
                loadusers = true;
                fillFilter = true;

                if (tn1.Parent.Parent.Parent.Parent != null)
                {
                    if (tn1.Parent.Parent.Parent.Parent.Text == "Все регионы")
                        all = true;
                    else
                        all = false;
                }
            }

            if (globalData.Div != tn1.Text)
            {
                globalData.Div = tn1.Text;
                loadusers = true;
                fillFilter = true;
            }

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            if (loadusers)
            {
                if (globalData.UserAccess == 5)
                {
                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                {
                    if (globalData.Region == "Российская федерация")
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                    else
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                }
                load = false;
                if (dt1 != null)
                {
                    cbUsers.DataSource = dt1;
                    cbUsers.DisplayMember = "user_name";
                    cbUsers.ValueMember = "user_id";
                }

                if (globalData.UserAccess == 5)
                {
                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, cbYearAcc.SelectedItem);
                if (dt1 != null)
                {
                    cbUsers2.DataSource = dt1;
                    cbUsers2.DisplayMember = "user_name";
                    cbUsers2.ValueMember = "user_id";
                }

                if (globalData.UserAccess == 5)
                {
                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker3.Value.Year);
                if (dt1 != null)
                {
                    cbUsers3.DataSource = dt1;
                    cbUsers3.DisplayMember = "user_name";
                    cbUsers3.ValueMember = "user_id";
                }

                if (globalData.UserAccess == 5)
                {
                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, cbYearMA.SelectedItem);
                if (dt1 != null)
                {
                    cbUsersMA.DataSource = dt1;
                    cbUsersMA.DisplayMember = "user_name";
                    cbUsersMA.ValueMember = "user_id";
                }

                if (globalData.UserAccess == 5)
                {
                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, cbYearAccAll.SelectedItem);
                if (dt1 != null)
                {
                    cbUsersAcc.DataSource = dt1;
                    cbUsersAcc.DisplayMember = "user_name";
                    cbUsersAcc.ValueMember = "user_id";
                }

                if (globalData.UserAccess == 5)
                {
                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                {
                    DataTable dtNY = new DataTable();
                    dtNY = sql1.GetRecords("exec GetSettings");

                    string year = dtNY.Rows[0].ItemArray[3].ToString();

                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, year);
                }
                if (dt1 != null)
                {
                    cbUsersAccNY.DataSource = dt1;
                    cbUsersAccNY.DisplayMember = "user_name";
                    cbUsersAccNY.ValueMember = "user_id";
                }
                loadusers = false;
                load = true;
            }
        }

        private void fillUsersAcc(ComboBox cbU, string reg, string year)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec SelUsersForAllAcc @p1, @p2, @p3, @p4", reg, globalData.Div, year, globalData.RD);

            load = false;
            if (dt1 != null)
            {
                cbU.DataSource = dt1;
                cbU.DisplayMember = "user_name";
                cbU.ValueMember = "user_id";
            }
            load = true;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                deleteRepPS(_dgv1);
            }
        }

        private void SelectAP()
        {
            EnableSave("ap");

            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode.Parent.Parent.Parent.Parent;

            TreeNode tn2 = new TreeNode();
            tn2 = treeView1.SelectedNode.Parent.Parent.Parent;

            if ((tn1.Text == "Мои регионы") && (tn2.Text == "Российская федерация"))
            {
                if ((!radioButton4.Checked) && (!radioButton6.Checked))
                    radioButton4.Checked = true;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                radioButton3.Enabled = false;
                radioButton5.Enabled = false;
                //radioButton6.Visible = true;
                radioButton6.Visible = false;
                chbOnlyPS.Checked = false;
                chbOnlyPS.Enabled = false;
                
                dateTimePicker1.MinDate = Convert.ToDateTime("2014-01-01");
                dateTimePicker2.MinDate = Convert.ToDateTime("2014-01-01");
            }
            else
            {
                if (radioButton6.Checked)
                    radioButton5.Checked = true;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton5.Enabled = true;
                radioButton6.Visible = false;
                chbOnlyPS.Enabled = true;

                if ((chbOnlyPS.Checked) || (!radioButton5.Checked))
                {
                    dateTimePicker1.MinDate = Convert.ToDateTime("2012-01-01");
                    dateTimePicker2.MinDate = Convert.ToDateTime("2012-01-01");
                }
                else
                {
                    dateTimePicker1.MinDate = globalData.CurDate;
                    dateTimePicker2.MinDate = globalData.CurDate;
                }
            }

            if (globalData.UserAccess == 1)
            {
                btnMove.Visible = true;
                btnEditReport.Visible = true;
            }
            else
            {
                if ((tn1.Text == "Мои регионы") && (tn2.Text == "Российская федерация"))
                {
                    btnMove.Visible = true;
                    btnEditReport.Visible = false;
                }
                else if ((tn1.Text == "Мои регионы") && (rb == 5))
                {
                    btnMove.Visible = false;
                    btnEditReport.Visible = true;
                }
                else
                {
                    btnMove.Visible = false;
                    btnEditReport.Visible = false;
                }
            }

            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            if (proc2 == String.Empty)
                setButtonColor(1);

            tabControl1.SelectedIndex = 0;
            tabControl1.Visible = true;

            SetComponents(globalData.Div, "AP");

            fillTableHeader("AP", _dgv1);

            String tproc1 = "";

            if (chbOnlyPS.Checked == true)
            {
                proc1 = "PersSales";
            }

            tproc1 = "SelAP" + proc1 + proc2;

            if ((!filter) || (fillFilter))
            {
                for (int j = 0; j < 6; j++)
                    f[j] = String.Empty;
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                    comboBox5.SelectedIndex = 0;
                    comboBox6.SelectedIndex = 0;
                }
            }

            if (proc1 == "PersSales")
            {
                int allUsers = 0;
                if (cbUsers.Visible == false)
                    allUsers = 1;
                dt1 = sql1.GetRecords("exec " + tproc1 + " @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", globalData.Region, cbUsers.SelectedValue.ToString(), allUsers, dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-" + "01", dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month.ToString() + "-" + "01", globalData.Div, f[0], f[1], f[2], f[3], f[4], f[5], button);
            }
            else
            {
                dt1 = sql1.GetRecords("exec " + tproc1 + " @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12", globalData.Region, dateTimePicker1.Value.Year.ToString() + "-" + dateTimePicker1.Value.Month.ToString() + "-" + "01", dateTimePicker2.Value.Year.ToString() + "-" + dateTimePicker2.Value.Month.ToString() + "-" + "01", globalData.Div, rb, f[0], f[1], f[2], f[3], f[4], f[5], button);
            }

            String str = String.Empty;
            
            _dgv1.Rows.Clear();

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                        
            int i = 0;
            
            foreach (DataRow row in dt1.Rows)
            {
                bool out1 = false;
                String user = "";

                if (rb == 6)
                {
                    _dgv1.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13], row.ItemArray[14], row.ItemArray[15], row.ItemArray[16], row.ItemArray[17], row.ItemArray[18], row.ItemArray[19], row.ItemArray[20], row.ItemArray[21], row.ItemArray[22], row.ItemArray[23]);
                }
                else
                {
                    if (proc1 == "PersSales")
                        user = row.ItemArray[29].ToString();
                    else
                        if (row.ItemArray[29].ToString() == "1")
                            out1 = true;

                    _dgv1.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13], row.ItemArray[14], row.ItemArray[15], row.ItemArray[16], row.ItemArray[17], row.ItemArray[18], row.ItemArray[19], row.ItemArray[20], row.ItemArray[21], row.ItemArray[22], row.ItemArray[23], row.ItemArray[24], row.ItemArray[25], row.ItemArray[26], row.ItemArray[27], row.ItemArray[28], out1, "", user);
                }
                i++;
            }

            if (chbOnlyPS.Checked)
            {
                foreach (DataGridViewRow row in _dgv1.Rows)
                {
                    if (globalData.UserAccess != 1)
                        if ((Convert.ToDateTime(row.Cells["date"].Value) != globalData.CurDate) || (!cbUsers.Visible))
                        {
                            row.DefaultCellStyle.BackColor = bbgray5;
                            row.ReadOnly = true;
                        }
                }
            }
            
            SetStatusStrip(true, "Строк: " + dt1.Rows.Count);

            if ((fillFilter) || (filter))
            {
                load = false;
                fillComboBox(dt1, "cust", comboBox1, 0);
                fillComboBox(dt1, "SBA", comboBox2, 1);
                fillComboBox(dt1, "MSG", comboBox4, 3);


                var query = (from c in dt1.AsEnumerable()
                             orderby c.Field<String>("mat_code") ascending
                             select new
                             {
                                 Number = c.Field<String>("mat_code")
                             }).Distinct();

                if (query.Count() != 0)
                {
                    comboBox5.Items.Clear();
                    comboBox5.Items.Add("все");
                    for (int j = 0; j < query.Count(); j++)
                        comboBox5.Items.Add(query.ElementAt(j).Number);
                    if (f[4] == String.Empty)
                        comboBox5.SelectedIndex = 0;
                    comboBox5.Enabled = true;
                }

                var query2 = (from c in dt1.AsEnumerable()
                             orderby c.Field<String>("mat_name") ascending
                             select new
                             {
                                 Name = c.Field<String>("mat_name")
                             }).Distinct();

                if (query2.Count() != 0)
                {
                    comboBox6.Items.Clear();
                    comboBox6.Items.Add("все");
                    for (int j = 0; j < query2.Count(); j++)
                        comboBox6.Items.Add(query2.ElementAt(j).Name);
                    if (f[5] == String.Empty)
                        comboBox6.SelectedIndex = 0;
                    comboBox6.Enabled = true;
                }

                if (dt1.Rows.Count > 0)
                {
                    if (f[0] != String.Empty)
                        comboBox1.SelectedIndex = 1;
                    else
                        comboBox1.SelectedIndex = 0;
                    if (f[1] != String.Empty)
                        comboBox2.SelectedIndex = 1;
                    else
                        comboBox2.SelectedIndex = 0;
                    /*
                    if (f[2] != String.Empty)
                        comboBox3.SelectedIndex = 1;
                    else
                        comboBox3.SelectedIndex = 0;
                    */
                    if (f[3] != String.Empty)
                        comboBox4.SelectedIndex = 1;
                    else
                        comboBox4.SelectedIndex = 0;
                    if (f[4] != String.Empty)
                        comboBox5.SelectedIndex = 1;
                    else
                        comboBox5.SelectedIndex = 0;
                    if (f[5] != String.Empty)
                        comboBox6.SelectedIndex = 1;
                    else
                        comboBox6.SelectedIndex = 0;
                }
                load = true;
                fillFilter = false;
                filter = true;
            }
            
            if (chbOnlyPS.Checked == true)
                _dgv1.Columns["bum"].HeaderText = "Остаток";
            else
                _dgv1.Columns["bum"].HeaderText = "Количество (шт)";

            if (subsum != 0)
            {
                int temp = subsum;
                subsum++;
                SubSum(temp);
            }
        }

        private void SelKosReport()
        {
            tabControl1.SelectedIndex = 2;
            tabControl1.Visible = true;

            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            globalData.Region = tn1.Parent.Parent.Text;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            fillTableHeader("Kos", _dgv3);

            treeView1.Focus();

            int allUsers = 0;
            if (cbUsers3.Visible == false)
                allUsers = 1;

            EnableSave("ap");

            dt1 = sql1.GetRecords("exec SelKosReport @p1, @p2, @p3, @p4, @p5, @p6", dateTimePicker3.Value.Year.ToString() + "-" + dateTimePicker3.Value.Month.ToString() + "-01", dateTimePicker4.Value.Year.ToString() + "-" + dateTimePicker4.Value.Month.ToString() + "-01", globalData.Region, globalData.Div, cbUsers3.SelectedValue, allUsers);

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные по косвенным продажам.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataRow row in dt1.Rows)
            {
                _dgv3.Rows.Add(row.ItemArray);
            }

            if (subsumKS != 0)
            {
                int temp = subsumKS;
                subsumKS++;
                subsumKosSales(temp, _dgv3, 11);
            }
        }
        
        private void SelAcc(string year, string lpu, string user, string reg, string RD, DataGridView dgv)
        {
            Cursor = Cursors.WaitCursor;
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
            //dt1 = sql1.GetRecords("exec SelAcc @p1, @p2, @p3, @p4, @p5, @p6", globalData.Div, year, lpu, user, reg, RD);
            dt1 = sql1.GetRecords("exec SelAcc_quart @p1, @p2, @p3, @p4, @p5, @p6", globalData.Div, year, lpu, user, reg, RD);

            Cursor = Cursors.Default;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fillAcc(dt1, dgv);
        }
        

        private void SelRegAcc()
        {
            tabControl1.SelectedIndex = 3;
            tabControl1.Visible = true;

            string lpu = "0", user = "0";

            if (cbLPU.Visible)
                lpu = cbLPU.SelectedValue.ToString();
            if (cbUsers2.Visible)
                user = cbUsers2.SelectedValue.ToString();

            SelAcc(cbYearAcc.SelectedItem.ToString(), lpu, user, globalData.Region, "", _dgv4);
        }

        private void SelDyn()
        {
            tabControl1.SelectedIndex = 4;
            tabControl1.Visible = true;

            loadDyn("0");
        }

        private void SetButtonColorDyn(int cur)
        {
            btnDK1.BackColor = Control.DefaultBackColor;
            btnDK2.BackColor = Control.DefaultBackColor;
            btnDK3.BackColor = Control.DefaultBackColor;
            btnDK4.BackColor = Control.DefaultBackColor;
            btnDK5.BackColor = Control.DefaultBackColor;
            btnDK6.BackColor = Control.DefaultBackColor;

            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            if (tn1.Text == "Динамика продаж по Кардиологии")
            {
                btnDK5.Visible = true;
                btnDK1.Text = "Ангиография";
                btnDK2.Text = "Ангиопластика";
                btnDK3.Text = "Гемодинамика";
                btnDK4.Text = "Имплантируемые порт-системы";
                btnDK5.Text = "Сосудистые протезы";
            }
            else
            {
                btnDK5.Visible = false;
                btnDK1.Text = "Шовные материалы";
                btnDK2.Text = "Степлеры и прочее";
                btnDK3.Text = "Герниопластика, продукты для лечения недержания и пр.";
                btnDK4.Text = "Гемостатики и Гистакрил";
            }

            switch (cur)
            {
                case 1:
                    {
                        btnDK1.BackColor = bbgreen3;
                        break;
                    }
                case 2:
                    {
                        btnDK2.BackColor = bbgreen3;
                        break;
                    }
                case 3:
                    {
                        btnDK3.BackColor = bbgreen3;
                        break;
                    }
                case 4:
                    {
                        btnDK4.BackColor = bbgreen3;
                        break;
                    }
                case 5:
                    {
                        btnDK5.BackColor = bbgreen3;
                        break;
                    }
                case 6:
                    {
                        btnDK6.BackColor = bbgreen3;
                        break;
                    }
            }
        }

        private void selAllPrivSales()
        {
            int basic = 0;
            if (btnBasic.BackColor == bbgreen3)
            {
                basic = 1;
            }
            int reg_id = 0;
            if (button28.Text == "Общая")
                reg_id = Convert.ToInt32(cbRegionAllPrivSales.SelectedValue);

            tabControl1.SelectedIndex = 5;
            tabControl1.Visible = true;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec SelAllPrivSales @p1, @p2, @p3, @p4", globalData.Div, cbYearAllPrivSales.SelectedItem, basic, reg_id);

            _dgv6.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные по личным продажам.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewColumn col in _dgv6.Columns)
                col.SortMode = DataGridViewColumnSortMode.Programmatic;

            _dgv6.Columns["user_name"].HeaderText = "Региональные представители";
            
            //_dgv6.Columns["pm8"].HeaderText = "08-" + (clsData.CurDate.Year - 1).ToString();
            //_dgv6.Columns["pm9"].HeaderText = "09-" + (clsData.CurDate.Year - 1).ToString();
            //_dgv6.Columns["pm10"].HeaderText = "10-" + (clsData.CurDate.Year - 1).ToString();
            //_dgv6.Columns["pm11"].HeaderText = "11-" + (clsData.CurDate.Year - 1).ToString();
            //_dgv6.Columns["pm12"].HeaderText = "12-" + (clsData.CurDate.Year - 1).ToString();

            _dgv6.Columns["cm1"].HeaderText = "01-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm2"].HeaderText = "02-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm3"].HeaderText = "03-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm4"].HeaderText = "04-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm5"].HeaderText = "05-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm6"].HeaderText = "06-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm7"].HeaderText = "07-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm8"].HeaderText = "08-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm9"].HeaderText = "09-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm10"].HeaderText = "10-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm11"].HeaderText = "11-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm12"].HeaderText = "12-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["total"].HeaderText = "Итого " + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm1R"].HeaderText = "01-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm2R"].HeaderText = "02-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm3R"].HeaderText = "03-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm4R"].HeaderText = "04-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm5R"].HeaderText = "05-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm6R"].HeaderText = "06-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm7R"].HeaderText = "07-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm8R"].HeaderText = "08-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm9R"].HeaderText = "09-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm10R"].HeaderText = "10-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm11R"].HeaderText = "11-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["cm12R"].HeaderText = "12-" + cbYearAllPrivSales.SelectedItem.ToString();
            _dgv6.Columns["totalR"].HeaderText = "Итого " + cbYearAllPrivSales.SelectedItem.ToString();

            //_dgv6.Columns["pm8"].Visible = false;
            //_dgv6.Columns["pm9"].Visible = false;
            //_dgv6.Columns["pm10"].Visible = false;
            //_dgv6.Columns["pm11"].Visible = false;
            //_dgv6.Columns["pm12"].Visible = false;

            _dgv6.Columns["user_name"].Frozen = true;

            for (int i = 1; i < 27; i++)
            {
                _dgv6.Columns[i].DefaultCellStyle.Format = "N3";
                _dgv6.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            visiblePrivSales(_dgv6, radioButton12, radioButton13);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about ab = new about();
            ab.ShowDialog();
        }

        private void cbUsers3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                loadData();
        }

        private void button19_Click(object sender, EventArgs e)
        {            
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

            xlSh.Cells[1, 1] = "№";
            xlSh.Cells[1, 2] = "ФИО";
            xlSh.Cells[1, 3] = "08 -" + (globalData.CurDate.Year - 1).ToString();
            xlSh.Cells[1, 4] = "09 -" + (globalData.CurDate.Year - 1).ToString();
            xlSh.Cells[1, 5] = "10 -" + (globalData.CurDate.Year - 1).ToString();
            xlSh.Cells[1, 6] = "11 -" + (globalData.CurDate.Year - 1).ToString();
            xlSh.Cells[1, 7] = "12 -" + (globalData.CurDate.Year - 1).ToString();
            xlSh.Cells[1, 8] = "01 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 9] = "02 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 10] = "03 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 11] = "04 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 12] = "05 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 13] = "06 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 14] = "07 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 15] = "08 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 16] = "09 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 17] = "10 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 18] = "11 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 19] = "12 -" + globalData.CurDate.Year.ToString();
            xlSh.Cells[1, 20] = "Итого за " + globalData.CurDate.Year.ToString() + " год";

            int i = 2;
            foreach (DataGridViewRow row in _dgv6.Rows)
            {
                xlSh.Cells[i, 1] = i - 1;
                for (int j = 0; j < _dgv6.ColumnCount; j++)
                {
                    xlSh.Cells[i, j + 2] = row.Cells[j].Value.ToString();
                }
                i++;
            }
            xlApp.Visible = true;
        }

        private void закрытьПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Закрыть период? Все продажи за " + globalData.CurDate.Month.ToString() + "." +
                globalData.CurDate.Year.ToString() + " перейдут в остаток.", "Закрытие периода", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                string m = "";
                string y = "";
                if (globalData.CurDate.Month == 12)
                {
                    m = "01";
                    y = (globalData.CurDate.Year + 1).ToString();
                }
                else
                {
                    m = (globalData.CurDate.Month + 1).ToString();
                    y = globalData.CurDate.Year.ToString();
                }
                String date2 = y + "-" + m + "-01";

                Sql sql1 = new Sql();

                DataTable dt1 = new DataTable();

                dt1 = sql1.GetRecords("exec Region_Select");

                foreach (DataRow row in dt1.Rows)
                {
                    sql1.GetRecords("exec ClosePeriod @p1, @p2", date2, row.ItemArray[0]);
                }

                
                MessageBox.Show("Период закрыт.", "Закрытие периода", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EnableSave(string report)
        {
            if ((globalData.Div == "OM") || (globalData.UserAccess == 1))
            {                
                btnAdd.Visible = true;
                btnEdit.Visible = true;
                btnDel.Visible = true;
            }
            else
            {
                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDel.Visible = false;
            }

            if (globalData.UserAccess != 1)
            {
                Sql sql1 = new Sql();

                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec GetUserEdit @p1, @p2, @p3", globalData.UserID, globalData.Region, globalData.Div);
                
                if(dt1 != null)
                {
                    switch (report)
                    {
                        case "ap":
                            {
                                if (dt1.Rows.Count == 0)
                                {
                                    btnSave.Enabled = false;
                                    btnAdd.Enabled = false;
                                    btnEdit.Enabled = false;
                                    btnDel.Enabled = false;
                                }
                                else
                                {
                                    if (dt1.Rows[0].ItemArray[0].ToString() == "0")
                                    {
                                        btnSave.Enabled = false;
                                        btnAdd.Enabled = false;
                                        btnEdit.Enabled = false;
                                        btnDel.Enabled = false;
                                    }
                                    else
                                    {
                                        TreeNode tn1 = new TreeNode();
                                        tn1 = treeView1.SelectedNode.Parent.Parent.Parent.Parent;

                                        if (tn1 == null)
                                            tn1 = treeView1.SelectedNode.Parent.Parent.Parent;

                                        if (tn1.Text == "Мои регионы")
                                            btnSave.Enabled = true;
                                        else
                                            btnSave.Enabled = false;
                                        
                                        if (cbUsers3.Visible)
                                        {
                                            btnAdd.Enabled = true;
                                            btnDel.Enabled = true;
                                        }
                                        else
                                        {
                                            btnAdd.Enabled = false;
                                            btnDel.Enabled = false;
                                        }
                                        btnEdit.Enabled = true;
                                        
                                    }
                                }
                                break;
                            }
                        case "accNY":
                            {
                                if (dt1.Rows.Count == 0)
                                {
                                    btnSaveAccNY.Enabled = false;
                                }
                                else
                                {
                                    if (dt1.Rows[0].ItemArray[1].ToString() == "0")
                                    {
                                        btnSaveAccNY.Enabled = false;
                                    }
                                    else
                                    {
                                        TreeNode tn1 = new TreeNode();
                                        tn1 = treeView1.SelectedNode.Parent.Parent.Parent;
                                        if (tn1.Text == "Мои регионы")
                                            btnSaveAccNY.Enabled = true;
                                        else
                                            btnSaveAccNY.Enabled = false;
                                    }
                                }
                                break;
                            }
                        case "ma":
                            {
                                if (dt1.Rows.Count == 0)
                                {
                                    btnMA_fill.Enabled = false;
                                    btnMA_edit.Enabled = false;
                                    btnMA_del.Enabled = false;
                                }
                                else
                                {
                                    TreeNode tn1 = new TreeNode();
                                    tn1 = treeView1.SelectedNode.Parent.Parent.Parent;
                                    if (tn1.Text == "Мои регионы")
                                    {
                                        if (dt1.Rows[0].ItemArray[2].ToString() == "0")
                                        {
                                            btnMA_fill.Enabled = false;
                                            btnMA_edit.Enabled = false;
                                            btnMA_del.Enabled = false;
                                        }
                                        else if (dt1.Rows[0].ItemArray[2].ToString() == "1")
                                        {
                                            btnMA_fill.Enabled = false;
                                            btnMA_edit.Enabled = true;
                                            btnMA_del.Enabled = false;
                                        }
                                        else
                                        {
                                            btnMA_fill.Enabled = true;
                                            btnMA_edit.Enabled = true;
                                            btnMA_del.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        btnMA_fill.Enabled = false;
                                        btnMA_edit.Enabled = false;
                                        btnMA_del.Enabled = false;
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 25)
                {
                    _dgv1.BeginEdit(true);
                }
            }
            catch
            {
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Sql sql1 = new Sql();
            sql1.GetRecords("exec WhiteHistory @p1, 2, null, null, null", globalData.UserID);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (_dgv1.CurrentCellAddress.X == 1)
            {
                DataGridViewComboBoxEditingControl combo =
                 (DataGridViewComboBoxEditingControl)e.Control;
                combo.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dgv1.Rows[_dgv1.CurrentCellAddress.Y].Cells[0].Value =
              _dgv1.CurrentCell.EditedFormattedValue.ToString();
        }

        private void SetStatusStrip(bool clear, String str)
        {
            if (clear)
                statusStrip1.Items.Clear();
            statusStrip1.Items.Add(str);
            statusStrip1.Items[statusStrip1.Items.Count-1].Alignment = ToolStripItemAlignment.Right;
        }

        private void CountSelectCell(DataGridView dgv)
        {
            try
            {
                if (dgv.SelectedCells.Count > 1)
                {
                    Double sum = 0;
                    foreach (DataGridViewCell cell in dgv.SelectedCells)
                    {
                        if (cell.Value.ToString() != String.Empty)
                            if (!cell.Value.ToString().Any(c => char.IsLetter(c)))
                                sum += Convert.ToDouble(cell.Value);
                    }
                    if (sum != 0)
                    {
                        SetStatusStrip(true, "     Сумма: " + (Math.Round(sum, 2).ToString()));
                        SetStatusStrip(false, "     Количество: " + dgv.SelectedCells.Count.ToString());
                        SetStatusStrip(false, "Среднее: " + (Math.Round(sum / dgv.SelectedCells.Count, 2)).ToString());
                    }
                    else
                    {
                        SetStatusStrip(true, "Количество: " + dgv.SelectedCells.Count.ToString());
                    }
                }
                else
                {
                    SetStatusStrip(true, "");
                }
            }
            catch
            {
                SetStatusStrip(true, "");
            }
        }

        private void _dgv1_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv1);
        }

        private void _dgv2_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv2);
        }

        private void _dgv3_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv3);
        }

        private void _dgv4_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv4);
        }

        private void _dgv5_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv5);
        }

        private void _dgv6_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv6);
        }

        private void _dgv7_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv7);
        }

        private void _dgv8_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv8);
        }

        private void _dgv9_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv9);
        }

        private void _dgv10_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv10);
        }

        private void btnHideUsers_Click(object sender, EventArgs e)
        {
            string user = "0";                

            if (btnHideUsers.Text == "Скрыть РП")
            {
                btnHideUsers.Text = "Показать РП";
                cbUsers2.Visible = false;

                fillLPU(cbLPU, user, globalData.Region, cbYearAcc.SelectedItem.ToString());
            }
            else
            {
                btnHideUsers.Text = "Скрыть РП";
                cbUsers2.Visible = true;
                user = cbUsers2.SelectedValue.ToString();

                fillLPU(cbLPU, user, globalData.Region, cbYearAcc.SelectedItem.ToString());
            }

            /*
            if (btnHideUsers.Text == "Скрыть пользователей")
            {
                btnHideUsers.Text = "Показать пользователей";
                cbLPU.Visible = false;
                cbUsers2.Visible = false;
                btnHideLPU.Visible = false;
            }
            else
            {
                if (btnHideLPU.Text == "Скрыть ЛПУ")
                    cbLPU.Visible = true;
                btnHideLPU.Visible = true;
                btnHideUsers.Text = "Скрыть пользователей";
                cbUsers2.Visible = true;
            }
            */
            SelRegAcc();
        }

        /*
        private void hideUsersAcc()
        {
            if (btnHideUsers.Text == "Показать пользователей")
            {
                clsSql sql1 = new clsSql();
                //_dgv4.Columns.Clear();
                DataTable dt1 = new DataTable();

                dt1 = sql1.GetRecords("exec SelAccNew @p1, @p2, 0, 0, @p3", clsData.Div, cbYearAcc.SelectedItem.ToString(), clsData.Region);
                //_dgv4.DataSource = dt1;

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAcc(dt1, _dgv4);
            }
            else
            {
                if (btnHideLPU.Text == "Скрыть ЛПУ")
                {
                    fillLPU(cbLPU, cbUsers2.SelectedValue.ToString(), clsData.Region, Convert.ToInt32(cbYearAcc.SelectedItem));
                    loadAccPlanByLPU();
                }
                else
                {
                    button18_Click(null, null);
                }
            }
        }
        */

        private void selAllAcc()
        {
            tabControl1.SelectedIndex = 6;
            tabControl1.Visible = true;

            string lpu = "0", user = "0", reg = "0";

            if ((cbLPUAcc.Visible) && (cbLPUAcc.Items.Count > 0))
                lpu = cbLPUAcc.SelectedValue.ToString();
            if ((cbUsersAcc.Visible) && (cbUsersAcc.Items.Count > 0))
                user = cbUsersAcc.SelectedValue.ToString();
            if ((cbRegAcc.Visible) && (cbRegAcc.Items.Count > 0))
                reg = cbRegAcc.SelectedValue.ToString();

            SelAcc(cbYearAccAll.SelectedItem.ToString(), lpu, user, reg, globalData.RD, _dgv7);
        }

        private void fillRegions(String RD, ComboBox cb)
        {
            load = false;
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            if (RD == "RF")
            {
                dt1 = sql1.GetRecords("exec SelRegDir @p1", "HC");

                cb.DataSource = dt1;
                cb.DisplayMember = "user_name";
                cb.ValueMember = "user_id";
                load = true;
            }
            else
            {
                if (RD == "")
                    dt1 = sql1.GetRecords("exec Region_Select");
                else
                    dt1 = sql1.GetRecords("exec Region_Select @p1, @p2", RD, globalData.Div);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить список регионов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                cb.DataSource = dt1;
                cb.DisplayMember = "reg_nameRus";
                cb.ValueMember = "reg_id";
                load = true;
            }
        }

        private void cbYearAccAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {                
                string reg = "0", user = "0";
                if ((cbRegAcc.Visible) && (cbRegAcc.Items.Count > 0))
                    reg = cbRegAcc.SelectedValue.ToString();

                fillUsersAcc(cbUsersAcc, reg, cbYearAccAll.SelectedItem.ToString());

                if ((cbUsersAcc.Visible) && (cbUsersAcc.Items.Count > 0))
                    user = cbUsersAcc.SelectedValue.ToString();
                
                fillLPU(cbLPUAcc, user, reg, globalData.CurDate.Year.ToString());
            }
        }

        private void cbRegAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                fillUsersAcc(cbUsersAcc, cbRegAcc.SelectedValue.ToString(), cbYearAccAll.SelectedItem.ToString());

                string user = "0";
                if ((cbUsersAcc.Visible) && (cbUsersAcc.Items.Count > 0))
                    user = cbUsersAcc.SelectedValue.ToString();
                
                fillLPU(cbLPUAcc, user, cbRegAcc.SelectedValue.ToString(), globalData.CurDate.Year.ToString());
            }
        }

        private void btnHideRegion_Click(object sender, EventArgs e)
        {
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;
            if (tn1.Parent == null)
                return;

            string reg = "0", user = "0";

            if (btnHideRegion.Text == "Скрыть регион")
            {
                btnHideRegion.Text = "Показать регион";
                cbRegAcc.Visible = false;
            }
            else
            {
                btnHideRegion.Text = "Скрыть регион";          
                cbRegAcc.Visible = true;
                reg = cbRegAcc.SelectedValue.ToString();
            }

            if (tn1.Parent.Text == "Общий ассортиментный план")
                globalData.RD = tn1.Parent.Parent.Text;
            else
                globalData.RD = String.Empty;

            fillUsersAcc(cbUsersAcc, reg, cbYearAccAll.SelectedItem.ToString());

            if ((cbUsersAcc.Visible) && (cbUsersAcc.Items.Count > 0))
                user = cbUsersAcc.SelectedValue.ToString();

            fillLPU(cbLPUAcc, user, reg, globalData.CurDate.Year.ToString());
        }

        private void button15_Click(object sender, EventArgs e)
        {
            addMarkAct();
        }

        private void editMarkAct(int RowIndex)
        {
            try
            {
                Sql sql1 = new Sql();

                if (_dgv8.Rows[_dgv8.SelectedCells[0].RowIndex].Cells["ma_id"].Value.ToString() == "0")
                {
                    MessageBox.Show("Для редактирования выделите маркетинговое мероприятие.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (cbUsersMA.Visible == true)
                    globalData.UserID2 = Convert.ToInt32(cbUsersMA.SelectedValue);
                else
                {                    
                    globalData.UserID2 = Convert.ToInt32(sql1.GetRecordsOne("exec SelUserByMA @p1, @p2", Convert.ToInt32(_dgv8.Rows[RowIndex].Cells["ma_id"].Value), Convert.ToInt32(_dgv8.Rows[RowIndex].Cells["db_id"].Value)));
                }
                if (globalData.Region == "")
                    globalData.Region = sql1.GetRecordsOne("exec SelRegByMA @p1, @p2", Convert.ToInt32(_dgv8.Rows[RowIndex].Cells["ma_id"].Value), Convert.ToInt32(_dgv8.Rows[RowIndex].Cells["db_id"].Value));

                AddEditMarkAct aema = new AddEditMarkAct(Convert.ToInt32(_dgv8.Rows[RowIndex].Cells["ma_id"].Value), Convert.ToInt32(_dgv8.Rows[RowIndex].Cells["db_id"].Value));
                aema.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось войти в режим редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addMarkAct()
        {
            int i;
            if (cbUsersMA.SelectedValue != null && tabControl1.SelectedTab != tabControl1.TabPages["tabPage25"])
            {
                globalData.UserID2 = Convert.ToInt32(cbUsersMA.SelectedValue);
                i = 1;
            }
            else
            {
                globalData.UserID2 = Convert.ToInt32(comboBox12.SelectedValue);
                i = 2;
            }
            AddEditMarkAct aema = new AddEditMarkAct(i);
            aema.ShowDialog();
        }

        private void delMarkAct()
        {
            if (_dgv8.Rows[_dgv8.SelectedCells[0].RowIndex].Cells["ma_id"].Value.ToString() == "0")
            {
                MessageBox.Show("Для удаления выделите маркетинговое мероприятие.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dr = MessageBox.Show("Удалить выделенную строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                Sql sql1 = new Sql();
                sql1.GetRecords("exec DelMarkAct @p1, @p2", _dgv8.Rows[_dgv8.SelectedCells[0].RowIndex].Cells["ma_id"].Value.ToString(), _dgv8.Rows[_dgv8.SelectedCells[0].RowIndex].Cells["db_id"].Value.ToString());
                MessageBox.Show("Маркетинговое мероприятие удалено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SelMA();
            }
        }

        private void _dgv8_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            editMarkAct(e.RowIndex);
        }

        private void btnMA_edit_Click(object sender, EventArgs e)
        {
            editMarkAct(_dgv8.SelectedCells[0].RowIndex);
        }

        private void btnMA_del_Click(object sender, EventArgs e)
        {
            delMarkAct();
        }

        private void _dgv8_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            point.X = e.ColumnIndex;
            point.Y = e.RowIndex;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMarkAct();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editMarkAct(_dgv8.SelectedCells[0].RowIndex);
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delMarkAct();
        }

        private void _cms8_Opening(object sender, CancelEventArgs e)
        {
            if (_dgv8.Rows[point.Y].Cells["ma_id"].Value.ToString() == "0")
            {
                _cms8.Items[1].Enabled = false;
                _cms8.Items[2].Enabled = false;
            }
            else
            {
                _cms8.Items[1].Enabled = true;
                _cms8.Items[2].Enabled = true;
            }
            _dgv8.CurrentCell = _dgv8[point.X, point.Y];
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            tabControl1.Visible = false;

            FormLogin log = new FormLogin();
            if (log.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Connect();
            else
                this.Close();
            /*
            treeView1.Nodes.Clear();
            tabControl1.Visible = false;

            login log = new login();
            log.ShowDialog();

            reConnect();

            CreateTree();
            ClearFilter();
            treeView1.Focus();
            */
            /* Ежедневный отчет */
            //DailyAccess();
            /*
            if (globalData.fp == 1)
            {
                tabControl1.SelectedIndex = 10;
                tabControl1.Visible = true;
            }
            */
        }

        private void _dgv1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            point.X = e.ColumnIndex;
            point.Y = e.RowIndex;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = _dgv1;
                switch (tabControl1.SelectedIndex)
                {
                    case 1:
                        {
                            dgv = _dgv2;
                            break;
                        }
                    case 2:
                        {
                            dgv = _dgv3;
                            break;
                        }
                    case 3:
                        {
                            dgv = _dgv4;
                            break;
                        }
                    case 4:
                        {
                            dgv = _dgv5;
                            break;
                        }
                    case 5:
                        {
                            dgv = _dgv6;
                            break;
                        }
                    case 6:
                        {
                            dgv = _dgv7;
                            break;
                        }
                    case 7:
                        {
                            dgv = _dgv8;
                            break;
                        }
                    case 8:
                        {
                            dgv = _dgv9;
                            break;
                        }
                    case 9:
                        {
                            dgv = _dgv10;
                            break;
                        }
                }
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                DataObject d = dgv.GetClipboardContent();

                if (d != null)
                {
                    Clipboard.SetDataObject(d);
                    Clipboard.GetData(DataFormats.Text);
                    IDataObject dt = Clipboard.GetDataObject();
                    if (dt.GetDataPresent(typeof(string)))
                    {
                        String tb = (string)(dt.GetData(typeof(string)));
                        Encoding encoding = Encoding.GetEncoding(1251);
                        byte[] dataStr = encoding.GetBytes(tb);
                        Clipboard.SetDataObject(encoding.GetString(dataStr));
                    }
                }
            }
            catch
            {
            }
        }

        private void CopyWithHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = _dgv1;
                switch (tabControl1.SelectedIndex)
                {
                    case 1:
                        {
                            dgv = _dgv2; 
                            break;
                        }
                    case 2:
                        {
                            dgv = _dgv3;
                            break;
                        }
                    case 3:
                        {
                            dgv = _dgv4;
                            break;
                        }
                    case 4:
                        {
                            dgv = _dgv5;
                            break;
                        }
                    case 5:
                        {
                            dgv = _dgv6;
                            break;
                        }
                    case 6:
                        {
                            dgv = _dgv7;
                            break;
                        }
                    case 7:
                        {
                            dgv = _dgv8;
                            break;
                        }
                    case 8:
                        {
                            dgv = _dgv9;
                            break;
                        }
                    case 9:
                        {
                            dgv = _dgv10;
                            break;
                        }
                }
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                DataObject d = dgv.GetClipboardContent();

                if (d != null)
                {
                    Clipboard.SetDataObject(d);
                    Clipboard.GetData(DataFormats.Text);
                    IDataObject dt = Clipboard.GetDataObject();
                    if (dt.GetDataPresent(typeof(string)))
                    {
                        String tb = (string)(dt.GetData(typeof(string)));
                        Encoding encoding = Encoding.GetEncoding(1251);
                        byte[] dataStr = encoding.GetBytes(tb);
                        Clipboard.SetDataObject(encoding.GetString(dataStr));
                    }
                }
            }
            catch
            {
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboard();
        }

        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                s = s.Replace("\r", "");
                string[] lines = s.Split('\n');
                int iFail = 0, iRow = _dgv1.CurrentCell.RowIndex;
                int iCol = _dgv1.CurrentCell.ColumnIndex;
                DataGridViewCell oCell;
                foreach (string line in lines)
                {
                    if (iRow < _dgv1.RowCount && line.Length > 0)
                    {
                        string[] sCells = line.Split('\t');
                        for (int i = 0; i < sCells.GetLength(0); ++i)
                        {
                            if (iCol + i < this._dgv1.ColumnCount)
                            {
                                oCell = _dgv1[iCol + i, iRow];
                                if (!oCell.ReadOnly)
                                {
                                    if (oCell.Value.ToString() != sCells[i])
                                    {
                                        oCell.Value = sCells[i];                                           
                                        _dgv1EndEdit(oCell.ColumnIndex, oCell.RowIndex);
                                    }
                                    else
                                        iFail++;//only traps a fail if the data has changed and you are pasting into a read only cell
                                }
                            }
                            else
                            { break; }
                        }
                        iRow++;
                    }
                    else
                    { break; }
                    if (iFail > 0)
                        MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (tabControl1.SelectedIndex == 3)
            { 
                foreach (DataGridViewCell cell in _dgv4.SelectedCells)
                    if (!cell.ReadOnly)
                    {
                        if (cell.Value.ToString() != String.Empty)
                        {
                            if (globalData.Div == "HC")
                            {
                                if (_dgv4.Rows[cell.RowIndex].Cells["nom_type"].Value.ToString() == "шт")
                                    begval = Convert.ToInt32(cell.Value);
                                else
                                    begval2 = Convert.ToDouble(cell.Value);
                            }
                            else
                            {
                                begval2 = Convert.ToDouble(cell.Value);
                            }
                            cell.Value = null;
                            _dgv4EndEdit(cell.ColumnIndex, cell.RowIndex);
                        }
                    }
            }
           else if (tabControl1.SelectedIndex == 14)
           {
               foreach (DataGridViewCell cell in _dgv14.SelectedCells)
               {
                   if (!cell.ReadOnly)
                   {
                       if (cell.Value.ToString() != String.Empty)
                       {
                           if (globalData.Div == "HC")
                           {
                               if (_dgv14.Rows[cell.RowIndex].Cells["nom_type"].Value.ToString() == "шт")
                                   begval = Convert.ToInt32(cell.Value);
                               else
                                   begval2 = Convert.ToDouble(cell.Value);
                           }
                           else
                           {
                               begval2 = Convert.ToDouble(cell.Value);
                           }
                           cell.Value = null;
                           _dgv14EndEdit(cell.ColumnIndex, cell.RowIndex);
                       }
                   }
               }
           }
            else
            {
                foreach (DataGridViewCell cell in _dgv1.SelectedCells)
                {
                    if (!cell.ReadOnly)
                    {
                        if (cell.Value.ToString() != String.Empty)
                        {
                            begval = Convert.ToInt32(cell.Value);
                            cell.Value = String.Empty;
                            _dgv1EndEdit(cell.ColumnIndex, cell.RowIndex);
                        }
                    }
                }
            }
        }

        private void loadPlanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Sales\Regional accounts HC\2012\Планы проверенные";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String filename in openFileDialog1.FileNames)
                {
                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlSh;
                    xlApp = new Excel.Application();
                    xlWorkBook = xlApp.Workbooks.Open(filename, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);

                    globalData.Region = xlSh.get_Range("C2", "C2").Value2.ToString();

                    xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    object misValue = System.Reflection.Missing.Value;
                    int err = 0;
                    int i = 3, j = 0;
                    Cursor = Cursors.WaitCursor;

                    String[] colname = {"D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", 
                                       "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF" };

                    try
                    {
                        Sql sql1 = new Sql();

                        String[] fname = xlWorkBook.Name.Split('.');

                        String user = fname[0] + "." + fname[1] + ".";
                        sql1.GetRecords("exec ClearAccPlan @p1, @p2, @p3, @p4", user, globalData.Region, "HC", globalData.CurDate.Year);

                        for (j = 0; j < 29; j++)
                        {
                            String lpu = xlSh.get_Range(colname[j] + "2", colname[j] + "2").Value2.ToString();//lpu

                            i = 3;
                            int idNom = 37;
                            bool minus = true;

                            while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                            {

                                if (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString() == "ИТОГО")
                                {
                                    idNom++;
                                    i++;
                                    continue;
                                }

                                if ((idNom == 111) && (minus))
                                {
                                    idNom--;
                                    minus = false;
                                }
                               

                                String unit = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();

                                double s1 = Convert.ToDouble(xlSh.get_Range(colname[j] + i.ToString(), colname[j] + i.ToString()).Value2);
                                double s2 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 1).ToString(), colname[j] + (i + 1).ToString()).Value2);
                                double s3 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 2).ToString(), colname[j] + (i + 2).ToString()).Value2);
                                double s4 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 3).ToString(), colname[j] + (i + 3).ToString()).Value2);
                                double s5 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 4).ToString(), colname[j] + (i + 4).ToString()).Value2);
                                double s6 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 5).ToString(), colname[j] + (i + 5).ToString()).Value2);
                                double s7 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 6).ToString(), colname[j] + (i + 6).ToString()).Value2);
                                double s8 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 7).ToString(), colname[j] + (i + 7).ToString()).Value2);
                                double s9 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 8).ToString(), colname[j] + (i + 8).ToString()).Value2);
                                double s10 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 9).ToString(), colname[j] + (i + 9).ToString()).Value2);
                                double s11 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 10).ToString(), colname[j] + (i + 10).ToString()).Value2);
                                double s12 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 11).ToString(), colname[j] + (i + 11).ToString()).Value2);

                                if ((s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10 + s11 + s12) > 0)
                                {
                                    DataTable dt1 = sql1.GetRecords("exec fillAccPlan @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19", lpu, user, globalData.Region, idNom, unit, "HC", globalData.CurDate.Year, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12);
                                    if (dt1 == null)
                                    {
                                        xlSh.get_Range(colname[j] + i.ToString(), colname[j] + i.ToString()).Value2 = "error";
                                        err++;
                                    }
                                }
                                if ((idNom == 44) || (idNom == 123) || (idNom == 141) || (idNom == 148))
                                    i = i + 12;
                                else
                                    i = i + 13;
                                idNom++;
                            }

                            if (lpu == "Прочие ЛПУ")
                                j = 29;
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + colname[j] + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        err++;
                    }
                    finally
                    {
                        if (err == 0)
                        {
                            MessageBox.Show("Ассортиментный план загружен.", "Загрузка завершена", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            releaseObject(xlSh);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                        }
                        else
                        {
                            MessageBox.Show("Ассортиментный план загружен. Найдено " + err.ToString() + " ошибок.", "Загрузка завершена с ошибками", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            xlApp.Visible = true;
                        }
                    }
                }
                Cursor = Cursors.Default;
                //MessageBox.Show("Ассортиментный план загружен.", "Загрузка завершена", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBasic_Click(object sender, EventArgs e)
        {
            if (btnBasic.BackColor == bbgreen3)
                btnBasic.BackColor = Button.DefaultBackColor;
            else
                btnBasic.BackColor = bbgreen3;

            selAllPrivSales();
        }

        private void cbYearMA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                Sql sql1 = new Sql();

                DataTable dt1 = new DataTable();

                string reg = String.Empty;
                if (cbRegMA.Visible == true)
                    reg = cbRegMA.SelectedValue.ToString();
                else
                    reg = globalData.Region;

                if (globalData.UserAccess == 5)
                {
                    bool all = false;

                    TreeNode tn1 = new TreeNode();
                    tn1 = treeView1.SelectedNode.Parent.Parent.Parent;

                    if (tn1.Text == "Все регионы")
                        all = true;

                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", reg, globalData.Div, cbYearMA.SelectedItem);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", reg, globalData.Div, cbYearMA.SelectedItem);
                cbUsersMA.DataSource = dt1;
                cbUsersMA.DisplayMember = "user_name";
                cbUsersMA.ValueMember = "user_id";

                if (reg == "")
                    fillUsersMA();
                //SelMA();
            }
        }

        private void cbUsersMA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                fillLPUMA();
                //SelMA();
            }
        }

        private void загрузитьПланыAEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "J:\\Sales\\Regional accounts AE\\Regional accounts AE 2012\\Планы 2012\\Планы проверенные";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (String filename in openFileDialog1.FileNames)
                {
                    Excel.Application xlApp;
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlSh;
                    xlApp = new Excel.Application();


                    xlWorkBook = xlApp.Workbooks.Open(filename, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                    xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);

                    globalData.Region = xlSh.get_Range("C2", "C2").Value2.ToString();

                    xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    object misValue = System.Reflection.Missing.Value;
                    int err = 0;
                    int i = 3;
                    Cursor = Cursors.WaitCursor;

                    String[] colname = {"C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", 
                                       "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE" };

                    int[] mas = { 2, 3, 4, 5, 1, 7, 8, 9, 10, 6, 11, 11, 13, 14, 15, 16, 17, 18, 12, 20, 21, 22, 23, 24, 25, 19, 27, 28, 29, 30, 26, 32, 33, 34, 35, 31 };
                    try
                    {
                        Sql sql1 = new Sql();

                        String[] fname = xlWorkBook.Name.Split('.');

                        String user = fname[0] + "." + fname[1] + ".";
                        sql1.GetRecords("exec ClearAccPlan @p1, @p2, @p3, @p4", user, globalData.Region, "AE", globalData.CurDate.Year);

                        for (int j = 0; j < colname.Count(); j++)
                        {
                            String lpu = xlSh.get_Range(colname[j] + "2", colname[j] + "2").Value2.ToString();//lpu

                            i = 3;
                            int idNom = 0;

                            while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                            {

                                if (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString() == "ИТОГО")
                                {
                                    idNom++;
                                    i++;
                                    continue;
                                }

                                double s1 = Convert.ToDouble(xlSh.get_Range(colname[j] + i.ToString(), colname[j] + i.ToString()).Value2);
                                double s2 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 1).ToString(), colname[j] + (i + 1).ToString()).Value2);
                                double s3 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 2).ToString(), colname[j] + (i + 2).ToString()).Value2);
                                double s4 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 3).ToString(), colname[j] + (i + 3).ToString()).Value2);
                                double s5 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 4).ToString(), colname[j] + (i + 4).ToString()).Value2);
                                double s6 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 5).ToString(), colname[j] + (i + 5).ToString()).Value2);
                                double s7 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 6).ToString(), colname[j] + (i + 6).ToString()).Value2);
                                double s8 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 7).ToString(), colname[j] + (i + 7).ToString()).Value2);
                                double s9 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 8).ToString(), colname[j] + (i + 8).ToString()).Value2);
                                double s10 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 9).ToString(), colname[j] + (i + 9).ToString()).Value2);
                                double s11 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 10).ToString(), colname[j] + (i + 10).ToString()).Value2);
                                double s12 = Convert.ToDouble(xlSh.get_Range(colname[j] + (i + 11).ToString(), colname[j] + (i + 11).ToString()).Value2);


                                if ((s1 + s2 + s3 + s4 + s5 + s6 + s7 + s8 + s9 + s10 + s11 + s12) > 0)
                                {
                                    DataTable dt1 = sql1.GetRecords("exec fillAccPlan @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19", lpu, user, globalData.Region, mas[idNom], "euro", "AE", globalData.CurDate.Year, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12);

                                    if (dt1 == null)
                                    {
                                        xlSh.get_Range(colname[j] + i.ToString(), colname[j] + i.ToString()).Value2 = "error";
                                        err++;
                                    }
                                }
                                if ((idNom == 44) || (idNom == 123) || (idNom == 141) || (idNom == 148))
                                    i = i + 12;
                                else
                                    i = i + 13;
                                idNom++;
                            }

                            if (lpu == "Прочие ЛПУ")
                                j = 29;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В строке " + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        err++;
                    }
                    finally
                    {

                        if (err == 0)
                        {
                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            releaseObject(xlSh);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                        }
                        else
                        {
                            MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            xlApp.Visible = true;
                        }
                    }
                }
                Cursor = Cursors.Default;
                //MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void fillLPU(ComboBox lpu, string user_id, string region, string year)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            load = false;

            if (globalData.RD == null)
                globalData.RD = String.Empty;

            dt1 = sql1.GetRecords("exec selLPU @p1, @p2, @p3, @p4, @p5", user_id, region, globalData.Div, year, globalData.RD);

            if (dt1.Rows.Count > 0)
            {
                lpu.DataSource = dt1;
                lpu.DisplayMember = "lpu_sname";
                lpu.ValueMember = "ulpu_id";
                lpu.Enabled = true;
            }
            else
                lpu.Enabled = false;

            load = true;
        }

        private void btnHideUsersAcc_Click(object sender, EventArgs e)
        {
            string reg = "0", user = "0";
            if ((cbRegAcc.Visible) && (cbRegAcc.Items.Count > 0))
                reg = cbRegAcc.SelectedValue.ToString();
                

            if (btnHideUsersAcc.Text == "Скрыть РП")
            {
                btnHideUsersAcc.Text = "Показать РП";
                cbUsersAcc.Visible = false;

                fillLPU(cbLPUAcc, user, reg, cbYearAccAll.SelectedItem.ToString());
            }
            else
            {
                btnHideUsersAcc.Text = "Скрыть РП";
                cbUsersAcc.Visible = true;
                user = cbUsersAcc.SelectedValue.ToString();

                fillLPU(cbLPUAcc, user, reg, cbYearAccAll.SelectedItem.ToString());
            }
        }

        private void btnHideLPUAcc_Click(object sender, EventArgs e)
        {
            if (btnHideLPUAcc.Text == "Скрыть ЛПУ")
            {
                btnHideLPUAcc.Text = "Показать ЛПУ";
                cbLPUAcc.Visible = false;
            }
            else
            {
                btnHideLPUAcc.Text = "Скрыть ЛПУ";
                cbLPUAcc.Visible = true;
            }
        }

        private void cbLPUAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (load)
            {
                selAllAcc(String.Empty);
            }
            */
        }

        private void cbUsersAcc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                string reg = "0";
                if (cbRegAcc.Visible)
                    reg = cbRegAcc.SelectedValue.ToString();

                string user;
                user = cbUsersAcc.SelectedValue.ToString();

                fillLPU(cbLPUAcc, user, reg, globalData.CurDate.Year.ToString());
            }
        }

        private void selEvo(DataGridView dgv, string sdiv, string month, string year, string regID, string RD, string userID)
        {
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            dgv.Columns.Clear();

            
            if (sdiv == "HC")
                dt1 = sql1.GetRecords("exec SelEvoPlan @p1, @p2, @p3, @p4, @p5, @p6", sdiv, month, year, regID, RD, userID);
            else if (sdiv == "RF")
                dt1 = sql1.GetRecords("exec SelEvoPlanRF2 @p1, @p2, @p3, @p4", "HC", month, year, RD);
            else
                dt1 = sql1.GetRecords("exec SelEvoPlanAE @p1, @p2, @p3", month, year, RD);

            dgv.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные для построения выполнения плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            formatEvo(sdiv, dgv);
        }

        private void formatEvo(string sdiv, DataGridView dgv)
        {
            if (sdiv == "RF")
            {
                cbRegEvo.Visible = true;

                btnHideUsersEvo.Visible = false;
                cbUsersEvo.Visible = false;

                button30.Visible = false;

                dgv.Columns["regPlan"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["regFact"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["regEvo"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["ed"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns["regPlan"].DefaultCellStyle.Format = "N3";
                dgv.Columns["regFact"].DefaultCellStyle.Format = "N3";
                dgv.Columns["regEvo"].DefaultCellStyle.Format = "N2";

                dgv.Columns["regPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["regFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["regEvo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["ed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgv.Columns["regPlan"].HeaderText = "план по всей России";
                dgv.Columns["regFact"].HeaderText = "факт по всей России";
                dgv.Columns["regEvo"].HeaderText = "к-т выполнения по России";
                dgv.Columns["totalPlan"].HeaderText = "план по рег.директору";          
                dgv.Columns["totalFact"].HeaderText = "факт по рег.директору";          
                dgv.Columns["totalEvo"].HeaderText = "к-т выполнения по рег.директору"; 
                dgv.Columns["srtEvo"].HeaderText = "ср. к-т выполнения плана по России";
                dgv.Columns["ed"].HeaderText = "ед.";

                dgv.Columns["ed"].Width = 50;

                dgv.Columns["nom"].Frozen = true;
                dgv.Columns["nom"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["totalPlan"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["totalFact"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["totalEvo"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["srtEvo"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns["totalPlan"].DefaultCellStyle.Format = "N3";
                dgv.Columns["totalFact"].DefaultCellStyle.Format = "N3";
                dgv.Columns["totalEvo"].DefaultCellStyle.Format = "N2";
                dgv.Columns["srtEvo"].DefaultCellStyle.Format = "N2";


                dgv.Columns["totalPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["totalFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["totalEvo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["srtEvo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                dgv.Columns["nom"].HeaderText = "Продукция";


                dgv.Columns["nom"].Width = 150;
                dgv.Columns["totalPlan"].Width = 150;
                dgv.Columns["totalFact"].Width = 150;
                dgv.Columns["totalEvo"].Width = 150;
                dgv.Columns["srtEvo"].Width = 150;

                if (cbRegEvo.SelectedIndex == 0)
                {
                    dgv.Columns["regPlan"].Visible = true;
                    dgv.Columns["regFact"].Visible = true;
                    dgv.Columns["regEvo"].Visible =  true;

                    dgv.Columns["totalPlan"].Visible = false;
                    dgv.Columns["totalFact"].Visible = false;
                    dgv.Columns["totalEvo"].Visible = false;
                }
                else
                {
                    dgv.Columns["regPlan"].Visible = false;
                    dgv.Columns["regFact"].Visible = false;
                    dgv.Columns["regEvo"].Visible =  false;

                    dgv.Columns["totalPlan"].Visible = true;
                    dgv.Columns["totalFact"].Visible = true;
                    dgv.Columns["totalEvo"].Visible =  true;
                }

            }
            else if (sdiv == "HC")
            {
                cbRegEvo.Visible = true;

                btnHideUsersEvo.Visible = true;

                if (btnHideUsersEvo.Text == "Скрыть пользователей")
                {
                    cbUsersEvo.Visible = true;
                }

                dgv.Columns["regPlan"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["regFact"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["regEvo"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["ed"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns["regPlan"].DefaultCellStyle.Format = "N3";
                dgv.Columns["regFact"].DefaultCellStyle.Format = "N3";
                dgv.Columns["regEvo"].DefaultCellStyle.Format = "N2";

                dgv.Columns["regPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["regFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["regEvo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["ed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgv.Columns["regPlan"].HeaderText = "план по региону";
                dgv.Columns["regFact"].HeaderText = "факт по региону";
                dgv.Columns["regEvo"].HeaderText = "к-т выполнения по региону";
                dgv.Columns["tPlan"].HeaderText = "план по всем регионам рег. директора";
                dgv.Columns["tFact"].HeaderText = "факт по всем регионам рег. директора";
                dgv.Columns["tEvo"].HeaderText = "к-т выполнения по всем регионам рег. директора";
                dgv.Columns["srtEvo"].HeaderText = "ср. к-т выполнения плана по всем регионам рег. директора";
                dgv.Columns["ed"].HeaderText = "ед.";

                dgv.Columns["ed"].Width = 50;

                dgv.Columns["nom"].Frozen = true;
            }
            else
            {
                cbRegEvo.Visible = false;
                cbUsersEvo.Visible = false;
                btnHideUsersEvo.Visible = false;

                dgv.Columns["id"].Visible = false;
                dgv.Columns["user_dismissed"].Visible = false;
                dgv.Columns["user_name"].HeaderText = "ФИО";
                dgv.Columns["tPlan"].HeaderText = "план " + globalData.CurDate.Year.ToString();
                dgv.Columns["tFact"].HeaderText = "факт " + globalData.CurDate.Year.ToString();
                dgv.Columns["tEvo"].HeaderText = "коэф-т";
                dgv.Columns["srtEvo"].HeaderText = "ср. коэф-т";

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells[1].Value.ToString() != String.Empty)
                        row.DefaultCellStyle.BackColor = bbgreen3;
                    if (row.Cells["user_dismissed"].Value.ToString() == "1")
                        row.DefaultCellStyle.BackColor = bbgray4;
                }
            }

            if (sdiv != "RF")
            {
                dgv.Columns["nom"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["tPlan"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["tFact"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["tEvo"].SortMode = DataGridViewColumnSortMode.Programmatic;
                dgv.Columns["srtEvo"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns["tPlan"].DefaultCellStyle.Format = "N3";
                dgv.Columns["tFact"].DefaultCellStyle.Format = "N3";
                dgv.Columns["tEvo"].DefaultCellStyle.Format = "N2";
                dgv.Columns["srtEvo"].DefaultCellStyle.Format = "N2";


                dgv.Columns["tPlan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["tFact"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["tEvo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["srtEvo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                dgv.Columns["nom"].HeaderText = "Продукция";


                dgv.Columns["nom"].Width = 150;
                dgv.Columns["tPlan"].Width = 150;
                dgv.Columns["tFact"].Width = 150;
                dgv.Columns["tEvo"].Width = 150;
                dgv.Columns["srtEvo"].Width = 150;
                button30.Visible = true;
            }
        }

        private void btnShowEvo_Click(object sender, EventArgs e)
        {
            if (globalData.Div == null || button30.Visible == false)
                selEvo(_dgv10, "RF", dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), "", cbRegEvo.SelectedValue.ToString(), "");
            else
            {
                string user = "0";
                if (cbUsersEvo.Visible)
                    user = cbUsersEvo.SelectedValue.ToString();
                if (globalData.Div == "AE")
                    selEvo(_dgv10, globalData.Div, dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), "0", globalData.RD, user);
                else 
                    selEvo(_dgv10, globalData.Div, dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), cbRegEvo.SelectedValue.ToString(), globalData.RD, user);
            }
        }

        private void selDynRD()
        {
            if (button20.Text == "Общая")
            {
                label14.Visible = true;
                cbRegions.Visible = true;
            }
            else
            {
                label14.Visible = false;
                cbRegions.Visible = false;
            }

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec SelDynRD @p1, @p2, @p3", globalData.Div, cbYearDyn.SelectedItem, globalData.RD);
            _dgv5.Columns.Clear();
            _dgv5.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Ошибка при подсчёте динамики продаж.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            formatDyn();
        }

        private void cbRegMA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                fillUsersMA();
                fillLPUMA();
                SelMA();
            }
        }
        private void fillUsersMA()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            string regID = "0";
            if (cbRegMA.Visible)
                regID = cbRegMA.SelectedValue.ToString();

            if (globalData.RD == null)
                globalData.RD = String.Empty;

            dt1 = sql1.GetRecords("exec SelUsersForAllAcc @p1, @p2, @p3, @p4", regID, globalData.Div, cbYearMA.SelectedItem, globalData.RD);

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные по пользователям", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            load = false;

            cbUsersMA.DataSource = dt1;
            cbUsersMA.DisplayMember = "user_name";
            cbUsersMA.ValueMember = "user_id";

            load = true;
        }

        private void updateAllAccToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Обновить все ассортиментные планы? Операция потребует значительного времени, пожалуйста, не закрывайте программу", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.No)
                return;

            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

            InputDialog id = new InputDialog("Выберите дивизион", "HC - 1, AE - 2, только число", true);
            id.ShowDialog();

            String div = globalData.input;
                        
            if (div == "1")
                globalData.Div = "HC";
            else
                globalData.Div = "AE";

            InputDialog ind = new InputDialog("Год", "Введите год", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;

            String year = globalData.input;
            
            InputDialog ind2 = new InputDialog("Месяц", "Введите номер месяца в формате 1,2,3...", true);
            ind2.ShowDialog();

            if (ind2.DialogResult == DialogResult.Cancel)
                return;

            if ((Convert.ToInt32(globalData.input) < 1) || (Convert.ToInt32(globalData.input) > 12))
            {
                MessageBox.Show("Месяц введён неверно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                globalData.CurDate = Convert.ToDateTime(year + "-" + globalData.input.ToString() + "-01");

            //for (int i = 3; i < 5; i++ )
            
            dt1 = sql1.GetRecords("exec SelAllLPUforUpdate @p1, @p2", globalData.Div, globalData.CurDate);

            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("Ассортиментный план не нужно обновлять", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            prBar1.Visible = true;

            //int count = Convert.ToInt32(sql1.GetRecordsOne("exec CountUpdAllAcc @p1, @p2", clsData.Div, clsData.CurDate));


            bgw = new BackgroundWorker[dt1.Rows.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                bgw[i] = new BackgroundWorker();
                bgw[i].DoWork += backgroundWorker_DoWork;
                bgw[i].RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                bgw[i].WorkerSupportsCancellation = true;
            }

            int j = 0;

            prBar1.Value = 0;
            prBar1.Maximum = dt1.Rows.Count;

            if (globalData.UserID != 1)
            {
                prBar1.Visible = false;
                Cursor = Cursors.WaitCursor;

                foreach (DataRow row in dt1.Rows)
                {
                    sql1.GetRecords("exec fillFactAcc @p1, @p2, @p3", row[0].ToString(), globalData.Div, globalData.CurDate);
                }

                loadData();
                Cursor = Cursors.Default;
                MessageBox.Show("Ассортиментный план обновлён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            foreach(DataRow row in dt1.Rows)
            {                    
                bgw[j].RunWorkerAsync(row[0].ToString());
                j++;
            }

            /*
            dt1 = sql1.GetRecords("exec SelRegionUpdAllAcc @p1, @p2", clsData.Div, clsData.CurDate);

            foreach (DataRow row in dt1.Rows)
            {
                dt2 = sql1.GetRecords("exec SelUsersForAllUpdate @p1, @p2, @p3", row["reg_id"], clsData.Div, clsData.CurDate);

                foreach (DataRow row2 in dt2.Rows)
                {
                    dt3 = sql1.GetRecords("exec SelLPUbyRegID @p1, @p2, @p3", row2["user_id"], row["reg_id"], clsData.Div);

                    if (dt3 == null)
                        continue;

                    foreach (DataRow row3 in dt3.Rows)
                    {
                        bgw[j].RunWorkerAsync(row3[0].ToString());
                        j++;
                    }
                }
            }
            //MessageBox.Show("Ассортиментный план обновлён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
             */
        }
            
        private void loadLPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog inDlg = new InputDialog();

                inDlg.ShowDialog();

                if (globalData.input == "0")
                {
                    MessageBox.Show("ЛПУ не были загружены.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    xlApp.Quit();
                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                    return;
                }

                int i = Convert.ToInt32(globalData.input);
                try
                {

                    Sql sql1 = new Sql();
                    DataTable dt1 = new DataTable();
                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                    {
                        String s1 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//sname
                        String s2 = xlSh.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString();//name
                        String s3 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString();//reg
                        String s4 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//user
                        String s5 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//sdiv
                        
                        dt1 = sql1.GetRecords("exec InsLPU @p1, @p2, @p3, @p4, @p5", s1, s2, s3, s4, s5);
                        if (dt1 == null)
                            err++;
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message);
                    err++;
                }
                finally
                {
                    xlWorkBook.Close(true, misValue, misValue);
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Количество добавленных записей - " + (i - Convert.ToInt32(globalData.input)).ToString());
                        xlApp.Quit();
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.");
                        xlApp.Quit();
                    }

                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
            }
        }
        
        private void btnLoadPrice_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Logistics\SCM\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Загрузка прайс листа";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;
                int i;
                tbResult.Clear();

                try
                {
                    for (int list = 1; list < 9; list++) //13
                    {
                        xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(list);
                        i = 3;
                        Sql sql1 = new Sql();
                        while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                        {
                            if (xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2 != null)
                            {
                                String s1 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Trim();//mat_code
                                double s2 = 0;

                                if(digit(xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString()))
                                    s2 = Convert.ToDouble(xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString().Trim());//price
                                else
                                    tbResult.Text += "Ошибка в цене. Лист " + list.ToString() + ", строка: " + i.ToString();
                                DataTable dt1 = sql1.GetRecords("exec SCM_loadPrice @p1, @p2", s1, s2);
                                if (dt1 == null)
                                {
                                    tbResult.Text += "Ошибка. Лист " + list.ToString() + ", строка: " + i.ToString();
                                    err++;
                                    break;
                                }
                            }
                            i++;
                        }
                        releaseObject(xlSh);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Загрузка выполнена с ошибками. Проверьте наличие цен.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"H:\Documents\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Сравнение цен";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;
                tbResult.Clear();

                String curh = "";
                int i = 2;

                try
                {
                    Sql sql1 = new Sql();
                    while (xlSh.get_Range("M" + i.ToString(), "M" + i.ToString()).Value2 != null)
                    {
                        curh = "R";
                        if (xlSh.get_Range("R" + i.ToString(), "R" + i.ToString()).Value2.ToString() != "0")
                        {
                            curh = "M";
                            string matCode = xlSh.get_Range("M" + i.ToString(), "M" + i.ToString()).Value2.ToString();//mat_code
                            curh = "AT";
                            double priceFloat = Convert.ToDouble(xlSh.get_Range("AT" + i.ToString(), "AT" + i.ToString()).Value2);//price
                            curh = "AB";
                            double s3 = Convert.ToDouble(xlSh.get_Range("AB" + i.ToString(), "AB" + i.ToString()).Value2);//кол-во

                            if (s3 > 0)
                                priceFloat /= s3;

                            string scmPrice = sql1.GetRecordsOne("exec SCM_Compare @p1, @p2", matCode, priceFloat);
                            if ((scmPrice == null) || (scmPrice == string.Empty))
                            {
                                tbResult.Text += "Строка " + i.ToString() + ": товар с артикулом " + matCode + " не найден в справочнике.\r\n";
                            }
                            else if (scmPrice != "0")
                            {
                                double scmPriceFloat = 0;
                                double.TryParse(scmPrice, out scmPriceFloat);

                                if (scmPriceFloat > priceFloat)
                                    tbResult.Text += "Строка " + i.ToString() + ": товар с артикулом " + matCode + " цена " + priceFloat + " меньше цены в прайсе " + scmPrice + "\r\n";
                                else
                                    tbResult.Text += "Строка " + i.ToString() + ": товар с артикулом " + matCode + " цена " + priceFloat + " больше цены в прайсе " + scmPrice + ". Внимание!!!!!!!!!!\r\n";
                            }
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка в ячейке " + curh + i.ToString() + ". Системная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                        MessageBox.Show("Сравнение завершено. Ошибок не найдено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Ошибка при сравнение.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
            }
        }

        private void importReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Controlling\Динамика продаж\Динамика продаж " + globalData.CurDate.Year.ToString() + @"\отчётность региональщиков\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|(*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Введите номер строки", "Номер строки:", true);
                ind.ShowDialog();
                if (globalData.input == "0")
                {
                    MessageBox.Show("Загрузка отчёта отменена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                    return;
                }
                int i = Convert.ToInt32(globalData.input);  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                String header = String.Empty;

                try
                {
                    Sql sql1 = new Sql();
                    while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                    {
                        header = "A";
                        String s1 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//comp
                        header = "B";
                        String s2 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString();//selOrg
                        header = "G";
                        int s3 = Convert.ToInt32(xlSh.get_Range("G" + i.ToString(), "G" + i.ToString()).Value2);//cust
                        header = "H";
                        String s4 = xlSh.get_Range("H" + i.ToString(), "H" + i.ToString()).Value2.ToString();//refDoc
                        header = "I";
                        String rep_month = xlSh.get_Range("I" + i.ToString(), "I" + i.ToString()).Value.ToString().Split('.')[0].Remove(0, 1);
                        String rep_year = xlSh.get_Range("I" + i.ToString(), "I" + i.ToString()).Value.ToString().Split('.')[1];
                        DateTime s5 = Convert.ToDateTime(rep_year + "-" + rep_month + "-01"); //date(period)
                        header = "J";
                        DateTime s6 = Convert.ToDateTime(xlSh.get_Range("J" + i.ToString(), "J" + i.ToString()).Value.ToString());//create
                        header = "K";
                        var s71 = xlSh.get_Range("K" + i.ToString(), "K" + i.ToString()).Value.ToString();//invoce
                        DateTime s7 = Convert.ToDateTime("1000-01-01");
                        if (s71 != null)
                            s7 = Convert.ToDateTime(s71);
                        header = "L";
                        String s8 = xlSh.get_Range("L" + i.ToString(), "L" + i.ToString()).Value2.ToString();//docNum
                        header = "M";
                        String s9 = xlSh.get_Range("M" + i.ToString(), "M" + i.ToString()).Value2.ToString();//salOrd
                        header = "N";
                        var s10 = xlSh.get_Range("N" + i.ToString(), "N" + i.ToString()).Value;//cancelDoc
                        if (s10 == null)
                            s10 = "";
                        header = "Q";
                        String s11 = Convert.ToString(xlSh.get_Range("Q" + i.ToString(), "Q" + i.ToString()).Value2).Remove(2);//SDiv
                        header = "P";
                        String s12 = xlSh.get_Range("P" + i.ToString(), "P" + i.ToString()).Value2.ToString();//SBA
                        header = "Q";
                        String s13 = Convert.ToString(xlSh.get_Range("Q" + i.ToString(), "Q" + i.ToString()).Value2).Remove(0, 6);//MSG
                        header = "R";
                        String s14 = xlSh.get_Range("R" + i.ToString(), "R" + i.ToString()).Value2.ToString();//Material
                        header = "S";
                        double s15 = Convert.ToDouble(xlSh.get_Range("S" + i.ToString(), "S" + i.ToString()).Value2.ToString());//salesQuan
                        header = "T";
                        int s16 = Convert.ToInt32(xlSh.get_Range("T" + i.ToString(), "T" + i.ToString()).Value2.ToString());//quanBum
                        header = "U";
                        double s17 = Convert.ToDouble(xlSh.get_Range("U" + i.ToString(), "U" + i.ToString()).Value2.ToString());//costGood
                        header = "V";
                        double s18 = Convert.ToDouble(xlSh.get_Range("V" + i.ToString(), "V" + i.ToString()).Value2.ToString());//salesRev
                        header = "W";
                        String s19 = xlSh.get_Range("W" + i.ToString(), "W" + i.ToString()).Value2.ToString();//currency
                        header = "X";
                        String s20 = xlSh.get_Range("X" + i.ToString(), "X" + i.ToString()).Value2.ToString();//salesDist - SubRegion
                        header = "Y";
                        var s21 = xlSh.get_Range("Y" + i.ToString(), "Y" + i.ToString()).Value2;//salesOffice - Region
                        if (s21 == null)
                            s21 = "";
                        header = "AA";
                        double s24 = Convert.ToDouble(xlSh.get_Range("AA" + i.ToString(), "AA" + i.ToString()).Value2.ToString());//COGS Customer Orders
                        header = "AB";
                        double s25 = Convert.ToDouble(xlSh.get_Range("AB" + i.ToString(), "AB" + i.ToString()).Value2.ToString());//Variable Production Costs
                        header = "AC";
                        double s26 = Convert.ToDouble(xlSh.get_Range("AC" + i.ToString(), "AC" + i.ToString()).Value2.ToString());//Fixed Production Costs
                        header = "AD";
                        double s27 = Convert.ToDouble(xlSh.get_Range("AD" + i.ToString(), "AD" + i.ToString()).Value2.ToString());//C_Cost of goods 
                        header = "AE";
                        double s28 = Convert.ToDouble(xlSh.get_Range("AE" + i.ToString(), "AE" + i.ToString()).Value2.ToString());//Standard Customer bo



                        if (((s20 == "RU3700") || (s20 == "RU4400")) && (s21.ToString() == "RUW2"))
                            s21 = "RUW3";

                        if ((s20 == "RU8600") && (s21.ToString() == "RUI6"))
                            s21 = "RUI6.1";

                        if ((s20 == "RU7000") && (s21.ToString() == "RUI3"))
                            s21 = "RUI3.1";

                        if ((s20 == "RU4700") && (s21.ToString() == "RUG2"))
                            s21 = "RUG5";

                        if ((s20 == "RU1800") && (s21.ToString() == "RUI4"))
                            s21 = "RUI4.1";

                        if ((s20 == "RU5600") && (s21.ToString() == "RUI7"))
                            s21 = "RUI7.1";

                        header = "Q";
                        String s22 = Convert.ToString(xlSh.get_Range("Q" + i.ToString(), "Q" + i.ToString()).Value2).Remove(0, 3).Remove(2);//MMG
                        header = "Q";
                        String s23 = xlSh.get_Range("Q" + i.ToString(), "Q" + i.ToString()).Value2.ToString();

                        if ((s23 == "AE 71 132") || (s23 == "AE 71 134") || (s23 == "AE 71 138") || (s23 == "AE 71 140") || (s23 == "AE 71 142"))
                            s23 = "HC";//SDiv_ap
                        else if ((s23 == "HC 36 197") && (s12 == "26"))
                        {
                            s12 = "36";
                            s13 = "783";
                            s22 = "78";
                            s23 = "AE";
                        }
                        else
                            s23 = s11;

                        DataTable dt1 = sql1.GetRecords("exec InsReport 0, 0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29", 3, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25, s26, s27, s28);
                        if (dt1 == null)
                        {
                            xlSh.Cells[i, 34] = "Ошибка";
                            err++;
                        }
                        i++;
                    }

                    sql1.GetRecords("exec InsKosCustFromReport");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void loadUsersForIT()
        {
            if (globalData.Region == null)
                globalData.Region = "";
            if (globalData.RD == null)
                globalData.RD = "";

            formatFP();

            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec SelUsersForIT @p1, @p2", globalData.Region, globalData.RD);

            if (dt1 == null)
                return;

            String temp = "";

            foreach (DataRow row in dt1.Rows)
            {
                temp = "";
                if ((temp != String.Empty) && (row.ItemArray[4].ToString() != ""))
                    temp += ", " + row.ItemArray[4].ToString();
                else if (row.ItemArray[4].ToString() != "")
                    temp = row.ItemArray[4].ToString();
                if ((temp != String.Empty) && (row.ItemArray[5].ToString() != ""))
                    temp += ", " + row.ItemArray[5].ToString();
                else if (row.ItemArray[5].ToString() != "")
                    temp = row.ItemArray[5].ToString();
                if ((temp != String.Empty) && (row.ItemArray[6].ToString() != ""))
                    temp += ", " + row.ItemArray[6].ToString();
                else if (row.ItemArray[6].ToString() != "")
                    temp = row.ItemArray[6].ToString();

                _dgv11.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], temp);
            }
        }

        private void formatFP()
        {
            _dgv11.Columns.Clear();
            _dgv11.Columns.Add("user_name", "Имя пользователя");
            _dgv11.Columns["user_name"].ReadOnly = true;
            _dgv11.Columns["user_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            _dgv11.Columns.Add("user_login", "Логин");
            _dgv11.Columns["user_login"].ReadOnly = true;
            _dgv11.Columns["user_login"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            _dgv11.Columns.Add("role_name", "Роль");
            _dgv11.Columns["role_name"].ReadOnly = true;
            _dgv11.Columns["role_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            _dgv11.Columns.Add("user_email", "e-mail");
            _dgv11.Columns["user_email"].ReadOnly = true;
            _dgv11.Columns["user_email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            _dgv11.Columns.Add("sdiv", "Дивизион");
            _dgv11.Columns["sdiv"].ReadOnly = true;
            _dgv11.Columns["sdiv"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void tabPage10_Enter(object sender, EventArgs e)
        {
            _cms1.Items[1].Enabled = false;
            _cms1.Items[3].Enabled = false;
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
            _cms1.Items[1].Enabled = true;
            _cms1.Items[3].Enabled = true;
        }

        private void _dgv11_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                fUser fu = new fUser(_dgv11[0, e.RowIndex].Value.ToString(), _dgv11[1, e.RowIndex].Value.ToString(), _dgv11[2, e.RowIndex].Value.ToString());
                fu.ShowDialog();
            }
            catch { }
        }

        private void selPSAcc()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelPSAcc @p1, @p2", globalData.Div, dateTimePicker6.Value.Year.ToString() + "-" + dateTimePicker6.Value.Month.ToString() + "-01");

            if (dt1 == null)
                return;

            _dgv12.DataSource = dt1;

            formatPSAcc();
        }

        private void formatPSAcc()
        {
            _dgv12.Columns[0].Visible = false;

            _dgv12.Columns["Регион"].ReadOnly = true;
            _dgv12.Columns["Регион"].Width = 150;

            _dgv12.Columns["Пользователь"].ReadOnly = true;
            _dgv12.Columns["Пользователь"].Width = 150;

            _dgv12.Columns["Динамика продаж"].ReadOnly = true;
            _dgv12.Columns["Динамика продаж"].Width = 100;
            _dgv12.Columns["Динамика продаж"].DefaultCellStyle.Format = "N3";

            _dgv12.Columns["Ассортиментный план"].ReadOnly = true;
            _dgv12.Columns["Ассортиментный план"].Width = 100;
            _dgv12.Columns["Ассортиментный план"].DefaultCellStyle.Format = "N3";

            _dgv12.Columns["Разница"].ReadOnly = true;
            _dgv12.Columns["Разница"].Width = 100;
            _dgv12.Columns["Разница"].DefaultCellStyle.Format = "N3";
        }

        private void _dgv11_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv11);
        }

        private void _dgv12_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv12);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            selPSAcc();
        }

        private void SelAllReport()
        {
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            fillTableHeader("All", _dgv13);

            if ((!filter) || (fillFilter))
            {
                for (int j = 0; j < 6; j++)
                    f[j] = String.Empty;
                if (cbRegionRep.Items.Count > 0)
                {
                    cbRegionRep.SelectedIndex = 0;
                    cbCustRep.SelectedIndex = 0;
                    cbSBARep.SelectedIndex = 0;
                }
            }

            dt1 = sql1.GetRecords("exec SelAllReport @p1, @p2, @p3, @p4", dateTimePicker7.Value.Year.ToString() + "-" + dateTimePicker7.Value.Month.ToString() + "-" + "01", f[0], f[1], f[2]);

            _dgv13.Rows.Clear();

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataRow row in dt1.Rows)
            {
                _dgv13.Rows.Add(row.ItemArray);
            }

            if (fillFilter)
            {
                load = false;
                fillComboBox(dt1, "cust", cbCustRep, 1);
                fillComboBox(dt1, "SBA", cbSBARep, 2);
                
                var query = (from c in dt1.AsEnumerable()
                             orderby c.Field<String>("reg_nameRus") ascending
                             select new
                             {
                                 Name = c.Field<String>("reg_nameRus")
                             }).Distinct();
                if (query.Count() != 0)
                {
                    cbRegionRep.Items.Clear();
                    cbRegionRep.Items.Add("все");
                    for (int i = 0; i < query.Count(); i++)
                    {
                        cbRegionRep.Items.Add(query.ElementAt(i).Name);
                    }
                    if (f[0] == String.Empty)
                        cbRegionRep.SelectedIndex = 0;
                    cbRegionRep.Enabled = true;
                }


                if (dt1.Rows.Count > 0)
                {
                    if (f[0] != String.Empty)
                        cbRegionRep.SelectedIndex = 1;
                    else
                        cbRegionRep.SelectedIndex = 0;
                    if (f[1] != String.Empty)
                        cbCustRep.SelectedIndex = 1;
                    else
                        cbCustRep.SelectedIndex = 0;                    
                    if (f[2] != String.Empty)
                        cbSBARep.SelectedIndex = 1;
                    else
                        cbSBARep.SelectedIndex = 0;
                }
                load = true;
                fillFilter = false;
                filter = true;
            }

            SetStatusStrip(true, "Строк: " + dt1.Rows.Count);

            if (subsum != 0)
            {
                int temp = subsum;
                subsum++;
                SubSumAllReport(temp);
            }
        }

        private void _dgv13_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SubSumAllReport(e.ColumnIndex);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            fillFilter = true;
            SelAllReport();
        }

        private void SubSumAllReport(int colnum)
        {
            try
            {
                Double sum = 0, sum2 = 0, sum3 = 0, sum4 = 0, sum5 = 0, sum6 = 0;
                Double msum = 0, msum2 = 0, msum3 = 0, msum4 = 0, msum5 = 0, msum6 = 0;

                for (int j = 0; j < _dgv13.Rows.Count; j++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row = _dgv13.Rows[j];
                    if ((row.DefaultCellStyle.BackColor == bbgreen1) || (row.DefaultCellStyle.BackColor == bbgreen3))
                    {
                        _dgv13.Rows.Remove(row);
                        j--;
                    }
                }

                if (subsum == colnum)
                {
                    subsum = 0;
                    return;
                }

                _dgv13.Sort(_dgv13.Columns[colnum], ListSortDirection.Ascending);
                String s1 = _dgv13.Rows[0].Cells[colnum].Value.ToString();

                int i = 0;
                foreach (DataGridViewRow row in _dgv13.Rows)
                {
                    if ((row.Cells[colnum].Value.ToString() != s1) && (s1 != ""))
                    {
                        s1 = _dgv13.Rows[i].Cells[colnum].Value.ToString();
                        
                        _dgv13.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3, sum4, sum5, sum6);
                        _dgv13.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                        msum += sum;
                        msum2 += sum2;
                        msum3 += sum3;
                        msum4 += sum4;
                        msum5 += sum5;
                        msum6 += sum6;
                        sum = 0;
                        sum2 = 0;
                        sum3 = 0;
                        sum4 = 0;
                        sum5 = 0;
                        sum6 = 0;
                    }
                    else if (s1 != "")
                    {
                        sum += Convert.ToDouble(row.Cells["rep_SalesQuan"].Value.ToString());
                        sum2 += Convert.ToDouble(row.Cells["rep_SalesQuanBum"].Value.ToString());
                        sum3 += Convert.ToDouble(row.Cells["rep_Cost"].Value.ToString());
                        sum4 += Convert.ToDouble(row.Cells["rep_CostRub"].Value.ToString());
                        sum5 += Convert.ToDouble(row.Cells["rep_SalesRev"].Value.ToString());
                        sum6 += Convert.ToDouble(row.Cells["rep_SalesRevRub"].Value.ToString());
                    }
                    i++;
                }
                msum += sum;
                msum2 += sum2;
                msum3 += sum3;
                msum4 += sum4;
                msum5 += sum5;
                msum6 += sum6;

                _dgv13.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3, sum4, sum5, sum6);
                _dgv13.Rows.Insert(i + 1, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", msum, msum2, msum3, msum4, msum5, msum6);
                _dgv13.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                _dgv13.Rows[i + 1].DefaultCellStyle.BackColor = bbgreen1;
                subsum = colnum;
            }
            catch
            {
                subsum = 0;
            }
        }

        private void deleteRepPS(DataGridView dgv)
        {
            Sql sql1 = new Sql();

            if (chbOnlyPS.Checked)
            {
                DialogResult dr = MessageBox.Show("Удалить выделенные строки?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (DataGridViewCell cell in dgv.SelectedCells)
                    {
                        if (dgv.Rows[cell.RowIndex].Cells[0].Value.ToString() == String.Empty)
                        {
                            dgv.Rows[cell.RowIndex].Cells["fact"].Value = String.Empty;
                        }
                        else
                        {
                            begval = Convert.ToInt32(dgv.Rows[cell.RowIndex].Cells["fact"].Value);
                            dgv.Rows[cell.RowIndex].Cells["fact"].Value = 0;
                            dgv.Rows[cell.RowIndex].Cells["bum"].Value = begval - Convert.ToInt32(dgv.Rows[cell.RowIndex].Cells["fact"].Value.ToString()) + Convert.ToInt32(dgv.Rows[cell.RowIndex].Cells["bum"].Value.ToString());
                            dgv.Rows[cell.RowIndex].Cells["sumSale"].Value = Convert.ToDouble(dgv.Rows[cell.RowIndex].Cells["fact"].Value.ToString()) * Convert.ToDouble(dgv.Rows[cell.RowIndex].Cells["UnitPrice"].Value);
                            dgv.Rows[cell.RowIndex].Cells["sumSaleRub"].Value = Convert.ToDouble(dgv.Rows[cell.RowIndex].Cells["fact"].Value.ToString()) * Convert.ToDouble(dgv.Rows[cell.RowIndex].Cells["UnitPriceRub"].Value);
                        }
                    }
                }
            }
            else if (globalData.UserAccess == 1)
            {
                DialogResult dr = MessageBox.Show("Удалить выделенные строки? (также будут удалены, распределённые по ЛПУ продажи)", "Удаление строки отчёта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (DataGridViewCell cell in dgv.SelectedCells)
                    {
                        if (dgv.Rows[cell.RowIndex].Cells[0].Value.ToString() != String.Empty)
                        {
                            sql1.GetRecords("exec DelReport @p1, @p2, @p3", dgv.Rows[cell.RowIndex].Cells[0].Value.ToString(), dgv.Rows[cell.RowIndex].Cells[1].Value.ToString(), globalData.UserID2);
                        }
                    }
                }
            }
        }

        private void _dgv13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                deleteRepPS(_dgv13);
            }
        }

        private void aeReport(string idRep, string idDB)
        {
            if ((globalData.UserAccess == 1) || (globalData.UserAccess == 5) || (globalData.UserAccess == 6))
            {
                AEReport aerep = new AEReport(idRep, idDB);
                aerep.ShowDialog();
            }
        }

        private void btnHideUsersMA_Click(object sender, EventArgs e)
        {
            if (btnHideUsersMA.Text == "Скрыть РП")
            {
                cbUsersMA.Visible = false;
                btnHideUsersMA.Text = "Отобразить РП";
            }
            else
            {
                cbUsersMA.Visible = true;
                btnHideUsersMA.Text = "Скрыть РП";
            }
            fillLPUMA();
            SelMA();
        }

        private void btnHideRegMA_Click(object sender, EventArgs e)
        {
            if (btnHideRegMA.Text == "Скрыть регионы")
            {
                cbRegMA.Visible = false;
                btnHideRegMA.Text = "Отобразить регионы";
                //btnHideUsersMA.Visible = false;
                //cbUsersMA.Visible = false;
            }
            else
            {
                cbRegMA.Visible = true;
                btnHideRegMA.Text = "Скрыть регионы";
                //btnHideUsersMA.Visible = true;
                /*
                if (btnHideUsersMA.Text == "Скрыть пользователей")
                    cbUsersMA.Visible = true;
                else
                    cbUsersMA.Visible = false;
                */
            }

            fillUsersMA();
            fillLPUMA();
            SelMA();
        }

        void loadTheme()
        {
            Sql sql1 = new Sql();

            globalData.update = false;
            cbThemeMA.DataSource = sql1.GetRecords("exec MarkAct_Select_ConfTheme");
            cbThemeMA.DisplayMember = "confth_name";
            cbThemeMA.ValueMember = "confth_id";
            globalData.update = true;
        }

        private void SetVisMA()
        {
            if (tabControl1.SelectedIndex != 7 || (tabControl1.SelectedIndex == 7 && ma_new - ma_old != 0))
            {
                tabControl1.SelectedIndex = 7;
                tabControl1.Visible = true;

                btnHideRegMA.Text = "Скрыть регионы";
                btnHideUsersMA.Text = "Скрыть РП";
                cbUsersMA.Visible = true;

                loadMAType(cbTypeMA);
                loadTheme();

                if (globalData.RD != String.Empty)
                {
                    btnMA_fill.Visible = false;
                    btnMA_edit.Visible = false;
                    btnMA_del.Visible = false;
                    cbRegMA.Visible = true;
                    btnHideRegMA.Visible = true;
                    btnHideUsersMA.Visible = true;
                }
                else if (globalData.Region != String.Empty)
                {
                    EnableSave("ma");
                    btnMA_fill.Visible = true;
                    btnMA_edit.Visible = true;
                    btnMA_del.Visible = true;
                    cbRegMA.Visible = false;
                    btnHideRegMA.Visible = false;
                    btnHideUsersMA.Visible = false;
                    fillLPUMA();
                }
                else
                {
                    btnMA_fill.Visible = false;
                    btnMA_edit.Visible = false;
                    btnMA_del.Visible = false;
                    cbRegMA.Visible = true;
                    btnHideRegMA.Visible = true;
                    btnHideUsersMA.Visible = true;
                }
            }
            SelMA();
        }

        private void SelMA()
        {

            Cursor = Cursors.WaitCursor;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            _dgv8.DataSource = null;

            string userID = "0";
            if (cbUsersMA.Visible)
                userID = cbUsersMA.SelectedValue.ToString();

            string reg = "0";
            if (globalData.Region == String.Empty)// && globalData.RD != String.Empty)
            {
                if (cbRegMA.Visible)
                    reg = cbRegMA.SelectedValue.ToString();
            }
            else
                reg = globalData.Region;

            string lpu = "-1";
            if (cbLPUMA.Visible && cbLPUMA.SelectedItem != null)
                lpu = cbLPUMA.SelectedValue.ToString();

            dt1 = sql1.GetRecords("exec SelMALPU @p1, @p2, @p3, @p4, @p5, @p6, @p7", globalData.Div, reg, userID, cbYearMA.SelectedItem, globalData.RD, lpu, cbTypeMA.SelectedValue);

            _dgv8.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось загрузить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            formatMarkAct(_dgv8);
            Cursor = Cursors.Default;
        }

        private void formatMarkAct(DataGridView _tempdgv)
        {
            foreach (DataGridViewColumn col in _tempdgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
            }
            _tempdgv.Columns["ma_id"].Visible = false;
            _tempdgv.Columns["db_id"].Visible = false;
            _tempdgv.Columns["num"].Visible = false;
            _tempdgv.Columns["matype_name"].HeaderText = "Название типа";
            _tempdgv.Columns["conf_name"].HeaderText = "Название конференции";
            _tempdgv.Columns["confth_name"].HeaderText = "Тематика конференции";
            _tempdgv.Columns["ma_name"].HeaderText = "Комментарий";
            _tempdgv.Columns["ma_plan1"].HeaderText = "План, квартал 1";
            _tempdgv.Columns["ma_plan2"].HeaderText = "План, квартал 2";
            _tempdgv.Columns["ma_plan3"].HeaderText = "План, квартал 3";
            _tempdgv.Columns["ma_plan4"].HeaderText = "План, квартал 4";
            _tempdgv.Columns["ma_fact1"].HeaderText = "Факт, квартал 1";
            _tempdgv.Columns["ma_fact2"].HeaderText = "Факт, квартал 2";
            _tempdgv.Columns["ma_fact3"].HeaderText = "Факт, квартал 3";
            _tempdgv.Columns["ma_fact4"].HeaderText = "Факт, квартал 4";
            _tempdgv.Columns["ma_plan_total"].HeaderText = "План, Итого";
            _tempdgv.Columns["ma_fact_total"].HeaderText = "Факт, Итого";
            _tempdgv.Columns["lpu_sname"].HeaderText = "ЛПУ";

            _tempdgv.Columns["matype_name"].Width = 200;
            _tempdgv.Columns["matype_name"].Frozen = true;

            _tempdgv.Columns["ma_name"].Width = 100;
            _tempdgv.Columns["ma_name"].Frozen = true;

            _tempdgv.Columns["conf_name"].Visible = false;
            _tempdgv.Columns["conf_name"].Width = 200;

            _tempdgv.Columns["confth_name"].Visible = false;
            _tempdgv.Columns["confth_name"].Width = 200;

            for (int i = 7; i < _tempdgv.ColumnCount - 1; i++)
            {
                if ((i % 2) == 1)
                    _tempdgv.Columns[i].DefaultCellStyle.BackColor = bbgray5;
                _tempdgv.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                _tempdgv.Columns[i].DefaultCellStyle.Format = "N2";
            }

            int j = 0;

            while (_tempdgv.Rows[j].Cells["num"].Value.ToString() != "2")
                j++;
            _tempdgv.Rows[j].DefaultCellStyle.BackColor = bbgreen3;
        }

        private void SendMail(string fileName)
        {
            try
            {
                Sql sql1 = new Sql();

                TreeNode tn1 = new TreeNode();
                tn1 = treeView1.SelectedNode.Parent;

                string subject = "Выгрузка из RegionalReport"; //заголовок
                string body = ""; //письмо
                string fromAddress = "finance.ru@bbraun.com"; //адрес отправителя
                string toAddress = String.Empty; //адрес получателя

                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec GetUserByID @p1", globalData.UserID);

                if (globalData.UserAccess == 1)
                {
                    InputDialog ind = new InputDialog("e-mail", "Введите e-mail для отправки", false);
                    ind.ShowDialog();
                    toAddress = globalData.input;
                }
                else
                    toAddress = dt1.Rows[0].ItemArray[1].ToString();

                if ((toAddress == null) || (toAddress == String.Empty) || (toAddress == "0"))
                {
                    MessageBox.Show("Невозможно отправить письмо. Ваш e-mail не был в системе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string password = ""; //пароль на почтовом сервере
                string userName = ""; //логин на почтовом сервере
                string host = "relay.peterstar.net";
                int port = 25;

                /*
                String filename = "";
                if ((tn1.Parent.Text == "Анализ продаж") && (checkBox1.Checked))
                    filename = @"C:\Temp\Личные продажи по региону " + clsData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";
                else
                    filename = @"C:\Temp\" + tn1.Text + " по региону " + clsData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";
                */

                Attachment att = new Attachment(fileName);
                MailMessage msg = new MailMessage(new MailAddress(fromAddress), new MailAddress(toAddress));

                msg.Subject = subject;

                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.Body = body;

                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = false;
                msg.Attachments.Add(att);
                SmtpClient client = new SmtpClient(host, port);

                client.Credentials = new System.Net.NetworkCredential(userName, password);

                //client.EnableSsl = true;

                client.Send(msg);
                msg.Dispose();
                MessageBox.Show("Выгрузка успешно выслана на вашу почту.", "Отправлено", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SmtpException err)
            {
                MessageBox.Show("Системная ошибка - " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void загрузитьEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelDoc excelDoc = new ExcelDoc(openFileDialog.FileName);
                int err = 0;
                int i = 2;
                try
                {
                    
                    bool quest = true;
                    string s1;
                    clQuestion question = new clQuestion(globalData.UserID);

                    while (excelDoc.getValue("A" + i.ToString(), "A" + i.ToString()) != null)
                    {
                        s1 = excelDoc.getValue("A" + i.ToString(), "A" + i.ToString()).ToString().Replace("\"", "'");

                        if (quest)
                        {
                            question = new clQuestion(globalData.UserID);
                            question.Text = s1;
                            question.Save();
                            
                            quest = false;
                        }
                        else
                        {
                            clAnswer answer = question.CreateAnswed();
                            answer.Right = (excelDoc.getValue("B" + i.ToString(), "B" + i.ToString()) != null);
                            answer.Text = s1;
                            answer.Save();
                        }

                        i++;

                        if (excelDoc.getValue("A" + i.ToString(), "A" + i.ToString()) == null)
                        {
                            i++;
                            quest = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В строке " + i.ToString());
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        excelDoc.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.");
                        excelDoc.Show();
                    }
                }
            }
        }

        private void ExportInExcel(DataGridView dgv)
        {
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

            TreeNode tn1 = new TreeNode();

            if (treeView1.SelectedNode.Parent != null)
                tn1 = treeView1.SelectedNode.Parent;
            else
                tn1 = treeView1.SelectedNode;

            try
            {
                int i = 1;

                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Columns[j].Visible)
                    {
                        xlSh.Cells[1, i] = dgv.Columns[j].HeaderText;
                        i++;
                    }
                }

                if (tn1.Text == "Анализ продаж")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 12;
                    ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 20;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 25;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 12;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 50;
                    ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 4;
                    ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 20;
                    ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 45;
                    ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 20;
                    ((Excel.Range)xlSh.Columns[13]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[14]).ColumnWidth = 20;
                    ((Excel.Range)xlSh.Columns[15]).ColumnWidth = 22;
                    ((Excel.Range)xlSh.Columns[16]).ColumnWidth = 21;
                    ((Excel.Range)xlSh.Columns[17]).ColumnWidth = 50;
                }
                else if (tn1.Text == "Косвенные продажи")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 12;
                    ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 30;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 30;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 30;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 40;
                    ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 30;
                    ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 40;
                    ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 12;
                    ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 13;
                    ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 15;
                }
                else if (tn1.Text == "Ассортиментный план")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 46;
                    ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 5;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[13]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[14]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[15]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[16]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[17]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[18]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[19]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[20]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[21]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[22]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[23]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[24]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[25]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[26]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[27]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[28]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[29]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[30]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[31]).ColumnWidth = 10;

                    ((Excel.Range)xlSh.Columns[32]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[33]).ColumnWidth = 10;
                }
                else if (tn1.Text == "Динамика продаж")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 62;
                    ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[13]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[14]).ColumnWidth = 10;
                }
                else if (tn1.Text == "Маркетинговые мероприятия")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 50;
                    ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 25;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 25;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 50;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 16;
                    ((Excel.Range)xlSh.Columns[13]).ColumnWidth = 12;
                    ((Excel.Range)xlSh.Columns[14]).ColumnWidth = 12;
                    ((Excel.Range)xlSh.Columns[15]).ColumnWidth = 30;                    
                }
                else if (tn1.Text == "Выполнение плана")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 28;
                    if(globalData.Div == "HC")
                        ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 5;
                    else
                        ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 20;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 15;
                    ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 15;
                }
                else if (tn1.Text == "Отчёты дистрибьюторов")
                {
                    ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 10;
                    ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 18;
                    ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 25;
                    ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 20;
                    ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 35;
                    ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 25;
                    ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 12;
                }
                i = 2;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    int k = 1;
                    if (row.DefaultCellStyle.BackColor == bbgreen3)
                    {
                        ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen3.ToArgb();
                        ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                    }
                    if (row.DefaultCellStyle.BackColor == bbgreen1)
                    {
                        ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen1.ToArgb();
                        ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                    }

                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        if (dgv.Columns[j].Visible)
                        {
                            if (row.Cells[j].Value != null)
                            {
                                if ((dgv.Columns[j].HeaderText == "Дата") && (row.Cells[j].Value.ToString() != String.Empty))
                                    xlSh.Cells[i, k] = Convert.ToDateTime(row.Cells[j].Value).ToShortDateString();
                                else
                                {
                                    if ((digit(row.Cells[j].Value.ToString())) &&
                                        ((dgv.Columns[j].HeaderText == "Продажи без НДС") ||
                                        (dgv.Columns[j].HeaderText == "Сумма продаж (евро)") ||
                                        (dgv.Columns[j].HeaderText == "Сумма продаж (руб.)")))
                                        xlSh.Cells[i, k] = Math.Round(float.Parse(row.Cells[j].Value.ToString()), 2);
                                    else
                                        xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                                }
                            }
                            k++;
                        }
                    }
                    i++;
                }
                if (globalData.db_id == 1)
                {
                    String filename = "";

                    Sql sql1 = new Sql();
                    DataTable dt1 = new DataTable();
                    dt1 = sql1.GetRecords("exec GetUserByID @p1", globalData.UserID);

                    if (tn1.Text == "Отчёты дистрибьюторов")
                        filename = @"C:\Temp\Отчеты дистрибьюторов (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";
                    else if ((tn1.Parent.Text == "Анализ продаж") && (chbOnlyPS.Checked))
                        filename = @"C:\Temp\Личные продажи по региону " + globalData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";
                    else                     
                        filename = @"C:\Temp\" + tn1.Text + " по региону " + globalData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";
                    
                    xlApp.DisplayAlerts = false;
                    xlWB.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal);
                    xlWB.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlSh);
                    releaseObject(xlWB);
                    releaseObject(xlApp);
                    
                    SendMail(filename);
                    System.IO.File.Delete(filename);
                }
                else
                {
                    xlApp.Visible = true;
                }
            }
            catch (Exception err)
            {
                try
                {
                    xlWB.Close(false, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlSh);
                    releaseObject(xlWB);
                    releaseObject(xlApp);
                }
                catch (Exception err2)
                {
                    MessageBox.Show("Ошибка при очистки объекта. Системная ошибка: " + err2.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcelAP_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv1);
        }

        private void btnExcelRepKos_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv3);
        }

        private void btnExcelAcc_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv4);
        }

        private void btnExcelDyn_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv5);
        }

        private void btnExcelMA_Click(object sender, EventArgs e)
        {
            int flag = 0;
            _dgv8.Columns["conf_name"].Visible = true;
            _dgv8.Columns["confth_name"].Visible = true;

            if (_dgv8.Columns["ma_name"].Visible == false)
            {
                _dgv8.Columns["ma_name"].Visible = true;
                flag = 1;
            }

            ExportInExcel(_dgv8);

            _dgv8.Columns["conf_name"].Visible = false;
            _dgv8.Columns["confth_name"].Visible = false;
            if (flag == 1)
                _dgv8.Columns["ma_name"].Visible = false;
        }

        private void _dgv13_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv13);
        }

        private void cbRegionRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (cbRegionRep.SelectedItem.ToString() != "все")
                {
                    String str = cbRegionRep.SelectedItem.ToString();
                    str = str.Split(' ')[0];
                    f[0] = str;
                    filter = true;
                    SelAllReport();
                }
                else
                {
                    f[0] = "";
                    SelAllReport();
                }
            }
        }

        private void cbCustRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (cbCustRep.SelectedItem.ToString() != "все")
                {
                    String str = cbCustRep.SelectedItem.ToString();
                    str = str.Split(' ')[0];
                    f[1] = str;
                    filter = true;
                    SelAllReport();
                }
                else
                {
                    f[1] = "";
                    SelAllReport();
                }
            }
        }

        private void cbSBARep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                if (cbSBARep.SelectedItem.ToString() != "все")
                {
                    String str = cbSBARep.SelectedItem.ToString();
                    str = str.Split(' ')[0];
                    f[2] = str;
                    filter = true;
                    SelAllReport();
                }
                else
                {
                    f[2] = "";
                    SelAllReport();
                }
            }
        }

        private void btnClearFilterRep_Click(object sender, EventArgs e)
        {
            filter = false;
            SelAllReport();
        }

        private void MatForBtnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Buttons btn1 = new Buttons();
            btn1.ShowDialog();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            filter = true;

            if (load)
            {
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();

                TreeNode tn1 = new TreeNode();
                tn1 = treeView1.SelectedNode.Parent.Parent.Parent.Parent;

                if (globalData.UserAccess == 5)
                {
                    bool all = false;

                    if (tn1.Text == "Все регионы")
                        all = true;

                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else if (treeView1.SelectedNode.Parent.Parent.Parent.Text == "Российская федерация")
                    dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker1.Value.Year);
                cbUsers.DataSource = dt1;
                cbUsers.DisplayMember = "user_name";
                cbUsers.ValueMember = "user_id";
            }

            loadData();
        }

        private void btnEditReport_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in _dgv1.SelectedCells)
                aeReport(_dgv1.Rows[cell.RowIndex].Cells[0].Value.ToString(), _dgv1.Rows[cell.RowIndex].Cells[1].Value.ToString());
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            aeReport(_dgv13.Rows[_dgv13.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), _dgv13.Rows[_dgv13.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
        }

        private void insertKosCustFromReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            sql1.GetRecords("exec InsKosCustFromReport");

            MessageBox.Show("Список косвенных покупателей обновлён", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnApplyKosRep_Click(object sender, EventArgs e)
        {
            if (load)
            {
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();

                if (globalData.UserAccess == 5)
                {
                    bool all = false;

                    TreeNode tn1 = new TreeNode();
                    tn1 = treeView1.SelectedNode.Parent.Parent.Parent;

                    if (tn1.Text == "Все регионы")
                        all = true;

                    if (all)
                        dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker3.Value.Year);
                    else
                        dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                }
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker3.Value.Year);
                cbUsers3.DataSource = dt1;
                cbUsers3.DisplayMember = "user_name";
                cbUsers3.ValueMember = "user_id";
            }
            loadData();
        }

        private bool digit(String str)
        {
            try
            {
                str = str.Trim();
                float f = float.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void divideToolStripMenuItem_Click(object sender, EventArgs e)
        {           
            InputDialog ind = new InputDialog("Разделение продажи", "Количество", true);
            ind.ShowDialog();

            int count = Convert.ToInt32(globalData.input);

            if (count == 0)
                return;

            Sql sql1 = new Sql();
            sql1.GetRecords("exec divideReport @p1, @p2, @p3, @p4", _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value,
                _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[1].Value, count, globalData.UserID);

            loadData();
        }

        private void _dgv1_MouseDown(object sender, MouseEventArgs e)
        {
            /*
            var h = _dgv1.HitTest(e.X, e.Y);
            if (h.Type == DataGridViewHitTestType.Cell)
                _dgv1.CurrentCell = _dgv1[point.X, point.Y];
            */
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorkerParams param = (BackgroundWorkerParams)e.Argument;

            int lpu_id = Convert.ToInt32(e.Argument);

            Sql sql1 = new Sql();
            sql1.GetRecords("exec fillFactAcc @p1, @p2, @p3", lpu_id, globalData.Div, globalData.CurDate);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This event handler is called when the background thread finishes.
            // This method runs on the main thread.
            if (e.Error != null)
                MessageBox.Show("Error: " + e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Word counting canceled.");
            else
            {
                lock (locker)
                    prBar1.Value += 1;
                fillPrBar();
            }
        }

        private void CancelUpdAcc()
        {
            if (MessageBox.Show("Остановить обновление ассортиментного плана?", "Обновление",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (BackgroundWorker worker in bgw)
                {
                    worker.CancelAsync();
                }
                prBar1.Visible = false;
            }
        }

        private void btnCancelUpdAcc_Click(object sender, EventArgs e)
        {
            CancelUpdAcc();
        }

        private void btnCancelUpdAcc2_Click(object sender, EventArgs e)
        {
            CancelUpdAcc();
        }

        private void btnSaveAcc_Click(object sender, EventArgs e)
        {
            try
            {
                Sql sql1 = new Sql();

                load = false;

                if (_dgv4.Rows[0].Cells["acc_id"].Value.ToString() != "0")
                {
                    foreach (DataGridViewRow row in _dgv4.Rows)
                    {
                        if (row.Cells["upd"].Value != null)
                        {
                            if (row.Cells["vc_sum1"].Value.ToString() == "")
                                row.Cells["vc_sum1"].Value = 0;
                            sql1.GetRecords("exec fillVC @p1, @p2, @p3, @p4", row.Cells["acc_id"].Value, cbYearAcc.SelectedItem.ToString() + "-01-01", row.Cells["vc_sum1"].Value, globalData.UserID);
                            load = true;
                        }
                        if (row.Cells["upd2"].Value != null)
                        {
                            if (row.Cells["vc_sum2"].Value.ToString() == "")
                                row.Cells["vc_sum2"].Value = 0;
                            sql1.GetRecords("exec fillVC @p1, @p2, @p3, @p4", row.Cells["acc_id"].Value, cbYearAcc.SelectedItem.ToString() + "-04-01", row.Cells["vc_sum2"].Value, globalData.UserID);
                            load = true;
                        }
                        if (row.Cells["upd3"].Value != null)
                        {
                            if (row.Cells["vc_sum3"].Value.ToString() == "")
                                row.Cells["vc_sum3"].Value = 0;
                            sql1.GetRecords("exec fillVC @p1, @p2, @p3, @p4", row.Cells["acc_id"].Value, cbYearAcc.SelectedItem.ToString() + "-07-01", row.Cells["vc_sum3"].Value, globalData.UserID);
                            load = true;
                        }
                        if (row.Cells["upd4"].Value != null)
                        {
                            if (row.Cells["vc_sum4"].Value.ToString() == "")
                                row.Cells["vc_sum4"].Value = 0;
                            sql1.GetRecords("exec fillVC @p1, @p2, @p3, @p4", row.Cells["acc_id"].Value, cbYearAcc.SelectedItem.ToString() + "-10-01", row.Cells["vc_sum4"].Value, globalData.UserID);
                            load = true;
                        }

                    }
                }
                MessageBox.Show("Информация по конкурентам сохранена.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (load)
                {
                    fillLPU(cbLPU, cbUsers2.SelectedValue.ToString(), globalData.Region, cbYearAcc.SelectedItem.ToString());
                    SelRegAcc();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить информацию по конкурентам, системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _dgv4_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                String cell = "";
                if (e.ColumnIndex == 42)
                    cell = "vc_sum1";
                else if (e.ColumnIndex == 45)
                    cell = "vc_sum2";
                else if (e.ColumnIndex == 48)
                    cell = "vc_sum3";
                else if (e.ColumnIndex == 51)
                    cell = "vc_sum4";

                if (globalData.Div == "HC")
                {
                    if (_dgv4.CurrentRow.Cells["nom_type"].Value.ToString() == "шт")
                        begval = Convert.ToInt32(_dgv4.CurrentRow.Cells[cell].Value.ToString());
                    else
                        begval2 = Convert.ToDouble(_dgv4.CurrentRow.Cells[cell].Value.ToString());
                }
                else
                    begval2 = Convert.ToInt32(_dgv4.CurrentRow.Cells[cell].Value.ToString());

            }
            catch
            {
                begval = 0;
                begval2 = 0;
            }
        }

        private void _dgv4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _dgv4EndEdit(e.ColumnIndex, e.RowIndex);
        }

        private void _dgv4EndEdit(int ColumnIndex, int RowIndex)
        {
            String cell = "";
            if (ColumnIndex == 46)
                cell = "vc_sum1";
            else if (ColumnIndex == 49)
                cell = "vc_sum2";
            else if (ColumnIndex == 52)
                cell = "vc_sum3";
            else if (ColumnIndex == 55)
                cell = "vc_sum4";

            bool subsum = false;
            subsum = SubSumVC(ColumnIndex, RowIndex);

            if (!subsum)
            {
                _dgv4.Rows[RowIndex].Cells[cell].Value = "0";
                
                if (cell == "vc_sum1")
                    _dgv4.Rows[RowIndex].Cells["upd"].Value = "";
                
                else if (cell == "vc_sum2")
                    _dgv4.Rows[RowIndex].Cells["upd2"].Value = "";

                else if (cell == "vc_sum3")
                    _dgv4.Rows[RowIndex].Cells["upd3"].Value = "";
                
                else 
                    _dgv4.Rows[RowIndex].Cells["upd4"].Value = "";

                SubSumVC(ColumnIndex, RowIndex);
                return;
            }

            try
            {
                if (globalData.Div == "HC")
                {
                    if ((_dgv4.Rows[RowIndex].Cells["nom_type"].Value.ToString() == "шт") && (_dgv4.Rows[RowIndex].Cells[cell].Value == null))
                    {
                        if (begval != 0)
                        {
                            _dgv4.Rows[RowIndex].Cells[cell].Value = "";

                            if (cell == "vc_sum1")
                                _dgv4.Rows[RowIndex].Cells["upd"].Value = "1";
                            
                            else if (cell == "vc_sum2")
                                _dgv4.Rows[RowIndex].Cells["upd2"].Value = "1";
                            
                            else if (cell == "vc_sum3")
                                _dgv4.Rows[RowIndex].Cells["upd3"].Value = "1";
                            
                            else
                                _dgv4.Rows[RowIndex].Cells["upd4"].Value = "1";
                        }
                            
                    }
                    else if ((_dgv4.Rows[RowIndex].Cells["nom_type"].Value.ToString() == "euro") && (_dgv4.Rows[RowIndex].Cells[cell].Value == null))
                    {
                        if (begval2 != 0)
                        {
                            _dgv4.Rows[RowIndex].Cells[cell].Value = "";
                            if (cell == "vc_sum1")
                                _dgv4.Rows[RowIndex].Cells["upd"].Value = "1";

                            else if (cell == "vc_sum2")
                                _dgv4.Rows[RowIndex].Cells["upd2"].Value = "1";

                            else if (cell == "vc_sum3")
                                _dgv4.Rows[RowIndex].Cells["upd3"].Value = "1";

                            else
                                _dgv4.Rows[RowIndex].Cells["upd4"].Value = "1";
                        }
                    }
                    else if (_dgv4.Rows[RowIndex].Cells[cell].Value.ToString() != "")
                    {
                        if (cell == "vc_sum1")
                            _dgv4.Rows[RowIndex].Cells["upd"].Value = "1";

                        else if (cell == "vc_sum2")
                            _dgv4.Rows[RowIndex].Cells["upd2"].Value = "1";

                        else if (cell == "vc_sum3")
                            _dgv4.Rows[RowIndex].Cells["upd3"].Value = "1";

                        else
                            _dgv4.Rows[RowIndex].Cells["upd4"].Value = "1";
                    }
                }
                else
                {
                    if ((_dgv4.Rows[RowIndex].Cells[cell].Value == null) && (begval2 != 0))
                    {
                        _dgv4.Rows[RowIndex].Cells[cell].Value = "";
                        if (cell == "vc_sum1")
                            _dgv4.Rows[RowIndex].Cells["upd"].Value = "1";

                        else if (cell == "vc_sum2")
                            _dgv4.Rows[RowIndex].Cells["upd2"].Value = "1";

                        else if (cell == "vc_sum3")
                            _dgv4.Rows[RowIndex].Cells["upd3"].Value = "1";

                        else
                            _dgv4.Rows[RowIndex].Cells["upd4"].Value = "1";
                    }
                    else if (_dgv4.Rows[RowIndex].Cells[cell].Value != null)
                    {
                        if (cell == "vc_sum1")
                            _dgv4.Rows[RowIndex].Cells["upd"].Value = "1";

                        else if (cell == "vc_sum2")
                            _dgv4.Rows[RowIndex].Cells["upd2"].Value = "1";

                        else if (cell == "vc_sum3")
                            _dgv4.Rows[RowIndex].Cells["upd3"].Value = "1";

                        else
                            _dgv4.Rows[RowIndex].Cells["upd4"].Value = "1";
                    }
                }                
            }
            catch
            {
                _dgv4.Rows[RowIndex].Cells[cell].Value = "";
                if (cell == "vc_sum1")
                    _dgv4.Rows[RowIndex].Cells["upd"].Value = "";

                else if (cell == "vc_sum2")
                    _dgv4.Rows[RowIndex].Cells["upd2"].Value = "";

                else if (cell == "vc_sum3")
                    _dgv4.Rows[RowIndex].Cells["upd3"].Value = "";

                else
                    _dgv4.Rows[RowIndex].Cells["upd4"].Value = "";
            }
        }

        private bool SubSumVC(int ColumnIndex, int RowIndex)
        {
            try
            {
                int sum = 0;
                Double sum2 = 0;
                Boolean sht = false;
                int i = RowIndex;
                String cell = "";
                int greenindex = 0;

                if (ColumnIndex == 46)
                    cell = "vc_sum1";
                else if (ColumnIndex == 49)
                    cell = "vc_sum2";
                else if (ColumnIndex == 52)
                    cell = "vc_sum3";
                else if (ColumnIndex == 55)
                    cell = "vc_sum4";

                if ((globalData.Div == "HC") && (_dgv4.Rows[RowIndex].Cells["nom_type"].Value.ToString() == "шт"))
                    sht = true;

                while (_dgv4.Rows[i].DefaultCellStyle.BackColor != bbgreen3)
                {
                    if ((_dgv4.Rows[i].Cells[cell].Value != null) && (sht))
                    {
                        if (_dgv4.Rows[i].Cells[cell].Value.ToString() != "")
                            sum += Convert.ToInt32(_dgv4.Rows[i].Cells[cell].Value.ToString());
                    }
                    else if ((_dgv4.Rows[i].Cells[cell].Value != null) && (!sht))
                    {
                        if (_dgv4.Rows[i].Cells[cell].Value.ToString() != "")
                            sum2 += Convert.ToDouble(_dgv4.Rows[i].Cells[cell].Value.ToString());
                    }
                    i--;
                }

                greenindex = i;

                i = RowIndex + 1;

                while (_dgv4.Rows[i].DefaultCellStyle.BackColor != bbgreen3)
                {
                    if ((_dgv4.Rows[i].Cells[cell].Value != null) && (sht))
                    {
                        if (_dgv4.Rows[i].Cells[cell].Value.ToString() != "")
                            sum += Convert.ToInt32(_dgv4.Rows[i].Cells[cell].Value.ToString());
                    }
                    else if ((_dgv4.Rows[i].Cells[cell].Value != null) && (!sht))
                    {
                        if (_dgv4.Rows[i].Cells[cell].Value.ToString() != "")
                            sum2 += Convert.ToDouble(_dgv4.Rows[i].Cells[cell].Value.ToString());
                    }
                    i++;
                }

                Boolean upd = false;

                if (sht)
                {

                    if (_dgv4.Rows[greenindex].Cells[cell].Value.ToString() == "")
                        upd = true;
                    else if (Convert.ToInt32(_dgv4.Rows[greenindex].Cells[cell].Value) != sum)
                        upd = true;

                    if (upd)
                    {
                        _dgv4.Rows[greenindex].Cells[cell].Value = sum;
                        if (cell == "vc_sum1")
                            _dgv4.Rows[greenindex].Cells["upd"].Value = "1";
                        else if (cell == "vc_sum2")
                            _dgv4.Rows[greenindex].Cells["upd2"].Value = "1";
                        else if (cell == "vc_sum3")
                            _dgv4.Rows[greenindex].Cells["upd3"].Value = "1";
                        else
                            _dgv4.Rows[greenindex].Cells["upd4"].Value = "1";
                    }
                }
                else
                {
                    if (_dgv4.Rows[greenindex].Cells[cell].Value.ToString() == "")
                        upd = true;
                    else if (Convert.ToInt32(_dgv4.Rows[greenindex].Cells[cell].Value) != sum2)
                        upd = true;

                    if (upd)
                    {
                        if (RowIndex != greenindex)
                            _dgv4.Rows[greenindex].Cells[cell].Value = sum2;
                        if (cell == "vc_sum1")
                            _dgv4.Rows[greenindex].Cells["upd"].Value = "1";
                        else if (cell == "vc_sum2")
                            _dgv4.Rows[greenindex].Cells["upd2"].Value = "1";
                        else if (cell == "vc_sum3")
                            _dgv4.Rows[greenindex].Cells["upd3"].Value = "1";
                        else
                            _dgv4.Rows[greenindex].Cells["upd4"].Value = "1";
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void selRepKosAll()
        {
            int idReg = 0;
            if (cbRegAllKos.Visible == false)
                idReg = 0;
            else
                idReg = Convert.ToInt32(cbRegAllKos.SelectedValue.ToString());

            int idUser = 0;
            if (cbUsersAllKos.Visible == false)
                idUser = 0;
            else
                idUser = Convert.ToInt32(cbUsersAllKos.SelectedValue.ToString());

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelRepKosAll @p1, @p2, @p3, @p4, @p5", globalData.Div, idReg, idUser, dateTimePicker8.Value.Year.ToString() + "-" + dateTimePicker8.Value.Month.ToString() + "-01", dateTimePicker9.Value.Year.ToString() + "-" + dateTimePicker9.Value.Month.ToString() + "-01");

            if (dt1 == null)
                return;

            formatRepKosAll(_dgv15);

            _dgv15.Rows.Clear();

            foreach (DataRow row in dt1.Rows)
                _dgv15.Rows.Add(row.ItemArray);
        }

        private void formatRepKosAll(DataGridView dgv)
        {
            if (dgv.Columns.Count == 0)
            {
                dgv.Columns.Add("rep_id", "rep_id");
                dgv.Columns["rep_id"].Visible = false;
                dgv.Columns.Add("db_id", "db_id");
                dgv.Columns["db_id"].Visible = false;

                dgv.Columns.Add("user_name", "Пользователь");
                dgv.Columns["user_name"].ReadOnly = true;
                dgv.Columns["user_name"].Width = 100;
                dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("rep_date", "Дата");
                dgv.Columns["rep_date"].ReadOnly = true;
                dgv.Columns["rep_date"].Width = 100;
                dgv.Columns["rep_date"].DefaultCellStyle.Format = "MM - yyyy";
                dgv.Columns["rep_date"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("comp_nameRus", "Компания");
                dgv.Columns["comp_nameRus"].ReadOnly = true;
                dgv.Columns["comp_nameRus"].Width = 100;
                dgv.Columns["comp_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("cust_code", "Покупатель (код)");
                dgv.Columns["cust_code"].ReadOnly = true;
                dgv.Columns["cust_code"].Width = 150;
                dgv.Columns["cust_code"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("cust_name", "Покупатель");
                dgv.Columns["cust_name"].ReadOnly = true;
                dgv.Columns["cust_name"].Width = 150;
                dgv.Columns["cust_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("rep_dist", "Промежуточный\nдистрибьютор");
                dgv.Columns["rep_dist"].ReadOnly = true;
                dgv.Columns["rep_dist"].Width = 150;
                dgv.Columns["rep_dist"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("subreg_nameRus", "Субрегион");
                dgv.Columns["subreg_nameRus"].ReadOnly = true;
                dgv.Columns["subreg_nameRus"].Width = 150;
                dgv.Columns["subreg_nameRus"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("lpu_sname", "ЛПУ");
                dgv.Columns["lpu_sname"].Width = 100;
                dgv.Columns["lpu_sname"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("nom_group", "Группа товаров");
                dgv.Columns["nom_group"].ReadOnly = true;
                dgv.Columns["nom_group"].Width = 100;
                dgv.Columns["nom_group"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("nom_name", "Номенклатура");
                dgv.Columns["nom_name"].ReadOnly = true;
                dgv.Columns["nom_name"].Width = 100;
                dgv.Columns["nom_name"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("rep_SalesQuanBum", "Количество");
                dgv.Columns["rep_SalesQuanBum"].ReadOnly = true;
                dgv.Columns["rep_SalesQuanBum"].Width = 100;
                dgv.Columns["rep_SalesQuanBum"].DefaultCellStyle.Format = "N0";
                dgv.Columns["rep_SalesQuanBum"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("rep_SalesRev", "Сумма (евро)");
                dgv.Columns["rep_SalesRev"].ReadOnly = true;
                dgv.Columns["rep_SalesRev"].Width = 100;
                dgv.Columns["rep_SalesRev"].DefaultCellStyle.Format = "N2";
                dgv.Columns["rep_SalesRev"].SortMode = DataGridViewColumnSortMode.Programmatic;

                dgv.Columns.Add("rep_SalesRevRub", "Сумма (рубли)");
                dgv.Columns["rep_SalesRevRub"].ReadOnly = true;
                dgv.Columns["rep_SalesRevRub"].Width = 100;
                dgv.Columns["rep_SalesRevRub"].DefaultCellStyle.Format = "N2";
                dgv.Columns["rep_SalesRevRub"].SortMode = DataGridViewColumnSortMode.Programmatic;
            }

            if (globalData.Div == "OM")
                dgv.Columns["lpu_sname"].ReadOnly = false;
            else
                dgv.Columns["lpu_sname"].ReadOnly = false;
        }

        private void btnHideUsersAP_Click(object sender, EventArgs e)
        {
            if (btnHideUsersAP.Text == "Скрыть")
            {
                cbUsers.Visible = false;
                btnHideUsersAP.Text = "Отобразить";
            }
            else
            {
                cbUsers.Visible = true;
                btnHideUsersAP.Text = "Скрыть";
            }

            loadData();
        }

        private void reConnect()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            String s1 = globalData.Login;

            dt1 = sql1.GetRecords("exec UserLogin @p1, @p2", s1, globalData.UserAccess);
            if (dt1.Rows.Count == 0)
            {
                //MessageBox.Show("Пользователь не найден в системе. Вход будет произведён под учётной записью гостя.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                s1 = "anonimus";
            }

            if (s1 == "anonimus")
                dt1 = sql1.GetRecords("exec UserLogin @p1, @p2", s1, globalData.UserAccess);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("Не удалось войти в систему.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                String str = dt1.Rows[0].ItemArray[0].ToString();
                globalData.UserID = Convert.ToInt32(str);
                str = dt1.Rows[0].ItemArray[1].ToString();
                globalData.UserAccess = Convert.ToInt32(str);
                //clsData.UserEdit = Convert.ToInt32(dt1.Rows[0].ItemArray[2]);
                globalData.fp = Convert.ToInt32(dt1.Rows[0].ItemArray[4]);
                globalData.UserName = dt1.Rows[0].ItemArray[3].ToString();
                str = "Здравствуйте, " + globalData.UserName + ".";

                if ((globalData.UserAccess == 1) && (globalData.UserID == 1))
                {
                    str = "Рад видеть тебя, мой Создатель.";
                }

                str += "  Последний раз вы были в системе: " + sql1.GetRecordsOne("exec LastEvent @p1", globalData.UserID);

                lbUserName.Text = str;
                globalData.Login = s1;
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://portal.bbraun.ru/Pages/help/help.aspx");
        }

        private void fillPrBar()
        {
            if (prBar1.Value == prBar1.Maximum)
            {
                prBar1.Visible = false;

                loadData();
                MessageBox.Show("Ассортиментный план обновлён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }

        private void SelAccNY()
        {
            Sql sql1 = new Sql();
            DataSet ds1 = new DataSet();

            EnableSave("accNY");

            tabControl1.SelectedIndex = 14;
            tabControl1.Visible = true;

            if (globalData.UserAccess == 1)
            {
                btnSaveAccNYDilCost.Visible = true;
                button71.Visible = true;
            }
            else
            {
                btnSaveAccNYDilCost.Visible = false;
                button71.Visible = false;
            }

            //load = false;

            fillLPU(cbLPUAccNY, cbUsersAccNY.SelectedValue.ToString(), globalData.Region, globalData.year);

            //ds1 = sql1.GetRecordsDS("exec selLPU @p1, @p2, @p3, @p4", cbUsersAccNY.SelectedValue, clsData.Region, clsData.Div, clsData.CurDate.Year);
            /*
        if (ds1 == null)
        {
            MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            load = true;
            return;
        }

        cbLPUAccNY.DataSource = ds1.Tables[0].DefaultView;
        cbLPUAccNY.DisplayMember = "lpu_sname";
        cbLPUAccNY.ValueMember = "ulpu_id";

        load = true;
            */
            if (globalData.Div == "AE")
                groupBox1.Visible = false;
            else
            {
                groupBox1.Visible = true;

                if (globalData.UserAccess != 1)
                {
                    string move = String.Empty;
                    move = sql1.GetRecordsOne("exec GetSettingsMove");
                   
                    if (move == "0")
                        groupBox1.Enabled = false;
                    else
                        groupBox1.Enabled = true;
                }
            }

            if (cbLPUAccNY.Items.Count > 0)
                cbLPUAccNY.Enabled = true;
            else
                cbLPUAccNY.Enabled = false;

            btnHideLPUAccNY.Text = "Скрыть ЛПУ";
            btnHideUsersAccNY.Text = "Скрыть пользователей";
            lbUsersAccNY.Visible = true;
            lbLPUAccNY.Visible = true;
            cbLPUAccNY.Visible = true;
            cbUsersAccNY.Visible = true;
            btnHideLPUAccNY.Visible = true;
            loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }

        private void loadAccPlanByLPUNY(DataGridView dgv, ComboBox cbLPU)
        {
            if (cbLPU.SelectedValue != null)
            {
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();

                if (globalData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccPlanNYAE @p1, @p2, '', 0", cbLPU.SelectedValue, cbUsers2.SelectedValue);
                else /*TODO: SelAccPlanNYnew*/
                    dt1 = sql1.GetRecords("exec SelAccPlanNYWithRub @p1, @p2, '', 0", cbLPU.SelectedValue, cbUsers2.SelectedValue);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAccNY(dt1, dgv);
            }
        }

        private void fillAccNY(DataTable dt, DataGridView dgv)
        {
            fillTableHeader("AccNY", dgv);

            foreach (DataRow row in dt.Rows)
            {
                dgv.Rows.Add(row.ItemArray);
            }

            formatAccNY(dgv);
        }

        private void formatAccNY(DataGridView dgv)
        {
            //-------------------------------------------------------------------
            if (globalData.UserID == 258)
                dgv.Columns["acc_id"].Visible = true;
            else
                dgv.Columns["acc_id"].Visible = false;
    
            dgv.Columns["nom_id"].Visible = false;
            dgv.Columns["nom_group"].Visible = false;
            dgv.Columns["nom_seq"].Visible = false;
            dgv.Columns["nom_year1"].Visible = false;
            dgv.Columns["nom_year2"].Visible = false;
            dgv.Columns["upd"].Visible = false;

            dgv.Columns["py3"].Visible = true;
            dgv.Columns["py1"].Visible = true;

            dgv.Columns["py3"].DefaultCellStyle.Format = "N2";
            dgv.Columns["py1"].DefaultCellStyle.Format = "N2";
            dgv.Columns["py4"].DefaultCellStyle.Format = "N0";
            dgv.Columns["py2"].DefaultCellStyle.Format = "N0";

            if (globalData.Div == "HC")
            {
                dgv.Columns["py4"].Visible = true;
                dgv.Columns["py2"].Visible = true;
                dgv.Columns["nom_type"].Visible = true;
                dgv.Columns["nom_type"].Width = 35;
                dgv.Columns["nom_type"].Frozen = true;

                dgv.Columns["lydilcost"].Visible = true;
                dgv.Columns["lydilcost"].Width = 65;

                dgv.Columns["cydilcost"].Visible = true;
                dgv.Columns["cydilcost"].Width = 65;

                dgv.Columns["lyplan"].Visible = true;
                dgv.Columns["lyfact"].Visible = true;
                dgv.Columns["cyplan"].Visible = true;
                dgv.Columns["pr"].Visible = true;
                dgv.Columns["prCyLyEuro"].Visible = false;
                dgv.Columns["cyDilCostRub"].Visible = true;
                dgv.Columns["cyPlanRub"].Visible = true;

                dgv.Columns["lyplanEuro"].DefaultCellStyle.Format = "N2";
                dgv.Columns["cyplanEuro"].DefaultCellStyle.Format = "N2";
                dgv.Columns["cyPlanRub"].DefaultCellStyle.Format = "N2";
                dgv.Columns["cyDilCostRub"].DefaultCellStyle.Format = "N2";

                dgv.Columns["nom_type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["lydilcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["cydilcost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns["cyDilCostRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               

                dgv.Columns["pr"].DefaultCellStyle.Format = "N2";
                dgv.Columns["pr"].Width = 50;
            }
            else
            {
                dgv.Columns["py4"].Visible = false;
                dgv.Columns["py2"].Visible = false;

                dgv.Columns["nom_type"].Visible = false;
                dgv.Columns["lydilcost"].Visible = false;
                dgv.Columns["cydilcost"].Visible = false;
                dgv.Columns["lyplan"].Visible = false;
                dgv.Columns["lyfact"].Visible = false;
                dgv.Columns["cyplan"].Visible = false;                
                dgv.Columns["pr"].Visible = false;
                dgv.Columns["cyDilCostRub"].Visible = false;
                dgv.Columns["cyPlanRub"].Visible = false;
                dgv.Columns["prCyLyEuro"].Visible = true;
            }

            dgv.Columns["lyplan"].DefaultCellStyle.Format = "N0";
            dgv.Columns["lyplan"].Width = 70;
            dgv.Columns["lyplanEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["lyplanEuro"].Width = 100;
            dgv.Columns["lyfact"].DefaultCellStyle.Format = "N0";
            dgv.Columns["lyfact"].Width = 70;
            dgv.Columns["lyfactEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["lyfactEuro"].Width = 100;
            dgv.Columns["cyplan"].DefaultCellStyle.Format = "N0";
            dgv.Columns["cyplan"].Width = 70;
            dgv.Columns["cyplanEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cyplanEuro"].Width = 100;
            dgv.Columns["prEuro"].DefaultCellStyle.Format = "N2";
            dgv.Columns["prEuro"].Width = 50;


            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.Programmatic;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            dgv.Columns["nom"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.Columns["nom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.Columns["nom"].Width = 250;
            dgv.Columns["nom"].Frozen = true;
            
            Sql sql1 = new Sql();
            DataTable dtNY = sql1.GetRecords("exec GetSettings");

            int year = Convert.ToInt32(dtNY.Rows[0].ItemArray[3].ToString());
            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == 0)
                {
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = bbgreen3;
                    row.DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == -1)
                {
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = bbgreen1;
                    row.DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_year2"].Value) <= (year-1))
                {
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = bbgray4;
                }
                if ((Convert.ToInt32(row.Cells["nom_year1"].Value) == year) && (Convert.ToInt32(row.Cells["nom_year2"].Value) == year) && (Convert.ToInt32(row.Cells["nom_group"].Value) != 0))
                {
                    row.DefaultCellStyle.BackColor = bbgray5;
                }
                if (Convert.ToInt32(row.Cells["nom_id"].Value) == 3)
                {
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = bbgreen3;
                }
            }

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.ReadOnly = true;
            }

            if (globalData.Div == "HC")
            {
                if (cbUsersAccNY.Visible)
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (((Convert.ToInt32(row.Cells["nom_group"].Value) != 0) && (Convert.ToInt32(row.Cells["nom_group"].Value) != -1)) || (Convert.ToInt32(row.Cells["nom_id"].Value) == 156))
                        {
                            if (row.Cells["nom_type"].Value.ToString() == "шт")
                            {
                                //if ((clsData.UserAccess == 1) || (clsData.UserAccess == 4) || (clsData.Region == "Пермь") || (clsData.Region == "Екатеринбург") || (clsData.Region == "Тюмень ХМАО") || (clsData.Region == "Петрозаводск") || (clsData.Region == "Самара") || (clsData.Region == "Челябинск")) //открыть всем для внесения в новом году
                                    row.Cells["cyplan"].ReadOnly = false;

                                row.Cells["cyplanEuro"].ReadOnly = true;
                                if (globalData.UserAccess == 1)
                                    row.Cells["cyDilCost"].ReadOnly = false;
                            }
                            else
                            {
                                if (Convert.ToInt32(row.Cells["nom_id"].Value) == 156)
                                {
                                    //if ((clsData.UserAccess == 1) || (clsData.UserAccess == 4) || (clsData.Region == "Пермь") || (clsData.Region == "Екатеринбург") || (clsData.Region == "Тюмень ХМАО") || (clsData.Region == "Петрозаводск") || (clsData.Region == "Самара") || (clsData.Region == "Челябинск")) //открыть всем для внесения в новом году
                                        row.Cells["cyplanEuro"].ReadOnly = false;
                                }
                                row.Cells["cyplan"].ReadOnly = true;
                                //if ((clsData.UserAccess == 1) || (clsData.UserAccess == 4) || (clsData.Region == "Пермь") || (clsData.Region == "Екатеринбург") || (clsData.Region == "Тюмень ХМАО") || (clsData.Region == "Петрозаводск") || (clsData.Region == "Самара") || (clsData.Region == "Челябинск")) //открыть всем для внесения в новом году
                                    row.Cells["cyplanEuro"].ReadOnly = false;
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        row.Cells["cyplan"].ReadOnly = true;
                        row.Cells["cyplanEuro"].ReadOnly = true;
                    }
                }
            }
            else 
            {
                if (cbLPUAccNY.Visible)
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if ((Convert.ToInt32(row.Cells["nom_group"].Value) != 0) && (Convert.ToInt32(row.Cells["nom_group"].Value) != -1))
                        {
                            if (cbUsersAccNY.Visible)
                                //if ((clsData.UserAccess == 1) || (clsData.UserAccess == 4) || (clsData.Region == "Пермь") || (clsData.Region == "Екатеринбург") || (clsData.Region == "Тюмень ХМАО") || (clsData.Region == "Петрозаводск") || (clsData.Region == "Самара") || (clsData.Region == "Челябинск")) //открыть всем для внесения в новом году
                                    dgv.Columns["cyplanEuro"].ReadOnly = false;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        row.Cells["cyplanEuro"].ReadOnly = true;
                    }
                }
            }
        }

        private void cbUsersAccNY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                SelAccNY();
        }

        private void cbLPUAccNY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }

        private void btnHideLPUAccNY_Click(object sender, EventArgs e)
        {
            if (btnHideLPUAccNY.Text == "Скрыть ЛПУ")
            {
                btnHideLPUAccNY.Text = "Показать ЛПУ";
                lbLPUAccNY.Visible = false;
                cbLPUAccNY.Visible = false;
                btnSaveAccNY.Enabled = false;
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();
                if (globalData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccPlanNYAE 0, @p1, @p2, 0", cbUsersAccNY.SelectedValue, globalData.Region);
                else
                    dt1 = sql1.GetRecords("exec SelAccPlanNYWithRub 0, @p1, @p2, 0", cbUsersAccNY.SelectedValue, globalData.Region);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAccNY(dt1, _dgv14);
            }
            else
            {
                btnHideLPUAccNY.Text = "Скрыть ЛПУ";
                lbLPUAccNY.Visible = true;
                cbLPUAccNY.Visible = true;
                btnSaveAccNY.Enabled = true;
                loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
            }
        }

        private void btnHideUsersAccNY_Click(object sender, EventArgs e)
        {
            if (btnHideUsersAccNY.Text == "Скрыть пользователей")
            {
                btnHideUsersAccNY.Text = "Показать пользователей";
                lbUsersAccNY.Visible = false;
                lbLPUAccNY.Visible = false;
                cbLPUAccNY.Visible = false;
                cbUsersAccNY.Visible = false;
                btnHideLPUAccNY.Visible = true;
                btnSaveAccNY.Enabled = false;
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();

                if (globalData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccPlanNYAE 0, 0, @p1, 0", globalData.Region);
                else
                    dt1 = sql1.GetRecords("exec SelAccPlanNYWithRub 0, 0, @p1, 0", globalData.Region);
                //_dgv4.DataSource = dt1;

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAccNY(dt1, _dgv14);
            }
            else
            {
                if (btnHideLPUAccNY.Text == "Скрыть ЛПУ")
                {
                    lbLPUAccNY.Visible = true;
                    cbLPUAccNY.Visible = true;
                    loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
                }
                else
                {
                    btnHideLPUAccNY.Text = "Скрыть ЛПУ";
                    btnHideLPUAccNY_Click(null, null);
                }
                btnHideLPUAccNY.Visible = true;
                btnHideUsersAccNY.Text = "Скрыть пользователей";
                lbUsersAccNY.Visible = true;
                cbUsersAccNY.Visible = true;
                btnSaveAccNY.Enabled = true;
            }
        }
        

        private void fillUsersAllKos()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", cbRegAllKos.SelectedValue, globalData.Div, dateTimePicker8.Value.Year);

            load = false;

            cbUsersAllKos.DataSource = dt1;
            cbUsersAllKos.DisplayMember = "user_name";
            cbUsersAllKos.ValueMember = "user_id";
            
            load = true;

            selRepKosAll();
        }

        private void btnHideRegAllKos_Click(object sender, EventArgs e)
        {
            if (btnHideRegAllKos.Text == "Скрыть регион")
            {
                btnHideRegAllKos.Text = "Показать регион";
                cbRegAllKos.Visible = false;
                cbUsersAllKos.Visible = false;
                label20.Visible = false;
                label19.Visible = false;
                btnHideUsersAllKos.Visible = false;
            }
            else
            {
                btnHideRegAllKos.Text = "Скрыть регион";
                cbRegAllKos.Visible = true;
                label19.Visible = true;
                btnHideUsersAllKos.Text = "Показать пользователей";
                btnHideUsersAllKos.Visible = true;
            }
            selRepKosAll();
        }

        private void btnHideUsersAllKos_Click(object sender, EventArgs e)
        {
            if (btnHideUsersAllKos.Text == "Скрыть пользователей")
            {
                btnHideUsersAllKos.Text = "Показать пользователей";
                cbUsersAllKos.Visible = false;
                label20.Visible = false;
            }
            else
            {
                btnHideUsersAllKos.Text = "Скрыть пользователей";
                cbUsersAllKos.Visible = true;
                label20.Visible = true;
            }
            selRepKosAll();
        }

        private void cbRegAllKos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                fillUsersAllKos();
        }

        private void cbUsersAllKos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                selRepKosAll();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (load)
            {
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();

                if (globalData.UserAccess == 5)
                    dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                else
                    dt1 = sql1.GetRecords("exec selUsers @p1, @p2, @p3", globalData.Region, globalData.Div, dateTimePicker8.Value.Year);
                cbUsersAllKos.DataSource = dt1;
                cbUsersAllKos.DisplayMember = "user_name";
                cbUsersAllKos.ValueMember = "user_id";
            }
            selRepKosAll();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if(_dgv15.Rows.Count > 0)
                EditRow(_dgv15, null);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (_dgv15.Rows.Count > 0)
                DelRow(_dgv15);
        }

        private void DelRow(DataGridView dgv)
        {
            try
            {
                //DataGridViewCell cell = dgv.SelectedCells[0];

                Sql sql1 = new Sql();

                foreach (DataGridViewCell cell in dgv.SelectedCells)
                    sql1.GetRecords("exec DelReportKos @p1, @p2, @p3", dgv.Rows[cell.RowIndex].Cells["rep_id"].Value.ToString(), dgv.Rows[cell.RowIndex].Cells["db_id"].Value.ToString(), globalData.UserID);

                loadData();
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось удалить продажу. Системная ошибка - " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _dgv15_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 12)
                subsumKosSales(e.ColumnIndex, _dgv15, 12);
        }

        private void _dgv14_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _dgv14EndEdit(e.ColumnIndex, e.RowIndex);
        }

        private void _dgv14EndEdit(int columnIndex, int rowIndex)
        {
            try
            {
                if (columnIndex == 15)
                {
                    _dgv14.Rows[rowIndex].Cells["cyplanEuro"].Value = Convert.ToInt32(_dgv14.Rows[rowIndex].Cells["cyplan"].Value) * Convert.ToDouble(_dgv14.Rows[rowIndex].Cells["cydilcost"].Value);
                    _dgv14.Rows[rowIndex].Cells["cyplanRub"].Value = Convert.ToInt32(_dgv14.Rows[rowIndex].Cells["cyplan"].Value) * Convert.ToDouble(_dgv14.Rows[rowIndex].Cells["cydilcostRub"].Value);
                }
                if (columnIndex == 16)
                {
                    _dgv14.Rows[rowIndex].Cells["cyDilCost"].Value = Convert.ToDouble(_dgv14.Rows[rowIndex].Cells["cyDilCost"].Value);
                }
                if (columnIndex == 17)
                {
                    _dgv14.Rows[rowIndex].Cells["cyplanEuro"].Value = Convert.ToDouble(_dgv14.Rows[rowIndex].Cells["cyplanEuro"].Value);
                }
            }
            catch
            {
                if (columnIndex == 17)
                    _dgv14.Rows[rowIndex].Cells["cyDilCost"].Value = begval2;
                else
                {
                    _dgv14.Rows[rowIndex].Cells["cyplan"].Value = "0";
                    _dgv14.Rows[rowIndex].Cells["cyplanEuro"].Value = "0";
                }
            }
            finally
            {
                SubSumAccNY(columnIndex, rowIndex);
            }
        }

        /*TODO: добавлена sumRub*/
        private void SubSumAccNY(int ColumnIndex, int RowIndex)
        {
            try
            {
                int sum = 0;
                Double sumEuro = 0;
                Double sumRub = 0;
                int i = RowIndex;
                int greenindex = 0;
                bool cicle2 = true;

                if (_dgv14.Rows[RowIndex].DefaultCellStyle.BackColor == bbgreen3)
                {
                    _dgv14.Rows[RowIndex].Cells["upd"].Value = "1";
                    return;
                }

                if (Convert.ToInt32(_dgv14.Rows[i].Cells["nom_id"].Value) < 6)
                {
                    SubSumAccNYN3(RowIndex);
                    return;
                }

                if ((Convert.ToInt32(_dgv14.Rows[i].Cells["nom_id"].Value) > 175) && (Convert.ToInt32(_dgv14.Rows[i].Cells["nom_id"].Value) < 179))
                {
                    i = 5;
                    cicle2 = false;
                }

                while (_dgv14.Rows[i].DefaultCellStyle.BackColor != bbgreen3)
                {
                    if (_dgv14.Rows[i].Cells["cyplan"].Value != null)
                    {
                        if (_dgv14.Rows[i].Cells["cyplan"].Value.ToString() != "")
                            sum += Convert.ToInt32(_dgv14.Rows[i].Cells["cyplan"].Value.ToString());
                    }
                    if (_dgv14.Rows[i].Cells["cyplanEuro"].Value != null)
                    {
                        if (_dgv14.Rows[i].Cells["cyplanEuro"].Value.ToString() != "")
                            sumEuro += Convert.ToDouble(_dgv14.Rows[i].Cells["cyplanEuro"].Value.ToString());
                    }
                    if (_dgv14.Rows[i].Cells["cyplanRub"].Value != null)
                    {
                        if (_dgv14.Rows[i].Cells["cyplanRub"].Value.ToString() != "")
                            sumRub += Convert.ToDouble(_dgv14.Rows[i].Cells["cyplanRub"].Value.ToString());
                    }
                    i--;
                }

                greenindex = i;


                if ((cicle2) && (RowIndex + 2 != _dgv14.Rows.Count))
                {
                    i = RowIndex + 1;

                    while (_dgv14.Rows[i].DefaultCellStyle.BackColor != bbgreen3)
                    {
                        if (_dgv14.Rows[i].Cells["cyplan"].Value != null)
                        {
                            if (_dgv14.Rows[i].Cells["cyplan"].Value.ToString() != "")
                                sum += Convert.ToInt32(_dgv14.Rows[i].Cells["cyplan"].Value.ToString());
                        }
                        if (_dgv14.Rows[i].Cells["cyplanEuro"].Value != null)
                        {
                            if (_dgv14.Rows[i].Cells["cyplanEuro"].Value.ToString() != "")
                                sumEuro += Convert.ToDouble(_dgv14.Rows[i].Cells["cyplanEuro"].Value.ToString());
                        }
                        if (_dgv14.Rows[i].Cells["cyplanRub"].Value != null)
                        {
                            if (_dgv14.Rows[i].Cells["cyplanRub"].Value.ToString() != "")
                                sumRub += Convert.ToDouble(_dgv14.Rows[i].Cells["cyplanRub"].Value.ToString());
                        }
                        i++;
                    }
                }

                Boolean upd = false;

                if (_dgv14.Rows[greenindex].Cells["cyplan"].Value.ToString() == "")
                    upd = true;
                else if (Convert.ToInt32(_dgv14.Rows[greenindex].Cells["cyplan"].Value) != sum)
                    upd = true;
                
                if (_dgv14.Rows[greenindex].Cells["cyplanEuro"].Value.ToString() == "")
                    upd = true;
                else if (Convert.ToInt32(_dgv14.Rows[greenindex].Cells["cyplanEuro"].Value) != sum)
                    upd = true;

                if (upd)
                {
                    if (sum != 0)
                        _dgv14.Rows[greenindex].Cells["cyplan"].Value = sum;
                    else if(_dgv14.Rows[greenindex].Cells["cyplan"].Value.ToString() != "")
                        _dgv14.Rows[greenindex].Cells["cyplan"].Value = "";
                    
                    if (sumEuro != 0)
                        _dgv14.Rows[greenindex].Cells["cyplanEuro"].Value = sumEuro;
                    else if (_dgv14.Rows[greenindex].Cells["cyplanEuro"].Value.ToString() != "")
                        _dgv14.Rows[greenindex].Cells["cyplanEuro"].Value = "";

                    if (globalData.Div == "HC")
                    {
                        if (sumRub != 0)
                            _dgv14.Rows[greenindex].Cells["cyplanRub"].Value = sumRub;
                        else if (_dgv14.Rows[greenindex].Cells["cyplanRub"].Value.ToString() != "")
                            _dgv14.Rows[greenindex].Cells["cyplanRub"].Value = "";
                    }
                    _dgv14.Rows[greenindex].Cells["upd"].Value = "1";
                    _dgv14.Rows[RowIndex].Cells["upd"].Value = "1";

                    if (_dgv14.Rows[greenindex].Cells["nom_id"].Value.ToString() == "3")
                    {
                        SubSumAccNYN3(0);
                    }
                }
            }
            catch
            {
            }
        }

        private void SubSumAccNYN3(int RowIndex)
        {
            try
            {
                int sum = 0;
                Double sumEuro = 0;
                Double sumRub = 0;
                int i = 7;
                int greenindex = 0;

                while (_dgv14.Rows[i].Cells["nom_id"].Value.ToString() != "1")
                {
                    if (_dgv14.Rows[i].Cells["cyplan"].Value != null)
                    {
                        if ((_dgv14.Rows[i].Cells["cyplan"].Value.ToString() != "") && (Convert.ToInt32(_dgv14.Rows[i].Cells["nom_id"].Value) < 100))
                            sum += Convert.ToInt32(_dgv14.Rows[i].Cells["cyplan"].Value.ToString());
                    }
                    if (_dgv14.Rows[i].Cells["cyplanEuro"].Value != null)
                    {
                        if ((_dgv14.Rows[i].Cells["cyplanEuro"].Value.ToString() != "") && (Convert.ToInt32(_dgv14.Rows[i].Cells["nom_id"].Value) < 100))
                            sumEuro += Convert.ToDouble(_dgv14.Rows[i].Cells["cyplanEuro"].Value.ToString());
                    }
                    if (_dgv14.Rows[i].Cells["cyplanRub"].Value != null)
                    {
                        if ((_dgv14.Rows[i].Cells["cyplanRub"].Value.ToString() != "") && (Convert.ToInt32(_dgv14.Rows[i].Cells["nom_id"].Value) < 100))
                            sumEuro += Convert.ToDouble(_dgv14.Rows[i].Cells["cyplanRub"].Value.ToString());
                    }
                    i--;
                }

                Boolean upd = false;

                if (_dgv14.Rows[greenindex].Cells["cyplan"].Value.ToString() == "")
                    upd = true;
                else if (Convert.ToInt32(_dgv14.Rows[greenindex].Cells["cyplan"].Value) != sum)
                    upd = true;

                if (_dgv14.Rows[greenindex].Cells["cyplanEuro"].Value.ToString() == "")
                    upd = true;
                else if (Convert.ToInt32(_dgv14.Rows[greenindex].Cells["cyplanEuro"].Value) != sum)
                    upd = true;

                if (upd)
                {
                    if (sum != 0)
                        _dgv14.Rows[greenindex].Cells["cyplan"].Value = sum;
                    else if (_dgv14.Rows[greenindex].Cells["cyplan"].Value.ToString() != "")
                        _dgv14.Rows[greenindex].Cells["cyplan"].Value = "";
                    
                    if (sumEuro != 0)
                        _dgv14.Rows[greenindex].Cells["cyplanEuro"].Value = sumEuro;
                    else if (_dgv14.Rows[greenindex].Cells["cyplanEuro"].Value.ToString() != "")
                        _dgv14.Rows[greenindex].Cells["cyplanEuro"].Value = "";

                    if (globalData.Div != "AE")
                    {
                        if (sumRub != 0)
                            _dgv14.Rows[greenindex].Cells["cyplanRub"].Value = sumRub;
                        else if (_dgv14.Rows[greenindex].Cells["cyplanRub"].Value.ToString() != "")
                            _dgv14.Rows[greenindex].Cells["cyplanRub"].Value = "";
                    }
                    _dgv14.Rows[greenindex].Cells["upd"].Value = "1";
                    _dgv14.Rows[RowIndex].Cells["upd"].Value = "1";
                }
            }
            catch
            {
            }
        }

        private void btnSaveAccNY_Click(object sender, EventArgs e)
        {
            try
            {
                Sql sql1 = new Sql();

                load = false;

                if (_dgv14.Rows[0].Cells["acc_id"].Value.ToString() != "0")
                {
                    foreach (DataGridViewRow row in _dgv14.Rows)
                    {
                        if (row.Cells["upd"].Value != null)
                        {
                            if (row.Cells["cyplan"].Value == null)
                                row.Cells["cyplan"].Value = 0;
                            else if (row.Cells["cyplan"].Value.ToString() == "")
                                row.Cells["cyplan"].Value = 0;

                            if (row.Cells["cyplanEuro"].Value == null)
                                row.Cells["cyplanEuro"].Value = 0;
                            else if (row.Cells["cyplanEuro"].Value.ToString() == "")
                                row.Cells["cyplanEuro"].Value = 0;

                            load = true;
                            /* Заполнение происходит на UserLPU 
                             * fillAccPlanNY  - сохраняет сразу на ЛПУ
                             * fillAccPlanNYold - сохраняет без рублей
                             * 
                             */
                            int Rub = 0;
                            if (globalData.Div == "HC")
                            {
                                if (row.Cells["nom_type"].Value.ToString() == "euro")
                                    Rub = 75 * Convert.ToInt32(row.Cells["cyplanEuro"].Value);
                                else if (row.Cells["cyplan"].Value.ToString() != "0")
                                    Rub = Convert.ToInt32(row.Cells["cyplanRub"].Value);
                            }
                            sql1.GetRecords("exec fillAccPlanNYoldWithRub @p1, @p2, @p3, @p4, @p5", row.Cells["acc_id"].Value, row.Cells["cyplan"].Value, row.Cells["cyplanEuro"].Value, cbUsersAccNY.SelectedValue, Rub);
                        }
                    }
                }
                MessageBox.Show("Информация по планам сохранена.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (load)
                    loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);

                load = true;
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить информацию по планам, системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                load = true;
            }
        }

        private void _dgv14_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 12)
                    begval2 = Convert.ToDouble(_dgv14.CurrentRow.Cells["cyDilCost"].Value);
            }
            catch
            {
            }
        }

        private void btnSaveAccNYDilCost_Click(object sender, EventArgs e)
        {
            try
            {
                Sql sql1 = new Sql();

                load = false;

                if (_dgv14.Rows[0].Cells["nom_id"].Value.ToString() != "0")
                {
                    foreach (DataGridViewRow row in _dgv14.Rows)
                    {
                        if (row.Cells["upd"].Value != null)
                        {
                            if (row.Cells["cyDilCost"].Value.ToString() == "")
                                continue;
                            sql1.GetRecords("exec fillAccPlanDilCost @p1, @p2, @p3", row.Cells["nom_id"].Value, row.Cells["cyDilCost"].Value, globalData.UserID);
                            load = true;
                        }
                    }
                }
                MessageBox.Show("Информация по дилерским ценам сохранена.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (load)
                    loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
                
                load = true;
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить информацию по дилерским ценам, системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*
        private void SelAllAccNY()
        {
            clsSql sql1 = new clsSql();
            DataSet ds1 = new DataSet();

            tabControl1.SelectedIndex = 16;
            tabControl1.Visible = true;

            load = false;

            
            ds1 = sql1.GetRecordsDS("exec selLPU @p1, '', @p2, @p3, @p4", cbUsersAllAccNY.SelectedValue, clsData.Div, clsData.CurDate.Year, cbRegAllAccNY.SelectedValue);

            if (ds1 == null)
            {
                MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                load = true;
                return;
            }

            cbLPUAllAccNY.DataSource = ds1.Tables[0].DefaultView;
            cbLPUAllAccNY.DisplayMember = "lpu_sname";
            cbLPUAllAccNY.ValueMember = "ulpu_id";
            

            load = true;

            if (cbLPUAllAccNY.Items.Count > 0)
                cbLPUAllAccNY.Enabled = true;
            else
                cbLPUAllAccNY.Enabled = false;

            
            btnHideLPUAllAccNY.Text = "Скрыть ЛПУ";
            btnHideUsersAllAccNY.Text = "Скрыть пользователей";
            cbLPUAllAccNY.Visible = true;
            cbUsersAllAccNY.Visible = true;
            btnHideLPUAllAccNY.Visible = true;
            
            loadAccPlanByLPUNY(_dgv16, cbLPUAllAccNY);
        }
        */

        private void cbLPUAllAccNY_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (load)
                loadAccPlanByLPUNY(_dgv16, cbLPUAllAccNY);
            */
        }

        private void btnHideLPUAllAccNY_Click(object sender, EventArgs e)
        {
            if (btnHideLPUAllAccNY.Text == "Скрыть ЛПУ")
            {
                btnHideLPUAllAccNY.Text = "Показать ЛПУ";
                cbLPUAllAccNY.Visible = false;
            }
            else
            {
                btnHideLPUAllAccNY.Text = "Скрыть ЛПУ";
                cbLPUAllAccNY.Visible = true;
            }
            /*
            if (btnHideLPUAllAccNY.Text == "Скрыть ЛПУ")
            {
                btnHideLPUAllAccNY.Text = "Показать ЛПУ";
                cbLPUAllAccNY.Visible = false;
                clsSql sql1 = new clsSql();
                DataTable dt1 = new DataTable();

                if(clsData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccPlanNYAE 0, @p1, '', @p2", cbUsersAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue);
                else
                    dt1 = sql1.GetRecords("exec SelAccPlanNY @p1, 0, @p2, '', @p3", clsData.Div, cbUsersAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAccNY(dt1, _dgv16);
            }
            else
            {
                btnHideLPUAllAccNY.Text = "Скрыть ЛПУ";
                cbLPUAllAccNY.Visible = true;
                loadAccPlanByLPUNY(_dgv16, cbLPUAllAccNY);
            }
            */
        }

        private void cbUsersAllAccNY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                string reg = "0", user = "0";

                if (cbRegAllAccNY.Visible)
                    reg = cbRegAllAccNY.SelectedValue.ToString();

                user = cbUsersAllAccNY.SelectedValue.ToString();
                
                fillLPU(cbLPUAllAccNY, user, reg, globalData.year);
            }
        }

        private void btnHideUsersAllAccNY_Click(object sender, EventArgs e)
        {   
            if (btnHideUsersAllAccNY.Text == "Скрыть РП")
            {
                btnHideUsersAllAccNY.Text = "Показать РП";
                cbUsersAllAccNY.Visible = false;            
                fillLPU(cbLPUAllAccNY, "0", cbRegAllAccNY.SelectedValue.ToString(), globalData.year);
                
                if (cbRegAllAccNY.Visible == false)
                    cbLPUAllAccNY.Enabled = false;
              
            }
            else
            {
                btnHideUsersAllAccNY.Text = "Скрыть РП";
                cbUsersAllAccNY.Visible = true;
                fillLPU(cbLPUAllAccNY, cbUsersAllAccNY.SelectedValue.ToString(), cbRegAllAccNY.SelectedValue.ToString(), globalData.year);
            }
            /*
                if (btnHideLPUAllAccNY.Text == "Скрыть ЛПУ")
                {
                    cbLPUAllAccNY.Visible = true;
                    //loadAccPlanByLPUNY(_dgv16, cbLPUAllAccNY);
                }
                else
                {
                    btnHideLPUAllAccNY.Text = "Скрыть ЛПУ";
                    //btnHideLPUAllAccNY_Click(null, null);
                }
                btnHideLPUAllAccNY.Visible = true;
                btnHideUsersAllAccNY.Text = "Скрыть РП";
                cbUsersAllAccNY.Visible = true;
            }
            
            if (btnHideUsersAllAccNY.Text == "Скрыть пользователей")
            {
                btnHideUsersAllAccNY.Text = "Показать пользователей";
                cbLPUAllAccNY.Visible = false;
                cbUsersAllAccNY.Visible = false;
                btnHideLPUAllAccNY.Visible = false;
                clsSql sql1 = new clsSql();
                DataTable dt1 = new DataTable();

                if(clsData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccPlanNYAE 0, 0, '', @p1, @p2", cbRegAllAccNY.SelectedValue, clsData.RD);
                else
                    dt1 = sql1.GetRecords("exec SelAccPlanNY @p1, 0, 0, '', @p2, @p3", clsData.Div, cbRegAllAccNY.SelectedValue, clsData.RD);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAccNY(dt1, _dgv16);
            }
            else
            {
                if (btnHideLPUAllAccNY.Text == "Скрыть ЛПУ")
                {
                    cbLPUAllAccNY.Visible = true;
                    loadAccPlanByLPUNY(_dgv16, cbLPUAllAccNY);
                }
                else
                {
                    btnHideLPUAllAccNY.Text = "Скрыть ЛПУ";
                    btnHideLPUAllAccNY_Click(null, null);
                }
                btnHideLPUAllAccNY.Visible = true;
                btnHideUsersAllAccNY.Text = "Скрыть пользователей";
                cbUsersAllAccNY.Visible = true;
            }
            */
        }

        private void cbRegAllAccNY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                string reg = "0", user = "0";
                reg = cbRegAllAccNY.SelectedValue.ToString();

                fillUsersAcc(cbUsersAllAccNY, reg, globalData.CurDate.Year.ToString());

                if (cbUsersAllAccNY.Visible)
                    user = cbUsersAllAccNY.SelectedValue.ToString();

                fillLPU(cbLPUAllAccNY, user, reg, globalData.CurDate.Year.ToString());
            }
        }

        private void btnHideRegionAllAccNY_Click(object sender, EventArgs e)
        {
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;
            if (tn1.Parent == null)
            {
                return;
            }
            Sql sql1 = new Sql();
            DataTable dtNY = new DataTable();
            DataTable dt1 = new DataTable();
            dtNY = sql1.GetRecords("exec GetSettings");

            if (btnHideRegionAllAccNY.Text == "Скрыть регион")
            {
                btnHideRegionAllAccNY.Text = "Показать регион";
                //cbLPUAllAccNY.Visible = false;
                //cbUsersAllAccNY.Visible = false;
                cbRegAllAccNY.Visible = false;
                //btnHideLPUAllAccNY.Visible = false;
                                
                string RD = "";
                            

                if (btnHideUsersAllAccNY.Visible == false)
                    cbLPUAllAccNY.Enabled = false;
                else
                {
                    cbLPUAllAccNY.Enabled = true;
                    fillUsersAcc(cbUsersAllAccNY, "0", dtNY.Rows[0].ItemArray[3].ToString());
                    fillLPU(cbLPUAllAccNY, cbUsersAllAccNY.SelectedValue.ToString(), "0", dtNY.Rows[0].ItemArray[3].ToString());
                }

                if (tn1.Parent.Text == "Общий ассортиментный план " + dtNY.Rows[0].ItemArray[3].ToString())
                    RD = tn1.Parent.Parent.Text;

                if (RD != "")
                {
                    dt1 = sql1.GetRecords("exec SelUsersByRD2 @p1, @p2, @p3", RD, dtNY.Rows[0].ItemArray[3].ToString(), tn1.Text);

                    if (dt1.Rows.Count > 0)
                    {
                        cbUsersAllAccNY.DataSource = dt1;
                        cbUsersAllAccNY.DisplayMember = "user_name";
                        cbUsersAllAccNY.ValueMember = "user_id";
                        cbUsersAllAccNY.Enabled = true;
                    }
                }
            }
            else
            {
                btnHideRegionAllAccNY.Text = "Скрыть регион";
                cbRegAllAccNY.Visible = true;
                btnHideUsersAllAccNY.Visible = true;
                btnHideLPUAllAccNY.Visible = true;
                cbLPUAllAccNY.Enabled = true;

                if (btnHideUsersAllAccNY.Text == "Скрыть РП")
                    cbUsersAllAccNY.Visible = true;
                if (btnHideLPUAllAccNY.Text == "Скрыть ЛПУ")
                    cbLPUAllAccNY.Visible = true;

                string reg = "0", user = "0";
                reg = cbRegAllAccNY.SelectedValue.ToString();

                fillUsersAcc(cbUsersAllAccNY, reg, globalData.CurDate.Year.ToString());

                if (cbUsersAllAccNY.Visible)
                    user = cbUsersAllAccNY.SelectedValue.ToString();

                fillLPU(cbLPUAllAccNY, user, reg, globalData.CurDate.Year.ToString());

            }
            /*
            if (btnHideRegionAllAccNY.Text == "Скрыть регион")
            {
                btnHideRegionAllAccNY.Text = "Показать регион";
                cbLPUAllAccNY.Visible = false;
                cbUsersAllAccNY.Visible = false;
                cbRegAllAccNY.Visible = false;
                btnHideLPUAllAccNY.Visible = false;
                btnHideUsersAllAccNY.Visible = false;

                string RD = "";

                clsSql sql1 = new clsSql();
                DataTable dtNY = new DataTable();
                dtNY = sql1.GetRecords("exec GetSettings");

                if (tn1.Parent.Text == "Общий ассортиментный план " + dtNY.Rows[0].ItemArray[3].ToString())
                    RD = tn1.Parent.Parent.Text;

                DataTable dt1 = new DataTable();

                if(clsData.Div == "AE")
                    dt1 = sql1.GetRecords("exec SelAccPlanNYAE 0, 0, '', 0, @p1", RD);
                else
                    dt1 = sql1.GetRecords("exec SelAccPlanNY @p1, 0, 0, '', 0, @p2", clsData.Div, RD);

                if (dt1 == null)
                {
                    MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                fillAccNY(dt1, _dgv16);
            }
            else
            {
                btnHideRegionAllAccNY.Text = "Скрыть регион";
                cbLPUAllAccNY.Visible = true;
                cbUsersAllAccNY.Visible = true;
                cbRegAllAccNY.Visible = true;
                btnHideLPUAllAccNY.Visible = true;
                btnHideUsersAllAccNY.Visible = true;
                SelAllAccNY();
            }
            */
        }

        private void btnExportInExcelAccNY_Click(object sender, EventArgs e)
        {
            if(globalData.Div == "AE")
                ExportInExcelAccAE(_dgv14, cbLPUAccNY, cbUsersAccNY);
            else
                ExportInExcelAcc(_dgv14, cbLPUAccNY, cbUsersAccNY);
        }

        private void ExportInExcelAcc(DataGridView dgv, ComboBox cbLPUExcel, ComboBox cbUserExcel)
        {
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);
            
            Sql sql1 = new Sql();

            int curIndex = cbLPUExcel.SelectedIndex;

            ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 46;
            ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 5;
            for (int k = 3; k < 13; k++)
                ((Excel.Range)xlSh.Columns[k]).ColumnWidth = 10;


            if (cbLPUExcel.Visible == false)
            {
                try
                {
                    int i = 1;

                    for (int j = 1; j < dgv.ColumnCount; j++)
                    {
                        if (dgv.Columns[j].Visible)
                        {
                            xlSh.Cells[1, i] = dgv.Columns[j].HeaderText;
                            i++;
                        }
                    }

                    i = 2;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        int k = 1;

                        if (row.DefaultCellStyle.BackColor == bbgreen3)
                        {
                            ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen3.ToArgb();
                            ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                        }
                        if (row.DefaultCellStyle.BackColor == bbgreen1)
                        {
                            ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen1.ToArgb();
                            ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                        }
                        if (row.DefaultCellStyle.BackColor == bbgray5)
                        {
                            ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgray5.ToArgb();
                        }

                        for (int j = 1; j < dgv.ColumnCount; j++)
                        {
                            if (dgv.Columns[j].Visible)
                            {
                                if (row.Cells[j].Value != null)
                                {
                                    xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                                }
                                k++;
                            }
                        }
                        i++;
                    }
                }
                catch (Exception err)
                {
                    try
                    {
                        xlWB.Close(false, misValue, misValue);
                        xlApp.Quit();
                        releaseObject(xlSh);
                        releaseObject(xlWB);
                        releaseObject(xlApp);
                    }
                    catch (Exception err2)
                    {
                        MessageBox.Show("Ошибка при очистки объекта. Системная ошибка: " + err2.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                for (int ilpu = 0; ilpu < cbLPUExcel.Items.Count; ilpu++)
                {
                    cbLPUExcel.SelectedIndex = ilpu;

                    if (dgv.Name == "_dgv16")
                        selAllAccNY();
                
                    if (ilpu < xlWB.Worksheets.Count)
                        xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(ilpu + 1);
                    else
                        xlSh = (Excel.Worksheet)xlWB.Worksheets.Add(misValue, misValue, misValue, misValue);


                    string lpu;

                    if (cbUserExcel.Visible)
                        lpu = sql1.GetRecordsOne("exec GetLPUName @p1", cbLPUExcel.SelectedValue);
                    else
                        lpu = sql1.GetRecordsOne("exec GetLPUName 0, @p1", cbLPUExcel.SelectedValue);

                    if(lpu.Length > 31)
                        lpu = lpu.Substring(0, 31);

                    xlSh.Name = lpu;

                    try
                    {
                        int i = 1;

                        for (int j = 1; j < dgv.ColumnCount; j++)
                        {
                            if (dgv.Columns[j].Visible)
                            {
                                xlSh.Cells[1, i] = dgv.Columns[j].HeaderText;
                                i++;
                            }
                        }

                        i = 2;
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            int k = 1;

                            if (row.DefaultCellStyle.BackColor == bbgreen3)
                            {
                                ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen3.ToArgb();
                                ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                            }
                            if (row.DefaultCellStyle.BackColor == bbgreen1)
                            {
                                ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen1.ToArgb();
                                ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                            }
                            if (row.DefaultCellStyle.BackColor == bbgray5)
                            {
                                ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgray5.ToArgb();
                            }

                            for (int j = 1; j < dgv.ColumnCount; j++)
                            {
                                if (dgv.Columns[j].Visible)
                                {
                                    if (row.Cells[j].Value != null)
                                    {
                                        xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                                    }
                                    k++;
                                }
                            }
                            i++;
                        }
                    }
                    catch (Exception err)
                    {
                        try
                        {
                            xlWB.Close(false, misValue, misValue);
                            xlApp.Quit();
                            releaseObject(xlSh);
                            releaseObject(xlWB);
                            releaseObject(xlApp);
                        }
                        catch (Exception err2)
                        {
                            MessageBox.Show("Ошибка при очистки объекта. Системная ошибка: " + err2.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (globalData.db_id == 1)
            {
                String filename = "";

                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec GetUserByID @p1", globalData.UserID);

                DataTable dtNY = new DataTable();
                dtNY = sql1.GetRecords("exec GetSettings");
                string year = dtNY.Rows[0].ItemArray[3].ToString();

                filename = @"C:\Temp\Ассортиментный план на " + year + " год по региону " + globalData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";

                xlApp.DisplayAlerts = false;
                xlWB.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal);
                xlWB.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlSh);


                SendMail(filename);
                System.IO.File.Delete(filename);
            }
            else
            {
                xlApp.Visible = true;
            }
            cbLPUExcel.SelectedIndex = curIndex;
        }

        private void ExportInExcelAccAE(DataGridView dgv, ComboBox cbLPUExcel, ComboBox cbUserExcel)
        {
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

            Sql sql1 = new Sql();

            //int curIndex = cbLPUAccNY.SelectedIndex;
            
            int ih = 1;

            for (int j = 1; j < dgv.ColumnCount; j++)
            {
                if (dgv.Columns[j].Visible)
                {
                    xlSh.Cells[1, ih] = dgv.Columns[j].HeaderText;
                    ih++;
                }
            }

            ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 46;
            ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 10;


            if (btnHideUsersAccNY.Text == "Скрыть пользователей")
            {
                if (btnHideLPUAccNY.Text == "Скрыть ЛПУ")
                    btnHideLPUAccNY_Click(null, null);
            }                

            int i = 2;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                int k = 1;

                if (row.DefaultCellStyle.BackColor == bbgreen3)
                {
                    ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen3.ToArgb();
                    ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                }
                if (row.DefaultCellStyle.BackColor == bbgreen1)
                {
                    ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgreen1.ToArgb();
                    ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                }
                if (row.DefaultCellStyle.BackColor == bbgray5)
                {
                    ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgray5.ToArgb();
                }

                for (int j = 1; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Columns[j].Visible)
                    {
                        if (row.Cells[j].Value != null)
                        {
                            xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                        }
                        k++;
                    }
                }
                i++;
            }
            if (btnHideUsersAccNY.Text == "Скрыть пользователей")
            {
                if (dgv.Name == "_dgv14")
                    btnHideLPUAccNY_Click(null, null);

                for (int ilpu = 0; ilpu < cbLPUExcel.Items.Count; ilpu++)
                {
                    cbLPUExcel.SelectedIndex = ilpu;

                    if(dgv.Name == "_dgv16")
                        selAllAccNY();

                    string lpu;

                    if (cbUserExcel.Visible)
                        lpu = sql1.GetRecordsOne("exec GetLPUName @p1", cbLPUExcel.SelectedValue);
                    else
                        lpu = sql1.GetRecordsOne("exec GetLPUName 0, @p1", cbLPUExcel.SelectedValue);

                    xlSh.Cells[1, ih] = lpu;
                    ((Excel.Range)xlSh.Columns[ih]).ColumnWidth = 10;

                    try
                    {
                        int k = 2;
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.Cells["cyPlanEuro"].Value != null)
                            {
                                if (row.Cells["cyPlanEuro"].Value.ToString() != "")
                                    xlSh.Cells[k, ih] = row.Cells["cyPlanEuro"].Value.ToString();
                            }
                            k++;
                        }
                    }
                    catch (Exception err)
                    {
                        try
                        {
                            xlWB.Close(false, misValue, misValue);
                            xlApp.Quit();
                            releaseObject(xlSh);
                            releaseObject(xlWB);
                            releaseObject(xlApp);
                        }
                        catch (Exception err2)
                        {
                            MessageBox.Show("Ошибка при очистки объекта. Системная ошибка: " + err2.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    ih++;
                }
            }
            if (globalData.db_id == 1)
            {
                String filename = "";

                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec GetUserByID @p1", globalData.UserID);

                DataTable dtNY = new DataTable();
                dtNY = sql1.GetRecords("exec GetSettings");
                string year = dtNY.Rows[0].ItemArray[3].ToString();

                filename = @"C:\Temp\Ассортиментный план на " + year + " год по региону " + globalData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";

                xlApp.DisplayAlerts = false;
                xlWB.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal);
                xlWB.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlSh);


                SendMail(filename);
                System.IO.File.Delete(filename);
            }
            else
            {
                xlApp.Visible = true;
            }
            //cbLPUAccNY.SelectedIndex = curIndex;
        }

        private void _dgv14_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv14);
        }

        private void _dgv15_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv15);
        }

        private void _dgv16_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgv16);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            SelMA();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            SelRegAcc();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            selAllAcc();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;
                
            if (tn1.Text == "Общая динамика продаж")
            {
                if ((globalData.RD == String.Empty) || (globalData.RD == null))
                    loadDynAll();
                else
                    selDynRD();
            }
            else if (tn1.Parent.Text == "Общая динамика продаж")
            {
                loadDyn(cbRegions.SelectedValue.ToString());
            }
            else
                loadDyn("0");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            selAllPrivSales();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (button28.Text == "Общая")
            {
                button28.Text = "По регионам";
                label21.Visible = false;
                cbRegionAllPrivSales.Visible = false;
            }
            else
            {
                button28.Text = "Общая";
                label21.Visible = true;
                cbRegionAllPrivSales.Visible = true;
            }

            selAllPrivSales();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnHideUsersEvo_Click(object sender, EventArgs e)
        {
            if (btnHideUsersEvo.Text == "Скрыть пользователей")
            {
                cbUsersEvo.Visible = false;
                btnHideUsersEvo.Text = "Отобразить пользователей";
            }
            else
            {
                cbUsersEvo.Visible = true;
                btnHideUsersEvo.Text = "Скрыть пользователей";
            }

            string user = "0";
            if (cbUsersEvo.Visible)
                user = cbUsersEvo.SelectedValue.ToString();
            selEvo(_dgv10, globalData.Div, dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), cbRegEvo.SelectedValue.ToString(), globalData.RD, user);
        }

        private void cbRegEvo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                fillUsersAcc(cbUsersEvo, cbRegEvo.SelectedValue.ToString(), cbYearEvo.SelectedItem.ToString());
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (cbRegEvo.Visible == true)
                ExportInExcelEvo(_dgv10);
            else
                ExportInExcel(_dgv10);
        }

        private void ExportInExcelEvo(DataGridView dgv)
        {
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

            Sql sql1 = new Sql();

            //int curIndexReg = cbRegEvo.SelectedIndex;

            ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 46;
            ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 5;
            ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 10;
            ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 10;

            int j = 0;
            for (; j < dgv.ColumnCount; j++)
            {
                if (dgv.Columns[j].Visible)
                {
                    xlSh.Cells[1, j + 1] = dgv.Columns[j].HeaderText;
                    j++;
                }
            }

            int rowIndex = 2;
            xlSh.Cells[rowIndex, j] = "ФИО";
            xlSh.Cells[rowIndex, j + 1] = "Регион";

            
            for (int iReg = 0; iReg < cbRegEvo.Items.Count; iReg++)
            {
                cbRegEvo.SelectedIndex = iReg;

                for (int iUser = 0; iUser < cbUsersEvo.Items.Count; iUser++)
                {
                    cbUsersEvo.SelectedIndex = iUser;                    
                    
                    selEvo(_dgv10, globalData.Div, dateTimePicker5.Value.Month.ToString(), cbYearEvo.SelectedItem.ToString(), cbRegEvo.SelectedValue.ToString(), globalData.RD, cbUsersEvo.SelectedValue.ToString());

                    string regName = sql1.GetRecordsOne("exec SelRegByID @p1", cbRegEvo.SelectedValue);

                    DataTable dt1 = new DataTable();
                    dt1 = sql1.GetRecords("exec SelUserByID @p1", cbUsersEvo.SelectedValue);
                    string userName = dt1.Rows[0].ItemArray[1].ToString();

                    if (rowIndex != 1)
                        rowIndex++;

                    try
                    {
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            int k = 1;
                            for (j = 0; j < dgv.ColumnCount; j++)
                            {
                                if (dgv.Columns[j].Visible)
                                {
                                    if (row.Cells[j].Value != null)
                                    {
                                        xlSh.Cells[rowIndex, k] = row.Cells[j].Value.ToString();
                                    }
                                    k++;
                                }
                            }
                            xlSh.Cells[rowIndex, k] = userName;
                            xlSh.Cells[rowIndex, k + 1] = regName;
                            rowIndex++;
                        }
                    }
                    catch (Exception err)
                    {
                        try
                        {
                            xlWB.Close(false, misValue, misValue);
                            xlApp.Quit();
                            releaseObject(xlSh);
                            releaseObject(xlWB);
                            releaseObject(xlApp);
                        }
                        catch (Exception err2)
                        {
                            MessageBox.Show("Ошибка при очистки объекта. Системная ошибка: " + err2.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (globalData.db_id == 1)
            {
                String filename = "";

                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec GetUserByID @p1", globalData.UserID);

                DataTable dtNY = new DataTable();
                dtNY = sql1.GetRecords("exec GetSettings");
                string year = dtNY.Rows[0].ItemArray[3].ToString();

                filename = @"C:\Temp\Ассортиментный план на " + year + " год по региону " + globalData.Region.ToString() + " (" + dt1.Rows[0].ItemArray[0].ToString() + ").xls";

                xlApp.DisplayAlerts = false;
                xlWB.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal);
                xlWB.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlSh);


                SendMail(filename);
                System.IO.File.Delete(filename);
            }
            else
            {
                xlApp.Visible = true;
            }
        }

        private void listLPUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLpuList formLpuList = new FormLpuList();
            formLpuList.ShowDialog();
        }

        private void nomForAccToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NomForAcc nfa = new NomForAcc();
            nfa.ShowDialog();
        }

        private void btnExportInExcelAcc_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv7);
        }

        private void btnExportInExcelAllAccNY_Click(object sender, EventArgs e)
        {
            if (globalData.Div == "AE")
                ExportInExcelAccAE(_dgv16, cbLPUAllAccNY, cbUsersAllAccNY);
            else
                ExportInExcelAcc(_dgv16, cbLPUAllAccNY, cbUsersAllAccNY);
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (globalData.acc_history != true)
                selAllAccNY();
            else
                selAllAccNY_HistoryLPU();
        }

        private void selAllAccNY()
        {
            Cursor = Cursors.WaitCursor;

            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            string procName = "exec SelAccPlanNY";
            if (globalData.Div == "AE")
                procName += "AE";
            if (globalData.Div == "HC")
                procName += "WithRub";
            


            string lpu = "0", reg = "0", user = "0";

            if (cbLPUAllAccNY.Visible && cbLPUAllAccNY.Enabled)
                lpu = cbLPUAllAccNY.SelectedValue.ToString();

          
            if (cbUsersAllAccNY.Visible)
                user = cbUsersAllAccNY.SelectedValue.ToString();

            if (cbRegAllAccNY.Visible)
                reg = cbRegAllAccNY.SelectedValue.ToString();

            dt1 = sql1.GetRecords(procName + " @p1, @p2, '', @p3, @p4", lpu, user, reg, globalData.RD);

            /*
            if ((cbLPUAllAccNY.Visible) && (cbUsersAllAccNY.Visible))
                dt1 = sql1.GetRecords(procName + " @p1, @p2, '', @p3, @p4", cbLPUAllAccNY.SelectedValue, cbUsersAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue, clsData.RD);
            else if (cbLPUAllAccNY.Visible)
                dt1 = sql1.GetRecords(procName + " @p1, 0, '', @p2, @p3", cbLPUAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue, clsData.RD);
            else if (cbUsersAllAccNY.Visible)
                dt1 = sql1.GetRecords(procName + " 0, @p1, '', @p2, @p3", cbUsersAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue, clsData.RD);
            else if (cbRegAllAccNY.Visible)
                dt1 = sql1.GetRecords(procName + " 0, 0, '', @p1, @p2", cbRegAllAccNY.SelectedValue, clsData.RD);
            else
                dt1 = sql1.GetRecords(procName + " 0, 0, '', 0, @p1", clsData.RD);
            */
            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fillAccNY(dt1, _dgv16);
            Cursor = Cursors.Default;
        }

        private void selAllAccNY_HistoryLPU()
        {
            Cursor = Cursors.WaitCursor;

            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            string procName = "exec AccPlanNY_Select_HistoryLPU_";
            if (globalData.Div == "AE")
                procName += "AE";
            if (globalData.Div == "HC")
                procName += "HC";



            string lpu = "0", reg = "0", user = "0";

            if (cbLPUAllAccNY.Visible && cbLPUAllAccNY.Enabled)
                lpu = cbLPUAllAccNY.SelectedValue.ToString();


            if (cbUsersAllAccNY.Visible)
                user = cbUsersAllAccNY.SelectedValue.ToString();

            if (cbRegAllAccNY.Visible)
                reg = cbRegAllAccNY.SelectedValue.ToString();

            dt1 = sql1.GetRecords(procName + " @p1, @p2, '', @p3, @p4", lpu, user, reg, globalData.RD);

            /*
            if ((cbLPUAllAccNY.Visible) && (cbUsersAllAccNY.Visible))
                dt1 = sql1.GetRecords(procName + " @p1, @p2, '', @p3, @p4", cbLPUAllAccNY.SelectedValue, cbUsersAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue, clsData.RD);
            else if (cbLPUAllAccNY.Visible)
                dt1 = sql1.GetRecords(procName + " @p1, 0, '', @p2, @p3", cbLPUAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue, clsData.RD);
            else if (cbUsersAllAccNY.Visible)
                dt1 = sql1.GetRecords(procName + " 0, @p1, '', @p2, @p3", cbUsersAllAccNY.SelectedValue, cbRegAllAccNY.SelectedValue, clsData.RD);
            else if (cbRegAllAccNY.Visible)
                dt1 = sql1.GetRecords(procName + " 0, 0, '', @p1, @p2", cbRegAllAccNY.SelectedValue, clsData.RD);
            else
                dt1 = sql1.GetRecords(procName + " 0, 0, '', 0, @p1", clsData.RD);
            */
            if (dt1 == null)
            {
                MessageBox.Show("Не удалось получить данные для построения ассортиментного плана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            fillAccNY(dt1, _dgv16);
            Cursor = Cursors.Default;
        }

        private void listMatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Materials mat = new Materials();
            mat.ShowDialog();
        }

        private void checkNewMessage()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelNewMessage @p1", globalData.UserID);
            if (dt1 != null)
            {
                foreach (DataRow row in dt1.Rows)
                {
                    UserMessage umes = new UserMessage(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString(), row.ItemArray[3].ToString());
                    umes.ShowDialog();
                }
            }
            timer1.Enabled = true;
        }

        private void checkNewFirstMessage()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelNewFirstMessage @p1", globalData.UserID);

            foreach (DataRow row in dt1.Rows)
            {
                UserMessage umes = new UserMessage(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString(), row.ItemArray[3].ToString());
                umes.ShowDialog();
            }
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Interval == 1000)
            {
                timer1.Interval = 300000;
                timer1.Enabled = false;
                checkNewFirstMessage();
            }
            else
            {
                timer1.Enabled = false;
                checkNewMessage();                
            }
        }

        private void allUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users us = new Users();
            us.ShowDialog();
        }

        private void sendMailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateUserMessage cumes = new CreateUserMessage();
            cumes.ShowDialog();
        }

        private void btnHideUsersKosRep_Click(object sender, EventArgs e)
        {
            if (btnHideUsersKosRep.Text == "Скрыть")
            {
                cbUsers3.Visible = false;
                btnHideUsersKosRep.Text = "Отобразить";
            }
            else
            {
                cbUsers3.Visible = true;
                btnHideUsersKosRep.Text = "Скрыть";
            }

            loadData();
        }

        private void загрузитьИерархиюПродукцииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                if (globalData.input == "0")
                {
                    MessageBox.Show("ЛПУ не были загружены.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    xlApp.Quit();
                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                    return;
                }

                int i = 2;

                try
                {
                    Sql sql1 = new Sql();
                    DataTable dt1 = new DataTable();

                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                    {
                        String s1 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//sdiv
                        String s2 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//pdiv
                        String s3 = xlSh.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString();//sba
                        String s4 = xlSh.get_Range("G" + i.ToString(), "G" + i.ToString()).Value2.ToString();//mmg
                        String s5 = xlSh.get_Range("I" + i.ToString(), "I" + i.ToString()).Value2.ToString();//msg

                        dt1 = sql1.GetRecords("exec InsProdHier @p1, @p2, @p3, @p4, @p5", s1, s2, s3, s4, s5);
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message);
                    err++;
                }
                finally
                {
                    xlWorkBook.Close(true, misValue, misValue);
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Количество добавленных записей - " + (i - Convert.ToInt32(globalData.input)).ToString());
                        xlApp.Quit();
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.");
                        xlApp.Quit();
                    }

                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Logistics\SCM\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Загрузка прайс листа";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;
                int i;
                tbResult.Clear();

                try
                {
                    for (int list = 1; list < 13; list++) //13
                    {
                        xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(list);
                        i = 3;
                        Sql sql1 = new Sql();
                        while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                        {
                            if (xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2 != null)
                            {
                                String s1 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Trim();//mat_code
                                double s2 = 0;

                                if (digit(xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString()))
                                    s2 = Convert.ToDouble(xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString().Trim());//price
                                else
                                    tbResult.Text += "Ошибка в цене. Лист " + list.ToString() + ", строка: " + i.ToString();
                                DataTable dt1 = sql1.GetRecords("exec SCM_loadPriceEuro @p1, @p2", s1, s2);
                                if (dt1 == null)
                                {
                                    tbResult.Text += "Ошибка. Лист " + list.ToString() + ", строка: " + i.ToString();
                                    err++;
                                    break;
                                }
                            }
                            i++;
                        }
                        releaseObject(xlSh);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Загрузка выполнена с ошибками. Проверьте наличие цен.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"H:\Documents\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Сравнение цен";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;
                tbResult.Clear();

                String curh = "";
                int i = 2;

                try
                {
                    Sql sql1 = new Sql();
                    while (xlSh.get_Range("M" + i.ToString(), "M" + i.ToString()).Value2 != null)
                    {
                        if (xlSh.get_Range("R" + i.ToString(), "R" + i.ToString()).Value2.ToString() != "0")
                        {
                            curh = "M";
                            String s1 = xlSh.get_Range("M" + i.ToString(), "M" + i.ToString()).Value2.ToString();//mat_code
                            curh = "AT";
                            double s2 = Convert.ToDouble(xlSh.get_Range("AT" + i.ToString(), "AT" + i.ToString()).Value2);//price


                            String res = sql1.GetRecordsOne("exec SCM_CompareEuro @p1, @p2", s1, s2);
                            if ((res == null) || (res == String.Empty))
                            {
                                tbResult.Text += "Строка " + i.ToString() + ": товар с артикулом " + s1 + " не найден в справочнике.\r\n";
                            }
                            else if ((Convert.ToDouble(res) < s2) && (res != "-1"))
                            {
                                tbResult.Text += "Строка " + i.ToString() + ": товар с артикулом " + s1 + " цена " + s2 + " больше цены в прайсе " + res + ". Внимание!!!!!!!!!!\r\n";
                            }
                            else if ((Convert.ToDouble(res) > s2) && (res != "-1"))
                            {
                                tbResult.Text += "Строка " + i.ToString() + ": товар с артикулом " + s1 + " цена " + s2 + " меньше цены в прайсе " + res + "\r\n";
                            }
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка в ячейке " + curh + i.ToString() + ". Системная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                        MessageBox.Show("Сравнение завершено. Ошибок не найдено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Ошибка при сравнение.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
            }
        }

        private void btnApplyCheck_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelCheck @p1, @p2, @p3", cbRegCheck.SelectedValue, globalData.Div, cbYearCheck.SelectedItem);

            if (dt1 != null)
            {
                addColumnsCheck(_dgv17);

                if (_dgv17.Rows.Count > 0)
                    _dgv17.Rows.Clear();

                foreach (DataRow row in dt1.Rows)
                {
                    _dgv17.Rows.Add(row.ItemArray);
                }                
                formatCheck(_dgv17);
            }
            else
            {
                MessageBox.Show("Не удалось получить информацию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addColumnsCheck(DataGridView dgv)
        {
            int year = Convert.ToInt32(cbYearCheck.SelectedItem);

            if (dgv.Columns.Count == 0)
            {
                dgv.Columns.Add("nom_id", "nom_id");
                dgv.Columns.Add("nom_seq", "nom_seq");
                dgv.Columns.Add("nom_group", "nom_group");
                dgv.Columns.Add("nom_name", "Номенклатура");
                dgv.Columns.Add("nom_type", "шт/\nевро");                

                dgv.Columns.Add("rep_salesQuanBumPY", "");
                dgv.Columns.Add("rep_salesRevPY", "");

                dgv.Columns.Add("acc_factPY", "");
                dgv.Columns.Add("acc_factEuroPY", "");

                dgv.Columns.Add("deltaPY", "");
                dgv.Columns.Add("deltaEuroPY", "");

                dgv.Columns.Add("rep_salesQuanBum", "");
                dgv.Columns.Add("rep_salesRev", "");

                dgv.Columns.Add("acc_fact", "");
                dgv.Columns.Add("acc_factEuro", "");

                dgv.Columns.Add("delta", "");
                dgv.Columns.Add("deltaEuro", "");

                dgv.Columns["nom_id"].Visible = false;
                dgv.Columns["nom_seq"].Visible = false;
                dgv.Columns["nom_group"].Visible = false;

                dgv.Columns["rep_salesQuanBumPY"].DefaultCellStyle.Format = "N0";
                dgv.Columns["rep_salesRevPY"].DefaultCellStyle.Format = "N2";
                dgv.Columns["acc_factPY"].DefaultCellStyle.Format = "N0";
                dgv.Columns["acc_factEuroPY"].DefaultCellStyle.Format = "N0";
                dgv.Columns["deltaPY"].DefaultCellStyle.Format = "N0";
                dgv.Columns["deltaEuroPY"].DefaultCellStyle.Format = "N2";

                dgv.Columns["rep_salesQuanBum"].DefaultCellStyle.Format = "N0";
                dgv.Columns["rep_salesRev"].DefaultCellStyle.Format = "N2";
                dgv.Columns["acc_fact"].DefaultCellStyle.Format = "N0";
                dgv.Columns["acc_factEuro"].DefaultCellStyle.Format = "N0";
                dgv.Columns["delta"].DefaultCellStyle.Format = "N0";
                dgv.Columns["deltaEuro"].DefaultCellStyle.Format = "N2";

                dgv.Columns["nom_name"].Width = 300;
                dgv.Columns["nom_type"].Width = 50;

                dgv.Columns["nom_name"].Frozen = true;
                dgv.Columns["nom_type"].Frozen = true;
            }

            dgv.Columns["rep_salesQuanBumPY"].HeaderText = "LISA " + (year - 1).ToString() + ", PC";
            dgv.Columns["rep_salesRevPY"].HeaderText = "LISA " + (year - 1).ToString() + ", EUR";
            dgv.Columns["acc_factPY"].HeaderText = (year - 1).ToString() + " факт, шт";
            dgv.Columns["acc_factEuroPY"].HeaderText = (year - 1).ToString() + " факт, EUR";
            dgv.Columns["deltaPY"].HeaderText = "delta " + (year - 1).ToString() + ", PC";
            dgv.Columns["deltaEuroPY"].HeaderText = "delta " + (year - 1).ToString() + ", EUR";
            dgv.Columns["rep_salesQuanBum"].HeaderText = "LISA " + year.ToString() + ", PC";
            dgv.Columns["rep_salesRev"].HeaderText = "LISA " + year.ToString() + ", EUR";
            dgv.Columns["acc_fact"].HeaderText = year.ToString() + " факт, шт";
            dgv.Columns["acc_factEuro"].HeaderText = year.ToString() + " факт, EUR";
            dgv.Columns["delta"].HeaderText = "delta " + year.ToString() + ", PC";
            dgv.Columns["deltaEuro"].HeaderText = "delta " + year.ToString() + ", EUR";
        }

        private void formatCheck(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == 0)
                {
                    dgv.Rows[row.Index].ReadOnly = true;
                    dgv.Rows[row.Index].DefaultCellStyle.BackColor = bbgreen3;
                    dgv.Rows[row.Index].DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_group"].Value) == -1)
                {
                    dgv.Rows[row.Index].ReadOnly = true;
                    dgv.Rows[row.Index].DefaultCellStyle.BackColor = bbgreen1;
                    dgv.Rows[row.Index].DefaultCellStyle.SelectionBackColor = bbgray4;
                }
                if (Convert.ToInt32(row.Cells["nom_id"].Value) == 3)
                {
                    row.ReadOnly = true;
                    row.DefaultCellStyle.BackColor = bbgreen3;
                }

                if ((Convert.ToInt32(row.Cells["nom_id"].Value) == 11) || (Convert.ToInt32(row.Cells["nom_id"].Value) == 156))
                {
                    dgv.Rows[row.Index].ReadOnly = false;
                }
            }
        }

        private void custListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customers cust = new Customers();
            cust.ShowDialog();
        }

        private void _dgv1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 29)
                _dgv1.EndEdit();
        }

        private void btnPSAccUpd_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();

                        
            TreeNode tn1 = new TreeNode();
            tn1 = treeView1.SelectedNode;

            globalData.Div = tn1.Text;            
                      

            globalData.CurDate = dateTimePicker6.Value;

            
            dt1 = sql1.GetRecords("exec SelAllLPUforUpdate @p1, @p2", globalData.Div, globalData.CurDate);

            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("Ассортиментный план не нужно обновлять", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            prBar1.Visible = true;

            bgw = new BackgroundWorker[dt1.Rows.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                bgw[i] = new BackgroundWorker();
                bgw[i].DoWork += backgroundWorker_DoWork;
                bgw[i].RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                bgw[i].WorkerSupportsCancellation = true;
            }

            int j = 0;

            prBar1.Value = 0;
            prBar1.Maximum = dt1.Rows.Count;

            if (globalData.UserID != 1)
            {
                prBar1.Visible = false;
                Cursor = Cursors.WaitCursor;

                foreach (DataRow row in dt1.Rows)
                {
                    sql1.GetRecords("exec fillFactAcc @p1, @p2, @p3", row[0].ToString(), globalData.Div, globalData.CurDate);
                }

                loadData();
                Cursor = Cursors.Default;
                MessageBox.Show("Ассортиментный план обновлён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                foreach (DataRow row in dt1.Rows)
                {
                    bgw[j].RunWorkerAsync(row[0].ToString());
                    j++;
                }
            }
        }

        private void cbYearEvo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((load) && (globalData.Div == "HC") && treeView1.SelectedNode.Text != "Выполнение плана по России")
            {
                fillRegions(globalData.RD, cbRegEvo);
                fillUsersAcc(cbUsersEvo, cbRegEvo.SelectedValue.ToString(), cbYearEvo.SelectedItem.ToString());
            }
        }

        private void списокЛПУРегпредставителейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ULPUforUsers ul = new ULPUforUsers();
            ul.ShowDialog();
        }

        private void ежедневныйОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DailyAccess();
            /*
            //if (clsData.UserAccess == )
            if (!load || clsData.UserID != clsData.UserID2)
            {
                LoadStatus(cbStatus, 1);
                LoadRegion(cbRegDaily);
                LoadStatus(cbStatusd, 0);
                LoadLPU(cbLPUd, 0, clsData.UserID);
                LoadUser(cbUserDaily);
                LoadLPU(cbLPUdaily, 1, cbUserDaily.SelectedValue);
                //clsData.UserID2 = clsData.UserID;
            }*/
        }

        private void label36_Click(object sender, EventArgs e)
        {
        }

        private void LoadRegion(ComboBox cb)
        {
            DataTable dt = new DataTable();

            Sql sql1 = new Sql();
            /* Рег.пред. и рег. менеджер */
            if ((globalData.UserAccess == 5) || (globalData.UserAccess == 6))
            {
                dt = sql1.GetRecords("exec SelRegionByUserID @p1", globalData.UserID);
                load = false;
                if (dt != null)
                {
                    cb.DataSource = dt;
                    cb.DisplayMember = "reg_nameRus";
                    cb.ValueMember = "reg_id";

                    load = true;
                }
            }
            /* Рег.дир */
            if (globalData.UserAccess == 4)
            {
                dt = sql1.GetRecords("exec SelRegByRD @p1", globalData.UserID);
                load = false;
                if (dt != null)
                {
                    cb.DataSource = dt;
                    cb.DisplayMember = "reg_nameRus";
                    cb.ValueMember = "reg_id";

                    load = true;
                }
            }
            /*Руководство 1,2, админы*/
            if ((globalData.UserAccess == 1) || (globalData.UserAccess == 2) || (globalData.UserAccess == 3))
            {
                dt = sql1.GetRecords("exec SelRegion2");
                load = false;
                if (dt != null)
                {
                    cb.DataSource = dt;
                    cb.DisplayMember = "reg_nameRus";
                    cb.ValueMember = "reg_id";

                    load = true;
                }
            }
        }
         
        private void LoadLPU(ComboBox cb, int flag, object idUser)
        {
            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelULPUbyUserID @p1, @p2", idUser, flag);
            load = false;
            if (dt != null)
            {
                cb.DataSource = dt;
                cb.DisplayMember = "lpu_sname";
                cb.ValueMember = "lpu_id";

                load = true;
            }            
        }

        private void LoadUser(ComboBox cb, ComboBox cbreg)
        {
            Sql sql1 = new Sql();
            DataTable dt = new DataTable();
            /* Рег.пред. */
            if (globalData.UserAccess == 5)
            {
                dt = sql1.GetRecords("exec SelUserByID @p1", globalData.UserID);
                load = false;

                if (dt != null)
                {
                    cb.DataSource = dt;
                    cb.DisplayMember = "user_name";
                    cb.ValueMember = "user_id";

                    load = true;
                }
            }
            /* Рег.пред. */
            else
            {
                dt = sql1.GetRecords("exec SelUsers3 @p1", cbreg.SelectedValue);
                load = false;

                if (dt != null)
                {
                    cb.DataSource = dt;
                    cb.DisplayMember = "user_name";
                    cb.ValueMember = "user_id";

                    load = true;
                }
            }
        }

        private void LoadStatus(ComboBox cb, int flag)
        {
            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelStatus @p1", flag);

            load = false;
            if (dt != null)
            {
                cb.DataSource = dt;
                cb.DisplayMember = "st_name";
                cb.ValueMember = "st_id";
                load = true;
            }
        }

        private void LoadPres(ComboBox cb, int flag)
        {
            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelPres @p1", flag);

            load = false;
            if (dt != null)
            {
                cb.DataSource = dt;
                cb.DisplayMember = "pres_name";
                cb.ValueMember = "pres_id";
                load = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void заполнитьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadStatus(cbStatusd, 0);
            LoadLPU(cbLPUd, 0, globalData.UserID);

            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();

            if (dgvDaily.Rows != null)
            {
                dgvDaily.Rows.Clear();
            }

            maskedTextBox1.Visible = true;
            maskedTextBox2.Visible = true;

            label31.Visible = true;
            label37.Visible = true;
            label36.Visible = true;
            label34.Visible = true;
            label35.Visible = true;

            label46.Visible = false;
            dateTimePicker16.Visible = false;

            if (dateTimePicker10.MinDate != dateTimePicker10.MaxDate)
            {
                Sql sql1 = new Sql();
                string serverdate = sql1.GetRecordsOne("exec GetDateServer");

                dateTimePicker10.MinDate = Convert.ToDateTime(serverdate);
                dateTimePicker10.MaxDate = Convert.ToDateTime(serverdate);
            }

            label32.Text = "Сумма аукциона";
            button34.Text = "Добавить еще аукцион";
            button35.Text = "Сохранить весь день";

            textBox4.Visible = true;
            textBox5.Visible = true;

            cbStatusd.Visible = true;

            button37.Visible = true;
            button38.Visible = true;
            
            
            maskedTextBox1.ValidatingType = typeof(DateTime);
            maskedTextBox2.ValidatingType = typeof(DateTime);
            maskedTextBox1.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox1_TypeValidationCompleted);
            maskedTextBox2.TypeValidationCompleted += new TypeValidationEventHandler(maskedTextBox2_TypeValidationCompleted);
            maskedTextBox1.KeyDown += new KeyEventHandler(maskedTextBox1_KeyDown);
            maskedTextBox2.KeyDown += new KeyEventHandler(maskedTextBox2_KeyDown);
            
            toolTip1.IsBalloon = true;

            tabControl1.SelectedIndex = 18;
            tabControl1.Visible = true;  

            if (splitContainer1.Panel1Collapsed == false)
                splitContainer1.Panel1Collapsed = true;
            LoadProducts();
            //LoadLPU(cbLPUd, 0, clsData.UserID);
            //LoadStatus(cbStatus);
            dgvDaily.Columns[4].Visible = true;
            flagdate = 0;
        }

        private void просмотретьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cbStatus.Visible = true;
            label33.Visible =  true;

            tabControl1.SelectedIndex = 18;
            tabControl1.Visible = true;
            
            LoadStatus(cbStatus, 1);
            LoadRegion(cbRegDaily);
            LoadUser(cbUserDaily, cbRegDaily);
            LoadLPU(cbLPUdaily, 1, cbUserDaily.SelectedValue);
            LoadPres(cbPres, 1);

            if (splitContainer1.Panel2Collapsed == false)
                splitContainer1.Panel2Collapsed = true;
       }

        private void cbRegDaily_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUser(cbUserDaily, cbRegDaily);
        }

        private void cbUserDaily_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLPU(cbLPUdaily, 1, cbUserDaily.SelectedValue);
        }


        private void LoadProducts()
        {
            Sql sql1 = new Sql();            
            DataTable dt2 = sql1.GetRecords("exec SelProd @p1", 3);

            if (dt2 != null)
            {
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_nom"]).DataSource = dt2;
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_nom"]).DisplayMember = "nom_name";
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_nom"]).ValueMember = "nom_id";
                //((DataGridViewComboBoxColumn)dgvDaily.Columns["col_nom"]).CellType.Equals()
            }

            DataTable dt1 = sql1.GetRecords("exec SelProd @p1",1);
           
            if (dt1 != null)
            {
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_btn"]).DataSource = dt1;
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_btn"]).DisplayMember = "nom_name";
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_btn"]).ValueMember = "nom_id";
            }
            
            DataTable dt3 = new DataTable();

            dt3 = sql1.GetRecords("exec SelProd @p1", 4);

            if (dt3 != null)
            {
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_ICU"]).DataSource = dt3;
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_ICU"]).DisplayMember = "nom_name";
                ((DataGridViewComboBoxColumn)dgvDaily.Columns["col_ICU"]).ValueMember = "nom_id";
            }

            DataTable dt4 = new DataTable();

            if (заполнитьОтчетToolStripMenuItem.Pressed == true)
            {
                dt4 = sql1.GetRecords("exec SelPres @p1", 0);

                if (dt4 != null)
                {
                    ((DataGridViewComboBoxColumn)dgvDaily.Columns["stat"]).DataSource = dt4;
                    ((DataGridViewComboBoxColumn)dgvDaily.Columns["stat"]).DisplayMember = "pres_name";
                    ((DataGridViewComboBoxColumn)dgvDaily.Columns["stat"]).ValueMember = "pres_id";
                }
            }
            if (составлениеПланаToolStripMenuItem.Pressed == true)
            {
                dt4 = sql1.GetRecords("exec SelPres @p1", 2);

                if (dt4 != null)
                {
                    ((DataGridViewComboBoxColumn)dgvDaily.Columns["stat"]).DataSource = dt4;
                    ((DataGridViewComboBoxColumn)dgvDaily.Columns["stat"]).DisplayMember = "pres_name";
                    ((DataGridViewComboBoxColumn)dgvDaily.Columns["stat"]).ValueMember = "pres_id";
                }
            }
        }

        private void dgvDaily_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void FillDaily()
        {
            if (dataGridView2.Rows != null)
            {
                dataGridView2.Columns.Clear();
                dataGridView2.Rows.Clear();
            }

            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7", cbRegDaily.SelectedValue, cbUserDaily.SelectedValue, cbLPUdaily.SelectedValue, cbStatus.SelectedValue, dateTimePicker13.Value, dateTimePicker14.Value, cbPres.SelectedValue);

            if (dt == null)
                MessageBox.Show("Ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);

            dataGridView2.Columns.Add("reg_name", "Регион");                  //0
            dataGridView2.Columns.Add("name", "Рег.пред.");                   //1
            dataGridView2.Columns.Add("date", "Число");                       //2 
            dataGridView2.Columns.Add("lpu", "ЛПУ");                          //3

            dataGridView2.Columns.Add("mat1", "AIS disposables");             //4
            dataGridView2.Columns.Add("mat2", "CVC");                         //5
            dataGridView2.Columns.Add("mat3", "Enteral nutrition");           //6
            dataGridView2.Columns.Add("mat4", "Enteroport");                  //7
            dataGridView2.Columns.Add("mat5", "Gematek");                     //8
            dataGridView2.Columns.Add("mat6", "Generics");                    //9
            dataGridView2.Columns.Add("mat7", "Infusion Stations & components (Euro sales)");   //10
            dataGridView2.Columns.Add("mat8", "Infusion systems");            //11
            dataGridView2.Columns.Add("mat9", "IVC");                         //12
            dataGridView2.Columns.Add("mat10", "L.I.F.E");                    //13
            dataGridView2.Columns.Add("mat11", "Lipoplus");                   //14
            dataGridView2.Columns.Add("mat12", "Other products for enteroport");    //15
            dataGridView2.Columns.Add("mat13", "Parenteral nutrition in vials");    //16
            dataGridView2.Columns.Add("mat14", "PE 2chBgs");                        //17
            dataGridView2.Columns.Add("mat15", "PE 3chBgs");                        //18    
            dataGridView2.Columns.Add("mat16", "Products for infusion");            //19
            dataGridView2.Columns.Add("mat17", "Pumps");                            //20
            dataGridView2.Columns.Add("mat18", "PVR");                              //21
            dataGridView2.Columns.Add("mat19", "Regional anaesthesia");             //22
            dataGridView2.Columns.Add("mat20", "Rest BC");                          //23
            dataGridView2.Columns.Add("mat21", "Small Volume");                     //24
            dataGridView2.Columns.Add("mat22", "Syringes 2-piece (100 pcs)");       //25
            dataGridView2.Columns.Add("mat23", "Syringes 3-piece (100 pcs)");       //26
            dataGridView2.Columns.Add("mat24", "Withdrawal and admixture tools");   //27
            dataGridView2.Columns.Add("mat25", "Электролиты");                      //28

            dataGridView2.Columns.Add("stat", "Статус продаж");                            //29
            dataGridView2.Columns.Add("price", "Цена в торгах");                             //30
            dataGridView2.Columns.Add("distr", "Дистрибьютор");                     //31
            dataGridView2.Columns.Add("dateT", "Планируемая дата торгов");          //32
            dataGridView2.Columns.Add("dateO", "Дата отгрузки");                    //33
            dataGridView2.Columns.Add(new DataGridViewLinkColumn());                //34 
            dataGridView2.Columns[34].HeaderText = "Cсылка на аукцион";
            dataGridView2.Columns.Add("kom", "Комментарий");                        //35
            dataGridView2.Columns.Add("day_id", "Номер");                           //36

            dataGridView2.Columns[36].Visible = false;
            dataGridView2.Columns["date"].DefaultCellStyle.Format = "ddd d MMM";
            dataGridView2.Columns["dateT"].DefaultCellStyle.Format = "dd.MM.yyyy";
            dataGridView2.Columns["dateO"].DefaultCellStyle.Format = "dd.MM.yyyy";

            dataGridView2.Columns[3].Frozen = true;
            dataGridView2.Columns[3].Width = 200;
                        
            string dayIDnew = "", dayIDold = "";

            foreach (DataRow row in dt.Rows)
            {
                if (dayIDnew == "")
                {
                    dataGridView2.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13], row.ItemArray[4]);
                    dayIDold = row.ItemArray[4].ToString();
                }
                if (row.ItemArray[6].ToString() != "")
                {
                    dayIDnew = row.ItemArray[4].ToString();

                    if (dayIDnew != dayIDold)
                    {
                        dataGridView2.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13], row.ItemArray[4]);
                        dayIDold = row.ItemArray[4].ToString();
                        dataGridView2[SearchNom(row.ItemArray[5].ToString()), dataGridView2.Rows.Count - 1].Value = row.ItemArray[6].ToString();
                        if (row.ItemArray[14].ToString() == "1")
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[SearchNom(row.ItemArray[5].ToString())].Style.BackColor = Color.FromArgb(151, 203, 255);
                        if (row.ItemArray[14].ToString() == "2")
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[SearchNom(row.ItemArray[5].ToString())].Style.BackColor = Color.FromArgb(115, 214, 186);
                        if (row.ItemArray[14].ToString() == "3")
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[SearchNom(row.ItemArray[5].ToString())].Style.BackColor = Color.FromArgb(252, 250, 160);
                    }
                    else
                    {
                        dataGridView2[SearchNom(row.ItemArray[5].ToString()), dataGridView2.Rows.Count - 1].Value = row.ItemArray[6].ToString();
                        if (row.ItemArray[14].ToString() == "1")
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[SearchNom(row.ItemArray[5].ToString())].Style.BackColor = Color.FromArgb(151, 203, 255);
                        if (row.ItemArray[14].ToString() == "2")
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[SearchNom(row.ItemArray[5].ToString())].Style.BackColor = Color.FromArgb(115, 214, 186);
                        if (row.ItemArray[14].ToString() == "3")
                            dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[SearchNom(row.ItemArray[5].ToString())].Style.BackColor = Color.FromArgb(252, 250, 160);
                        //dataGridView2[4, 0].Value = row.ItemArray[6];                   
                    }
                }
            }
            if (globalData.UserAccess == 5)
            {
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
            }
            else
            {
                dataGridView2.Columns[0].Visible = true;
                dataGridView2.Columns[1].Visible = true;
            }

            if (cbRegDaily.SelectedIndex != 0)
                dataGridView2.Columns[0].Visible = false;

            if (cbUserDaily.SelectedIndex != 0)
                dataGridView2.Columns[1].Visible = false;

            if (cbLPUdaily.SelectedIndex != 0)
                dataGridView2.Columns[3].Visible = false;
        }
        private void btn_НС_Click(object sender, EventArgs e)
        {
            FillDaily();
        }

        private int SearchNom(string str)
        {
            if (str == "AIS disposables")
                return 4;
            if (str == "CVC")                         
                return 5;
            if (str == "Enteral nutrition")
                return 6;          
            if (str == "Enteroport")
                return 7;                 
            if (str == "Gematek")
                return 8;                    
            if (str == "Generics")
                return 9;                   
            if (str ==  "Infusion Stations & components (Euro sales)") 
                return 10;
            if (str == "Infusion systems")
                return 11;            
            if (str == "IVC")
                return 12;                         
            if (str ==  "L.I.F.E")
                return 13;                    
            if (str ==  "Lipoplus")
                return 14;
            if (str == "Other products for enteroport")
                return 15;
            if (str == "Parenteral nutrition in vials")
                return 16;
            if (str ==  "PE 2chBgs")
                return 17;                  
            if (str ==  "PE 3chBgs")
                return 18;                  
            if (str ==  "Products for infusion")
                return 19;      
            if (str ==  "Pumps")
                return 20;                      
            if (str ==  "PVR")
                return 21;                        
            if (str ==  "Regional anaesthesia")
                return 22;       
            if (str ==  "Rest BC")
                return 23;                    
            if (str ==  "Small Volume")
                return 24;               
            if (str ==  "Syringes 2-piece (100 pcs)")
                return 25; 
            if (str ==  "Syringes 3-piece (100 pcs)")
                return 26;
            if (str == "Withdrawal and admixture tools")
                return 27;
            if (str ==  "Электролиты")                
                return 28;            
            else
                return -1;
        }

        private void btn_BC_Click(object sender, EventArgs e)
        {
            FillDaily();
            for (int i = 4; i < 8; i++)
                dataGridView2.Columns[i].Visible = false;
            for (int i = 9; i < 12; i++)
                dataGridView2.Columns[i].Visible = false;
            for (int i = 14; i < 22; i++)
                dataGridView2.Columns[i].Visible = false;
            dataGridView2.Columns[28].Visible = false;
        }

        private void btn_CC_Click(object sender, EventArgs e)
        {
            FillDaily();
            for (int i = 23; i < 28; i++)
                dataGridView2.Columns[i].Visible = false;
            dataGridView2.Columns[8].Visible = false;
            dataGridView2.Columns[12].Visible = false;
            dataGridView2.Columns[13].Visible = false;
        }

        private void btn_CN_Click(object sender, EventArgs e)
        {
            FillDaily();
            for (int i = 4; i < 6; i++)
                dataGridView2.Columns[i].Visible = false;
            for (int i = 8; i < 14; i++)
                dataGridView2.Columns[i].Visible = false;
            for (int i = 19; i < 29; i++)
                dataGridView2.Columns[i].Visible = false;
        }

        private void btn_ICU_Click(object sender, EventArgs e)
        {
            FillDaily();
            for (int i = 6; i < 9; i++)
                dataGridView2.Columns[i].Visible = false;
            for (int i = 12; i < 19; i++)
                dataGridView2.Columns[i].Visible = false;
            for (int i = 23; i < 28; i++)
                dataGridView2.Columns[i].Visible = false;
        }

        private int SaveDaily()
        {
            DateTime date1 = new DateTime();
            DateTime date2 = new DateTime();
            int flag1 = 0;
            int flag2 = 0;
            bool flag3 = false;
            bool flag4 = false;
            try
            {
                Sql sql1 = new Sql();                
                int count = 0;
                
                //load = false;
                if (dgvDaily.RowCount == 0)
                {
                    MessageBox.Show("Нет товара!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }

                else
                {
                    string str1 = "";
                    int pres = 2;
                    
                    if (maskedTextBox1.Visible == true)
                    {
                        if (maskedTextBox1.Text == "  .  .")
                        {
                            MessageBox.Show("Введите планируемую дату торгов или скройте параметр", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return 0;
                        }
                        else
                        {
                            date1 = Convert.ToDateTime(maskedTextBox1.Text);
                            flag1 = 1;
                        }
                    }
                    if (maskedTextBox2.Visible == true)
                    {
                        if (maskedTextBox2.Text == "  .  .")
                        {
                            MessageBox.Show("Введите дату отгрузки или скройте параметр", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return 0;
                        }
                        else
                        {
                            date2 = Convert.ToDateTime(maskedTextBox2.Text);
                            flag2 = 1;
                        }
                    }

                    foreach (DataGridViewRow row in dgvDaily.Rows)
                    {
                        if (dgvDaily.Rows[row.Index].Cells["col_btn"].Value != null)
                            if (dgvDaily.Rows[row.Index].Cells["col_btn"].Value.ToString() != "0")
                            {
                                str1 = "col_btn";
                                count++;
                            }
                        if (dgvDaily.Rows[row.Index].Cells["col_nom"].Value != null && dgvDaily.Rows[row.Index].Cells["col_nom"].Value.ToString() != "0")
                        {
                            str1 = "col_nom";
                            count++;
                        }
                        if (dgvDaily.Rows[row.Index].Cells["col_ICU"].Value != null && dgvDaily.Rows[row.Index].Cells["col_ICU"].Value.ToString() != "0")
                        {
                            str1 = "col_ICU";
                            count++;
                        }
                        if (dgvDaily.Rows[row.Index].Cells["stat"].Value == null || dgvDaily.Rows[row.Index].Cells["stat"].Value.ToString() == "1")
                        {
                            pres = 1;
                        }
                        else if (dgvDaily.Rows[row.Index].Cells["stat"].Value.ToString() == "2")
                            pres = 2;
                        if (row.Cells["col_count"].Value != null && count == 1)
                        {
                           
                            if (flag1 == 1)
                            {
                                if (flag2 == 1)
                                    sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                        globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                        textBox6.Text, row.Cells[str1].Value, row.Cells["col_count"].Value, date1.Date, date2.Date, pres);
                                else
                                    sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                        globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                        textBox6.Text, row.Cells[str1].Value, row.Cells["col_count"].Value, date1.Date, "", pres);
                                flag3 = true;
                            }
                            else
                            {
                                if (flag2 == 1)
                                    sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                        globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                        textBox6.Text, row.Cells[str1].Value, row.Cells["col_count"].Value, "", date2.Date);
                                else
                                    sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                        globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                        textBox6.Text, row.Cells[str1].Value, row.Cells["col_count"].Value, "", "", pres);
                                flag3 = true;
                            }
                        }
                        if (dgvDaily.RowCount - 1 == 0 && count == 1)
                        {
                            MessageBox.Show("Вы не ввели кол-во товара в" + row.Index.ToString() + " строке. Или не выбрали продукцию. Можно ставить кол-во = 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return 0;
                        }
                        count = 0;
                        flag4 = true;
                    }
                    if (flag4 == false)
                    {
                        if (flag1 == 1)
                        {
                            if (flag2 == 1)
                                sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                    globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                    textBox6.Text, 0, 0, date1.Date, date2.Date, pres);
                            else
                                sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                    globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                    textBox6.Text, 0, 0, date1.Date, "", pres);
                            flag3 = true;
                        }
                        else
                        {
                            if (flag2 == 1)
                                sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                    globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                    textBox6.Text, 0, 0, "", date2.Date);
                            else
                                sql1.GetRecords("exec FillDaily @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13", dateTimePicker10.Value.Date,
                                    globalData.UserID, cbLPUd.SelectedValue, cbStatusd.SelectedValue, textBox2.Text, textBox4.Text, textBox5.Text,
                                    textBox6.Text, 0, 0, "", "", pres);
                            flag3 = true;
                        }
                    }
                }
                if (flag3 == true)
                {
                    MessageBox.Show("Продукция сохранена.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 1;
                }
                else
                {
                    MessageBox.Show("Не сохранено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }
                //LoadProduction();
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить информацию по продукции, системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }


        private void SavePlanWeek()
        {
            try
            {
                Sql sql1 = new Sql();
         
                if (dgvDaily.RowCount == 0)
                {
                    MessageBox.Show("Нет товара!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                else
                {
                    if (textBox6.Text.Trim() == "" || textBox2.Text.Trim() == "")
                    {
                        MessageBox.Show("Не все поля заполнены!", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < dgvDaily.RowCount - 1; i++)
                            foreach (DataGridViewCell cell in dgvDaily.Rows[i].Cells)
                                if (cell.ColumnIndex == 3)
                                    break;
                                else
                                    sql1.GetRecords("exec InsPlanWeek @p1, @p2, @p3, @p4, @p5, @p6, @p7", dateTimePicker10.Value.Date,
                                       cbLPUd.SelectedValue, textBox2.Text, cell.Value, textBox6.Text, globalData.UserID, dgvDaily[3,i].Value);
                        MessageBox.Show("План сохранен.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                 }               
                 
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить план, системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }
        }
        //Добавить аукцион
        private void button34_Click(object sender, EventArgs e)
        {
            if (button34.Text == "Добавить еще аукцион")
            {
                SaveDaily();

                textBox2.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                maskedTextBox1.Clear();
                maskedTextBox2.Clear();

                if (dgvDaily.Rows != null)
                {
                    dgvDaily.Rows.Clear();
                }
            }
            else
            {
                SavePlanWeek();
            }
        }

        //Сохранить
        private void button35_Click(object sender, EventArgs e)
        {
            if (button35.Text == "Сохранить весь день")
            {
                if (SaveDaily() == 1)
                {
                    if (splitContainer1.Panel2Collapsed == false)
                        splitContainer1.Panel2Collapsed = true;
                    FillDaily();
                }
            }
            else
            {
                SavePlanWeek();

                tabControl1.SelectedIndex = 19;
                tabControl1.Visible = true;

                LoadRegion(comboBox3);
                LoadUser(comboBox7, comboBox3);
                LoadLPU(comboBox11, 1, comboBox7.SelectedValue);

                FillPlanWeek();
            }
        }

        // Отменить изменения
        private void button36_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            
            if (dgvDaily.Rows != null)
            {
                dgvDaily.Rows.Clear();
            }
        }

        private void dgvDaily_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            String value = e.Value as string;
            if ((value != null) && value.Equals(e.CellStyle.DataSourceNullValue))
            {
                e.Value = e.CellStyle.NullValue;
                e.FormattingApplied = true;
            }

        }

        private void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTip1.ToolTipTitle = "Неправильный формат даты";
                toolTip1.Show("Дата должна быть в формате день.месяц.год.", maskedTextBox1, 0, -20, 5000);
            }
            else
            {
                //Now that the type has passed basic type validation, enforce more specific type rules.
                DateTime userDate = (DateTime)e.ReturnValue;
                if (userDate < DateTime.Now)
                {
                    toolTip1.ToolTipTitle = "Неправильная дата";
                    toolTip1.Show("Дата в этом поле должна быть больше, чем сегодняшнее число.", maskedTextBox1, 0, -20, 5000);
                    e.Cancel = true;
                }
            }
        }

        private void maskedTextBox2_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
            {
                toolTip1.ToolTipTitle = "Неправильный формат даты";
                toolTip1.Show("Дата должна быть в формате день.месяц.год.", maskedTextBox2, 0, -20, 5000);
            }
            else
            {
                //Now that the type has passed basic type validation, enforce more specific type rules.
                DateTime userDate = (DateTime)e.ReturnValue;
                if (userDate < DateTime.Now)
                {
                    toolTip1.ToolTipTitle = "Неправильная дата";
                    toolTip1.Show("Дата в этом поле должна быть больше, чем сегодняшнее число.", maskedTextBox2, 0, -20, 5000);
                    e.Cancel = true;
                }
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            toolTip1.Hide(maskedTextBox1);
        }

        private void maskedTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            toolTip1.Hide(maskedTextBox2);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Visible == false)
                maskedTextBox1.Visible = true;
            else
                maskedTextBox1.Visible = false;
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (maskedTextBox2.Visible == false)
                maskedTextBox2.Visible = true;
            else             
                maskedTextBox2.Visible = false;
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.ColumnIndex == 35)
            {
                Komment kom = new Komment();

                kom.komment = dataGridView2[35, dataGridView2.CurrentCell.RowIndex].Value.ToString();
                kom.ShowDialog();
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 18)
            {
                if (dataGridView2.Rows.Count != 0)
                {
                    Sql sql1 = new Sql();

                    foreach (DataGridViewCell cell in dataGridView2.SelectedCells)
                    {                       
                        if (cell.RowIndex != -1)
                        {
                            sql1.GetRecords("exec DelString @p1", dataGridView2.Rows[cell.RowIndex].Cells[36].Value);                           
                            dataGridView2.Rows.RemoveAt(cell.RowIndex);
                        }
                        
                    }
                }
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            //FillDaily();
            if (dataGridView2.Columns[5].Visible == true)
            {
                for (int i = 4; i < 29; i++)
                    dataGridView2.Columns[i].Visible = false;
            }
            else
            {
                for (int i = 4; i < 29; i++)
                    dataGridView2.Columns[i].Visible = true;
            }
        }

        private int flagdate = 0;

        private void составлениеПланаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox6.Clear();
            
            if (dgvDaily.Rows != null)
            {
                dgvDaily.Rows.Clear();
            }

            maskedTextBox1.Visible = false;
            maskedTextBox2.Visible = false;

            label31.Visible = false;
            label37.Visible = false;
            label36.Visible = false;
            label34.Visible = false;
            label35.Visible = false;

            label46.Visible = true;

            if (flagdate == 0)
            {
                DateTime date = new DateTime();
                date = dateTimePicker10.Value;

                dateTimePicker16.Visible = true;
                dateTimePicker10.MaxDate = dateTimePicker10.Value.AddMonths(2);

                if (date.DayOfWeek == DayOfWeek.Monday)
                {
                    if (date.Hour >= 12)
                    {
                        dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(7);
                        dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                    }
                    else
                    {
                        dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(0);
                        dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                    }
                }
                if (date.DayOfWeek == DayOfWeek.Tuesday)
                {
                    dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(6);
                    dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                }
                if (date.DayOfWeek == DayOfWeek.Wednesday)
                {
                    dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(5);
                    dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                }
                if (date.DayOfWeek == DayOfWeek.Thursday)
                {
                    dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(4);
                    dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                }
                if (date.DayOfWeek == DayOfWeek.Friday)
                {
                    dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(3);
                    dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                }
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(2);
                    dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                }
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    dateTimePicker10.MinDate = dateTimePicker10.Value.AddDays(1);
                    dateTimePicker16.MinDate = dateTimePicker10.Value.AddDays(7);
                }
                flagdate = 1;
            }
            label32.Text = "Контактное лицо";
            button34.Text = "Добавить еще ЛПУ";
            button35.Text = "Сохранить план";

            textBox4.Visible = false;
            textBox5.Visible = false;

            cbStatusd.Visible = false;

            button37.Visible = false;
            button38.Visible = false;

            tabControl1.SelectedIndex = 18;
            tabControl1.Visible = true;

            if (splitContainer1.Panel1Collapsed == false)
                splitContainer1.Panel1Collapsed = true;
            
            LoadProducts();
            LoadLPU(cbLPUd, 0, globalData.UserID);

            dgvDaily.Columns[4].Visible = false;
        }

        private void просмотретьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 19;
            tabControl1.Visible = true;
        
            LoadRegion(comboBox3);
            LoadUser(comboBox7, comboBox3);
            LoadLPU(comboBox11, 1, comboBox7.SelectedValue);
        }

     
        private bool IsRepeatedCellValue(int rowIndex, int colIndex)
        {
            DataGridViewCell currCell = dataGridView2.Rows[rowIndex].Cells[0];
            DataGridViewCell prevCell = dataGridView2.Rows[rowIndex - 1].Cells[0];

            if (dataGridView2.Rows[rowIndex].Cells[1].Value.Equals(dataGridView2.Rows[rowIndex - 1].Cells[1].Value)
                && dataGridView2.Rows[rowIndex].Cells[2].Value.Equals(dataGridView2.Rows[rowIndex - 1].Cells[2].Value)
                && dataGridView2.Rows[rowIndex].Cells[3].Value.Equals(dataGridView2.Rows[rowIndex - 1].Cells[3].Value)
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Первую строку всегда показывать
            if (e.RowIndex == 0)
                return;


            if (IsRepeatedCellValue(e.RowIndex, e.ColumnIndex) && e.ColumnIndex < 4)
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        private void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            // Пропуск заголовков колонок и строк, и первой строки
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;

            if (IsRepeatedCellValue(e.RowIndex, e.ColumnIndex) && e.ColumnIndex < 4)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Single;
            }
        }

        private void FillPlanWeek()
        {
            if (dgvPlan.Rows != null)
            {
                // dgvPlan.Columns.Clear();
                dgvPlan.Rows.Clear();
            }

            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec SelPlanWeek @p1, @p2, @p3, @p4, @p5", comboBox3.SelectedValue, comboBox7.SelectedValue, comboBox11.SelectedValue, dateTimePicker11.Value, dateTimePicker12.Value);

            if (dt == null)
                MessageBox.Show("Ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);

           dgvPlan.Columns[2].DefaultCellStyle.Format = "ddd d MMM";

            foreach (DataRow row in dt.Rows)
            {
                dgvPlan.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8]);
            }
            if (globalData.UserAccess == 5)
            {
                dgvPlan.Columns[0].Visible = false;
                dgvPlan.Columns[1].Visible = false;
            }
            else
            {
                dgvPlan.Columns[0].Visible = true;
                dgvPlan.Columns[1].Visible = true;
            }

            if (comboBox3.SelectedIndex != 0)
                dgvPlan.Columns[0].Visible = false;

            if (comboBox7.SelectedIndex != 0)
                dgvPlan.Columns[1].Visible = false;

            if (comboBox11.SelectedIndex != 0)
                dgvPlan.Columns[3].Visible = false;
        }

        private void button41_Click(object sender, EventArgs e)
        {
            FillPlanWeek();
        }

        private bool IsRepeatedCellValue1(int rowIndex, int colIndex)
        {
            DataGridViewCell currCell = dgvPlan.Rows[rowIndex].Cells[0];
            DataGridViewCell prevCell = dgvPlan.Rows[rowIndex - 1].Cells[0];

            if (dgvPlan.RowCount >= 2 && dgvPlan.Rows[rowIndex].Cells[1].Value != null)
            {
                if (dgvPlan.Rows[rowIndex].Cells[1].Value.Equals(dgvPlan.Rows[rowIndex - 1].Cells[1].Value)
                    && dgvPlan.Rows[rowIndex].Cells[2].Value.Equals(dgvPlan.Rows[rowIndex - 1].Cells[2].Value)
                    && dgvPlan.Rows[rowIndex].Cells[3].Value.Equals(dgvPlan.Rows[rowIndex - 1].Cells[3].Value)
                    )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void dgvPlan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Первую строку всегда показывать
            if (e.RowIndex == 0)
                return;


            if (IsRepeatedCellValue1(e.RowIndex, e.ColumnIndex) && e.ColumnIndex < 4)
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        private void dgvPlan_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            // Пропуск заголовков колонок и строк, и первой строки
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;

            if (IsRepeatedCellValue1(e.RowIndex, e.ColumnIndex) && e.ColumnIndex < 4)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.Single;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUser(comboBox7, comboBox3);
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLPU(comboBox11, 1, comboBox7.SelectedValue);
        }

        private void dgvPlan_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPlan.CurrentCell.ColumnIndex == 7)
            {
                Komment kom = new Komment();

                if (dgvPlan[7, dgvPlan.CurrentCell.RowIndex].Value != null)
                {
                    kom.komment = dgvPlan[7, dgvPlan.CurrentCell.RowIndex].Value.ToString();
                    kom.ShowDialog();
                }
            }
        }

        private void button42_Click(object sender, EventArgs e)
        {
            textBox6.Clear();

            if (dgvDaily.Rows != null)
            {
                dgvDaily.Rows.Clear();
            }

            maskedTextBox1.Visible = false;
            maskedTextBox2.Visible = false;

            label31.Visible = false;
            label37.Visible = false;
            label36.Visible = false;
            label34.Visible = false;
            label35.Visible = false;

            label46.Visible = true;
            dateTimePicker16.Visible = true;
            //dateTimePicker16.Value = dateTimePicker10.Value.AddDays(7);

            label32.Text = "Контактное лицо";
            button34.Text = "Добавить еще ЛПУ";
            button35.Text = "Сохранить план";

            textBox4.Visible = false;
            textBox5.Visible = false;

            cbStatusd.Visible = false;

            button37.Visible = false;
            button38.Visible = false;

            tabControl1.SelectedIndex = 18;
            tabControl1.Visible = true;

            if (splitContainer1.Panel1Collapsed == false)
                splitContainer1.Panel1Collapsed = true;

            LoadProducts();
            LoadLPU(cbLPUd, 0, globalData.UserID);

            dgvDaily.Columns[4].Visible = false;
        }
        // Выгрузить в Excel
        private void button43_Click(object sender, EventArgs e)
        {
            ExportDaily(dgvPlan);
        }

        private void button45_Click(object sender, EventArgs e)
        {
            CultureInfo myCI = new CultureInfo("ru-RU");
            Calendar myCal = myCI.Calendar;
            DateTime date = new DateTime();
            
            date = DateTime.Now;
            DateTime dtNow = DateTime.Now;
            int nWeekNumber = myCal.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            //if (date.DayOfWeek == DayOfWeek.Monday && date.TimeOfDay.Hours == 12)

            if (tabControl1.SelectedIndex == 19)
            {
                if (dgvPlan.Rows.Count != 0)
                {
                    Sql sql1 = new Sql();

                    foreach (DataGridViewCell cell in dgvPlan.SelectedCells)
                    {
                        if (cell.RowIndex != -1)
                        {
                            dtNow = Convert.ToDateTime(dgvPlan.Rows[cell.RowIndex].Cells[2].Value.ToString());
                            int nWeekNumberOld = myCal.GetWeekOfYear(dtNow, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                            if (date.DayOfWeek == DayOfWeek.Monday && date.TimeOfDay.Hours <= 12 && nWeekNumber == nWeekNumberOld)
                            {
                                sql1.GetRecords("exec DelString2 @p1", dgvPlan.Rows[cell.RowIndex].Cells[8].Value);
                                dgvPlan.Rows.RemoveAt(cell.RowIndex);
                            }
                            if (nWeekNumber < nWeekNumberOld)
                            {
                                sql1.GetRecords("exec DelString2 @p1", dgvPlan.Rows[cell.RowIndex].Cells[8].Value);
                                dgvPlan.Rows.RemoveAt(cell.RowIndex);
                            }
                            if (nWeekNumber >= nWeekNumberOld)
                                continue;
                        }
                    }
                }
            }
        }
        //Выгрузить в Excel
        private void button44_Click(object sender, EventArgs e)
        {
            //ExportDaily(dataGridView2);
        }

        private void ExportDaily(DataGridView dgv)
        {
            object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

            try
            {
                int i = 1;

                // Пишем заголовки в Excel файл
                for (int j = 0; j < dgv.ColumnCount-1; j++)
                {
                   xlSh.Cells[1, i] = dgv.Columns[j].HeaderText;

                   ((Excel.Range)xlSh.Cells[1, i]).Font.Bold = true;
                   ((Excel.Range)xlSh.Cells[1, i]).HorizontalAlignment = Excel.Constants.xlCenter;

                   i++;
                }

                ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 25; //регион 
                ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 10; // рег.пред
                ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 13; // число
                ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 10; // лпу
                ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 15; // контактное лицо
                ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 10; // наименование
                ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 13; // статус
                ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 10; //комментарий


                i = 2;
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    int k = 1;

                    for (int j = 0; j < dgv.ColumnCount-1; j++)
                    {
                        if (row.Cells[j].Value != null)
                        {
                            if ((dgv.Columns[j].HeaderText == "Число") && (row.Cells[j].Value.ToString() != String.Empty))
                                xlSh.Cells[i, k] = Convert.ToDateTime(row.Cells[j].Value).ToShortDateString();
                            else
                                xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                        }

                        ((Excel.Range)xlSh.Cells[i, k]).HorizontalAlignment = Excel.Constants.xlLeft;
                        //((Excel.Range)xlSh.Cells[i, 2]).HorizontalAlignment = Excel.Constants.xlRight;
                        //((Excel.Range)xlSh.Cells[i, 2]).NumberFormat = "###";
                        //((Excel.Range)xlSh.Cells[i, 7]).HorizontalAlignment = Excel.Constants.xlRight;
                        //((Excel.Range)xlSh.Cells[i, 7]).NumberFormat = "###,##" + " руб";
                        k++;
                    }
                    i++;
                }
                //String filename = "";
                SaveFileDialog dial = new SaveFileDialog();
                dial.Filter = "Excel | *.xls";
                dial.ShowDialog();
                //filename = @"J:\Hospital Care\Kasyanova Tatyana\" + " по региону " + ".xls";

                xlApp.DisplayAlerts = false;
                //xlWB.SaveAs(filename, Excel.XlFileFormat.xlWorkbookNormal);
                xlWB.SaveAs(dial.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                xlWB.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlSh);
                releaseObject(xlWB);
                releaseObject(xlApp);

                //System.IO.File.Delete(filename);

            }
            catch (Exception err)
            {
                try
                {
                    xlWB.Close(false, misValue, misValue);
                    xlApp.Quit();
                    releaseObject(xlSh);
                    releaseObject(xlWB);
                    releaseObject(xlApp);
                }
                catch (Exception err2)
                {
                    MessageBox.Show("Ошибка при очистки объекта. Системная ошибка: " + err2.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Ошибка при выгрузке данных. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbPres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                FillDaily();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 4 && e.ColumnIndex < 29)
                return;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridView2.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
            {
                Process.Start(this.dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            if (_dgv1.Rows.Count == 0)
                return;
            if (Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["bum"].Value) == 0)
            {
                MessageBox.Show("Нулевое количество нельзя переносить", "Запрет на перенос", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Sql sql1 = new Sql();

            string str = sql1.GetRecordsOne("exec GetRepDistAllowNew @p1, @p2", _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value, _dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[1].Value);

            if (str != "1")
            {
                MessageBox.Show("По данному дистрибьютору отсутствует отчёт, распределять продажи можно только после получения отчёта от дистрибьютора", "Запрет на разнесение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }            

            MoveReport mr = new MoveReport(Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value), Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[1].Value), Convert.ToInt32(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells["bum"].Value));
            mr.ShowDialog();

            if (globalData.update)
            {
                loadData();
                globalData.update = false;
            }
            
        }

        private void SelRepDist()
        {
            tabControl1.SelectedIndex = 8;
            tabControl1.Visible = true;

            if (globalData.UserAccess == 1)
                btnDelRepDist.Visible = true;
            else
                btnDelRepDist.Visible = false;

            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
            if (cbSubRegRepDist.Visible == false)
                dt1 = sql1.GetRecords("exec SelRepDist @p1, @p2, @p3", cbCustRepDist.SelectedValue, dateTimePicker17.Value.Year.ToString() + "-" + dateTimePicker17.Value.Month.ToString() + "-01", dateTimePicker18.Value.Year.ToString() + "-" + dateTimePicker18.Value.Month.ToString() + "-01");
            else
                dt1 = sql1.GetRecords("exec SelRepDist2 @p1, @p2, @p3, @p4", cbCustRepDist.SelectedValue, cbSubRegRepDist.SelectedValue, dateTimePicker17.Value.Year.ToString() + "-" + dateTimePicker17.Value.Month.ToString() + "-01", dateTimePicker18.Value.Year.ToString() + "-" + dateTimePicker18.Value.Month.ToString() + "-01");
            _dgv9.DataSource = dt1;

            if (dt1 == null)
            {
                _dgv9.Rows.Clear();
                return;
            }

            if (globalData.UserID != 1 || globalData.UserID != 258)
            {
                _dgv9.Columns["rd_id"].Visible = false;
                _dgv9.Columns["db_id"].Visible = false;
            }
            else
            {
                _dgv9.Columns["rd_id"].Visible = true;
                _dgv9.Columns["db_id"].Visible = true;
            }

            _dgv9.Columns["rd_date"].HeaderText = "Дата";
            _dgv9.Columns["cust_code"].HeaderText = "Код дистрибьютора";
            _dgv9.Columns["cust_name"].HeaderText = "Название дистрибьютора";
            _dgv9.Columns["subreg_nameRus"].HeaderText = "Субрегион";
            _dgv9.Columns["mat_code"].HeaderText = "Артикул";
            _dgv9.Columns["mat_name"].HeaderText = "Название продукции";            
            _dgv9.Columns["rd_lpu"].HeaderText = "Покупатель";
            _dgv9.Columns["rd_count"].HeaderText = "Количество";
        }

        private void btnRepDistExcel_Click(object sender, EventArgs e)
        {
            ExportInExcel(_dgv9);
        }

        private string replaceMonth(string monthName)
        {
            monthName = monthName.ToLower();

            if (monthName == "январь")
                return "1";
            if (monthName == "февраль")
                return "2";
            if (monthName == "март")
                return "3";
            if (monthName == "апрель")
                return "4";
            if (monthName == "май")
                return "5";
            if (monthName == "июнь")
                return "6";
            if (monthName == "июль")
                return "7";
            if (monthName == "август")
                return "8";
            if (monthName == "сентябрь")
                return "9";
            if (monthName == "октябрь")
                return "10";
            if (monthName == "ноябрь")
                return "11";
            return "12";
        }              

        private void alfaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 2, "20564279_Петербургский проект", "H", "B", "C", "F", "", "9");
        }

        private void protekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 8, "20438427_Протек", "G", "I", "K", "P", "B", "18");
        }                

        private void listMatDistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Material2 m2 = new Material2();
            m2.ShowDialog();
        }

        private void RepDistRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RepDistRight rdr = new RepDistRight();
            rdr.ShowDialog();
        }

        private void cbCustRepDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelRepDist();
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            SelRepDist();
        }      

        private void loadMaterialRegion2(string folderName)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;


                String header = String.Empty;

                int i = 2;

                try
                {
                    Sql sql1 = new Sql();

                    string s4 = folderName.Split('_')[0];

                    while (xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2 != null)
                    {
                        header = "B";
                        string s1 = "";
                        if (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                            s1 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString();//mat2_code
                        header = "A";
                        string s2 = "";
                        if (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                            s2 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Replace("\"", "-");//mat2_name
                        header = "D";
                        string s3 = "1";
                        if (xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2 != null)
                            s3 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//mat2_count
                        if (s3 == "-2146826246")
                            s3 = "1";
                        header = "C";
                        string s5 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//mat_code

                        string res = sql1.GetRecordsOne("exec InsMaterial2 @p1, @p2, @p3, @p4, @p5", s1, s2, s3, s4, s5);

                        if (res != "1")
                        {
                            xlSh.Cells[i, 6] = res;
                            err++;
                        }
                        i++;
                    }

                    i = 2;
                    xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);
                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                    {
                        header = "A";
                        string s1 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//subreg2_name
                        header = "B";

                        if (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 == null)
                        {
                            i++;
                            continue;
                        }
                        string s2 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString();//subreg_name

                        string res = sql1.GetRecordsOne("exec InsSubRegion2 @p1, @p2, @p3", s1, s2, s4);

                        if (res != "1")
                        {
                            xlSh.Cells[i, 3] = res;
                            err++;
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void rFarmToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438646_Р-Фарм");
        }

        private void euroServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20437526_Евросервис");
        }

        private void delRusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438401_Дельрус");
        }

        private void lMIToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438412_ЛМИ");
        }

        private void bioTechnoTronicToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438587_Биотехнотроник");
        }

        private void bSSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438929_БСС");
        }

        private void katrenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20439804_Катрен");
        }

        private void farmProjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20529590_Фарм-Проект");
        }

        private void riFarm59ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20537866_Рифарм 59");
        }

        private void alfaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20564279_Петербургский проект");
        }

        private void iMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20439661_ИМС");
        }

        private void protekToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438427_Протек");
        }

        private void unixToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20438448_Юникс");
        }

        private void farmSKDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20437463_Фарм СКД");
        }

        private void фармацевтическаяКомпанияРутаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20440182_Рута");
        }
        

        private void loadRepDist(string pass, int begin, string folderName, string reg, string mat2Code, string mat2Name, string count, string lpu, string last)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, pass, "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                String header = String.Empty;

                int i = begin;

                try
                {
                    Sql sql1 = new Sql();

                    InputDialog ind = new InputDialog("Год", "Введите год", true);
                    ind.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    String year = globalData.input;

                    InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                    id.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    string month = globalData.input;

                    string s1 = year + "-" + month + "-01";

                    string s2 = folderName.Split('_')[0];

                    while (xlSh.get_Range(mat2Name + i.ToString(), mat2Name + i.ToString()).Value2 != null)
                    {
                        header = reg;
                        string s3 = xlSh.get_Range(reg + i.ToString(), reg + i.ToString()).Value2.ToString();//Region
                        header = mat2Name;
                        string s4 = xlSh.get_Range(mat2Name + i.ToString(), mat2Name + i.ToString()).Value2.ToString().Replace("\"", "-");//mat2Name
                        header = count;
                        string s5 = xlSh.get_Range(count + i.ToString(), count + i.ToString()).Value2.ToString().Replace(',', '.').Replace("уп", "").Replace("шт", "");//count
                        if (s5 == "Не определено")
                        {
                            i++;
                            continue;
                        }
                        header = lpu;
                        string s6 = "";
                        if (lpu != "")
                            if (xlSh.get_Range(lpu + i.ToString(), lpu + i.ToString()).Value2 != null)
                                s6 = xlSh.get_Range(lpu + i.ToString(), lpu + i.ToString()).Value2.ToString();//lpu
                        string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                        if (res != "1")
                        {
                            xlSh.Cells[i, Convert.ToInt16(last)] = res;
                            err++;
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void bioTechnoTronicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderName = "20438587_Биотехнотроник";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                String header = String.Empty;

                int i = 3;

                try
                {
                    Sql sql1 = new Sql();

                    InputDialog ind = new InputDialog("Год", "Введите год", true);
                    ind.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    String year = globalData.input;

                    InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                    id.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    string month = globalData.input;

                    string s1 = year + "-" + month + "-01";

                    string s2 = folderName.Split('_')[0];

                    while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                    {
                        if (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 == null)
                        {
                            i++;
                            continue;
                        }

                        header = "D";
                        string s3 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//Region
                        header = "B";
                        string s4 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString().Replace("\"", "-");//matName
                        header = "C";
                        string s5 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//count
                        header = "E";
                        string s6 = xlSh.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString();//LPU

                        string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                        if (res != "1")
                        {
                            xlSh.Cells[i, 7] = res;
                            err++;
                        }
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void bSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderName = "20438929_БСС";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                String header = String.Empty;

                int i = 2;

                try
                {
                    Sql sql1 = new Sql();

                    InputDialog ind = new InputDialog("Год", "Введите год", true);
                    ind.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    String year = globalData.input;

                    InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                    id.ShowDialog();

                    if (ind.DialogResult == DialogResult.Cancel)
                        return;

                    string month = globalData.input;

                    string s1 = year + "-" + month + "-01";

                    string s2 = folderName.Split('_')[0];

                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                    {
                        if (xlSh.get_Range("A" + (i + 1).ToString(), "A" + (i + 1).ToString()).Value2 != null)
                        {
                            i++;
                            continue;
                        }

                        header = "A";
                        string s4 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Replace("\"", "-");//matName
                        i++;

                        while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                        {
                            header = "C";
                            string s3 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString();//Region
                            header = "D";
                            string s5 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//count
                            header = "B";
                            string s6 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString();//LPU

                            string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                            if (res != "1")
                            {
                                xlSh.Cells[i, 6] = res;
                                err++;
                            }
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void delRusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("bbraun", 2, "20438401_Дельрус", "B", "C", "C", "F", "B", "7");
        }

        private void euroServiceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadRepDist("", 5, "20437526_Евросервис", "C", "E", "F", "H", "D", "9");
        }

        private void interMedServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 5, "20439661_ИМС", "B", "C", "D", "E", "I", "10");
        }

        private void katrenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 2, "20439804_Катрен", "G", "F", "B", "C", "", "9");
        }

        private void фармацевтическаяКомпанияРутаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 4, "20440182_Рута", "B", "", "D", "G", "C", "8");
        }

        private void lMIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderName = "20438412_ЛМИ";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Год", "Введите год", true);
                ind.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                String year = globalData.input;

                InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                id.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                string month = globalData.input;

                string s1 = year + "-" + month + "-01";

                String header = String.Empty;

                string s2 = folderName.Split('_')[0];

                int i = 11;

                try
                {
                    Sql sql1 = new Sql();

                    header = "A";
                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                    {
                        header = "A";
                        string s6 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//LPU

                        string s3 = s6;

                        i++;
                        while (xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2 != null)
                        {
                            if (xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString() == " ")
                                break;
                            header = "A";
                            string s4 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Replace("\"", "-");//matName
                            header = "D";
                            string s5 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString().Replace(',', '.');//count

                            string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                            if (res != "1")
                            {
                                xlSh.Cells[i, 5] = res;
                                err++;
                            }
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void rFarmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 2, "20438646_Р-Фарм", "L", "M", "G", "I", "K", "13");
        }

        private void riFarm59ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 3, "20537866_Рифарм 59", "F", "B", "B", "A", "D", "7");
        }

        private void farmProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //loadRepDist("", 2, "20529590_Фарм-Проект", "C", "", "A", "D", "B", "6");
            string folderName = "20529590_Фарм-Проект";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Год", "Введите год", true);
                ind.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                String year = globalData.input;

                InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                id.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                string month = globalData.input;

                string s1 = year + "-" + month + "-01";

                String header = String.Empty;

                string s2 = folderName.Split('_')[0];

                int i = 12;

                try
                {
                    Sql sql1 = new Sql();

                    header = "C";
                    while (xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2 != null)
                    {

                        header = "C";
                        string s6 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString().Replace("\"", "-");//LPU

                        i++;
                        header = "C";
                        while ((bool)xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Font.Bold != true)
                        {

                            header = "F";
                            string s5 = xlSh.get_Range("F" + i.ToString(), "F" + i.ToString()).Value2.ToString();//count

                            header = "C";
                            string s4 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString().Replace("\"", "-");//matName

                            header = "D";
                            string s3 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//Region

                            string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s6, s5, s4);
                            if (res != "1")
                            {
                                xlSh.Cells[i, 7] = res;
                                err++;
                            }

                            i++;
                        }
                        //i++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void unixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderName = "20438448_Юникс";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Год", "Введите год", true);
                ind.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                String year = globalData.input;

                InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                id.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                string month = globalData.input;

                string s1 = year + "-" + month + "-01";

                String header = String.Empty;

                string s2 = folderName.Split('_')[0];

                int i = 11;

                string s6 = "";

                try
                {
                    Sql sql1 = new Sql();

                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString() != "Итого")
                    {
                        header = "A";
                        string s3 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//Region
                        header = "C";

                        i += 1;
                        while (xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2 != null)
                        {
                            header = "A";
                            string s4 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Trim().Replace("\"", "-");//matName
                            header = "G";
                            string s5 = xlSh.get_Range("G" + i.ToString(), "G" + i.ToString()).Value2.ToString();//count                            

                            string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                            if (res != "1")
                            {
                                xlSh.Cells[i, 8] = res;
                                err++;
                            }

                            //if ((xlSh.get_Range("C" + (i + 1).ToString(), "C" + (i + 1).ToString()).Value2 == null) && (xlSh.get_Range("C" + (i + 2).ToString(), "C" + (i + 2).ToString()).Value2 != null))
                            //    i += 2;
                            //else
                                i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void farmSKDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string folderName = "20437463_Фарм СКД";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Год", "Введите год", true);
                ind.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                String year = globalData.input;

                InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                id.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                string month = globalData.input;

                string s1 = year + "-" + month + "-01";

                String header = String.Empty;

                string s2 = folderName.Split('_')[0];

                int i = 2;
                //int i = 5;

                string s6 = "";

                try
                {
                    Sql sql1 = new Sql();

                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null || xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString() != "Итого:")
                    {
                        header = "B";
                        string s4 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString().Replace("\"", "-").Replace("(", ":").Replace(" с. ", ":").Split(':')[0].Trim();//matName

                        i++;
                        while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 == null)
                        {
                            header = "B";
                            string s3 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString().Split(':')[0].Trim();//reg + lpu
                            //string s3 = xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString().Trim();//reg + lpu
                            s6 = s3;
                            header = "C";
                            string s5 = xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString().Replace(",", ".");//count

                            string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                            if (res != "1")
                            {
                                xlSh.Cells[i, 5] = res;
                                err++;
                            }
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void checkKosReport()
        {
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec selDist");
            load = false;
            cbDist.DataSource = dt1;
            cbDist.DisplayMember = "cust_name";
            cbDist.ValueMember = "cust_id";

            cbDistDiv.SelectedIndex = 0;

            if (KosReg == false)
            {
                cbDistDiv.Visible = true;
                dt1 = sql1.GetRecords("exec selDistMat @p1, @p2, @p3", dateTimePicker19.Value.Year.ToString() + "-" + dateTimePicker19.Value.Month.ToString() + "-01", cbDist.SelectedValue, cbDistDiv.SelectedItem);

                cbDistMaterial.DataSource = dt1;
                cbDistMaterial.DisplayMember = "mat_name";
                cbDistMaterial.ValueMember = "mat_id";
                load = true;
            }
            else
            {
                cbDistDiv.Visible = false;
                if (globalData.UserAccess == 1)
                {
                    dt1 = sql1.GetRecords("exec SelRegion2");

                    cbDistMaterial.DataSource = dt1;
                    cbDistMaterial.DisplayMember = "reg_nameRus";
                    cbDistMaterial.ValueMember = "reg_id";
                    load = true;
                }
                else
                {
                    dt1 = sql1.GetRecords("exec SelRegionByUserID @p1", globalData.UserID);

                    cbDistMaterial.DataSource = dt1;
                    cbDistMaterial.DisplayMember = "reg_nameRus";
                    cbDistMaterial.ValueMember = "reg_id";
                    load = true;
                }
            }

            //checkKosReportLoadData();
        }

        private void checkKosReportLoadData()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            if (KosReg == false)
            {
                dt1 = sql1.GetRecords("exec SelCheckDistRepNew @p1, @p2, @p3", dateTimePicker19.Value.Year.ToString() + "-" + dateTimePicker19.Value.Month.ToString() + "-01", cbDist.SelectedValue, cbDistMaterial.SelectedValue);
            }
            else
            {
                dt1 = sql1.GetRecords("exec SelCheckDistRepRegNew @p1, @p2, @p3", dateTimePicker19.Value.Year.ToString() + "-" + dateTimePicker19.Value.Month.ToString() + "-01", cbDist.SelectedValue, cbDistMaterial.SelectedValue);
            }
            _dgv18.DataSource = dt1;

            if (dt1 == null)
                return;

            formatCheckDistRep();
        }

        private void formatCheckDistRep()
        {
            if (KosReg == true)
            {
                _dgv18.Columns["id"].Visible = false;
                _dgv18.Columns["idmat"].Visible = false;

                _dgv18.Columns["mat_name"].HeaderText = "Номенклатура";
                _dgv18.Columns["RF"].HeaderText = "РФ";
                _dgv18.Columns["repDist"].HeaderText = "Отчёт дистрибьютора";
                _dgv18.Columns["rasp"].HeaderText = "Распределено";
                _dgv18.Columns["tail"].HeaderText = "Остаток";

                _dgv18.Columns["RF"].DefaultCellStyle.Format = "N0";
                _dgv18.Columns["repDist"].DefaultCellStyle.Format = "N0";
                _dgv18.Columns["rasp"].DefaultCellStyle.Format = "N0";
                _dgv18.Columns["tail"].DefaultCellStyle.Format = "N0";

                _dgv18.Columns["mat_name"].Width = 200;

                foreach (DataGridViewRow row in _dgv18.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "2")
                        row.DefaultCellStyle.BackColor = bbgreen3;
                }

                _dgv18.Columns["oldtail"].Visible = false;
            }
            else
            {
                _dgv18.Columns["id"].Visible = false;
                _dgv18.Columns["reg_id"].Visible = false;

                _dgv18.Columns["reg_name"].HeaderText = "Регион";
                _dgv18.Columns["sap"].HeaderText = "SAP";
                _dgv18.Columns["sap"].Visible = false;
                _dgv18.Columns["repDist"].HeaderText = "Отчёт дистрибьютора";
                _dgv18.Columns["rasp"].HeaderText = "Распределено";
                _dgv18.Columns["tail"].HeaderText = "Остаток";

                _dgv18.Columns["sap"].DefaultCellStyle.Format = "N0";
                _dgv18.Columns["repDist"].DefaultCellStyle.Format = "N0";
                _dgv18.Columns["rasp"].DefaultCellStyle.Format = "N0";
                _dgv18.Columns["tail"].DefaultCellStyle.Format = "N0";

                _dgv18.Columns["reg_name"].Width = 200;

                foreach (DataGridViewRow row in _dgv18.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "1")
                        row.DefaultCellStyle.BackColor = bbgray5;
                    if (row.Cells[0].Value.ToString() == "3")
                        row.DefaultCellStyle.BackColor = bbgreen3;
                }

                _dgv18.Columns["oldtail"].Visible = false;
                _dgv18.Columns["oldtail2"].Visible = false;
            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            checkKosReportLoadData();
        }

        private void cbDistMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(load)
                checkKosReportLoadData();
        }

        private void btnDelRepDist_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            sql1.GetRecords("exec DelRepDist @p1, @p2", dateTimePicker17.Value.Year.ToString() + "-" + dateTimePicker17.Value.Month.ToString() + "-01", cbCustRepDist.SelectedValue);
        }

        private void checkDistReport()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec selDist");

            load = false;
            cbYear8.SelectedIndex = 0;

            cbDistr8.DataSource = dt1;
            cbDistr8.DisplayMember = "cust_name";
            cbDistr8.ValueMember = "cust_id";

            dt1 = sql1.GetRecords("exec SelRegion2");

            cbReg8.DataSource = dt1;
            cbReg8.DisplayMember = "reg_nameRus";
            cbReg8.ValueMember = "reg_id";
            load = true;

            checkDistReportLoadData();
        }

        private void checkDistReportLoadData()
        {
            Sql sql1 = new Sql();
            
            if (_dgv21.Rows != null)
            {   
                _dgv21.Columns.Clear();
                _dgv21.Rows.Clear();
            }

            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec SelCheckDistRep2 @p1, @p2, @p3", Convert.ToInt32(cbYear8.SelectedItem.ToString()), cbDistr8.SelectedValue, cbReg8.SelectedValue);

            if (dt1 == null)
                MessageBox.Show("Нет записей!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _dgv21.Columns.Add("nom", "Номенклатура");
            _dgv21.Columns.Add("delta", "Дельта");
            
            foreach (DataRow row in dt1.Rows)
            {
                _dgv21.Rows.Add(row.ItemArray[0], row.ItemArray[1]);
            }
           // formatcheckDistReport();
        }

        private void formatcheckDistReport()
        {
            
            
        }

        private void cbYear8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                checkDistReportLoadData();
        }

        private void cbDistr8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                checkDistReportLoadData();
        }

        private void cbReg8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
                checkDistReportLoadData();
        }

        private void cbDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
            load = false;

            if (KosReg == false)
            {
                dt1 = sql1.GetRecords("exec selDistMat @p1, @p2, @p3", dateTimePicker19.Value.Year.ToString() + "-" + dateTimePicker19.Value.Month.ToString() + "-01", cbDist.SelectedValue, cbDistDiv.SelectedItem);

                cbDistMaterial.DataSource = dt1;
                cbDistMaterial.DisplayMember = "mat_name";
                cbDistMaterial.ValueMember = "mat_id";
                load = true;
            }
            else
            {
                if (globalData.UserAccess == 1)
                {
                    dt1 = sql1.GetRecords("exec SelRegion2");

                    cbDistMaterial.DataSource = dt1;
                    cbDistMaterial.DisplayMember = "reg_nameRus";
                    cbDistMaterial.ValueMember = "reg_id";
                    load = true;
                }
                else
                {
                    dt1 = sql1.GetRecords("exec SelRegionByUserID @p1", globalData.UserID);

                    cbDistMaterial.DataSource = dt1;
                    cbDistMaterial.DisplayMember = "reg_nameRus";
                    cbDistMaterial.ValueMember = "reg_id";
                    load = true;
                }
            }

            checkKosReportLoadData();
        }

        private string getMatName(string begStr)
        {
            int i = 0;
            for(i = 0; i < begStr.Count(); i++)
            {
                if (begStr.Count() > i + 3)
                    if ((begStr[i] == ' ') && (begStr[i + 1] == 'с') && (begStr[i + 2] == '.'))
                        break;
            }
            return begStr.Remove(i);
        }

        private void fillReg(ComboBox cbReg)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelRegByRD @p1", cbRDEvoRP.SelectedValue);

            fillComboBox(dt1, cbReg, "reg_nameRus", "reg_id");
        }

        private void fillRD(ComboBox cbRD)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec GetRDByUser @p1", globalData.UserID);

            fillComboBox(dt1, cbRD, "user_name", "user_id");
        }

        private void fillComboBox(DataTable dt, ComboBox cb, string Display, string Value)
        {
            globalData.load = false;
            cb.DataSource = dt;
            cb.DisplayMember = Display;
            cb.ValueMember = Value;
            globalData.load = true;
        }

        private void fillUsersEvoRP(ComboBox cbU)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsersForAllAcc @p1, @p2, @p3, @p4", cbRegEvoRP.SelectedValue, cbSDivEvoRP.SelectedItem, cbYearEvoRP.SelectedItem, cbRDEvoRP.SelectedValue);

            fillComboBox(dt1, cbU, "user_name", "user_id");
        }

        private void cbSDivEvoRP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSDivEvoRP.SelectedItem.ToString() == "HC")
            {
                btnHideEvoUsersRP.Visible = true;
                cbRegEvoRP.Visible = true;
                if (btnHideEvoUsersRP.Text == "Отобразить РП")
                    cbUsersEvoRP.Visible = false;
                else
                    cbUsersEvoRP.Visible = true;
            }
            else
            {
                cbUsersEvoRP.Visible = false;
                btnHideEvoUsersRP.Visible = false;
                cbRegEvoRP.Visible = false;
            }
        }

        private void btnHideEvoUsersRP_Click(object sender, EventArgs e)
        {
            if (btnHideEvoUsersRP.Text == "Отобразить РП")
            {
                btnHideEvoUsersRP.Text = "Скрыть РП";
                cbUsersEvoRP.Visible = true;
            }
            else
            {
                btnHideEvoUsersRP.Text = "Отобразить РП";
                cbUsersEvoRP.Visible = false;                
            }
        }

        private void cbRDEvoRP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                fillReg(cbRegEvoRP);
                if (cbSDivEvoRP.SelectedItem.ToString() == "HC")
                    fillUsersEvoRP(cbUsersEvoRP);
            }
        }

        private void cbYearEvoRP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((globalData.load) && (cbSDivEvoRP.SelectedItem.ToString() == "HC"))
                fillUsersEvoRP(cbUsersEvoRP);
        }

        private void cbRegEvoRP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((globalData.load) && (cbSDivEvoRP.SelectedItem.ToString() == "HC"))
                fillUsersEvoRP(cbUsersEvoRP);
        }

        private void btnApplyEvoRP_Click(object sender, EventArgs e)
        {
            if ((cbSDivEvoRP.SelectedItem.ToString() == "HC") && (cbUsersEvoRP.Items.Count == 0))
            {
                _dgv19.Columns.Clear();
                MessageBox.Show("Построение выполнения плана не возможно. Пользователи не найдены.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string user = "0";
            if ((cbUsersEvoRP.Visible) && (cbSDivEvoRP.SelectedItem.ToString() == "HC"))
                user = cbUsersEvoRP.SelectedValue.ToString();

            selEvo(_dgv19, cbSDivEvoRP.SelectedItem.ToString(), dateTimePicker20.Value.Month.ToString(), cbYearEvoRP.SelectedItem.ToString(), cbRegEvoRP.SelectedValue.ToString(), cbRDEvoRP.SelectedValue.ToString(), user);
        }

        private void trackersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Trackers tr = new Trackers();
            tr.ShowDialog();
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            if (cbSubRegRepDist.Visible == false)
            {
                button18.Text = "Скрыть субрегион";
                cbSubRegRepDist.Visible = true;
                cbCustRepDist.Enabled = false;
            }
            else
            {
                button18.Text = "Показать субрегион";
                cbSubRegRepDist.Visible = false;
                cbCustRepDist.Enabled = true;
            }
        }

        private void cbSubRegRepDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelRepDist();
        }

        /*---------------------------------------------Визиты----------------------------------*/

        private void btnMonthLeftVisitPlan_Click(object sender, EventArgs e)
        {
            if (dtpVisitPlanDay.Value.Month == 1)
                dtpVisitPlanDay.Value = Convert.ToDateTime("01-12-" + (dtpVisitPlanDay.Value.Year - 1).ToString());
            else
                dtpVisitPlanDay.Value = Convert.ToDateTime("01-" + (dtpVisitPlanDay.Value.Month - 1).ToString() + "-" + dtpVisitPlanDay.Value.Year.ToString());
        }

        private void btnMonthRightVisitPlan_Click(object sender, EventArgs e)
        {
            if (dtpVisitPlanDay.Value.Month == 12)
                dtpVisitPlanDay.Value = Convert.ToDateTime("01-01-" + (dtpVisitPlanDay.Value.Year + 1).ToString());
            else
                dtpVisitPlanDay.Value = Convert.ToDateTime("01-" + (dtpVisitPlanDay.Value.Month + 1).ToString() + "-" + dtpVisitPlanDay.Value.Year.ToString());
        }                       

        private void _dgvVisitPlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0) || (_dgvVisitPlan[e.ColumnIndex, e.RowIndex].Value.ToString() == ""))
                return;

            DateTime selDate = Convert.ToDateTime(dtpVisitPlanDay.Value.Year.ToString() + "-" + dtpVisitPlanDay.Value.Month.ToString() + "-" + _dgvVisitPlan[e.ColumnIndex, e.RowIndex].Value.ToString().Split('\n')[0]);

            VisitPlanDay vpd = new VisitPlanDay(Convert.ToInt32(cbUsersVisitPlanDays.SelectedValue), selDate);
            vpd.ShowDialog();          
        }
        
        private void cbUsersVisitPlanDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadVisitPlanDays();
        }

        private void dateTimePicker21_ValueChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadVisitPlanDays();
        }

        private void _dgvVisitPlan_Resize(object sender, EventArgs e)
        {
            resizeVisitPlanDays();
        }

        private void loadVisitPlanDays()
        {
            Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();

            int x = 0, y = 0;

            if (_dgvVisitPlan.Rows.Count > 0)
            {
                x = _dgvVisitPlan.CurrentCell.ColumnIndex;
                y = _dgvVisitPlan.CurrentCell.RowIndex;
            }

            dt1 = sql1.GetRecords("exec SelMonthForVisitPlan @p1", dtpVisitPlanDay.Value.Year.ToString() + "-" + dtpVisitPlanDay.Value.Month.ToString() + "-01");

            if (dt1 == null)
                return;

            insertColVisitPlan();

            if (_dgvVisitPlan.Rows.Count > 0)
                _dgvVisitPlan.Rows.Clear();

            foreach (DataRow row in dt1.Rows)
            {
                _dgvVisitPlan.Rows.Add(row.ItemArray);

            }

            _dgvVisitPlan.Rows[y].Cells[x].Selected = true;
            _dgvVisitPlan.CurrentCell = _dgvVisitPlan.Rows[y].Cells[x];

            resizeVisitPlanDays();

            visitList = new VisitList(Convert.ToInt32(cbUsersVisitPlanDays.SelectedValue), new DateTime(dtpVisitPlanDay.Value.Year, dtpVisitPlanDay.Value.Month, 1));
        }

        private void insertColVisitPlan()
        {
            if (_dgvVisitPlan.Columns.Count == 0)
            {
                _dgvVisitPlan.Columns.Add("mo", "Пн");
                _dgvVisitPlan.Columns.Add("tu", "Вт");
                _dgvVisitPlan.Columns.Add("we", "Ср");
                _dgvVisitPlan.Columns.Add("th", "Чт");
                _dgvVisitPlan.Columns.Add("fr", "Пт");
                _dgvVisitPlan.Columns.Add("sa", "Сб");
                _dgvVisitPlan.Columns.Add("su", "Вс");

                foreach(DataGridViewColumn column in _dgvVisitPlan.Columns)
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void resizeVisitPlanDays()
        {
            foreach (DataGridViewColumn col in _dgvVisitPlan.Columns)
                col.Width = _dgvVisitPlan.Width / _dgvVisitPlan.Columns.Count;

            foreach (DataGridViewRow row in _dgvVisitPlan.Rows)
                row.Height = (_dgvVisitPlan.Height - _dgvVisitPlan.ColumnHeadersHeight) / _dgvVisitPlan.Rows.Count;
        }

        private void _dgvVisitPlan_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
                return;

            if (_dgvVisitPlan.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == string.Empty)
                return;

            int day = Convert.ToInt32(_dgvVisitPlan.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

            e.PaintBackground(e.ClipBounds, true); // фон отрисовывается стандартный

            Font font = _dgvVisitPlan.DefaultCellStyle.Font; // стандартный шрифт ячейки, используется для первой половины текста
            Font font2 = new Font("Microsoft Sans Serif", 7, FontStyle.Italic);

            using (var sf = new StringFormat())
            using (var firstBrush = new SolidBrush(e.CellStyle.ForeColor)) // кисть для первой половины текста
            using (var RedBrush = new SolidBrush(Color.Red)) // кисть для последней половины текста
            using (var GreenBrush = new SolidBrush(Color.Green)) // кисть для последней половины текста
            {
                sf.FormatFlags = StringFormatFlags.NoWrap; // чтобы при сжатии колонки текст не переносился на вторую строку
                sf.Trimming = StringTrimming.EllipsisWord; // будет отрисовываться многоточие, если текст не влезает в колонку

                string text = (string)e.FormattedValue; // полное (форматированное) значение ячейки
                string firstHalf = day.ToString() + "\n"; // первая часть текста

                e.Graphics.DrawString(firstHalf, font, firstBrush, e.CellBounds, sf); // первую половину текста рисуем стандартным шрифтом и цветом
                e.Graphics.DrawString("\n", font, firstBrush, e.CellBounds, sf); // первую половину текста рисуем стандартным шрифтом и цветом
                SizeF size = e.Graphics.MeasureString(firstHalf, font, e.CellBounds.Location, StringFormat.GenericTypographic); // размер первой половины текста

                float y = e.CellBounds.Y;

                StringBuilder sbCellToolTipText = new StringBuilder();

                var list = from visit in visitList.visitList
                           where visit.DateVisit.Day == day
                           select visit; 

                foreach (Visit visit in list)
                {
                    sbCellToolTipText.AppendLine(visit.GetPlanAndTime());

                    string lastHalf = visit.GetPlanAndTime();

                    var rect = new RectangleF(e.CellBounds.X, y + size.Height, e.CellBounds.Width, e.CellBounds.Height - size.Height); // ограничивающий прямоугольник для оставшейся части текста

                    Brush currentBrush = firstBrush;

                    if (visit.Status == Visit.VisitStatus.Complited)
                        currentBrush = GreenBrush;
                    if (visit.Status == Visit.VisitStatus.Uncomplited)
                        currentBrush = RedBrush;

                    e.Graphics.DrawString(lastHalf, font, currentBrush, rect, sf); // выводим вторую часть текста жирным шрифтом и другим цветом

                    if (visit.CommRD != string.Empty)
                    {
                        sbCellToolTipText.AppendLine(visit.CommRD);

                        size = e.Graphics.MeasureString(firstHalf, font, e.CellBounds.Location, StringFormat.GenericTypographic); // размер первой половины текста
                        y += size.Height;

                        lastHalf = visit.CommRD; // оставшаяся часть текста

                        rect = new RectangleF(e.CellBounds.X, y + size.Height, e.CellBounds.Width, e.CellBounds.Height - size.Height); // ограничивающий прямоугольник для оставшейся части текста
                        e.Graphics.DrawString(lastHalf, font2, firstBrush, rect, sf); // первую половину текста рисуем стандартным шрифтом и цветом
                    }

                    e.CellStyle.WrapMode = DataGridViewTriState.True;

                    firstHalf = lastHalf;

                    size = e.Graphics.MeasureString(firstHalf, font, e.CellBounds.Location, StringFormat.GenericTypographic); // размер первой половины текста
                    y += size.Height;
                }

                _dgvVisitPlan.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = sbCellToolTipText.ToString();
            }
            e.Handled = true; // сигнализируем, что закончили обработку
        }

        private void cbUsersVisitPlanReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
                loadVisitPlanReport();
        }

        private void dateVisitPlanReportBegin_ValueChanged(object sender, EventArgs e)
        {
            loadVisitPlanReport();
        }

        private void dateVisitPlanReportEnd_ValueChanged(object sender, EventArgs e)
        {
            loadVisitPlanReport();
        }

        private void loadVisitPlanReport()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec VisitPlanDay_Select_Report @p1, @p2, @p3", cbUsersVisitPlanReport.SelectedValue, dateVisitPlanReportBegin.Value, dateVisitPlanReportEnd.Value);

            _dgvVisitPlanReport.DataSource = dt1;

            _dgvVisitPlanReport.Columns[0].Visible = false;
            _dgvVisitPlanReport.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void loadUsersVisitPlan(ComboBox cbU)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            if (globalData.UserAccess == 5)
            {
                dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
                fillComboBox(dt1, cbU, "user_name", "user_id");
            }
            else
            {
                loadUsersVisPlan(cbU, globalData.CurDate.Year.ToString());
            }
        }

        private void loadUsersVisPlan(ComboBox cbU, string year)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            if (treeView1.SelectedNode.Parent.Parent != null)
                globalData.Div = treeView1.SelectedNode.Parent.Parent.Text;
            else
                globalData.Div = "";


            dt1 = sql1.GetRecords("exec SelUsersVisitPlan @p1, @p2, @p3, @p4", year, globalData.RD, globalData.UserID, globalData.Div);

            globalData.load = false;

            if (dt1 != null)
            {
                cbU.DataSource = dt1;
                cbU.DisplayMember = "user_name";
                cbU.ValueMember = "user_id";
            }
            globalData.load = true;
        }

        private void _dgv1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex != _dgv1.Columns[27].Index)
            {
                MessageBox.Show("Data error occurs:" + e.Exception.Message);
            }
        }

        private void userManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userManager um = new userManager();
            um.ShowDialog();
        }

        private void userBCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userBC ubc = new userBC();
            ubc.ShowDialog();
        }

        private void btnClearUserAcc_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in _dgv12.SelectedCells)
            {
                int id = Convert.ToInt32(_dgv12.Rows[cell.RowIndex].Cells[0].Value);

                User user = new User(id);
                user.ClearAcc(dateTimePicker6.Value);
            }

            selPSAcc();
        }

        private void btnHideLPUMA_Click(object sender, EventArgs e)
        {
            if (btnHideLPUMA.Text == "Скрыть ЛПУ")
            {
                cbLPUMA.Visible = false;
                btnHideLPUMA.Text = "Отобразить ЛПУ";
                
            }
            else
            {
                cbLPUMA.Visible = true;
                btnHideLPUMA.Text = "Скрыть ЛПУ";
               
            }

            fillLPUMA();
            SelMA();
        }

        void fillLPUMA()
        {
            Sql sql1 = new Sql();
            string user = "0";
            string reg = "0";

            if (cbUsersMA.Visible)
                user = cbUsersMA.SelectedValue.ToString();

            if (cbRegMA.Visible)
                reg = cbRegMA.SelectedValue.ToString();

            DataTable dt = new DataTable();

            dt = sql1.GetRecords("exec SelLPU_MA2 @p1, @p2, @p3, @p4, @p5", user, reg, globalData.Div, cbYearMA.SelectedItem, globalData.RD);
            
            load = false;
            cbLPUMA.DataSource = dt;
            cbLPUMA.DisplayMember = "lpu_sname";
            cbLPUMA.ValueMember = "lpu_id";
            load = true;
        }

        private void cbLPUMA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (load)
            {
                SelMA();
            }
        }

        private void экопромToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 3, "20497031_Экопром", "H", "B", "C", "D", "G", "9");
        }

        private void медикалИнтертрейдToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadRepDist("", 7, "20484247_МедикалИнтертрейд", "H", "B", "C", "D", "G", "9");
        }

        private void экопромToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20497031_Экопром");
        }

        private void медикалИнтертрейдToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20484247_МедикалИнтертрейд");
        }

        private void cbDistDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
             Sql sql1 = new Sql();

            DataTable dt1 = new DataTable();
     
            load = false;
           
            dt1 = sql1.GetRecords("exec selDistMat @p1, @p2, @p3", dateTimePicker19.Value.Year.ToString() + "-" + dateTimePicker19.Value.Month.ToString() + "-01", cbDist.SelectedValue, cbDistDiv.SelectedItem);

            cbDistMaterial.DataSource = dt1;
            cbDistMaterial.DisplayMember = "mat_name";
            cbDistMaterial.ValueMember = "mat_id";
            load = true;
            
            if (load)
                checkKosReportLoadData();
        }

        private void найтиСчетфактуруToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefDoc refdoc = new RefDoc();
            refdoc.ShowDialog();
        }

        void AddPlanHC(int nom)
        {
            Sql sql1 = new Sql();

            if (btnHideUsersAccNY.Text != "Показать пользователей" || btnHideLPUAccNY.Text != "Показать ЛПУ")
                sql1.GetRecords("exec AddAccPlanInLPU @p1, @p2, @p3", _dgv14.Rows[1].Cells["acc_id"].Value, 1, nom);
        }

        private void bCN_Click(object sender, EventArgs e)
        {
            AddPlanHC(3);
            loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }

        private void bBC_Click(object sender, EventArgs e)
        {
            AddPlanHC(1);
            loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }

        private void bCC_Click(object sender, EventArgs e)
        {
            AddPlanHC(3);
            AddPlanHC(4);
            loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }

        private void bICU_Click(object sender, EventArgs e)
        {
            AddPlanHC(4);
            loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }

        private void button44_Click_1(object sender, EventArgs e)
        {
            AddPlanHC(0);
            loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);
        }


        private void SelMAWithSBA()
        {
            Cursor = Cursors.WaitCursor;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            _dgv8.DataSource = null;


            string userID = "0";
            if (cbUsersMA.Visible)
                userID = cbUsersMA.SelectedValue.ToString();

            string reg = "0";
            if (globalData.Region == String.Empty)
            {
                if (cbRegMA.Visible)
                    reg = cbRegMA.SelectedValue.ToString();
            }
            else
                reg = globalData.Region;

            string lpu = "-1";
            if (cbLPUMA.Visible && cbLPUMA.SelectedItem != null)
                lpu = cbLPUMA.SelectedValue.ToString();

            int[] sba = new int[30];

            for (int i = 0; i < 30; i++)
                sba[i] = Convert.ToInt32(globalData.dtSBA.Rows[i][1]) == 1 ? Convert.ToInt32(globalData.dtSBA.Rows[i][0]) : 0;

            dt1 = sql1.GetRecords("exec SelMALPUWithSBA @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20,@p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36",
                globalData.Div, reg, userID, cbYearMA.SelectedItem, globalData.RD, lpu,
                sba[0], sba[1], sba[2], sba[3], sba[4], sba[5], sba[6], sba[7], sba[8], sba[9],
                sba[10], sba[11], sba[12], sba[13], sba[14], sba[15], sba[16], sba[17], sba[18], sba[19],
                sba[20], sba[21], sba[22], sba[23], sba[24], sba[25], sba[26], sba[27], sba[28], sba[29]);

            _dgv8.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось загрузить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            formatMarkAct(_dgv8);
            Cursor = Cursors.Default;
        }

        private void button47_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            if (globalData.dtSBA == null)
                globalData.dtSBA = sql1.GetRecords("exec fillSBA");

            AllocCosts ac;

            ac = new AllocCosts();
            ac.ShowDialog();

            SelMAWithSBA();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            if (button46.Text == "Показать доли")
            {
                button46.Text = "Скрыть доли";
                splitContainer2.Panel1Collapsed = false;
            }
            else
            {
                button46.Text = "Показать доли";
                splitContainer2.Panel1Collapsed = true;
            }
        }
   
        private void _dgv8_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void _dgv8_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (_dgv8.Rows.Count > 3 && e.Cell.Selected == true)
            {
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();
                if (_dgv8.Rows[e.Cell.RowIndex].DefaultCellStyle.BackColor == bbgreen3)
                {
                    string userID = "0";
                    if (cbUsersMA.Visible)
                        userID = cbUsersMA.SelectedValue.ToString();

                    string reg = "0";
                    if (globalData.Region == String.Empty)
                    {
                        if (cbRegMA.Visible)
                            reg = cbRegMA.SelectedValue.ToString();
                    }
                    else
                        reg = globalData.Region;

                    string lpu = "-1";
                    if (cbLPUMA.Visible && cbLPUMA.SelectedItem != null)
                        lpu = cbLPUMA.SelectedValue.ToString();

                    dt1 = sql1.GetRecords("exec fillMASBA_All @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10", 
                        globalData.Div, globalData.RD, userID, reg, lpu, cbYearMA.SelectedItem.ToString(),
                        _dgv8.Rows[e.Cell.RowIndex].Cells["ma_fact1"].Value.ToString().Replace(",", "."), _dgv8.Rows[e.Cell.RowIndex].Cells["ma_fact2"].Value.ToString().Replace(",", "."),
                        _dgv8.Rows[e.Cell.RowIndex].Cells["ma_fact3"].Value.ToString().Replace(",", "."), _dgv8.Rows[e.Cell.RowIndex].Cells["ma_fact4"].Value.ToString().Replace(",", "."));
                }
                else
                    dt1 = sql1.GetRecords("exec fillMASBA @p1, @p2", _dgv8.Rows[e.Cell.RowIndex].Cells["ma_id"].Value.ToString(), _dgv8.Rows[e.Cell.RowIndex].Cells["db_id"].Value.ToString());

                if (_dgvUpd.Rows != null)
                {
                    _dgvUpd.Rows.Clear();
                }

                foreach (DataRow row in dt1.Rows)
                {
                    _dgvUpd.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4]);
                }


            }
        }

        private void обновитьКолвоВОтчётахДистрибьюторовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdRepDist urd = new UpdRepDist();

            urd.ShowDialog();
        }

        private void загрузитьАссИркутскToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportAcc2015();
        }


        private void ImportAcc2015()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Hospital Care\Kasyanova Tatyana\";
            openFileDialog1.Filter = "Excel files (*.xls)|*.xls|(*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Введите номер строки", "Номер строки:", true);
                ind.ShowDialog();
                if (globalData.input == "0")
                {
                    MessageBox.Show("Загрузка отчёта отменена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlSh);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                    return;
                }
                int i = Convert.ToInt32(globalData.input);  //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                String header = String.Empty;

                try
                {
                    Sql sql1 = new Sql();
                    while (xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2 != null)
                    {
                        header = "A";
                        String s1 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//nom
                        header = "B";
                        int s2 = Convert.ToInt32(xlSh.get_Range("B" + i.ToString(), "B" + i.ToString()).Value2.ToString());//шт
                        header = "C";
                        double s3 = Convert.ToDouble(xlSh.get_Range("C" + i.ToString(), "C" + i.ToString()).Value2.ToString());//евро
                        header = "D";
                        String s4 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString();//РП
                        header = "E";
                        String s5 = xlSh.get_Range("E" + i.ToString(), "E" + i.ToString()).Value2.ToString();//ЛПУ
                        

                        DataTable dt1 = sql1.GetRecords("exec InsAcc2015 @p1, @p2, @p3, @p4, @p5", s1, s2, s3, s4, s5);
                        if (dt1 == null)
                        {
                            xlSh.Cells[i, 6] = "Ошибка";
                            err++;
                        }
                        i++;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }

        private void любойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Acc2015 dr = new Acc2015();
            dr.ShowDialog();

            if (globalData.colReg == "" || globalData.colReg == null)
                return;

            loadRepDist(globalData.filePass, globalData.rowBegin, globalData.folderName, globalData.colReg, globalData.colMatCode, globalData.colMatName, globalData.colCount, globalData.colLPU, globalData.colLast);
        }

        /************************************************************************************************************/
        /* маркетинг 2 для менеджеров пордкции */
        /* ПМ */
        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadRegPM();
        }

        /* Скрыть ПМ */
        private void button62_Click(object sender, EventArgs e)
        {
            if (button62.Text == "Скрыть ПМ")
            {
                comboBox12.Visible = false;
                button62.Text = "Показать ПМ";
            }
            else
            {
                comboBox12.Visible = true;
                button62.Text = "Скрыть ПМ";
            }
        }

        /* Регион */
        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /* Скрыть регионы */
        private void button63_Click(object sender, EventArgs e)
        {
            if (button63.Text == "Скрыть регионы")
            {
                comboBox10.Visible = false;
                button63.Text = "Показать регионы";
            }
            else
            {
                comboBox10.Visible = true;
                button63.Text = "Скрыть регионы";
            }
        }

        private void button61_Click(object sender, EventArgs e)
        {
            int flag = 0;
            _dgvMA2.Columns["conf_name"].Visible = true;

            if (_dgvMA2.Columns["ma_name"].Visible == false)
            {
                _dgvMA2.Columns["ma_name"].Visible = true;
                flag = 1;
            }

            ExportInExcel(_dgvMA2);

            _dgvMA2.Columns["conf_name"].Visible = false;
            if (flag == 1)
                _dgvMA2.Columns["ma_name"].Visible = false;
        }

        private void button57_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            if (globalData.dtSBA == null)
                globalData.dtSBA = sql1.GetRecords("exec fillSBA");

            AllocCosts ac;

            ac = new AllocCosts();
            ac.ShowDialog();

            SelMAWithSBA2(_dgvMA2);
        }


        private void loadAccessPM()
        {
            if (globalData.UserAccess == 3)
                button62.Visible = false;
            else
                button62.Visible = true;
        }

        private void loadPM()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            if (globalData.UserAccess == 3)
                dt1 = sql1.GetRecords("exec selUserbyID @p1", globalData.UserID);
            else
                dt1 = sql1.GetRecords("exec SelUsersByRole @p1", 3);

            fillComboBox(dt1, comboBox12, "user_name", "user_id");
        }


        private void loadRegPM()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec SelRegPM @p1", comboBox12.SelectedValue);

            fillComboBox(dt1, comboBox10, "reg_nameRus", "reg_id");

        }

        private void SelMAPM()
        {
            Cursor = Cursors.WaitCursor;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            _dgvMA2.DataSource = null;

            string userID = "0";
            if (comboBox12.Visible)
                userID = comboBox12.SelectedValue.ToString();

            string reg = "0";
            if (comboBox10.Visible)
                reg = comboBox10.SelectedValue.ToString();


            string lpu = "-1";
            if (cbLPUMA.Visible && cbLPUMA.SelectedItem != null)
                lpu = cbLPUMA.SelectedValue.ToString();

            dt1 = sql1.GetRecords("exec SelMAPM @p1, @p2, @p3, @p4, @p5", "HC", reg, userID, comboBox9.SelectedItem, cbTypeMAPM.SelectedValue);

            _dgvMA2.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось загрузить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            formatMarkAct(_dgvMA2);
            Cursor = Cursors.Default;
        }

        private void button60_Click(object sender, EventArgs e)
        {
            SelMAPM();
        }

        private void button65_Click(object sender, EventArgs e)
        {
            editMarkAct2(_dgvMA2.SelectedCells[0].RowIndex);
        }

        private void editMarkAct2(int RowIndex)
        {
            try
            {
                if (_dgvMA2.Rows[_dgvMA2.SelectedCells[0].RowIndex].Cells["ma_id"].Value.ToString() == "0")
                {
                    MessageBox.Show("Для редактирования выделите маркетинговое мероприятие.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Sql sql1 = new Sql();
                DataTable dt1 = sql1.GetRecords("exec SelMarkActByID @p1, @p2", Convert.ToInt32(_dgvMA2.Rows[RowIndex].Cells["ma_id"].Value), Convert.ToInt32(_dgvMA2.Rows[RowIndex].Cells["db_id"].Value));

                globalData.UserID2 = Convert.ToInt32(dt1.Rows[0].ItemArray[2].ToString());

                AddEditMarkAct aema = new AddEditMarkAct(Convert.ToInt32(_dgvMA2.Rows[RowIndex].Cells["ma_id"].Value), Convert.ToInt32(_dgvMA2.Rows[RowIndex].Cells["db_id"].Value), 2);
                aema.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Не удалось войти в режим редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delMarkAct2()
        {
            if (_dgvMA2.Rows[_dgvMA2.SelectedCells[0].RowIndex].Cells["ma_id"].Value.ToString() == "0")
            {
                MessageBox.Show("Для удаления выделите маркетинговое мероприятие.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult dr = MessageBox.Show("Удалить выделенную строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                Sql sql1 = new Sql();
                sql1.GetRecords("exec DelMarkAct @p1, @p2", _dgvMA2.Rows[_dgvMA2.SelectedCells[0].RowIndex].Cells["ma_id"].Value.ToString(), _dgvMA2.Rows[_dgvMA2.SelectedCells[0].RowIndex].Cells["db_id"].Value.ToString());
                MessageBox.Show("Маркетинговое мероприятие удалено.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SelMAPM();
            }
        }

        private void button64_Click(object sender, EventArgs e)
        {
            delMarkAct2();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            if (button58.Text == "Показать доли")
            {
                splitContainer4.Panel1Collapsed = false;
                button58.Text = "Скрыть доли";
            }
            else
            {
                splitContainer4.Panel1Collapsed = true;
                button58.Text = "Показать доли";
            }
        }

        private void _dgvMA2_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void _dgvMA2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            editMarkAct2(e.RowIndex);
        }

        private void _dgvMA2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            point.X = e.ColumnIndex;
            point.Y = e.RowIndex;
        }

        private void _dgvMA2_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (_dgvMA2.Rows.Count > 3 && e.Cell.Selected == true)
            {
                Sql sql1 = new Sql();
                DataTable dt1 = new DataTable();
                dt1 = sql1.GetRecords("exec fillMASBA @p1, @p2", _dgvMA2.Rows[e.Cell.RowIndex].Cells["ma_id"].Value.ToString(), _dgvMA2.Rows[e.Cell.RowIndex].Cells["db_id"].Value.ToString());

                if (_dgvSBA2.Rows != null)
                {
                    _dgvSBA2.Rows.Clear();
                }

                foreach (DataRow row in dt1.Rows)
                {
                    _dgvSBA2.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4]);
                }


            }
        }

        private void _dgvMA2_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgvMA2);
        }

        private void SelMAWithSBA2(DataGridView _tempdgv)
        {
            Cursor = Cursors.WaitCursor;

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            _tempdgv.DataSource = null;


            string userID = "0";
            if (comboBox12.Visible)
                userID = comboBox12.SelectedValue.ToString();

            string reg = "0";
            if (comboBox10.Visible)
                reg = comboBox10.SelectedValue.ToString();



            string lpu = "-3";

            int[] sba = new int[30];

            for (int i = 0; i < 30; i++)
                sba[i] = Convert.ToInt32(globalData.dtSBA.Rows[i][1]) == 1 ? Convert.ToInt32(globalData.dtSBA.Rows[i][0]) : 0;

            dt1 = sql1.GetRecords("exec SelMALPUWithSBA @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20,@p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36",
               "HC", reg, userID, cbYearMA.SelectedItem, globalData.RD, lpu,
               sba[0], sba[1], sba[2], sba[3], sba[4], sba[5], sba[6], sba[7], sba[8], sba[9],
               sba[10], sba[11], sba[12], sba[13], sba[14], sba[15], sba[16], sba[17], sba[18], sba[19],
               sba[20], sba[21], sba[22], sba[23], sba[24], sba[25], sba[26], sba[27], sba[28], sba[29]);

            _tempdgv.DataSource = dt1;

            if (dt1 == null)
            {
                MessageBox.Show("Не удалось загрузить данные.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            formatMarkAct(_tempdgv);
            Cursor = Cursors.Default;
        }

        private void button66_Click(object sender, EventArgs e)
        {
            if (comboBox12.Visible == true)
                addMarkAct();
            else
                MessageBox.Show("Для заполнения нужно выбрать человека!", "Ошибка!");

        }

        private void _dgvSBA2_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgvSBA2);
        }
        /***************************/
        /* Асс в рублях */
        private void rbRub_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(rbRub, rbEuro);
            visibleAcc(_dgv4, rbRub, rbEuro);
        }

        private void rbEuro_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(rbEuro, rbRub);
            visibleAcc(_dgv4, rbRub, rbEuro);
        }

        private void CheckCheck( RadioButton rb1, RadioButton rb2 )
        {
            if (rb1.Checked == true)
                rb2.Checked = false;
            
            else
                rb2.Checked = true;
        }

        private void visibleAcc(DataGridView dgv, RadioButton rbR, RadioButton rbE)
        {
            bool flagR, flagE;

            flagR = rbR.Checked;
            flagE = rbE.Checked;

            if (globalData.Div != "HC")
            {
                flagR = false;
                flagE = true;
            }

            if (dgv.Columns.Count == 0)
                return;

            dgv.Columns["dilcost"].Visible = flagE;
            dgv.Columns["dilcostRub"].Visible = flagR;
         
            dgv.Columns["cyfactEuro"].Visible = flagE;
            dgv.Columns["cyfactRub"].Visible = flagR;

            dgv.Columns["cyplanEuro"].Visible = flagE;
            dgv.Columns["cyplanRub"].Visible = flagR;

            dgv.Columns["prEuro"].Visible = flagE;
            dgv.Columns["prRub"].Visible = flagR;

            dgv.Columns["prRub"].DisplayIndex = 18;

            dgv.Columns["JanFact"].Visible = flagE;
            dgv.Columns["FebFact"].Visible = flagE;
            dgv.Columns["MartFact"].Visible = flagE;
            dgv.Columns["AprFact"].Visible = flagE;
            dgv.Columns["MayFact"].Visible = flagE;
            dgv.Columns["JuneFact"].Visible = flagE;
            dgv.Columns["JuleFact"].Visible = flagE;
            dgv.Columns["AugFact"].Visible = flagE;
            dgv.Columns["SepFact"].Visible = flagE;
            dgv.Columns["OktFact"].Visible = flagE;
            dgv.Columns["NovFact"].Visible = flagE;
            dgv.Columns["DecFact"].Visible = flagE;

            dgv.Columns["JanFactR"].Visible = flagR;
            dgv.Columns["FebFactR"].Visible = flagR;
            dgv.Columns["MartFactR"].Visible = flagR;
            dgv.Columns["AprFactR"].Visible = flagR;
            dgv.Columns["MayFactR"].Visible = flagR;
            dgv.Columns["JuneFactR"].Visible = flagR;
            dgv.Columns["JuleFactR"].Visible = flagR;
            dgv.Columns["AugFactR"].Visible = flagR;
            dgv.Columns["SepFactR"].Visible = flagR;
            dgv.Columns["OktFactR"].Visible = flagR;
            dgv.Columns["NovFactR"].Visible = flagR;
            dgv.Columns["DecFactR"].Visible = flagR;

        }

        private void visiblePrivSales(DataGridView dgv, RadioButton rbR, RadioButton rbE)
        {
            bool flagR, flagE;

            flagR = rbR.Checked;
            flagE = rbE.Checked;

            if (dgv.Columns.Count == 0)
                return;
            
            dgv.Columns["cm1"].Visible = flagE;
            dgv.Columns["cm2"].Visible = flagE;
            dgv.Columns["cm3"].Visible = flagE;
            dgv.Columns["cm4"].Visible = flagE;
            dgv.Columns["cm5"].Visible = flagE;
            dgv.Columns["cm6"].Visible = flagE;
            dgv.Columns["cm7"].Visible = flagE;
            dgv.Columns["cm8"].Visible = flagE;
            dgv.Columns["cm9"].Visible = flagE;
            dgv.Columns["cm10"].Visible =flagE;
            dgv.Columns["cm11"].Visible =flagE;
            dgv.Columns["cm12"].Visible = flagE;
            dgv.Columns["total"].Visible = flagE;

            dgv.Columns["cm1R"].Visible = flagR;
            dgv.Columns["cm2R"].Visible = flagR;
            dgv.Columns["cm3R"].Visible = flagR;
            dgv.Columns["cm4R"].Visible = flagR;
            dgv.Columns["cm5R"].Visible = flagR;
            dgv.Columns["cm6R"].Visible = flagR;
            dgv.Columns["cm7R"].Visible = flagR;
            dgv.Columns["cm8R"].Visible = flagR;
            dgv.Columns["cm9R"].Visible = flagR;
            dgv.Columns["cm10R"].Visible =flagR;
            dgv.Columns["cm11R"].Visible =flagR;
            dgv.Columns["cm12R"].Visible = flagR;
            dgv.Columns["totalR"].Visible = flagR;

        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(radioButton11, radioButton10);
            visibleAcc(_dgv7,radioButton11, radioButton10);
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(radioButton10, radioButton11);
            visibleAcc(_dgv7, radioButton11, radioButton10);
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(radioButton12, radioButton13);
            visiblePrivSales(_dgv6, radioButton12, radioButton13);
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(radioButton13, radioButton12);
            visiblePrivSales(_dgv6, radioButton12, radioButton13);
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(radioButton15, radioButton14);
            visiblePrivSales(_dgv5, radioButton15, radioButton14);
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            CheckCheck(radioButton14, radioButton15);
            visiblePrivSales(_dgv5, radioButton15, radioButton14);
        }

        private void _dgvRent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.ColumnIndex == 21) || (e.ColumnIndex == 23))
                {
                    if (_dgvRent.Rows[e.RowIndex].Cells["rentval"].Value.ToString() == "%")
                        _dgvRent.Rows[e.RowIndex].Cells["sum"].Value = Convert.ToInt32(_dgvRent.Rows[e.RowIndex].Cells["cost3"].Value) * Convert.ToInt32(_dgvRent.Rows[e.RowIndex].Cells["rent"].Value) / 100;
                    else
                        _dgvRent.Rows[e.RowIndex].Cells["sum"].Value = _dgvRent.Rows[e.RowIndex].Cells["rent"].Value;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void взаимозаменяемостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Merge m = new Merge();

            m.ShowDialog();
        }

        private void энимедToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20619942_Энимед");
        }

        private void энимедToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadRepDist("", 2, "20619942_Энимед", "B", "C", "D", "F", "B", "8");
        }

        private void нДАToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadMaterialRegion2("20440997_НДА");
        }

        private void любойToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InputDialog ind = new InputDialog("Дистрибьютор", "Введите дистрибьютора в формате 20440997_НДА", true);
            ind.ShowDialog();

            if (ind.DialogResult == DialogResult.Cancel)
                return;

            loadMaterialRegion2(globalData.input);
        }

        private void organizationRRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOrganizationList formOrganizationList = new FormOrganizationList();
            formOrganizationList.ShowDialog();
        }

        private void personSFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPersonList formPersonList = new FormPersonList();
            formPersonList.ShowDialog();
        }

        private void userRoleSFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUserRoleSFList formUserRoleSFList = new FormUserRoleSFList();
            formUserRoleSFList.ShowDialog();
        }

        private void прометейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderName = "20438696_Прометей";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = @"J:\Information Technology\Development\отчётность региональщиков\Доработки\загрузка данных от дистрибюторов\" + folderName;
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Cursor = Cursors.WaitCursor;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlSh;
                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);

                xlSh = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                object misValue = System.Reflection.Missing.Value;
                int err = 0;

                InputDialog ind = new InputDialog("Год", "Введите год", true);
                ind.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                String year = globalData.input;

                InputDialog id = new InputDialog("Введите месяц", "В формате 1, 2, 3...", true);
                id.ShowDialog();

                if (ind.DialogResult == DialogResult.Cancel)
                    return;

                string month = globalData.input;

                string s1 = year + "-" + month + "-01";

                String header = String.Empty;

                string s2 = folderName.Split('_')[0];

                int i = 11;

                try
                {
                    Sql sql1 = new Sql();

                    header = "A";
                    while (xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2 != null)
                    {
                        header = "A";
                        string s6 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString();//LPU

                        string s3 = s6;

                        i++;
                        while ((bool)(xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Font.Bold) == false)
                        {
                            if (xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString() == " ")
                                break;
                            header = "A";
                            string s4 = xlSh.get_Range("A" + i.ToString(), "A" + i.ToString()).Value2.ToString().Replace("\"", "-");//matName
                            header = "D";
                            string s5 = xlSh.get_Range("D" + i.ToString(), "D" + i.ToString()).Value2.ToString().Replace(',', '.');//count

                            string res = sql1.GetRecordsOne("exec InsRepDist 0, @p1, @p2, @p3, @p4, @p5, @p6", s1, s2, s3, s4, s5, s6);
                            if (res != "1")
                            {
                                xlSh.Cells[i, 5] = res;
                                err++;
                            }
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при чтении файла. Системная ошибка: " + ex.Message + " В ячейке " + header + i.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    err++;
                }
                finally
                {
                    if (err == 0)
                    {
                        MessageBox.Show("Загрузка завершена. Ошибок не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlSh);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                    }
                    else
                    {
                        MessageBox.Show("Загрузка завершена. Найдено " + err.ToString() + " ошибок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        xlApp.Visible = true;
                    }
                }
                Cursor = Cursors.Default;
            }
        }


        private void _dgvUpd_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgvUpd);
        }

        private void button71_Click(object sender, EventArgs e)
        {
            try
            {
                Sql sql1 = new Sql();

                load = false;

                if (_dgv14.Rows[0].Cells["nom_id"].Value.ToString() != "0")
                {
                    foreach (DataGridViewRow row in _dgv14.Rows)
                    {
                        if (row.Cells["upd"].Value != null)
                        {
                            if (row.Cells["cyDilCost"].Value.ToString() == "")
                                continue;
                            sql1.GetRecords("exec fillAccPlanDilCostRub @p1, @p2, @p3", row.Cells["nom_id"].Value, row.Cells["cyDilCost"].Value, globalData.UserID);
                            load = true;
                        }
                    }
                }
                MessageBox.Show("Информация по дилерским ценам сохранена.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (load)
                    loadAccPlanByLPUNY(_dgv14, cbLPUAccNY);

                load = true;
            }
            catch (Exception err)
            {
                MessageBox.Show("Не удалось сохранить информацию по дилерским ценам, системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFillPlan_Click(object sender, EventArgs e)
        {
            FillComparePlan();
            SelComparePlan();
        }

        void FillComparePlan()
        {
            Sql sql1 = new Sql();

            foreach (DataGridViewCell cell in dgvCheckPlan.SelectedCells)
            {
                String delta = dgvCheckPlan.Rows[cell.RowIndex].Cells[9].Value.ToString();
                if (delta == "0")
                {
                    delta = dgvCheckPlan.Rows[cell.RowIndex].Cells[10].Value.ToString();
                    if (delta == "0")
                        delta = dgvCheckPlan.Rows[cell.RowIndex].Cells[11].Value.ToString();
                }

                if (dgvCheckPlan.Rows[cell.RowIndex].Cells[1].Value.ToString() != String.Empty && delta != "0")
                {
                    sql1.GetRecords("exec TransfAccUserLPUOnLPU @p1, @p2", dgvCheckPlan.Rows[cell.RowIndex].Cells[1].Value.ToString(), cbYearPlan.SelectedItem);
                }
            }
        }

        private void btnClearPlan_Click(object sender, EventArgs e)
        {
            SelComparePlan();
        }

        void formatComparePlan()
        {
            dgvCheckPlan.Columns["sumplEurUser"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["sumplRubUser"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["sumplRub"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["sumplEur"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["sumplUser"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["sumpl"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["delta"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["deltaEur"].DefaultCellStyle.Format = "N3";
            dgvCheckPlan.Columns["deltaRub"].DefaultCellStyle.Format = "N3";
            
            dgvCheckPlan.Columns["lpuID"].Visible = false;
            
            dgvCheckPlan.Columns["regName"].HeaderText = "Регион";
            dgvCheckPlan.Columns["lpuSName"].HeaderText = "ЛПУ";
            dgvCheckPlan.Columns["sumplUser"].HeaderText = "Сумма шт. на UserLPU";
            dgvCheckPlan.Columns["sumplEurUser"].HeaderText = "Сумма евро на UserLPU";
            dgvCheckPlan.Columns["sumplRubUser"].HeaderText = "Сумма руб. на UserLPU";
            dgvCheckPlan.Columns["sumplRub"].HeaderText = "Сумма руб. на ЛПУ";
            dgvCheckPlan.Columns["sumplEur"].HeaderText = "Сумма евро на ЛПУ";
            dgvCheckPlan.Columns["sumpl"].HeaderText = "Сумма шт.";
            dgvCheckPlan.Columns["delta"].HeaderText = "Разница шт.";
            dgvCheckPlan.Columns["deltaEur"].HeaderText = "Разница евро";
            dgvCheckPlan.Columns["deltaRub"].HeaderText = "Разница руб.";

            dgvCheckPlan.Columns["sumplEurUser"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["sumplRubUser"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["sumplRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["sumplEur"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["sumplUser"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["sumpl"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["delta"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["deltaEur"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCheckPlan.Columns["deltaRub"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            
        }

        void SelComparePlan()
        {
            Sql sql1 = new Sql();
            DataTable dt = sql1.GetRecords("exec CompareAccPlan @p1", cbYearPlan.SelectedItem);

            if (dt != null)
                dgvCheckPlan.DataSource = dt;

            formatComparePlan();
        }
        
        String DelComparePlan()
        {
            Sql sql1 = new Sql();
            String res = "Не удалось удалить план! Проверьте, что Вы выделили целиком строку!";

            foreach (DataGridViewRow row in dgvCheckPlan.SelectedRows)
            {
                res = sql1.GetRecordsOne("exec DelCompareAccPlan @p1, @p2", cbYearPlan.SelectedItem, row.Cells["lpuID"].Value.ToString());               
            }

            return res;
        }

        private void button72_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Вы уверены, что хотите удалить план с ЛПУ?\n Для удаления выделите ВСЮ СТРОКУ!!!", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                DelComparePlan();
            }

        }

        private void cbTypeMA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dgv8.DataSource != null)
                SelMA();
        }

        private void _dgv8_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = _dgv8;
                switch (tabControl1.SelectedIndex)
                {
                    case 7:
                        {
                            dgv = _dgv8;
                            break;
                        }
                    case 28:
                        {
                            dgv = _dgvMA2;
                            break;
                        }
                }


                if (dgv.Columns["ma_name"].Visible == true)
                    dgv.Columns["ma_name"].Visible = false;
                else
                    dgv.Columns["ma_name"].Visible = true;
            }
            catch
            {
            }
        }

        private void cbTypeMA_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (_dgv8.DataSource != null)
                SelMA();
        }

        void loadMAType(ComboBox cb)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec MarkAct_Sel_Type @p1", 1);
            fillComboBox(dt1, cb, "matype_name", "matype_id");
        }

        private void cbTypeMAPM_Click(object sender, EventArgs e)
        {
            if (_dgvMA2.DataSource != null)
                SelMAPM();
        }

        private void cbTypeMAPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dgvMA2.DataSource != null)
                SelMAPM();
        }

    }
}