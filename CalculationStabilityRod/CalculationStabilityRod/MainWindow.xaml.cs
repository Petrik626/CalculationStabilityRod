﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
  
    internal struct PairBoundaryConditions:IEquatable<PairBoundaryConditions>
    {
        #region FIELDS
        private readonly BorderConditions _left;
        private readonly BorderConditions _right;
        #endregion
        #region CONSTRUCTOR
        public PairBoundaryConditions(BorderConditions left, BorderConditions right)
        {
            _left = left;
            _right = right;
        }
        #endregion
        #region PROPERTIES
        public BorderConditions Left
        {
            get => _left;
        }

        public BorderConditions Right
        {
            get => _right;
        }
        #endregion
        #region METHODS
        public bool Equals(PairBoundaryConditions other)
        {
            return (_left == other._left) && (_right == other._right);
        }

        public override bool Equals(object obj)
        {
            return (obj is PairBoundaryConditions) ? Equals((PairBoundaryConditions)obj) : false;
        }

        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _right.GetHashCode();
        }
        #endregion
    }

    public partial class MainWindow : Window
    {
        private Func<double, double> SpringStartDrawing = (x) => 22.0 + 338.0 * x + 328.0 * x * x - 512.0 * x * x * x + 256.0 * x * x * x * x;
        private List<Line> fixedSupportLeft;
        private List<Line> hingelessFixedSupportleft;
        private List<Line> sliderLeft;
        private List<Line> hingedSupportLeft;

        private List<Line> fixedSupportRight;
        private List<Line> hingelessFixedSupportRight;
        private List<Line> sliderRight;
        private List<Line> hingedSupportRight;

        public ObservableCollection<DataPoint> PointsDeflection { get; private set; }
        public ObservableCollection<DataPoint> PointsAngle { get; private set; }
        public ObservableCollection<DataPoint> PointsMoment { get; private set; }
        public ObservableCollection<DataPoint> PointsForce { get; private set; }

        private IList<SpringView> Springs = new List<SpringView>();

        private MatrixFunction spanMatrix;
        private MatrixFunction equationMatrixExtension;
        private Mathematics.Objects.Vector startVector = new Mathematics.Objects.Vector(0, 1, 0, 1);
        private Mathematics.Objects.Vector endVector = new Mathematics.Objects.Vector(0, 1, 0, 1);
        Dictionary<int, SpringView> springsPictures = new Dictionary<int, SpringView>();
        readonly Dictionary<PairBoundaryConditions, int> pairs = new Dictionary<PairBoundaryConditions, int>()
        {
            [new PairBoundaryConditions(BorderConditions.HingedSupport, BorderConditions.HingelessFixedSupport)] = 1,
            [new PairBoundaryConditions(BorderConditions.HingedSupport, BorderConditions.FixedSupport)] = 2,
            [new PairBoundaryConditions(BorderConditions.Slider, BorderConditions.FixedSupport)] = 3,
            [new PairBoundaryConditions(BorderConditions.FixedSupport, BorderConditions.HingedSupport)] = 4,
            [new PairBoundaryConditions(BorderConditions.FixedSupport, BorderConditions.Slider)] = 5,
            [new PairBoundaryConditions(BorderConditions.HingelessFixedSupport, BorderConditions.HingedSupport)] = 6
        };
        private Balk balk = Balk.Source;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            SpringGrid.ItemsSource = balk.Springs;

            fixedSupportLeft = new List<Line>()
            {
                fixedSupportLine0,
                fixedSupportLine1,
                fixedSupportLine2,
                fixedSupportLine3,
                fixedSupportLine4,
                fixedSupportLine5
            };
            hingelessFixedSupportleft = new List<Line>()
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
            sliderLeft = new List<Line>()
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
            hingedSupportLeft = new List<Line>()
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

            hingedSupportRight = new List<Line>()
            {
                hingedSupportLineRight0,
                hingedSupportLineRight1,
                hingedSupportLineRight2,
                hingedSupportLineRight3,
                hingedSupportLineRight4,
                hingedSupportLineRight5,
                hingedSupportLineRight6,
                hingedSupportLineRight7,
                hingedSupportLineRight8,
                hingedSupportLineRight9
            };

            sliderRight = new List<Line>()
            {
                RightSliderLine0,
                RightSliderLine1,
                RightSliderLine2,
                RightSliderLine3,
                RightSliderLine4,
                RightSliderLine5,
                RightSliderLine6,
                RightSliderLine7
            };

            fixedSupportRight = new List<Line>()
            {
                RightFixedSupportLine0,
                RightFixedSupportLine1,
                RightFixedSupportLine2,
                RightFixedSupportLine3,
                RightFixedSupportLine4,
                RightFixedSupportLine5,
                RightFixedSupportLine6
            };

            hingelessFixedSupportRight = new List<Line>()
            {
                hingedFixedSupportLineRight0,
                hingedFixedSupportLineRight1,
                hingedFixedSupportLineRight2,
                hingedFixedSupportLineRight3,
                hingedFixedSupportLineRight5,
                hingedFixedSupportLineRight6,
                hingedFixedSupportLineRight7,
                hingedFixedSupportLineRight8,
                hingedFixedSupportLineRight9
            };

            balk.K = Math.Sqrt(balk.ExternalForce / (balk.ElasticModulus * balk.MomentInertion));
            spanMatrix = new MatrixFunction(4, 4)
            {
                Components = new Function[4, 4]
                {
                    { 1, new Function((x)=>x), new Function((x)=>(1-Cos(balk.K*x))/(balk.K*balk.K*balk.MomentInertion*balk.ElasticModulus)),new Function((x)=>(balk.K*x-Sin(balk.K*x))/(Pow(balk.K,3)*balk.ElasticModulus*balk.MomentInertion)) },
                    { 0, 1, new Function((x)=>(Sin(balk.K*x))/(balk.K*balk.ElasticModulus*balk.MomentInertion)), new Function((x)=>(1-Cos(balk.K*x))/(balk.K*balk.K*balk.ElasticModulus*balk.MomentInertion))},
                    { 0, 0, new Function((x)=>Cos(balk.K*x)), new Function((x)=>(Sin(balk.K*x))/(balk.K)) },
                    { 0, 0, new Function((x)=>-balk.K*Sin(balk.K*x)), new Function((x)=>Cos(balk.K*x))}
                }
            };
            equationMatrixExtension = new MatrixFunction(4, 4)
            {
                Components = new Function[4, 4]
                {
                    { 1, new Function((x)=>balk.Length), new Function((x)=>(1-Cos(balk.Length*x))/(x*x*balk.MomentInertion*balk.ElasticModulus)),new Function((x)=>(x*balk.Length-Sin(x*balk.Length))/(Pow(x,3)*balk.ElasticModulus*balk.MomentInertion)) },
                    { 0, 1, new Function((x)=>(Sin(balk.Length*x))/(x*balk.ElasticModulus*balk.MomentInertion)), new Function((x)=>(1-Cos(balk.Length*x))/(x*x*balk.ElasticModulus*balk.MomentInertion))},
                    { 0, 0, new Function((x)=>Cos(balk.Length *x)), new Function((x)=>(Sin(balk.Length *x))/(x)) },
                    { 0, 0, new Function((x)=>-x*Sin(balk.Length*x)), new Function((x)=>Cos(balk.Length*x))}
                }
            };

            ComboBoxTypeOfSealing.SelectedIndex = 4;
            ComboBoxRightTypeOfSealing.SelectedIndex = 1;
            balk.LeftBorderConditions = BorderConditions.HingelessFixedSupport;
            balk.RightBorderConditios = BorderConditions.HingedSupport;

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
            balk.RightBorderConditionChanged += Balk_RightBorderConditionChanged;

            PointsDeflection.CollectionChanged += FormLossStability_CollectionChanged;
            PointsAngle.CollectionChanged += FormLossStability_CollectionChanged;
            PointsMoment.CollectionChanged += FormLossStability_CollectionChanged;
            PointsForce.CollectionChanged += FormLossStability_CollectionChanged;

            FindSolutionStabilityProblemRod(balk);
        }

        private void Balk_RightBorderConditionChanged(object sender, BorderConditionChangedEventArgs e)
        {
            Balk b = sender as Balk;

            switch(b.RightBorderConditios)
            {
                case BorderConditions.HingedSupport: endVector[0] = 0; endVector[1] = 1;endVector[2] = 0; endVector[3] = 1; break;
                case BorderConditions.HingelessFixedSupport: endVector[0] = 0; endVector[1] = 1; endVector[2] = 0; endVector[3] = 1; break;
                case BorderConditions.Slider: endVector[0] = 0; endVector[1] = 0; endVector[2] = 1; endVector[3] = 1; break;
                case BorderConditions.FixedSupport: endVector[0] = 0; endVector[1] = 0; endVector[2] = 1; endVector[3] = 1; break;
            }
            FindSolutionStabilityProblemRod(b);
        }

        private void Springs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Remove: FindSolutionStabilityProblemRod(balk);break;
                default: break;
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
                parameter = 1;

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
                    SetStrokeThicknessLines(sliderLeft, 0.0);
                    SetStrokeThicknessLines(fixedSupportLeft, 0.0);
                    SetStrokeThicknessLines(hingelessFixedSupportleft, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 1.0;
                    hingedSupportEllipse.StrokeThickness = 1.0;
                    SetStrokeThicknessLines(hingedSupportLeft, 1.0);
                    break;
                case 2:
                    balk.LeftBorderConditions = BorderConditions.Slider;
                    SetStrokeThicknessLines(hingedSupportLeft,0.0);
                    SetStrokeThicknessLines(fixedSupportLeft, 0.0);
                    SetStrokeThicknessLines(hingelessFixedSupportleft, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 0.0;
                    hingedSupportEllipse.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(sliderLeft, 1.0);
                    break;
                case 3:
                    balk.LeftBorderConditions = BorderConditions.FixedSupport;
                    SetStrokeThicknessLines(hingedSupportLeft,0.0);
                    SetStrokeThicknessLines(sliderLeft, 0.0);
                    SetStrokeThicknessLines(hingelessFixedSupportleft, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 0.0;
                    hingedSupportEllipse.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(fixedSupportLeft, 1.0);
                    break;
                case 4:
                    balk.LeftBorderConditions = BorderConditions.HingelessFixedSupport;
                    SetStrokeThicknessLines(sliderLeft, 0.0);
                    SetStrokeThicknessLines(fixedSupportLeft, 0.0);
                    SetStrokeThicknessLines(hingedSupportLeft, 0.0);
                    hingelessFixedSupportEllipse.StrokeThickness = 1.0;
                    hingedSupportEllipse.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(hingelessFixedSupportleft, 1.0);
                    break;
            }

            ValidatingBorderConditions(balk.LeftBorderConditions, balk.RightBorderConditios, ComboBoxRightTypeOfSealing);
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

        private void balk_LeftBorderConditionChanged(object sender, BorderConditionChangedEventArgs e)
        {
            Balk b = sender as Balk;

            switch(e.NewBorderConditions)
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

            double length = balk.Length == 0.0 ? 1.0 : balk.Length;

            if (countSpring == 0)
            {
                double root = MethodNewton(f, startPoint);
                return (root * root * balk.ElasticModulus * balk.MomentInertion) / (length * length);
            }

            ObservableCollection<Spring> springs = model.Springs.OrderBy(s => s.CoordsX).ToObservableCollection();

            double[] coords = new double[countSpring + 1];
            double[] roots = new double[countSpring + 1];
            double[] criticalForce = new double[countSpring + 1];

            double sum = springs[0].CoordsX;
            coords[0] = springs[0].CoordsX;
            int i = 1;

            for(; i < countSpring; i++)
            {
                coords[i] = springs[i].CoordsX - sum;
                sum += coords[i];
            }
            coords[i] = model.Length - sum;

            for(int j=0; j<criticalForce.Length; j++)
            {
                roots[j] = MethodNewton(f, startPoint);
                criticalForce[j] = (Math.Pow(roots[j], 2) * model.MomentInertion * model.ElasticModulus) / (coords[j] * coords[j]);
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

            Mathematics.Objects.Matrix identity = Mathematics.Objects.Matrix.Indentity(4, 4);
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

            double step = (end - start) / (50);
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
                    if(matrixRigidity[3,0] == 0.0) { break; }

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
            int value;
            MatrixFunction matrix = new MatrixFunction(2, 2);
            pairs.TryGetValue(new PairBoundaryConditions(balk.LeftBorderConditions, balk.RightBorderConditios), out value);

            switch(value)
            {
                case 1: matrix = equationMatrixExtension.Minor(3, 0).Minor(1, 1); break;
                case 2: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
                case 3: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
                case 4: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
                case 5: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
                case 6: matrix = equationMatrixExtension.Minor(3, 0).Minor(1, 1); break;
                default: matrix = equationMatrixExtension.Minor(3, 0).Minor(2, 0); break;
            }
            return FindDeterminant(matrix);
        }

        private double FindStartPoint(Balk model)
        {
            int value;
            pairs.TryGetValue(new PairBoundaryConditions(model.LeftBorderConditions, model.RightBorderConditios), out value);
            switch(value)
            {
                case 1: return PI;
                case 2: return 1.4292126 * PI;
                case 3: return 2 * PI;
                case 4: return 1.4292126 * PI; 
                case 5: return 2 * PI;
                case 6: return PI;
                default: return model.K;
            }
        }

        private void CopyDataPoint<T>(ObservableCollection<T> input, ObservableCollection<T> output)
        {
            foreach(var el in input)
            {
                output.Add(el);
            }
        }

        private Function FindDeflectionFunction(Balk model)
        {
            Function f = 0.0;
            int n1 = model.Springs.Count + 1;
            double n2 = PI * (model.Springs.Count + 1 + 0.5);
            double length = model.Length == 0.0 ? 1.0 : model.Length;
            int value;

            pairs.TryGetValue(new PairBoundaryConditions(model.LeftBorderConditions, model.RightBorderConditios), out value);

            switch(value)
            {
                case 1: f = new Function((x) => Sin((n1 * x * PI) / length)); break;
                case 2: f = new Function((x) => Sin(n2 * x / length) - (x / length) * Sin(n2)); break;
                case 3: f = new Function((x) => Sin(n1 * x * PI / length) * Sin(n1 * x * PI / length)); break;
                case 4: f = new Function((x) => Sin(n2 * x / length) - (x / length) * Sin(n2)); break;
                case 5: f = new Function((x) => Sin(n1 * x * PI / length) * Sin(n1 * x * PI / length)); break;
                case 6: f = new Function((x) => Sin((n1 * x * PI) / length)); break;
            }

            return f;
        }

        private void FindSolutionStabilityProblemRod(Balk model)
        {
            PointsDeflection.Clear();
            PointsAngle.Clear();
            PointsMoment.Clear();
            PointsForce.Clear();
           
            Mathematics.Objects.Vector currentStartVector = startVector;
            double root = FindRoot(model);
            model.CriticalForce = root;
            CriticalForceTextBox.Text = model.CriticalForce.Value.ToString();

            int countSpring = model.Springs.Count;
            model.K = Sqrt(root / (model.ElasticModulus * model.MomentInertion));
            Mathematics.Objects.Vector newV;
            Function deflactionFunction = FindDeflectionFunction(model);


            double h = model.Length / 100;
            for (double x = 0; x <= model.Length; x += h)
            {
                newV = spanMatrix.ToMatrixDouble(x) * currentStartVector;
                PointsDeflection.Add(new DataPoint(x, deflactionFunction.Invoke(x)));
                PointsAngle.Add(new DataPoint(x, deflactionFunction.FindFirstDerivative(x)));
                PointsForce.Add(new DataPoint(x, deflactionFunction.FindThirdDerivative(x)));
                PointsMoment.Add(new DataPoint(x, deflactionFunction.FindSecondDerivative(x)));
            }
        }

        private void FormLossStability_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void ComboBoxRightTypeOfSealing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ComboBoxRightTypeOfSealing.SelectedIndex;

            switch(index)
            {
                case 1:
                    balk.RightBorderConditios = BorderConditions.HingedSupport;
                    SetStrokeThicknessLines(hingelessFixedSupportRight, 0.0);
                    SetStrokeThicknessLines(sliderRight, 0.0);
                    SetStrokeThicknessLines(fixedSupportRight, 0.0);
                    hingelessFixedSupportEllipseRight2.StrokeThickness = 1.0;
                    hingelessFixedSupportEllipseRight.StrokeThickness = 1.0;
                    SetStrokeThicknessLines(hingedSupportRight, 1.0);
                    break;
                case 2:
                    balk.RightBorderConditios = BorderConditions.Slider;
                    SetStrokeThicknessLines(hingelessFixedSupportRight, 0.0);
                    SetStrokeThicknessLines(hingedSupportRight, 0.0);
                    SetStrokeThicknessLines(fixedSupportRight, 0.0);
                    hingelessFixedSupportEllipseRight2.StrokeThickness = 0.0;
                    hingelessFixedSupportEllipseRight.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(sliderRight, 1.0);
                    break;
                case 3:
                    balk.RightBorderConditios = BorderConditions.FixedSupport;
                    SetStrokeThicknessLines(hingelessFixedSupportRight, 0.0);
                    SetStrokeThicknessLines(sliderRight, 0.0);
                    SetStrokeThicknessLines(hingedSupportRight, 0.0);
                    hingelessFixedSupportEllipseRight2.StrokeThickness = 0.0;
                    hingelessFixedSupportEllipseRight.StrokeThickness = 0.0;
                    SetStrokeThicknessLines(fixedSupportRight, 1.0);
                    break;
                case 4:
                    balk.RightBorderConditios = BorderConditions.HingelessFixedSupport;
                    SetStrokeThicknessLines(sliderRight, 0.0);
                    SetStrokeThicknessLines(hingedSupportRight, 0.0);
                    SetStrokeThicknessLines(fixedSupportRight, 0.0);
                    hingelessFixedSupportEllipseRight2.StrokeThickness = 0.0;
                    hingelessFixedSupportEllipseRight.StrokeThickness = 1.0;
                    SetStrokeThicknessLines(hingelessFixedSupportRight, 1.0);
                    break;
            }

            ValidatingBorderConditions(balk.RightBorderConditios, balk.LeftBorderConditions, ComboBoxTypeOfSealing);
        }

        private void ValidatingBorderConditions(BorderConditions verifiable, BorderConditions variable, ComboBox box)
        {
            string message = "Выбранны неверные кинематические связи. Ваш выбор был изменен на более удачный вариант";
            MessageBoxButton boxButton = MessageBoxButton.OK;
            MessageBoxImage boxImage = MessageBoxImage.Information;
            int variableInt = (int)variable;
            switch (verifiable)
            {
                case BorderConditions.HingedSupport:
                    if(variableInt != 3 && variableInt != 4)
                    {
                        MessageBox.Show(message, "Предупреждение", boxButton, boxImage);
                        box.SelectedIndex = 4;
                    }
                    break;
                case BorderConditions.Slider:
                    if(variableInt!=3)
                    {
                        MessageBox.Show(message, "Предупреждение", boxButton, boxImage);
                        box.SelectedIndex = 3;
                    }
                    break;
                case BorderConditions.FixedSupport:
                    if(variableInt!=1 && variableInt!=2)
                    {
                        MessageBox.Show(message, "Предупреждение", boxButton, boxImage);
                        box.SelectedIndex = 1;
                    }
                    break;
                case BorderConditions.HingelessFixedSupport:
                    if(variableInt!=1)
                    {
                        MessageBox.Show(message, "Предупреждение", boxButton, boxImage);
                        box.SelectedIndex = 1;
                    }
                    break;
            }
        }
    }

    internal static class Extensions
    {
        public static double FindFirstDerivative(this Function f, double x)
        {
            return Function.FindFirstDerivative(f, x);
        }

        public static double FindSecondDerivative(this Function f, double x)
        {
            return Function.FindSecondDerivative(f, x);
        }

        public static double FindDerivative(this Function f, double x, int n)
        {
            return Function.FindDerivative(f, x, n);
        }

        public static double FindThirdDerivative(this Function f, double x)
        {
            double step1 = 0.01;
            
            return - (f.Invoke(x - step1) - 3 * f.Invoke(x) + 3 * f.Invoke(x + step1) - f.Invoke(x + 2 * step1)) / (step1 * step1 * step1);
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
