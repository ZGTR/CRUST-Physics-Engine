using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.MusicBased
{
    class MPCGHelper
    {
        public static List<int> ConvertFileToTTN()
        {
            List<int> intTimes = new List<int>();

            String str = @"0.541
                        1.455
                        1.809
                        2.162
                        3.292
                        3.652
                        3.999
                        5.156
                        5.503
                        5.843
                        ";
            string[] times = str.Split('\n');
            foreach (string time in times)
            {
                try
                {
                    int i = (int)(float.Parse(time.Trim()) * 1000);
                    intTimes.Add(i);
                }
                catch (Exception)
                {
                }
            }
            
            List<int> intDur = new List<int>();
            //intDur.Add(0);
            for (int i = 0; i < intTimes.Count - 1; i++)
            {
                int dur = intTimes[i + 1] - intTimes[i];
                intDur.Add(dur);
            }
            intDur.Add(0);
            return intDur;
        }
    }
}

