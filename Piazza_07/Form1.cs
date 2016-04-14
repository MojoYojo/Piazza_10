using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Piazza_07
{
    public partial class Form1 : Form
    {
        Drawer drawer;
        string filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";

        public Form1()
        {
            InitializeComponent();
            drawer = new Drawer(pictureBox1);
        }

        List<String> elementValues = new List<String> { "3", "6", "9", "12" };
        List<String> eraserValues = new List<String> { "20", "40", "60", "80" };

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawer.prev = e.Location;
                drawer.paintStarted = true;

                if (drawer.tool == Tool.Eraser)
                {
                    comboBox1.Items.Clear();
                    foreach (String value in eraserValues)
                        comboBox1.Items.Add(value);
                }
                else if (drawer.tool != Tool.Eraser && drawer.tool != Tool.Fill)
                {
                    comboBox1.Items.Clear();
                    foreach (String value in elementValues)
                        comboBox1.Items.Add(value);
                }
                else
                {
                    drawer.fill(e.Location);
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawer.paintStarted)
            {
                drawer.Draw(e.Location);
            }
        }



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drawer.paintStarted = false;
            drawer.saveLastPath();
        }

        private void utilize_Pencil(object sender, EventArgs e)
        {
            drawer.tool = Tool.Pencil;
        }

        private void utilize_Line(object sender, EventArgs e)
        {
            drawer.tool = Tool.Line;
        }

        private void utilize_Rectangle(object sender, EventArgs e)
        {
            drawer.tool = Tool.Rectangle;
        }

        private void utilize_Circle(object sender, EventArgs e)
        {
            drawer.tool = Tool.Circle;
        }

        private void utilize_Eraser(object sender, EventArgs e)
        {
            drawer.tool = Tool.Eraser;
            drawer.Draw(new Point(0, 0));
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawer.g.Clear(pictureBox1.BackColor);
            pictureBox1.Refresh();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = filter;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                drawer.loadImage(openFileDialog1.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = filter;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                drawer.saveImage(saveFileDialog1.FileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawer.pen.Width = int.Parse(comboBox1.Items[comboBox1.SelectedIndex] as string);
        }

        private void colorChange(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                drawer.pen.Color = colorDialog1.Color;
                drawer.color = colorDialog1.Color;
            }
        }

        private void utilize_triangle(object sender, EventArgs e)
        {
            drawer.tool = Tool.Triangle;
        }

        private void utilize_Fill(object sender, EventArgs e)
        {
            drawer.tool = Tool.Fill;
        }
    }
}
