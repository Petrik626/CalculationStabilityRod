using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;

namespace CalculationStabilityRod
{
    internal class ChangeLengthBalkEventArgs:EventArgs
    {
        private readonly double _oldLength;
        private readonly double _newLength;

        public ChangeLengthBalkEventArgs(double oldLength, double newLength)
        {
            _oldLength = oldLength;
            _newLength = newLength;
        }

        public double OldLength => _oldLength;
        public double NewLength => _newLength;
    }

    internal class ChangeMomentInertionBalkEventArgs:EventArgs
    {
        private readonly double _oldMoment;
        private readonly double _newMoment;

        public ChangeMomentInertionBalkEventArgs(double oldMoment, double newMoment)
        {
            _oldMoment = oldMoment;
            _newMoment = newMoment;
        }

        public double OldMoment => _oldMoment;
        public double NewMoment => _newMoment;
    }

    internal class ChangeLeftBorderConditionsEventArgs:EventArgs
    {
        private readonly BorderConditions _oldLeftBorderConditions;
        private readonly BorderConditions _newLeftBorderConditions;

        public ChangeLeftBorderConditionsEventArgs(BorderConditions oldleft, BorderConditions newLeft)
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
        public event EventHandler<ChangeLengthBalkEventArgs> ChangeLength;
        public event EventHandler<ChangeMomentInertionBalkEventArgs> ChangeMomentInertion;
        public event EventHandler<ChangeLeftBorderConditionsEventArgs> ChangeLeftBorderConditions;

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

        private void OnChageLength(ChangeLengthBalkEventArgs e)
        {
            ChangeLength?.Invoke(this, e);
        }

        private void OnChangeMomentInertion(ChangeMomentInertionBalkEventArgs e)
        {
            ChangeMomentInertion?.Invoke(this, e);
        }

        private void OnChangeLeftBorderConditions(ChangeLeftBorderConditionsEventArgs e)
        {
            ChangeLeftBorderConditions?.Invoke(this, e);
        }

        public double Length
        {
            set
            {
                double oldLength = Length;
                if(oldLength == value) { return; }

                SetValue(LengthProperty, value);
                OnChageLength(new ChangeLengthBalkEventArgs(oldLength, value));
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
                OnChangeMomentInertion(new ChangeMomentInertionBalkEventArgs(oldMoment, value));
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
                OnChangeLeftBorderConditions(new ChangeLeftBorderConditionsEventArgs(old, value));
            }
        }
        public BorderConditions RightBorderConditions { get => _rigthBorderConditions; }
        public static Balk Source { get => instance.Value; }
    } 
}
