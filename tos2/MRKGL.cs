using UnityEngine;

namespace MRK
{
    public class MRKGL
    {
        public static Material? _lineMaterial = null;

        public static void CreateMaterial()
        {
            if (_lineMaterial != null)
                return;

            _lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
            _lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            _lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }

        public static void SetLineMaterial(Material material)
        {
            _lineMaterial = material;
        }

        public static void DrawLine(
            Vector2 start,
            Vector2 end,
            Color scolor,
            Color ecolor,
            float width
        )
        {
            if (Event.current == null || Event.current.type != EventType.Repaint)
                return;

            CreateMaterial();

            _lineMaterial?.SetPass(0);

            Vector3 startPt;
            Vector3 endPt;

            if (width == 1)
            {
                GL.Begin(GL.LINES);
                GL.Color(scolor);
                startPt = new Vector3(start.x, start.y, 0);
                endPt = new Vector3(end.x, end.y, 0);
                GL.Vertex(startPt);
                GL.Color(ecolor);
                GL.Vertex(endPt);
            }
            else
            {
                GL.Begin(GL.QUADS);
                GL.Color(scolor);
                startPt = new Vector3(end.y, start.x, 0);
                endPt = new Vector3(start.y, end.x, 0);
                Vector3 perpendicular = (startPt - endPt).normalized * width;
                Vector3 v1 = new Vector3(start.x, start.y, 0);
                Vector3 v2 = new Vector3(end.x, end.y, 0);
                GL.Vertex(v1 - perpendicular);
                GL.Vertex(v1 + perpendicular);
                GL.Color(ecolor);
                GL.Vertex(v2 + perpendicular);
                GL.Vertex(v2 - perpendicular);
            }
            GL.End();
            //Utils.NativeLMSLog(string.Format("Drawn line from {0} to {1}", start, end));
        }

        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            DrawLine(start, end, color, color, width);
        }

        public static void DrawBox(Rect box, Color color, float width)
        {
            Vector2 p1 = new Vector2(box.xMin, box.yMin);
            Vector2 p2 = new Vector2(box.xMax, box.yMin);
            Vector2 p3 = new Vector2(box.xMax, box.yMax);
            Vector2 p4 = new Vector2(box.xMin, box.yMax);
            DrawLine(p1, p2, color, width);
            DrawLine(p2, p3, color, width);
            DrawLine(p3, p4, color, width);
            DrawLine(p4, p1, color, width);
        }

        public static void DrawBoxGUI(Rect rect, Color color, float thickness = 1f)
        {
            var prevColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(
                new Rect(rect.x, rect.y, rect.width, thickness),
                Texture2D.whiteTexture
            );
            GUI.DrawTexture(
                new Rect(rect.x, rect.yMax - thickness, rect.width, thickness),
                Texture2D.whiteTexture
            );
            GUI.DrawTexture(
                new Rect(rect.x, rect.y, thickness, rect.height),
                Texture2D.whiteTexture
            );
            GUI.DrawTexture(
                new Rect(rect.xMax - thickness, rect.y, thickness, rect.height),
                Texture2D.whiteTexture
            );
            GUI.color = prevColor;
        }

        public static void DrawBox(
            Vector2 topLeftCorner,
            Vector2 bottomRightCorner,
            Color color,
            float width
        )
        {
            Rect box = new Rect(
                topLeftCorner.x,
                topLeftCorner.y,
                bottomRightCorner.x - topLeftCorner.x,
                bottomRightCorner.y - topLeftCorner.y
            );
            DrawBox(box, color, width);
        }

        public static void DrawRoundedBox(Rect box, float radius, Color color, float width)
        {
            Vector2 p1,
                p2,
                p3,
                p4,
                p5,
                p6,
                p7,
                p8;
            p1 = new Vector2(box.xMin + radius, box.yMin);
            p2 = new Vector2(box.xMax - radius, box.yMin);
            p3 = new Vector2(box.xMax, box.yMin + radius);
            p4 = new Vector2(box.xMax, box.yMax - radius);
            p5 = new Vector2(box.xMax - radius, box.yMax);
            p6 = new Vector2(box.xMin + radius, box.yMax);
            p7 = new Vector2(box.xMin, box.yMax - radius);
            p8 = new Vector2(box.xMin, box.yMin + radius);

            DrawLine(p1, p2, color, width);
            DrawLine(p3, p4, color, width);
            DrawLine(p5, p6, color, width);
            DrawLine(p7, p8, color, width);

            Vector2 t1,
                t2;
            float halfRadius = radius / 2;

            t1 = new Vector2(p8.x, p8.y + halfRadius);
            t2 = new Vector2(p1.x - halfRadius, p1.y);
            DrawBezier(p8, t1, p1, t2, color, width);

            t1 = new Vector2(p2.x + halfRadius, p2.y);
            t2 = new Vector2(p3.x, p3.y - halfRadius);
            DrawBezier(p2, t1, p3, t2, color, width);

            t1 = new Vector2(p4.x, p4.y + halfRadius);
            t2 = new Vector2(p5.x + halfRadius, p5.y);
            DrawBezier(p4, t1, p5, t2, color, width);

            t1 = new Vector2(p6.x - halfRadius, p6.y);
            t2 = new Vector2(p7.x, p7.y + halfRadius);
            DrawBezier(p6, t1, p7, t2, color, width);
        }

        public static void DrawConnectingCurve(Vector2 start, Vector2 end, Color color, float width)
        {
            Vector2 distance = start - end;

            Vector2 tangentA = start;
            tangentA.x -= (distance / 2).x;
            Vector2 tangentB = end;
            tangentB.x += (distance / 2).x;

            int segments = Mathf.FloorToInt(distance.magnitude / 20 * 3);

            DrawBezier(start, tangentA, end, tangentB, color, width, segments);
        }

        public static void DrawBezier(
            Vector2 start,
            Vector2 startTangent,
            Vector2 end,
            Vector2 endTangent,
            Color color,
            float width
        )
        {
            int segments = Mathf.FloorToInt((start - end).magnitude / 20) * 3; // three segments per distance of 20
            DrawBezier(start, startTangent, end, endTangent, color, width, segments);
        }

        public static void DrawBezier(
            Vector2 start,
            Vector2 startTangent,
            Vector2 end,
            Vector2 endTangent,
            Color color,
            float width,
            int segments
        )
        {
            Vector2 startVector = CubeBezier(start, startTangent, end, endTangent, 0);
            for (int i = 1; i <= segments; i++)
            {
                Vector2 endVector = CubeBezier(
                    start,
                    startTangent,
                    end,
                    endTangent,
                    i / (float)segments
                );
                DrawLine(startVector, endVector, color, width);
                startVector = endVector;
            }
        }

        static Vector2 CubeBezier(Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t)
        {
            float rt = 1 - t;
            float rtt = rt * t;
            return rt * rt * rt * s + 3 * rt * rtt * st + 3 * rtt * t * et + t * t * t * e;
        }

        public static void DrawCircle(Vector2 pos, float radius, Color c, int segments = 20)
        {
            float twoPI = Mathf.PI * 2f;
            float advance = twoPI / segments;
            Vector2 lastSegment = Vector2.zero,
                intl = Vector2.zero;

            for (float theta = 0f; theta <= twoPI; theta += advance)
            {
                Vector2 xy = new Vector2
                {
                    x = pos.x + Mathf.Cos(theta) * radius,
                    y = pos.y - Mathf.Sin(theta) * radius,
                };

                if (lastSegment != Vector2.zero)
                {
                    DrawLine(lastSegment, xy, c, 1.4f);
                }
                else
                    intl = xy;

                lastSegment = xy;
            }

            DrawLine(lastSegment, intl, c, 1.4f);
        }
    }
}
