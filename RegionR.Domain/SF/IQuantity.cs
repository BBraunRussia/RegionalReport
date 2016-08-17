using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public interface IQuantity
    {
        int BedsTotal { get; }
        int BedsIntensiveCare { get; }
        int BedsSurgical { get; }
        int BedsOperating { get; }
        int GDMachine { get; }
        int GDFMachine { get; }
        int CRRTMachine { get; }
        int Shift { get; }
        int GDPatient { get; }
        int PDPatient { get; }
        int CRRTPatient { get; }
    }
}
