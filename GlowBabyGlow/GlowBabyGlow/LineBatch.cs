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
        /*
          float y = (float)Math.Pow( (((i * specialInc) - (maxLength / 2) )) * a, 2);

                    points.Add(new Vector2(x + pos.X,
                        y + pos.Y - (float)Math.Pow((specialInc * (numPoints - 1) * a) / 2, 2) ));
         */

        public static void DrawArc(SpriteBatch sb, float l, Vector2 pos, Color c)
        {
            if (l != 0)
            {
                // y = ax2 + bx + c
                int numPoints = 50;
                float maxLength = 360 * Config.screenR;
                float length = maxLength * l;
                float a = 0.31f * Config.screenR;
                List<Vector2> points = new List<Vector2>();

                float specialInc = maxLength / numPoints;

                for (int i = 0; i < numPoints; i++)
                {
                    float increment = length / numPoints;
                    float x = increment * i;
                    float y = (float)Math.Pow(((i - (numPoints / 2))), 2) * a;

                    points.Add(new Vector2(x + pos.X,
                        y + pos.Y - (float)Math.Pow(((numPoints - 1)) / 2, 2) * a));
                }

                int max = numPoints - (numPoints / 4);
                for (int i = 0; i < max - 1; i++)
                {
                    int i2 = i + 1;
                    if (i2 > max - 1)
                    { i2 = 0; }

                    LineBatch.DrawLine(sb, c, points[i], points[i2]);
                }

                float slope = (points[max - 2].Y - points[max - 3].Y) /
                     (points[max - 2].X - points[max - 3].X);
                float slope2 = (slope - (float)(Math.PI / 2)) - 0.4f;
                float slope3 = (slope - (float)(Math.PI / 2)) + 0.4f;

                Vector2 lAngle = new Vector2(((float)Math.Cos(slope2) * (40 * Config.screenR)) + points[max - 2].X,
                    ((float)Math.Sin(slope2) * (40 * Config.screenR)) + points[max - 2].Y);
                Vector2 rAngle = new Vector2(((float)Math.Cos(slope3) * (40 * Config.screenR)) + points[max - 2].X,
                    ((float)Math.Sin(slope3) * (40 * Config.screenR)) + points[max - 2].Y);

                int s = (int)(50 * Config.screenR);
                SpriteEffects se = length > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

                sb.Draw(TextureManager.throwArrow, new Rectangle((int)points[max - 1].X, (int)points[max - 1].Y, s, s),
                    new Rectangle(0, 0, TextureManager.throwArrow.Width, TextureManager.throwArrow.Height),
                    Color.Black, (float)(((Math.PI / 4) )),
                    new Vector2(TextureManager.throwArrow.Width / 2, TextureManager.throwArrow.Height / 2),
                    SpriteEffects.None, 0);
            }
            else
            {
                LineBatch.DrawLine(sb, Color.Black, new Vector2(pos.X, pos.Y), new Vector2(pos.X, pos.Y - 175 * Config.screenR));
                int s = (int)(50 * Config.screenR);
                sb.Draw(TextureManager.throwArrow, new Rectangle((int)pos.X, (int)(pos.Y - (175 * Config.screenR)), s, s),
                    new Rectangle(0, 0, TextureManager.throwArrow.Width, TextureManager.throwArrow.Height),
                    Color.Black, (float)(Math.PI / 4) * 5,
                    new Vector2(TextureManager.throwArrow.Width / 2, TextureManager.throwArrow.Height / 2),
                    SpriteEffects.None, 0);
            }
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
