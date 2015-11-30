using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClassLibrary;

namespace RegionR.addedit
{
    public partial class RegionAddEdit : Form
    {
        Regions reg;
        bool cancelClose;

        public RegionAddEdit()
        {
            InitializeComponent();

            reg = new Regions();
        }

        public RegionAddEdit(Regions reg)
        {
            InitializeComponent();

            this.reg = reg;

            code.Text = reg.getCode();
            name.Text = reg.getName();
            nameRus.Text = reg.getNameRus();
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (isFill())
            {
                reg.save(code.Text, name.Text, nameRus.Text);
                globalData.update = true;
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Не удалось сохранить", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cancelClose = true;
            }
        }

        private bool isFill()
        {
            return (code.Text != string.Empty) && (name.Text != string.Empty) && (nameRus.Text != string.Empty);
        }

        private void RegionAddEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cancelClose)
            {
                e.Cancel = true;
                cancelClose = false;
            }
        }
    }
}
