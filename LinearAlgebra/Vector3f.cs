using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    /// <summary>
    /// 3-dimensional vector with single precision
    /// </summary>
    public struct Vector3f
    {
        /// <summary>
        /// Returns new zero vector
        /// </summary>
        public static readonly Vector3f Zero = new Vector3f();

        // x - right, y - forward, z - up
        /// <summary>
        /// Returns new right vector (x = 1, y = 0, z = 0). Same as UnitX
        /// </summary>
        public static readonly Vector3f Right = new Vector3f(1f, 0f, 0f);
        /// <summary>
        /// Returns new forward vector (x = 0, y = 1, z = 0). Same as UnitY
        /// </summary>
        public static readonly Vector3f Forward = new Vector3f(0f, 1f, 0f);
        /// <summary>
        /// Returns new up vector (x = 0, y = 0, z = 1). Same as UnitZ
        /// </summary>
        public static readonly Vector3f Up = new Vector3f(0f, 0f, 1f);
        /// <summary>
        /// Returns new unit x vector (x = 1, y = 0, z = 0). Same as Right
        /// </summary>
        public static readonly Vector3f UnitX = new Vector3f(1f, 0f, 0f);
        /// <summary>
        /// Returns new unit y vector (x = 0, y = 1, z = 0). Same as Forward
        /// </summary>
        public static readonly Vector3f UnitY = new Vector3f(0f, 1f, 0f);
        /// <summary>
        /// Returns new unit z vector (x = 0, y = 0, z = 1). Same as Up
        /// </summary>
        public static readonly Vector3f UnitZ = new Vector3f(0f, 0f, 1f);

        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public Vector2f xy { get { return new Vector2f(x, y); } }
        public Vector3f(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3f(Vector2f vec, float z = 0f)
        {
            x = vec.x;
            y = vec.y;
            this.z = z;
        }
        public Vector3f(params float[] values)
        {
            if (values.Length != 3)
                throw new Exception("Array length must be 3.");

            x = values[0];
            y = values[1];
            z = values[2];
        }

        public static explicit operator Vector3f(Vector3 vec) => new Vector3f((float)vec.x, (float)vec.y, (float)vec.z);

        /// <summary>
        /// Magnitude of vector. Same as length
        /// </summary>
        public float magnitude()
        {
            return (float)Math.Sqrt(squaredMagnitude());
        }
        /// <summary>
        /// Magnitude of vector without root. Same as squaredLength
        /// </summary>
        public float squaredMagnitude()
        {
            return dot(this);
        }
        /// <summary>
        /// Length of vector. Same as length
        /// </summary>
        public float length()
        {
            return magnitude();
        }
        /// <summary>
        /// Length of vector without root. Same as squaredMagnitude
        /// </summary>
        public float squaredLength()
        {
            return squaredMagnitude();
        }
        /// <summary>
        /// Checks if vector small enough to be considered a zero vector
        /// </summary>
        public bool isZero()
        {
            return squaredMagnitude() < Constants.SqrFloatEpsilon;
        }
        public static Vector3f operator +(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3f operator -(Vector3f v1, Vector3f v2)
        {
            return new Vector3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }
        public static Vector3f operator *(Vector3f vec, float value)
        {
            return new Vector3f(vec.x * value, vec.y * value, vec.z * value);
        }
        public static Vector3f operator *(float value, Vector3f vec)
        {
            return new Vector3f(vec.x * value, vec.y * value, vec.z * value);
        }
        public static Vector3f operator /(Vector3f vec, float value)
        {
            return new Vector3f(vec.x / value, vec.y / value, vec.z / value);
        }
        public static Vector3f operator -(Vector3f vec)
        {
            return new Vector3f(-vec.x, -vec.y, -vec.z);
        }
        /// <summary>
        /// Cross product
        /// </summary>
        public static Vector3f operator %(Vector3f v1, Vector3f v2)
        {
            return v1.vecMul(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public static float operator *(Vector3f v1, Vector3f v2)
        {
            return v1.dot(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public float dot(Vector3f vec)
        {
            return x * vec.x + y * vec.y + z * vec.z;
        }
        /// <summary>
        /// Component multiplication
        /// </summary>
        /// <returns>New vector: (x1*x2, y1*y2, z1*z2)</returns>
        public Vector3f compMul(Vector3f vec)
        {
            return new Vector3f(x * vec.x, y * vec.y, z * vec.z);
        }
        /// <summary>
        /// Component division
        /// </summary>
        /// <returns>New vector: (x1/x2, y1/y2, z1/z2)</returns>
        public Vector3f compDiv(Vector3f vec)
        {
            return new Vector3f(x / vec.x, y / vec.y, z / vec.z);
        }
        /// <summary>
        /// Returns normalized copy of this vector
        /// </summary>
        public Vector3f normalized()
        {
            return this / magnitude();
        }
        /// <summary>
        /// Normalizes this vector
        /// </summary>
        public void normalize()
        {
            float magn = magnitude();
            x /= magn;
            y /= magn;
            z /= magn;
        }
        /// <summary>
        /// Checks if vectors are equal enough to be considered equal
        /// </summary>
        public bool equals(Vector3f vec)
        {
            return (vec - this).isZero();
        }
        /// <summary>
        /// Projects vector on another vector
        /// </summary>
        public Vector3f projectOnVector(Vector3f vec)
        {
            if (vec.isZero())
                return Vector3f.Zero;
            return vec * (this * vec / vec.squaredMagnitude());
        }
        /// <summary>
        /// Projects vector on flat
        /// </summary>
        /// <param name="flatNorm">Normal vector to flat (not necessary normalized)</param>
        /// <returns></returns>
        public Vector3f projectOnFlat(Vector3f flatNorm)
        {
            return this - flatNorm * (this * flatNorm / flatNorm.squaredMagnitude());
        }
        /// <summary>
        /// Cross product. Same as cross
        /// </summary>
        public Vector3f vecMul(Vector3f vec)
        {
            return new Vector3f(y * vec.z - z * vec.y, z * vec.x - x * vec.z, x * vec.y - y * vec.x);
        }
        /// <summary>
        /// Cross product. Same as vecMul
        /// </summary>
        public Vector3f cross(Vector3f vec)
        {
            return vecMul(vec);
        }
        /// <summary>
        /// Checks if vectors are parallel enough to be considered collinear
        /// </summary>
        /// <returns>True if vectors are collinear, false otherwise</returns>
        public bool isCollinearTo(Vector3f vec)
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
