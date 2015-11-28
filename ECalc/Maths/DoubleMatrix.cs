using System;

namespace ECalc.Maths
{
    [Serializable]
    public class DoubleMatrix : AMatrix
    {
        private readonly double[] mData;
        internal double[] Data { get { return mData; } }

        #region Constructors
        public DoubleMatrix(int rows, int columns): base(rows, columns)
        {
            mData = new double[rows * columns];
        }
        public DoubleMatrix(int rows, int columns, double value): this(rows, columns)
        {
            for (int i = 0; i < mData.Length; i++)
            {
                mData[i] = value;
            }
        }
        public DoubleMatrix(DoubleMatrix other): this(other.Rows, other.Columns)
        {
            if (other != null)
            {
                Buffer.BlockCopy(other.mData, 0, mData, 0, mData.Length * sizeof(double));
            }
        }
        public DoubleMatrix(double[,] array): this(array.GetLength(0), array.GetLength(1))
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


        #region Abstract implementations
        public override void CopyTo(AMatrix target)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (Rows != target.Rows || Columns != target.Columns) throw new Exception("Target and Source row/column count mismatch");

            DoubleMatrix Dbl = target as DoubleMatrix;
            if (Dbl == null) base.CopyTo(target);
            else Buffer.BlockCopy(mData, 0, Dbl.mData, 0, mData.Length * sizeof(double));
        }

        public override AMatrix Clone()
        {
            return new DoubleMatrix(this);
        }

        protected internal override void ValueAt(int row, int column, double value)
        {
            mData[column * Rows + row] = value;
        }

        protected internal override double ValueAt(int row, int column)
        {
            return mData[column * Rows + row];
        }

        protected internal override double ValueAt(int loc)
        {
            return mData[loc];
        }

        protected internal override void ValueAt(int loc, double value)
        {
            mData[loc] = value;
        }

        protected internal override AMatrix CreateMatrix(int numberOfRows, int numberOfColumns)
        {
            return new DoubleMatrix(numberOfRows, numberOfColumns);
        }

        public override double Determinant()
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
                        if (mPivots[j] != j) mDeterminant = -mDeterminant * mMatrix.ValueAt(j, j);
                        else mDeterminant *= mMatrix.ValueAt(j, j);
                    }
                }
            }
            return mDeterminant;
        }

        public override AMatrix Transpose()
        {
            DoubleMatrix ret = new DoubleMatrix(Columns, Rows);
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

        public override AMatrix Inverse()
        {
            Compute();
            if (mIsSingular) throw new ArgumentException("Can't compute inverse, because matrix is singular");
            int order = this.Rows;
            DoubleMatrix inverse = new DoubleMatrix(order, order);
            for (int i = 0; i < order; i++)
            {
                inverse.Data[i + order * i] = 1.0;
            }

            Solve(mMatrix, mPivots, inverse);
            return inverse;
        }

        public override void Clear()
        {
            Array.Clear(mData, 0, mData.Length);
        }

        public override double[] GetRow(int index)
        {
            double[] array = new double[this.Columns];
            for (int i = 0; i < this.Columns; i++) array[i] = ValueAt(index, i);
            return array;
        }

        public override double[] GetColumn(int index)
        {
            double[] array = new double[this.Rows];
            for (int i = 0; i < this.Rows; i++) array[i] = ValueAt(i, index);
            return array;
        }

        public override void SetColumn(int index, double[] source)
        {
            if (index < 0 || index >= Columns) throw new ArgumentOutOfRangeException("index");
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != Rows) throw new Exception("Array element number is not equal with matrix row count");
            Buffer.BlockCopy(source, 0, mData, index * Rows * sizeof(double), source.Length * sizeof(double));
        }

        public override void SetRow(int index, double[] source)
        {
            if (index < 0 || index >= Rows) throw new ArgumentOutOfRangeException("index");
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length != Columns) throw new Exception("Array element number is not equal with matrix Column count");
            for (int j = 0; j < Columns; j++) ValueAt(index, j, source[j]);
        }

        public DoubleMatrix TrimTo(int rows, int columns)
        {
            if (rows > Rows || columns > Columns) throw new ArgumentException("Can't trim the matrix to a bigger matrix");
            DoubleMatrix ret = new DoubleMatrix(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ret[i, j] = this[i, j];
                }
            }
            return ret;
        }
        #endregion

        #region Features

        public static DoubleMatrix operator +(DoubleMatrix left, DoubleMatrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Rows != right.Rows || left.Columns != right.Columns) throw new Exception("Input matrix row/column count mismatch");
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    ret[i, j] = left[i, j] + right[i, j];
                }
            }
            return ret;
        }

        public static DoubleMatrix operator -(DoubleMatrix left, DoubleMatrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Rows != right.Rows || left.Columns != right.Columns) throw new Exception("Input matrix row/column count mismatch");
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < right.Columns; j++)
                {
                    ret[i, j] = left[i, j] - right[i, j];
                }
            }
            return ret;
        }

        public static bool operator ==(DoubleMatrix left, DoubleMatrix right)
        {
            if (object.Equals(right, null)) return false;
            if (left.Columns != right.Columns || left.Rows != right.Rows)
            {
                return false;
            }

            // Accept if the argument is the same object as this.
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            // If all else fails, perform elementwise comparison.
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Columns; j++)
                {
                    if (left.ValueAt(i, j) != right.ValueAt(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool operator !=(DoubleMatrix left, DoubleMatrix right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hashNum = System.Math.Min(Rows * Columns, 25);
            double[] hashBase = new double[hashNum];
            for (int i = 0; i < hashNum; i++)
            {
                int col = i % Columns;
                int row = (i - col) / Rows;
                hashBase[i] = this[row, col];
            }
            return hashBase.GetHashCode();
        }

        public static DoubleMatrix operator *(DoubleMatrix left, DoubleMatrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Columns != right.Rows) throw new Exception("Input matrix row/column count mismatch");
            DoubleMatrix ret = new DoubleMatrix(left.Rows, right.Columns);

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

        public static DoubleMatrix operator /(DoubleMatrix left, DoubleMatrix right)
        {
            if (left == null) throw new ArgumentNullException("left");
            if (right == null) throw new ArgumentNullException("right");
            if (left.Rows != right.Rows && left.Columns != right.Columns) throw new Exception("Input matrix row/column count mismatch");
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] / right[i, j];
                }
            }
            return ret;
        }

        public static DoubleMatrix operator + (DoubleMatrix left, double right)
        {
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] + right;
                }
            }
            return ret;
        }

        public static DoubleMatrix operator -(DoubleMatrix left, double right)
        {
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] - right;
                }
            }
            return ret;
        }

        public static DoubleMatrix operator *(DoubleMatrix left, double right)
        {
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] * right;
                }
            }
            return ret;
        }

        public static DoubleMatrix operator /(DoubleMatrix left, double right)
        {
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] / right;
                }
            }
            return ret;
        }

        public static DoubleMatrix operator %(DoubleMatrix left, double right)
        {
            DoubleMatrix ret = new DoubleMatrix(left.Rows, left.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = left[i, j] % right;
                }
            }
            return ret;
        }

        #endregion

        protected DoubleMatrix mMatrix;
        private bool mComputed;
        private double mDeterminant = Double.MinValue;
        private bool mIsSingular;
        protected int[] mPivots;

        private void Compute()
        {
            if (!mComputed)
            {
                mMatrix = (DoubleMatrix)this.Clone();
                mPivots = new int[mMatrix.Rows];

                DoCompute(mMatrix, mPivots);
                for (int j = 0; j < mMatrix.Rows; j++)
                {
                    if (mMatrix.ValueAt(j, j) == 0)
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

        protected void Solve(AMatrix factor, int[] pivots, AMatrix result)
        {
            Solve(factor.Rows, result.Columns, ((DoubleMatrix)factor).Data, pivots, ((DoubleMatrix)result).Data);
        }

        protected void DoCompute(DoubleMatrix data, int[] pivots)
        {

            DoubleMatrix matrix = data;
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
    }
}
