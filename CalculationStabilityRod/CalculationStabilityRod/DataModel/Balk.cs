using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
    internal sealed class Balk
    {
        public double Length { get; set; } = 0.0;
        public double MomentInertion { get; set; } = 0.0;
        public IList<Spring> Springs { get; set; } = new List<Spring>();
    } 
}
