using System;
using System.Collections.Generic;

namespace TOK.Utilities
{
    class Program
    {
        static void TestClosedTriangle(float tension)
        {
            // triangle
            float[] pts = new float[] { 260.0f, 240.0f, 360.0f, 240.0f, 310.0f, 340.0f };

            ClosedBezpline spl = new ClosedBezpline(pts, tension);

            var ss = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
            for (int k = 0; k != spl.Length; ++k)
            {
                for (int i = 0; i != ss.Length; ++i)
                {
                    Point2f va = spl.Calculate(k, ss[i]);
                    Console.WriteLine("   {0}   {1}", va.X, va.Y);
                }
            }
        }

        static void TestClosedSquare(float tension)
        {
            // square
            float[] pts = new float[] { 50.0f, 200.0f, 150.0f, 200.0f, 150.0f, 300.0f, 50.0f, 300.0f };

            ClosedBezpline spl = new ClosedBezpline(pts, tension);

            var ss = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
            for (int k = 0; k != spl.Length; ++k)
            {
                for (int i = 0; i != ss.Length; ++i)
                {
                    Point2f va = spl.Calculate(k, ss[i]);
                    Console.WriteLine("   {0}   {1}", va.X, va.Y);
                }
            }
        }

        static void TestOpenCurve(float tension)
        {
            // curve
            float[] pts = new float[] { 20.0f, 50.0f, 100.0f, 100.0f,
                                       150.0f, 50.0f, 200.0f, 150.0f,
                                       250.0f, 50.0f, 300.0f, 70.0f,
                                       310.0f, 130.0f, 380.0f, 30.0f};

            var spl = new OpenBezpline(pts, tension);
            var ss = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f };
            for (int k = 0; k != spl.Length; ++k)
            {
                for (int i = 0; i != ss.Length; ++i)
                {
                    Point2f va = spl.Calculate(k, ss[i]);
                    Console.WriteLine("   {0}   {1}", va.X, va.Y);
                }
            }
        }

        static void Main(string[] args)
        {
            /*
            TestClosedTriangle(0.5f);
            Console.WriteLine("");

            TestClosedSquare(0.5f);
            Console.WriteLine("");
             * */

            TestOpenCurve(2.0f);
        }
    }
}
