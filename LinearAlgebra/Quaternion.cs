using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearAlgebra
{
    public enum EulerOrder
    {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX
    }
    // do NOT ask me about this, i don't give a fuck how this works. (times tried to understand: 2)
    public struct Quaternion
    {
        public double w, x, y, z;

        public static Quaternion Identity { get { return new Quaternion(1); } }
        public static Quaternion Zero { get { return new Quaternion(); } }
        public Quaternion(double w = 0, double x = 0, double y = 0, double z = 0)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// Dot product
        /// </summary>
        public double dot(Quaternion q)
        {
            return w * q.w + x * q.x + y * q.y + z * q.z;
        }
        /// <summary>
        /// Norm of quaternion
        /// </summary>
        public double norm()
        {
            return dot(this);
        }
        /// <summary>
        /// Length of quaternion. Same as magnitude
        /// </summary>
        public double length()
        {
            return Math.Sqrt(norm());
        }
        /// <summary>
        /// Magnitude of quaternion. Same as length
        /// </summary>
        public double magnitude()
        {
            return length();
        }
        /// <summary>
        /// Checks if quaternion small enough to be considered a zero quaternion
        /// </summary>
        public bool isZero()
        {
            return norm() < Constants.SqrEpsilon;
        }
        /// <summary>
        /// Normalizes this quaternion
        /// </summary>
        public void normalize()
        {
            double l = length();
            w /= l;
            x /= l;
            y /= l;
            z /= l;
        }
        /// <summary>
        /// Returns normalized copy of this quaternion
        /// </summary>
        public Quaternion normalized()
        {
            return this / length();
        }
        /// <summary>
        /// Conjugates this quaternion
        /// </summary>
        public void conjugate()
        {
            x = -x;
            y = -y;
            z = -z;
        }
        /// <summary>
        /// Returns conjugated copy of this quaternion
        /// </summary>
        public Quaternion conjugated()
        {
            return new Quaternion(w, -x, -y, -z);
        }
        /// <summary>
        /// Cross product
        /// </summary>
        public Quaternion cross(Quaternion q)
        {
            return new Quaternion(0.0, y * q.z - z * q.y, z * q.x - x * q.z, x * q.y - y * q.x);
        }
        /// <summary>
        /// Checks if quaternions are parallel enough to be considered collinear
        /// </summary>
        /// <returns>True if vectors are collinear, false otherwise</returns>
        public bool isCollinearTo(Quaternion q)
        {
            return cross(q).isZero();
        }
        /// <summary>
        /// Returns inverse copy of this quaternion
        /// </summary>
        public Quaternion inverse()
        {
            return conjugated() / norm();
        }
        /// <summary>
        /// Inverts this quaternion
        /// </summary>
        public void invert()
        {
            double n = norm();
            conjugate();
            w /= n;
            x /= n;
            y /= n;
            z /= n;
        }
        /// <summary>
        /// Creates quaternion that represents rotation around axis for angle
        /// </summary>
        public static Quaternion FromAxisAngle(Vector3 axis, double angle)
        {
            if (Math.Abs(axis.squaredLength() - 1.0) > Constants.SqrEpsilon)
                throw new ArgumentException("Axis is not normalized.");
            double sinhalf = Math.Sin(angle / 2.0);
            return new Quaternion(Math.Cos(angle / 2.0), axis.x * sinhalf, axis.y * sinhalf, axis.z * sinhalf);
        }
        /// <summary>
        /// Creates quaternion from euler angles in specified order
        /// </summary>
        public static Quaternion FromEuler(Vector3 euler, EulerOrder order = EulerOrder.ZXY)
        {
            double sinhalfx = Math.Sin(euler.x / 2.0);
            double sinhalfy = Math.Sin(euler.y / 2.0);
            double sinhalfz = Math.Sin(euler.z / 2.0);
            double coshalfx = Math.Cos(euler.x / 2.0);
            double coshalfy = Math.Cos(euler.y / 2.0);
            double coshalfz = Math.Cos(euler.z / 2.0);
            switch (order)
            {
                case EulerOrder.XYZ:
                    return new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0);
                case EulerOrder.XZY:
                    return new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0);
                case EulerOrder.YXZ:
                    return new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0);
                case EulerOrder.YZX:
                    return new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0);
                case EulerOrder.ZXY:
                    return new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz);
                case EulerOrder.ZYX:
                    return new Quaternion(coshalfx, sinhalfx, 0.0, 0.0) *
                           new Quaternion(coshalfy, 0.0, sinhalfy, 0.0) *
                           new Quaternion(coshalfz, 0.0, 0.0, sinhalfz);
            }
            throw new NotImplementedException();
        }

        public static Quaternion operator /(Quaternion lhs, double rhs)
        {
            return new Quaternion(lhs.w / rhs, lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }
        public static Quaternion operator *(Quaternion lhs, double rhs)
        {
            return new Quaternion(lhs.w * rhs, lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }
        public static Quaternion operator *(double lhs, Quaternion rhs)
        {
            return new Quaternion(lhs * rhs.w, lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }
        public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z,
                                  lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y,
                                  lhs.w * rhs.y - lhs.x * rhs.z + lhs.y * rhs.w + lhs.z * rhs.x,
                                  lhs.w * rhs.z + lhs.x * rhs.y - lhs.y * rhs.x + lhs.z * rhs.w);
        }
        public static Vector3 operator *(Quaternion lhs, Vector3 rhs)
        {
            Quaternion q = lhs * new Quaternion(0.0, rhs.x, rhs.y, rhs.z) * lhs.inverse();
            return new Vector3(q.x, q.y, q.z);
        }
        public static Quaternion operator +(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(lhs.w + rhs.w, lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        public static Quaternion operator -(Quaternion lhs, Quaternion rhs)
        {
            return new Quaternion(lhs.w - rhs.w, lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        //public static Quaternion operator *(Quaternion q, Vector3 v)
        //{
        //    return new Quaternion(-q.x * v.x - q.y - v.y - q.z * v.z,
        //                          q.w * v.x + q.y * v.z - q.z * v.y,
        //                          q.w * v.y + q.x * v.z - q.z * v.x,
        //                          q.w * v.z + q.x * v.y - q.y * v.x);
        //}
        ///// <summary>
        ///// Rotates vector v by quaternion q
        ///// </summary>
        //public Vector3 rotateVector(Vector3 vec)
        //{
        //    Quaternion result = this * vec * inversed();
        //    return new Vector3(result.x, result.y, result.z);
        //}
        ///// <summary>
        ///// Returns eulers representation of rotation in this quaternion
        ///// </summary>
        //public Vector3 toEuler()
        //{
        //    if (x * y + z * w == 0.5)
        //        return new Vector3(Math.Asin(2 * x * y + 2 * z * w), 2 * Math.Atan2(x, w), 0);
        //    else
        //    if (x * y + z * w == -0.5)
        //        return new Vector3(Math.Asin(2 * x * y + 2 * z * w), -2 * Math.Atan2(x, w), 0);
        //    else
        //        return new Vector3(Math.Asin(2 * x * y + 2 * z * w), Math.Atan2(2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z), Math.Atan2(2 * x * w - 2 * y * z, 1 - 2 * x * x - 2 * z * z));
        //}
        public override string ToString()
        {
            return "(" + w.ToString() + ", " + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
        }
        public string ToString(string format)
        {
            return "(" + w.ToString(format) + ", " + x.ToString(format) + ", " + y.ToString(format) + ", " + z.ToString(format) + ")";
        }
    }
}
