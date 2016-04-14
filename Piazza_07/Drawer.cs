using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Piazza_07
{
    public enum Tool { Pencil, Line, Rectangle, Circle, Eraser, Triangle, Fill };

    class Drawer
    {
        public Pen pen;
        public Tool tool;
        public bool paintStarted = false;
        public Point prev;
        public PictureBox picture;
        public Rectangle rect;
        public Graphics g;
        public Bitmap bmp;
        public GraphicsPath path;

        public Pen eraserPen = new Pen(Color.Black);
        public Size eraserSize = new Size(100, 100);
        Queue<Point> q = new Queue<Point>();
        bool[,] used = new bool[760, 476];
        public Color color = Color.Black;

        private int width, height;

        public Drawer(PictureBox p)
        {
            this.picture = p;
            pen = new Pen(Color.Black);
            loadImage("");
            picture.Paint += picturePaint;
        }

        public void picturePaint(object sender, PaintEventArgs e)
        {
            if (path != null)
                e.Graphics.DrawPath(pen, path);
        }

        public void saveLastPath()
        {
            if (path != null && tool != Tool.Eraser)
                g.DrawPath(pen, path);
        }

        public void Draw(Point cur)
        {
            switch (tool)
            {
                case Tool.Pencil:
                    g.DrawLine(pen, prev, cur);
                    prev = cur;
                    break;
                case Tool.Line:
                    path = new GraphicsPath();
                    path.AddLine(prev, cur);
                    break;
                case Tool.Rectangle:
                case Tool.Circle:
                    path = new GraphicsPath();

                    if (cur.X - prev.X > 0)
                        width = 0;
                    else
                        width = cur.X - prev.X;

                    if (cur.Y - prev.Y > 0)
                        height = 0;
                    else
                        height = cur.Y - prev.Y;

                    rect = new Rectangle(prev.X + width, prev.Y + height, Math.Abs(cur.X - prev.X), Math.Abs(cur.Y - prev.Y));

                    if (tool == Tool.Rectangle)
                        path.AddRectangle(rect);
                    else if (tool == Tool.Circle)
                        path.AddEllipse(rect);
                    
                    break;
                case Tool.Eraser:
                    path = new GraphicsPath();
                    path.AddRectangle(new Rectangle(cur.X - eraserSize.Width / 2, cur.Y - eraserSize.Width / 2, eraserSize.Width, eraserSize.Height));
                    if(paintStarted)
                    {
                        SolidBrush erase = new SolidBrush(picture.BackColor);
                        g.FillRectangle(erase, new Rectangle(cur.X - eraserSize.Width / 2, cur.Y - eraserSize.Width / 2, eraserSize.Width, eraserSize.Height));
                    }
                    break;
                case Tool.Triangle:
                    path = new GraphicsPath();
                    path.AddPolygon(new Point[] { new Point(prev.X, prev.Y), new Point(cur.X, cur.Y),
                        new Point(cur.X - 2 * (cur.X - prev.X), cur.Y) });
                    //path.AddPolygon(new Point[] { new Point(prev.X, prev.Y), new Point(cur.X, cur.Y), new Point(prev.X, cur.Y) });
                    break;
                default:
                    break;
            }
            picture.Refresh();
        }

        public void loadImage(string fileName)
        {
            bmp = fileName == "" ? new Bitmap(picture.Width, picture.Height) : new Bitmap(fileName);
            g = Graphics.FromImage(bmp);
            picture.Image = bmp;
        }

        public void saveImage(string fileName)
        {
            bmp.Save(fileName);
        }

        public void fill(Point cur)
        {
            Color clicked_color = bmp.GetPixel(cur.X, cur.Y);
            checkNeighbors(cur.X, cur.Y, clicked_color);
            while (q.Count > 0)
            {
                Point p = q.Dequeue();
                checkNeighbors(p.X - 1, p.Y, clicked_color);
                checkNeighbors(p.X, p.Y - 1, clicked_color);
                checkNeighbors(p.X, p.Y + 1, clicked_color);
                checkNeighbors(p.X + 1, p.Y, clicked_color);
            }
            picture.Refresh();
        }

        public void checkNeighbors(int x, int y, Color clicked_color)
        {
            if (x > 0 && y > 0 && x < picture.Width && y < picture.Height)
            {
                if (used[x, y] == false && bmp.GetPixel(x, y) == clicked_color)
                {
                    used[x, y] = true;
                    q.Enqueue(new Point(x, y));
                    bmp.SetPixel(x, y, color);
                }
            }
        }

    }
}
