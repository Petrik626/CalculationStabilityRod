using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot.Wpf;
using OxyPlot;

namespace CalculationStabilityRod
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Line> fixedSupport;
        private List<Line> hingelessFixedSupport;
        private List<Line> slider;
        private List<Line> hingedSupport;

        public IList<DataPoint> Points { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            fixedSupport = new List<Line>()
            {
                fixedSupportLine0,
                fixedSupportLine1,
                fixedSupportLine2,
                fixedSupportLine3,
                fixedSupportLine4,
                fixedSupportLine5
            };
            hingelessFixedSupport = new List<Line>()
            {
                hingelessFixedSupportLine0,
                hingelessFixedSupportLine1,
                hingelessFixedSupportLine2,
                hingelessFixedSupportLine3,
                hingelessFixedSupportLine4,
                hingelessFixedSupportLine5,
                hingelessFixedSupportLine6,
                hingelessFixedSupportLine7
            };
            slider = new List<Line>()
            {
                sliderLine0,
                sliderLine1,
                sliderLine2,
                sliderLine3,
                sliderLine4,
                sliderLine5,
                sliderLine6,
                sliderLine7
            };
            hingedSupport = new List<Line>()
            {
                hingedSupportLine0,
                hingedSupportLine1,
                hingedSupportLine2,
                hingedSupportLine3,
                hingedSupportLine4,
                hingedSupportLine5,
                hingedSupportLine6,
                hingedSupportLine7,
                hingedSupportLine8,
                hingedSupportLine9,
                hingedSupportLine10
            };

            ComboBoxTypeOfSealing.SelectedIndex = 4;

            Points = new List<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(7,12),
                new DataPoint(8,2),
                new DataPoint(6,3),
                new DataPoint(4,4)
            };

        }

        private void SetStrokeThicknessLines(List<Line> lines, double thikness)
        {
            foreach(Line l in lines)
            {
                l.StrokeThickness = thikness;
            }
        }

        private void AddSpringButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxTypeOfSealing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            int index = ComboBoxTypeOfSealing.SelectedIndex;

            switch(index)
            {
                case 1: SetStrokeThicknessLines(slider, 0.0); SetStrokeThicknessLines(fixedSupport, 0.0); SetStrokeThicknessLines(hingelessFixedSupport, 0.0); hingelessFixedSupportEllipse.StrokeThickness = 1.0; hingedSupportEllipse.StrokeThickness = 1.0; SetStrokeThicknessLines(hingedSupport, 1.0); break;
                case 2: SetStrokeThicknessLines(hingedSupport,0.0); SetStrokeThicknessLines(fixedSupport, 0.0); SetStrokeThicknessLines(hingelessFixedSupport, 0.0); hingelessFixedSupportEllipse.StrokeThickness = 0.0; hingedSupportEllipse.StrokeThickness = 0.0; SetStrokeThicknessLines(slider, 1.0); break;
                case 3: SetStrokeThicknessLines(hingedSupport,0.0); SetStrokeThicknessLines(slider, 0.0); SetStrokeThicknessLines(hingelessFixedSupport, 0.0); hingelessFixedSupportEllipse.StrokeThickness = 0.0; hingedSupportEllipse.StrokeThickness = 0.0; SetStrokeThicknessLines(fixedSupport, 1.0); break;
                case 4: SetStrokeThicknessLines(slider, 0.0); SetStrokeThicknessLines(fixedSupport, 0.0); SetStrokeThicknessLines(hingedSupport, 0.0); hingelessFixedSupportEllipse.StrokeThickness = 1.0; hingedSupportEllipse.StrokeThickness = 0.0; SetStrokeThicknessLines(hingelessFixedSupport, 1.0); break;
            }
        }
    }
}
