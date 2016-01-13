using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using RegionR.SF;

namespace RegionR
{
    public partial class FormPersonList : Form
    {
        private MyStatusStrip _myStatusStrip;
        private SearchInDgv _seacher;
        private Organization _organization;

        public FormPersonList(Organization organization = null)
        {
            InitializeComponent();

            _organization = organization;

            if (_organization != null)
                this.Text = string.Concat("Справочник: Персоны-SF, Организация: ", _organization.ShortName);

            _seacher = new SearchInDgv(dgv);
            _myStatusStrip = new MyStatusStrip(dgv, statusStrip1);
        }
        
        private void FormPersonList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            PersonList personList = PersonList.GetUniqueInstance();
            UserList userList = UserList.GetUniqueInstance();
            User user = userList.GetItem(globalData.UserID) as User;
            
            DataTable dt = (_organization != null) ? personList.ToDataTable(_organization) : (user.RoleSF == RolesSF.Администратор) ? personList.ToDataTable() : personList.ToDataTable(user);
            
            dgv.DataSource = dt;

            dgv.Columns[0].Visible = false;
            dgv.Columns[4].Width = 300;
            dgv.Columns[5].Width = 150;
        }

        private void NotImpliment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В процессе разработки", "Функция не реализована", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            _myStatusStrip.writeStatus();
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_organization == null)
            {
                Add();
            }
            else
            {
                Person person = new Person();
                person.Organization = _organization;
                Add(person);
            }
        }

        private void Add()
        {
            Person person = new Person();

            FormFirstStepAddPerson formFirstStepAddPerson = new FormFirstStepAddPerson(person);
            if (formFirstStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Add(person);
            }
        }

        private void Add(Person person)
        {
            FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
            if (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadData();
            }
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditPerson();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditPerson();
        }

        private void EditPerson()
        {
            Person person = GetPerson();

            FormAddPerson formAddPerson = new FormAddPerson(person);
            if (formAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadData();
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить персону?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                Person person = GetPerson();
                person.Delete();
                LoadData();
            }
        }

        private Person GetPerson()
        {
            int id;
            int.TryParse(dgv.Rows[dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            PersonList personList = PersonList.GetUniqueInstance();
            return personList.GetItem(id) as Person;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDGV();
        }

        public void RefreshDGV()
        {
            PersonList personList = PersonList.GetUniqueInstance();
            personList.Reload();

            LoadData();
        }

        private void dgv_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void sortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count == 0)
                return;

            int rowIndex = dgv.CurrentCell.RowIndex;
            int columnIndex = dgv.CurrentCell.ColumnIndex;

            DataGridViewColumn column = dgv.Columns[dgv.CurrentCell.ColumnIndex];
            System.ComponentModel.ListSortDirection sortDirection;

            if ((dgv.SortedColumn == null) || (dgv.SortedColumn != column))
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;
            else if (dgv.SortOrder == SortOrder.Ascending)
                sortDirection = System.ComponentModel.ListSortDirection.Descending;
            else
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;

            dgv.Sort(column, sortDirection);

            dgv.CurrentCell = dgv.Rows[rowIndex].Cells[columnIndex];
        }

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentCell == null)
                return;

            string columnName = dgv.Columns[dgv.CurrentCell.ColumnIndex].HeaderText;

            string value = dgv.CurrentCell.Value.ToString();

            ApplyFilter(columnName, value);
        }

        private void ApplyFilter(string columnName, string value)
        {
            foreach (DataGridViewRow row in dgv.Rows)
                row.Visible = (row.Cells[columnName].Value.ToString() == value);

            btnDeleteFilter.Visible = true;
        }

        private void btnDeleteFilter_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv.Rows)
                row.Visible = true;

            btnDeleteFilter.Visible = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Search();
            }
        }
        
        private void Search()
        {
            _seacher.Find(tbSearch.Text);
        }
    }
}
