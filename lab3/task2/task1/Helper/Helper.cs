using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace task2
{
    public static class Helper
    {
        public static void DrawCircle(float x, float y, float radius)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(x, y);
            for (float i = 0.0f; i <= 360; i++)
            {
                GL.Vertex2(radius * (Math.Cos(Math.PI * i / 180.0) + x),
                    radius * (Math.Sin(Math.PI * i / 180.0) + y));
            }

            GL.End();
        }

        public static void DrawEllipse(float x, float y, float width, float height)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(x, y);

            for (float i = 0.0f; i <= 360; i++)
            {
                GL.Vertex2(width * Math.Cos(Math.PI * i / 180.0) + x,
                    height * Math.Sin(Math.PI * i / 180.0) + y);
            }

            GL.End();
        }

        public static void DrawCasteljauBezier(List<Vector2> points, bool fill)
        {
            int segments = 1;

            if (points.Count < 2) return;

            if (fill)
            {
                GL.Begin(PrimitiveType.TriangleFan);
            }
            else
            {
                GL.Begin(PrimitiveType.LineStrip);
            }

            List<Vector2> bezierPoints = new List<Vector2>();

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector2 bezierPoint = GetCasteljauPoint(points, points.Count - 1, 0, t);
                bezierPoints.Add(bezierPoint);
            }

            foreach (var point in bezierPoints)
            {
                GL.Vertex2(point.X, point.Y);
            }

            GL.End();
        }

        private static Vector2 GetCasteljauPoint(List<Vector2> points, int r, int i, float t)
        {
            if (r == 0) return points[i];

            Vector2 p1 = GetCasteljauPoint(points, r - 1, i, t);
            Vector2 p2 = GetCasteljauPoint(points, r - 1, i + 1, t);

            return new Vector2((1 - t) * p1.X + t * p2.X, (1 - t) * p1.Y + t * p2.Y);
        }
    }
}
