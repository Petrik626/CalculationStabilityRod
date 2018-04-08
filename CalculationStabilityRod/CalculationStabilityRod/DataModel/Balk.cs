using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
    internal sealed class Balk:UIElement
    {
        private static readonly Lazy<Balk> instance = new Lazy<Balk>(() => new Balk());
        private Balk() { }

        static Balk()
        {
            FrameworkPropertyMetadata metadataLength = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataMoment = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataSprings = new FrameworkPropertyMetadata(new List<Spring>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);

            LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Balk), metadataLength);
            MomentInertionProperty = DependencyProperty.Register("MomentInertion", typeof(double), typeof(Balk), metadataMoment);
            SpringsProperty = DependencyProperty.Register("Springs", typeof(IList<Spring>), typeof(Balk), metadataSprings);
        }

        public static readonly DependencyProperty LengthProperty;
        public static readonly DependencyProperty MomentInertionProperty;
        public static readonly DependencyProperty SpringsProperty;

        public double Length
        {
            set => SetValue(LengthProperty, value);
            get => (double)GetValue(LengthProperty);
        }
        public double MomentInertion
        {
            set => SetValue(MomentInertionProperty, value);
            get => (double)GetValue(MomentInertionProperty);
        }
        public IList<Spring> Springs
        {
            set => SetValue(SpringsProperty, value);
            get => (IList<Spring>)(GetValue(SpringsProperty));
        }
        public static Balk Source { get => instance.Value; }
    } 
}
