using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TOK.Utilities
{
    /// <summary>
    /// Open contour spline implementation
    /// </summary>
    public class OpenBezpline : Bezpline
    {
        /// <summary>
        /// Construct spline from points and tension
        /// </summary>
        /// <param name="points">Array of pairs of coordinates x0,y0,x1,y1,...</param>
        /// <param name="tension">Spline tension</param>
        public OpenBezpline(float[] points, float tension) :
            base(tension)
        {
            _length = points.Length / 2 - 1; // number of segments in open case is one less than number of knots

            if (_length < 2)
                throw new ArgumentException("Not enough points to contruct open spline");

            _knots = new Point2f[_length + 1];

            for (int k = 0; k != _knots.Length; ++k)
            {
                _knots[k] = new Point2f(points[2 * k], points[2 * k + 1]);
            }

            _controls = InitCPs(_knots, _tension);
        }

        /// <summary>
        /// Construct spline from points and tension
        /// </summary>
        /// <param name="points">Array of 2D points</param>
        /// <param name="tension">Spline tension</param>
        public OpenBezpline(Point2f[] points, float tension) :
            base(tension)
        {
            _length = points.Length - 1;

            if (_length < 2)
                throw new ArgumentException("Not enough points to contruct open spline");

            _knots = new Point2f[points.Length];

            Array.Copy(points, _knots, points.Length);

            _controls = InitCPs(_knots, _tension);
        }

        /// <summary>
        /// Control points initialization
        /// </summary>
        /// <param name="knots">Spline knots</param>
        /// <param name="l">Length of CPs</param>
        /// <param name="tension">Spline tension</param>
        /// <returns>Control points in x,y format</returns>
        private static Point2f[] InitCPs(Point2f[] knots, float tension)
        {
            int l = knots.Length - 2;
            var ctrl_points = new List<Point2f>();
            for (int k = 0; k != l; ++k)
            {
                var cps = GetControlPoints(ref knots[k],
                                           ref knots[k + 1],
                                           ref knots[k + 2],
                                           tension);
                ctrl_points.Add(cps.Key);
                ctrl_points.Add(cps.Value);
            }
            return ctrl_points.ToArray();
        }

        /// <summary>
        /// Computes position of the pair of control points given three spline knots and tension
        /// </summary>
        /// <param name="p0">The start knot</param>
        /// <param name="p1">The end knot</param>
        /// <param name="p2">The next knot</param>
        /// <param name="t">tension which controls how far the control points spread.</param>
        /// <returns>
        /// cp1 is the control point calculated here, from p1 back toward p0,
        /// cp2 is the next control point, calculated here and returned to become the next segment's cp1
        /// </returns>
        /// <remarks>Scaling factors: distances from this knot to the previous and following knots.</remarks>
        private static KeyValuePair<Point2f, Point2f> GetControlPoints(ref Point2f p0,
                                                                       ref Point2f p1,
                                                                       ref Point2f p2,
                                                                       float t)
        {
            float d01 = Point2f.Distance(p0, p1);
            float d12 = Point2f.Distance(p1, p2);

            float fa = t * d01 / (d01 + d12);
            float fb = t - fa;

            return new KeyValuePair<Point2f, Point2f>
                       (
                            new Point2f(p1.X + fa * (p0.X - p2.X), p1.Y + fa * (p0.Y - p2.Y)),
                            new Point2f(p1.X - fb * (p0.X - p2.X), p1.Y - fb * (p0.Y - p2.Y))
                       );
        }

        /// <summary>
        /// Calculate interpolated value for a given segment
        /// </summary>
        /// <param name="k">Segment index, in the range [0...Length-1]</param>
        /// <param name="s">Interpolation position, must be in [0...1] range, at 0 start knot, at 1 end knot</param>
        /// <returns>Interplated point</returns>
        public override Point2f Calculate(int k, float s)
        {
            Debug.Assert(s >= 0.0f);
            Debug.Assert(s <= 1.0f);

            // first segment, can do only quadratic spline
            if (k == 0)
                return Calculate(ref _knots[0], ref _controls[0], ref _knots[1], s);

            // last segment, can do only quadratic spline
            if (k == _length - 1)
                return Calculate(ref _knots[k], ref _controls[_controls.Length - 1], ref _knots[k + 1], s);

            // somewhere in the middle, can do cubic
            return Calculate(ref _knots[k],
                             ref _controls[2 * k - 1],
                             ref _controls[2 * k],
                             ref _knots[k + 1],
                             s);
        }
    }
}
