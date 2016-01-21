using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class MyStatusStrip
    {
        private DataGridView _dgv;
        private StatusStrip _st;

        public MyStatusStrip(DataGridView dgv, StatusStrip st)
        {
            _dgv = dgv;
            _st = st;
        }

        public void writeStatus()
        {
            Clear();
            Add("Количество: " + RowsCount().ToString());
        }

        private int RowsCount()
        {
            int count = 0;

            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Visible)
                    count++;
            }

            return count;
        }

        public void WriteCountSelectCell()
        {
            try
            {
                Clear();

                if (_dgv.SelectedCells.Count > 1)
                {
                    Double sum = 0;
                    foreach (DataGridViewCell cell in _dgv.SelectedCells)
                    {
                        string value = cell.Value.ToString().Replace('.', ',').Trim();

                        if (!value.Any(c => char.IsLetter(c)))
                        {
                            double valueDouble = 0;
                            double.TryParse(value, out valueDouble);
                            sum += valueDouble;
                        }
                    }
                    if (sum != 0)
                    {
                        Add("     Сумма: " + (Math.Round(sum, 2).ToString()));
                        Add("     Количество: " + _dgv.SelectedCells.Count.ToString());
                        Add("Среднее: " + (Math.Round(sum / _dgv.SelectedCells.Count, 2)).ToString());
                    }
                    else
                    {
                        Add("Количество: " + _dgv.SelectedCells.Count.ToString());
                    }
                }
            }
            catch
            {
                Clear();
            }
        }

        private void Clear()
        {
            _st.Items.Clear();
        }

        private void Add(String str)
        {
            _st.Items.Add(str);
            _st.Items[_st.Items.Count - 1].Alignment = ToolStripItemAlignment.Right;
        }
    }
}
