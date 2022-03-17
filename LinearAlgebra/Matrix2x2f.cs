using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    /// <summary>
    /// 2 by 2 matrix with single precision in row-major order
    /// </summary>
    public struct Matrix2x2f
    {
        public float v00 { get; set; } // [rowIndex, columnIndex]
        public float v01 { get; set; }
        public float v10 { get; set; }
        public float v11 { get; set; }

        /// <summary>
        /// Returns new identity matrix
        /// </summary>
        public static Matrix2x2f Identity
        {
            get
            {
                return new Matrix2x2f(1f, 0f,
                                      0f, 1f);
            }
        }
        public Matrix2x2f(params float[] values)
        {
            if (values.Length != 4)
                throw new Exception("Array length must be 4.");
            v00 = values[0];
            v01 = values[1];
            v10 = values[2];
            v11 = values[3];
        }
        public Matrix2x2f(Vector2f vec1, Vector2f vec2, bool rows = true)
        {
            if (rows)
            {
                v00 = vec1.x;
                v01 = vec1.y;
                v10 = vec2.x;
                v11 = vec2.y;
            }
            else
            {
                v00 = vec1.x;
                v01 = vec2.x;
                v10 = vec1.y;
                v11 = vec2.y;
            }
        }
        public static explicit operator Matrix2x2f(Matrix2x2 mat) => new Matrix2x2f((float)mat.v00, (float)mat.v01,
                                                                                    (float)mat.v10, (float)mat.v11);
        public static Matrix2x2f operator *(Matrix2x2f m1, Matrix2x2f m2)
        {
            return new Matrix2x2f(m1.v00 * m2.v00 + m1.v01 * m2.v10, m1.v00 * m2.v01 + m1.v01 * m2.v11,
                                  m1.v10 * m2.v00 + m1.v11 * m2.v10, m1.v10 * m2.v01 + m1.v11 * m2.v11);
        }
        public static Matrix2x2f operator *(Matrix2x2f mat, float value)
        {
            return new Matrix2x2f(mat.v00 * value, mat.v01 * value,
                                  mat.v10 * value, mat.v11 * value);
        }
        public static Matrix2x2f operator /(Matrix2x2f mat, float value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            return new Matrix2x2f(mat.v00 / value, mat.v01 / value,
                                  mat.v10 / value, mat.v11 / value);
        }
        public static Vector2f operator *(Matrix2x2f mat, Vector2f vec)
        {
            return new Vector2f(mat.v00 * vec.x + mat.v01 * vec.y, mat.v10 * vec.x + mat.v11 * vec.y);
        }
        public static Vector2f operator *(Vector2f vec, Matrix2x2f mat)
        {
            return new Vector2f(mat.v00 * vec.x + mat.v10 * vec.y, mat.v01 * vec.x + mat.v11 * vec.y);
        }
        /// <summary>
        /// Returns transposed copy of this matrix
        /// </summary>
        public Matrix2x2f transposed()
        {
            return new Matrix2x2f(v00, v10,
                                  v01, v11);
        }
        /// <summary>
        /// Transposes this matrix
        /// </summary>
        public void transpose()
        {
            float tmp = v10;
            v10 = v01;
            v01 = tmp;
        }
        /// <summary>
        /// Returns inverse copy of this matrix
        /// </summary>
        public Matrix2x2f inverse()
        {
            float determinant = v00 * v11 - v01 * v10;
            if (determinant == 0)
                throw new Exception("This matrix is singular. (determinant = 0)");

            return new Matrix2x2f(v11 / determinant, -v01 / determinant,
                                  -v10 / determinant, v00 / determinant);
        }
        /// <summary>
        /// Inverts this matrix
        /// </summary>
        public void invert()
        {
            float determinant = v00 * v11 - v01 * v10;
            if (determinant == 0)
                throw new Exception("This matrix is singular. (determinant = 0)");

            float tmp = v00;
            v00 = v11 / determinant;
            v11 = tmp / determinant;
            v01 = -v01 / determinant;
            v10 = -v10 / determinant;
        }
        public override string ToString()
        {
            return "| " + v00.ToString() + " " + v01.ToString() + " |\n" +
                   "| " + v10.ToString() + " " + v11.ToString() + " |";
        }
        public string ToString(string format)
        {
            return "| " + v00.ToString(format) + " " + v01.ToString(format) + " |\n" +
                   "| " + v10.ToString(format) + " " + v11.ToString(format) + " |";
        }
    }
}
