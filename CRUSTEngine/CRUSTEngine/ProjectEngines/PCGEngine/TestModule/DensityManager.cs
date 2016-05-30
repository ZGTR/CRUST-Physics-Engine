using System;
using System.Collections.Generic;
using System.Linq;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;

namespace CRUSTEngine.ProjectEngines.PCGEngine.TestModule
{
    class DensityManager
    {
        public static List<double> GetDensityArrOfLevelFile(List<String> phenos)
        {
            List<List<Component>> levels = new List<List<Component>>();
            foreach (string pheno in phenos)
            {
                List<Component> items = LevelGenerator.ConvertToItems(pheno);
                levels.Add(items); 
            }

            int index = 0;
            List<Double> densityArr = new List<Double>();
            for (int i = 0; i < levels.Count; i++)
            {
                List<Component> currentLevel = levels[i];
                int[][] levelCells = ActivatedCells(currentLevel);
                double mean = currentLevel.Count() / NumOfActivated(levelCells, n, m);
                double std = CalcStd(levelCells, n, m, mean);
                densityArr.Add(std);
            }
            return Normalize(densityArr);
        }

        private static double CalcStd(int[][] levelCells, int n, int m, double mean)
        {
            double std = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                {
                    std += (levelCells[i][j] == 0 ? 0 : Math.Pow(levelCells[i][j] - mean, 2));
                }
            std /= (NumOfActivated(levelCells, n, m) - 1);
            return std;
        }

        private static List<Double> Normalize(List<Double> arr)
        {
            double max = GetMax(arr);
            double min = GetMin(arr);
            if (min < 0)
                for (int i = 0; i < arr.Count(); i++)
                    arr[i] = arr[i] - min;

            max = GetMax(arr);
            min = GetMin(arr);
            double diff = max - min;
            List<Double> normalizedArr = new List<Double>();
            for (int i = 0; i < arr.Count(); i++)
            {
                normalizedArr.Add(arr[i] / diff);
            }
            return normalizedArr;
        }

        private static double GetMax(List<Double> arr)
        {
            double max = Double.MinValue;
            for (int i = 0; i < arr.Count(); i++)
            {
                if (arr[i] > max)
                    max = arr[i];
            }
            return max;
        }

        private static double GetMin(List<Double> arr)
        {
            double min = Double.MaxValue;
            for (int i = 0; i < arr.Count(); i++)
            {
                if (arr[i] < min)
                    min = arr[i];
            }
            return min;
        }

        private static int NumOfActivated(int[][] arr, int n, int m)
        {
            int result = 0;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    result += (arr[i][j] == 0 ? 0 : 1);
            return result;
        }

        private static int n = 3;
        private static int m = 3;
        public static int[][] ActivatedCells(List<Component> currentLevel)
        {
            int[][] result = new int[n][];
            for (int i = 0; i < n; i++)
            {
                result[i] = new int[m];
            }
            for (int i = 0; i < currentLevel.Count(); i++)
            {
                Component currentComp = currentLevel[i];
                double x = (double)(currentComp.X - 200) / (540 - 200);
                if (x < 0.333333)
                    x = 0;
                else if (x < 0.66666)
                    x = 1;
                else x = 2;

                double y = (double)(currentComp.Y - 40) / (460 - 40);
                if (y < 0.333333)
                    y = 0;
                else if (y < 0.66666)
                    y = 1;
                else y = 2;
                result[(int)y][(int)x]++;
            }
            return result;
        }
    }
}
