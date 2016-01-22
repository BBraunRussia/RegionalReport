using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class SortDGV
    {
        private DataGridView _dgv;

        public SortDGV(DataGridView dgv)
        {
            _dgv = dgv;
        }

        public void Sort()
        {
            if (_dgv.SelectedCells.Count == 0)
                return;

            DataGridViewColumn column = _dgv.Columns[_dgv.CurrentCell.ColumnIndex];
            System.ComponentModel.ListSortDirection sortDirection = (_dgv.SortOrder == SortOrder.Ascending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending;

            _dgv.Sort(column, sortDirection);
        }
    }
}
