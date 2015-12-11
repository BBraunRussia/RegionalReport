using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LpuList : IDataTable
    {
        private List<LPU> _list;

        public LpuList()
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            _list = organizationList.List;
        }

        public DataTable ToDataTable()
        {
            return (_list.Count == 0) ? null : _list.Select(item => item.GetRow()).CopyToDataTable();
        }
    }
}
