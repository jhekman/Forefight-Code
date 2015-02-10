using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Forefight.Drawing {
	public static class Shapes {
		public static void DrawCircle (float x, float y, float radius, int segments, Color color){
			// http://slabode.exofire.net/circle_draw.shtml
			GL.Color3(color);

			GL.LineWidth (4);
			GL.Begin(PrimitiveType.LineLoop);

			for (int i = 0; i < segments; i++) {
				float theta = (2.0f * (float) Math.PI * (float) i) / (float) segments;
				float xx = radius * (float) Math.Cos(theta);
				float yy = radius * (float) Math.Sin(theta);
				GL.Vertex2(x + xx, y + yy);
			}

			GL.End();
		}

		public static void DrawLine (float x1, float y1, float x2, float y2, Color color){
			GL.Color3(color);

			GL.LineWidth (1);
			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(x1, y1);
			GL.Vertex2(x2, y2);
			GL.End();
		}

		public static void DrawBox (float x, float y, float size, Color color){
			GL.Color3(color);

			float half = (size / 2);
			float x1 = x - half;
			float y1 = y - half;
			float x2 = x + half;
			float y2 = y + half;

			GL.Begin(PrimitiveType.Lines);
			GL.Vertex2(x1, y1);
			GL.Vertex2(x2, y1); // bottom
			GL.Vertex2(x1, y2);
			GL.Vertex2(x2, y2); // upper
			GL.Vertex2(x1, y1);
			GL.Vertex2(x1, y2); // left
			GL.Vertex2(x2, y1);
			GL.Vertex2(x2, y2); // right
			GL.End();
		}
	}
}

