using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.HelperModules
{
    class GenericHelperModule
    {
        public static float GetProperOrientation(Direction dir)
        {
            switch (dir)
            {
                case Direction.East:
                    return 0;
                    break;
                case Direction.SouthEast:
                    return 45;
                    break;
                case Direction.South:
                    return 90;
                    break;
                case Direction.SouthWest:
                    return 135;
                    break;
                case Direction.West:
                    return 180;
                    break;
                case Direction.NorthWest:
                    return 225;
                    break;
                case Direction.North:
                    return 270;
                    break;
                case Direction.NorthEast:
                    return 315;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }
        }

        //public static bool IsIntersecting(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        //{
        //                //Cross Product (hope I got it right here)
        //    float fc = (B.X*C.Y) - (B.Y*C.X); //<0 == to the left, >0 == to the right
        //    float fd = (B.X*D.Y) - (B.Y*D.X);

        //    if( (fc<0) && (fd<0)) //both to the left  -> No Cross!
        //        return false;
        //    if ((fc > 0) && (fd > 0)) //both to the right -> No Cross!
        //        return false;
        //    return true;
        //}

        //public static bool IsIntersecting(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        //{
        //    float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
        //    float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
        //    float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

        //    // Detect coincident lines (has a problem, read below)
        //    if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

        //    float r = numerator1 / denominator;
        //    float s = numerator2 / denominator;

        //    return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        //}

        //public static int GetNumericalDirection(Dir direction)
        //{
        //    switch (direction)
        //    {
        //        case Dir.East:
        //            return (int) Dir.East + 1;
        //            break;
        //        case Dir.SouthEast:
        //            break;
        //        case Dir.South:
        //            break;
        //        case Dir.SoutWest:
        //            break;
        //        case Dir.West:
        //            break;
        //        case Dir.NorthWest:
        //            break;
        //        case Dir.North:
        //            break;
        //        case Dir.NorthEast:
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException("direction");
        //    }
        //}

        public static bool CookieOutsideWindow()
        {
            if (StaticData.EngineManager.CookieRB.PositionXNACenter.X > 900 || StaticData.EngineManager.CookieRB.PositionXNACenter.X < 0
                        || StaticData.EngineManager.CookieRB.PositionXNACenter.Y > 520 || StaticData.EngineManager.CookieRB.PositionXNACenter.Y < 20)
                return true;
            return false;
        }

        public static string GetActionsString(List<Action> performedActions)
        {
            if (performedActions != null)
            {
                String str = String.Empty;
                for (int i = 0; i < performedActions.Count; i++)
                {
                    Action currentAction = performedActions[i];
                    str += currentAction.ToString() + " ";
                }
                return str;
            }
            else
            {
                return String.Empty;
            }
        }

        public static bool CookieIsInDirectionOf(Vector2 posCompCenterCenter, Direction dir)
        {
            Vector2 posCookie = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            return InDirectionOf(posCompCenterCenter, posCookie, dir);
        }

        public static bool InDirectionOf(Vector2 posBlowerCenter, Vector2 posCompCenter, Direction dir)
        {
            switch (dir)
            {
                case Direction.East:
                    return (posBlowerCenter.X < posCompCenter.X);
                    break;
                case Direction.SouthEast:
                    return (posBlowerCenter.X < posCompCenter.X && posBlowerCenter.Y < posCompCenter.Y);
                    break;
                case Direction.South:
                    return (posBlowerCenter.Y < posCompCenter.Y);
                    break;
                case Direction.SouthWest:
                    return (posBlowerCenter.X > posCompCenter.X && posBlowerCenter.Y < posCompCenter.Y);
                    break;
                case Direction.West:
                    return (posBlowerCenter.X > posCompCenter.X);
                    break;
                case Direction.NorthWest:
                    return (posBlowerCenter.X > posCompCenter.X && posBlowerCenter.Y > posCompCenter.Y);
                    break;
                case Direction.North:
                    return (posBlowerCenter.Y > posCompCenter.Y);
                    break;
                case Direction.NorthEast:
                    return (posBlowerCenter.X < posCompCenter.X && posBlowerCenter.Y > posCompCenter.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }
        }

        public static string GetVector3ListString(List<Vector3> performedVel)
        {
            String velString = String.Empty;
            for (int i = 0; i < performedVel.Count; i++)
            {
                velString += "(" + (int)performedVel[i].X + "," + (int)performedVel[i].Y + ")";
            }
            return velString;
        }


        public static void RunJavaProcess(String path)
        {
            var process = new Process();
            process.EnableRaisingEvents = false;
            process.StartInfo.FileName = "java.exe";
            process.StartInfo.Arguments = "-jar " + '"' + path;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();
            process.Close();

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

        public static string GetCTPString(List<CATimePair> bestCTPPairs, bool convertToSec)
        {
            String str = "";
            if (bestCTPPairs != null)
            {
                foreach (CATimePair ctpPair in bestCTPPairs)
                {
                    String val = String.Empty;
                    if (convertToSec)
                        val = String.Format("{0:0}", ((ctpPair.KeyTime*1000)/(double) 60));
                    else
                        val = ctpPair.KeyTime.ToString();
                    str += ctpPair.ToString() + "(" + val + ")";
                }
            }
            return str;
        }
    }
}
