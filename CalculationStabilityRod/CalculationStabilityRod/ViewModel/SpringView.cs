using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
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

        static SpringView()
        {
            SpringCanvas = new Canvas();
        }

        private SpringView()
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

        public SpringView(double leftCanvas) : this()
        {
            _hingeTop.Width = 10; _hingeTop.Height = 10;
            _hingeTop.Fill = Brushes.White; _hingeTop.Stroke = Brushes.Black;

            _hingeBottom.Width = 10; _hingeBottom.Height = 10;
            _hingeBottom.Fill = Brushes.White; _hingeBottom.Stroke = Brushes.Black;

            _verticalLineTopSpring.Stroke = Brushes.Black; _verticalLineTopSpring.StrokeThickness = 1;
            _verticalLineTopSpring.Y1 = 3;

            _rightAngleLineSpring.Stroke = Brushes.Black; _rightAngleLineSpring.StrokeThickness = 1;
            _rightAngleLineSpring.X1 = 3; _rightAngleLineSpring.Y1 = 3;

            _leftAngleLineSpring.Stroke = Brushes.Black; _leftAngleLineSpring.StrokeThickness = 1;
            _leftAngleLineSpring.X1 = -3; _leftAngleLineSpring.Y1 = 3;

            _verticalLineBottomSpring.Stroke = Brushes.Black; _verticalLineBottomSpring.StrokeThickness = 1;
            _verticalLineBottomSpring.Y1 = 3;

            _horizontalLineSpring.Stroke = Brushes.Black; _horizontalLineSpring.StrokeThickness = 1;
            _horizontalLineSpring.X1 = 25;

            _angleLineBottomSpring1.Stroke = Brushes.Black; _angleLineBottomSpring1.StrokeThickness = 1;
            _angleLineBottomSpring1.X1 = -8; _angleLineBottomSpring1.Y1 = 8;


            _angleLineBottomSpring2.Stroke = Brushes.Black; _angleLineBottomSpring2.StrokeThickness = 1;
            _angleLineBottomSpring2.X1 = -8; _angleLineBottomSpring2.Y1 = 8;


            _angleLineBottomSpring3.Stroke = Brushes.Black; _angleLineBottomSpring3.StrokeThickness = 1;
            _angleLineBottomSpring3.X1 = -8; _angleLineBottomSpring3.Y1 = 8;


            _angleLineBottomSpring4.Stroke = Brushes.Black; _angleLineBottomSpring4.StrokeThickness = 1;
            _angleLineBottomSpring4.X1 = -8; _angleLineBottomSpring4.Y1 = 8;


            _angleLineBottomSpring5.Stroke = Brushes.Black; _angleLineBottomSpring5.StrokeThickness = 1;
            _angleLineBottomSpring5.X1 = -8; _angleLineBottomSpring5.Y1 = 8;


            _angleLineBottomSpring6.Stroke = Brushes.Black; _angleLineBottomSpring6.StrokeThickness = 1;
            _angleLineBottomSpring6.X1 = -8; _angleLineBottomSpring6.Y1 = 8;

            Canvas.SetLeft(_hingeTop, leftCanvas); Canvas.SetTop(_hingeTop, 55);
            Canvas.SetLeft(_verticalLineTopSpring, leftCanvas + 5); Canvas.SetTop(_verticalLineTopSpring, 65);
            Canvas.SetLeft(_rightAngleLineSpring, leftCanvas + 5); Canvas.SetTop(_rightAngleLineSpring, 68);
            Canvas.SetLeft(_leftAngleLineSpring, leftCanvas + 8); Canvas.SetTop(_leftAngleLineSpring, 71);
            Canvas.SetLeft(_verticalLineBottomSpring, leftCanvas + 5); Canvas.SetTop(_verticalLineBottomSpring, 74);
            Canvas.SetLeft(_hingeBottom, leftCanvas); Canvas.SetTop(_hingeBottom, 75);
            Canvas.SetLeft(_horizontalLineSpring, leftCanvas - 7); Canvas.SetTop(_horizontalLineSpring, 85);
            Canvas.SetLeft(_angleLineBottomSpring1, leftCanvas - 6); Canvas.SetTop(_angleLineBottomSpring1, 85);
            Canvas.SetLeft(_angleLineBottomSpring2, leftCanvas - 1); Canvas.SetTop(_angleLineBottomSpring2, 85);
            Canvas.SetLeft(_angleLineBottomSpring3, leftCanvas + 4); Canvas.SetTop(_angleLineBottomSpring3, 85);
            Canvas.SetLeft(_angleLineBottomSpring4, leftCanvas + 9); Canvas.SetTop(_angleLineBottomSpring4, 85);
            Canvas.SetLeft(_angleLineBottomSpring5, leftCanvas + 14); Canvas.SetTop(_angleLineBottomSpring5, 85);
            Canvas.SetLeft(_angleLineBottomSpring6, leftCanvas + 19); Canvas.SetTop(_angleLineBottomSpring6, 85);
        }

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

        public static Canvas SpringCanvas { get; set; }
    }
}
