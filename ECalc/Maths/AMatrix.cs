using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ECalc.Maths
{
    [Serializable]
    public abstract class AMatrix : IFormattable, ICloneable, IEquatable<AMatrix>, IEnumerable<double>
    {
        private int nRows, nCols;

        protected AMatrix(int rows, int columns)
        {
            if (rows < 1) throw new ArgumentException("must be greater than 0", "rows");
            if (columns < 1) throw new ArgumentException("must be greater than 0", "columns");
            nRows = rows;
            nCols = columns;
        }

        #region Properties
        public virtual double this[int row, int column]
        {
            get
            {
                RangeCheck(row, column);
                return ValueAt(row, column);
            }
            set
            {
                RangeCheck(row, column);
                ValueAt(row, column, value);
            }
        }

        public virtual double this[int loc]
        {
            get { return ValueAt(loc); }
            set { ValueAt(loc, value); }
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
        #endregion

        #region Interface Implementations & Object overrides
        public override bool Equals(object obj)
        {
            return Equals(obj as AMatrix);
        }

        public bool Equals(AMatrix other)
        {
            // Reject equality when the argument is null or has a different shape.
            if (other == null)
            {
                return false;
            }
            if (Columns != other.Columns || Rows != other.Rows)
            {
                return false;
            }

            // Accept if the argument is the same object as this.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // If all else fails, perform elementwise comparison.
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (ValueAt(i, j) != other.ValueAt(i, j))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashNum = System.Math.Max(Rows * Columns, 25);
            double[] hashBase = new double[hashNum];
            for (int i = 0; i < hashNum; i++)
            {
                int col = i % Columns;
                int row = (i - col) / Rows;
                hashBase[i] = this[row, col];
            }
            return hashBase.GetHashCode();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public virtual AMatrix Clone()
        {
            AMatrix result = CreateMatrix(Rows, Columns);
            CopyTo(result);
            return result;
        }

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append(ValueAt(i, j).ToString(format, formatProvider));
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

        public IEnumerator<double> GetEnumerator()
        {
            return new MatrixEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new MatrixEnumerator(this);
        }

        #endregion

        private void RangeCheck(int row, int column)
        {
            if (row < 0 || row >= nRows) throw new ArgumentOutOfRangeException("row");
            if (column < 0 || column >= nCols) throw new ArgumentOutOfRangeException("column");
        }

        public virtual void CopyTo(AMatrix target)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (Rows != target.Rows || Columns != target.Columns) throw new Exception("Target & Source rows/column count mismatch");

            for (int i = 0; i < this.nRows; i++)
            {
                for (int j = 0; j < this.nCols; j++)
                {
                    target.ValueAt(i, j, this.ValueAt(i, j));
                }
            }
        }

        /// <summary>
        /// Negates the matrix
        /// </summary>
        public void Negate()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                   this.ValueAt(i, j, -1*this.ValueAt(i, j));
                }
            }
        }

        #region Abstract parts
        protected internal abstract void ValueAt(int row, int column, double value);
        protected internal abstract double ValueAt(int row, int column);
        protected internal abstract double ValueAt(int loc);
        protected internal abstract void ValueAt(int loc, double value);
        protected internal abstract AMatrix CreateMatrix(int numberOfRows, int numberOfColumns);
        /// <summary>
        /// Returns the determinant of the matrix
        /// </summary>
        public abstract double Determinant();
        /// <summary>
        /// Returnes the transposed matrix of the matrix
        /// </summary>
        public abstract AMatrix Transpose();
        /// <summary>
        /// Returns the inverse of the matrix
        /// </summary>
        public abstract AMatrix Inverse();
        public abstract void Clear();
        /// <summary>
        /// Returnes a row as an array
        /// </summary>
        public abstract double[] GetRow(int index);
        /// <summary>
        /// Returns a column as an array
        /// </summary>
        public abstract double[] GetColumn(int index);
        /// <summary>
        /// Sets a column based on an array
        /// </summary>
        public abstract void SetColumn(int index, double[] source);
        /// <summary>
        /// Sets a row based on an array
        /// </summary>
        public abstract void SetRow(int index, double[] source);
        #endregion
    }

    internal class MatrixEnumerator : IEnumerator<double>
    {
        private AMatrix _data;
        private int i;

        public MatrixEnumerator(AMatrix Data)
        {
            _data = Data;
            i = -1;
        }

        public double Current
        {
            get { return _data.ValueAt(i); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return _data.ValueAt(i); }
        }

        public bool MoveNext()
        {
            i++;
            if (i < _data.Rows * _data.Columns) return true;
            else return false;
        }

        public void Reset()
        {
            i = -1;
        }
    }
}
