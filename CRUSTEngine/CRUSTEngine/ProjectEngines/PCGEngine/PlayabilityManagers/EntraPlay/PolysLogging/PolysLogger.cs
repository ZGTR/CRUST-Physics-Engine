using System;
using System.Collections.Generic;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.PolysLogging
{
    [Serializable]
    public class PolysLogger
    {
        public List<PolyLog> Logs; 
        public PolysLogger()
        {
            Logs = new List<PolyLog>();
        }

        public void Log(PolyLog log)
        {
            //if (log.Poly != null)
            {
                //if (log.ApPairs.Count != 0)
                {
                    var addedLog = Logs.Find(l => l.Comp == log.Comp);
                    if (addedLog != null)
                    {
                        //EntraDrawer.DrawIntoFileTesting(log.Poly);
                        //EntraDrawer.DrawIntoFileTesting(addedLog.Poly);
                        addedLog.ApPairs.AddRange(log.ApPairs);
                        //= EntraSolver.GetPolySolution(addedLog.Poly, log.Poly, ClipType.ctUnion);
                        //EntraDrawer.DrawIntoFileTesting(addedLog.Poly);
                        //_logs.Remove(addedLog);
                    }
                    else
                    {
                        Logs.Add(log);
                    }
                }
            }
        }

        public List<PolyLog> GetLog()
        {
            return Logs;
        }
    }
}
