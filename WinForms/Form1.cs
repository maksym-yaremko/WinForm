using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsForms.BL;

namespace WindowsForms
{
    public partial class Form1 : Form
    {
        public List<Models.Rectangle> Rectangles { get; private set; }
        Models.Rectangle selectedShape;
        bool moving;
        Point previousPoint = Point.Empty;
        Point firstPoint = Point.Empty;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Rectangles = new List<Models.Rectangle>();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            for (var i = Rectangles.Count - 1; i >= 0; i--)
            {
                if (Rectangles[i].HitTest(e.Location))
                {
                    selectedShape = Rectangles[i];
                    
                    break;
                }
            }
            if (selectedShape != null && e.Button == MouseButtons.Left)
            {
                moving = true;
                previousPoint = e.Location;
            }
            if (selectedShape != null && e.Button == MouseButtons.Right)
            {
                colorDialog1.ShowDialog();
                selectedShape.FillColor = colorDialog1.Color.ToArgb();
                this.Invalidate();
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (moving)
            {
                var d = new Point(e.X - previousPoint.X, e.Y - previousPoint.Y);
                selectedShape.Move(d);
                previousPoint = e.Location;
                this.Invalidate();
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (moving)
            {
                selectedShape = null;
                moving = false;
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (var shape in Rectangles)
            {
                shape.Draw(e.Graphics);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (firstPoint == Point.Empty)
            {
                firstPoint = new Point(e.X, e.Y);
            }
            else
            {
                Rectangles.Add(new Models.Rectangle(firstPoint, new Point(e.X, e.Y)));
                firstPoint = Point.Empty;
                this.Refresh();
            }       
        }

        private void New_Click(object sender, EventArgs e)
        {
            Rectangles.Clear();
            this.Refresh();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog
            {
                Filter = "(*.xml)|*.xml",
                RestoreDirectory = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Title = "Choose file"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Rectangles.Clear();
                Rectangles = BL.BL.DeserializeList(openFileDialog1.FileName);
                this.Invalidate();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            saveFileDialog1 =  new SaveFileDialog
            {
                RestoreDirectory = true,
                DefaultExt = "xml",
                CheckPathExists = true,
                Title = "Save your work",
                ValidateNames = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                BL.BL.SerializeList(Rectangles, saveFileDialog1.FileName);
            }
        }

        private void ShapesMenu_Click(object sender, EventArgs e)
        {
            ShapesMenuItem.DropDownItems.Clear();
            List<ToolStripMenuItem> ul = new List<ToolStripMenuItem>();
            foreach (var rect in Rectangles)
            {
                ToolStripMenuItem li = new ToolStripMenuItem(rect.Name);
                li.Click += new EventHandler(ShapesMenuDropDown_Click);
                ul.Add(li);
            }
            ShapesMenuItem.DropDownItems.AddRange(ul.ToArray());
        }

        private void ShapesMenuDropDown_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsim = sender as ToolStripMenuItem;
            var rect = Rectangles.Find(p => p.Name.ToString() == tsim.Text);
            Rectangles.Remove(rect);
            Rectangles.Add(rect);
            this.Invalidate();
        }

        private void Info_Click(object sender, EventArgs e)
        {
            string caption = "Information box";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            if (MessageBox.Show(INFO_TEXT, caption, buttons) == DialogResult.Yes)
            {
                Close();
            }
        }

        private const string INFO_TEXT = "Для того щоб намалювати прямокутник, клацніть два рази лівою кнопкою миші на полотні.Додайте так само протилежну вершину, на осові цих двох точок, програма намалює чорний прямокутник.Щоб змінити колір натисніть на прямокутник правою правою кнопкою миші, та оберіть колір.Щоб перемістити прямокутник затисніть його лівою кнопокою миші і пересуньте його у зручне вам місце.Щоб створити полотно натисніть кнопку 'New'.Щоб зберегти полотно натисніть кнопку 'Save'.Щоб відкрити існуюче полотно, натисніть кнопку 'Open'";
    }
}
