using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GlowBabyGlow
{
    class GFont
    {
        Texture2D fontSheet;
        int rows;
        int columns;
        public static int width = (int)(90 * Config.fontRatio);
        public static int height = (int)(106 * Config.fontRatio);

        Color[] colorBank = new Color[255];
        Dictionary<char, Point> fontMap = new Dictionary<char, Point>();

        public Point Size
        {
            get { return new Point(width, height); }
        }

        public GFont(Texture2D fs, int rows, int columns)
        {
            Init();
            fontSheet = fs;
            this.rows = rows;
            this.columns = columns;

            for (int i = 0; i < 255; i++)
            {
                int r = Config.rand.Next(255);
                int g = Config.rand.Next(255);
                int b = Config.rand.Next(255);
                colorBank[i] = new Color(r, g, b);
            }
        }

        #region Initialization

        public void Init()
        {
            fontMap.Add(' ', new Point(-1,-1));
            
            fontMap.Add('a', new Point(0, 0));
            fontMap.Add('b', new Point(1, 0));
            fontMap.Add('c', new Point(2, 0));
            fontMap.Add('d', new Point(3, 0));
            fontMap.Add('e', new Point(4, 0));
            fontMap.Add('f', new Point(5, 0));
            fontMap.Add('g', new Point(6, 0));
            fontMap.Add('h', new Point(7, 0));
            fontMap.Add('i', new Point(8, 0));
            fontMap.Add('j', new Point(9, 0));

            fontMap.Add('k', new Point(0, 1));
            fontMap.Add('l', new Point(1, 1));
            fontMap.Add('m', new Point(2, 1));
            fontMap.Add('n', new Point(3, 1));
            fontMap.Add('o', new Point(4, 1));
            fontMap.Add('p', new Point(5, 1));
            fontMap.Add('q', new Point(6, 1));
            fontMap.Add('r', new Point(7, 1));
            fontMap.Add('s', new Point(8, 1));
            fontMap.Add('t', new Point(9, 1));

            fontMap.Add('u', new Point(0, 2));
            fontMap.Add('v', new Point(1, 2));
            fontMap.Add('w', new Point(2, 2));
            fontMap.Add('x', new Point(3, 2));
            fontMap.Add('y', new Point(4, 2));
            fontMap.Add('z', new Point(5, 2));
            fontMap.Add('0', new Point(6, 2));
            fontMap.Add('1', new Point(7, 2));
            fontMap.Add('2', new Point(8, 2));
            fontMap.Add('3', new Point(9, 2));

            fontMap.Add('4', new Point(0, 3));
            fontMap.Add('5', new Point(1, 3));
            fontMap.Add('6', new Point(2, 3));
            fontMap.Add('7', new Point(3, 3));
            fontMap.Add('8', new Point(4, 3));
            fontMap.Add('9', new Point(5, 3));
            fontMap.Add(':', new Point(6, 3));
            fontMap.Add(',', new Point(7, 3));
            fontMap.Add('!', new Point(8, 3));
            fontMap.Add('-', new Point(9, 3));

            fontMap.Add('@', new Point(0, 4)); // a button
            fontMap.Add('#', new Point(1, 4)); // x button
            fontMap.Add('$', new Point(2, 4)); // control stick
            fontMap.Add('%', new Point(3, 4)); // arrow keys
            fontMap.Add('^', new Point(4, 4)); // z key
            fontMap.Add('&', new Point(5, 4)); // x key
        }

        #endregion

        public void SetTexture(Texture2D t)
        {
            fontSheet = t;
        }

        public void Draw(SpriteBatch sb, Vector2 pos, string word, Color c, bool small = false)
        {
            int offset = 5;
            int spacing = 1;

            int w = width;
            int h = height;

            if (small)
            {
                w /= 2;
                h /= 2;
            }

            word = word.ToLower();

            for (int i = 0; i < word.Length; i++)
            {
                Rectangle rect = new Rectangle(
                    (int)(fontMap[word[i]].X * (fontSheet.Width / columns)),
                    (int)(fontMap[word[i]].Y * (fontSheet.Height / rows)),
                    fontSheet.Width / columns, fontSheet.Height / rows);
                sb.Draw(fontSheet, new Rectangle((int)pos.X + (spacing * i) + (w * i) - offset, (int)pos.Y + offset, w, h), 
                    rect, new Color(0,0,0,50));
            }
           
            for (int i = 0; i < word.Length; i++)
            {
                Color color = c == Color.White ? colorBank[i] : c;
                Rectangle rect = new Rectangle(
                    (int)(fontMap[word[i]].X * (fontSheet.Width / columns)),
                    (int)(fontMap[word[i]].Y * (fontSheet.Height / rows)),
                    fontSheet.Width / columns, fontSheet.Height / rows);
                sb.Draw(fontSheet, new Rectangle((int)pos.X + (spacing * i) + (w * i), (int)pos.Y, w, h), rect, color);
            }
        }

        public void DrawDescrition(SpriteBatch sb, Vector2 pos, string word, Color c, int maxWidth)
        {
            int offset = 5;
            int spacing = 1;
            int yspacing = 6;

            int w = width / 3;
            int h = height / 3;
            int yoff = 0;

            word = word.ToLower();

            int index = 0;

            for (int i = 0; i < word.Length; i++)
            {
                Rectangle rect = new Rectangle(
                    (int)(fontMap[word[i]].X * (fontSheet.Width / columns)),
                    (int)(fontMap[word[i]].Y * (fontSheet.Height / rows)),
                    fontSheet.Width / columns, fontSheet.Height / rows);
                sb.Draw(fontSheet, new Rectangle((int)pos.X + (spacing * index) + (w * index) - offset, (int)pos.Y + offset + yoff, w, h),
                    rect, new Color(0, 0, 0, 50));

                index++;

                int checkspace = i;
                while (checkspace < word.Length)
                {
                    if (word[checkspace] == ' ' && word[i] == ' ')
                    {
                        if ((spacing * (index + (checkspace - i))) + (w * (index + (checkspace - i))) > maxWidth)
                        {
                            index = 0;
                            yoff += h + yspacing;
                        }

                        break;
                    }

                    checkspace++;
                }

            }

            index = 0;
            yoff = 0;

            for (int i = 0; i < word.Length; i++)
            {
                Color color = c == Color.White ? colorBank[i] : c;
                Rectangle rect = new Rectangle(
                    (int)(fontMap[word[i]].X * (fontSheet.Width / columns)),
                    (int)(fontMap[word[i]].Y * (fontSheet.Height / rows)),
                    fontSheet.Width / columns, fontSheet.Height / rows);
                sb.Draw(fontSheet, new Rectangle((int)pos.X + (spacing * index) + (w * index), (int)pos.Y + yoff, w, h), rect, color);

                index++;

                int checkspace = i;
                while (checkspace < word.Length)
                {
                    if (word[checkspace] == ' ' && word[i] == ' ')
                    {
                        if ((spacing * (index + (checkspace - i))) + (w * (index + (checkspace - i))) > maxWidth)
                        {
                            index = 0;
                            yoff += h + yspacing;
                        }

                        break;
                    }

                    checkspace++;
                }
            }
        }
    }
}
