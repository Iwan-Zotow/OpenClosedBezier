using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace TOK.Utilities
{
    /// <summary>
    /// Closed contour spline implementation
    /// </summary>
    public class ClosedBezpline : Bezpline
    {
        /// <summary>
        /// Construct spline from points and tension
        /// </summary>
        /// <param name="points">Array of pairs of coordinates x0,y0,x1,y1,...</param>
        /// <param name="tension">Spline tension</param>
        public ClosedBezpline(float[] points, float tension) :
            base(tension)
        {
            int l = points.Length;

            _length = l / 2;

            if (_length < 3)
                throw new ArgumentException("Not enough points to contruct closed spline");

            _knots = new Point2f[l / 2 + 3];

            _knots[0] = new Point2f(points[l - 2], points[l - 1]);

            for (int k = 1; k != l / 2 + 1; ++k)
            {
                _knots[k] = new Point2f(points[2 * k - 2], points[2 * k - 1]);
            }

            _knots[l / 2 + 1] = new Point2f(points[0], points[1]);
            _knots[l / 2 + 2] = new Point2f(points[2], points[3]);

            _controls = InitCPs(_knots, l, _tension);
        }

        /// <summary>
        /// Construct spline from points and tension
        /// </summary>
        /// <param name="points">Array of 2D points</param>
        /// <param name="tension">Spline tension</param>
        public ClosedBezpline(Point2f[] points, float tension) :
            base(tension)
        {
            _length = points.Length;

            if (_length < 3)
                throw new ArgumentException("Not enough points to contruct closed spline");

            int l = 2 * _length;

            _knots = new Point2f[l / 2 + 3];

            _knots[0] = new Point2f(points[points.Length - 1]);

            for (int k = 1; k != l / 2 + 1; ++k)
            {
                _knots[k] = new Point2f(points[k - 1]);
            }

            _knots[l / 2 + 1] = new Point2f(points[0]);
            _knots[l / 2 + 2] = new Point2f(points[1]);

            _controls = InitCPs(_knots, l, _tension);
        }

        /// <summary>
        /// Control points initialization
        /// </summary>
        /// <param name="knots">Spline knots</param>
        /// <param name="l">Length of CPs</param>
        /// <param name="tension">Spline tension</param>
        /// <returns>Control points in x,y format</returns>
        private static Point2f[] InitCPs(Point2f[] knots, int l, float tension)
        {
            var ctrl_points = new List<Point2f>();
            for (int k = 0; k != l / 2; ++k)
            {
                var cps = GetControlPoints(ref knots[k],
                                           ref knots[k + 1],
                                           ref knots[k + 2],
                                           tension);
                ctrl_points.Add(cps.Key);
                ctrl_points.Add(cps.Value);
            }
            ctrl_points.Add(new Point2f(ctrl_points[0]));

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

            // index shift because first point was rotated
            int kk = k + 1;

            return Calculate(ref _knots[kk],
                             ref _controls[2 * kk - 1],
                             ref _controls[2 * kk],
                             ref _knots[kk + 1],
                             s);
        }
    }
}
