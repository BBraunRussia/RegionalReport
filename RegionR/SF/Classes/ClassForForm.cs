using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace RegionR.SF
{
    public static class ClassForForm
    {
        public static void LoadDictionary(ComboBox combo, DataTable dt)
        {
            combo.DataSource = dt;

            if (dt == null)
            {
                MessageBox.Show("Отсутствуют данные в зависимых ячейках", "Нет данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            combo.ValueMember = dt.Columns[0].ColumnName;
            combo.DisplayMember = dt.Columns[1].ColumnName;
        }

        public static void CheckFilled(string text, string fieldName)
        {
            if (text == string.Empty)
                throw new NullReferenceException(string.Concat("Не заполнено поле \"", fieldName, "\""));
        }
    }
}
