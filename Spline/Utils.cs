using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace TOK.Utilities
{
    sealed public class Utils
    {
        /// <summary>
        /// Computes the square value of the argument
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Single"/> value to be squared
        /// </param>
        /// <returns>
        /// The <see cref="System.Single"/> value which is the square of the argument
        /// </returns>
        public static float squared(float x)
        {
            return x * x;
        }

        /// <summary>
        /// Computes the square value of the argument
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Double"/> value to be squared
        /// </param>
        /// <returns>
        /// The <see cref="System.Double"/> value which is the square of the argument
        /// </returns>
        public static double squared(double x)
        {
            return x * x;
        }

        /// <summary>
        /// Computes the cube value of the argument
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Single"/> value to be cubed
        /// </param>
        /// <returns>
        /// The <see cref="System.Single"/> value which is the cube of the argument
        /// </returns>
        public static float cubed(float x)
        {
            return x * x * x;
        }

        /// <summary>
        /// Computes the cube value of the argument
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Double"/> value to be cubed
        /// </param>
        /// <returns>
        /// The <see cref="System.Double"/> value which is the cube of the argument
        /// </returns>
        public static double cubed(double x)
        {
            return x * x * x;
        }

        /// <summary>
        /// Computes the rounded value of the argument
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Single"/> value to be rounded
        /// </param>
        /// <returns>
        /// The <see cref="System.Int16"/> value which is the round of the argument
        /// </returns>
        public static short Round(float x)
        {
            if (x > 0.0f)
                return (short)(x + 0.5f);

            if (x < 0.0f)
                return (short)(x - 0.5f);

            return 0;
        }

        /// <summary>
        /// Computes the rounded value of the argument
        /// </summary>
        /// <param name="x">
        /// The <see cref="System.Double"/> value to be rounded
        /// </param>
        /// <returns>
        /// The <see cref="System.Int16"/> value which is the round of the argument
        /// </returns>
        public static short Round(double x)
        {
            if (x > 0.0)
                return (short)(x + 0.5);

            if (x < 0.0)
                return (short)(x - 0.5);

            return 0;
        }

        /// <summary>
        /// Computes value clamped between min and max
        /// </summary>
        /// <param name="val">
        /// The value to be clamped
        /// </param>
        /// <param name="min">
        /// The minimum value to be clamped against
        /// </param>
        /// <param name="max">
        /// The maximum value to be clamped against
        /// </param>
        /// <returns>
        /// The value clamped between min and max
        /// </returns>
        /// <remarks>
        /// Input value as well as "min" and "max" shall implement IComparable<T> interface
        /// </remarks>
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            Debug.Assert(max.CompareTo(min) > 0);

            if (val.CompareTo(min) < 0)
                return min;

            if (val.CompareTo(max) > 0)
                return max;

            return val;
        }

        public static bool InRange(float val, float min, float max)
        {
            return (val >= min) && (val <= max);
        }

        /// <summary>
        /// Makes deep copy of the object via in-memory serialization
        /// </summary>
        /// <param name="obj">
        /// Object to be cloned
        /// </param>
        /// <returns>
        /// The deeply cloned copy of the object
        /// </returns>
        /// <remarks>
        /// Object must be marked as [Serializable]
        /// </remarks>
        public static T DeepClone<T>(T obj) // where T : ISerializable
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Linear spherical interpolation between two 3D points
        /// </summary>
        /// <param name="a">
        /// Start point
        /// </param>
        /// <param name="b">
        /// End point point
        /// </param>
        /// <param name="t">
        /// Value between 0 and 1, where interpolated point to be computed
        /// </param>
        /// <param name="theta">
        /// Angle between start and end point, radians
        /// </param>
        /// <returns>
        /// Interpolated point at position "t"
        /// </returns>
        /// <remarks>
        /// http://en.wikipedia.org/wiki/Slerp
        /// </remarks>
        public static Point3f Slerp(Point3f a, Point3f b, float t, float theta)
        {
            float norm = (float)Math.Sin(theta);
            float A = (1.0f - t);
            float B = t;
            if (norm != 0.0f)
            {
                norm = 1.0f / norm;
                A = (float)Math.Sin(A * theta) * norm;
                B = (float)Math.Sin(B * theta) * norm;
            }
            return new Point3f(A * a._x + B * b._x, A * a._y + B * b._y, A * a._z + B * b._z);
        }

        /// <summary>
        /// Makes cloned copy of the 2D plane
        /// </summary>
        /// <param name="src">
        /// Plane to be cloned
        /// </param>
        /// <returns>
        /// The cloned copy of the plane
        /// </returns>
        public static T[,] CopyPlane<T>(T[,] src) where T: struct
        {
            int nr = src.GetLength(0);
            int nc = src.GetLength(1);

            T[,] dst = new T[nr, nc];

            Buffer.BlockCopy(src, 0, dst, 0, Buffer.ByteLength(src));

            return dst;
        }

        /// <summary>
        /// Makes cloned copy of the volume (jagged array of plane slices)
        /// </summary>
        /// <param name="src">
        /// Volume to be cloned
        /// </param>
        /// <returns>
        /// The cloned copy of the object
        /// </returns>
        /// <remarks>
        /// Ran in parallel over all planes and copy plane one-by-one
        /// </remarks>
        public static T[][,] CopyVolume<T>(T[][,] src) where T: struct
        {
            int nz = src.Length;

            T[][,] dest = new T[nz][,];

            Parallel.For(0, nz, iz =>
            {
                T[,] pln = src[iz];

                dest[iz] = CopyPlane<T>(pln);
            });
            return dest;
        }

        /// <summary>
        /// Makes bit image out of the plane
        /// </summary>
        /// <param name="bm">
        /// Plane to be converted to bitmap
        /// </param>
        /// <param name="nr">
        /// Number of rows
        /// </param>
        /// <param name="nc">
        /// Number of columns
        /// </param>
        /// <returns>
        /// The bitmap where 1 are set for negative values
        /// </returns>
        /// <remarks>
        /// if there is no negative values, returns null
        /// </remarks>
        public static bool[,] MakeBitImage(short[,] bm, int nr, int nc)
        {
            if (bm == null)
                return null;

            bool[,] image = new bool[nr, nc];
            int sum = 0;
            for (int r = 0; r != nr; ++r)
            {
                for (int c = 0; c != nc; ++c)
                {
                    image[r, c] = false;
                    if (bm[r, c] < 0)
                    {
                        image[r, c] = true;
                        ++sum;
                    }
                }
            }
            if (sum == 0)
                return null;

            return image;
        }

        /// <summary>
        /// Computes center of mass of the contour
        /// </summary>
        /// <param name="contour">
        /// Set of 2D points
        /// </param>
        /// <returns>
        /// The center of mass of the contour
        /// </returns>
        /// <remarks>
        /// all points assumed to have the same weight
        /// </remarks>
        public static Point2f CenterOfMass(System.Drawing.Point[] contour)
        {
            int l = contour.Length;
            float x = 0.0f;
            float y = 0.0f;

            for (int k = 0; k != l; ++k)
            {
                x += (float)contour[k].X;
                y += (float)contour[k].Y;
            }

            float n = 1.0f / (float)l;
            return new Point2f(x * n, y * n);
        }

        /// <summary>
        /// Computes center of mass of the contour
        /// </summary>
        /// <param name="contour">
        /// Set of 2D points, X and Y in consequitive positions
        /// </param>
        /// <returns>
        /// The center of mass of the contour
        /// </returns>
        /// <remarks>
        /// all points assumed to have the same weight
        /// </remarks>
        public static Point2f CenterOfMass(float[] contour)
        {
            int l = contour.Length / 2;
            float x = 0.0f;
            float y = 0.0f;

            for (int k = 0; k != l; ++k)
            {
                int idx = 2*k;
                x += contour[idx];
                y += contour[idx + 1];
            }

            float n = 1.0f / (float)l;
            return new Point2f(x * n, y * n);
        }
    }
}
