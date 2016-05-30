using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers;
using Color = System.Drawing.Color;
using Rectangle = System.Drawing.Rectangle;

namespace CRUSTEngine.ProjectEngines.PCGEngine.TestModule
{
    class DiversityManager
    {
        public static void GetDiversityImage(List<String> phenos)
        {
            var allComps = BuildCompsList(phenos);
            List<Color[,]> colors;
            List<int[,]> counts;
            List<float[,]> perc;
            BuildDiversityMap(allComps, out colors, out counts, out perc);
            PrintDiversityMap(colors);
            PrintCompCount(counts);
            //PrintCompCount(counts[0], );
            //PrintCompPos(allComps[0], "RopesPos.txt");
        }

        //private static void PrintCompCount(int[,] ints, string strOut)
        //{
        //    StreamWriter sw = new StreamWriter(strOut);
        //    foreach (int count in ints)
        //    {
        //        sw.WriteLine(count + "\t");
        //    }
        //    sw.Close();
        //}

        private static void PrintCompCount(List<int[,]> allComps)
        {
            StreamWriter sw = new StreamWriter("Diversity.txt");
            foreach (int[,] compCount in allComps)
            {
                for (int i = 0; i < compCount.GetLength(0); i++)
                {
                    for (int j = 0; j < compCount.GetLength(1); j++)
                    {
                        sw.Write(compCount[i, j] + "\t");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("_________________");
            }
            sw.Close();
        }

        private static void PrintCompPos(List<Vector2> vecs, String strOut)
        {
            StreamWriter sw = new StreamWriter(strOut);
            foreach (Vector2 vec in vecs)
            {
                sw.WriteLine(vec.X  + "\t" + vec.Y);
            }
            sw.Close();
        }

        private static void PrintDiversityMap(List<Color[,]> compsColorMap)
        {

            Bitmap image = new Bitmap(302, 299);
            int compCounter = 0;
            foreach (Color[,] compMap in compsColorMap)
            {
                PrintColorMap(image, compMap, compCounter);
                compCounter++;
            }
            image.Save("Diversity.jpg");
        }

        //private static int minY = 0;
        //private static int maxY = 460;

        //private static int minX = 200;
        //private static int maxX = 540;

        //static int xdist = (maxX - minX) / 3;
        //static int ydist = (maxY - minY) / 3;

        private static void PrintColorMap(Bitmap image, Color[,] compMap, int compCounter)
        {
            int dist = 100;
            int distX = dist/5;
            int diffBetweenBoxes = 1;
            for (int i = 0; i < compMap.GetLength(0); i++)
            {
                for (int j = 0; j < compMap.GetLength(1); j++)
                {

                    int xStart = j*(dist + diffBetweenBoxes) + compCounter*(distX);
                    int yStart = i*dist;
                    image.SetPixel(xStart, yStart, compMap[i, j]);

                    for (int k2 = xStart; k2 < xStart + distX; k2++)
                    {
                        for (int k1 = yStart; k1 < yStart + dist - 1; k1++)
                        {
                            image.SetPixel(k2, k1, compMap[i, j]);
                        }
                    }
                }
            }

            Color bColor = Color.DarkRed;
            for (int i = 0; i < image.Height; i++)
            {
                image.SetPixel(distX*5, i, bColor);
                image.SetPixel(2*(distX*5) + diffBetweenBoxes, i, bColor);
            }

            for (int i = 0; i < image.Width; i++)
            {
                image.SetPixel(i, distX*5 - diffBetweenBoxes, bColor);
                image.SetPixel(i, 2*(distX*5) - diffBetweenBoxes, bColor);
            }
        }

        private static List<List<Vector2>> BuildCompsList(List<string> phenos)
        {
            List<Vector2> ropes = new List<Vector2>(),
                          bumpers = new List<Vector2>(),
                          blowers = new List<Vector2>(),
                          bubbles = new List<Vector2>(),
                          rockets = new List<Vector2>();

            foreach (string pheno in phenos)
            {
                String[] args = new string[2];
                args[1] = pheno;
                var levelGeneratorEngine = new LevelGenerator(pheno);
                //levelGeneratorEngine.GenerateLevel();
                var comps = levelGeneratorEngine.Items;

                ropes.AddRange(comps.Where(c => c is Rope).Select(r => new Vector2(r.X, r.Y)).ToList());
                bumpers.AddRange(comps.Where(c => c is Bump).Select(r => new Vector2(r.X, r.Y)).ToList());
                blowers.AddRange(comps.Where(c => c is Blower).Select(r => new Vector2(r.X, r.Y)).ToList());
                bubbles.AddRange(comps.Where(c => c is Bubble).Select(r => new Vector2(r.X, r.Y)).ToList());
                rockets.AddRange(comps.Where(c => c is Rocket).Select(r => new Vector2(r.X, r.Y)).ToList());
            }
            List<List<Vector2>> allComps = new List<List<Vector2>>() { ropes, bumpers, blowers, bubbles, rockets };
            return allComps;
        }

        private static void BuildDiversityMap(List<List<Vector2>> allComps, out List<Color[,]> resColor,
             out List<int[,]> resCountInBox, out List<float[,]> resPerc)
        {
            resColor = new List<Color[,]>();
            resCountInBox = new List<int[,]>();
            resPerc = new List<float[,]>();
            int counterComp = 0;
            foreach (List<Vector2> compPositions in allComps)
            {
                int compCount = compPositions.Count;
                Color[,] colorArr = new Color[3, 3];
                int[,] intArr = new int[3, 3];
                float[,] percArr = new float[3, 3];

                for (int i = 0; i < colorArr.GetLength(0); i++)
                {
                    for (int j = 0; j < colorArr.GetLength(1); j++)
                    {
                        Rectangle cellBox = GetCellBox(i, j);
                        int countInBox = compPositions.Count(c => InsideBox(c, cellBox));
                        float perc = countInBox / (compCount / (float)5);
                        colorArr[i, j] = ControlPaint.Light(GetColor(counterComp), 1 - perc);
                        intArr[i, j] = countInBox;
                        percArr[i, j] = perc;
                    }
                }
                resColor.Add(colorArr);
                resCountInBox.Add(intArr);
                resPerc.Add(percArr);
                counterComp++;
            }
        }

        private static Color GetColor(int counterComp)
        {
            Color color;
            switch (counterComp)
            {
                case 0:
                    color = Color.Brown;
                    break;
                case 1:
                    color = Color.DarkOrange;
                    break;
                case 2:
                    color = Color.Blue;
                    break;
                case 3:
                    color = Color.Purple;
                    break;
                case 4:
                    color = Color.Red;
                    break;
                //case 0:
                //    color = Color.Peru;
                //    break;
                //case 1:
                //    color = Color.LightSalmon;
                //    break;
                //case 2:
                //    color = Color.CornflowerBlue;
                //    break;
                //case 3:
                //    color = Color.MediumPurple;
                //    break;
                //case 4:
                //    color = Color.LightCoral;
                //    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return color;
        }

        private static bool InsideBox(Vector2 compPos, Rectangle cellBox)
        {
            float x = compPos.X;
            float y = compPos.Y;
            if (x >= cellBox.X && x < cellBox.X + cellBox.Width)
                if (y >= cellBox.Y && y < cellBox.Y + cellBox.Height)
                    return true;
            return false;
        }

        private static int minY = 40;
        private static int maxY = 460;

        private static int minX = 200;
        private static int maxX = 540;

        static int xdist = (maxX - minX) / 3;
        static int ydist = (maxY - minY) / 3;

        private static Rectangle GetCellBox(int i, int j)
        {

            Rectangle rect = new Rectangle(minX + i*xdist,
                                           minY + j*ydist,
                                           xdist,
                                           ydist);
            return rect;
        }
    }
}
