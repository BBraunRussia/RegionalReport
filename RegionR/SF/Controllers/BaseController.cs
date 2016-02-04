using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.SF
{
    public class BaseOperations
    {
        private FilteredDGV _filtredDGV;
        private SortDGV _sortDGV;
        private SearchInDgv _seacher;

        public BaseOperations(DataGridView dgv)
        {
            _filtredDGV = new FilteredDGV(dgv);
            _sortDGV = new SortDGV(dgv);
            _seacher = new SearchInDgv(dgv);
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
    }
}
