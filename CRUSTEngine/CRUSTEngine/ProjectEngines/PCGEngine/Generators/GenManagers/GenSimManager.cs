using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim.GevaInterpreter;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;

namespace CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers
{
    class GenSimManager
    {
        public static void GenerateGevaLevel(String[] args, bool isGenSimComp, bool isDirRandomized,
                                             bool isRopesRandomized)
        {
            List<TLEvent> events = TLEventConverter.ConvertToItems(args[1]);
            int MAXTRY = 40;

            GenSimAgentWrapper agent;
            if (!isGenSimComp)
            {
                List<ActionTimePair> aTPairs = GenSimHelper.ConvertTLEventstoActionTimePair(events);
                agent = new GenSimAgentWrapper(aTPairs, MAXTRY, false);
            }
            else
            {
                List<CATimePair> caTPairs = GenSimHelper.ConvertTLEventstoCATimePair(events);
                agent = new GenSimAgentWrapper(caTPairs, isDirRandomized, isRopesRandomized, MAXTRY, false);
            }
            DateTime d1 = DateTime.Now;
            agent.ScatterComps();
            DateTime d2 = DateTime.Now;

            float fitness = GetFitness(agent.WAgent);

            PrintToFile(args, isGenSimComp, agent.WAgent, fitness, d1, d2);
        }

        public static float GetFitness(GenSimAgent agent)
        {
            float fitness = 0;
            if (agent.IsSuccess)
            {
                fitness = 0;
            }
            else
            {
                float pIntersection = agent.IsPathIntersection ? 1 : 0;
                float pOverlapping = MathHelperModule.Normalize(agent.CompsScatteredSoFar - 1,
                                                          agent.GetAllComponents().Count, 0);
                float pEventsExec = 1 - MathHelperModule.Normalize(agent.ActionsSoFar, agent.GetActionsToDo().Count, 0);
                //fitness = (1 / (float)2) * pEventsExec +
                //          (1 / (float)2) * pIntersection;
                if (pIntersection == 1)
                {
                    fitness = 1;
                }
                else
                {
                    if (pEventsExec == 0)
                    {
                        if (pOverlapping == 0)
                        {
                            fitness = 0.1f;
                        }
                        else
                        {
                            fitness = pOverlapping;    
                        }
                    }
                    else
                    {
                        fitness = pEventsExec;    
                    }
                }
            }
            return fitness;
        }

        private static void PrintToFile(String[] args, bool isGinSimComp,
                                        GenSimAgent agent, float fitness, DateTime d1, DateTime d2)
        {

            StreamWriter swGEVA = new StreamWriter(@"C:\CTREngine\PlayabilityVal_ZGTREngine.txt");
            swGEVA.WriteLine(fitness);
            swGEVA.Flush();
            swGEVA.Close();

            String playableFileName = isGinSimComp
                                          ? @"C:\CTREngine\GenSimCompLevelsPlayable.txt"
                                          : @"C:\CTREngine\GenSimLevelsPlayable.txt";

            String nonPlayableFileName = isGinSimComp
                                             ? @"C:\CTREngine\GenSimCompLevelsNonPlayable.txt"
                                             : @"C:\CTREngine\GenSimLevelsNonPlayable.txt";

            bool solved = false;
            if (agent.IsSuccess)
            {
                solved = true;
                StreamWriter swPlayLevelGeva = new StreamWriter(@"C:/CTREngine/GenSimPlayabilityLevel_ZGTREngine.txt");
                swPlayLevelGeva.WriteLine(agent.LevelStr);
                swPlayLevelGeva.Flush();
                swPlayLevelGeva.Close();
            }

            String strToFile = solved
                               + "\t" + agent.ActionsSoFar
                               + "\t" + agent.GetActionsToDo().Count
                               + "\t" + (d2 - d1).TotalMilliseconds
                               + "\t" + String.Format("{0:0.00}", fitness)
                               + "\t" + agent.LevelStr
                               + "\t" + args[1]
                               + "\t" + GenericHelperModule.GetActionsString(agent.Actions)
                               + "\t" + GenericHelperModule.GetCTPString(agent.BestCTPPairs, true)
                               + "\t" + GenericHelperModule.GetCTPString(agent.BestCTPPairs, false);

            if (agent.IsSuccess)
            {
                StreamWriter sw = new StreamWriter(playableFileName, true);
                sw.WriteLine(strToFile);
                sw.Flush();
                sw.Close();
            }
            else
            {
                StreamWriter sw2 = new StreamWriter(nonPlayableFileName, true);
                sw2.WriteLine(strToFile);
                sw2.Flush();
                sw2.Close();
            }
        }

        public static void TestGevaLevelCAAll(String[] args, bool isTestingOn, bool isGenSimComp)
        {
            List<TLEvent> events = TLEventConverter.ConvertToItems(args[1]);
            GenSimAgentWrapper agent;
            if (!isGenSimComp)
            {
                List<ActionTimePair> aTPairs = GenSimHelper.ConvertTLEventstoActionTimePair(events);
                agent = new GenSimAgentWrapper(aTPairs, 20, isTestingOn);
            }
            else
            {
                List<CATimePair> caTPairs = GenSimHelper.ConvertTLEventstoCATimePair(events);
                agent = new GenSimAgentWrapper(caTPairs, true, true, 20, isTestingOn);
            }

            //List<CATimePair> caTPairs = GenSimHelper.ConvertTLEventstoCATimePair(events);
            //GenSimAgent agent = new GenSimAgent(caTPairs, true, true)
            agent.ScatterComps();

            if (isGenSimComp)
            {
                if (agent.WAgent.Actions.Count > 0)
                {
                    SimulateOnWindow(agent.WAgent);
                }
            }

            float fitness = GetFitness(agent.WAgent);
            PrintToFile(args, isGenSimComp, agent.WAgent, fitness, DateTime.Now, DateTime.Now);

            
            //new EngineShotsManager().TakeEngineShot();

            if (isGenSimComp)
            {
                if (agent.WAgent.Actions.Count > 0)
                {
                    StreamWriter sw = new StreamWriter(@"C:\CTREngine\GenSimLevelsTest.txt", true);
                    sw.WriteLine(GetListString(agent.WAgent.Simulator.CookiePosList));
                    sw.WriteLine(GetListString(ActionsExecuterGenSim.CookiePosList));
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        private static String GetListString(List<string> cookiePosList)
        {
            String res = String.Empty;
            foreach (string s in cookiePosList)
            {
                res += s + " ";
            }
            return res;
        }

        private static void SimulateOnWindow(GenSimAgent agent)
        {
            LevelBuilder.CreateRestedLevel(agent.LevelStr, false);
            StaticData.GameSessionMode = SessionMode.PlayingMode;
            LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
            simulator.SimulateNewWindow(agent.Actions, false, false);
        }

        //public static void TestGevaLevelActionOnly(String[] args)
        //{
        //    List<TLEvent> events = TLEventConverter.ConvertToItems(args[1]);
        //    List<ActionTimePair> aTPairs = GenSimHelper.ConvertTLEventstoActionTimePair(events);
        //    GenSimAgentWrapper agent = new GenSimAgentWrapper(aTPairs,20);
        //    agent.ScatterComps();

        //    if (agent.WAgent.Actions.Count > 0)
        //    {
        //        StreamWriter sw = new StreamWriter(@"C:\CTREngine\GenSimLevels.txt", true);
        //        sw.WriteLine(agent.WAgent.LevelStr
        //            + "\t" + HelperModules.GenericHelperModule.GetActionsString(agent.WAgent.Actions)
        //            + "\t" + GenericHelperModule.GetCTPString(agent.WAgent.BestCTPPairs, true)
        //            + "\t" + GenericHelperModule.GetCTPString(agent.WAgent.BestCTPPairs, false)
        //            );
        //        sw.Flush();
        //        sw.Close();
        //    }

        //    if (agent.WAgent.Actions.Count > 0)
        //    {
        //        LevelBuilder.CreateRestedLevel(agent.WAgent.LevelStr, false);
        //        //var c = agent.Actions[15];
        //        StaticData.GameSessionMode = SessionMode.PlayingMode;
        //        LivePlayabilitySimulator simulator = new LivePlayabilitySimulator(StaticData.EngineManager);
        //        simulator.SimulateNewWindow(agent.WAgent.Actions);
        //    }
        //}
    }
}
