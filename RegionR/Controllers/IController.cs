using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR
{
    public interface IController
    {
        DataGridView ToDataGridView();
        void ReLoad();
    }
}
