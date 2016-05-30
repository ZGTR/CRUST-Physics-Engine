using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    public static class CompsOfInterestProlog
    {
        private static String _prologEnginePath = @"C:\CTREngine\CompsInterestEngine.jar";
        private static Process _process;
        public static String GetCompsOfInterestFromProlog()
        {
            String str = String.Empty;
            SetEngineStateIntoPredicatesFile();
            RunProlog();
            String strResult = GetPrologActionsString();
            if (strResult != String.Empty)
            {
                String[] positions = GetPositionsString(strResult);
                str = GetCompsString(positions);
            }
            return str;
        }

        private static string GetCompsString(string[] positions)
        {
            List<Point> list = new List<Point>();
            String strFinal = String.Empty;
            for (int i = 0; i < positions.Count(); i++)
            {
                if (positions[i] != String.Empty)
                {
                    int x = Int32.Parse(positions[i].Split(',')[0].Trim());
                    int y = Int32.Parse(positions[i].Split(',')[1].Trim());
                    list.Add(new Point(x, y));
                }
            }
            strFinal = EngineStateManager.GetEngineStateFactStringWithEnterDelimiterToProlog(list);
            return strFinal;
        }

        private static String[] GetPositionsString(string strResult)
        {
            String[] pos = strResult.Split('(');
            for (int i = 0; i < pos.Count(); i++)
            {
                pos[i] = pos[i].Replace(")", "").Trim();
            }
            return pos;
        }

        private static void SetEngineStateIntoPredicatesFile()
        {
            String factsString = EngineStateManager.GetEngineStateFactStringWithEnterDelimiterToProlog();

            StreamReader sR = new StreamReader(@"C:\CTREngine\ActionsOnlyPred.txt");
            String actionsOnlyString = sR.ReadToEnd();
            sR.Close();

            StreamWriter sW = new StreamWriter(@"C:\CTREngine\CompsFinalPred.pl");
            sW.WriteLine(factsString);
            sW.WriteLine(actionsOnlyString);
            sW.Close();
            //Console.WriteLine(factsString);
        }

        private static void RunProlog()
        {
            _process = new Process();
            _process.EnableRaisingEvents = false;
            _process.StartInfo.FileName = "java.exe";
            _process.StartInfo.Arguments = "-jar " + '"' + _prologEnginePath;
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.UseShellExecute = false;
            _process.Start();
            _process.WaitForExit();
            _process.Close();

            ////processInfo.WorkingDirectory = _prologEnginePath; // this is where your jar file is.
            //Process proc;

            //if ((proc = Process.Start(processInfo)) == null)
            //{
            //    throw new InvalidOperationException("??");
            //}

            //proc.WaitForExit();

            //int exitCode = proc.ExitCode;
            //proc.Close();
        }

        private static string GetPrologActionsString()
        {
            StreamReader sR = new StreamReader(@"C:\CTREngine\CompsInterestFromProlog.txt");
            String prologString = sR.ReadToEnd();
            sR.Close();
            return prologString;
        }
    }
}
