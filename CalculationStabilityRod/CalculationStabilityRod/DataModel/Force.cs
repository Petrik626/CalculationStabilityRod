﻿using System.Runtime.InteropServices;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
    internal struct Force
    {
        public double Value { get; set; }

        public static implicit operator double(Force f)=>f.Value;
    }
}