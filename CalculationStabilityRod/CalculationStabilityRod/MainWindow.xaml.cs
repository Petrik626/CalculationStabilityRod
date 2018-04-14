using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
using Mathematics.Objects;
using static System.Math;

namespace CalculationStabilityRod
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        private Func<double, double> SpringStartDrawing = (x) => 22.0 + 338.0 * x + 328.0 * x * x - 512.0 * x * x * x + 256.0 * x * x * x * x;
        private List<Line> fixedSupport;
        private List<Line> hingelessFixedSupport;
        private List<Line> slider;
        private List<Line> hingedSupport;

        public IList<DataPoint> PointsDeflection { get; private set; }
        public IList<DataPoint> PointsAngle { get; private set; }
        public IList<DataPoint> PointsMoment { get; private set; }
        public IList<DataPoint> PointsForce { get; private set; }

        private IList<SpringView> Springs = new List<SpringView>();

        private MatrixFunction spanMatrix;
        Dictionary<int, SpringView> springsPictures = new Dictionary<int, SpringView>();
        private Balk balk = Balk.Source;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            SpringGrid.ItemsSource = balk.Springs;

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
            balk.LeftBorderConditions = BorderConditions.HingelessFixedSupport;

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

            SpringView.SpringCanvas = OutlineBalkCanvas;
            spanMatrix = new MatrixFunction(4, 4)
            {
                Components = new Function[4,4]
                {
                    { new Function((x)=>1), new Function((x)=>x), new Function((x)=>x-Cos(x)),new Function((x)=>x-Sin(x)) },
                    { new Function((x)=>0), new Function((x)=>1), new Function(Sin), new Function((x)=>1-Cos(x))},
                    { new Function((x)=>0), new Function((x)=>0), new Function(Cos), new Function(Sin) },
                    { new Function((x)=>0), new Function((x)=>0), new Function((x)=>-Sin(x)), new Function(Cos)}
                }
            };
        }

        private void AddSpringInSpringView(Balk balk, IList<SpringView> elements)
        {
            foreach(var el in balk.Springs)
            {
                elements.Add(new SpringView(el.CoordsX));
            }
        }

        private void AddElementsSpringInCanvas(Canvas canvas, IEnumerable<System.Windows.UIElement> elements)
        {
            foreach(var el in elements)
            {
                canvas.Children.Add(el);
            }
        }

        private void RemoveElementsSpringOfCanvas(Canvas canvas, IEnumerable<System.Windows.UIElement> elements)
        {
            foreach(var el in elements)
            {
                canvas.Children.Remove(el);
            }
        }

        private void SetStrokeThicknessLines(IList<Line> lines, double thikness)
        {
            foreach(Line l in lines)
            {
                l.StrokeThickness = thikness;
            }
        }

        private void ValidatingCorrectInput(string input, out string output, out double parameter)
        {
            StringBuilder sb = new StringBuilder(input);
            string pattern = @"^\d+[.,]?(\d+)?$";
            bool flag = Regex.IsMatch(sb.ToString(), pattern, RegexOptions.Compiled);
            int countCommas = 0;

            if (!flag)
            {
                for (int i = 0; i < sb.ToString().Length; i++)
                {
                    if(sb[i]=='.' || sb[i] == ',')
                    {
                        countCommas++;
                        if(countCommas>1)
                        {
                            sb.Remove(i, 1);
                            countCommas--;
                        }
                    }
                    else if (!(char.IsNumber(sb[i]) || sb[i] == '.' || sb[i] == ','))
                    {
                        sb.Remove(i, 1);
                    }
                }
            }
            else
            {
                sb.Replace(".", ",");
            }


            if (sb.ToString() == string.Empty)
            {
                output = string.Empty;
                parameter = 0.0;

                return;
            }

            output = sb.ToString();
            parameter = double.Parse(output);
        }

        private void ComboBoxTypeOfSealing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            int index = ComboBoxTypeOfSealing.SelectedIndex;

            switch(index)
            {
                case 1:
                    balk.LeftBorderConditions = BorderConditions.HingedSupport;
                    SetStrokeThicknessLines(slider, 0.0);
                    SetStrokeThicknessLines(fixedSupport, 0.0);
                    SetStrokeThicknessLines(hingelessFixedSupport, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 1.0;
                    hingedSupportEllipse.StrokeThickness = 1.0;
                    SetStrokeThicknessLines(hingedSupport, 1.0);
                    break;
                case 2:
                    balk.LeftBorderConditions = BorderConditions.Slider;
                    SetStrokeThicknessLines(hingedSupport,0.0);
                    SetStrokeThicknessLines(fixedSupport, 0.0);
                    SetStrokeThicknessLines(hingelessFixedSupport, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 0.0;
                    hingedSupportEllipse.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(slider, 1.0);
                    break;
                case 3:
                    balk.LeftBorderConditions = BorderConditions.FixedSupport;
                    SetStrokeThicknessLines(hingedSupport,0.0);
                    SetStrokeThicknessLines(slider, 0.0);
                    SetStrokeThicknessLines(hingelessFixedSupport, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 0.0;
                    hingedSupportEllipse.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(fixedSupport, 1.0);
                    break;
                case 4:
                    balk.LeftBorderConditions = BorderConditions.HingelessFixedSupport;
                    SetStrokeThicknessLines(slider, 0.0);
                    SetStrokeThicknessLines(fixedSupport, 0.0);
                    SetStrokeThicknessLines(hingedSupport, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 1.0;
                    hingedSupportEllipse.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(hingelessFixedSupport, 1.0);
                    break;
            }
        }

        private void SpringGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                if(MessageBox.Show("Удалить пружину?","",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
                {
                    int index = SpringGrid.SelectedIndex;
                    int springID = balk.Springs[index].ID;

                    RemoveElementsSpringOfCanvas(OutlineBalkCanvas, springsPictures[springID]);
                    springsPictures.Remove(springID);
                    SpringGrid.CanUserDeleteRows = true;
                }
                else
                {
                    SpringGrid.CanUserDeleteRows = false;
                }
            }
        }

        private void LengthBalkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string output;
            double length;

            ValidatingCorrectInput(LengthBalkTextBox.Text, out output, out length);

            LengthBalkTextBox.Text = output;
            balk.Length = length;
            LengthBalkTextBox.SelectionStart = output.Length;
        }

        private void MomentInertionBalkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string output;
            double momentInertion;

            ValidatingCorrectInput(MomentInertionBalkTextBox.Text, out output, out momentInertion);
            MomentInertionBalkTextBox.Text = output;
            balk.MomentInertion = momentInertion;
            MomentInertionBalkTextBox.SelectionStart = output.Length;
        }

        private void SpringGrid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                int index = SpringGrid.SelectedIndex - 1;
                int springID = balk.Springs[index].ID;

                double x = balk.Springs[index].CoordsX / balk.Length;
                double leftCanvas = SpringStartDrawing(x);
               

                if (springsPictures.ContainsKey(springID))
                {
                    RemoveElementsSpringOfCanvas(OutlineBalkCanvas, springsPictures[springID]);
                    springsPictures.Remove(springID);
                }


                springsPictures[springID] = new SpringView(leftCanvas);
                AddElementsSpringInCanvas(OutlineBalkCanvas, springsPictures[springID]);
            }
        }
    }
}
