using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    /// <summary>
    /// 3 by 3 matrix with single precision in row-major order
    /// </summary>
    public struct Matrix3x3f
    {
        public float v00 { get; set; } // [rowIndex, columnIndex]
        public float v01 { get; set; }
        public float v02 { get; set; }
        public float v10 { get; set; }
        public float v11 { get; set; }
        public float v12 { get; set; }
        public float v20 { get; set; }
        public float v21 { get; set; }
        public float v22 { get; set; }

        /// <summary>
        /// Returns new identity matrix
        /// </summary>
        public static Matrix3x3f Identity
        {
            get
            {
                return new Matrix3x3f(1f, 0f, 0f,
                                      0f, 1f, 0f,
                                      0f, 0f, 1f);
            }
        }
        public Matrix3x3f(params float[] values)
        {
            if (values.Length != 9)
                throw new Exception("Array length must be 9.");
            v00 = values[0];
            v01 = values[1];
            v02 = values[2];
            v10 = values[3];
            v11 = values[4];
            v12 = values[5];
            v20 = values[6];
            v21 = values[7];
            v22 = values[8];
        }
        public Matrix3x3f(Vector3f vec1, Vector3f vec2, Vector3f vec3, bool rows = true)
        {
            if (rows)
            {
                v00 = vec1.x;
                v01 = vec1.y;
                v02 = vec1.z;
                v10 = vec2.x;
                v11 = vec2.y;
                v12 = vec2.z;
                v20 = vec3.x;
                v21 = vec3.y;
                v22 = vec3.z;
            }
            else
            {
                v00 = vec1.x;
                v01 = vec2.x;
                v02 = vec3.x;
                v10 = vec1.y;
                v11 = vec2.y;
                v12 = vec3.y;
                v20 = vec1.z;
                v21 = vec2.z;
                v22 = vec3.z;
            }
        }
        public static explicit operator Matrix3x3f(Matrix3x3 mat) => new Matrix3x3f((float)mat.v00, (float)mat.v01, (float)mat.v02,
                                                                                    (float)mat.v10, (float)mat.v11, (float)mat.v12,
                                                                                    (float)mat.v20, (float)mat.v21, (float)mat.v22);
        public static Matrix3x3f operator *(Matrix3x3f m1, Matrix3x3f m2)
        {
            return new Matrix3x3f(m1.v00 * m2.v00 + m1.v01 * m2.v10 + m1.v02 * m2.v20,
                                  m1.v00 * m2.v01 + m1.v01 * m2.v11 + m1.v02 * m2.v21,
                                  m1.v00 * m2.v02 + m1.v01 * m2.v12 + m1.v02 * m2.v22,

                                  m1.v10 * m2.v00 + m1.v11 * m2.v10 + m1.v12 * m2.v20,
                                  m1.v10 * m2.v01 + m1.v11 * m2.v11 + m1.v12 * m2.v21,
                                  m1.v10 * m2.v02 + m1.v11 * m2.v12 + m1.v12 * m2.v22,

                                  m1.v20 * m2.v00 + m1.v21 * m2.v10 + m1.v22 * m2.v20,
                                  m1.v20 * m2.v01 + m1.v21 * m2.v11 + m1.v22 * m2.v21,
                                  m1.v20 * m2.v02 + m1.v21 * m2.v12 + m1.v22 * m2.v22);
        }
        public static Matrix3x3f operator *(Matrix3x3f mat, float value)
        {
            return new Matrix3x3f(mat.v00 * value, mat.v01 * value, mat.v02 * value,
                                  mat.v10 * value, mat.v11 * value, mat.v12 * value,
                                  mat.v20 * value, mat.v21 * value, mat.v22 * value);
        }
        public static Matrix3x3f operator /(Matrix3x3f mat, float value)
        {
            if (value == 0)
                throw new DivideByZeroException();
            return new Matrix3x3f(mat.v00 / value, mat.v01 / value, mat.v02 / value,
                                  mat.v10 / value, mat.v11 / value, mat.v12 / value,
                                  mat.v20 / value, mat.v21 / value, mat.v22 / value);
        }
        public static Vector3f operator *(Matrix3x3f mat, Vector3f vec)
        {
            return new Vector3f(mat.v00 * vec.x + mat.v01 * vec.y + mat.v02 * vec.z,
                                mat.v10 * vec.x + mat.v11 * vec.y + mat.v12 * vec.z,
                                mat.v20 * vec.x + mat.v21 * vec.y + mat.v22 * vec.z);
        }
        public static Vector3f operator *(Vector3f vec, Matrix3x3f mat)
        {
            return new Vector3f(mat.v00 * vec.x + mat.v10 * vec.y + mat.v20 * vec.z,
                                mat.v01 * vec.x + mat.v11 * vec.y + mat.v21 * vec.z,
                                mat.v02 * vec.x + mat.v12 * vec.y + mat.v22 * vec.z);
        }
        /// <summary>
        /// Returns transposed copy of this matrix
        /// </summary>
        public Matrix3x3f transposed()
        {
            return new Matrix3x3f(v00, v10, v20,
                                  v01, v11, v21,
                                  v02, v12, v22);
        }
        /// <summary>
        /// Transposes this matrix
        /// </summary>
        public void transpose()
        {
            float tmp = v10;
            v10 = v01;
            v01 = tmp;

            tmp = v20;
            v20 = v02;
            v02 = tmp;

            tmp = v21;
            v21 = v12;
            v12 = tmp;
        }
        /// <summary>
        /// Returns inverse copy of this matrix
        /// </summary>
        public Matrix3x3f inverse()
        {
            float det00 = v11 * v22 - v12 * v21;
            float det01 = v10 * v22 - v12 * v20;
            float det02 = v10 * v21 - v11 * v20;

            float determinant = v00 * det00 - v01 * det01 + v02 * det02;
            if (determinant == 0)
                throw new Exception("This matrix is singular. (determinant = 0)");

            float det10 = v01 * v22 - v02 * v21;
            float det11 = v00 * v22 - v02 * v20;
            float det12 = v00 * v21 - v01 * v20;
            float det20 = v01 * v12 - v02 * v11;
            float det21 = v00 * v12 - v02 * v10;
            float det22 = v00 * v11 - v01 * v10;

            return new Matrix3x3f(det00 / determinant, -det10 / determinant, det20 / determinant,
                                 -det01 / determinant, det11 / determinant, -det21 / determinant,
                                  det02 / determinant, -det12 / determinant, det22 / determinant);
        }
        /// <summary>
        /// Inverts this matrix
        /// </summary>
        public void invert()
        {
            float det00 = v11 * v22 - v12 * v21;
            float det01 = v10 * v22 - v12 * v20;
            float det02 = v10 * v21 - v11 * v20;

            float determinant = v00 * det00 - v01 * det01 + v02 * det02;
            if (determinant == 0)
                throw new Exception("This matrix is singular. (determinant = 0)");

            float det10 = v01 * v22 - v02 * v21;
            float det11 = v00 * v22 - v02 * v20;
            float det12 = v00 * v21 - v01 * v20;
            float det20 = v01 * v12 - v02 * v11;
            float det21 = v00 * v12 - v02 * v10;
            float det22 = v00 * v11 - v01 * v10;

            v00 = det00 / determinant;
            v01 = -det10 / determinant;
            v02 = det20 / determinant;

            v10 = -det01 / determinant;
            v11 = det11 / determinant;
            v12 = -det21 / determinant;

            v20 = det02 / determinant;
            v21 = -det12 / determinant;
            v22 = det22 / determinant;
        }
        public static Matrix3x3f FromQuaternion(Quaternion q)
        {
            float s2 = 2.0f / (float)q.norm();
            float x = (float)q.x;
            float y = (float)q.y;
            float z = (float)q.z;
            float w = (float)q.w;
            return new Matrix3x3f(1 - s2 * (y * y + z * z), s2 * (x * y - w * z), s2 * (x * z + w * y),
                                  s2 * (x * y + w * z), 1 - s2 * (x * x + z * z), s2 * (y * z - w * x),
                                  s2 * (x * z - w * y), s2 * (y * z + w * x), 1 - s2 * (x * x + y * y));
        }
        public override string ToString()
        {
            return "| " + v00.ToString() + " " + v01.ToString() + " " + v02.ToString() + " |\n" +
                   "| " + v10.ToString() + " " + v11.ToString() + " " + v12.ToString() + " |\n" +
                   "| " + v20.ToString() + " " + v21.ToString() + " " + v22.ToString() + " |";
        }
        public string ToString(string format)
        {
            return "| " + v00.ToString(format) + " " + v01.ToString(format) + " " + v02.ToString(format) + " " + " |\n" +
                   "| " + v10.ToString(format) + " " + v11.ToString(format) + " " + v12.ToString(format) + " " + " |\n" +
                   "| " + v20.ToString(format) + " " + v21.ToString(format) + " " + v22.ToString(format) + " " + " |";
        }
    }
}
