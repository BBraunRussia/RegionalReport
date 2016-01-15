using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary.SF
{
    public interface IHaveRegion
    {
        RealRegion RealRegion { get; set; }
        City City { get; set; }
    }
}
