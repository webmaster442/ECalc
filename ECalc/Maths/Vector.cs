using System;

namespace ECalc.Maths
{
    /// <summary>
    /// Vector Implementation. Supports 2D & 3D
    /// </summary>
    public class Vector: IEquatable<Vector>, ICloneable
    {
        /// <summary>
        /// Crates a new instance of a 2D vector
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Vector(double x, double y)
        {
            X = x; Y = y; Z = null;
        }

        /// <summary>
        /// Crates a new instance of a 3D vector
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Vector(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }

        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public double? Z
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the number of Dimensions of a vector
        /// </summary>
        public int Dimensions
        {
            get
            {
                if (Z == null) return 2;
                else return 3;
            }
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
                    return Math.Sqrt((X * X) + (Y * Y));
                }
                else
                {
                    return Math.Sqrt((X * X) + (Y * Y) + (double)(Z * Z));
                }
            }
        }

        #region Operators

        public static bool operator == (Vector v1, Vector v2)
        {
            if (v1.Dimensions != v2.Dimensions) return false;
            if (v1.X != v2.X) return false;
            if (v1.Y != v2.Y) return false;
            if (v1.Z != v2.Z) return false;
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
                x = v1.X + v2.X;
                y = v1.Y + v2.Y;
                if (v1.Z == null) z = (double)v2.Z;
                else if (v2.Z == null) z = (double)v1.Z;
                else z = (double)v1.Z + (double)v2.Z;
                return new Vector(x, y, z);
            }
            else
            {
                x = v1.X + v2.X;
                y = v1.Y + v2.Y;
                return new Vector(x, y);
            }
        }

        public static Vector operator + (double num, Vector v)
        {
            return v + num;
        }

        public static Vector operator + (Vector v, double num)
        {
            if (v.Dimensions == 2) return new Vector(v.X * num, v.Y * num);
            else return new Vector(v.X + num, v.Y + num, (double)v.Z + num);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            int dimensions = Math.Max(v1.Dimensions, v2.Dimensions);
            double x, y, z;
            if (dimensions == 3)
            {
                x = v1.X - v2.X;
                y = v1.Y - v2.Y;
                if (v1.Z == null) z = 0 - (double)v2.Z;
                else if (v2.Z == null) z = 0 - (double)v1.Z;
                else z = (double)v1.Z - (double)v2.Z;
                return new Vector(x, y, z);
            }
            else
            {
                x = v1.X - v2.X;
                y = v1.Y - v2.Y;
                return new Vector(x, y);
            }
        }

        public static Vector operator -(Vector v, double num)
        {
            if (v.Dimensions == 2) return new Vector(v.X - num, v.Y - num);
            else return new Vector(v.X - num, v.Y - num, (double)v.Z - num);
        }

        public static Vector operator -(double num, Vector v)
        {
            if (v.Dimensions == 2) return new Vector(num - v.X, num - v.Y);
            else return new Vector(num - v.X, num - v.Y, num - (double)v.Z);
        }

        public static Vector operator *(Vector v, double num)
        {
            if (v.Dimensions == 2) return new Vector(v.X * num, v.Y * num);
            else return new Vector(v.X * num, v.Y * num, (double)v.Z * num);
        }

        public static Vector operator /(Vector v, double num)
        {
            if (v.Dimensions == 2) return new Vector(v.X / num, v.Y / num);
            else return new Vector(v.X / num, v.Y / num, (double)v.Z / num);
        }

        public static Vector operator /(double num, Vector v)
        {
            if (v.Dimensions == 2) return new Vector(num / v.X, num / v.Y);
            else return new Vector(num / v.X, num / v.Y, num / (double)v.Z);
        }

        public static Vector operator *(double num, Vector v)
        {
            return v * num;
        }

        public static double operator * (Vector v1, Vector v2)
        {
            int dimensions = Math.Max(v1.Dimensions, v2.Dimensions);
            if (dimensions == 2)
            {
                return (v1.X * v2.X) + (v1.Y * v2.Y);
            }
            else
            {
                double z = 0;
                if (v1.Z == null) z = 0;
                else if (v2.Z == null) z = 0;
                else z = (double)v1.Z * (double)v2.Z;
                return (v1.X * v2.X) + (v1.Y * v2.Y) + z;
            }
        }
        #endregion

        #region Object overrides
        /// <summary>
        /// Converts the current vector into a string representation
        /// </summary>
        /// <returns>a string representing the vector</returns>
        public override string ToString()
        {
            if (Dimensions == 2) return string.Format("x: {0}; y: {1}", X, Y);
            else return string.Format("x: {0}; y: {1}; z: {2}", X, Y, Z);
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
            return Dimensions.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
        #endregion

        public bool Equals(Vector other)
        {
            return this == other;
        }

        public object Clone()
        {
            if (Dimensions == 2) return new Vector(X, Y);
            else return new Vector(X, Y, (double)Z);
        }
    }
}
