using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ClassLibraryBBAuto;

namespace ClassLibrary.SF
{
    public class CreateExcel
    {
        private DataTable _dt;

        public CreateExcel(DataTable dt)
        {
            _dt = dt;
        }

        public void Show()
        {
            ExcelDoc excel = new ExcelDoc();

            CreateHeader(excel);

            CreateBody(excel);

            excel.Show();
        }

        private void CreateHeader(ExcelDoc excel)
        {
            int i = 1;

            foreach (DataColumn column in _dt.Columns)
            {
                excel.setValue(1, i, column.ColumnName);

                i++;
            }
        }

        private void CreateBody(ExcelDoc excel)
        {
            int i = 2;

            foreach (DataRow row in _dt.Rows)
            {
                for (int j = 0; j < row.ItemArray.Count(); j++ )
                {
                    if (row.ItemArray[j].ToString() == string.Empty)
                        continue;

                    excel.setValue(i, (j + 1), row.ItemArray[j].ToString());
                }

                i++;
            }
        }
    }
}
