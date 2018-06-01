using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsForms.Interfaces
{
    public interface IShape
    {
        GraphicsPath GetPath();
        bool HitTest(Point p);
        void Draw(Graphics g);
        void Move(Point d);
    }
}
