using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegionR.addedit;
using RegionR.other;
using DataLayer;
using ClassLibrary;
using RegionReport.Domain;

namespace RegionR.Directories
{
    public partial class UserLpuAccess : Form
    {
        private LpuRRController _lpuRRController;
        private UserLpuRRListController _userLpuRRListController;

        private RegionRRList _regionRRList;
        private LpuRRList _lpuRRList;
        private UserLpuRRList _userLpuRRList;
        private UserList _userList;

        Color bbgreen3 = Color.FromArgb(115, 214, 186);
        Color bbgray4 = Color.FromArgb(150, 150, 150);

        public UserLpuAccess()
        {
            InitializeComponent();

            _regionRRList = RegionRRList.GetUniqueInstance();
            _lpuRRList = LpuRRList.GetUniqueInstance();
            _userLpuRRList = UserLpuRRList.GetUniqueInstance();
            _userList = UserList.GetUniqueInstance();

            _lpuRRController = new LpuRRController(_dgv1);
            _userLpuRRListController = new UserLpuRRListController(_dgv2);
            
            globalData.load = false;

            cbSDiv.SelectedIndex = 0;

            globalData.Div = cbSDiv.SelectedItem.ToString();

            globalData.load = true;

            fillRegion();
            fillUsers();
        }

        private void UserLpuAccess_Load(object sender, EventArgs e)
        {
            loadData1();
            loadData2();
        }
        
        private void fillRegion()
        {
            globalData.load = false;

            DataTable dt = _regionRRList.ToDataTable();

            cbRegions.DataSource = dt;
            cbRegions.DisplayMember = dt.Columns[1].ColumnName;
            cbRegions.ValueMember = dt.Columns[0].ColumnName;
            
            globalData.load = true;
        }

        private void fillUsers()
        {
            globalData.load = false;

            Sql sql1 = new Sql();
            DataTable dt1 = sql1.GetRecords("exec SelUsersAP @p1, @p2", cbSDiv.SelectedItem, cbRegions.SelectedValue);

            if (dt1 != null)
            {
                cbUsers.DataSource = dt1;
                cbUsers.DisplayMember = "user_name";
                cbUsers.ValueMember = "user_id";
            }

            globalData.load = true;
        }

        private void loadData1()
        {
            RegionRR regionRR = GetRegionRR();

            _dgv1 = _lpuRRController.ToDataGridView(regionRR);
        }

        private RegionRR GetRegionRR()
        {
            int idRegionRR;
            int.TryParse(cbRegions.SelectedValue.ToString(), out idRegionRR);
            return _regionRRList.GetItem(idRegionRR) as RegionRR;
        }

        private LpuRR GetLpuRR()
        {
            int idLPU;
            int.TryParse(_dgv1.Rows[_dgv1.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out idLPU);
            return _lpuRRList.GetItem(idLPU) as LpuRR;
        }

        private UserLpuRR GetUserLpuRR()
        {
            int idUserLpu;
            int.TryParse(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out idUserLpu);
            return _userLpuRRList.GetItem(idUserLpu) as UserLpuRR;
        }

        private User GetUser()
        {
            int idUser;
            int.TryParse(cbUsers.SelectedValue.ToString(), out idUser);
            return _userList.GetItem(idUser) as User;
        }

        private SDiv GetSDiv()
        {
            return (SDiv)(cbSDiv.SelectedIndex + 1);
        }

        private void loadData2()
        {
            User user = GetUser();
            RegionRR regionRR = GetRegionRR();
            SDiv sdiv = GetSDiv();
            
            _dgv2 = _userLpuRRListController.ToDataGridView(user, regionRR, sdiv);
        }

        private void cbSDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                globalData.Div = cbSDiv.SelectedItem.ToString();
                fillUsers();
                loadData2();
            }
        }

        private void cbRegions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                fillUsers();
                loadData1();
                loadData2();
            }
        }

        private void cbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (globalData.load)
            {
                loadData2();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RegionRR regionRR = GetRegionRR();

            LpuRR lpuRR = new LpuRR(regionRR);

            AddEditLPU aeLPU = new AddEditLPU(lpuRR);
            if (aeLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                loadData1();
        }

        private void _dgv1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.ColumnIndex < 0))
                return;

            LpuRR lpuRR = GetLpuRR();
            
            AddEditLPU aeLPU = new AddEditLPU(lpuRR);
            if (aeLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                loadData1();
        }

        private void LPU_Activated(object sender, EventArgs e)
        {
            if (globalData.update)
            {
                loadData2();
                globalData.update = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteLPU();
        }
        
        private void DeleteLPU()
        {
            if (globalData.UserAccess == 1)
            {
                LpuRR lpuRR = GetLpuRR();

                if (lpuRR.StatusLPU == StatusLPU.Активен)
                {
                    MessageBox.Show("Перед удалением нужно задать статус ЛПУ \"Неактивен\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_userLpuRRList.IsInList(lpuRR))
                {
                    MessageBox.Show("Перед удалением нужно удалить из списка \"ЛПУ пользователя\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string preDeleteMessage = CreatePreDeleteMessage(lpuRR);
                
                if (MessageBox.Show(preDeleteMessage, "Удаление ЛПУ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (_lpuRRList.Delete(lpuRR) == "1")
                        loadData1();
                    else
                        MessageBox.Show("Не удалось удалить, сначала необходимо удалить ЛПУ из списка \"ЛПУ пользователя\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string CreatePreDeleteMessage(LpuRR lpuRR)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Удалить ЛПУ?");
            sb.Append("Сокр. наим.: ");
            sb.AppendLine(lpuRR.Name);
            sb.Append("Полное наим.: ");
            sb.AppendLine(lpuRR.FullName);
            sb.Append("Номер: ");
            sb.AppendLine(lpuRR.ID.ToString());

            return sb.ToString();
        }

        private void btnAddUserLPU_Click(object sender, EventArgs e)
        {
            LpuRR lpuRR = GetLpuRR();

            if (lpuRR.StatusLPU == StatusLPU.Неактивен)
            {
                MessageBox.Show("Невозможно прикрепить ЛПУ к пользователю, ЛПУ имеет статус \"Неактивен\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            User user = GetUser();
            SDiv sdiv = GetSDiv();

            UserLpuRR userLpuRR = new UserLpuRR(user, lpuRR, sdiv);
            userLpuRR.Save(UserLogged.Get());

            loadData2();
        }
        
        private void _dgv2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            UserLpuRR userLpu = GetUserLpuRR();

            EditUserLPU editUserLPU = new EditUserLPU(userLpu);
            if (editUserLPU.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                loadData2();
        }

        private void btnDelUserLPU_Click(object sender, EventArgs e)
        {
            UserLpuRR userLpuRR = GetUserLpuRR();

            string res = _userLpuRRList.Delete(userLpuRR);

            if (res == "1")
                loadData2();
            else
                MessageBox.Show("Удаление невозможно, на ЛПУ разнесены продажи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnMoveSales_Click(object sender, EventArgs e)
        {
            MoveSales ms = new MoveSales(_dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells["ulpu_id"].Value.ToString(), _dgv2.Rows[_dgv2.SelectedCells[0].RowIndex].Cells["lpu_sname"].Value.ToString(), cbUsers.SelectedValue.ToString(), cbRegions.SelectedValue.ToString(), cbSDiv.SelectedItem.ToString());
            ms.ShowDialog();
        }

        private void _dgv2_Sorted(object sender, EventArgs e)
        {
            _userLpuRRListController.SetStyle();
        }

        private void _dgv1_Sorted(object sender, EventArgs e)
        {
            _lpuRRController.SetStyle();
        }
    }
}
