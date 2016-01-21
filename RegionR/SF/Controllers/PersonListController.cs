using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibrary.SF;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class PersonListController
    {
        private PersonList _personList;
        private Organization _organization;
        private User _user;
        private DataGridView _dgv;
        private SearchInDgv _seacher;

        string _filterColumnName;
        string _filterValue;

        public PersonListController(DataGridView dgv, Organization organization)
        {
            _organization = organization;
            _dgv = dgv;

            _seacher = new SearchInDgv(_dgv);
            _personList = PersonList.GetUniqueInstance();
            _user = UserLogged.Get();

            _filterColumnName = string.Empty;
            _filterValue = string.Empty;
        }

        public DataGridView ToDataGridView()
        {
            DataTable dt = (_organization != null) ? _personList.ToDataTable(_organization) : (_user.RoleSF == RolesSF.Администратор) ? _personList.ToDataTable() : _personList.ToDataTable(_user);

            _dgv.DataSource = dt;

            _dgv.Columns[0].Visible = false;
            _dgv.Columns[4].Width = 300;
            _dgv.Columns[5].Width = 150;
            
            return _dgv;
        }

        public void ReLoad()
        {
            _personList.Reload();
        }

        public bool Add()
        {
            if (_organization == null)
            {
                return AddPerson();
            }
            else
            {
                Person person = new Person();
                person.Organization = _organization;
                return AddPerson(person);
            }
        }

        private bool AddPerson()
        {
            Person person = new Person();

            FormFirstStepAddPerson formFirstStepAddPerson = new FormFirstStepAddPerson(person);
            if (formFirstStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return AddPerson(person);
            }

            return false;
        }

        private bool AddPerson(Person person)
        {
            FormSecondStepAddPerson formSecondStepAddPerson = new FormSecondStepAddPerson(person);
            return (formSecondStepAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }

        public void Sort()
        {
            if (_dgv.SelectedCells.Count == 0)
                return;
            
            DataGridViewColumn column = _dgv.Columns[_dgv.CurrentCell.ColumnIndex];
            System.ComponentModel.ListSortDirection sortDirection = (_dgv.SortOrder == SortOrder.Ascending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending;
            /*
            if ((_dgv.SortedColumn == null) || (_dgv.SortedColumn != column))
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;
            else if (_dgv.SortOrder == SortOrder.Ascending)
                sortDirection = System.ComponentModel.ListSortDirection.Descending;
            else
                sortDirection = System.ComponentModel.ListSortDirection.Ascending;
            */
            _dgv.Sort(column, sortDirection);
        }

        public void Search(string text)
        {
            _seacher.Find(text);
        }

        public void CreateFilter()
        {
            _filterColumnName = _dgv.Columns[_dgv.CurrentCell.ColumnIndex].HeaderText;
            _filterValue = _dgv.CurrentCell.Value.ToString();

            ApplyFilter();
        }

        public void DeleteFilter()
        {
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (!row.Visible)
                    row.Visible = true;
            }

            _filterColumnName = string.Empty;
            _filterValue = string.Empty;
        }

        public void ApplyFilter()
        {
            if (_filterColumnName == string.Empty)
                return;

            int rowIndex = _dgv.CurrentCell.RowIndex;
            int columnIndex = _dgv.CurrentCell.ColumnIndex;

            _dgv.CurrentCell = null;

            foreach (DataGridViewRow row in _dgv.Rows)
                row.Visible = (row.Cells[_filterColumnName].Value.ToString() == _filterValue);

            if (!_dgv.Rows[rowIndex].Cells[columnIndex].Visible)
            {
                foreach (DataGridViewRow row in _dgv.Rows)
                {
                    if (row.Visible)
                    {
                        _dgv.CurrentCell = row.Cells[columnIndex];
                        return;
                    }
                }
            }

            _dgv.CurrentCell = _dgv.Rows[rowIndex].Cells[columnIndex];
        }

        public bool DeletePerson()
        {
            if (MessageBox.Show("Вы действительно хотите удалить персону?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                Person person = GetPerson();
                person.Delete();
                return true;
            }

            return false;
        }

        public bool EditPerson()
        {
            Person person = GetPerson();

            FormAddPerson formAddPerson = new FormAddPerson(person);
            return (formAddPerson.ShowDialog() == System.Windows.Forms.DialogResult.OK);
        }

        private Person GetPerson()
        {
            int id;
            int.TryParse(_dgv.Rows[_dgv.CurrentCell.RowIndex].Cells[0].Value.ToString(), out id);

            return _personList.GetItem(id) as Person;
        }
    }
}
