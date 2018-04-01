using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace CalculationStabilityRod
{
    internal sealed class SpringView:IEnumerable<UIElement>
    {
        private Ellipse _hingeTop;
        private Ellipse _hingeBottom;
        private Line _verticalLineTopSpring;
        private Line _rightAngleLineSpring;
        private Line _leftAngleLineSpring;
        private Line _verticalLineBottomSpring;
        private Line _horizontalLineSpring;
        private Line _angleLineBottomSpring1;
        private Line _angleLineBottomSpring2;
        private Line _angleLineBottomSpring3;
        private Line _angleLineBottomSpring4;
        private Line _angleLineBottomSpring5;
        private Line _angleLineBottomSpring6;

        public SpringView()
        {
            _hingeTop = new Ellipse();
            _hingeBottom = new Ellipse();
            _verticalLineTopSpring = new Line();
            _rightAngleLineSpring = new Line();
            _leftAngleLineSpring = new Line();
            _verticalLineBottomSpring = new Line();
            _horizontalLineSpring = new Line();
            _angleLineBottomSpring1 = new Line();
            _angleLineBottomSpring2 = new Line();
            _angleLineBottomSpring3 = new Line();
            _angleLineBottomSpring4 = new Line();
            _angleLineBottomSpring5 = new Line();
            _angleLineBottomSpring6 = new Line();
        }

        public SpringView(double x) : this() { }

        public SpringView(Ellipse e1, Ellipse e2, Line l1, Line l2, Line l3, Line l4, Line l5, Line l6, Line l7, Line l8, Line l9, Line l10, Line l11)
        {
            _hingeTop = e1;
            _hingeBottom = e2;
            _verticalLineTopSpring = l1;
            _rightAngleLineSpring = l2;
            _leftAngleLineSpring = l3;
            _verticalLineBottomSpring = l4;
            _horizontalLineSpring = l5;
            _angleLineBottomSpring1 = l6;
            _angleLineBottomSpring2 = l7;
            _angleLineBottomSpring3 = l8;
            _angleLineBottomSpring4 = l9;
            _angleLineBottomSpring5 = l10;
            _angleLineBottomSpring6 = l11;
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            yield return _hingeTop;
            yield return _hingeBottom;
            yield return _verticalLineTopSpring;
            yield return _rightAngleLineSpring;
            yield return _leftAngleLineSpring;
            yield return _verticalLineBottomSpring;
            yield return _horizontalLineSpring;
            yield return _angleLineBottomSpring1;
            yield return _angleLineBottomSpring2;
            yield return _angleLineBottomSpring3;
            yield return _angleLineBottomSpring4;
            yield return _angleLineBottomSpring5;
            yield return _angleLineBottomSpring6;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Ellipse HingeTop { get => _hingeTop; set => _hingeTop = value; }
        public Ellipse HingeBottom { get => _hingeBottom; set => _hingeBottom = value; }
        public Line VerticalLineTopSpring { get => _verticalLineTopSpring; set => _verticalLineTopSpring = value; }
        public Line RightAngleLineSpring { get => _rightAngleLineSpring; set => _rightAngleLineSpring = value; }
        public Line LeftAngleLineSpring { get => _leftAngleLineSpring; set => _leftAngleLineSpring = value; }
        public Line VerticalLineBottomSpring { get => _verticalLineBottomSpring; set => _verticalLineBottomSpring = value; }
        public Line HorizontalLineSpring { get => _horizontalLineSpring; set => _horizontalLineSpring = value; }
        public Line AngleLineBottomSpring1 { get => _angleLineBottomSpring1; set => _angleLineBottomSpring1 = value; }
        public Line AngleLineBottomSpring2 { get => _angleLineBottomSpring2; set => _angleLineBottomSpring2 = value; }
        public Line AngleLineBottomSpring3 { get => _angleLineBottomSpring3; set => _angleLineBottomSpring3 = value; }
        public Line AngleLineBottomSpring4 { get => _angleLineBottomSpring4; set => _angleLineBottomSpring4 = value; }
        public Line AngleLineBottomSpring5 { get => _angleLineBottomSpring5; set => _angleLineBottomSpring5 = value; }
        public Line AngleLineBottomSpring6 { get => _angleLineBottomSpring6; set => _angleLineBottomSpring6 = value; }
    }
}
