using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TOK.Utilities
{
    /// <summary>
    /// Bezier Splines, inspired by http://scaledinnovation.com/analytics/splines/aboutSplines.html,
    /// </summary>
    public abstract class Bezpline
    {
#region Data
        /// <summary>
        /// The spline knots
        /// </summary>
        protected Point2f[] _knots = null;

        /// <summary>
        /// The spline tension
        /// </summary>
        protected float _tension = Single.NaN;

        /// <summary>
        /// The spline control points
        /// </summary>
        protected Point2f[] _controls = null;

        /// <summary>
        /// Number of segments in the spline
        /// </summary>
        protected int _length = -1;
#endregion

#region Properties
        /// <summary>
        /// Returns spline knots
        /// </summary>
        public Point2f[] Knots
        {
            get
            {
                return _knots;
            }
        }

        /// <summary>
        /// Return spline controls
        /// </summary>
        public Point2f[] Controls
        {
            get
            {
                return _controls;
            }
        }

        /// <summary>
        /// Returns spline tension
        /// </summary>
        public float Tension
        {
            get
            {
                return _tension;
            }
        }

        /// <summary>
        /// Number of segments in the spline
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }
        }
#endregion

        /// <summary>
        /// Common contructor
        /// </summary>
        /// <param name="tension">spline tension</param>
        public Bezpline(float tension)
        {
            _tension = tension;
        }

        /// <summary>
        /// Calculate interpolated value using cubic Bezier expression
        /// </summary>
        /// <param name="p0">Start knot</param>
        /// <param name="cpA">Control point A</param>
        /// <param name="cpB">Control point B</param>
        /// <param name="p1">End knot</param>
        /// <param name="s">Interpolation position, must be in [0...1] range, @0 start knot, @1 end knot</param>
        /// <returns>Interpolated point</returns>
        public static Point2f Calculate(ref Point2f p0,
                                        ref Point2f cpA,
                                        ref Point2f cpB,
                                        ref Point2f p1,
                                        float s)
        {
            const float THREE = 3.0f;

            Debug.Assert(s >= 0.0f);
            Debug.Assert(s <= 1.0f);

            float one_m_s = 1.0f - s;

            float A = one_m_s * one_m_s * one_m_s;
            float B = one_m_s * one_m_s * s * THREE;
            float C = one_m_s * s * s * THREE;
            float D = s * s * s;

            return new Point2f(p0.X * A + cpA.X * B + cpB.X * C + p1.X * D,
                               p0.Y * A + cpA.Y * B + cpB.Y * C + p1.Y * D);
        }

        /// <summary>
        /// Calculate interpolated value using quadratic Bezier expression
        /// </summary>
        /// <param name="p0">Start knot</param>
        /// <param name="cp">Control point</param>
        /// <param name="p1">End knot</param>
        /// <param name="s">Interpolation position, must be in [0...1] range, @0 start knot, @1 end knot</param>
        /// <returns>Interpolated point</returns>
        public static Point2f Calculate(ref Point2f p0,
                                        ref Point2f cp,
                                        ref Point2f p1,
                                        float s)
        {
            Debug.Assert(s >= 0.0f);
            Debug.Assert(s <= 1.0f);

            float one_m_s = 1.0f - s;

            float A = one_m_s * one_m_s;
            float B = 2.0f * one_m_s * s;
            float C = s * s;

            return new Point2f(p0.X * A + cp.X * B + p1.X * C,
                               p0.Y * A + cp.Y * B + p1.Y * C);
        }

        abstract public Point2f Calculate(int k, float s);
    }
}
