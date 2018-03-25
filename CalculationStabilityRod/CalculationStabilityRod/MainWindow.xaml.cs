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
    /// 
  
    public partial class MainWindow : Window
    {
        private List<Line> fixedSupport;
        private List<Line> hingelessFixedSupport;
        private List<Line> slider;
        private List<Line> hingedSupport;

        public IList<DataPoint> PointsDeflection { get; private set; }
        public IList<DataPoint> PointsAngle { get; private set; }
        public IList<DataPoint> PointsMoment { get; private set; }
        public IList<DataPoint> PointsForce { get; private set; }

        private LineSeries deflectionSeries = new LineSeries();
        private LineSeries angleSeries = new LineSeries();
        private LineSeries momentSeries = new LineSeries();
        private LineSeries forceSeries = new LineSeries();


        internal IList<Spring> Springs { get; set; } = new List<Spring>();

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

            PointsDeflection = new List<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };
            PointsAngle = new List<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };
            PointsMoment = new List<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };
            PointsForce = new List<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };

            deflectionSeries.ItemsSource = PointsDeflection;
            angleSeries.ItemsSource = PointsAngle;
            momentSeries.ItemsSource = PointsMoment;
            forceSeries.ItemsSource = PointsForce;

            diagramDeflection.Series.Add(deflectionSeries);
            diagramAngle.Series.Add(angleSeries);
            diagramMoment.Series.Add(momentSeries);
            diagramForce.Series.Add(forceSeries);

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

        private void DeleteSpringButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SpringGrid_Loaded(object sender, RoutedEventArgs e)
        {
            SpringGrid.ItemsSource = Springs;
        }
    }
}
