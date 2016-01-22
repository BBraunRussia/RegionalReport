using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class FilteredDGV
    {
        private DataGridView _dgv;
        private string _filterColumnName;
        private string _filterValue;

        public FilteredDGV(DataGridView dgv)
        {
            _dgv = dgv;

            _filterColumnName = string.Empty;
            _filterValue = string.Empty;
        }

        public void Create()
        {
            if (_dgv.CurrentCell == null)
                return;

            _filterColumnName = _dgv.Columns[_dgv.CurrentCell.ColumnIndex].HeaderText;
            _filterValue = _dgv.CurrentCell.Value.ToString();

            Apply();
        }

        public void Delete()
        {
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (!row.Visible)
                    row.Visible = true;
            }

            _filterColumnName = string.Empty;
            _filterValue = string.Empty;
        }

        public void Apply()
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
    }
}
