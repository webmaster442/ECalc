using System;
using System.Text;

namespace ECalc.IronPythonEngine.Types
{
    public class Matrix : ICloneable, IFormattable
    {
        private int nRows, nCols;
        private double[] mData;

        public Matrix(int rows, int columns)
        {
            if (rows < 1) throw new ArgumentException("must be greater than 0", "rows");
            if (columns < 1) throw new ArgumentException("must be greater than 0", "columns");
            nRows = rows;
            nCols = columns;
            mData = new double[rows * columns];
        }

        #region Constructors
        public Matrix(int rows, int columns, double value) : this(rows, columns)
        {
            for (int i = 0; i < mData.Length; i++)
            {
                mData[i] = value;
            }
        }
        public Matrix(Matrix other) : this(other.Rows, other.Columns)
        {
            if (other != null)
            {
                Buffer.BlockCopy(other.mData, 0, mData, 0, mData.Length * sizeof(double));
            }
        }
        public Matrix(double[,] array) : this(array.GetLength(0), array.GetLength(1))
        {
            for (int j = 0; j < Columns; j++)
            {
                int jIndex = j * Rows;
                for (int i = 0; i < Rows; i++)
                {
                    mData[jIndex + i] = array[i, j];
                }
            }
        }
        #endregion

        internal double[] Data
        {
            get { return mData; }
        }

        /// <summary>
        /// Number of collums
        /// </summary>
        public int Columns
        {
            get { return nCols; }
            protected set { nCols = value; }
        }

        /// <summary>
        /// Number of rows
        /// </summary>
        public int Rows
        {
            get { return nRows; }
            protected set { nRows = value; }
        }

        public double this[int row, int column]
        {
            get
            {
                RangeCheck(row, column);
                return mData[column * Rows + row];
            }
            set
            {
                RangeCheck(row, column);
                mData[column * Rows + row] = value;
            }
        }

        public double this[int loc]
        {
            get { return mData[loc]; }
            set { mData[loc] = value; }
        }


        private void RangeCheck(int row, int column)
        {
            if (row < 0 || row >= nRows) throw new ArgumentOutOfRangeException("row");
            if (column < 0 || column >= nCols) throw new ArgumentOutOfRangeException("column");
        }

        public void CopyTo(Matrix target)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (Rows != target.Rows || Columns != target.Columns) throw new Exception("Target and Source row/column count mismatch");

            Buffer.BlockCopy(mData, 0, target.mData, 0, target.mData.Length * sizeof(double));
        }

        public void Negate()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    this[i, j] = -1 * this[i, j];
                }
            }
        }

        protected Matrix mMatrix;
        private bool mComputed;
        private double mDeterminant = Double.MinValue;
        private bool mIsSingular;
        protected int[] mPivots;

        private void Compute()
        {
            if (!mComputed)
            {
                mMatrix = (Matrix)this.Clone();
                mPivots = new int[mMatrix.Rows];

                DoCompute(mMatrix, mPivots);
                for (int j = 0; j < mMatrix.Rows; j++)
                {
                    if (mMatrix[j, j] == 0)
                    {
                        mIsSingular = true;
                        break;
                    }
                }
                mComputed = true;
            }
        }


        private static void Pivot(int m, int n, double[] B, int[] pivots)
        {
            for (int i = 0; i < pivots.Length; i++)
            {
                if (pivots[i] != i)
                {
                    int p = pivots[i];
                    for (int j = 0; j < n; j++)
                    {
                        int indexk = j * m;
                        int indexkp = indexk + p;
                        int indexkj = indexk + i;
                        double temp = B[indexkp];
                        B[indexkp] = B[indexkj];
                        B[indexkj] = temp;
                    }
                }
            }
        }

        private static void Solve(int order, int columns, double[] factor, int[] pivots, double[] data)
        {
            Pivot(order, columns, data, pivots);

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < order; k++)
            {
                int korder = k * order;
                for (int i = k + 1; i < order; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        int index = j * order;
                        data[i + index] -= data[k + index] * factor[i + korder];
                    }
                }
            }
            // Solve U*X = Y;
            for (int k = order - 1; k >= 0; k--)
            {
                int korder = k + k * order;
                for (int j = 0; j < columns; j++)
                {
                    data[k + j * order] /= factor[korder];
                }
                korder = k * order;
                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        int index = j * order;
                        data[i + index] -= data[k + index] * factor[i + korder];
                    }
                }
            }
        }

        protected void Solve(Matrix factor, int[] pivots, Matrix result)
        {
            Solve(factor.Rows, result.Columns, ((Matrix)factor).Data, pivots, ((Matrix)result).Data);
        }

        protected void DoCompute(Matrix data, int[] pivots)
        {

            Matrix matrix = data;
            int order = matrix.Rows;
            for (int i = 0; i < order; i++) pivots[i] = i;
            double[] LUcolj = new double[order];
            int indexj, indexjj;

            // Outer loop.
            for (int j = 0; j < order; j++)
            {
                indexj = j * order;
                indexjj = indexj + j;
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < order; i++) LUcolj[i] = matrix.Data[indexj + i];

                // Apply previous transformations.
                for (int i = 0; i < order; i++)
                {
                    // Most of the time is spent in the following dot product.
                    int kmax = System.Math.Min(i, j);
                    double s = 0.0;
                    for (int k = 0; k < kmax; k++) s += matrix.Data[k * order + i] * LUcolj[k];
                    matrix.Data[indexj + i] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < order; i++)
                {
                    if (Math.Abs(LUcolj[i]) > Math.Abs(LUcolj[p])) p = i;
                }
                if (p != j)
                {
                    for (int k = 0; k < order; k++)
                    {
                        int indexk = k * order;
                        int indexkp = indexk + p;
                        int indexkj = indexk + j;
                        double temp = matrix.Data[indexkp];
                        matrix.Data[indexkp] = matrix.Data[indexkj];
                        matrix.Data[indexkj] = temp;
                    }
                    pivots[j] = p;
                }

                // Compute multipliers.
                if (j < order & matrix.Data[indexjj] != 0.0)
                {
                    for (int i = j + 1; i < order; i++) matrix.Data[indexj + i] /= matrix.Data[indexjj];
                }
            }
        }

        public double Determinant()
        {
            Compute();
            if (mIsSingular) return 0;
            if (mDeterminant == double.MinValue)
            {
                lock (mMatrix)
                {
                    mDeterminant = 1.0;
                    for (int j = 0; j < mMatrix.Rows; j++)
                    {
                        if (mPivots[j] != j) mDeterminant = -mDeterminant * mMatrix[j, j];
                        else mDeterminant *= mMatrix[j, j];
                    }
                }
            }
            return mDeterminant;
        }

        public Matrix Transpose()
        {
            var ret = new Matrix(Columns, Rows);
            for (int j = 0; j < Columns; j++)
            {
                int index = j * Rows;
                for (int i = 0; i < Rows; i++)
                {
                    ret.mData[i * Columns + j] = mData[index + i];
                }
            }
            return ret;
        }

        public Matrix Inverse()
        {
            Compute();
            if (mIsSingular) throw new ArgumentException("Can't compute inverse, because matrix is singular");
            int order = this.Rows;
            var inverse = new Matrix(order, order);
            for (int i = 0; i < order; i++)
            {
                inverse.Data[i + order * i] = 1.0;
            }

            Solve(mMatrix, mPivots, inverse);
            return inverse;
        }

        public double[] GetRow(int index)
        {
            double[] array = new double[this.Columns];
            for (int i = 0; i < this.Columns; i++) array[i] = this[index, i];
            return array;
        }

        public double[] GetColumn(int index)
        {
            double[] array = new double[this.Rows];
            for (int i = 0; i < this.Rows; i++) array[i] = this[i, index];
            return array;
        }

        public void SetColumn(int index, double[] source)
        {
            if (index < 0 || index >= Columns) throw new ArgumentOutOfRangeException("index");
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != Rows) throw new Exception("Array element number is not equal with matrix row count");
            Buffer.BlockCopy(source, 0, mData, index * Rows * sizeof(double), source.Length * sizeof(double));
        }

        public void SetRow(int index, double[] source)
        {
            if (index < 0 || index >= Rows) throw new ArgumentOutOfRangeException("index");
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != Columns) throw new Exception("Array element number is not equal with matrix Column count");
            for (int j = 0; j < Columns; j++) this[index, j] = source[j];
        }

        public Matrix TrimTo(int rows, int columns)
        {
            if (rows > Rows || columns > Columns) throw new ArgumentException("Can't trim the matrix to a bigger matrix");
            var ret = new Matrix(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ret[i, j] = this[i, j];
                }
            }
            return ret;
        }

        public override int GetHashCode()
        {
            return mData.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var m = obj as Matrix;
            return this == m;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(this[i, j].ToString());
                    if (j != Columns - 1)
                    {
                        sb.Append(", ");
                    }
                }
                if (i != Rows - 1)
                {
                    sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(this[i, j].ToString(format, formatProvider));
                    if (j != Columns - 1)
                    {
                        sb.Append(", ");
                    }
                }
                if (i != Rows - 1)
                {
                    sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        public object Clone()
        {
            return new Matrix(this);
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Rows != right.Rows || left.Columns != right.Columns) throw new Exception("Input matrix row/column count mismatch");
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    ret[i, j] = left[i, j] + right[i, j];
                }
            }
            return ret;
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }
            if (left.Columns != right.Columns || left.Rows != right.Rows) return false;
            if (ReferenceEquals(left, right)) return true;
            for (int i=0; i<left.Rows; i++)
            {
                for (int j=0; j<right.Columns; j++)
                {
                    if (left[i, j] != right[i, j]) return false;
                }
            }
            return true;
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !(left == right);
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Rows != right.Rows || left.Columns != right.Columns) throw new Exception("Input matrix row/column count mismatch");
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    ret[i, j] = left[i, j] - right[i, j];
                }
            }
            return ret;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Columns != right.Rows) throw new Exception("Input matrix row/column count mismatch");
            var ret = new Matrix(left.Rows, right.Columns);

            for (int j = 0; j != right.Columns; j++)
            {
                for (int i = 0; i != left.Rows; i++)
                {
                    double s = 0;
                    for (int l = 0; l != left.Columns; l++)
                    {
                        s += left[i, l] * right[l, j];
                    }
                    ret[i, j] = s;
                }
            }
            return ret;
        }

        public static Matrix operator /(Matrix left, Matrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Rows != right.Rows && left.Columns != right.Columns) throw new Exception("Input matrix row/column count mismatch");
            var ret = new Matrix(left.Rows, left.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] / right[i, j];
                }
            }
            return ret;
        }

        public static Matrix operator +(Matrix left, double right)
        {
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] + right;
                }
            }
            return ret;
        }

        public static Matrix operator -(Matrix left, double right)
        {
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] - right;
                }
            }
            return ret;
        }

        public static Matrix operator *(Matrix left, double right)
        {
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] * right;
                }
            }
            return ret;
        }

        public static Matrix operator /(Matrix left, double right)
        {
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] / right;
                }
            }
            return ret;
        }

        public static Matrix operator %(Matrix left, double right)
        {
            var ret = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] % right;
                }
            }
            return ret;
        }
    }
}
