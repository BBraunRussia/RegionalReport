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
        private Dictionary<string, string> _filter;

        public FilteredDGV(DataGridView dgv)
        {
            _dgv = dgv;

            _filter = new Dictionary<string, string>();
        }

        public void Create()
        {
            if (_dgv.CurrentCell == null)
                return;

            if (!(_filter.ContainsKey(_dgv.Columns[_dgv.CurrentCell.ColumnIndex].HeaderText)))
            {
                _filter.Add(_dgv.Columns[_dgv.CurrentCell.ColumnIndex].HeaderText, _dgv.CurrentCell.Value.ToString());
                Apply();
            }
        }

        public void Delete()
        {
            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (!row.Visible)
                    row.Visible = true;
            }

            _filter.Clear();
        }

        public void Apply()
        {
            if (_filter.Count == 0)
                return;

            int rowIndex = _dgv.CurrentCell.RowIndex;
            int columnIndex = _dgv.CurrentCell.ColumnIndex;

            _dgv.CurrentCell = null;

            foreach (DataGridViewRow row in _dgv.Rows)
                row.Visible = true;

            foreach (DataGridViewRow row in _dgv.Rows)
            {
                foreach (var item in _filter.Keys)
                    row.Visible = ((_filter.ContainsValue(row.Cells[item.ToString()].Value.ToString())) && (row.Visible));
            }

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
