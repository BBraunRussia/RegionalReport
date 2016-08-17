using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF.Lists;
using ClassLibrary.SF.Entities;
using RegionReport.Domain;
using ClassLibrary;

namespace RegionR.SF
{
    public partial class FormCityList : Form
    {
        private RealRegionList _realRegionList;
        private CityList _cityList;

        private bool _isLoad;

        public FormCityList()
        {
            InitializeComponent();

            _realRegionList = RealRegionList.GetUniqueInstance();
            _cityList = CityList.GetUniqueInstance();
            _isLoad = false;

            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;

            if (user.RoleSF == RolesSF.Администратор)
            {
                btnAddCity.Enabled = true;
                btnDeleteCity.Enabled = true;
            }

            LoadRealRegion();
        }

        private void LoadRealRegion()
        {
            _isLoad = false;
            DataTable dt = _realRegionList.ToDataTable();

            cbRealRegion.DataSource = dt;
            cbRealRegion.ValueMember = dt.Columns[0].ColumnName;
            cbRealRegion.DisplayMember = dt.Columns[1].ColumnName;
            _isLoad = true;

            LoadData();
        }

        private void cbRealRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnDeleteCity_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить населенный пункт?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                BaseDictionary dic = GetItem();

                LpuList lpuList = new LpuList();
                LPU lpu = lpuList.GetItem(dic as City);

                if (lpu == null)
                {
                    _cityList.Delete(dic);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Удаление не возможно, так как данный населённый пункт используется в таблице \"Сопоставление ЛПУ\"", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex < 0) || (e.RowIndex < 0))
                return;

            EditCity();
        }

        private void EditCity()
        {
            BaseDictionary dic = GetItem();
            OpenAddCity(dic as City);
        }

        private BaseDictionary GetItem()
        {
            int id;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            return _cityList.GetItem(id);
        }
        
        private void btnAddCity_Click(object sender, EventArgs e)
        {
            RealRegion realRegion = GetRealRegion();

            City city = new City(realRegion);

            OpenAddCity(city);
        }

        private void OpenAddCity(City city)
        {
            FormAddCity formAddCity = new FormAddCity(city);
            if (formAddCity.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadData();
        }

        private void LoadData()
        {
            if (_isLoad)
            {
                RealRegion realRegion = GetRealRegion();
                dgv.DataSource = _cityList.ToDataTable(realRegion);
                dgv.Columns[0].Visible = false;
            }
        }

        private RealRegion GetRealRegion()
        {
            int idRealRegion;
            int.TryParse(cbRealRegion.SelectedValue.ToString(), out idRealRegion);
            return _realRegionList.GetItem(idRealRegion) as RealRegion;
        }
    }
}
