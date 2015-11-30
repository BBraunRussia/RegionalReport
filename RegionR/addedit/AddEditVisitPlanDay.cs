using RegionR.other;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace RegionR.addedit
{
    public partial class AddEditVisitPlanDay : Form
    {
        Visit visit1;

        enum status { Planed = 1, Completed, NoCompleted};

        public AddEditVisitPlanDay(Visit visit1)
        {
            InitializeComponent();

            this.visit1 = visit1;
            
            DataTable dt1 = new DataTable();

            dt1 = visit1.GetULPUList();
            fillComboBox(dt1, cbULPU, "ulpu_sname", "ulpu_id");

            dt1 = Activity.getDataTable();
            fillComboBox(dt1, cbActivity, "act_name", "act_id");

            globalData.load = false;
            dt1 = VisitPlanStatus.getDataTable();
            fillComboBox(dt1, cbStatus, "vpst_name", "vpst_id");
            globalData.load = true;

            cbULPU.SelectedValue = visit1.IdULPU;
            tbPlan.Text = visit1.Plan;
            tbFact.Text = visit1.Fact;
            tbNext.Text = visit1.Next;
            cbActivity.SelectedValue = visit1.IdActivity;
            commRD.Text = visit1.CommRD;
            cbStatus.SelectedValue = (int)visit1.Status;

            if (visit1.HaveNewVisitDate)
            {
                enabledNewVisit.Checked = true;
                dateNewVisit.Value = visit1.NewVisitDate;
            }

            setComboBoxEnabled();           
            
            setTextBoxReadlyOnly();
        }

        private void setComboBoxEnabled()
        {
            if (visit1.IsMyVisit(globalData.UserID))
            {
                cbStatus.Enabled = visit1.canEditFact;
                cbULPU.Enabled = visit1.canEditPlan;
                cbActivity.Enabled = visit1.canEditPlan;
            }
            else
            {
                cbStatus.Enabled = true;
                cbULPU.Enabled = false;
                cbActivity.Enabled = false;
            }            
        }

        private void setTextBoxReadlyOnly()
        {
            if (visit1.IsMyVisit(globalData.UserID))
            {
                tbPlan.ReadOnly = !visit1.canEditPlan;
                tbFact.ReadOnly = !visit1.canEditFact;
                tbNext.ReadOnly = !visit1.canEditFact;                

                commRD.ReadOnly = true;
            }
            else
            {
                tbPlan.ReadOnly = true;
                tbNext.ReadOnly = true;
                tbFact.ReadOnly = true;
                commRD.ReadOnly = false;
            }            
        }

        private void fillComboBox(DataTable dt, ComboBox cb, string Display, string Value)
        {
            globalData.load = false;
            cb.DataSource = dt;
            cb.DisplayMember = Display;
            cb.ValueMember = Value;
            globalData.load = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveVisitPlanDay();

            Close();
        }

        private void createNewVisit_Click(object sender, EventArgs e)
        {
            AddEditVisitPlanDay aeVPD = new AddEditVisitPlanDay(new Visit(visit1, dateNewVisit.Value));

            if (aeVPD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                saveVisitPlanDay();
        }

        private void saveVisitPlanDay()
        {
            if (!visit1.IsVacantTimeAndSetNewTime())
            {
                MessageBox.Show("Нет доступного времени на эту дату", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            visit1.Save(globalData.UserID, Convert.ToInt32(cbStatus.SelectedValue), Convert.ToInt32(cbULPU.SelectedValue), Convert.ToInt32(cbActivity.SelectedValue), 
                tbPlan.Text, tbFact.Text, tbNext.Text, commRD.Text, Convert.ToInt16(enabledNewVisit.Checked), dateNewVisit.Value);

            globalData.update = true;
        }

        private void validText(object sender, EventArgs e)
        {
            validated();
        }

        private void validated()
        {
            if ((visit1.IsMyVisit(globalData.UserID)) && (globalData.load))
            {
                if ((cbStatus.SelectedValue.ToString() == ((int)status.Planed).ToString()) && (tbPlan.Text.Trim() != String.Empty))
                {
                    btnSave.Enabled = true;
                }
                else if ((cbStatus.SelectedValue.ToString() == ((int)status.Completed).ToString()) && (tbPlan.Text.Trim() != String.Empty) && (tbFact.Text.Trim() != String.Empty) && (tbNext.Text.Trim() != String.Empty))
                {
                    btnSave.Enabled = true;
                }
                else if ((cbStatus.SelectedValue.ToString() == ((int)status.NoCompleted).ToString()) && (tbPlan.Text.Trim() != String.Empty) && (tbFact.Text.Trim() != String.Empty) && (tbNext.Text.Trim() != String.Empty))
                {
                    btnSave.Enabled = true;
                }
                else if (commRD.Text.Trim() != String.Empty)
                {
                    btnSave.Enabled = true;
                }
                else
                    btnSave.Enabled = false;

                changeVisibleTip();
            }
        }

        private void changeVisibleTip()
        {
            if (cbStatus.SelectedValue.ToString() == ((int)status.Planed).ToString())
                tip.Visible = false;
            else
                tip.Visible = isFactOrNextEmpty();
        }

        private bool isFactOrNextEmpty()
        {
            return (tbFact.Text == string.Empty) || (tbNext.Text == string.Empty);
        }

        private void enabledNewVisit_CheckedChanged(object sender, EventArgs e)
        {
            if (enabledNewVisit.Checked)
            {
                dateNewVisit.Value = visit1.DateVisit;
                checkNewDateVisit();
                dateNewVisit.Visible = true;
                createNewVisit.Visible = true;
            }
            else
            {
                btnSave.Enabled = true;
                dateNewVisit.Visible = false;
                createNewVisit.Visible = false;
            }
        }

        private void dateNewVisit_ValueChanged(object sender, EventArgs e)
        {
            checkNewDateVisit();
        }

        private void checkNewDateVisit()
        {
            bool valid = visit1.IsValidNewDate(dateNewVisit.Value);

            createNewVisit.Enabled = valid;
            btnSave.Enabled = valid;
        }

        private void tbPlan_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showTextInViewTextForm("План действий", tbPlan.Text);
        }

        private void tbFact_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showTextInViewTextForm("Выполнение", tbFact.Text);
        }

        private void tbNext_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showTextInViewTextForm("Последующие шаги", tbNext.Text);
        }

        private void tbCom_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showTextInViewTextForm("Комментарии руководителя", commRD.Text);
        }

        private void showTextInViewTextForm(string title, string text)
        {
            ViewText vt = new ViewText(title, text);
            vt.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
