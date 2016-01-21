using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class MainDGV
    {
        private DataGridView _dgv;

        public DataGridViewSelectedCellCollection SelectedCells { get { return _dgv.SelectedCells; } }

        public DataGridViewCell CurrentCell { get { return _dgv.CurrentCell; } }

        public MainDGV(DataGridView dgv)
        {
            _dgv = dgv;
        }

        public int GetID()
        {
            return _dgv.CurrentCell == null ? 0 : GetID(0, _dgv.CurrentCell.RowIndex);
        }

        public int GetID(int rowIndex)
        {
            return GetID(0, rowIndex);
        }

        public int GetCarID()
        {
            return _dgv.CurrentCell == null ? 0 : GetID(1, _dgv.CurrentCell.RowIndex);
        }

        public int GetCarID(int rowIndex)
        {
            return GetID(1, rowIndex);
        }

        private int GetID(int columnIndex, int rowIndex)
        {
            if (_dgv.CurrentCell == null)
            {
                MessageBox.Show("Перед действием необходимо выделить запись в таблице", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            return Convert.ToInt32(_dgv.Rows[rowIndex].Cells[columnIndex].Value);
        }

        public DataGridView GetDGV()
        {
            return _dgv;
        }
    }
}
