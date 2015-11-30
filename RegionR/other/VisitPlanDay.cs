using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Calendar;
using RegionR.addedit;
using ClassLibrary;

namespace RegionR.other
{
    public partial class VisitPlanDay : Form
    {
        int userID = 0;
        DateTime curDate;
        bool loadFirst = true;

        public VisitPlanDay(int userID, DateTime date)
        {
            InitializeComponent();

            curDate = date;
            this.userID = userID;
        }

        private void VisitPlanDay_Load(object sender, EventArgs e)
        {
            if (calendar1.ViewStart > curDate)
            {
                calendar1.ViewStart = curDate;
                calendar1.ViewEnd = curDate;
            }
            else
            {
                calendar1.ViewEnd = curDate;
                calendar1.ViewStart = curDate;
            }

            loadVisits();
        }

        private void VisitPlanDay_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
                loadVisits();

            if (loadFirst)
            {
                loadFirst = false;

                DataTable dt1 = new DataTable();
                dt1 = Visit.GetDataTableForDayByUser(userID, curDate);                

                if (dt1.Rows.Count == 0)
                    return;

                AddEditVisitPlanList aevpl = new AddEditVisitPlanList(userID, curDate);
                aevpl.ShowDialog();
            }
        }

        private void loadVisits()
        {
            DataTable dt1 = Visit.GetDataTableForDayByUser(userID, calendar1.ViewStart);

            calendar1.Items.Clear();

            foreach (DataRow row in dt1.Rows)
            {
                DateTime date1 = Convert.ToDateTime(row.ItemArray[2].ToString());
                DateTime date2 = Convert.ToDateTime(date1.Year.ToString() + "-" + date1.Month.ToString() + "-" + date1.Day.ToString() + " " + (date1.Hour + 1).ToString() + ":00");

                CalendarItem item = new CalendarItem(calendar1, date1, date2, row.ItemArray[6].ToString() + " - " + row.ItemArray[5].ToString(), row.ItemArray[0].ToString());
                if (row.ItemArray[3].ToString() == "Не выполнено")
                    item.BackgroundColor = Color.Red;
                if (row.ItemArray[3].ToString() == "Выполнено")
                    item.BackgroundColor = Color.Green;
                calendar1.Items.Add(item);
            }
        }

        private void calendar1_ItemDoubleClick(object sender, CalendarItemEventArgs e)
        {
            Visit visit1 = new Visit(Convert.ToInt32(userID), e.Item.StartDate);

            if (e.Item.ID == null)
            {
                e.Item.ID = "0";
            }
            if ((e.Item.ID == "0") && (!visit1.canEditPlan) && (!visit1.canEditFact))
            {               
                calendar1.Items.RemoveAt(calendar1.Items.Count - 1);
                MessageBox.Show("Невозможно добавить план в прошлое", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;               
            }

            AddEditVisitPlanDay aeVPD;            

            if (e.Item.ID != "0")
                aeVPD = new AddEditVisitPlanDay(new Visit(Convert.ToInt32(e.Item.ID), Convert.ToInt32(userID)));
            else
                aeVPD = new AddEditVisitPlanDay(visit1);

            if ((aeVPD.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) && (e.Item.ID == "0"))
                calendar1.Items.RemoveAt(calendar1.Items.Count - 1);          
        }

        private void VisitPlanDay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        private void calendar1_ItemDeleting(object sender, CalendarItemCancelEventArgs e)
        {
            Visit visit1 = new Visit(Convert.ToInt32(e.Item.ID), userID);

            if (!visit1.canEditPlan)
            {
                MessageBox.Show("Невозможно удалить план в прошлом", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            
            if ((visit1.CommRD != "") && (globalData.UserAccess > 4))
            {
                MessageBox.Show("С комментариями руководителя нельзя удалить план", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            else
            {
                visit1.Delete();
                globalData.update = true;
            }
        }
    }
}
