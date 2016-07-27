using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using ClassLibrary.SF.Common;

namespace RegionR
{
    public abstract class BaseOperations
    {
        private FilteredDGV _filtredDGV;
        private SortDGV _sortDGV;
        private SearchInDgv _seacher;
        protected DataGridView _dgv;

        public BaseOperations(DataGridView dgv)
        {
            _filtredDGV = new FilteredDGV(dgv);
            _sortDGV = new SortDGV(dgv);
            _seacher = new SearchInDgv(dgv);

            _dgv = dgv;
        }

        public void CreateFilter()
        {
            _filtredDGV.Create();
        }

        public void DeleteFilter()
        {
            _filtredDGV.Delete();
        }

        public void ApplyFilter()
        {
            _filtredDGV.Apply();
        }

        public void Sort()
        {
            _sortDGV.Sort();
        }

        public void Search(string text)
        {
            _seacher.Find(text);
        }

        public void ExportInExcel()
        {
            DataTable dt = CreateDataTable();

            foreach (DataGridViewRow row in _dgv.Rows)
            {
                if (row.Visible)
                {
                    object[] items = new object[row.Cells.Count];

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        items[cell.ColumnIndex] = cell.Value.ToString();
                    }

                    dt.Rows.Add(items);
                }
            }

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();// _dgv.DataSource as DataTable;

            foreach (DataGridViewColumn column in _dgv.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }

            return dt;
        }
    }
}
