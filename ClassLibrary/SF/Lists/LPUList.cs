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

            var list = _list.Where(item => userRightList2.Contains(item.LpuRR.RegionRR)).ToList();
            return CreateTable(list);
        }

        public DataTable ToDataTable()
        {
            return CreateTable(_list);
        }

        private DataTable CreateTable(List<LPU> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("Номер SF");
            dt.Columns.Add("Название организации");
            dt.Columns.Add("Тип");
            dt.Columns.Add("ИНН");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("Город");
            dt.Columns.Add("Сопоставленное ЛПУ-RR");
            dt.Columns.Add("Регион RR");

            foreach (LPU lpu in list)
                dt.Rows.Add(lpu.GetRow());

            return dt;
        }

        public LPU GetItem(int idLPU)
        {
            var list = _list.Where(item => item.ID == idLPU);
            return (list.Count() == 0) ? null : list.First();
        }

        public LPU GetItem(LpuRR lpuRR)
        {
            var list = _list.Where(item => item.LpuRR == lpuRR);
            return (list.Count() == 0) ? null : list.First();
        }
    }
}
