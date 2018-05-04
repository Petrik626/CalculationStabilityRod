using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathematics.Objects;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CalculationStabilityRod
{
    [StructLayout(LayoutKind.Auto)]
    internal class VectorFunction
    {
        #region FIELDS
        private readonly int _dimension;
        private readonly Function[] _components;
        #endregion;
        #region CONSTRUCTORS
        public VectorFunction(int dimension)
        {
            _dimension = dimension;
            _components = new Function[_dimension];
        }

        public VectorFunction(Function f1, Function f2):this(2)
        {
            _components[0] = f1;
            _components[1] = f2;
        }

        public VectorFunction(Function f1, Function f2, Function f3):this(3)
        {
            _components[0] = f1; _components[1] = f2; _components[2] = f3;
        }

        public VectorFunction(Function f1, Function f2, Function f3, Function f4):this(4)
        {
            _components[0] = f1; _components[1] = f2;
            _components[2] = f3; _components[3] = f3;
        }

        public VectorFunction(Function f1, Function f2, Function f3, Function f4, Function f5):this(5)
        {
            _components[0] = f1; _components[1] = f2;
            _components[2] = f3; _components[3] = f4;
            _components[4] = f5;
        }

        public VectorFunction(params Function[] functions):this(functions.Length)
        {
            int i = 0;
            foreach(var el in functions)
            {
                _components[i++] = el;
            }
        }
        #endregion
        #region METHODS
        public Vector ToVectorDouble(double x)
        {
            return _components.Select(el => el.Invoke(x)).ToArray<double>();
        }
        #endregion
        #region PROPERTIES
        public int Dimension { get => _dimension; }
        public Function[] Components
        {
            get => _components;
            set
            {
                if (_components == null) { throw new NullReferenceException(); }
                int i = 0;
                foreach(var el in value)
                {
                    _components[i++] = el;
                }
            }
        }
        [IndexerName("Function")]
        public Function this[int index]
        {
            get
            {
                if(index<0 || index >= _dimension) { throw new ArgumentException(); }
                return _components[index];
            }
            set
            {
                if (index < 0 || index >= _dimension) { throw new ArgumentException(); }
                _components[index] = value;
            }
        }
        #endregion
        #region STATIC MEMBERS
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator+(VectorFunction v1, VectorFunction v2)
        {
            if (v1._dimension != v2._dimension) { throw new ArgumentException(); }
            VectorFunction v3 = new VectorFunction(v1._dimension);

            for(int i=0; i<v3._dimension; i++)
            {
                v3._components[i] = v1._components[i] + v2._components[i];
            }

            return v3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator +(VectorFunction v1, Vector v2)
        {
            if (v1._dimension != v2.Measurement) { throw new ArgumentException(); }
            VectorFunction v3 = new VectorFunction(v1._dimension);

            for (int i = 0; i < v3._dimension; i++)
            {
                v3._components[i] = v1._components[i] + v2[i];
            }

            return v3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator+(Vector v1, VectorFunction v2)
        {
            return v2 + v1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator-(VectorFunction v1, VectorFunction v2)
        {
            if (v1._dimension != v2._dimension) { throw new ArgumentException(); }
            VectorFunction v3 = new VectorFunction(v1._dimension);

            for(int i=0; i<v3._dimension; i++)
            {
                v3._components[i] = v1._components[i] - v2._components[i];
            }

            return v3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator-(VectorFunction v1, Vector v2)
        {
            if (v1._dimension != v2.Measurement) { throw new ArgumentException(); }
            VectorFunction v3 = new VectorFunction(v1._dimension);

            for (int i = 0; i < v3._dimension; i++)
            {
                v3._components[i] = v1._components[i] - v2[i];
            }

            return v3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator-(Vector v1, VectorFunction v2)
        {
            if (v1.Measurement != v2._dimension) { throw new ArgumentException(); }
            VectorFunction v3 = new VectorFunction(v2._dimension);

            for (int i = 0; i < v3._dimension; i++)
            {
                v3._components[i] = v1[i] - v2._components[i];
            }

            return v3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Function operator*(VectorFunction v1, VectorFunction v2)
        {
            if (v1._dimension != v2._dimension) { throw new ArgumentException(); }
            Function f = new Function();

            for(int i=0; i<v1._dimension; i++)
            {
                f += v1._components[i] * v2._components[i];
            }

            return f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator*(VectorFunction v, MatrixFunction m)
        {
            if (v._dimension != m.NumberOfRow) { throw new ArgumentException("Size of matrix and vector are not equaling each other"); }

            VectorFunction result = new VectorFunction(m.NumberOfColumns);

            Function f;

            for(int i=0; i<result._dimension; i++)
            {
                f = 0.0;
                for(int j=0;j<m.NumberOfRow;j++)
                {
                    f = f + v[j] * m[j, i];
                }
                result._components[i] = f;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator*(VectorFunction v, double a)
        {
            return v._components.Select(f => f * a).ToArray<Function>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction operator*(double a, VectorFunction v)
        {
            return v * a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator VectorFunction(Function[] functions)
        {
            VectorFunction vectorFunction = new VectorFunction(functions.Length);

            int i = 0;
            foreach(var el in functions)
            {
                vectorFunction[i++] = el;
            }

            return vectorFunction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Add(VectorFunction v1, VectorFunction v2) => v1 + v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Add(VectorFunction v1, Vector v2) => v1 + v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Add(Vector v1, VectorFunction v2) => v1 + v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Subtract(VectorFunction v1, VectorFunction v2) => v1 - v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Subtract(VectorFunction v1, Vector v2) => v1 - v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Subtract(Vector v1, VectorFunction v2) => v1 - v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Function Multiply(VectorFunction v1, VectorFunction v2) => v1 * v2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Multiply(VectorFunction v, MatrixFunction m) => v * m;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Multiply(VectorFunction v, double b) => v * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorFunction Multiply(double b, VectorFunction v) => v * b;
        #endregion
    }
}
