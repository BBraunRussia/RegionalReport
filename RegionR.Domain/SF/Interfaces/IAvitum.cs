using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF.Interfaces
{
    public interface IAvitum
    {
        string MachineGD { get; set; }
        string MachineGDF { get; set; }
        string MachineCRRT { get; set; }
        string Shift { get; set; }
        string PatientGD { get; set; }
        string PatientPD { get; set; }
        string PatientCRRT { get; set; }
    }
}
