using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    /// <summary>
    /// Line Batch
    /// For drawing lines in a spritebatch
    /// </summary>
    static public class LineBatch
    {
        static private Texture2D _empty_texture;
        static private bool _set_data = false;

        static public void Init(GraphicsDevice device)
        {
            _empty_texture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
        }

        static public void DrawLine(SpriteBatch batch, Color color,
                                    Vector2 point1, Vector2 point2)
        {
            DrawLine(batch, color, point1, point2, 0);
        }

        /// <summary>
        /// Draw a line into a SpriteBatch
        /// </summary>
        /// <param name="batch">SpriteBatch to draw line</param>
        /// <param name="color">The line color</param>
        /// <param name="point1">Start Point</param>
        /// <param name="point2">End Point</param>
        /// <param name="Layer">Layer or Z position</param>
        static public void DrawLine(SpriteBatch batch, Color color, Vector2 point1,
                                    Vector2 point2, float Layer)
        {
            //Check if data has been set for texture
            //Do this only once otherwise
            if (!_set_data)
            {
                _empty_texture.SetData(new[] { Color.White });
                _set_data = true;
            }


            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = (point2 - point1).Length();
            
            batch.Draw(_empty_texture, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, 3),
                       SpriteEffects.None, Layer);
        }

        public static void DrawCircle(SpriteBatch sb, Vector2 pos, int radius, Color c)
        {
            List<Vector2> points = new List<Vector2>();
            int cRad = radius - 1;
            Rectangle rect = new Rectangle((int)(pos.X - cRad), (int)(pos.Y - cRad), cRad * 2, cRad * 2);

            int numPoints = 10 + (radius / 3);

            for (int i = 0; i < numPoints; i++)
            {
                double angle = ((Math.PI * 2) / numPoints) * i;

                points.Add(new Vector2((int)(pos.X + Math.Cos(angle) * radius),
                    (int)(pos.Y + Math.Sin(angle) * radius)));
            }

            sb.Draw(TextureManager.babyGlow, rect, c);
            sb.Draw(TextureManager.babyGlow, new Rectangle(rect.X + Config.screenW, 
                rect.Y, rect.Width, rect.Height), c);
            sb.Draw(TextureManager.babyGlow, new Rectangle(rect.X - Config.screenW, 
                rect.Y, rect.Width, rect.Height), c);

            for (int i = 0; i < points.Count; i++)
            {
                int i2 = i + 1;
                if (i2 > points.Count - 1)
                { i2 = 0; }

                Vector2 leftPoint = new Vector2(points[i].X - Config.screenW, points[i].Y);
                Vector2 leftPoint2 = new Vector2(points[i2].X - Config.screenW, points[i2].Y);
                Vector2 rightPoint = new Vector2(points[i].X + Config.screenW, points[i].Y);
                Vector2 rightPoint2 = new Vector2(points[i2].X + Config.screenW, points[i2].Y);

                LineBatch.DrawLine(sb, new Color(0, 0, 0, 125), points[i], points[i2]);
                LineBatch.DrawLine(sb, new Color(0, 0, 0, 125), leftPoint, leftPoint2);
                LineBatch.DrawLine(sb, new Color(0, 0, 0, 125), rightPoint, rightPoint2);
            }
        }
    }
}
