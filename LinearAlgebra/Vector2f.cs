using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    /// <summary>
    /// 2-dimensional vector with single precision
    /// </summary>
    public struct Vector2f
    {
        /// <summary>
        /// Returns new zero vector
        /// </summary>
        public static readonly Vector2f Zero = new Vector2f();
        /// <summary>
        /// Returns new unit x vector (x = 1, y = 0)
        /// </summary>
        public static readonly Vector2f UnitX = new Vector2f(1f, 0f);
        /// <summary>
        /// Returns new unit y vector (x = 0, y = 1)
        /// </summary>
        public static readonly Vector2f UnitY = new Vector2f(0f, 1f);

        public float x { get; set; }
        public float y { get; set; }
        public Vector2f(float x = 0f, float y = 0f)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2f(float[] values)
        {
            if (values.Length != 2)
                throw new Exception("Array length must be 2.");

            x = values[0];
            y = values[1];
        }

        public static explicit operator Vector2f(Vector2 vec) => new Vector2f((float)vec.x, (float)vec.y);

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
        public static Vector2f operator +(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector2f operator -(Vector2f v1, Vector2f v2)
        {
            return new Vector2f(v1.x - v2.x, v1.y - v2.y);
        }
        public static Vector2f operator *(Vector2f vec, float value)
        {
            return new Vector2f(vec.x * value, vec.y * value);
        }
        public static Vector2f operator *(float value, Vector2f vec)
        {
            return new Vector2f(vec.x * value, vec.y * value);
        }
        public static Vector2f operator /(Vector2f vec, float value)
        {
            return new Vector2f(vec.x / value, vec.y / value);
        }
        public static Vector2f operator -(Vector2f vec)
        {
            return new Vector2f(-vec.x, -vec.y);
        }
        /// <summary>
        /// Cross product
        /// </summary>
        public static float operator %(Vector2f v1, Vector2f v2)
        {
            return v1.vecMul(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public static float operator *(Vector2f v1, Vector2f v2)
        {
            return v1.dot(v2);
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public float dot(Vector2f vec)
        {
            return x * vec.x + y * vec.y;
        }
        /// <summary>
        /// Component multiplication
        /// </summary>
        /// <returns>New vector: (x1*x2, y1*y2)</returns>
        public Vector2f compMul(Vector2f vec)
        {
            return new Vector2f(x * vec.x, y * vec.y);
        }
        /// <summary>
        /// Component division
        /// </summary>
        /// <returns>New vector: (x1/x2, y1/y2)</returns>
        public Vector2f compDiv(Vector2f vec)
        {
            return new Vector2f(x / vec.x, y / vec.y);
        }
        /// <summary>
        /// Returns normalized copy of this vector
        /// </summary>
        public Vector2f normalized()
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
        }
        /// <summary>
        /// Checks if vectors are equal enough to be considered equal
        /// </summary>
        public bool equals(Vector2f vec)
        {
            return (vec - this).isZero();
        }
        /// <summary>
        /// Projects vector on another vector
        /// </summary>
        public Vector2f projectOnVector(Vector2f vec)
        {
            if (vec.isZero())
                return Vector2f.Zero;
            return vec * (this * vec / vec.squaredMagnitude());
        }
        /// <summary>
        /// Cross product. Same as cross
        /// </summary>
        public float vecMul(Vector2f vec)
        {
            return x * vec.y - y * vec.x;
        }
        /// <summary>
        /// Cross product. Same as vecMul
        /// </summary>
        public float cross(Vector2f vec)
        {
            return vecMul(vec);
        }
        /// <summary>
        /// Checks if vectors are parallel enough to be considered collinear
        /// </summary>
        /// <returns>True if vectors are collinear, false otherwise</returns>
        public bool isCollinearTo(Vector2f vec)
        {
            return Math.Abs(this % vec) < Constants.FloatEpsilon;
        }
        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }
        public string ToString(string format)
        {
            return "(" + x.ToString(format) + ", " + y.ToString(format) + ")";
        }
    }
}
