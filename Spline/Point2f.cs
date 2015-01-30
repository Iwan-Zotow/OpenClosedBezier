using System;

namespace TOK.Utilities
{
    /// <summary>
    /// Represents an x- and  y-coordinate point in 2-D space, floating point values
    /// </summary>
    /// <remarks>
    /// Implements IEquatable<Point3f>, common operators, norm
    /// </remarks>
    public struct Point2f : IEquatable<Point2f>
    {
        internal float _x;
        internal float _y;

        /// <summary>
        /// Construct 2D point from X and Y coordinates
        /// </summary>
        /// <param name="x">
        /// X coordinate
        /// </param>
        /// <param name="y">
        /// Y coordinate
        /// </param>
        public Point2f(float x, float y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Construct 2D point from another 2D point, make a copy
        /// </summary>
        /// <param name="other">
        /// 2D point to be used as copy source
        /// </param>
        public Point2f(Point2f other)
        {
            _x = other._x;
            _y = other._y;
        }

        /// <summary>
        /// Get or set point X coordinate
        /// </summary>
        public float X { get { return _x; } set { _x = value; } }

        /// <summary>
        /// Get or set point Y coordinate
        /// </summary>
        public float Y { get { return _y; } set { _y = value; } }

#region Helpers
        /// <summary>
        /// Computes the difference between two points
        /// </summary>
        public static Point2f operator -(Point2f a, Point2f b)
        {
            return new Point2f(a._x - b._x, a._y - b._y);
        }

        /// <summary>
        /// Computes the sum of two points
        /// </summary>
        public static Point2f operator +(Point2f a, Point2f b)
        {
            return new Point2f(a._x + b._x, a._y + b._y);
        }

        /// <summary>
        /// Tests the equality of two points
        /// </summary>
        public static bool operator ==(Point2f a, Point2f b)
        {
            return (a._x == b._x) && (a._y == b._y);
        }

        /// <summary>
        /// Tests the unequality of two points
        /// </summary>
        public static bool operator !=(Point2f a, Point2f b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Ascending strict ordering of two points
        /// </summary>
        public static bool operator >(Point2f a, Point2f b)
        {
            return (a._x > b._x) && (a._y > b._y);
        }

        /// <summary>
        /// Descending strict ordering of two points
        /// </summary>
        public static bool operator <(Point2f a, Point2f b)
        {
            return (a._x < b._x) && (a._y < b._y);
        }

        /// <summary>
        /// Tests the equality of two points
        /// </summary>
        public static bool Equals(Point2f a, Point2f b)
        {
            return a == b;
        }

        /// <summary>
        /// Tests the equality of this point and object
        /// </summary>
        public override bool Equals(object o)
        {
            if ((null == o) || !(o is Point2f))
            {
                return false;
            }

            return this == (Point2f)o;
        }

        /// <summary>
        /// Tests the equality of this point and value
        /// </summary>
        public bool Equals(Point2f value)
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
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Computes squared distance between two points
        /// </summary>
        public static float Distance2(Point2f a, Point2f b)
        {
            return (Utils.squared(a._x - b._x) + Utils.squared(a._y - b._y));
        }

        /// <summary>
        /// Computes distance between two points
        /// </summary>
        public static float Distance(Point2f a, Point2f b)
        {
            return (float)Math.Sqrt(Distance2(a, b));
        }

        /// <summary>
        /// Computes squared norm of the point
        /// </summary>
        public static float Norm2(Point2f a)
        {
            return (Utils.squared(a._x) + Utils.squared(a._y));
        }

        /// <summary>
        /// Computes norm of the point
        /// </summary>
        public static float Norm(Point2f a)
        {
            return (float)Math.Sqrt(Norm2(a));
        }

        /// <summary>
        /// Computes the dot product of two points
        /// </summary>
        public static float Dot(Point2f a, Point2f b)
        {
            return a._x * b._x + a._y * b._y;
        }

        internal const float eps = 0.001f;

        /// <summary>
        /// Tests near-equality of two points
        /// </summary>
        public static bool AlmostEqual(Point2f a, Point2f b)
        {
            return Math.Abs(a._x - b._x) < eps && Math.Abs(a._y - b._y) < eps;
        }
#endregion
    }
}
