using DataLayer;
using RegionR.Directories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;


namespace RegionR
{
    public partial class RR : Form
    {
        #region Загрузка
        /* загрузка вкладки Заполнение и отчёт для РД */
        protected void loadRent( bool flagRD )
        {
            globalData.update = true;
  
            globalData.role = CheckRole();
            globalData.tabFlagRD = flagRD;
                       
            loadData(globalData.role, globalData.tabFlagRD);

            if (flagRD == false)
                CheckDate();

            globalData.update = false;
         
        }

        /* Загрузка вкладок Отчёт для ДМ и Сводный Отчёт */
        protected void loadSvodRent(bool _flSvod)
        {
            globalData.update = true;

            globalData.role = CheckRole();
            globalData.tabFlagSvod = _flSvod;

            loadDataDM(globalData.role, globalData.tabFlagSvod);

            globalData.update = false;

        }

        /* Загрузка вкладок Отчёт для контроллинга и Сторно */
        protected void loadStorno( bool _flSt )
        {
            globalData.update = true;

            globalData.role = CheckRole();

            loadDataSt(globalData.role, _flSt);

            globalData.update = false;

        }

        /* Установка роли для рент. */
        private int CheckRole()
        {
            // админы и ДМ
            if ((globalData.UserAccess == 1) || (globalData.UserID == 6) || (globalData.UserID == 78) || (globalData.UserID == 7))
                return 1;
            else if (globalData.UserAccess == 2)
                return 6;
            else
                return globalData.UserAccess;
        }

        /* Установка активности комбобоксов */
        private void CheckEnabl()
        {
            if (checkBox5.Checked == true)
                rd_rdRent.Enabled = false;
            else
                rd_rdRent.Enabled = true;

            if (checkBox6.Checked == true)
                rp_rdRent.Enabled = false;
            else
                rp_rdRent.Enabled = true;
        }

        /* Установка даты отчета */
        private void CheckDate()
        {
            Sql sql1 = new Sql();

            if (User2_rent.SelectedValue != null)
                dtpRent.Value = Convert.ToDateTime(sql1.GetRecordsOne("exec Rent_Select_UrtDate @p1", User2_rent.SelectedValue));
        }

        /* Области видимости в зависимости от роли */
        private void loadData( int role, bool flagRD = false )
        {
            /* Настройки по умолчанию */
            /* Панель свернута*/
            splitContainer3.Panel1Collapsed = true;
            /* Кнопка для доступа к правам невидима */
            button50.Text = "Показать параметры";
            button50.Visible = false;
            /* Все галочки неактивны и недоступны */
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;

            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;

            /* Кнопки применить и сохранить - зелёненькие */
            button54.BackColor = bbgreen3;
            button48.BackColor = bbgreen3;

            /* Все фильтры - активны */
            Region_rent.Enabled = true;
            RD_rent.Enabled = true;
            User_rent.Enabled = true;           

            /* Даты - доступны */
            label64.Visible = true;
            label48.Visible = true;
            label49.Visible = true;
            dateTimePicker21.Visible = true;
            dateTimePicker22.Visible = true;
            
            /* На кого заполняют отчёт - доступен, но менять нельзя */
            User2_rent.Enabled = false;
            User2_rent.Visible = true;

            /* Месяц отчёта - только для чтения */
            dtpRent.Enabled = false;
            /* Кнопка по умолчанию называется сохранить */
            button48.Text = "Сохранить";
            /* Кнопка по умолчанию называется сохранить */
            button54.Text = "Применить";

            /* Материал по умолчанию всегда виден */
            label62.Visible = true;
            Mat_rent.Visible = true;

            switch (role)
            {                
                /* админы + ДМ, всё видно, все функции возможны */
                case 1:
                    /*  все рд 
                     *  все регионы 
                     *  все пользователи 
                     *  все дивизионы 
                     *  заполнение на добавленных пользователей 
                     */
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = true;

                    if (flagRD == false)
                    {
                        button50.Visible = true;
                        User2_rent.Enabled = true;
                        
                    }
                    else
                    {
                        label48.Visible = false;
                        label49.Visible = false;
                        label64.Visible = false;
                        dateTimePicker21.Visible = false;
                        dateTimePicker22.Visible = false;

                        User2_rent.Visible = false;
                        dtpRent.Enabled = true;
                        button48.Text = "Применить";
                        button54.Text = "Подтвердить";

                        label62.Visible  = false;
                        Mat_rent.Visible = false;

                        button50.Text = "Сохранить коммент.";
                        button50.Visible = true;
                    }
                    break;

                /* рег. представители, видят только себя, доступны все функции */
                case 5:
                    /* только свой рд */
                    /* регион */     
                    /* только сам */
                    /* дивизион свой */
                    /* заполнение на себя */
                    
                    break;
                    
                /* рег. менеджеры видят всех рп, могут заполнять не на себя, доступны все функции */
                case 6:
                    /* только свой рд */
                    /* регионы свои */
                    /* рп регионов */
                    /* дивизион свой */
                    /* заполнение на себя и рп */
                    User2_rent.Enabled = true;
                    break;
                                       
                /* рег. директора, видят всё, в пределах своих регионов, все функции возможны */
                case 4:
                    /* только свой рд */
                    /* регионы свои */
                    /* рп и рм свои */
                    /* дивизионы все */
                    /* заполнение на себя, рп, рд */
                    
                    if (globalData.UserID == 102)
                    {
                        checkBox2.Visible = false;
                        checkBox3.Visible = false;
                    }
                    else
                    {
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                    }

                    if (flagRD == false)
                    {
                        button50.Visible = true;
                        User2_rent.Enabled = true;
                    }
                    else
                    {
                        label48.Visible = false;
                        label49.Visible = false;
                        label64.Visible = false;
                        dateTimePicker21.Visible = false;
                        dateTimePicker22.Visible = false;

                        User2_rent.Visible = false;
                        dtpRent.Enabled = true;
                        button48.Text = "Применить";
                        button54.Text = "Подтвердить";

                        label62.Visible = false;
                        Mat_rent.Visible = false;

                        button50.Text = "Сохранить коммент.";
                        button50.Visible = true;

                        
                    }
                    break;
                /* бухгалтерия */
                case 13:
                    /*  все рд 
                     *  все регионы 
                     *  все пользователи 
                     *  все дивизионы 
                     *  заполнение на добавленных пользователей 
                     */
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    checkBox3.Visible = true;

                    if (flagRD == false)
                    {
                        button50.Visible = true;
                        User2_rent.Enabled = true;

                    }
                    else
                    {
                        label48.Visible = false;
                        label49.Visible = false;
                        label64.Visible = false;
                        dateTimePicker21.Visible = false;
                        dateTimePicker22.Visible = false;

                        User2_rent.Visible = false;
                        dtpRent.Enabled = true;
                        button48.Text = "Применить";
                        button54.Text = "Подтвердить";

                        label62.Visible = false;
                        Mat_rent.Visible = false;

                        button50.Text = "Сохранить коммент.";
                        button50.Visible = true;
                    }
                    break;
                default:
                    return;
            }

            if (flagRD == true && globalData.UserID == 102)
                rent_fill("RD", -1, RD_rent);
            else
                rent_fill("RD", globalData.role, RD_rent);
            
            rent_fill("Region", globalData.role, Region_rent, null, null, null, RD_rent);
            if (globalData.role == 1)
            {
                rent_fill("Div", globalData.role, Div_rent, User_rent, null, Region_rent, RD_rent);
                rent_fill("User", globalData.role, User_rent, null, Div_rent, Region_rent, RD_rent);
            }
            else
            {
                rent_fill("User", globalData.role, User_rent, null, null, Region_rent, RD_rent);
                rent_fill("Div", globalData.role, Div_rent, User_rent, null, Region_rent, null);

            }
            rent_fill("UserRent", globalData.role, User2_rent, null, Div_rent, Region_rent, RD_rent);

            //rent_fill("RD", role, RD_rent);
            //rent_fill("Region", role, Region_rent, null, null, null,RD_rent);
            //rent_fill("User", role, User_rent, null, null, Region_rent, RD_rent);
            //rent_fill("Div", role, Div_rent, User_rent, null, Region_rent, RD_rent);
            //rent_fill("UserRent", role, User2_rent, null, Div_rent, Region_rent, RD_rent);

         
        }

        /* Область видимости в зависимости от вкладки */
        private void loadDataDM( int role, bool flSvod = false )
        {
            CheckEnabl();

            label51.Visible = true;
            div_rdRent.Visible = true;
            /* Есть в отчёте для РД */
            button59.Visible = false;
            /* Выгрузка в Excel */
            button53.Visible = true;
            /* Кнопка Применить - зелёненькая */
            button55.BackColor = bbgreen3;
            /* Кнопка Сохранить  */
            button56.Visible = true;
            /* Кнопка Сохранить  */
            button69.Visible = false;


            rent_fill("RD", globalData.role, rd_rdRent);
            
            switch (flSvod)
            {
                    /* Вкладка Сводный отчёт */
                case true:
                    label51.Visible = false;
                    div_rdRent.Visible = false;
                    /* Подтверждать может только Маша */
                    if (role != 13 && globalData.UserID != 258)
                        button56.Visible = false;
                    
                    button53.Visible = false;
                    rent_fill("UserForKontrol", globalData.role, rp_rdRent, null, null, null, rd_rdRent);
                    
                    SelectSvod();
                    break;

                    /* Вкладка Отчёт для ДМ */
                case false:
                    if (role == 13 || globalData.UserID == 258)
                        button69.Visible = true;

                    rent_fill("Div", globalData.role, div_rdRent, null, null, null, rd_rdRent);
                    rent_fill("UserForDM", globalData.role, rp_rdRent, null, div_rdRent, null, rd_rdRent);

                    SelectRepRent();
                    break;

                default:
                    return;
            }
        }

        /* Сторно или контроллинг*/
        private void loadDataSt(int role, bool flSt = false)
        {           
            label54.Visible = true;
            dtpSt.Visible = true;
            
            /* Обновить сторно */
            button67.Visible = true;
            /* Применить */
            button68.Visible = true;
            /* Выгрузка в Excel */
            button49.Visible = true;

            /* Распределено */
            button70.Visible = false;
            
            /* Кнопка Применить - зелёненькая */
            button68.BackColor = bbgreen3;
            button70.BackColor = bbgreen3;
            
            switch (flSt)
            {
                /* Вкладка Сторно */
                case true:
                    SelectStorno();
                    break;

                /* Вкладка Отчёт для контроллинга */
                case false:
                    label54.Visible = false;
                    dtpSt.Visible = false;

                    button67.Visible = false;
                    button68.Visible = false;

                    button70.Visible = true;

                    SelectKontrol();
                    break;

                default:
                    return;
            }
        }

        /* Загрузка панели доступа к отчёту */
        private void loadUserAccess()
        {
            if (_dgvUsRent.Rows.Count != 0)
                _dgvUsRent.Rows.Clear();

            string rd = RD_rent.SelectedValue.ToString();

            if (checkBox1.Checked == true)
                rd = "0";

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec  Rent_Select_UserAccess @p1", rd);

            if (globalData.UserID == 258 || globalData.UserID == 1)
                _dgvUsRent.Columns["user_id"].Visible = true;
            else
                _dgvUsRent.Columns["user_id"].Visible = false;

            int i = 0;
            foreach (DataRow row in dt1.Rows)
            {
                _dgvUsRent.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3]);

                if (Convert.ToInt32(row.ItemArray[4]) == 0)
                    _dgvUsRent.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                i++;
            }

        }

        /* ХП */
        public void rent_fill( string name, int role, ComboBox cb, ComboBox cbUser = null, 
                                                                   ComboBox cbDiv = null, 
                                                                   ComboBox cbReg = null, 
                                                                   ComboBox cbRD = null )
        {

            Sql sql1 = new Sql();
            DataTable dt = new DataTable();
            string reg = "0", rd = "0", div = "0", user = globalData.UserID.ToString();
            string _name = "user_name", _id = "user_id";

            if (cbDiv != null)
            {
                if (cbDiv.SelectedValue == null)
                    div = "0";
                else
                    div = cbDiv.SelectedValue.ToString();
            }

            if (cbReg != null)
            {
                if (cbReg.Enabled == false || cbReg.Visible == false || cbReg.SelectedValue == null)
                    reg = "0";
                else
                    reg = cbReg.SelectedValue.ToString();
            }

            if (cbRD != null)
            {
                if (cbRD.Enabled == false || cbRD.Visible == false)
                    rd = "0";
                else
                {
                    if (cbRD.SelectedValue == null)
                        rd = "0";
                    else
                        rd = cbRD.SelectedValue.ToString();
                }
            }  
            if (name == "Region" && role == 4)
                user = "0";

            if (name == "UserForDM")
                dt = sql1.GetRecords("exec Rent_Select_" + name + " @p1, @p2, @p3, @p4, @p5, @p6", role, user, div, rd, reg, dtp23.Value.Year.ToString() + "-" + dtp23.Value.Month + "-01");
            else
                dt = sql1.GetRecords("exec Rent_Select_" + name + " @p1, @p2, @p3, @p4, @p5", role, user, div, rd, reg);

            if (name == "Div")
            {
                _name = "sdiv_code";
                _id = "sdiv_id";
            }
            
            if (name == "Region")
            {
                _name = "reg_nameRus";
                _id = "reg_id";
            }
            fillComboBox(dt, cb, _name, _id);
    
        }
        #endregion   
      
        #region Заголовки отчётов
        /* Заголовки отчёта Заполнение */
        private void fillTableHeaderRent(DataGridView dgv, bool flagmat, bool flagsba, bool flaguser)
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
            dgv.Columns.Add("date2", "Дата отгрузки");
            dgv.Columns["date2"].DefaultCellStyle.Format = "MM - yyyy";
            dgv.Columns["date2"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["date2"].ReadOnly = true;
            dgv.Columns["date2"].Width = 60;
            dgv.Columns.Add("comp", "Компания");
            dgv.Columns["comp"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["comp"].ReadOnly = true;
            dgv.Columns.Add("reg", "Регион");
            dgv.Columns["reg"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["reg"].ReadOnly = true;
            dgv.Columns["reg"].Width = 150;
      
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
            dgv.Columns["sba_code"].Visible = flagsba;
            dgv.Columns.Add("sba_name", "SBA (наименование)");
            dgv.Columns["sba_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sba_name"].ReadOnly = true;
            dgv.Columns["sba_name"].Width = 140;
            dgv.Columns["sba_name"].Visible = flagsba;
    
            dgv.Columns.Add("mat_code", "Артикул товара");
            dgv.Columns["mat_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["mat_code"].ReadOnly = true;
            dgv.Columns["mat_code"].Width = 120;
            dgv.Columns["mat_code"].Visible = flagmat;
            dgv.Columns.Add("mat_name", "Наименование товара");
            dgv.Columns["mat_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["mat_name"].ReadOnly = true;
            dgv.Columns["mat_name"].Width = 150;
            dgv.Columns["mat_name"].Visible = flagmat;
                     
            dgv.Columns.Add("ps_count", "Факт. продажи (шт)");
            dgv.Columns["ps_count"].Width = 110;
            dgv.Columns["ps_count"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["ps_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns.Add("cost2", "База для расчёта (евро)");
            dgv.Columns["cost2"].ReadOnly = true;
            dgv.Columns["cost2"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cost2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cost2"].Width = 110;
            dgv.Columns["cost2"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns.Add("cost3", "База для расчёта (руб.)");
            dgv.Columns["cost3"].ReadOnly = true;
            dgv.Columns["cost3"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cost3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cost3"].Width = 120;
            dgv.Columns["cost3"].SortMode = DataGridViewColumnSortMode.Programmatic;

            dgv.Columns.Add("lpu_sname", "ЛПУ");
            dgv.Columns["lpu_sname"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["lpu_sname"].ReadOnly = true;
            dgv.Columns["lpu_sname"].SortMode = DataGridViewColumnSortMode.Programmatic;

            dgv.Columns.Add("user_name", "Пользователь");
            dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["user_name"].ReadOnly = true;
            dgv.Columns["user_name"].Visible = flaguser;
          
            dgv.Columns.Add("rent", "Величина");
            dgv.Columns["rent"].Visible = true;
            dgv.Columns["rent"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rent"].DefaultCellStyle.Format = "N2";
            dgv.Columns["rent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["rent"].ReadOnly = false;

            DataGridViewComboBoxColumn comboCol2 = new DataGridViewComboBoxColumn();
            comboCol2.Items.Add("%");
            comboCol2.Items.Add("сум");
            
            comboCol2.Name = "rentval";
            dgv.Columns.Insert(22, comboCol2);

            dgv.Columns[22].Width = 100;
            dgv.Columns[22].HeaderText = "Ед.измерения";
            dgv.Columns["rentval"].SortMode = DataGridViewColumnSortMode.Programmatic;

            dgv.Columns.Add("rent_lpu", "Прочие ЛПУ");
            dgv.Columns["rent_lpu"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rent_lpu"].ReadOnly = false;
            dgv.Columns["rent_lpu"].Visible = true;

            dgv.Columns.Add("upd", "Были ли изменения");
            dgv.Columns["upd"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["upd"].ReadOnly = true;
            dgv.Columns["upd"].Visible = false;

            dgv.Columns.Add("sum", "Сумма, руб.");
            dgv.Columns["sum"].Visible = true;
            dgv.Columns["sum"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sum"].DefaultCellStyle.Format = "N2";
            dgv.Columns["sum"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sum"].ReadOnly = true;

            dgv.Columns.Add("cm2", "CM2, %");
            dgv.Columns["cm2"].Visible = false;
            if ((globalData.role == 1 || globalData.role == 4 || globalData.role == 13) && globalData.UserID != 102)
                dgv.Columns["cm2"].Visible = true;
            dgv.Columns["cm2"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["cm2"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cm2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cm2"].ReadOnly = true;

        }
        /* Заголовки отчёта для РД + ДМ */
        private void fillTableHeaderReportRent( DataGridView dgv )
        {
            dgv.Columns.Clear();
            /*0*/
            dgv.Columns.Add("date", "Месяц отчёта");
            dgv.Columns["date"].DefaultCellStyle.Format = "MM - yyyy";
            dgv.Columns["date"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["date"].ReadOnly = true;
            dgv.Columns["date"].Width = 60;
            /*1*/
            dgv.Columns.Add("user_name", "Фамилия");
            dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["user_name"].ReadOnly = true;
            /*2*/
            dgv.Columns.Add("rep_date", "Месяц отгрузки");
            dgv.Columns["rep_date"].DefaultCellStyle.Format = "MM - yyyy";
            dgv.Columns["rep_date"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rep_date"].ReadOnly = true;
            dgv.Columns["rep_date"].Width = 60;
            /*3*/
            dgv.Columns.Add("refDoc", "Billing Document");
            dgv.Columns["refDoc"].ReadOnly = true;
            dgv.Columns["refDoc"].Visible = true;
            dgv.Columns["refDoc"].SortMode = DataGridViewColumnSortMode.Programmatic;
            /*4*/
            dgv.Columns.Add("lpu_sname", "Конечный контрагент");
            dgv.Columns["lpu_sname"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["lpu_sname"].ReadOnly = true;
            /*5*/
            dgv.Columns.Add("cust_code", "Покупатель");
            dgv.Columns["cust_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["cust_code"].ReadOnly = true;
            dgv.Columns["cust_code"].Visible = false;
            /*6*/
            dgv.Columns.Add("cust_name", "Контрагент, на кот. отгружался товар");
            dgv.Columns["cust_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["cust_name"].ReadOnly = true;
            dgv.Columns["cust_name"].Width = 200;
            dgv.Columns["cust_name"].Visible = true;
            /*7*/
            dgv.Columns.Add("sba_code", "SBA");
            dgv.Columns["sba_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sba_code"].ReadOnly = true;
            dgv.Columns["sba_code"].Width = 40;
            dgv.Columns["sba_code"].Visible = false;
            /*8*/      
            dgv.Columns.Add("sba_name", "SBA");
            dgv.Columns["sba_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sba_name"].ReadOnly = true;
            dgv.Columns["sba_name"].Width = 140;
            dgv.Columns["sba_name"].Visible = true;
            /*9*/
            dgv.Columns.Add("comm", "Комментарий");
            dgv.Columns["comm"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["comm"].ReadOnly = false;
            /*10*/
            dgv.Columns.Add("cost3", "База для расчёта");
            dgv.Columns["cost3"].ReadOnly = true;
            dgv.Columns["cost3"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cost3"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cost3"].Width = 120;
            dgv.Columns["cost3"].SortMode = DataGridViewColumnSortMode.Programmatic;
            /*11*/
            dgv.Columns.Add("pr", "%");
            dgv.Columns["pr"].Visible = true;
            dgv.Columns["pr"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["pr"].DefaultCellStyle.Format = "N2";
            dgv.Columns["pr"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["pr"].ReadOnly = false;
            /*12*/
            dgv.Columns.Add("rent", "Сумма");
            dgv.Columns["rent"].Visible = true;
            dgv.Columns["rent"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rent"].DefaultCellStyle.Format = "N2";
            dgv.Columns["rent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["rent"].ReadOnly = true;
            /*13*/
            dgv.Columns.Add("cm2", "CM 2");
            dgv.Columns["cm2"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["cm2"].DefaultCellStyle.Format = "N2";
            dgv.Columns["cm2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cm2"].ReadOnly = true;
            dgv.Columns["cm2"].Width = 150;
            /*14*/
            dgv.Columns.Add("upd", "update");
            dgv.Columns["upd"].Visible = false;
        }
        /* Заголовки сводного отчёта  */
        private void fillTableHeaderSvodRent( DataGridView dgv )
        {
            dgv.Columns.Clear();

            dgv.Columns.Add("date", "Месяц отчёта");
            dgv.Columns["date"].DefaultCellStyle.Format = "MM - yyyy";
            dgv.Columns["date"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["date"].ReadOnly = true;
            dgv.Columns["date"].Width = 60;

            dgv.Columns.Add("user_name", "Фамилия");
            dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["user_name"].ReadOnly = true;

            dgv.Columns.Add("rent", "Сумма");
            dgv.Columns["rent"].Visible = true;
            dgv.Columns["rent"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rent"].ReadOnly = true;
            dgv.Columns["rent"].DefaultCellStyle.Format = "N2";

            DataGridViewCheckBoxColumn chcol = new DataGridViewCheckBoxColumn();

            chcol.Name = "chcol";
            dgv.Columns.Insert(3, chcol);

            dgv.Columns[3].Width = 100;
            dgv.Columns[3].HeaderText = "Подтверждение";

            dgv.Columns.Add("comm", "Комментарий");
            dgv.Columns["comm"].Visible = true;
            dgv.Columns["comm"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["comm"].ReadOnly = false;

            dgv.Columns.Add("sum", "Проверка подтвержденной суммы");
            dgv.Columns["sum"].Visible = true;
            dgv.Columns["sum"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sum"].ReadOnly = true;
            dgv.Columns["sum"].DefaultCellStyle.Format = "N2";

            dgv.Columns.Add("upd", "update");
            dgv.Columns["upd"].Visible = false;
            
        }

        /* Заголовки отчёта сторно */
        private void fillTableHeaderSvodSt( DataGridView dgv )
        {
            dgv.Columns.Clear();

            dgv.Columns.Add("user2_name", "ФИО заполняющего");
            dgv.Columns["user2_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["user2_name"].ReadOnly = true;

            dgv.Columns.Add("user_name", "ФИО личных продаж");
            dgv.Columns["user_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["user_name"].ReadOnly = true;

            dgv.Columns.Add("rent_lpu2", "ЛПУ");
            dgv.Columns["rent_lpu2"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rent_lpu2"].ReadOnly = true;

            dgv.Columns.Add("rent", "Сумма, руб.");
            dgv.Columns["rent"].Visible = true;
            dgv.Columns["rent"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rent"].ReadOnly = true;
            dgv.Columns["rent"].DefaultCellStyle.Format = "N2";
            
            dgv.Columns.Add("rep_refdocStorn", "С.-ф.");
            dgv.Columns["rep_refdocStorn"].Visible = true;
            dgv.Columns["rep_refdocStorn"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["rep_refdocStorn"].ReadOnly = true;

            dgv.Columns.Add("nom_name", "Номенклатура");
            dgv.Columns["nom_name"].Visible = true;
            dgv.Columns["nom_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["nom_name"].ReadOnly = true;
           

        }

        /* Заголовки отчёта контроллинга */
        private void fillTableHeaderSvodKontr(DataGridView dgv)
        {
            dgv.Columns.Clear();

            dgv.Columns.Add("sba_code", "Код SBA");
            dgv.Columns["sba_code"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sba_code"].ReadOnly = true;

            dgv.Columns.Add("sba_name", "SBA");
            dgv.Columns["sba_name"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sba_name"].ReadOnly = true;

            dgv.Columns.Add("sumNew", "Сумма, руб.");
            dgv.Columns["sumNew"].Visible = true;
            dgv.Columns["sumNew"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sumNew"].ReadOnly = true;
            dgv.Columns["sumNew"].DefaultCellStyle.Format = "N2";

            dgv.Columns.Add("sumOld", "Сумма выплаты с накоплением");
            dgv.Columns["sumOld"].Visible = true;
            dgv.Columns["sumOld"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["sumOld"].ReadOnly = true;
            dgv.Columns["sumOld"].DefaultCellStyle.Format = "N2";

            dgv.Columns.Add("stornOld", "Сумма сторно с накоплением");
            dgv.Columns["stornOld"].Visible = true;
            dgv.Columns["stornOld"].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgv.Columns["stornOld"].ReadOnly = true;
            dgv.Columns["stornOld"].DefaultCellStyle.Format = "N2";

        }
        #endregion

        #region Вывод отчётов
        /* Вывод отчёта для РД и главного - заполнение */
        private void SelectRent(bool flagRD = false)
        {

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            string rd = "0";
            string reg = "0";
            int all = 0;
            int gr = 1;
            string cust = "";
            string mat = "";
            string sba = "";
            string refdoc = "";
            string lpu = "";

            bool fl1, fl2, fl3;

            fl1 = true;
            fl2 = true;
            fl3 = false;

            
            if (User_rent.Enabled == false)
                fl3 = true;
            
            /* Вкладка заполнение */
            if (flagRD == false)
                fillTableHeaderRent(_dgvRent, fl1, fl2, fl3);
            else
            {
                fillTableHeaderReportRent(_dgvRent);
                gr = 2;
            }
            if (User_rent.Enabled == false)
                all = 1;

            if (RD_rent.Enabled == true)
                rd = RD_rent.SelectedValue.ToString();
            
            if (Region_rent.Enabled == true)
                reg = Region_rent.SelectedValue.ToString();

            if (Cust_rent.SelectedItem != null)
            {
                if (Cust_rent.SelectedItem.ToString() == "все")
                {
                    cust = "";
                    f[0] = "";
                }
                else
                {
                    cust = Cust_rent.SelectedItem.ToString();
                    cust = cust.Split(' ')[0];
                }
            }

            if (Mat_rent.SelectedItem != null)
            {
                if (Mat_rent.SelectedItem.ToString() == "все")
                {
                    mat = "";
                    f[1] = "";
                }
                else
                {
                    mat = Mat_rent.SelectedItem.ToString();
                    mat = mat.Split(' ')[0];
                }
            }

            if (SBA_rent.SelectedItem != null)
            {
                if (SBA_rent.SelectedItem.ToString() == "все")
                {
                    sba = "";
                    f[2] = "";
                }
                else
                {
                    sba = SBA_rent.SelectedItem.ToString();
                    sba = sba.Split(' ')[0];
                }
            }

            if (Ref_rent.SelectedItem != null)
            {
                if (Ref_rent.SelectedItem.ToString() == "все")
                {
                    refdoc = "";
                    f[3] = "";
                }
                else
                {
                    refdoc = Ref_rent.SelectedItem.ToString();
                    refdoc = refdoc.Split(' ')[0];
                }
            }

            if (LPU_rent.SelectedItem != null)
            {
                if (LPU_rent.SelectedItem.ToString() == "все")
                {
                    lpu = "";
                    f[4] = "";
                }
                else
                    lpu = LPU_rent.SelectedItem.ToString();
            }

            if (cust != "")
                f[0] = cust;
            if (mat != "")
                f[1] = mat;
            if (sba != "")
                f[2] = sba;
            if (refdoc != "")
                f[3] = refdoc;
            if (lpu != "")
                f[4] = lpu;

            if (gr == 2)
            {
                string rp = "0";
                if (User_rent.Enabled == true)
                    rp = User_rent.SelectedValue.ToString();

                _dgvRent.Rows.Clear();

                dt1 = sql1.GetRecords("exec Rent_Select_ReportRD @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11",
                    rd, rp, Div_rent.SelectedValue,
                    dtpRent.Value.Year.ToString() + "-" + dtpRent.Value.Month.ToString() + "-" + "01",
                    1,
                    reg, cust, mat, sba, refdoc, lpu);

                if (dt1 == null)
                {
                    MessageBox.Show("Ошибка");
                    return;
                }

                int t = 0;
                foreach (DataRow row in dt1.Rows)
                {
                    _dgvRent.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13]);

                    if (row.ItemArray[1].ToString() != "Итого")
                    {
                        if (Convert.ToInt32(row.ItemArray[14]) == 1)
                        {
                            _dgvRent.Rows[t].DefaultCellStyle.BackColor = bbgray4;
                            _dgvRent.Rows[t].Cells["comm"].ReadOnly = true;
                        }
                    }

                    t++;
                }
                _dgvRent.Columns[0].DefaultCellStyle.BackColor = bbgreen3;
                _dgvRent.Columns[2].DefaultCellStyle.BackColor = bbgreen3;

                _dgvRent.Rows[_dgvRent.Rows.Count - 1].DefaultCellStyle.BackColor = bbgray5;
                
            }

            if (gr == 1)
            {
                dt1 = sql1.GetRecords("exec Rent_Select_Report @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13",
                    reg, User_rent.SelectedValue, all,
                    dateTimePicker21.Value.Year.ToString() + "-" + dateTimePicker21.Value.Month.ToString() + "-" + "01",
                    dateTimePicker22.Value.Year.ToString() + "-" + dateTimePicker22.Value.Month.ToString() + "-" + "01", Div_rent.SelectedValue, rd, gr, cust, mat, sba, refdoc, lpu);


                if (Div_rent.SelectedValue.ToString() == "3")
                    _dgvRent.Columns["lpu_sname"].Visible = false;
                else
                    _dgvRent.Columns["lpu_sname"].Visible = true;

                _dgvRent.Rows.Clear();

                if (dt1 == null)
                {
                    MessageBox.Show("Ошибка");
                    return;
                }
              
                int i = 0;

                foreach (DataRow row in dt1.Rows)
                {
                    double s = 0.0;
                    if (row.ItemArray[21] == null)
                        _dgvRent.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13], row.ItemArray[14], row.ItemArray[15], row.ItemArray[16], row.ItemArray[17], row.ItemArray[18], row.ItemArray[19], row.ItemArray[20], "", "%", row.ItemArray[23], "", "", row.ItemArray[24]);
                    else
                    {
                        if (row.ItemArray[21].ToString() != "" && row.ItemArray[18].ToString() != "")
                            s = Convert.ToDouble(row.ItemArray[21]) / 100 * Convert.ToDouble(row.ItemArray[18]);
                        _dgvRent.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13], row.ItemArray[14], row.ItemArray[15], row.ItemArray[16], row.ItemArray[17], row.ItemArray[18], row.ItemArray[19], row.ItemArray[20], row.ItemArray[21], "%", row.ItemArray[23], "", s, row.ItemArray[24]);
                    }
                    if (Convert.ToInt32(row.ItemArray[22]) == 1)
                    {
                        _dgvRent.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                        _dgvRent.Rows[i].Cells["rent"].ReadOnly = true;
                        _dgvRent.Rows[i].Cells["rentval"].ReadOnly = true;
                        _dgvRent.Rows[i].Cells["rent_lpu"].ReadOnly = true;
                    }

                    i++;
                }
            }

            globalData.update = true;

            if (gr == 2 && _dgvRent.RowCount == 1)
                return;
            
            fillComboBox(dt1, "cust", Cust_rent, 0);
            if (dt1.Rows.Count > 0)
            {
                if (f[0] != String.Empty)
                    Cust_rent.SelectedIndex = 1;
                else
                    Cust_rent.SelectedIndex = 0;
            }
            if (gr == 1)
            {
                fillComboBox(dt1, "mat", Mat_rent, 1);
                if (dt1.Rows.Count > 0)
                {
                    if (f[1] != String.Empty)
                        Mat_rent.SelectedIndex = 1;
                    else
                        Mat_rent.SelectedIndex = 0;
                }
            }
            fillComboBox(dt1, "sba", SBA_rent, 2);
            if (dt1.Rows.Count > 0)
            {
                if (f[2] != String.Empty)
                    SBA_rent.SelectedIndex = 1;
                else
                    SBA_rent.SelectedIndex = 0;
            }

            fillComboBox2(dt1, "rep_refDoc", Ref_rent, 3);
            if (dt1.Rows.Count > 0)
            {
                if (f[3] != String.Empty)
                    Ref_rent.SelectedIndex = 1;
                else
                    Ref_rent.SelectedIndex = 0;
            }

            fillComboBox2(dt1, "lpu_sname", LPU_rent, 4);
            if (dt1.Rows.Count > 0)
            {
                if (f[4] != String.Empty)
                    LPU_rent.SelectedIndex = 1;
                else
                    LPU_rent.SelectedIndex = 0;
            }

            globalData.update = false;

            if (subsum != 0)
            {
                int temp = subsum;
                subsum++;
                SubSumRent(temp);
            }
        }

        /* Вывод отчёта для ДМ*/
        private void SelectRepRent()
        {
            if (_dgvRentRD.Rows.Count != 0)
                _dgvRentRD.Rows.Clear();
            
            fillTableHeaderReportRent(_dgvRentRD);
            
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            string rd = "0", user = "0";

            if (rd_rdRent.Enabled != false && rd_rdRent.SelectedValue != null)
                rd = rd_rdRent.SelectedValue.ToString();
            if (rp_rdRent.Enabled == true && rp_rdRent.SelectedValue != null)
                user = rp_rdRent.SelectedValue.ToString();

            dt1 = sql1.GetRecords("exec Rent_Select_ReportRD @p1, @p2, @p3, @p4, @p5", rd, user, div_rdRent.SelectedValue, dtp23.Value.Year.ToString() + "-" + dtp23.Value.Month + "-01", 0);


            if (dt1 == null)
            {
                MessageBox.Show("Ошибка");
                return;
            }

            int i = 0;
            foreach (DataRow row in dt1.Rows)
            {
                _dgvRentRD.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5], row.ItemArray[6], row.ItemArray[7], row.ItemArray[8], row.ItemArray[9], row.ItemArray[10], row.ItemArray[11], row.ItemArray[12], row.ItemArray[13]);

                if (row.ItemArray[1].ToString() != "Итого")
                {
                    if (Convert.ToInt32(row.ItemArray[14]) == 1)
                    {
                        _dgvRentRD.Rows[i].DefaultCellStyle.BackColor = bbgray4;
                        _dgvRentRD.Rows[i].Cells["comm"].ReadOnly = true;
                    }
                }

                i++;
            }
            _dgvRentRD.Columns[0].DefaultCellStyle.BackColor = bbgreen3;
            _dgvRentRD.Columns[2].DefaultCellStyle.BackColor = bbgreen3;

            _dgvRentRD.Rows[_dgvRentRD.Rows.Count - 1].DefaultCellStyle.BackColor = bbgray5;
            
            //globalData.update = true;
            
        }

        /* Вывод сводного отчёта */
        private void SelectSvod()
        {
            fillTableHeaderSvodRent(_dgvRentRD);

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();
            string rd = "0", user = "0";

            if (rd_rdRent.Enabled != false && rp_rdRent.SelectedValue != null)
                rd = rd_rdRent.SelectedValue.ToString();
            if (rp_rdRent.Enabled == true && rp_rdRent.SelectedValue != null)
                user = rp_rdRent.SelectedValue.ToString();

            dt1 = sql1.GetRecords("exec Rent_Select_ReportSvod @p1, @p2, @p3", rd, user, dtp23.Value.Year.ToString() + "-" + dtp23.Value.Month + "-01");

            _dgvRentRD.Rows.Clear();

            if (dt1 != null)
            {
                int i = 0;
                
                foreach (DataRow row in dt1.Rows)
                {
                    if (row.ItemArray[3].ToString() == "1")
                        _dgvRentRD.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], true, row.ItemArray[5], row.ItemArray[4]);
                    else
                        _dgvRentRD.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], false, row.ItemArray[5], row.ItemArray[4]);

                    if (row.ItemArray[3].ToString() != "1" && row.ItemArray[4].ToString() != "0")
                        _dgvRentRD.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    i++;
                }
            }
        }

        /* Вывод сторно */
        private void SelectStorno()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable(); 

            fillTableHeaderSvodSt(_dgvStorno);

            dt1 = sql1.GetRecords("exec Rent_Select_Storno @p1", dtpSt.Value.Year.ToString() + "-" + dtpSt.Value.Month + "-01");

            _dgvStorno.Rows.Clear();

            if (dt1 != null)
            {
                foreach (DataRow row in dt1.Rows)
                    _dgvStorno.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4], row.ItemArray[5]);
            }
        }

        /* Отчёт для контроллинга */
        private void SelectKontrol()
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            fillTableHeaderSvodKontr(_dgvStorno);

            dt1 = sql1.GetRecords("exec Rent_Select_Kontrol @p1", globalData.UserID);

            _dgvStorno.Rows.Clear();

            if (dt1 != null)
                 foreach (DataRow row in dt1.Rows)
                    _dgvStorno.Rows.Add(row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3], row.ItemArray[4]);
        }

#endregion

        #region вспомогательные функции
        
        private void ExportInExcelRent(DataGridView dgv)
        {
            try
            {
                object misValue = System.Reflection.Missing.Value;

                Excel.Application xlApp;
                Excel.Workbook xlWB;
                Excel.Worksheet xlSh;

                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(misValue);
                xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

                int i = 1;

                // Пишем заголовки в Excel файл
                for (int j = 0; j < dgv.ColumnCount - 1; j++)
                {
                    if (j != 7 && j != 5)
                    {
                        xlSh.Cells[2, i] = dgv.Columns[j].HeaderText;

                        ((Excel.Range)xlSh.Cells[2, i]).Font.Bold = true;
                        ((Excel.Range)xlSh.Cells[2, i]).Font.Size = 10;
                        ((Excel.Range)xlSh.Cells[2, i]).Font.Name = "Arial Cyr";
                        ((Excel.Range)xlSh.Cells[2, i]).HorizontalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)xlSh.Cells[2, i]).VerticalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)xlSh.Cells[2, i]).Interior.Color = Color.FromArgb(204, 255, 204).ToArgb();
                        ((Excel.Range)xlSh.Cells[2, i]).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic);

                        //((Excel.Range)xlSh.Rows[2]).AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                        i++;
                    }
                }
                ((Excel.Range)xlSh.Cells[2, 1]).AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //((Excel.Range)xlSh.Cells[2, 2]).AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //((Excel.Range)xlSh.Cells[2, 3]).AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 10;
                ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 15;
                ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 10;
                ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 25;
                ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 30;
                ((Excel.Range)xlSh.Columns[6]).ColumnWidth = 40;
                ((Excel.Range)xlSh.Columns[7]).ColumnWidth = 30;
                ((Excel.Range)xlSh.Columns[8]).ColumnWidth = 25;
                ((Excel.Range)xlSh.Columns[9]).ColumnWidth = 20;
                ((Excel.Range)xlSh.Columns[10]).ColumnWidth = 10;
                ((Excel.Range)xlSh.Columns[11]).ColumnWidth = 20;
                ((Excel.Range)xlSh.Columns[12]).ColumnWidth = 10;
                i = 3;
                try
                {

                    xlSh.Cells[1, 1] = "Рентабельность";
                    //((Excel.Range)xlSh.Rows[1]).Interior.Color = bbgray5.ToArgb();
                    ((Excel.Range)xlSh.Rows[1]).Font.Bold = true;
                    ((Excel.Range)xlSh.Cells[1, 1]).Interior.Color = bbgray5.ToArgb();

                    xlSh.Range[xlSh.Cells[1, 1], xlSh.Cells[1, 12]].Merge(Type.Missing);
                    xlSh.Range[xlSh.Cells[1, 1], xlSh.Cells[1, 12]].HorizontalAlignment = Excel.Constants.xlCenter;
                    xlSh.Range[xlSh.Cells[1, 1], xlSh.Cells[1, 12]].VerticalAlignment = Excel.Constants.xlCenter;
                    xlSh.Range[xlSh.Cells[1, 1], xlSh.Cells[1, 12]].BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlHairline, Excel.XlColorIndex.xlColorIndexAutomatic);

                    //Excel.Range = xlSh.GetType().InvokeMember("Range", BindingFlags.GetProperty,
                    //null, xlSh, new object[] { "A1" });
                    //object[] args = new object[] { "HorizontalAlignment" };
                    //Excel.Range.GetType().InvokeMember("MergeCells", BindingFlags.SetProperty, null, Excel.Range, new object[] { true });
                    //Excel.Range.GetType().InvokeMember("HorizontalAlignment", BindingFlags.SetProperty, null, Excel.Range, args);
                    //xlSh.Cells[1,12] = xlSh.get_Range("A", "L").MergeCells;
                    //xlSh.Cells.MergeCells(Type.Missing);

                    int k = 0;

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        k = 1;
                        if (row.DefaultCellStyle.BackColor == bbgreen3)
                        {
                            ((Excel.Range)xlSh.Rows[i]).Interior.Color = Color.FromArgb(204, 255, 204).ToArgb();
                            ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                        }
                        if (row.DefaultCellStyle.BackColor == bbgray5)
                        {
                            ((Excel.Range)xlSh.Rows[i]).Interior.Color = bbgray5.ToArgb();
                            ((Excel.Range)xlSh.Rows[i]).Font.Bold = true;
                        }

                        for (int j = 0; j < dgv.ColumnCount; j++)
                        {
                            if (dgv.Columns[j].Visible)
                            {
                                if (row.Cells[j].Value != null)
                                {
                                    if (((dgv.Columns[j].HeaderText == "Месяц отчёта") || (dgv.Columns[j].HeaderText == "Месяц отгрузки")) && (row.Cells[j].Value.ToString() != String.Empty))
                                    {
                                        xlSh.Cells[i, k] = Convert.ToDateTime(row.Cells[j].Value).ToShortDateString();
                                        ((Excel.Range)xlSh.Cells[i, k]).Interior.Color = Color.FromArgb(204, 255, 204).ToArgb();
                                    }
                                    else
                                    {
                                        if ((digit(row.Cells[j].Value.ToString())) &&
                                            ((dgv.Columns[j].HeaderText == "База для расчёта") ||
                                            (dgv.Columns[j].HeaderText == "%") ||
                                            (dgv.Columns[j].HeaderText == "Сумма") ||
                                            (dgv.Columns[j].HeaderText == "Рентабельность") ||
                                            (dgv.Columns[j].HeaderText == "CM 2")))
                                        {
                                            xlSh.Cells[i, k] = Math.Round(float.Parse(row.Cells[j].Value.ToString()), 2);
                                            ((Excel.Range)xlSh.Cells[i, k]).NumberFormat = "### ##0,00";
                                            if (dgv.Columns[j].HeaderText == "Рентабельность")
                                            {
                                                ((Excel.Range)xlSh.Cells[i, k]).Font.Color = Color.FromArgb(0, 51, 102).ToArgb();
                                                ((Excel.Range)xlSh.Cells[i, k]).Font.Bold = true;
                                            }
                                        }
                                        else
                                            xlSh.Cells[i, k] = row.Cells[j].Value.ToString();

                                    }
                                    ((Excel.Range)xlSh.Cells[i, k]).BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic);
                                }
                                k++;
                            }
                        }
                        i++;
                    }

                    i -= 1;

                    ((Excel.Range)xlSh.Cells[i, 9]).FormulaLocal = "=ПРОМЕЖУТОЧНЫЕ.ИТОГИ(9;I3:I" + Convert.ToString(i - 1) + ")";
                    ((Excel.Range)xlSh.Cells[i, 11]).FormulaLocal = "=ПРОМЕЖУТОЧНЫЕ.ИТОГИ(9;K3:K" + Convert.ToString(i - 1) + ")";
                    ((Excel.Range)xlSh.Cells[i, 10]).FormulaLocal = "=K" + Convert.ToString(i) + "/I" + Convert.ToString(i) + "* 100";


                    string s = "rr_rr";
                    xlWB.Protect(s, true, false);
                    xlSh.Protect(s, true, true, true, false, false, false, false, false, false, false, false, false, true, true, false);

                    if (globalData.db_id == 1)
                    {
                        String filename = "";

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
            catch (Exception err)
            {
                MessageBox.Show("Ошибка при экспорте данных в Excel. Системная ошибка: " + err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportInExcelSt(DataGridView dgv)
        {
            Object misValue = System.Reflection.Missing.Value;
            Excel.Application xlApp;
            Excel.Workbook xlWB;
            Excel.Worksheet xlSh;

            xlApp = new Excel.Application();
            xlWB = xlApp.Workbooks.Add(misValue);
            xlSh = (Excel.Worksheet)xlWB.Worksheets.get_Item(1);

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

                ((Excel.Range)xlSh.Columns[1]).ColumnWidth = 12;
                ((Excel.Range)xlSh.Columns[2]).ColumnWidth = 10;
                ((Excel.Range)xlSh.Columns[3]).ColumnWidth = 20;
                ((Excel.Range)xlSh.Columns[4]).ColumnWidth = 25;
                ((Excel.Range)xlSh.Columns[5]).ColumnWidth = 15;

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
                                if ((digit(row.Cells[j].Value.ToString())) &&
                                    ((dgv.Columns[j].HeaderText == "Сумма") ||
                                    (dgv.Columns[j].HeaderText == "Сумма выплаты с накоплением") ||
                                    (dgv.Columns[j].HeaderText == "Сумма сторно с накоплением")))
                                        xlSh.Cells[i, k] = Math.Round(float.Parse(row.Cells[j].Value.ToString()), 2);
                                    else
                                        xlSh.Cells[i, k] = row.Cells[j].Value.ToString();
                               
                            }
                            k++;
                        }
                    }
                    i++;
                }
                if (globalData.db_id == 1)
                {
                    String filename = "";

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

        private void ConfirmRepRentRD(ComboBox cbRD, ComboBox cbRP, DateTimePicker _date)
        {
            if (cbRD.Enabled == false)
            {
                MessageBox.Show("Нужно выбрать директора!", "Ошибка!");
                return;
            }
            else if (cbRP.Enabled == false || cbRP.SelectedValue == null)
            {
                MessageBox.Show("Нужно выбрать рп, отчёт которого Вы подтверждаете!", "Ошибка!");
                return;
            }
            else
            {
                Sql sql1 = new Sql();

                string res = sql1.GetRecordsOne("exec Rent_Insert_RDAccess @p1, @p2, @p3", cbRD.SelectedValue, cbRP.SelectedValue, _date.Value.Year.ToString() + "-" + _date.Value.Month + "-01");
                MessageBox.Show(res, "Подтверждение");
            }
        }

        private void ConfirmExport()
        {
            if (_dgvRentRD.RowCount != 0)
            {
                Sql sql1 = new Sql();
                DataTable dt = new DataTable();
                string rd = "0", rp = "0";
                string res = "Отчёт готов: \n";

                if (rd_rdRent.Enabled == true)
                    rd = rd_rdRent.SelectedValue.ToString();

                if (rp_rdRent.Enabled == true && rp_rdRent.SelectedValue != null)
                    rp = rp_rdRent.SelectedValue.ToString();

                dt = sql1.GetRecords("exec Rent_Select_RDAccess @p1, @p2, @p3", rp, dtp23.Value.Year.ToString() + "-" + dtp23.Value.Month + "-01", rd);

                if (dt != null)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        res += row.ItemArray[0].ToString();
                        res += "\n";
                    }
                    if (dt.Rows.Count == 0)
                        res += "никто из директоров не подтвердил.\n";
                }
                else
                    res += "никто из директоров не подтвердил.\n";

                // res += "\n\n\nДиректора:\n Бачегов, Бритов, Васильченко, Горун, Ильющенков, \n Киреева, Матвеева, Начаров, Одинцов, Озеров,\nОлиферовский, Плотников, Турчев, Шевченко. \n\nВыгрузить в Excel?";
                res += "\n\nВыгрузить в Excel?";

                DialogResult dr = MessageBox.Show(res, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == System.Windows.Forms.DialogResult.Yes)
                    ExportInExcelRent(_dgvRentRD);
            }
            else
                MessageBox.Show("Постройте отчёт!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void fillComboBox2(DataTable dt1, String nameCol, ComboBox cb1, int filtnum)
        {
            var query = (from c in dt1.AsEnumerable()
                         orderby c.Field<String>(nameCol) ascending
                         select new
                         {
                             Number = c.Field<String>(nameCol),
                             Name = c.Field<String>(nameCol)
                         }).Distinct();
            if (query.Count() != 0)
            {
                cb1.Items.Clear();
                cb1.Items.Add("все");
                for (int i = 0; i < query.Count(); i++)
                {
                    if (query.ElementAt(i).Name != null)
                        cb1.Items.Add(query.ElementAt(i).Name);
                }
                if (f[filtnum] == String.Empty)
                    cb1.SelectedIndex = 0;
                cb1.Enabled = true;
            }
        }

        private void SubSumRent( int colnum )
        {
          
            try
            {
                Double sum = 0, sum2 = 0, sum3 = 0, sum4 = 0;
                Double msum = 0, msum2 = 0, msum3 = 0, msum4 = 0;

                for (int j = 0; j < _dgvRent.Rows.Count; j++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row = _dgvRent.Rows[j];
                    if ((row.DefaultCellStyle.BackColor == bbgreen1) || (row.DefaultCellStyle.BackColor == bbgreen3))
                    {
                        _dgvRent.Rows.Remove(row);
                        j--;
                    }
                }

                if (subsum == colnum)
                {
                    subsum = 0;
                    return;
                }

                _dgvRent.Sort(_dgvRent.Columns[colnum], ListSortDirection.Ascending);
                String s1 = _dgvRent.Rows[0].Cells[colnum].Value.ToString();

                int i = 0;
                foreach (DataGridViewRow row in _dgvRent.Rows)
                {
                    if ((row.Cells[colnum].Value.ToString() != s1) && (s1 != ""))
                    {
                        s1 = _dgvRent.Rows[i].Cells[colnum].Value.ToString();

                        _dgvRent.Rows.Insert(i, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3, "", "", "", "", "", "", sum4);

                        _dgvRent.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                        msum += sum;
                        msum2 += sum2;
                        msum3 += sum3;
                        msum4 += sum4;
                        sum  = 0;
                        sum2 = 0;
                        sum3 = 0;
                        sum4 = 0;
                    }
                    else if (s1 != "")
                    {
                        sum += Convert.ToDouble(row.Cells["ps_count"].Value.ToString());
                        sum2 += Convert.ToDouble(row.Cells["cost2"].Value.ToString());
                        sum3 += Convert.ToDouble(row.Cells["cost3"].Value.ToString());
                        if (row.Cells["sum"].Value != null)
                            sum4 += Convert.ToDouble(row.Cells["sum"].Value.ToString());
                        else
                            sum4 = 0;
                    }
                    i++;
                }
                msum += sum;
                msum2 += sum2;
                msum3 += sum3;
                msum4 += sum4;

                _dgvRent.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sum, sum2, sum3, "", "", "", "", "", "", sum4);
                _dgvRent.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", msum, msum2, msum3, "", "", "", "", "", "", msum4);

                _dgvRent.Rows[i].DefaultCellStyle.BackColor = bbgreen3;
                _dgvRent.Rows[i + 1].DefaultCellStyle.BackColor = bbgreen1;
                subsum = colnum;
            }
            catch
            {
                subsum = 0;
            }
        }
#endregion

        #region Сохранение
        private string SaveRent()
        {
            try
            {
                Sql sql1 = new Sql();

                string ps, db, sba, lpu, user2;
                double rent, rentsum;
                DateTime dps;
                                
                load = false;

                if (User2_rent.SelectedValue == null)
                    return "Необходимо выбрать пользователя!";
                if (globalData.role == 5)
                    user2 = globalData.UserID.ToString();
                else
                    user2 = User2_rent.SelectedValue.ToString();

                //if (sql1.GetRecordsOne("exec GetUserRentAccess @p1", User_rent.SelectedValue.ToString()) == "1")
                //    return "Нет доступа к сохранению! Обратитесь к своему рег.директору.";

                if (_dgvRent.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in _dgvRent.Rows)
                    {
                        if ((row.Cells["upd"].Value != null) && (row.Cells["upd"].Value.ToString() == "1") && (row.DefaultCellStyle.BackColor != bbgreen3) && (row.DefaultCellStyle.BackColor != bbgreen1))
                        {
                            if ((row.Cells["rent"].Value == null) || (row.Cells["rent"].Value.ToString() == ""))
                                row.Cells["rent"].Value = 0;

                            if (row.Cells["rentval"].Value.ToString() == "%")
                            {                            
                                rent = Convert.ToDouble(row.Cells["rent"].Value.ToString());
                                rentsum = Convert.ToDouble(row.Cells["cost3"].Value.ToString()) * rent / 100;
                            }
                            else
                            {
                                rentsum = Convert.ToDouble(row.Cells["rent"].Value.ToString());
                                rent = rentsum / Convert.ToDouble(row.Cells["cost3"].Value.ToString())*100;
                            }

                            if ((row.Cells["ps_id"].Value == null) || (row.Cells["ps_id"].Value.ToString() == ""))
                                ps = "0";
                            else
                                ps = row.Cells["ps_id"].Value.ToString();

                            if ((row.Cells["db_id_ps"].Value == null) || (row.Cells["db_id_ps"].Value.ToString() == ""))
                                db = "0";
                            else
                                db = row.Cells["db_id_ps"].Value.ToString();

                            if ((row.Cells["sba_name"].Value == null) || (row.Cells["sba_name"].Value.ToString() == ""))
                                sba = "";
                            else
                                sba = row.Cells["sba_name"].Value.ToString();

                            if ((row.Cells["rent_lpu"].Value == null) || (row.Cells["rent_lpu"].Value.ToString() == ""))
                                lpu = "";
                            else
                                lpu = row.Cells["rent_lpu"].Value.ToString();
                            
                            load = true;

                            dps = Convert.ToDateTime(row.Cells["date"].Value.ToString());

                            string res = sql1.GetRecordsOne("exec Rent_Insert_Save @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11", rent, rentsum, ps, db, user2,
                                row.Cells["refDoc"].Value, sba, dps.Year.ToString() + "-" + dps.Month.ToString() + "-01", row.Cells["cust_name"].Value, lpu, globalData.UserID);

                            if (res != "1")
                                return res;
                        }
                    }
                }
               
                if (load)
                    SelectRent(globalData.tabFlagRD);

                load = true;
                return "Информация сохранена.";
            }
            catch (Exception err)
            {
                load = true;
                return "Не удалось сохранить информацию по планам, системная ошибка: " + err.Message;
            }
        }

        private string SaveSvod()
        {
            try
            {
                Sql sql1 = new Sql();

                string comm;
                int flag;
                DateTime dps, dps2;


                load = false;

              
                if (_dgvRentRD.Rows.Count != 0)
                {
                    foreach (DataGridViewRow row in _dgvRentRD.Rows)
                    {
                        if (row.Cells["upd"].Value != null)
                        {   
                            if (row.Cells["comm"].Value == null)
                                comm = "";
                            else if (row.Cells["comm"].Value.ToString() == "")
                                comm = "";
                            else
                                comm = row.Cells["comm"].Value.ToString();

                            load = true;

                            if (Convert.ToBoolean(row.Cells["chcol"].Value) == true)
                                flag = 1;
                            else
                                flag = 0;
                           
                            dps = Convert.ToDateTime(row.Cells["date"].Value.ToString());
                            dps2 = dps.AddMonths(1);

                            sql1.GetRecords("exec Rent_Insert_SaveSvod @p1, @p2, @p3, @p4, @p5, @p6", row.Cells["rent"].Value, row.Cells["user_name"].Value,
                                dps.Year.ToString() + "-" + dps.Month.ToString() + "-01", 
                                dps2.Year.ToString() + "-" + dps2.Month.ToString() + "-01", 
                                comm, flag);
                        }
                    }
                }

                if (load)
                    SelectSvod();

                load = true;
                return "Информация сохранена.";
            }
            catch (Exception err)
            {
                load = true;
                return "Не удалось сохранить информацию, системная ошибка: " + err.Message;
            }
        }
  
        private string SaveRDComm( DataGridView _dgv )
        {
            try
            {
                Sql sql1 = new Sql();

                string comm;
                DateTime dps, dps2;

                load = false;


                if (_dgv.Rows.Count != 0)
                {
                    foreach (DataGridViewRow row in _dgv.Rows)
                    {
                        if (row.Cells["upd"].Value != null)
                        {
                            if (row.Cells["comm"].Value == null)
                                comm = "";
                            else if (row.Cells["comm"].Value.ToString() == "")
                                comm = "";
                            else
                                comm = row.Cells["comm"].Value.ToString();

                            load = true;
                                                       
                            dps = Convert.ToDateTime(row.Cells["date"].Value.ToString());
                            dps2 = Convert.ToDateTime(row.Cells["rep_date"].Value.ToString());

                            sql1.GetRecords("exec Rent_Insert_SaveComm @p1, @p2, @p3, @p4, @p5, @p6, @p7", row.Cells["refdoc"].Value, row.Cells["user_name"].Value,
                                dps.Year.ToString() + "-" + dps.Month.ToString() + "-01", dps2.Year.ToString() + "-" + dps2.Month.ToString() + "-01", 
                                row.Cells["lpu_sname"].Value, row.Cells["sba_name"].Value, comm);
                        }
                    }
                }

                if (load)
                {
                    if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage24"])
                        SelectRepRent();
                    else
                        SelectRent(globalData.tabFlagRD);
                }
                load = true;
                return "Информация сохранена.";
            }
            catch (Exception err)
            {
                load = true;
                return "Не удалось сохранить информацию, системная ошибка: " + err.Message;
            }
        }
       
        #endregion 

        #region Кнопки        
        /*Заполнение / отчёт для РД == 23 вкладка*/     
        /*сохранить комментарии или показать параметры */
        private void button50_Click(object sender, EventArgs e)
        {
            if (button50.Text == "Сохранить коммент.")
            {
                string res = SaveRDComm(_dgvRent);

                MessageBox.Show(res, "Результат");
            }
            else
            {
                if (button50.Text == "Показать параметры")
                {
                    button50.Text = "Скрыть параметры";
                    splitContainer3.Panel1Collapsed = false;
                }
                else
                {
                    button50.Text = "Показать параметры";
                    splitContainer3.Panel1Collapsed = true;
                }
                loadUserAccess();
            }
        }
       /*вывод*/
        private void button54_Click(object sender, EventArgs e)
        {
            if (globalData.tabFlagRD == false)
                SelectRent(globalData.tabFlagRD);
            else
            {
                DialogResult dr = MessageBox.Show("Вы уверены, что хотите подтвердить отчёт за " +
                        dtpRent.Value.Month.ToString() + " месяц " + User_rent.Text +
                        "?\n Эту операцию отменить невозможно.\n После подтверждения, доступ РП будет заблокирован, а отчёт будет доступен для ДМ.",
                        "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                    ConfirmRepRentRD(RD_rent, User_rent, dtpRent);
            }
        }
        /*сохранить*/
        private void button48_Click(object sender, EventArgs e)
        {
            if (button48.Text == "Сохранить")
            {
                string res = SaveRent();
                MessageBox.Show(res, "Результат");
            }
            else
            {
                SelectRent(globalData.tabFlagRD);
            }

        }
        /*панель -- закрыть доступ*/
        private void button51_Click(object sender, EventArgs e)
        {
            string rd = RD_rent.SelectedValue.ToString();

            if (checkBox1.Checked == true)
                rd = "0";

            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec Rent_Update_Access @p1, @p2, @p3", 0, 0, rd);

            MessageBox.Show("Доступ всем успешно закрыт", "Результат!");
            loadUserAccess();
        }
        /*панель --- редактировать права*/
        private void button52_Click(object sender, EventArgs e)
        {
            UserRent ur = new UserRent();

            ur.ShowDialog();
            loadUserAccess();
        }    

        /*Отчёт для ДМ / сводный отчёт == 24 вкладка */
        private void button53_Click(object sender, EventArgs e)
        {
            ConfirmExport();
        }

        private void button59_Click(object sender, EventArgs e)
        {
            ConfirmRepRentRD(rd_rdRent, rp_rdRent, dtp23);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            CheckEnabl();

            if (globalData.tabFlagSvod == false)
                SelectRepRent();
            else
                SelectSvod();
        }
        
        private void button56_Click(object sender, EventArgs e)
        {
            string res = String.Empty;

            if (globalData.tabFlagSvod == true)
                res = SaveSvod();
            else
                res = SaveRDComm(_dgvRentRD);

            MessageBox.Show(res, "Результат");
        }

        private void button69_Click(object sender, EventArgs e)
        {
            UserRent ur = new UserRent();

            ur.ShowDialog();
        }

        /* вкладка 26 */
        /* Обновить сторно */
        private void button67_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            sql1.GetRecords("exec Rent_Update_Storno");

            MessageBox.Show("Обновлено!");
        }

        /* Применить */
        private void button68_Click(object sender, EventArgs e)
        {
            SelectStorno();
        }
        /* Выгрузить в Excel */
        private void button49_Click(object sender, EventArgs e)
        {
            ExportInExcelSt(_dgvStorno);
        }

        private void button70_Click(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();

            sql1.GetRecords("exec Rent_Insert_Kontrol @p1", globalData.UserID);

            MessageBox.Show("Обновлено!");
        }

        #endregion

        #region Комбобоксы
        /*Заполнение / отчёт для РД == 23 вкладка*/
        /*******************************************Фильтры главные */ 
        private void Div_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;

            rent_fill("User", globalData.role, User_rent, null, null, Region_rent, RD_rent);
            rent_fill("UserRent", globalData.role, User2_rent, null, null, Region_rent, RD_rent);  

        }
        private void RD_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;

            rent_fill("Region", globalData.role, Region_rent, null, null, null, RD_rent);
            rent_fill("Div", globalData.role, Div_rent, User_rent, null, Region_rent, RD_rent);
            loadUserAccess();

        }
        
        private void Region_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;
            rent_fill("User", globalData.role, User_rent, null, Div_rent, Region_rent, RD_rent);
            rent_fill("UserRent", globalData.role, User2_rent, null, Div_rent, Region_rent, RD_rent);
            rent_fill("Div", globalData.role, Div_rent, User_rent, null, Region_rent, null);
        }
        
        private void User_rent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void User2_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.tabFlagRD == false)
            {
                Sql sql1 = new Sql();

                if (User2_rent.SelectedValue != null && User2_rent.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    string res = sql1.GetRecordsOne("exec Rent_Select_UrtDate @p1", User2_rent.SelectedValue);
                    dtpRent.Value = Convert.ToDateTime(res);
                }
                if (globalData.update == true)
                    return;
            }
        }
        
        /********************************************Побочные фильтры */
        private void Cust_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;
            SelectRent(globalData.tabFlagRD);
        }
        
        private void Mat_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;
            SelectRent(globalData.tabFlagRD);
        }

        private void Ref_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;
            SelectRent(globalData.tabFlagRD);
        }

        private void SBA_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;
            SelectRent(globalData.tabFlagRD);
        }

        private void LPU_rent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.update == true)
                return;
            SelectRent(globalData.tabFlagRD);
        }

        /*Отчёт для ДМ / сводный отчёт == 24 вкладка */

        private void div_rdRent_SelectedIndexChanged(object sender, EventArgs e)
        {
            rent_fill("UserForDM", globalData.role, rp_rdRent, null, div_rdRent, null, rd_rdRent);
        }

        private void rd_rdRent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.tabFlagSvod == false)
            {
                rent_fill("Div", globalData.role, div_rdRent, null, null, null, rd_rdRent);
                rent_fill("UserForDM", globalData.role, rp_rdRent, null, div_rdRent, null, rd_rdRent);
            }
            else
                rent_fill("UserForKontrol", globalData.role, rp_rdRent, null, null, null, rd_rdRent);
        }

        private void rp_rdRent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void dtp23_ValueChanged(object sender, EventArgs e)
        {
            if (globalData.tabFlagSvod == false)
                rent_fill("UserForDM", globalData.role, rp_rdRent, null, div_rdRent, null, rd_rdRent);
        }

        #endregion

        #region Чекбоксы
        /*Заполнение / отчёт для РД == 23 вкладка*/
        /* рд*/
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                RD_rent.Enabled = false;
            else
                RD_rent.Enabled = true;

            rent_fill("Region", globalData.role, Region_rent, null, null, null, RD_rent);
            rent_fill("User", globalData.role, User_rent, null, Div_rent, Region_rent, RD_rent);
            rent_fill("UserRent", globalData.role, User2_rent, null, Div_rent, Region_rent, RD_rent);
            loadUserAccess();
        }
        /* регион */
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                Region_rent.Enabled = false;
            else
                Region_rent.Enabled = true;
            rent_fill("User", globalData.role, User_rent, null, Div_rent, Region_rent, RD_rent);
            rent_fill("UserRent", globalData.role, User2_rent, null, Div_rent, Region_rent, RD_rent);
        }
        /* рп */
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
                User_rent.Enabled = false;
            else
                User_rent.Enabled = true;
            rent_fill("User", globalData.role, User_rent, null, Div_rent, Region_rent, RD_rent);
            rent_fill("UserRent", globalData.role, User2_rent, null, Div_rent, Region_rent, RD_rent);
        }

        /*Отчёт для ДМ / сводный отчёт == 24 вкладка */
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
                rd_rdRent.Enabled = false;
            else
                rd_rdRent.Enabled = true;

            rent_fill("Div", globalData.role, div_rdRent, null, null, null, rd_rdRent);

            if (globalData.tabFlagSvod == false)
                rent_fill("UserForDM", globalData.role, rp_rdRent, null, div_rdRent, null, rd_rdRent);
            else
                rent_fill("UserForKontrol", globalData.role, rp_rdRent, null, null, null, rd_rdRent);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
                rp_rdRent.Enabled = false;
            else
                rp_rdRent.Enabled = true;
        }
        #endregion

        #region События таблицы
        /*Заполнение / отчёт для РД == 23 вкладка*/

        private void _dgvUsRent_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = new DataTable();

            dt1 = sql1.GetRecords("exec Rent_Update_Access @p1, @p2", _dgvUsRent.Rows[e.RowIndex].Cells[2].Value,_dgvUsRent.Rows[e.RowIndex].Cells[0].Value);
            
        }

        private void _dgvRent_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgvRent);
        }

        private void _dgvStorno_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgvStorno);
        }


        private void _dgvRent_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (((e.ColumnIndex >= 3) && (e.ColumnIndex <= 16)) || (e.ColumnIndex == 19))
                {
                    SubSumRent(e.ColumnIndex);
                }
            }
        }

        private void _dgvRent_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Data error occurs:" + e.Exception.Message);
        }

        private void _dgvRent_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (globalData.tabFlagRD == false)
            {
                if (_dgvRent.Rows[e.RowIndex].DefaultCellStyle.BackColor == bbgray4)
                    _dgvRent.Rows[e.RowIndex].Cells["rent"].ReadOnly = true;
                else if ((e.ColumnIndex == 21) || (e.ColumnIndex == 23))
                    _dgvRent.Rows[e.RowIndex].Cells["upd"].Value = 1;
            }
            else
            {
                if (e.ColumnIndex == 9)
                    _dgvRent.Rows[e.RowIndex].Cells["upd"].Value = 1;
            }
        }
        
        private void _dgvRentRD_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _dgvRentRD.Rows[e.RowIndex].Cells["upd"].Value = 1;
        }

        private void _dgvRentRD_SelectionChanged(object sender, EventArgs e)
        {
            CountSelectCell(_dgvRentRD);
        }
        #endregion

    }
}
