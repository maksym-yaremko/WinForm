using System;
using System.Drawing;
using WindowsForms.Interfaces;
using System.Drawing.Drawing2D;

namespace WindowsForms.Models
{
    [Serializable]
    public class Rectangle : IShape
    {
        public int FillColor { get; set; }
        public Point UpperLeft { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Name { get; set; }

        public Rectangle()
        {
            Name = DateTime.Now.Ticks.ToString();
            FillColor = Color.Purple.ToArgb();
        }

        public Rectangle(Point first, Point second)
            :this()
        {
            SetUpperLeft(first, second);
            Width = Math.Abs(first.X - second.X);
            Height = Math.Abs(first.Y - second.Y);
        }

        public Rectangle(Color color, Point first, Point second)
            :this(first, second)
        {
            FillColor = color.ToArgb();
        }

        public GraphicsPath GetPath()
        {
            var path = new GraphicsPath();
            path.AddRectangle(new RectangleF(UpperLeft.X, UpperLeft.Y, Width, Height));
            return path;
        }

        public bool HitTest(Point p)
        {
            var result = false;
            using (var path = GetPath())
                result = path.IsVisible(p);
            return result;
        }

        public void Draw(Graphics g)
        {
            using (var path = GetPath())
            using (var brush = new SolidBrush(Color.FromArgb(FillColor)))
                g.FillPath(brush, path);
        }

        public void Move(Point d)
        {
            UpperLeft = new Point(UpperLeft.X + d.X, UpperLeft.Y + d.Y);
        }

        private void SetUpperLeft(Point first, Point second)
        {
            if(first.Y < second.Y)
            {
                if(first.X < second.X)
                {
                    UpperLeft = first;
                }
                else
                {
                    UpperLeft = new Point(second.X, first.Y);
                }
            }
            else
            {
                if(second.X < first.X)
                {
                    UpperLeft = second;
                }
                else
                {
                    UpperLeft = new Point(first.X, second.Y);
                }
            }
        }
    }
}
