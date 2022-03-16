using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    /// <summary>
    /// 4-dimensional vector with double precision
    /// </summary>
    public struct Vector4
    {
        /// <summary>
        /// Returns new zero vector
        /// </summary>
        public static readonly Vector4 Zero = new Vector4();
        /// <summary>
        /// Returns new unit x vector (x = 1, y = 0, z = 0, w = 0)
        /// </summary>
        public static readonly Vector4 UnitX = new Vector4(1.0, 0.0, 0.0, 0.0);
        /// <summary>
        /// Returns new unit y vector (x = 0, y = 1, z = 0, w = 0)
        /// </summary>
        public static readonly Vector4 UnitY = new Vector4(0.0, 1.0, 0.0, 0.0);
        /// <summary>
        /// Returns new unit z vector (x = 0, y = 0, z = 1, w = 0)
        /// </summary>
        public static readonly Vector4 UnitZ = new Vector4(0.0, 0.0, 1.0, 0.0);
        /// <summary>
        /// Returns new unit w vector (x = 0, y = 0, z = 0, w = 1)
        /// </summary>
        public static readonly Vector4 UnitW = new Vector4(0.0, 0.0, 0.0, 1.0);

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double w { get; set; }
        public Vector3 xyz { get { return new Vector3(x, y, z); } }
        public Vector4(double x = 0.0, double y = 0.0, double z = 0.0, double w = 0.0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public Vector4(Vector2 vec, double z = 0.0, double w = 0.0)
        {
            x = vec.x;
            y = vec.y;
            this.z = z;
            this.w = w;
        }
        public Vector4(Vector3 vec, double w = 0.0)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
            this.w = w;
        }
        public Vector4(params double[] values)
        {
            if (values.Length != 4)
                throw new Exception("Array length must be 4.");

            x = values[0];
            y = values[1];
            z = values[2];
            w = values[3];
        }

        public static implicit operator Vector4(Vector4f vec) => new Vector4(vec.x, vec.y, vec.z);

        /// <summary>
        /// Magnitude of vector. Same as length
        /// </summary>
        public double magnitude()
        {
            return Math.Sqrt(squaredMagnitude());
        }
        /// <summary>
        /// Magnitude of vector without root. Same as squaredLength
        /// </summary>
        public double squaredMagnitude()
        {
            return dot(this);
        }
        /// <summary>
        /// Length of vector. Same as length
        /// </summary>
        public double length()
        {
            return magnitude();
        }
        /// <summary>
        /// Length of vector without root. Same as squaredMagnitude
        /// </summary>
        public double squaredLength()
        {
            return squaredMagnitude();
        }
        /// <summary>
        /// Checks if vector small enough to be considered a zero vector
        /// </summary>
        public bool isZero()
        {
            return squaredMagnitude() < Constants.SqrEpsilon;
        }
        public static Vector4 operator +(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }
        public static Vector4 operator -(Vector4 v1, Vector4 v2)
        {
            return new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }
        public static Vector4 operator *(Vector4 vec, double value)
        {
            return new Vector4(vec.x * value, vec.y * value, vec.z * value, vec.w * value);
        }
        public static Vector4 operator *(double value, Vector4 vec)
        {
            return new Vector4(vec.x * value, vec.y * value, vec.z * value, vec.w * value);
        }
        public static Vector4 operator /(Vector4 vec, double value)
        {
            return new Vector4(vec.x / value, vec.y / value, vec.z / value, vec.w / value);
        }
        public static Vector4 operator -(Vector4 vec)
        {
            return new Vector4(-vec.x, -vec.y, -vec.z, -vec.w);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public static double operator *(Vector4 v1, Vector4 v2)
        {
            return v1.dot(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public double dot(Vector4 vec)
        {
            return x * vec.x + y * vec.y + z * vec.z + w * vec.w;
        }
        /// <summary>
        /// Component multiplication
        /// </summary>
        /// <returns>New vector: (x1*x2, y1*y2, z1*z2, w1*w2)</returns>
        public Vector4 compMul(Vector4 vec)
        {
            return new Vector4(x * vec.x, y * vec.y, z * vec.z, w * vec.w);
        }
        /// <summary>
        /// Component division
        /// </summary>
        /// <returns>New vector: (x1/x2, y1/y2, z1/z2, w1/w2)</returns>
        public Vector4 compDiv(Vector4 vec)
        {
            return new Vector4(x / vec.x, y / vec.y, z / vec.z, w / vec.w);
        }
        /// <summary>
        /// Returns normalized copy of this vector
        /// </summary>
        public Vector4 normalized()
        {
            return this / magnitude();
        }
        /// <summary>
        /// Normalizes this vector
        /// </summary>
        public void normalize()
        {
            double magn = magnitude();
            x /= magn;
            y /= magn;
            z /= magn;
            w /= magn;
        }
        /// <summary>
        /// Checks if vectors are equal enough to be considered equal
        /// </summary>
        public bool equals(Vector4 vec)
        {
            return (vec - this).isZero();
        }
        /// <summary>
        /// Projects vector on another vector
        /// </summary>
        public Vector4 projectOnVector(Vector4 vec)
        {
            if (vec.isZero())
                return Vector4.Zero;
            return vec * (this * vec / vec.squaredMagnitude());
        }
        /// <summary>
        /// Projects vector on flat
        /// </summary>
        /// <param name="flatNorm">Normal vector to flat (not necessary normalized)</param>
        /// <returns></returns>
        public Vector4 projectOnFlat(Vector4 flatNorm)
        {
            return this - flatNorm * (this * flatNorm / flatNorm.squaredMagnitude());
        }
        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ", " + w.ToString() + ")";
        }
        public string ToString(string format)
        {
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ", " + w.ToString() + ")";
        }
    }
}
