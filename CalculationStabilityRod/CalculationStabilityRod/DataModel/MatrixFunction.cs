using System;
using Mathematics.Objects;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace CalculationStabilityRod.DataModel
{
    [StructLayout(LayoutKind.Auto)]
    internal sealed class MatrixFunction
    {
        #region FIELDS
        private readonly Function[,] _components;
        private readonly int _numberOfRow;
        private readonly int _numberOfColumn;
        #endregion
        #region CONSTURCTORS
        public MatrixFunction(int numberRow, int numberColumn)
        {
            _numberOfRow = numberRow;
            _numberOfColumn = numberColumn;
            _components = new Function[_numberOfRow, _numberOfColumn];
        }

        public MatrixFunction(Function[,] components)
        {
            _numberOfRow = components.GetLength(0);
            _numberOfColumn = components.GetLength(1);
            _components = new Function[_numberOfRow, _numberOfColumn];

            for(int i=0; i<_numberOfRow; i++)
            {
                for(int j=0; j<_numberOfColumn; j++)
                {
                    _components[i, j] = new Function(components[i, j].Expression);
                }
            }
        }

        public MatrixFunction(MatrixFunction obj) : this(obj._components) { }
        #endregion
        #region STATIC MEMBERS
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator +(MatrixFunction a, MatrixFunction b)
        {
            if ((a._numberOfRow != b._numberOfRow) || (a._numberOfColumn != b._numberOfColumn)) { throw new ArgumentException(); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, a._numberOfColumn);

            for(int i=0;i<matrix._numberOfRow;i++)
            {
                for(int j=0;j<matrix._numberOfColumn;j++)
                {
                    matrix._components[i, j] = a._components[i, j] + b._components[i, j];
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator+(MatrixFunction a, Matrix b)
        {
            if(a._numberOfRow!=b.CountOfRow || a._numberOfColumn != b.CountOfColumn) { throw new ArgumentException(); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, a._numberOfColumn);

            for(int i=0;i<matrix._numberOfRow;i++)
            {
                for(int j=0;j<matrix._numberOfColumn;j++)
                {
                    matrix._components[i, j] = a._components[i, j] + b[i, j];
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator +(Matrix a, MatrixFunction b) => b + a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator-(MatrixFunction a, MatrixFunction b)
        {
            if((a._numberOfRow!=b._numberOfRow) || (a._numberOfColumn != b._numberOfColumn)) { throw new ArgumentException(); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, a._numberOfColumn);

            for(int i=0;i<matrix._numberOfRow;i++)
            {
                for(int j=0;j<matrix._numberOfColumn;j++)
                {
                    matrix._components[i, j] = a._components[i, j] - b._components[i, j];
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator-(MatrixFunction a, Matrix b)
        {
            if (a._numberOfRow != b.CountOfRow || a._numberOfColumn != b.CountOfColumn) { throw new ArgumentException(); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, a._numberOfColumn);

            for (int i = 0; i < matrix._numberOfRow; i++)
            {
                for (int j = 0; j < matrix._numberOfColumn; j++)
                {
                    matrix._components[i, j] = a._components[i, j] - b[i, j];
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator-(Matrix b, MatrixFunction a)
        {
            if (a._numberOfRow != b.CountOfRow || a._numberOfColumn != b.CountOfColumn) { throw new ArgumentException(); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, a._numberOfColumn);

            for (int i = 0; i < matrix._numberOfRow; i++)
            {
                for (int j = 0; j < matrix._numberOfColumn; j++)
                {
                    matrix._components[i, j] = b[i, j] - a._components[i, j];
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator*(MatrixFunction a, MatrixFunction b)
        {
            if (a._numberOfRow != b._numberOfColumn) { throw new ArgumentException("Number of columns and rows are not equal each other in argument"); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, b._numberOfColumn);
            Function s;

            for(int i=0; i<matrix._numberOfRow;i++)
            {
                for(int j=0; j<matrix._numberOfColumn; j++)
                {
                    s = new Function((x) => 0.0);
                    for (int k=0;k<a._numberOfColumn;k++)
                    {
                        s = s + a._components[i, j] * b._components[i, j];
                    }

                    matrix._components[i, j] = s;
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator*(MatrixFunction a, double b)
        {
            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, a._numberOfColumn);

            for(int i=0; i<matrix._numberOfRow; i++)
            {
                for(int j=0; j<matrix._numberOfColumn; j++)
                {
                    matrix._components[i, j] = a._components[i, j] * b;
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator *(double a, MatrixFunction b) => b * a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator*(MatrixFunction a, Matrix b)
        {
            if (a.NumberOfRow != b.CountOfColumn) { throw new ArgumentException("Number of columns and rows are not equal each other in argument"); }

            MatrixFunction matrix = new MatrixFunction(a._numberOfRow, b.CountOfColumn);
            Function s;

            for (int i = 0; i < matrix._numberOfRow; i++)
            {
                for (int j = 0; j < matrix._numberOfColumn; j++)
                {
                    s = new Function((x) => 0.0);
                    for (int k = 0; k < a._numberOfColumn; k++)
                    {
                        s = s + a._components[i, j] * b[i, j];
                    }

                    matrix._components[i, j] = s;
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction operator *(Matrix a, MatrixFunction b)
        {
            if (a.CountOfRow != b._numberOfColumn) { throw new ArgumentException("Number of columns and rows are not equal each other in argument"); }

            MatrixFunction matrix = new MatrixFunction(a.CountOfRow, b._numberOfColumn);
            Function s;

            for (int i = 0; i < matrix._numberOfRow; i++)
            {
                for (int j = 0; j < matrix._numberOfColumn; j++)
                {
                    s = new Function((x) => 0.0);
                    for (int k = 0; k < a.CountOfColumn; k++)
                    {
                        s = s + a[i, j] * b._components[i, j];
                    }

                    matrix._components[i, j] = s;
                }
            }

            return matrix;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator*(MatrixFunction m, VectorFunction v)
        {
            if (m._numberOfColumn != v.Dimension) { throw new ArgumentException("Size of matrix and vector are not equaling each other"); }

            Function[] functions = new Function[m._numberOfRow];

            Function f;
            for(int i=0; i<m._numberOfRow; i++)
            {
                f = 0.0;
                for(int j=0; j<v.Dimension; j++)
                {
                    f = f + m._components[i, j] * v[j];
                }

                functions[i] = f;
            }

            return functions;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Add(MatrixFunction a, MatrixFunction b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Add(MatrixFunction a, Matrix b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Add(Matrix a, MatrixFunction b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Subtract(MatrixFunction a, MatrixFunction b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Subtract(MatrixFunction a, Matrix b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Subtract(Matrix a, MatrixFunction b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Multiply(MatrixFunction a, MatrixFunction b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Multiply(MatrixFunction a, double b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Multiply(double a, MatrixFunction b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Multiply(MatrixFunction a, Matrix b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MatrixFunction Multiply(Matrix a, MatrixFunction b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Multiply(MatrixFunction m, VectorFunction v) => m * v;

        #endregion
        #region PROPERTIES
        public int NumberOfRow { get => _numberOfRow; }
        public int NumberOfColumns { get => _numberOfColumn; }
        public Function[,] Components
        {
            get => _components;
            set
            {
                if (_components == null)
                {
                    throw new NullReferenceException();
                }
                else if(_numberOfRow != value.GetLength(0) || _numberOfColumn != value.GetLength(1))
                {
                    throw new ArgumentException();
                }

                for(int i=0; i<_numberOfRow; i++)
                {
                    for(int j=0;j<_numberOfColumn;j++)
                    {
                        _components[i, j] = value[i, j];
                    }
                }
            }
        }
        [IndexerName("Function")]
        public Function this[int index1, int index2]
        {
            get
            {
                if (index1 < 0 || index2 < 0 || index1 > _numberOfRow - 1 || index2 > _numberOfColumn - 1) { throw new IndexOutOfRangeException(); }
                return _components[index1, index2];
            }
            set
            {
                if (index1 < 0 || index2 < 0 || index1 > _numberOfRow - 1 || index2 > _numberOfColumn - 1) { throw new IndexOutOfRangeException(); }
                _components[index1, index2] = value;
            }
        }
        public int AmountOfElements { get => _numberOfRow * _numberOfColumn; }
        #endregion
        #region METHODS
        public Matrix ToMatrixDouble(double x)
        {
            Matrix matrix = new Matrix(_numberOfRow, _numberOfColumn);

            for(int i=0;i<_numberOfRow;i++)
            {
                for(int j=0;j<_numberOfColumn;j++)
                {
                    matrix[i, j] = _components[i, j].Invoke(x);
                }
            }

            return matrix;
        }
        #endregion
    }
}
