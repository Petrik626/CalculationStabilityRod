using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;

namespace CalculationStabilityRod
{
    internal class LengthChangedBalkEventArgs:EventArgs
    {
        private readonly double _oldLength;
        private readonly double _newLength;

        public LengthChangedBalkEventArgs(double oldLength, double newLength)
        {
            _oldLength = oldLength;
            _newLength = newLength;
        }

        public double OldLength => _oldLength;
        public double NewLength => _newLength;
    }

    internal class MomentInertionChangedBalkEventArgs:EventArgs
    {
        private readonly double _oldMoment;
        private readonly double _newMoment;

        public MomentInertionChangedBalkEventArgs(double oldMoment, double newMoment)
        {
            _oldMoment = oldMoment;
            _newMoment = newMoment;
        }

        public double OldMoment => _oldMoment;
        public double NewMoment => _newMoment;
    }

    internal class LeftBorderConditionChangedEventArgs:EventArgs
    {
        private readonly BorderConditions _oldLeftBorderConditions;
        private readonly BorderConditions _newLeftBorderConditions;

        public LeftBorderConditionChangedEventArgs(BorderConditions oldleft, BorderConditions newLeft)
        {
            _oldLeftBorderConditions = oldleft;
            _newLeftBorderConditions = newLeft;
        }

        public BorderConditions OldLefrBorderConditions => _oldLeftBorderConditions;
        public BorderConditions NewLeftBorderConditions => _newLeftBorderConditions;
    }

    internal enum BorderConditions
    {
        HingedSupport = 1, Slider, FixedSupport, HingelessFixedSupport
    }

    [StructLayout(LayoutKind.Auto)]
    internal sealed class Balk:UIElement
    {
        public static readonly DependencyProperty LengthProperty;
        public static readonly DependencyProperty MomentInertionProperty;
        public static readonly DependencyProperty SpringsProperty;
        public static readonly DependencyProperty CriticalForceProperty;
        public static readonly DependencyProperty LeftBorderConditionsProperty;
        private static readonly Lazy<Balk> instance = new Lazy<Balk>(() => new Balk());
        private readonly BorderConditions _rigthBorderConditions;
        public event EventHandler<LengthChangedBalkEventArgs> LengthChanged;
        public event EventHandler<MomentInertionChangedBalkEventArgs> MomentInertionChanged;
        public event EventHandler<LeftBorderConditionChangedEventArgs> LeftBorderConditionChanged;

        private Balk()
        {
            _rigthBorderConditions = BorderConditions.HingedSupport;
        }

        static Balk()
        {
            FrameworkPropertyMetadata metadataLength = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataMoment = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataSprings = new FrameworkPropertyMetadata(new List<Spring>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataCriticalForce = new FrameworkPropertyMetadata(new Force(), FrameworkPropertyMetadataOptions.None);
            FrameworkPropertyMetadata metadataLeftBorderConditions = new FrameworkPropertyMetadata(BorderConditions.HingelessFixedSupport, FrameworkPropertyMetadataOptions.None);

            LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Balk), metadataLength);
            MomentInertionProperty = DependencyProperty.Register("MomentInertion", typeof(double), typeof(Balk), metadataMoment);
            SpringsProperty = DependencyProperty.Register("Springs", typeof(IList<Spring>), typeof(Balk), metadataSprings);
            CriticalForceProperty = DependencyProperty.Register("CriticalForce", typeof(Force), typeof(Balk), metadataCriticalForce);
            LeftBorderConditionsProperty = DependencyProperty.Register("LeftBorderConditions", typeof(BorderConditions), typeof(Balk), metadataLeftBorderConditions);
        }

        private void OnChageLength(LengthChangedBalkEventArgs e)
        {
            LengthChanged?.Invoke(this, e);
        }

        private void OnChangeMomentInertion(MomentInertionChangedBalkEventArgs e)
        {
            MomentInertionChanged?.Invoke(this, e);
        }

        private void OnChangeLeftBorderConditions(LeftBorderConditionChangedEventArgs e)
        {
            LeftBorderConditionChanged?.Invoke(this, e);
        }

        public double Length
        {
            set
            {
                double oldLength = Length;
                if(oldLength == value) { return; }

                SetValue(LengthProperty, value);
                OnChageLength(new LengthChangedBalkEventArgs(oldLength, value));
            }
            get => (double)GetValue(LengthProperty);
        }
        public double MomentInertion
        {
            set
            {
                double oldMoment = MomentInertion;
                if(oldMoment == value) { return; }

                SetValue(MomentInertionProperty, value);
                OnChangeMomentInertion(new MomentInertionChangedBalkEventArgs(oldMoment, value));
            }
            get => (double)GetValue(MomentInertionProperty);
        }
        public IList<Spring> Springs
        {
            set => SetValue(SpringsProperty, value);
            get => (IList<Spring>)(GetValue(SpringsProperty));
        }
        public Force CriticalForce
        {
            set => SetValue(CriticalForceProperty, value);
            get => (Force)(GetValue(CriticalForceProperty));
        }
        public BorderConditions LeftBorderConditions
        {
            get => (BorderConditions)GetValue(LeftBorderConditionsProperty);
            set
            {
                BorderConditions old = LeftBorderConditions;
                if (old == value) { return; }

                SetValue(LeftBorderConditionsProperty, value);
                OnChangeLeftBorderConditions(new LeftBorderConditionChangedEventArgs(old, value));
            }
        }
        public Force ExternalForce { get; set; } = 1000000.0;
        public double ElasticModulus { get; set; } = 100000.0;
        public double K => Math.Sqrt(ExternalForce / (ElasticModulus * MomentInertion));
        public BorderConditions RightBorderConditions { get => _rigthBorderConditions; }
        public static Balk Source { get => instance.Value; }
    } 
}
