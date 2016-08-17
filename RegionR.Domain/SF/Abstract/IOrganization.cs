using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegionReport.Domain.SF.Abstract
{
    public interface IOrganization
    {
        int ID { set; get; }
        string NumberSF { get; set; }
        string CrmID { get; set; }
        string Name { get; set; }
        string ShortName { get; set; }
        TypeOrg TypeOrg { get; set; }
        string Email { get; set; }
        string Website { get; set; }
        string Phone { get; set; }
        bool Deleted { get; set; }
        HistoryType Type { get; }
        int MainSpecID { get; set; }

        string KPP { get; set; }
        string PostIndex { get; set; }
        string Street { get; set; }
        int CityID { get; set; }
        string INN { get; set; }
        int RealRegionID { get; set; }

        string MachineGD { get; set; }
        string MachineGDF { get; set; }
        string MachineCRRT { get; set; }
        string Shift { get; set; }
        string PatientGD { get; set; }
        string PatientPD { get; set; }
        string PatientCRRT { get; set; }
        IOrganization ParentOrganization { get; set; }
    }
}
