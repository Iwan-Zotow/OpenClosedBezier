using System;

namespace TOK.Utilities
{
    /// <summary>
    /// Represents an x-, y-, and z-coordinate point in 3-D space, floating point values
    /// </summary>
    /// <remarks>
    /// Implements IEquatable<Point3f>, common operators, norm
    /// </remarks>
    public struct Point3f : IEquatable<Point3f>
    {
        internal float _x; 
        internal float _y;
        internal float _z;

        /// <summary>
        /// Construct 3D point from X, Y  and Z coordinates
        /// </summary>
        /// <param name="x">
        /// X coordinate
        /// </param>
        /// <param name="y">
        /// Y coordinate
        /// </param>
        /// <param name="z">
        /// Z coordinate
        /// </param>
        public Point3f(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>
        /// Construct 3D point from another 3D point, make a copy
        /// </summary>
        /// <param name="other">
        /// 2D point to be used as copy source
        /// </param>
        public Point3f(Point3f other)
        {
            _x = other._x;
            _y = other._y;
            _z = other._z;
        }

        /// <summary>
        /// Get or set point X coordinate
        /// </summary>
        public float X { get { return _x; } set { _x = value; } }

        /// <summary>
        /// Get or set point Y coordinate
        /// </summary>
        public float Y { get { return _y; } set { _y = value; } }

        /// <summary>
        /// Get or set point Z coordinate
        /// </summary>
        public float Z { get { return _z; } set { _z = value; } }

#region Helpers
        /// <summary>
        /// Computes the difference between two points
        /// </summary>
        public static Point3f operator -(Point3f a, Point3f b)
        {
            return new Point3f(a._x - b._x, a._y - b._y, a._z - b._z);
        }

        /// <summary>
        /// Computes the sum of two points
        /// </summary>
        public static Point3f operator +(Point3f a, Point3f b)
        {
            return new Point3f(a._x + b._x, a._y + b._y, a._z + b._z);
        }

        /// <summary>
        /// Tests the equality of two points
        /// </summary>
        public static bool operator ==(Point3f a, Point3f b)
        {
            return (a._z == b._z) && (a._x == b._x) && (a._y == b._y);
        }

        /// <summary>
        /// Tests the unequality of two points
        /// </summary>
        public static bool operator !=(Point3f a, Point3f b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Ascending strict ordering of two points
        /// </summary>
        /// <returns>true if points are ordered, false otherwise</returns>
        public static bool operator >(Point3f a, Point3f b)
        {
            return (a._z > b._z) && (a._x > b._x) && (a._y > b._y);
        }

        /// <summary>
        /// Non-strict ascending oredering of the two points
        /// </summary>
        /// <param name="a">Point A</param>
        /// <param name="b">Point B</param>
        /// <returns>true if points are ordered, false otherwise</returns>
        public static bool operator >=(Point3f a, Point3f b)
        {
            return (a._z >= b._z) && (a._x >= b._x) && (a._y >= b._y);
        }

        /// <summary>
        /// Descending strict ordering of two points
        /// </summary>
        public static bool operator <(Point3f a, Point3f b)
        {
            return (a._z < b._z) && (a._x < b._x) && (a._y < b._y);
        }

        /// <summary>
        /// Non-stric descending ordering of two points
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <returns>true if points are ordered, false otherwise</returns>
        public static bool operator <=(Point3f a, Point3f b)
        {
            return (a._z <= b._z) && (a._x <= b._x) && (a._y <= b._y);
        }

        /// <summary>
        /// Tests the equality of two points
        /// </summary>
        public static bool Equals(Point3f a, Point3f b)
        {
            return a == b;
        }

        /// <summary>
        /// Tests the equality of this point and object
        /// </summary>
        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point3f))
            {
                return false;
            }

            return this == (Point3f)o;
        }

        /// <summary>
        /// Tests the equality of this point and value
        /// </summary>
        public bool Equals(Point3f value)
        {
            return this == value;
        }

        /// <summary>
        /// Computes point hash code
        /// </summary>
        /// <remarks>
        /// field-by-field XOR of coordinates HashCodes
        /// </remarks>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^
                   Y.GetHashCode() ^
                   Z.GetHashCode();
        }

        /// <summary>
        /// Computes squared distance between two points
        /// </summary>
        public static float Distance2(Point3f a, Point3f b)
        {
            return (Utils.squared(a._x - b._x) + Utils.squared(a._y - b._y) + Utils.squared(a._z - b._z));
        }

        /// <summary>
        /// Computes distance between two points
        /// </summary>
        public static float Distance(Point3f a, Point3f b)
        {
            return (float)Math.Sqrt(Distance2(a, b));
        }

        /// <summary>
        /// Computes squared norm of the point
        /// </summary>
        public static float Norm2(Point3f a)
        {
            return (Utils.squared(a._x) + Utils.squared(a._y) + Utils.squared(a._z));
        }

        /// <summary>
        /// Computes norm of the point
        /// </summary>
        public static float Norm(Point3f a)
        {
            return (float)Math.Sqrt(Norm2(a));
        }

        /// <summary>
        /// Computes the dot product of two points
        /// </summary>
        public static float Dot(Point3f a, Point3f b)
        {
            return a._x * b._x + a._y * b._y + a._z * b._z;
        }

        internal const float eps = 0.001f;

        /// <summary>
        /// Tests near-equality of two points
        /// </summary>
        public static bool AlmostEqual(Point3f a, Point3f b)
        {
            return Math.Abs(a._x - b._x) < eps && Math.Abs(a._y - b._y) < eps && Math.Abs(a._z - b._z) < eps;
        }
#endregion
    }
}
