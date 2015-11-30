using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace RegionR.other
{
    public partial class InputDialog : Form
    {
        public InputDialog()
        {
            InitializeComponent();
        }

        bool num;

        public InputDialog(String title, String label, bool OnlyNumeric)
        {
            InitializeComponent();
            label1.Text = label;
            this.Text = title;
            num = OnlyNumeric;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (textbox1.Text == String.Empty)
            {
                MessageBox.Show("Введите номер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            globalData.input = textbox1.Text;
            
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            globalData.input = "0";

            this.Close();
        }

        private void textbox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            validation(e);
        }

        private void validation(KeyPressEventArgs e)
        {
            if (num)
            {
                if (!(Char.IsDigit(e.KeyChar)))
                {
                    if (e.KeyChar != (char)Keys.Back)
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
