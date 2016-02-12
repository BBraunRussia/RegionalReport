using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.addedit
{
    public partial class AllocCosts : Form
    {
        int index;
        int db;
        int kvart;
        public string price;

        public AllocCosts()
        {
            InitializeComponent();
            label1.Text = "Сумма, уе";

            index = 0;
            kvart = 0;
            tbSum.Visible = false;
            label1.Visible = false;
            btnSave.Visible = false;
            btnFilt.Visible = true;
            btnFiltUse.Visible = true;
            CheckFilter();
        }

        public AllocCosts(int i, int d, int k, string pr)
        {
            InitializeComponent();
            label1.Text = "Сумма, уе";

            tbSum.Visible = true;
            label1.Visible = true;
            btnSave.Visible = true;
            btnFilt.Visible = false;
            btnFiltUse.Visible = false;            
            index = i;
            db = d;
            kvart = k;
            tbSum.Text = pr;
            price = pr;

            CheckExist();
            
        }

        public AllocCosts(int k)
        {
            InitializeComponent();

            kvart = k;

            label1.Text = "Сумма, уе";

            tbSum.Visible   = true;
            label1.Visible  = true;
            btnSave.Visible = true;
            btnFilt.Visible = false;
            btnFiltUse.Visible = false;
        }


        public void CheckExist()
        {
            Sql sql1 = new Sql();

         
            DataTable dt1 = new DataTable();
            dt1 = sql1.GetRecords("exec CheckExistMA @p1, @p2, @p3", index, db, kvart);

            if (dt1 == null || dt1.Rows.Count == 0)
            {
                tbSum.Text = price;
                return;
            }

            foreach (DataRow row in dt1.Rows)
            {
                switch (row.ItemArray[0].ToString())
                {
                    case "":
                        if (row.ItemArray[1].ToString() == "1")
                            chHC.Checked = true;
                        if (row.ItemArray[1].ToString() == "2")
                            chAE.Checked = true;
                        if (row.ItemArray[1].ToString() == "3")
                            chOM.Checked = true;
                        break;
                    /*HC*/
                    case "01":
                        ch01.Checked = true;
                        break;
                    case "02":
                        ch02.Checked = true;
                        break;
                    case "04":
                        ch04.Checked = true;
                        break;
                    case "05":
                        ch05.Checked = true;
                        break;
                    case "07":
                        ch07.Checked = true;
                        break;
                    case "09":
                        ch09.Checked = true;
                        break;
                    case "10":
                        ch10.Checked = true;
                        break;
                    case "12":
                        ch12.Checked = true;
                        break;
                    case "16":
                        ch16.Checked = true;
                        break;
                    case "20":
                        ch20.Checked = true;
                        break;
                    case "21":
                        ch21.Checked = true;
                        break;
                    case "23":
                        ch23.Checked = true;
                        break;
                    case "25":
                        ch25.Checked = true;
                        break;
                    case "26":
                        ch26.Checked = true;
                        break;
                    case "27":
                        ch27.Checked = true;
                        break;
                    case "29":
                        ch29.Checked = true;
                        break;
                    /*AE*/
                    case "30":
                        chST.Checked = true;
                        break;
                    case "35":
                        chCT.Checked = true;
                        break;
                    case "33":
                        chOT.Checked = true;
                        break;
                    case "31":
                        chNE.Checked = true;
                        break;
                    case "36":
                        chVS.Checked = true;
                        break;
                    case "39":
                        chAM.Checked = true;
                        break;
                    case "34":
                        chSP.Checked = true;
                        break;                   
                    /*OM*/  
                    case "61":
                        ch61.Checked = true;
                        break;
                    case "62":
                        ch62.Checked = true;
                        break;
                    case "63":
                        ch63.Checked = true;
                        break;
                    case "64":
                        ch64.Checked = true;
                        break;
                    case "66":
                        ch66.Checked = true;
                        break;
                    case "69":
                        ch69.Checked = true;
                        break;
                    default:
                        continue;
                }
               price = row.ItemArray[2].ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tbSum.Text.Trim() != String.Empty)
            {
                /*старые записи помечаем для удаления*/
                UpdFlag(1);
                if (SaveCostsMA() != 0)
                {
                    /*обновляем цену*/
                    UpdPlan();
                    MessageBox.Show("Успешно сохранено!");
                    price = tbSum.Text.Trim();
                    /*удаляем старые записи, если они были*/
                    DelCosts();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Нужно выбрать хотя бы один SBA!");
                    /*снимаем флаг удаления со старых записей */
                    UpdFlag(0);
                }
            }
            else
                MessageBox.Show("Введите сумму!");
        }

        void UpdFlag( int flag )
        {
            Sql sql1 = new Sql();

            sql1.GetRecords("exec UpdMarkActFlag @p1, @p2, @p3, @p4", index, db, kvart, flag);
        }

        void UpdPlan()
        {
            Sql sql1 = new Sql();

            sql1.GetRecords("exec UpdMarkActPlan @p1, @p2, @p3, @p4", index, db, tbSum.Text.Trim(), kvart);
        }

        void DelCosts()
        {
            Sql sql1 = new Sql();

            sql1.GetRecords("exec DeleteCostsMA @p1, @p2, @p3", index, db, kvart);
        }

        int SaveCostsMA()
        {
            int count = 0;

            /* AE */
            if (Check(chAE) == 1)
                count += InsBD(-2);
            else
            {
                if (Check(chST) == 1)
                    count += InsBD(30);
                if (Check(chCT) == 1)
                    count += InsBD(35);
                if (Check(chOT) == 1)
                    count += InsBD(33);
                if (Check(chNE) == 1)
                    count += InsBD(31);
                if (Check(chSP) == 1)
                    count += InsBD(34);
                if (Check(chVS) == 1)
                    count += InsBD(36);
                if (Check(chAM) == 1)
                    count += InsBD(39);               
            }

            /* OM */
            if (Check(chOM) == 1)
                count += InsBD(-3);
            else
            {
                if (Check(ch61) == 1)
                    count += InsBD(61);
                if (Check(ch62) == 1)
                    count += InsBD(62);
                if (Check(ch63) == 1)
                    count += InsBD(63);
                if (Check(ch64) == 1)
                    count += InsBD(64);
                if (Check(ch66) == 1)
                    count += InsBD(66);
                if (Check(ch69) == 1)
                    count += InsBD(69);
            }
            
            /* HC */
            if (Check(chHC) == 1)
                count += InsBD(-1);
            else
            {
                if (Check(ch01) == 1)
                    count += InsBD(1);
                if (Check(ch02) == 1)
                    count += InsBD(2);
                if (Check(ch04) == 1)
                    count += InsBD(4);
                if (Check(ch05) == 1)
                    count += InsBD(5);
                if (Check(ch07) == 1)
                    count += InsBD(7);
                if (Check(ch09) == 1)
                    count += InsBD(9);
                if (Check(ch10) == 1)
                    count += InsBD(10);
                if (Check(ch12) == 1)
                    count += InsBD(12);
                if (Check(ch16) == 1)
                    count += InsBD(16);
                if (Check(ch20) == 1)
                    count += InsBD(20);
                if (Check(ch21) == 1)
                    count += InsBD(21);
                if (Check(ch23) == 1)
                    count += InsBD(23);
                if (Check(ch25) == 1)
                    count += InsBD(25);
                if (Check(ch26) == 1)
                    count += InsBD(26);
                if (Check(ch27) == 1)
                    count += InsBD(27);
                if (Check(ch29) == 1)
                    count += InsBD(29);
            }

            return count;
        } 

        int Check(CheckBox ch)
        {
            if (ch.Checked)
                return 1;
            else
                return 0;
        }

        /*sba_code*/
        int InsBD( int sbaCode )
        {
            string _sba = "0";
            int _sdiv = 0;
            Sql sql1 = new Sql();

            if (sbaCode < 0)
            {
                if (sbaCode == -1)
                    _sdiv = 1;
                if (sbaCode == -2)
                    _sdiv = 2;
                if (sbaCode == -3)
                    _sdiv = 3;
            }
            else
            {
                if (sbaCode < 10)
                    _sba = "0" + sbaCode.ToString();
                else
                    _sba = sbaCode.ToString();
            }

            sql1.GetRecords("exec InsMASBA @p1, @p2, @p3, @p4, @p5", index, db, _sba, _sdiv, kvart);

            return 1;
        }

        private void tbSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            validation(e);
        }

        private void validation(KeyPressEventArgs e)
        {
            if ((!(Char.IsDigit(e.KeyChar))) && (e.KeyChar != ','))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Вы уверены, что хотите закрыть окно? Изменения не будут сохранены!","Информация", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            price = tbSum.Text.Trim();
            this.Close();
        }

        private void btnFilt_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in globalData.dtSBA.Rows)
            {
                row[1] = 0;
            }
               chHC.Checked = false;
               chAE.Checked = false;
               chOM.Checked = false;
               ch01.Checked = false;
               ch02.Checked = false;
               ch04.Checked = false;
               ch05.Checked = false;
               ch07.Checked = false;
               ch09.Checked = false;
               ch10.Checked = false;
               ch12.Checked = false;
               ch16.Checked = false;
               ch20.Checked = false;
               ch21.Checked = false;
               ch23.Checked = false;
               ch25.Checked = false;
               ch26.Checked = false;
               ch27.Checked = false;
               ch29.Checked = false;
               chST.Checked = false;
               chCT.Checked = false;
               chOT.Checked = false;
               chNE.Checked = false;
               chSP.Checked = false;
               chVS.Checked = false;
               chAM.Checked = false;              
               ch61.Checked = false;
               ch62.Checked = false;
               ch63.Checked = false;
               ch64.Checked = false;
               ch66.Checked = false;
               ch69.Checked = false;
        }

        private void btnFiltUse_Click(object sender, EventArgs e)
        {
            globalData.dtSBA.Rows[0][1] = Convert.ToInt32(ch01.Checked);
            globalData.dtSBA.Rows[29][1] = Convert.ToInt32(ch02.Checked);
            globalData.dtSBA.Rows[1][1] = Convert.ToInt32(ch04.Checked);
            globalData.dtSBA.Rows[2][1] = Convert.ToInt32(ch05.Checked);
            globalData.dtSBA.Rows[3][1] = Convert.ToInt32(ch07.Checked);
            globalData.dtSBA.Rows[4][1] = Convert.ToInt32(ch09.Checked);
            globalData.dtSBA.Rows[5][1] = Convert.ToInt32(ch10.Checked);
            globalData.dtSBA.Rows[6][1] = Convert.ToInt32(ch12.Checked);
            globalData.dtSBA.Rows[7][1] = Convert.ToInt32(ch16.Checked);
            globalData.dtSBA.Rows[8][1] = Convert.ToInt32(ch20.Checked);
            globalData.dtSBA.Rows[9][1] = Convert.ToInt32(ch21.Checked);
            globalData.dtSBA.Rows[10][1] = Convert.ToInt32(ch23.Checked);
            globalData.dtSBA.Rows[11][1] = Convert.ToInt32(ch25.Checked);
            globalData.dtSBA.Rows[12][1] = Convert.ToInt32(ch26.Checked);
            globalData.dtSBA.Rows[13][1] = Convert.ToInt32(ch27.Checked);
            globalData.dtSBA.Rows[14][1] = Convert.ToInt32(ch29.Checked);
            globalData.dtSBA.Rows[15][1] = Convert.ToInt32(chST.Checked);
            globalData.dtSBA.Rows[16][1] = Convert.ToInt32(chCT.Checked);
            globalData.dtSBA.Rows[17][1] = Convert.ToInt32(chSP.Checked);
            globalData.dtSBA.Rows[18][1] = Convert.ToInt32(chOT.Checked);
            globalData.dtSBA.Rows[19][1] = Convert.ToInt32(chNE.Checked);
            globalData.dtSBA.Rows[21][1] = Convert.ToInt32(chVS.Checked);
            globalData.dtSBA.Rows[22][1] = Convert.ToInt32(chAM.Checked);            
            globalData.dtSBA.Rows[23][1] = Convert.ToInt32(ch61.Checked);
            globalData.dtSBA.Rows[24][1] = Convert.ToInt32(ch62.Checked);
            globalData.dtSBA.Rows[25][1] = Convert.ToInt32(ch63.Checked);
            globalData.dtSBA.Rows[26][1] = Convert.ToInt32(ch64.Checked);
            globalData.dtSBA.Rows[27][1] = Convert.ToInt32(ch66.Checked);
            globalData.dtSBA.Rows[28][1] = Convert.ToInt32(ch69.Checked);
            

            if (chHC.Checked == true)
            {
                globalData.dtSBA.Rows[0][1]  = 1;
                globalData.dtSBA.Rows[29][1] = 1;
                globalData.dtSBA.Rows[1][1] = 1;
                globalData.dtSBA.Rows[2][1] = 1;
                globalData.dtSBA.Rows[3][1] = 1;
                globalData.dtSBA.Rows[4][1] = 1;
                globalData.dtSBA.Rows[5][1] = 1;
                globalData.dtSBA.Rows[6][1] = 1;
                globalData.dtSBA.Rows[7][1] = 1;
                globalData.dtSBA.Rows[8][1] = 1;
                globalData.dtSBA.Rows[9][1] = 1;
                globalData.dtSBA.Rows[10][1] = 1;
                globalData.dtSBA.Rows[11][1] = 1;
                globalData.dtSBA.Rows[12][1] = 1;
                globalData.dtSBA.Rows[13][1] = 1;
                globalData.dtSBA.Rows[14][1] = 1;
            }

            if (chAE.Checked == true)
            {
                globalData.dtSBA.Rows[15][1] = 1;
                globalData.dtSBA.Rows[16][1] = 1;
                globalData.dtSBA.Rows[18][1] = 1;
                globalData.dtSBA.Rows[19][1] = 1;
                globalData.dtSBA.Rows[21][1] = 1;
                globalData.dtSBA.Rows[22][1] = 1;
                globalData.dtSBA.Rows[17][1] = 1;                
            }
            if (chOM.Checked == true)
            {
                globalData.dtSBA.Rows[23][1] = 1;
                globalData.dtSBA.Rows[24][1] = 1;
                globalData.dtSBA.Rows[25][1] = 1;
                globalData.dtSBA.Rows[26][1] = 1;
                globalData.dtSBA.Rows[27][1] = 1;
                globalData.dtSBA.Rows[28][1] = 1;
            }

            this.Close();
        }


        private void CheckFilter()
        {
            if (globalData.dtSBA != null)
            {
                ch01.Checked = Convert.ToInt32(globalData.dtSBA.Rows[0][1])== 1 ? true : false;
                ch02.Checked = Convert.ToInt32(globalData.dtSBA.Rows[29][1]) == 1 ? true : false;
                ch04.Checked = Convert.ToInt32(globalData.dtSBA.Rows[1][1])== 1 ? true : false;
                ch05.Checked = Convert.ToInt32(globalData.dtSBA.Rows[2][1])== 1 ? true : false;
                ch07.Checked = Convert.ToInt32(globalData.dtSBA.Rows[3][1])== 1 ? true : false;
                ch09.Checked = Convert.ToInt32(globalData.dtSBA.Rows[4][1])== 1 ? true : false;
                ch10.Checked = Convert.ToInt32(globalData.dtSBA.Rows[5][1])== 1 ? true : false;
                ch12.Checked = Convert.ToInt32(globalData.dtSBA.Rows[6][1])== 1 ? true : false;
                ch16.Checked = Convert.ToInt32(globalData.dtSBA.Rows[7][1])== 1 ? true : false;
                ch20.Checked = Convert.ToInt32(globalData.dtSBA.Rows[8][1])== 1 ? true : false;
                ch21.Checked = Convert.ToInt32(globalData.dtSBA.Rows[9][1])== 1 ? true : false;
                ch23.Checked = Convert.ToInt32(globalData.dtSBA.Rows[10][1]) == 1 ? true : false;
                ch25.Checked = Convert.ToInt32(globalData.dtSBA.Rows[11][1]) == 1 ? true : false;
                ch26.Checked = Convert.ToInt32(globalData.dtSBA.Rows[12][1]) == 1 ? true : false;
                ch27.Checked = Convert.ToInt32(globalData.dtSBA.Rows[13][1]) == 1 ? true : false;
                ch29.Checked = Convert.ToInt32(globalData.dtSBA.Rows[14][1]) == 1 ? true : false;
                chST.Checked = Convert.ToInt32(globalData.dtSBA.Rows[15][1]) == 1 ? true : false;
                chCT.Checked = Convert.ToInt32(globalData.dtSBA.Rows[16][1]) == 1 ? true : false;
                chOT.Checked = Convert.ToInt32(globalData.dtSBA.Rows[18][1]) == 1 ? true : false;
                chNE.Checked = Convert.ToInt32(globalData.dtSBA.Rows[19][1]) == 1 ? true : false;
                chVS.Checked = Convert.ToInt32(globalData.dtSBA.Rows[21][1]) == 1 ? true : false;
                chAM.Checked = Convert.ToInt32(globalData.dtSBA.Rows[22][1]) == 1 ? true : false;
                chSP.Checked = Convert.ToInt32(globalData.dtSBA.Rows[17][1]) == 1 ? true : false;
                ch61.Checked = Convert.ToInt32(globalData.dtSBA.Rows[23][1]) == 1 ? true : false;
                ch62.Checked = Convert.ToInt32(globalData.dtSBA.Rows[24][1]) == 1 ? true : false;
                ch63.Checked = Convert.ToInt32(globalData.dtSBA.Rows[25][1]) == 1 ? true : false;
                ch64.Checked = Convert.ToInt32(globalData.dtSBA.Rows[26][1]) == 1 ? true : false;
                ch66.Checked = Convert.ToInt32(globalData.dtSBA.Rows[27][1]) == 1 ? true : false;
                ch69.Checked = Convert.ToInt32(globalData.dtSBA.Rows[28][1]) == 1 ? true : false;
            }
        }
    }
}
