using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary.SF;
using System.Data;

namespace RegionR
{
    public abstract class BaseOperations
    {
        private FilteredDGV _filtredDGV;
        private SortDGV _sortDGV;
        private SearchInDgv _seacher;
        private DataGridView _dgv;

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
            DataTable dt = _dgv.DataSource as DataTable;

            CreateExcel excel = new CreateExcel(dt);
            excel.Show();
        }
    }
}
