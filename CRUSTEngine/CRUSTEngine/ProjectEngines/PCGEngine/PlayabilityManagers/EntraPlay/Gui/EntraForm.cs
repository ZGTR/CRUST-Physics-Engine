using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Gui
{
    public partial class EntraForm : Form
    {
        public Graphics Graphics;
        public String imageName = null;

        public EntraForm(string imageName)
        {
            InitializeComponent();
            this.imageName = imageName;
            Bitmap bitmap = new Bitmap(imageName);
            this.pictureBox1.Image = new Bitmap(bitmap.Width, bitmap.Height);
            //SetBackGround(this.pictureBox1.Image as Bitmap);
            Graphics = Graphics.FromImage(this.pictureBox1.Image);
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public EntraForm()
        {
            InitializeComponent();
            this.pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //SetBackGround(this.pictureBox1.Image as Bitmap);
            Graphics = Graphics.FromImage(this.pictureBox1.Image);
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        private void SetBackGround(Bitmap image)
        {
            int w = image.Width;
            int h = image.Height;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    image.SetPixel(i, j, Color.Black);
                }
            }
        }

        private static int iCounter = 0;
        public void SaveEntraOutputImageIntoHDD()
        {
            SaveEntraOutputImageIntoHDD(iCounter);
        }

        public void SaveEntraOutputImageIntoHDD(int counter)
        {
            try
            {
                Bitmap imagePolys = this.pictureBox1.Image as Bitmap;
                imagePolys.Save(StaticData.EntraImageOutputPolysOnly);

                if (!String.IsNullOrEmpty(this.imageName))
                {
                    Bitmap imageEntraInput = new Bitmap(this.imageName);
                    Bitmap imageOut = MergeImages(imageEntraInput, imagePolys);

                    imageOut.Save(@"PolysTesting\poly" + counter + ".jpg");
                    iCounter++;
                }
                else
                {
                    imagePolys.Save(@"PolysTesting\poly" + counter + ".jpg");
                    iCounter++;
                }
            }
            catch (Exception)
            {
            }
        }

        //public void SaveEntraOutputImageIntoHDD()
        //{
        //    try
        //    {
        //        Bitmap imagePolys = this.pictureBox1.Image as Bitmap;
        //        imagePolys.Save(StaticData.EntraImageOutputPolysOnly);

        //        if (!String.IsNullOrEmpty(this.imageName))
        //        {
        //            Bitmap imageEntraInput = new Bitmap(this.imageName);
        //            Bitmap imageOut = MergeImages(imageEntraInput, imagePolys);

        //            imageOut.Save(StaticData.EntraImageOutput);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}


        public Bitmap SavePolyOnlyIntoBitmap()
        {
            Bitmap imagePolys = this.pictureBox1.Image as Bitmap;
            return imagePolys;
        }

        private Bitmap FixImagePolyTransparentAreas(Bitmap imagePolys)
        {
            int minA = FindMinA(imagePolys);
            FixPixels(imagePolys, minA);
            return imagePolys;
        }

        private int FindMinA(Bitmap imagePolys)
        {
            int minA = int.MaxValue;
            int w = imagePolys.Width;
            int h = imagePolys.Height;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color cColor = imagePolys.GetPixel(i, j);
                    if (cColor.A < minA )
                    {
                        if (cColor.A != 0)
                            minA = cColor.A;
                    }
                }
            }
            return minA;
        }

        private void FixPixels(Bitmap imagePolys, int minA)
        {
            int w = imagePolys.Width;
            int h = imagePolys.Height;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color cColor = imagePolys.GetPixel(i, j);
                    if (cColor.A >= minA) // EntraDrawer.ColorSubjs.A)
                    {
                        imagePolys.SetPixel(i, j, Color.Transparent);
                    }
                    else
                    {
                        int h1;
                        h1 = 100;
                    }
                }
            }
        }

        private Bitmap MergeImages(Bitmap imageEntraInput, Bitmap imagePolyFixed)
        {
            using (Graphics grfx = Graphics.FromImage(imageEntraInput))
            {
                grfx.DrawImage(imagePolyFixed, 0, 0);
                return imageEntraInput;
            }
        }

        //private Bitmap MergeImages(Bitmap imageEntraInput, Bitmap imagePolyFixed)
        //{
        //    int w = imageEntraInput.Width;
        //    int h = imageEntraInput.Height;
        //    for (int i = 0; i < w; i++)
        //    {
        //        for (int j = 0; j < h; j++)
        //        {
        //            Color c1 = imageEntraInput.GetPixel(i, j);
        //            Color c2 = imagePolyFixed.GetPixel(i, j);
        //            Color c = Color.FromArgb(c1.A + c2.A, c1.R + c2.R, c1.G + c2.G, c1.B + c2.B);
        //            imageEntraInput.SetPixel(i, j, c);
        //        }
        //    }
        //    return imageEntraInput;
        //}

    }
}
