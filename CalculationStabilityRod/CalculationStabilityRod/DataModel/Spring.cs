using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
    internal sealed class Spring
    {
        static Spring() { CountSprings = 0; }
        public Spring() { CountSprings++; }

        public static int CountSprings { get; private set; }

        public int ID { get; set; }
        public double CoordsX { get; set; }
        public double Rigidity { get; set; }
    }
}
