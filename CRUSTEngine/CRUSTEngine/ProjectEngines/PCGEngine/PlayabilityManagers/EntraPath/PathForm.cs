using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Point = Microsoft.Xna.Framework.Point;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPath
{
    public partial class PathForm : Form
    {
        private readonly List<Point> _path;
        public Graphics Graphics;

        public PathForm(List<Point> path, bool withInputImage)
        {
            _path = path;
            InitializeComponent();
            if (withInputImage)
            {
                this.pictureBox1.Image = new Bitmap(StaticData.EntraImageInput); //new Bitmap(pictureBox1.Width, pictureBox1.Height);    
            }
            else
            {
                this.pictureBox1.Image = new Bitmap(StaticData.LevelFarWidth, StaticData.LevelFarHeight);    
            }
            
            Graphics = Graphics.FromImage(this.pictureBox1.Image);
            DrawPath();
        }

        private void DrawPath()
        {
            try
            {
                Pen pen = new Pen(Color.Yellow, 2);
                Point p1 = _path[0];
                for (int i = 1; i < _path.Count; i++)
                {
                    Point p2 = _path[i];
                    this.Graphics.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
                    p1 = p2;
                }

            }
            catch (Exception)
            {
            }
            
        }

        public void DrawPathIntoOutput(String imageOutput)
        {
            try
            {
                Bitmap imageOut = GetPathBitmap();
                imageOut.Save(imageOutput);
            }
            catch (Exception)
            {
            }
        }

        public Bitmap GetPathBitmap()
        {
            Bitmap imageOut = this.pictureBox1.Image as Bitmap;
            return imageOut;
        }
    }
}