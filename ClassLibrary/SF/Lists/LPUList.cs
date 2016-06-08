using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ClassLibrary.SF
{
    public class LpuList
    {
        private List<LPU> _list;
        private List<LPU> _listBranch;
        private List<OtherOrganization> _listOther;

        public LpuList()
        {
            InitLists();
        }

        private void InitLists()
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            _list = organizationList.ListLpu;
            _listBranch = organizationList.ListBranch;
            _listOther = organizationList.ListOther;
        }

        public DataTable ToDataTable(User user = null)
        {
            if (user == null)
                return CreateTable(_list, _listOther);

            var list = GetList(user);
            var listOther = GetListOther(_listOther, user);
            return CreateTable(list, listOther);
        }

        public DataTable ToDataTableWithBranch(User user = null)
        {
            if (user == null)
                return ToDataTableWithBranch(_list, _listOther);

            var list = GetList(user);
            return ToDataTableWithBranch(list, _listOther);
        }

        private List<LPU> GetList(User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            var userRightList2 = userRightList.ToList(user);

            return _list.Where(item => userRightList2.Contains(item.RealRegion.RegionRR)).ToList();
        }

        private List<OtherOrganization> GetListOther(List<OtherOrganization> listOther, User user)
        {
            UserRightList userRightList = UserRightList.GetUniqueInstance();
            var userRightList2 = userRightList.ToList(user);

            return listOther.Where(item => userRightList2.Contains(item.RealRegion.RegionRR)).ToList();
        }

        private DataTable ToDataTableWithBranch(List<LPU> list, List<OtherOrganization> listOther)
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();

            List<LPU> listNew = new List<LPU>();
            
            foreach (Organization item in list)
            {
                listNew.Add(item as LPU);

                var listBranch = organizationList.GetBranchList(item);
                foreach (Organization itemBranch in listBranch)
                    listNew.Add(itemBranch as LPU);
            }

            return CreateTable(listNew, listOther);
        }

        private DataTable CreateTable(List<LPU> list, List<OtherOrganization> listOther)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RR ID", typeof(int));
            dt.Columns.Add("SF ID");
            dt.Columns.Add("Название организации");
            dt.Columns.Add("Тип");
            dt.Columns.Add("ИНН");
            dt.Columns.Add("Регион России");
            dt.Columns.Add("Город");
            dt.Columns.Add("Сопоставленное ЛПУ-RR");
            dt.Columns.Add("Сопоставленное ЛПУ-RR 2");
            dt.Columns.Add("Регион RR");
            
            foreach (LPU lpu in list)
                dt.Rows.Add(lpu.GetRow());

            if (listOther != null)
            {
                foreach (OtherOrganization other in listOther)
                    dt.Rows.Add(other.GetRow());
            }

            return dt;
        }

        public LPU GetItem(int idLPU)
        {
            var list = _list.Where(item => item.ID == idLPU);
            return (list.Count() == 0) ? null : list.First();
        }

        public LPU GetItem(LpuRR lpuRR)
        {
            var list = _list.Where(item => ((item.LpuRR == lpuRR) || (item.LpuRR2 == lpuRR)));
            var listBranch = _listBranch.Where(item => ((item.LpuRR == lpuRR) || (item.LpuRR2 == lpuRR)));
            return ((list.Count() == 0) && (listBranch.Count() == 0)) ? null : (list.Count() != 0) ? list.First() : listBranch.First();
        }

        public LPU GetItem(City city)
        {
            var list = _list.Where(item => item.City == city);
            return (list.Count() == 0) ? null : list.First();
        }

        public bool IsInList(string inn)
        {
            return _list.Exists(item => item.INN == inn);
        }

        public void ReLoad()
        {
            OrganizationList organizationList = OrganizationList.GetUniqueInstance();
            organizationList.Reload();

            InitLists();
        }
    }
}
