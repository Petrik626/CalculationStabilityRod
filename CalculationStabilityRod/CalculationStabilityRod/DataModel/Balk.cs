using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CalculationStabilityRod.DataModel
{
    internal class LengthBalkChangedEventArgs : EventArgs
    {
        private readonly double _oldLength;
        private readonly double _newLength;

        public LengthBalkChangedEventArgs(double oldLength, double newLength)
        {
            _oldLength = oldLength;
            _newLength = newLength;
        }

        public double OldLength => _oldLength;
        public double NewLength => _newLength;
    }

    internal class MomentInertionBalkChangedEventArgs : EventArgs
    {
        private readonly double _oldMoment;
        private readonly double _newMoment;

        public MomentInertionBalkChangedEventArgs(double oldMoment, double newMoment)
        {
            _oldMoment = oldMoment;
            _newMoment = newMoment;
        }

        public double OldMoment => _oldMoment;
        public double NewMoment => _newMoment;
    }

    internal class BorderConditionChangedEventArgs:EventArgs
    {
        private readonly BorderConditions _oldBorderConditions;
        private readonly BorderConditions _newBorderConditions;

        public BorderConditionChangedEventArgs(BorderConditions old, BorderConditions newB)
        {
            _oldBorderConditions = old;
            _newBorderConditions = newB;
        }

        public BorderConditions OldBorderConditions => _oldBorderConditions;
        public BorderConditions NewBorderConditions => _newBorderConditions;
    }

    internal enum BorderConditions
    {
        HingedSupport = 1, Slider, FixedSupport, HingelessFixedSupport
    }

    [StructLayout(LayoutKind.Auto)]
    internal sealed class Balk : UIElement
    {
        public static readonly DependencyProperty LengthProperty;
        public static readonly DependencyProperty MomentInertionProperty;
        public static readonly DependencyProperty SpringsProperty;
        public static readonly DependencyProperty CriticalForceProperty;
        public static readonly DependencyProperty LeftBorderConditionsProperty;
        public static readonly DependencyProperty RightBorderConditionsProperty;
        private static readonly Lazy<Balk> instance = new Lazy<Balk>(() => new Balk());
        public event EventHandler<LengthBalkChangedEventArgs> LengthChanged;
        public event EventHandler<MomentInertionBalkChangedEventArgs> MomentInertionChanged;
        public event EventHandler<BorderConditionChangedEventArgs> LeftBorderConditionChanged;
        public event EventHandler<BorderConditionChangedEventArgs> RightBorderConditionChanged;

        private Balk()
        {
            
        }

        static Balk()
        {
            FrameworkPropertyMetadata metadataLength = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataMoment = new FrameworkPropertyMetadata(new double(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataSprings = new FrameworkPropertyMetadata(new ObservableCollection<Spring>(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault);
            FrameworkPropertyMetadata metadataCriticalForce = new FrameworkPropertyMetadata(new Force(), FrameworkPropertyMetadataOptions.None);
            FrameworkPropertyMetadata metadataLeftBorderConditions = new FrameworkPropertyMetadata(BorderConditions.HingelessFixedSupport, FrameworkPropertyMetadataOptions.None);
            FrameworkPropertyMetadata metadataRigthBorderConditions = new FrameworkPropertyMetadata(BorderConditions.HingedSupport, FrameworkPropertyMetadataOptions.None);

            LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Balk), metadataLength);
            MomentInertionProperty = DependencyProperty.Register("MomentInertion", typeof(double), typeof(Balk), metadataMoment);
            SpringsProperty = DependencyProperty.Register("Springs", typeof(ObservableCollection<Spring>), typeof(Balk), metadataSprings);
            CriticalForceProperty = DependencyProperty.Register("CriticalForce", typeof(Force), typeof(Balk), metadataCriticalForce);
            LeftBorderConditionsProperty = DependencyProperty.Register("LeftBorderConditions", typeof(BorderConditions), typeof(Balk), metadataLeftBorderConditions);
            RightBorderConditionsProperty = DependencyProperty.Register("RightBorderConditions", typeof(BorderConditions), typeof(Balk), metadataRigthBorderConditions);
        }

        private void OnLengthChanged(LengthBalkChangedEventArgs e)
        {
            LengthChanged?.Invoke(this, e);
        }

        private void OnMomentInertionChanged(MomentInertionBalkChangedEventArgs e)
        {
            MomentInertionChanged?.Invoke(this, e);
        }

        private void OnLeftBorderConditionChanged(BorderConditionChangedEventArgs e)
        {
            LeftBorderConditionChanged?.Invoke(this, e);
        }

        private void OnRightBorderConditionChanged(BorderConditionChangedEventArgs e)
        {
            RightBorderConditionChanged?.Invoke(this, e);
        }

        public double Length
        {
            set
            {
                double oldLength = Length;
                if (oldLength == value) { return; }

                SetValue(LengthProperty, value);
                OnLengthChanged(new LengthBalkChangedEventArgs(oldLength, value));
            }
            get => (double)GetValue(LengthProperty);
        }
        public double MomentInertion
        {
            set
            {
                double oldMoment = MomentInertion;
                if (oldMoment == value) { return; }

                SetValue(MomentInertionProperty, value);
                OnMomentInertionChanged(new MomentInertionBalkChangedEventArgs(oldMoment, value));
            }
            get => (double)GetValue(MomentInertionProperty);
        }
        public ObservableCollection<Spring> Springs
        {
            set => SetValue(SpringsProperty, value);
            get => (ObservableCollection<Spring>)(GetValue(SpringsProperty));
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
                OnLeftBorderConditionChanged(new BorderConditionChangedEventArgs(old, value));
            }
        }
        public BorderConditions RightBorderConditios
        {
            get => (BorderConditions)GetValue(RightBorderConditionsProperty);
            set
            {
                BorderConditions old = RightBorderConditios;
                if(old == value) { return; }

                SetValue(RightBorderConditionsProperty, value);
                OnRightBorderConditionChanged(new BorderConditionChangedEventArgs(old, value));
            }
        }
        public Force ExternalForce { get; set; } = 17765.5407;
        public double ElasticModulus { get; set; } = 100000.0;
        public double K { get; set; }

        public static Balk Source { get => instance.Value; }
    } 
}
