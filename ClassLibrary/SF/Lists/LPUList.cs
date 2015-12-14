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

        public DataTable ToDataTable(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            var userRightList2 = userRightList.ToList(user);

            var list = _list.Where(item => userRightList2.Contains(item.LpuRR.RegionRR));
            return (list.Count() == 0) ? null : list.Select(item => item.GetRow()).CopyToDataTable();
        }

        public DataTable ToDataTable()
        {
            return (_list.Count == 0) ? null : _list.Select(item => item.GetRow()).CopyToDataTable();
        }

        public LPU GetItem(int idLPU)
        {
            var list = _list.Where(item => item.ID == idLPU);
            return (list.Count() == 0) ? null : list.First();
        }
    }
}
