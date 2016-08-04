using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegionR.Controllers
{
    public interface IController<T> : IController
    {
        DataGridView GetDataGridView(T parametr);
    }
}
