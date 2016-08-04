using ClassLibrary;
using ClassLibrary.SF.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ClassLibrary.SF.Models;
using RegionR.Controllers;

namespace RegionR.SF.Controllers
{
    public class LPUController<T> : IController<T>
        where T: RegionRR
    {
        private DataGridView dgv;
        private LpuList lpuList;

        public LPUController()
        {
            dgv = new DataGridView();

            lpuList = new LpuList();
        }

        public DataGridView GetDataGridView(T filter)
        {
            ReLoad();

            dgv.DataSource = lpuList.ToDataTable(filter);
            dgv.Location = new Point(10, 142);
            dgv.Size = new Size(480, 175);
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.ReadOnly = true;
            dgv.RowHeadersVisible = false;

            return dgv;
        }

        public void ReLoad()
        {
            lpuList.ReLoad();
        }

        public DataGridView ToDataGridView()
        {
            throw new NotImplementedException();
        }

        public void DeleteFilter()
        {
            throw new NotImplementedException();
        }

        public void CreateFilter()
        {
            throw new NotImplementedException();
        }

        public void ApplyFilter()
        {
            throw new NotImplementedException();
        }

        public void Sort()
        {
            throw new NotImplementedException();
        }

        public void Search(string text)
        {
            throw new NotImplementedException();
        }

        public void ExportInExcel()
        {
            throw new NotImplementedException();
        }

        public void SetStyle()
        {
            throw new NotImplementedException();
        }
    }
}
