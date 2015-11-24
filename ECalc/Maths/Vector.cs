using System;

namespace ECalc.Maths
{
    /// <summary>
    /// Vector Implementation. Supports 2D & 3D
    /// </summary>
    public class Vector
    {
        private double _x, _y;
        private double? _z;

        /// <summary>
        /// Crates a new instance of a 2D vector
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Vector(double x, double y)
        {
            Dimensions = 2;
            _x = x;
            _y = y;
            _z = null;
        }

        /// <summary>
        /// Crates a new instance of a 3D vector
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Vector(double x, double y, double z)
        {
            Dimensions = 3;
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Gets the number of Dimensions of a vector
        /// </summary>
        public int Dimensions
        {
            get;
            private set;
        }

        /// <summary>
        /// Vector magnitude
        /// </summary>
        public double Magnitude
        {
            get
            {
                if (Dimensions == 2)
                {
                    return Math.Sqrt((_x * _x) + (_y * _y));
                }
                else
                {
                    return Math.Sqrt((_x * _x) + (_y * _y) + (double)(_z * _z));
                }
            }
        }

        #region Operators

        

        public static bool operator == (Vector v1, Vector v2)
        {
            if (v1.Dimensions != v2.Dimensions) return false;
            if (v1._x != v2._x) return false;
            if (v1._y != v2._y) return false;
            if (v1._z != v2._z) return false;
            return true;
        }

        public static bool operator != (Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            int dimensions = Math.Max(v1.Dimensions, v2.Dimensions);
            double x, y, z;
            if (dimensions == 3)
            {
                x = v1._x + v2._x;
                y = v1._y + v2._y;
                if (v1._z == null) z = (double)v2._z;
                else if (v2._z == null) z = (double)v1._z;
                else z = (double)v1._z + (double)v2._z;
                return new Vector(x, y, z);
            }
            else
            {
                x = v1._x + v2._x;
                y = v1._y + v2._y;
                return new Vector(x, y);
            }
        }

        public static Vector operator + (Vector v, double num)
        {
            if (v.Dimensions == 2) return new Vector(v._x * num, v._y * num);
            else return new Vector(v._x + num, v._y + num, (double)v._z + num);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            int dimensions = Math.Max(v1.Dimensions, v2.Dimensions);
            double x, y, z;
            if (dimensions == 3)
            {
                x = v1._x - v2._x;
                y = v1._y - v2._y;
                if (v1._z == null) z = 0 - (double)v2._z;
                else if (v2._z == null) z = 0 - (double)v1._z;
                else z = (double)v1._z - (double)v2._z;
                return new Vector(x, y, z);
            }
            else
            {
                x = v1._x - v2._x;
                y = v1._y - v2._y;
                return new Vector(x, y);
            }
        }

        public static Vector operator -(Vector v, double num)
        {
            if (v.Dimensions == 2) return new Vector(v._x - num, v._y - num);
            else return new Vector(v._x - num, v._y - num, (double)v._z - num);
        }
        #endregion

        #region Object overrides
        /// <summary>
        /// Converts the current vector into a string representation
        /// </summary>
        /// <returns>a string representing the vector</returns>
        public override string ToString()
        {
            if (Dimensions == 2) return string.Format("x: {0}; y: {1}", _x, _y);
            else return string.Format("x: {0}; y: {1}; z: {2}", _x, _y, _z);
        }

        /// <summary>
        /// Checks if two instances are equal or not
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns>true, if the object is identical to the requested</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Vector v = obj as Vector;
            return this == v;
        }

        /// <summary>
        /// returns the hash code of the object
        /// </summary>
        /// <returns>an int value</returns>
        public override int GetHashCode()
        {
            return Dimensions.GetHashCode() ^ _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
        }
        #endregion
    }
}
