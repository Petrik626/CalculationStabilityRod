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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using CalculationStabilityRod.DataModel;

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

        public ObservableCollection<DataPoint> PointsDeflection { get; private set; }
        public ObservableCollection<DataPoint> PointsAngle { get; private set; }
        public ObservableCollection<DataPoint> PointsMoment { get; private set; }
        public ObservableCollection<DataPoint> PointsForce { get; private set; }

        private IList<SpringView> Springs = new List<SpringView>();

        private MatrixFunction spanMatrix;
        private MatrixFunction equationMatrixExtension;
        private Mathematics.Objects.Vector startVector = new Mathematics.Objects.Vector(0, 1, 0, 1);
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

            balk.K = Math.Sqrt(balk.ExternalForce / (balk.ElasticModulus * balk.MomentInertion));
            spanMatrix = new MatrixFunction(4, 4)
            {
                Components = new Function[4, 4]
                {
                    { 1, new Function((x)=>x), new Function((x)=>(1-Cos(balk.K*x))/(balk.K*balk.K*balk.MomentInertion*balk.ElasticModulus)),new Function((x)=>(balk.K*x-Sin(x))/(Pow(balk.K,3)*balk.ElasticModulus*balk.MomentInertion)) },
                    { 0, 1, new Function((x)=>(Sin(balk.K*x))/(balk.K*balk.ElasticModulus*balk.MomentInertion)), new Function((x)=>(1-Cos(balk.K*x))/(balk.K*balk.K*balk.ElasticModulus*balk.MomentInertion))},
                    { 0, 0, new Function((x)=>Cos(balk.K*x)), new Function((x)=>(Sin(balk.K*x))/(balk.K)) },
                    { 0, 0, new Function((x)=>-balk.K*Sin(balk.K*x)), new Function((x)=>Cos(balk.K*x))}
                }
            };
            equationMatrixExtension = new MatrixFunction(4, 4)
            {
                Components = new Function[4, 4]
                {
                    { 1, new Function((x)=>balk.Length), new Function((x)=>(1-Cos(balk.Length*x))/(x*x*balk.MomentInertion*balk.ElasticModulus)),new Function((x)=>(x*balk.Length-Sin(balk.Length))/(Pow(x,3)*balk.ElasticModulus*balk.MomentInertion)) },
                    { 0, 1, new Function((x)=>(Sin(balk.Length*x))/(x*balk.ElasticModulus*balk.MomentInertion)), new Function((x)=>(1-Cos(balk.Length*x))/(x*x*balk.ElasticModulus*balk.MomentInertion))},
                    { 0, 0, new Function((x)=>Cos(balk.Length *x)), new Function((x)=>(Sin(balk.Length *x))/(x)) },
                    { 0, 0, new Function((x)=>-x*Sin(balk.Length*x)), new Function((x)=>Cos(balk.Length*x))}
                }
            };

            ComboBoxTypeOfSealing.SelectedIndex = 4;
            balk.LeftBorderConditions = BorderConditions.HingelessFixedSupport;

            PointsDeflection = new ObservableCollection<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };
            PointsAngle = new ObservableCollection<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };
            PointsMoment = new ObservableCollection<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };
            PointsForce = new ObservableCollection<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,0),
                new DataPoint(2,0)
            };

            diagramDeflection.InvalidatePlot(true);
            diagramAngle.InvalidatePlot(true);
            diagramMoment.InvalidatePlot(true);
            diagramForce.InvalidatePlot(true);

            SpringView.SpringCanvas = OutlineBalkCanvas;


            balk.LengthChanged += balkView_SpringViewChanged;
            balk.LengthChanged += balk_LengthChanged;
            balk.Springs.CollectionChanged += Springs_CollectionChanged;

            balk.MomentInertionChanged += balk_MomentInertionChanged;

            balk.LeftBorderConditionChanged += balk_LeftBorderConditionChanged;

            PointsDeflection.CollectionChanged += FormLossStability_CollectionChanged;
            PointsAngle.CollectionChanged += FormLossStability_CollectionChanged;
            PointsMoment.CollectionChanged += FormLossStability_CollectionChanged;
            PointsForce.CollectionChanged += FormLossStability_CollectionChanged;

            FindSolutionStabilityProblemRod(balk);
        }

        private void Springs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FindSolutionStabilityProblemRod(balk);
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
                FindSolutionStabilityProblemRod(balk);
            }
        }

        private void balkView_SpringViewChanged(object sender, LengthBalkChangedEventArgs e)
        {
            if (springsPictures.Count == 0) { return; }

            Balk b = sender as Balk;
            double newLength = e.NewLength, leftCanvas;

            foreach (var el in b.Springs)
            {
                RemoveElementsSpringOfCanvas(OutlineBalkCanvas, springsPictures[el.ID]);
                leftCanvas = SpringStartDrawing(el.CoordsX / newLength);
                springsPictures[el.ID] = new SpringView(leftCanvas);
                AddElementsSpringInCanvas(OutlineBalkCanvas, springsPictures[el.ID]);
            }
        }

        private void balk_LengthChanged(object sender, LengthBalkChangedEventArgs e)
        {
            Balk b = sender as Balk;

            FindSolutionStabilityProblemRod(b);
        }

        private void balk_MomentInertionChanged(object sender, MomentInertionBalkChangedEventArgs e)
        {
            Balk b = sender as Balk;
            b.K = Math.Sqrt(b.ExternalForce / (b.ElasticModulus * e.NewMoment));
            FindSolutionStabilityProblemRod(b);
        }

        private void balk_LeftBorderConditionChanged(object sender, LeftBorderConditionChangedEventArgs e)
        {
            Balk b = sender as Balk;

            switch(e.NewLeftBorderConditions)
            {
                case BorderConditions.HingedSupport: startVector[0] = 0; startVector[1] = 1; startVector[2] = 0; startVector[3] = 1; break;
                case BorderConditions.HingelessFixedSupport: startVector[0] = 0; startVector[1] = 1; startVector[2] = 0; startVector[3] = 1; break;
                case BorderConditions.Slider: startVector[0] = 0;startVector[1] = 0;startVector[2] = 1; startVector[3] = 1; break;
                case BorderConditions.FixedSupport: startVector[0] = 0; startVector[1] = 0; startVector[2] = 1; startVector[3] = 1; break;
            }
            FindSolutionStabilityProblemRod(b);
        }

        private double MethodNewton(Function f, double startPoint)
        {
            double newPoint, oldPoint = startPoint;

            for(; ; )
            {
                newPoint = oldPoint - f.Invoke(oldPoint) / f.FindFirstDerivative(oldPoint);

                if (Math.Abs(newPoint - oldPoint) <= 0.00001) { break; }

                oldPoint = newPoint;
            }

            return newPoint; 
        }

        private double FindRoot(Balk model)
        {
            int countSpring = model.Springs.Count;
            Function f = FindEquation();
            double startPoint = FindStartPoint(model);

            if (countSpring == 0)
            {
                double root = MethodNewton(f, startPoint);
                return (root * root * balk.ElasticModulus * balk.MomentInertion) / (balk.Length * balk.Length);
            }

            double[] coords = new double[countSpring + 1];
            double[] roots = new double[countSpring + 1];
            double[] criticalForce = new double[countSpring + 1];

            double sum = model.Springs[0].CoordsX;
            coords[0] = model.Springs[0].CoordsX;
            int i = 1;

            for(; i < countSpring; i++)
            {
                coords[i] = model.Springs[i].CoordsX - sum;
                sum += coords[i];
            }
            coords[i] = balk.Length - sum;

            for(int j=0; j<criticalForce.Length; j++)
            {
                roots[j] = MethodNewton(f, startPoint);
                criticalForce[j] = (Math.Pow(roots[j], 2) * balk.MomentInertion * balk.ElasticModulus) / (coords[j] * coords[j]);
            }

            return criticalForce.Min(x => Math.Abs(x));
        }

        private Function FindDeterminant(MatrixFunction matrix)
        {
            return (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
        }

        private Tuple<ObservableCollection<DataPoint>, ObservableCollection<DataPoint>, ObservableCollection<DataPoint>, ObservableCollection<DataPoint>, Mathematics.Objects.Vector> FindFormsLossStability(double start, double end, double rigidity, Mathematics.Objects.Vector startV)
        {
            ObservableCollection<DataPoint> deflaction = new ObservableCollection<DataPoint>();
            ObservableCollection<DataPoint> angle = new ObservableCollection<DataPoint>();
            ObservableCollection<DataPoint> moment = new ObservableCollection<DataPoint>();
            ObservableCollection<DataPoint> force = new ObservableCollection<DataPoint>();

            Mathematics.Objects.Vector newVector;

            Mathematics.Objects.Matrix matrixRigidity = new Mathematics.Objects.Matrix(4, 4)
            {
                Components = new double[4, 4]
                {
                        {1,0,0,0 },
                        {0,1,0,0 },
                        {0,0,1,0 },
                        {-rigidity,0,0,1 }
                }
            };

            double step = (end - start) / (60);
            newVector = spanMatrix.ToMatrixDouble(start) * startV;
            deflaction.Add(new DataPoint(start, newVector[0]));
            angle.Add(new DataPoint(start, newVector[1]));
            moment.Add(new DataPoint(start, newVector[2]));
            force.Add(new DataPoint(start, newVector[3]));


            for (double x = start + step; x<=end; x+=step)
            {
                newVector = spanMatrix.ToMatrixDouble(x) * startV;

                if (x == end)
                {
                    newVector = matrixRigidity * newVector;
                }


                deflaction.Add(new DataPoint(x, newVector[0]));
                angle.Add(new DataPoint(x, newVector[1]));
                moment.Add(new DataPoint(x, newVector[2]));
                force.Add(new DataPoint(x, newVector[3]));

            }

            return new Tuple<ObservableCollection<DataPoint>, ObservableCollection<DataPoint>, ObservableCollection<DataPoint>, ObservableCollection<DataPoint>, Mathematics.Objects.Vector>(deflaction, angle, moment, force, newVector);
        }

        public Function FindEquation()
        {
            MatrixFunction matrix = new MatrixFunction(2, 2);
            switch(balk.LeftBorderConditions)
            {
                case BorderConditions.HingedSupport: matrix = equationMatrixExtension.Minor(3, 0).Minor(1, 1); break;
                case BorderConditions.Slider: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
                case BorderConditions.FixedSupport: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
                case BorderConditions.HingelessFixedSupport: matrix = equationMatrixExtension.Minor(3, 0).Minor(1, 1); break;
            }

            return FindDeterminant(matrix);
        }

        private double FindStartPoint(Balk model)
        {
            switch(model.LeftBorderConditions)
            {
                case BorderConditions.HingedSupport: return PI;
                case BorderConditions.HingelessFixedSupport: return PI;
                case BorderConditions.Slider: return 4.5;
                case BorderConditions.FixedSupport: return 4.5;
                default: return balk.K;
            }
        }

        private void FindSolutionStabilityProblemRod(Balk model)
        {
            PointsDeflection.Clear();
            PointsAngle.Clear();
            PointsMoment.Clear();
            PointsForce.Clear();

            double root = FindRoot(model);
            model.CriticalForce = root;
            RootTextBox.Text = root.ToString();
            CriticalForceTextBox.Text = model.CriticalForce.Value.ToString();

            model.K = Math.Sqrt(root / (model.ElasticModulus * model.MomentInertion));


            Mathematics.Objects.Vector newV;

            double h = model.Length / 100;
            for (double x = 0; x <= model.Length; x += h)
            {
                 newV = spanMatrix.ToMatrixDouble(x) * startVector;
                 PointsDeflection.Add(new DataPoint(x, newV[0]));
                 PointsAngle.Add(new DataPoint(x, newV[1]));
                 PointsForce.Add(new DataPoint(x, newV[3]));
                 PointsMoment.Add(new DataPoint(x, newV[2]));
            }
          
            /*else
            {
                double[] rigidity = new double[countSpring + 2];
                double[] coordsStartInterval = new double[countSpring + 2];
                coordsStartInterval[0] = 0.0; rigidity[0] = 0.0;
                coordsStartInterval[coordsStartInterval.Length - 1] = model.Length; rigidity[rigidity.Length - 1] = 0.0;

                for (int i = 1; i < coordsStartInterval.Length - 1; i++)
                {
                    coordsStartInterval[i] = model.Springs[i - 1].CoordsX;
                }

                for (int i = 0; i < 1; i++)
                {
                    var solve = FindFormsLossStability(coordsStartInterval[i], 50, 50, startVector);
                    PointsDeflection = PointsDeflection.Union(solve.Item1).OrderBy(p => p.X).ToObservableCollection();
                    PointsAngle = PointsAngle.Union(solve.Item2).OrderBy(p=>p.X).ToObservableCollection();
                    PointsMoment = PointsMoment.Union(solve.Item3).OrderBy(p => p.X).ToObservableCollection();
                    PointsForce = PointsForce.Union(solve.Item4).OrderBy(p => p.X).ToObservableCollection();
                    startVector = solve.Item5;
                }
            }   */       
        }

        private void FormLossStability_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }
    }

    internal static class Extensions
    {
        public static double FindFirstDerivative(this Function f, double x)
        {
            return Function.FindFirstDerivative(f, x);
        }

        public static double FindDerivative(this Function f, double x, int n)
        {
            return Function.FindDerivative(f, x, n);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> ts)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();

            foreach(var el in ts)
            {
                collection.Add(el);
            }

            return collection;
        }
    }
}
