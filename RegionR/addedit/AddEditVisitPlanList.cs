using RegionR.other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataLayer;
using ClassLibrary;

namespace RegionR.addedit
{
    public partial class AddEditVisitPlanList : Form
    {
        int userID;
        DateTime date;
        GroupBox[] gb1;
        ComboBox[,] cb1;
        Label[,] lb1;
        TextBox[,] tb1;
        Button[,] btn1;
        CheckBox[] check1;
        DateTimePicker[] dateNew1;
        bool firstLoad = true;
        enum status { Planed = 1, Completed, NoCompleted };
        string[] tbCommentRD;
        Visit[] visit1;


        public AddEditVisitPlanList(int userID, DateTime date)
        {
            InitializeComponent();

            this.userID = userID;
            this.date = date;
        }

        private void fillComboBox(DataTable dt, ComboBox cb, string Display, string Value)
        {
            globalData.load = false;
            cb.DataSource = dt;
            cb.DisplayMember = Display;
            cb.ValueMember = Value;
            globalData.load = true;
        }

        private void AddEditVisitPlanList_Load(object sender, EventArgs e)
        {
            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelVisitPlanByUser @p1, @p2", userID, date);

            int y = 0, i = 0;

            visit1 = new Visit[dt1.Rows.Count];

            gb1 = new GroupBox[dt1.Rows.Count];
            cb1 = new ComboBox[dt1.Rows.Count, 3];
            lb1 = new Label[dt1.Rows.Count, 7];            
            tb1 = new TextBox[dt1.Rows.Count, 4];
            btn1 = new Button[dt1.Rows.Count, 2];
            check1 = new CheckBox[dt1.Rows.Count];
            dateNew1 = new DateTimePicker[dt1.Rows.Count];
            tbCommentRD = new string[dt1.Rows.Count];

            foreach (DataRow row in dt1.Rows)
            {
                visit1[i] = new Visit(Convert.ToInt32(row.ItemArray[0]), Convert.ToInt32(userID));

                gb1[i] = new GroupBox();

                gb1[i].Location = new Point(10, 40 + y);
                gb1[i].Size = new Size(807, 320);
                gb1[i].Name = "gb" + row[0].ToString();

                DateTime date2 = Convert.ToDateTime(row[2]);
                if (date2.Hour < 10)
                    gb1[i].Text = "0" + date2.Hour.ToString() + ":00";
                else
                    gb1[i].Text = date2.Hour.ToString() + ":00";

                lb1[i, 0] = new Label();
                lb1[i, 0].Location = new Point(8, 20);
                lb1[i, 0].Text = "Статус:";
                lb1[i, 0].Size = new Size(44, 13);
                gb1[i].Controls.Add(lb1[i, 0]);

                lb1[i, 1] = new Label();
                lb1[i, 1].Location = new Point(134, 20);
                lb1[i, 1].Text = "Название ЛПУ:";
                lb1[i, 1].Size = new Size(87, 13);
                gb1[i].Controls.Add(lb1[i, 1]);

                lb1[i, 2] = new Label();
                lb1[i, 2].Location = new Point(311, 20);
                lb1[i, 2].Text = "Вид деятельности:";
                lb1[i, 2].Size = new Size(102, 13);
                gb1[i].Controls.Add(lb1[i, 2]);

                lb1[i, 3] = new Label();
                lb1[i, 3].Location = new Point(8, 67);
                lb1[i, 3].Text = "1 - План действий:";
                lb1[i, 3].Size = new Size(101, 13);
                gb1[i].Controls.Add(lb1[i, 3]);

                lb1[i, 4] = new Label();
                lb1[i, 4].Location = new Point(271, 67);
                lb1[i, 4].Text = "2 - Выполнение:";
                lb1[i, 4].Size = new Size(88, 13);
                gb1[i].Controls.Add(lb1[i, 4]);

                lb1[i, 5] = new Label();
                lb1[i, 5].Location = new Point(535, 67);
                lb1[i, 5].Text = "3 - Последующие шаги:";
                lb1[i, 5].Size = new Size(125, 13);
                gb1[i].Controls.Add(lb1[i, 5]);

                lb1[i, 6] = new Label();
                lb1[i, 6].Location = new Point(8, 197);
                lb1[i, 6].Text = "Комментарии руководителя:";
                lb1[i, 6].Size = new Size(153, 13);
                gb1[i].Controls.Add(lb1[i, 6]);


                cb1[i, 0] = new ComboBox();
                cb1[i, 0].Location = new Point(10, 35);
                cb1[i, 0].Size = new Size(105, 21);
                cb1[i, 0].DropDownStyle = ComboBoxStyle.DropDownList;                
                cb1[i, 0].FormattingEnabled = true;
                cb1[i, 0].Name = "cmb" + i.ToString();

                DataTable dt2 = new DataTable();
                dt2 = sql1.GetRecords("exec SelVPStatus");
                fillComboBox(dt2, cb1[i, 0], "vpst_name", "vpst_id");
                cb1[i, 0].Enabled = canChangeStatus(visit1[i]);
                cb1[i, 0].SelectedValueChanged += new EventHandler(valid);
                gb1[i].Controls.Add(cb1[i, 0]);                

                cb1[i, 1] = new ComboBox();
                cb1[i, 1].Location = new Point(137, 35);
                cb1[i, 1].Size = new Size(171, 21);
                cb1[i, 1].DropDownStyle = ComboBoxStyle.DropDownList;

                DataTable dt3 = new DataTable();
                dt3 = sql1.GetRecords("exec SelULPUbyUserIDVisPlan @p1", userID);
                fillComboBox(dt3, cb1[i, 1], "ulpu_sname", "ulpu_id");
                cb1[i, 1].Enabled = !readlyOnlyEditPlan(visit1[i]);
                gb1[i].Controls.Add(cb1[i, 1]);

                cb1[i, 2] = new ComboBox();
                cb1[i, 2].Location = new Point(314, 35);
                cb1[i, 2].Size = new Size(171, 21);
                cb1[i, 2].DropDownStyle = ComboBoxStyle.DropDownList;

                DataTable dt4 = new DataTable();
                dt4 = sql1.GetRecords("exec SelActivity");
                fillComboBox(dt4, cb1[i, 2], "act_name", "act_id");
                cb1[i, 2].Enabled = !readlyOnlyEditPlan(visit1[i]);
                gb1[i].Controls.Add(cb1[i, 2]);

                tb1[i, 0] = new TextBox();
                tb1[i, 0].Location = new Point(10, 83);
                tb1[i, 0].Multiline = true;
                tb1[i, 0].MaxLength = 500;
                tb1[i, 0].Size = new Size(258, 110);
                tb1[i, 0].Text = row.ItemArray[6].ToString();
                tb1[i, 0].Name = "txb0" + i.ToString();
                tb1[i, 0].TextChanged += new EventHandler(valid);
                tb1[i, 0].MouseDoubleClick += new MouseEventHandler(tbPlan_MouseDoubleClick);

                tb1[i, 0].ReadOnly = readlyOnlyEditPlan(visit1[i]);
                gb1[i].Controls.Add(tb1[i, 0]);

                tb1[i, 1] = new TextBox();
                tb1[i, 1].Location = new Point(274, 83);
                tb1[i, 1].Multiline = true;
                tb1[i, 1].MaxLength = 500;
                tb1[i, 1].Size = new Size(258, 110);
                tb1[i, 1].Text = row.ItemArray[7].ToString();
                tb1[i, 1].Name = "txb1" + i.ToString();
                tb1[i, 1].TextChanged += new EventHandler(valid);
                tb1[i, 1].MouseDoubleClick += new MouseEventHandler(tbFact_MouseDoubleClick);
                tb1[i, 1].ReadOnly = readlyOnlyEditFact(visit1[i]);
                gb1[i].Controls.Add(tb1[i, 1]);

                tb1[i, 2] = new TextBox();
                tb1[i, 2].Location = new Point(538, 83);
                tb1[i, 2].Multiline = true;
                tb1[i, 2].MaxLength = 500;
                tb1[i, 2].Size = new Size(258, 110);
                tb1[i, 2].Text = row.ItemArray[8].ToString();
                tb1[i, 2].Name = "txb2" + i.ToString();
                tb1[i, 2].TextChanged += new EventHandler(valid);
                tb1[i, 2].MouseDoubleClick += new MouseEventHandler(tbNext_MouseDoubleClick);
                tb1[i, 2].ReadOnly = readlyOnlyEditFact(visit1[i]);
                gb1[i].Controls.Add(tb1[i, 2]);

                tb1[i, 3] = new TextBox();
                tb1[i, 3].Location = new Point(11, 213);
                tb1[i, 3].Multiline = true;
                tb1[i, 3].MaxLength = 500;
                tb1[i, 3].Size = new Size(500, 100);
                tb1[i, 3].Text = row.ItemArray[10].ToString();
                tb1[i, 3].TextChanged += new EventHandler(valid);
                tb1[i, 3].MouseDoubleClick += new MouseEventHandler(tbCom_MouseDoubleClick);
                tb1[i, 3].ReadOnly = readlyOnlyAddCommRD(visit1[i]);
                tbCommentRD[i] = row.ItemArray[10].ToString().Trim();
                gb1[i].Controls.Add(tb1[i, 3]);

                check1[i] = new CheckBox();
                check1[i].Location = new Point(538, 197);
                check1[i].Text = "Назначена новая встреча";
                check1[i].Size = new Size(157, 17);
                check1[i].Name = "chk" + i.ToString();
                if (row.ItemArray[14].ToString() != String.Empty)
                    check1[i].Checked = true;
                check1[i].CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
                gb1[i].Controls.Add(check1[i]);

                dateNew1[i] = new DateTimePicker();
                dateNew1[i].Location = new Point(538, 220);
                dateNew1[i].Name = "date" + i.ToString();
                if (row.ItemArray[14].ToString() != String.Empty)
                    dateNew1[i].Value = Convert.ToDateTime(row.ItemArray[14].ToString());
                else
                    dateNew1[i].Visible = false;
                dateNew1[i].Size = new Size(139, 20);
                dateNew1[i].ValueChanged += new EventHandler(dateNewVisit_ValueChanged);
                gb1[i].Controls.Add(dateNew1[i]);

                btn1[i, 0] = new Button();
                btn1[i, 0].Location = new Point(725, 290);
                btn1[i, 0].Size = new Size(75, 23);
                btn1[i, 0].Text = "Сохранить";
                btn1[i, 0].Name = "btn0" + i.ToString();
                btn1[i, 0].Click += new EventHandler(button1_Click);
                gb1[i].Controls.Add(btn1[i, 0]);

                btn1[i, 1] = new Button();
                btn1[i, 1].Location = new Point(690, 215);
                btn1[i, 1].Size = new Size(113, 23);
                btn1[i, 1].Text = "Создать визит";
                btn1[i, 1].Name = "btn1" + i.ToString();
                btn1[i, 1].Click += new EventHandler(button2_Click);
                if (row.ItemArray[14].ToString() == String.Empty)
                    btn1[i, 1].Visible = false;
                gb1[i].Controls.Add(btn1[i, 1]);

                y += 330;
                i++;
            }

            this.Controls.AddRange(gb1);
        }

        private bool readlyOnlyEditPlan(Visit visit1)
        {
            if (visit1.IsMyVisit(globalData.UserID))
                return !visit1.canEditPlan;
            else
                return true;
        }

        private bool readlyOnlyEditFact(Visit visit1)
        {
            if (visit1.IsMyVisit(globalData.UserID))
                return !visit1.canEditFact;
            else
                return true;
        }

        private bool readlyOnlyAddCommRD(Visit visit1)
        {
            return visit1.IsMyVisit(globalData.UserID);
        }

        private bool canChangeStatus(Visit visit1)
        {
            if (visit1.IsMyVisit(globalData.UserID))
                return visit1.canEditFact;
            else
                return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < visit1.Count(); i++)
            {
                saveVisitPlanDay(i);
            }

            globalData.update = true;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveVisitPlanDay(getNumberComponent("btn0", sender));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int num = getNumberComponent("btn1", sender);

            AddEditVisitPlanDay aeVPD = new AddEditVisitPlanDay(new Visit(visit1[num], dateNew1[num].Value));
            if (aeVPD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                saveVisitPlanDay(num);
        }

        private void saveVisitPlanDay(int numberVisit)
        {
            visit1[numberVisit].Save(globalData.UserID, Convert.ToInt32(cb1[numberVisit, 0].SelectedValue), Convert.ToInt32(cb1[numberVisit, 1].SelectedValue), Convert.ToInt32(cb1[numberVisit, 2].SelectedValue),
                    tb1[numberVisit, 0].Text, tb1[numberVisit, 1].Text, tb1[numberVisit, 2].Text, tb1[numberVisit, 3].Text, Convert.ToInt16(check1[numberVisit].Checked), dateNew1[numberVisit].Value);

            globalData.update = true;
        }

        private void AddEditVisitPlanList_Activated(object sender, EventArgs e)
        {
            if (firstLoad)
            {
                firstLoad = false;
                Sql sql1 = new Sql();

                DataTable dt1 = sql1.GetRecords("exec SelVisitPlanByUser @p1, @p2", userID, date);

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    cb1[i, 0].SelectedValue = dt1.Rows[i].ItemArray[11].ToString();
                    cb1[i, 1].SelectedValue = dt1.Rows[i].ItemArray[12].ToString();
                    cb1[i, 2].SelectedValue = dt1.Rows[i].ItemArray[13].ToString();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int num = getNumberComponent("chk", sender);

            if (check1[num].Checked)
            {
                dateNew1[num].Value = date;
                checkNewDateVisit(num);
                dateNew1[num].Visible = true;
                btn1[num, 1].Visible = true;
            }
            else
            {
                btnSave.Enabled = true;
                btn1[num, 0].Enabled = true;
                dateNew1[num].Visible = false;
                btn1[num, 1].Visible = false;
            }
        }

        private void dateNewVisit_ValueChanged(object sender, EventArgs e)
        {
            int num = getNumberComponent("date", sender);

            checkNewDateVisit(num);
        }

        private void checkNewDateVisit(int num)
        {
            bool valid = visit1[num].IsValidNewDate(dateNew1[num].Value);

            btn1[num, 0].Enabled = valid;
            btn1[num, 1].Enabled = valid;
            btnSave.Enabled = valid;
        }

        private int getNumberComponent(string name, object sender)
        {
            switch (name.Substring(0, 3))
            {
                case "dat":
                    {
                        DateTimePicker date1 = new DateTimePicker();
                        date1 = sender as DateTimePicker;
                        return Convert.ToInt32(date1.Name.Replace(name, ""));
                    }
                case "btn":
                    {
                        Button btn1 = new Button();
                        btn1 = sender as Button;
                        return Convert.ToInt32(btn1.Name.Replace(name, ""));
                    }
                case "chk":
                    {
                        CheckBox chk1 = new CheckBox();
                        chk1 = sender as CheckBox;
                        return Convert.ToInt32(chk1.Name.Replace(name, ""));
                    }
                case "txb":
                    {
                        TextBox tb1 = new TextBox();
                        tb1 = sender as TextBox;
                        return Convert.ToInt32(tb1.Name.Replace(name, ""));
                    }
                case "cmb":
                    {
                        ComboBox cb1 = new ComboBox();
                        cb1 = sender as ComboBox;
                        return Convert.ToInt32(cb1.Name.Replace(name, ""));
                    }
            }
            return -1;
        }

        private void valid(object sender, EventArgs e)
        {
            validated();
        }

        private void validated()
        {
            if (globalData.UserID == userID)
            {
                for (int i = 0; i < check1.Count(); i++)
                {
                    if ((cb1[i, 0].SelectedValue.ToString() == ((int)status.Planed).ToString()) && (tb1[i, 0].Text.Trim() != String.Empty))
                    {
                        btn1[i, 0].Enabled = true;
                    }
                    else if ((cb1[i, 0].SelectedValue.ToString() == ((int)status.Completed).ToString()) && (tb1[i, 0].Text.Trim() != String.Empty) && (tb1[i, 1].Text.Trim() != String.Empty) && (tb1[i, 2].Text.Trim() != String.Empty))
                    {
                        btn1[i, 0].Enabled = true;
                    }
                    else if ((cb1[i, 0].SelectedValue.ToString() == ((int)status.NoCompleted).ToString()) && (tb1[i, 0].Text.Trim() != String.Empty) && (tb1[i, 1].Text.Trim() != String.Empty) && (tb1[i, 2].Text.Trim() != String.Empty))
                    {
                        btn1[i, 0].Enabled = true;
                    }
                    else if (tb1[i, 3].Text.Trim() != String.Empty)
                    {
                        btn1[i, 0].Enabled = true;
                    }
                    else
                    {
                        btn1[i, 0].Enabled = false;
                    }
                }

                for (int i = 0; i < check1.Count(); i++)
                {
                    if ((cb1[i, 0].SelectedValue.ToString() == ((int)status.Planed).ToString()) && (tb1[i, 0].Text.Trim() != String.Empty))
                    {
                        btnSave.Enabled = true;
                    }
                    else if ((cb1[i, 0].SelectedValue.ToString() == ((int)status.Completed).ToString()) && (tb1[i, 0].Text.Trim() != String.Empty) && (tb1[i, 1].Text.Trim() != String.Empty) && (tb1[i, 2].Text.Trim() != String.Empty))
                    {
                        btnSave.Enabled = true;
                    }
                    else if ((cb1[i, 0].SelectedValue.ToString() == ((int)status.NoCompleted).ToString()) && (tb1[i, 0].Text.Trim() != String.Empty) && (tb1[i, 1].Text.Trim() != String.Empty) && (tb1[i, 2].Text.Trim() != String.Empty))
                    {
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = false;
                        break;
                    }
                }
            }
        }

        private void tbPlan_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Trim() != string.Empty)
            {
                ViewText vt = new ViewText(tb.Text, "План действий");
                vt.ShowDialog();
            }
        }

        private void tbFact_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Trim() != string.Empty)
            {
                ViewText vt = new ViewText(tb.Text, "Выполнение");
                vt.ShowDialog();
            }
        }

        private void tbNext_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Trim() != string.Empty)
            {
                ViewText vt = new ViewText(tb.Text, "Последующие шаги");
                vt.ShowDialog();
            }
        }

        private void tbCom_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text.Trim() != string.Empty)
            {
                ViewText vt = new ViewText(tb.Text, "Комментарии руководителя");
                vt.ShowDialog();
            }
        }
    }
}
