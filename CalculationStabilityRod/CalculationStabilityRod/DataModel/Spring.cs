using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;

namespace CalculationStabilityRod.DataModel
{
    [StructLayout(LayoutKind.Auto)]
    internal sealed class Spring:UIElement
    {
        static Spring()
        {
            CountSprings = 0;

            FrameworkPropertyMetadata metadataCoords = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataRigidity = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataID = new FrameworkPropertyMetadata(new int(), FrameworkPropertyMetadataOptions.None);

            CoordsXProperty = DependencyProperty.Register("CoordsX", typeof(double), typeof(Spring), metadataCoords);
            RigidityProperty = DependencyProperty.Register("Rigidity", typeof(double), typeof(Spring), metadataRigidity);
            IDProperty = DependencyProperty.Register("ID", typeof(int), typeof(Spring), metadataID);
        }
        public Spring() { ID = CountSprings++; }

        public static readonly DependencyProperty CoordsXProperty;
        public static readonly DependencyProperty RigidityProperty;
        public static readonly DependencyProperty IDProperty;

        public static int CountSprings { get; private set; }

        public int ID
        {
            private set => SetValue(IDProperty, value);
            get => (int)GetValue(IDProperty);
        }
        public double CoordsX
        {
            set => SetValue(CoordsXProperty, value);
            get => (double)GetValue(CoordsXProperty);
        }
        public double Rigidity
        {
            set => SetValue(RigidityProperty, value);
            get => (double)GetValue(RigidityProperty);
        }
    }
}
