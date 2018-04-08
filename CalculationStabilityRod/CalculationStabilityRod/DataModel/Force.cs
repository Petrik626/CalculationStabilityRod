using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
    internal struct Force
    {
        public double ModuleForce { get; set; }
    }
}
