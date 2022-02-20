using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    /// <summary>
    /// 3-dimensional vector with double precision
    /// </summary>
    public struct Vector3
    {
        /// <summary>
        /// Returns new zero vector
        /// </summary>
        public static readonly Vector3 Zero = new Vector3();

        // x - right, y - forward, z - up
        /// <summary>
        /// Returns new right vector (x = 1, y = 0, z = 0). Same as UnitX
        /// </summary>
        public static readonly Vector3 Right = new Vector3(1.0, 0.0, 0.0);
        /// <summary>
        /// Returns new forward vector (x = 0, y = 1, z = 0). Same as UnitY
        /// </summary>
        public static readonly Vector3 Forward = new Vector3(0.0, 1.0, 0.0);
        /// <summary>
        /// Returns new up vector (x = 0, y = 0, z = 1). Same as UnitZ
        /// </summary>
        public static readonly Vector3 Up = new Vector3(0.0, 0.0, 1.0);
        /// <summary>
        /// Returns new unit x vector (x = 1, y = 0, z = 0). Same as Right
        /// </summary>
        public static readonly Vector3 UnitX = new Vector3(1.0, 0.0, 0.0);
        /// <summary>
        /// Returns new unit y vector (x = 0, y = 1, z = 0). Same as Forward
        /// </summary>
        public static readonly Vector3 UnitY = new Vector3(0.0, 1.0, 0.0);
        /// <summary>
        /// Returns new unit z vector (x = 0, y = 0, z = 1). Same as Up
        /// </summary>
        public static readonly Vector3 UnitZ = new Vector3(0.0, 0.0, 1.0);

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Vector2 xy { get { return new Vector2(x, y); } }
        public Vector3(double x = 0.0, double y = 0.0, double z = 0.0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3(Vector2 vec, double z = 0.0)
        {
            x = vec.x;
            y = vec.y;
            this.z = z;
        }
        public Vector3(params double[] values)
        {
            if (values.Length != 3)
                throw new Exception("Array length must be 3.");

            x = values[0];
            y = values[1];
            z = values[2];
        }

        public static implicit operator Vector3(Vector3f vec) => new Vector3(vec.x, vec.y, vec.z);

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
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static Vector3 operator *(Vector3 vec, double value)
        {
            return new Vector3(vec.x * value, vec.y * value, vec.z * value);
        }
        public static Vector3 operator *(double value, Vector3 vec)
        {
            return new Vector3(vec.x * value, vec.y * value, vec.z * value);
        }
        public static Vector3 operator /(Vector3 vec, double value)
        {
            return new Vector3(vec.x / value, vec.y / value, vec.z / value);
        }
        public static Vector3 operator -(Vector3 vec)
        {
            return new Vector3(-vec.x, -vec.y, -vec.z);
        }
        /// <summary>
        /// Cross product
        /// </summary>
        public static Vector3 operator %(Vector3 v1, Vector3 v2)
        {
            return v1.vecMul(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public static double operator *(Vector3 v1, Vector3 v2)
        {
            return v1.dot(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public double dot(Vector3 vec)
        {
            return x * vec.x + y * vec.y + z * vec.z;
        }
        /// <summary>
        /// Component multiplication
        /// </summary>
        /// <returns>New vector: (x1*x2, y1*y2, z1*z2)</returns>
        public Vector3 compMul(Vector3 vec)
        {
            return new Vector3(x * vec.x, y * vec.y, z * vec.z);
        }
        /// <summary>
        /// Component division
        /// </summary>
        /// <returns>New vector: (x1/x2, y1/y2, z1/z2)</returns>
        public Vector3 compDiv(Vector3 vec)
        {
            return new Vector3(x / vec.x, y / vec.y, z / vec.z);
        }
        /// <summary>
        /// Returns normalized copy of this vector
        /// </summary>
        public Vector3 normalized()
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
        }
        /// <summary>
        /// Checks if vectors are equal enough to be considered equal
        /// </summary>
        public bool equals(Vector3 vec)
        {
            return (vec - this).isZero();
        }
        /// <summary>
        /// Projects vector on another vector
        /// </summary>
        public Vector3 projectOnVector(Vector3 vec)
        {
            if (vec.isZero())
                return Vector3.Zero;
            return vec * (this * vec / vec.squaredMagnitude());
        }
        /// <summary>
        /// Projects vector on flat
        /// </summary>
        /// <param name="flatNorm">Normal vector to flat (not necessary normalized)</param>
        /// <returns></returns>
        public Vector3 projectOnFlat(Vector3 flatNorm)
        {
            return this - flatNorm * (this * flatNorm / flatNorm.squaredMagnitude());
        }
        /// <summary>
        /// Cross product. Same as cross
        /// </summary>
        public Vector3 vecMul(Vector3 vec)
        {
            return new Vector3(y * vec.z - z * vec.y, z * vec.x - x * vec.z, x * vec.y - y * vec.x);
        }
        /// <summary>
        /// Cross product. Same as vecMul
        /// </summary>
        public Vector3 cross(Vector3 vec)
        {
            return vecMul(vec);
        }
        /// <summary>
        /// Checks if vectors are parallel enough to be considered collinear
        /// </summary>
        /// <returns>True if vectors are collinear, false otherwise</returns>
        public bool isCollinearTo(Vector3 vec)
        {
            return (this % vec).isZero();
        }
        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }
        public string ToString(string format)
        {
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")";
        }
    }
}
