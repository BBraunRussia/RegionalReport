using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RegionR.SF
{
    public class SearchInDgv
    {
        private DataGridView _dgv;
        private string _prevText;
        private Point _prevCell;
        private string _currentText;

        public SearchInDgv(DataGridView dgv)
        {
            _dgv = dgv;
            _prevText = string.Empty;
            _prevCell = new Point(0, 0);
        }

        public void Find(string text)
        {
            _currentText = text.ToLower();

            if (IsNotValid())
                return;

            if (text != _prevText)
                Search(new Point(0, 0));
            else
                Search(_prevCell);
        }

        private bool IsNotValid()
        {
            return ((_dgv == null) || (string.IsNullOrEmpty(_currentText)));
        }

        private void Search(Point point)
        {
            DataGridViewCell cell = BeginSearch(point);

            if ((cell == null) && (point != new Point(0, 0)))
                cell = BeginSearch(new Point(0, 0));

            SetPrevValues(cell);

            if ((cell != null) && (cell.Visible))
                _dgv.CurrentCell = cell;
        }

        private DataGridViewCell BeginSearch(Point beginPoint)
        {
            for (int i = beginPoint.X; i < _dgv.Columns.Count; i++)
            {
                if (_dgv.Columns[i].Visible)
                {
                    for (int j = beginPoint.Y; j < _dgv.Rows.Count; j++)
                    {
                        DataGridViewCell cell = _dgv.Rows[j].Cells[i];
                        string cellText = cell.Value.ToString().ToLower();

                        if (cellText.Contains(_currentText))
                        {
                            return cell;
                        }
                    }
                }
            }

            return null;
        }

        private void SetPrevValues(DataGridViewCell cell)
        {
            int columnIndex = 0;
            int rowIndex = 0;

            if (cell != null)
            {
                columnIndex = cell.ColumnIndex;
                rowIndex = cell.RowIndex;

                if (rowIndex < _dgv.Rows.Count - 1)
                    rowIndex++;
                else if (columnIndex < _dgv.Columns.Count - 1)
                    columnIndex++;
            }

            _prevCell = new Point(columnIndex, rowIndex);
            _prevText = _currentText;
        }
    }
}
