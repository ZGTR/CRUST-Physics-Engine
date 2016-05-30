using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse
{
    class RyseAgent
    {
        //private String _engineFileString =
        //    @"Z:\ZGTR Physics Engine - PCG 13\SourceCode\CRUSTEngine_withEclipse\XNAInForms\XNAInForms\bin\x86\Debug\engineStateRested";
        //private String _levelGenFileString =
        //    @"Z:\ZGTR Physics Engine - PCG 13\SourceCode\CRUSTEngine_withEclipse\XNAInForms\XNAInForms\bin\x86\Debug\levelGenBytes";
        
        private static String _actionsFinalPredFilePath = @"C:\CTREngine\ActionsFinalPred.pl";
        private static String _actionsOnlyPredFilePath = @"C:\CTREngine\ActionsOnlyPred.txt";
        private static String _actionsFromPrologFilePath = @"C:\CTREngine\ActionsFromProlog.txt";
        private static String _prologEnginePath = @"C:\CTREngine\PrologEngine.jar";
        public static int VoidInitPlayTotalCount = 0;
        public int MaxNrOfActionsPreformed = 70 ;
        public int MaxNrOfNodesExplored = 1500 ;
        public LevelGenerator LevelGeneratorEngine;
        private const int RunFreelyToRestFreq = 10*60;

        public bool IsSaveImage = false;
        public bool IsTotalNew;
        public bool IsShowWindow = false;
        //public bool IsZGTRPlaying = false;
        //public bool IsDesign = false;
        //public bool IsTestHere = false;
        public bool IsPrintPositionOnly = true;
        public static bool WithWalls = false;
        public int NrOfTerminates = 0;
        public int TerminateLevelSum = 0;

        //public string[] Args;
        public PlayabilitySimulatorEngineProlog Simulator;
        public EngineShotsManager EngineShotsManager;

        public RyseAgent(int voidInitPlayTotalCount)
        {
            VoidInitPlayTotalCount = voidInitPlayTotalCount;
            MaxNrOfActionsPreformed += VoidInitPlayTotalCount;
            MaxNrOfNodesExplored += VoidInitPlayTotalCount;
            prologTime = 0;
        }

        //public RYSEManager(int voidInitPlayTotalCount, string strLevel, bool isPlayabilityCheckerOnly)
        //{
        //    StaticData.GameSessionMode = SessionMode.DesignMode;
        //    this.StrLevel = strLevel;
        //    LevelBuilder.CreateRestedLevel(this.StrLevel, isPlayabilityCheckerOnly);
        //    VoidInitPlayTotalCount = voidInitPlayTotalCount;
        //    MaxNrOfActionsPreformed += VoidInitPlayTotalCount;
        //    MaxNrOfNodesExplored += VoidInitPlayTotalCount;
        //}

        public int prologTime = 0;
        public int totalTime = 0;
        public int maxDepthArr = 0;
        public int nodesExplored = 0;
        public double bestClosestFrogCookieDist = Double.MaxValue;
        public List<Action> bestPerformedActions = new List<Action>();
        public List<Vector3> bestPerformedActionsVelocities = new List<Vector3>();
        public int NrLevelterminatesStdMin = 1000;
        public int NrLevelterminatesStdMax = -1000;

        //public void SimulatePlayability(EngineManager engineManager, ref bool isPlayable,
        //    int nrOfActionSoFar, List<Action> performedActions, List<Vector3> velocitesInActions, int voidInitPlayCount, PlayabilityCheckMode checkMode)
        //{
        //    SimulatePlayability(null, engineManager, ref isPlayable,
        //                        nrOfActionSoFar, performedActions, velocitesInActions, voidInitPlayCount, checkMode);
        //}


        public void SimulatePlayability(ActionNode node, EngineManager engineManager, ref bool isPlayable,
                                        int nrOfActionSoFar, List<Action> performedActions,
                                        List<Vector3> velocitesInActions, int voidInitPlayCount,
                                        PlayabilityCheckMode checkMode)
        {
            try
            {


                StaticData.GameSessionMode = SessionMode.PlayingMode;
                nrOfActionSoFar++;
                if (nrOfActionSoFar == 1)
                {
                    int k = 0;
                }
                //EngineManager currentOriginalEngineManager = ObjectSerializer.DeepCopy(engineManager);
                //StaticData.EngineManager = currentEngineManager;
                if (!isPlayable)
                {
                    if (nodesExplored < MaxNrOfNodesExplored)
                    {
                        if (nrOfActionSoFar < MaxNrOfActionsPreformed)
                        {
                            if (nrOfActionSoFar > maxDepthArr)
                            {
                                maxDepthArr = nrOfActionSoFar;
                            }
                            // Shortcut - Check if the cookie is so far away; cut this branch of tree then!
                            if (StaticData.EngineManager.CookieRB.GetVelocity() == Vector3.Zero)
                            {
                                return;
                            }

                            if (GenericHelperModule.CookieOutsideWindow())
                            {
                                return;
                            }
                            DateTime d1 = DateTime.Now;
                            List<Action> listOfAllPossibleActions = GetNextActionsSet(ref voidInitPlayCount, checkMode);
                            DateTime d2 = DateTime.Now;
                            prologTime += (d2 - d1).Milliseconds;

                            if (node != null)
                                node.Childs.AddRange(listOfAllPossibleActions.ConvertAll(a => new ActionNode(a)));
                            if (listOfAllPossibleActions.Count == 1)
                            {
                                if (listOfAllPossibleActions[0] is TerminateBranch)
                                {
                                    NrOfTerminates++;
                                    TerminateLevelSum += nrOfActionSoFar;
                                    if (nrOfActionSoFar < NrLevelterminatesStdMin)
                                    {
                                        NrLevelterminatesStdMin = nrOfActionSoFar;
                                    }

                                    if (nrOfActionSoFar > NrLevelterminatesStdMax)
                                    {
                                        NrLevelterminatesStdMax = nrOfActionSoFar;
                                    }
                                    return;
                                }
                            }

                            if (listOfAllPossibleActions.Count == 0)
                            {
                                listOfAllPossibleActions.Add(new VoidAction());
                            }

                            //listOfAllPossibleActions = ReOrderActions(listOfAllPossibleActions);
                            foreach (Action action in listOfAllPossibleActions)
                            {
                                if (action is RocketPress)
                                {
                                    //if (((RopeCut)action).RopeId == 2 || ((RopeCut)action).RopeId == 3)
                                    //{
                                    int i = 231231231;
                                    //}
                                }
                                if (action is BubblePinch)
                                {
                                    //if (((RopeCut)action).RopeId == 2 || ((RopeCut)action).RopeId == 3)
                                    //{
                                    int i = 231231231;
                                    //}
                                }
                                this.Simulator.ActionsFrequency = GetActionsFrequency(action);
                                if (!isPlayable)
                                {
                                    nodesExplored++;
                                    StaticData.EngineManager = ObjectSerializer.DeepCopy(engineManager);
                                    performedActions.Add(action);
                                    velocitesInActions.Add(StaticData.EngineManager.CookieRB.GetVelocity());
                                    isPlayable = SimulatePlayabilityForPartialAction(new List<Action>() {action});
                                    if (IsShowWindow)
                                        EngineShotsManager.ShowXNAWindow();

                                    double newBest = Simulator.ClosestCookieFrogDistance;
                                    if (newBest < bestClosestFrogCookieDist)
                                    {
                                        bestClosestFrogCookieDist = newBest;
                                        bestPerformedActions = new List<Action>();
                                        bestPerformedActions.AddRange(performedActions);

                                        bestPerformedActionsVelocities = new List<Vector3>();
                                        bestPerformedActionsVelocities.AddRange(velocitesInActions);
                                    }
                                    ActionNode childNode = null;
                                    if (node != null)
                                        childNode = node.Childs.Where(cNode => cNode.Action == action).First();
                                    SimulatePlayability(childNode, StaticData.EngineManager, ref isPlayable,
                                                        nrOfActionSoFar,
                                                        performedActions, velocitesInActions, voidInitPlayCount,
                                                        checkMode);
                                    if (!isPlayable)
                                    {
                                        if (performedActions.Count > 0)
                                            performedActions.RemoveAt(performedActions.Count - 1);
                                        if (velocitesInActions.Count > 0)
                                            velocitesInActions.RemoveAt(velocitesInActions.Count - 1);
                                    }
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            StaticData.EngineManager = ObjectSerializer.DeepCopy(engineManager);
                            isPlayable = SimulatePlayabilityForNoCurrentAction(RunFreelyToRestFreq);
                            return;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("In Simulate " + e.ToString());
            }
        }

        public static List<Action> GetNextActionsSet(ref int voidInitPlayCount, PlayabilityCheckMode checkMode)
        {
            if (voidInitPlayCount < VoidInitPlayTotalCount)
            {
                voidInitPlayCount++;
                return new List<Action>() { new VoidAction() };
            }
            List<Action> listOfAllPossibleActions = null;
            switch (checkMode)
            {
                case PlayabilityCheckMode.NormalCheck:
                    // Set Engine State Into Predicates File
                    SetEngineStateIntoPredicatesFile();
                    
                    // Run Prolog
                    GenericHelperModule.RunJavaProcess(_prologEnginePath);

                    // Get new actions to test
                    String fullPrologString = GetPrologActionsString();
                    listOfAllPossibleActions = GetAllPossibleActions(fullPrologString);
                    break;
                case PlayabilityCheckMode.RandomCheck:
                    listOfAllPossibleActions = new List<Action>() { RandomPlayabilityGenerator.GetNewRandomAction() };
                    break;
                default:
                    throw new ArgumentOutOfRangeException("checkMode");
            }
            return listOfAllPossibleActions;
        }

        private List<Action> ReOrderActions(List<Action> listOfAllPossibleActions)
        {
            List<Action> orderedList = new List<Action>();
            List<RopeCut> ropesAction = (from action in listOfAllPossibleActions
                                         where action is RopeCut
                                         select action as RopeCut).ToList();
            ropesAction.Sort(delegate(RopeCut r1, RopeCut r2)
                                 {
                                     return (StaticData.EngineManager.SpringsManagerEngine.ListOfServices[r1.RopeId].GetMass(0).PositionXNA.X <
                                         StaticData.EngineManager.SpringsManagerEngine.ListOfServices[r2.RopeId].GetMass(0).PositionXNA.X ? 0 : 1);
                                 }); 
            orderedList.AddRange(listOfAllPossibleActions);
            ropesAction.ForEach(rope => orderedList.Remove(rope));
            ropesAction.ForEach(rope => orderedList.Insert(0, rope));
            return orderedList;
        }

        public static int GetActionsFrequency(Action action)
        {
            int newTimeStep = 15;
            switch (action.AType)
            {
                case ActionType.BlowerPress:
                    newTimeStep = 15;
                    break;
                case ActionType.RopeCut:
                    newTimeStep = 10;
                    break;
                case ActionType.VoidAction:
                    newTimeStep = 12;
                    break;
                case ActionType.BubblePinch:
                    newTimeStep = 15;
                    break;
                case ActionType.RocketPress:
                    newTimeStep = 17;
                    break;
                default:
                    newTimeStep = 15;
                    break;
            }
            return newTimeStep;
        }

        private bool SimulatePlayabilityForNoCurrentAction(int freelyFreq)
        {
            double frogCookieDist = Double.MaxValue;
            frogCookieDist = this.RunEngineFreely(freelyFreq, false);
            if (frogCookieDist < PlayabilitySimulatorEngineProlog.NarrativeDist)
            {
                return true;
            }
            return false;
        }

        public bool SimulatePlayabilityForPartialAction(List<Action> partialActions)
        {
            double frogCookieDist = Simulator.RunEngine(new ActionsGenerator(partialActions));
            if (frogCookieDist < PlayabilitySimulatorEngineProlog.NarrativeDist)
            {
                return true;
            }
            return false;
        }

        //public double RunEngine(List<Action> listOfActions)
        //{
        //    ActionsGenerator actionsGenerator = new ActionsGenerator(listOfActions);
        //    //simulator = new PlayabilityEngineSimulatorProlog(this.IsSaveImage);
        //    double frogCookieDist = Simulator.RunEngine(actionsGenerator);
        //    return frogCookieDist;
        //}

        public double RunEngineFreely(int time, bool withResting)
        {
            //PlayabilityEngineSimulatorProlog simulator = new PlayabilityEngineSimulatorProlog(this.IsSaveImage);
            double frogCookieDist = Simulator.RunEngineFreelyWithPlayabilityCheck(time, new GameTime());
            return frogCookieDist;
        }

        private static List<Action> GetAllPossibleActions(string fullPrologString)
        {
            ActionsGenerator actionsGenerator = new ActionsGenerator(fullPrologString);
            return actionsGenerator.Actions;
        }

        private static string GetPrologActionsString()
        {
            StreamReader sR = new StreamReader(_actionsFromPrologFilePath);
            String prologString = sR.ReadToEnd();
            sR.Close();
            prologString = ReShapeString(prologString);
            return prologString;
        }

        private static string ReShapeString(string prologString)
        {
            String strOut = "";
            List<String> strArr = prologString.Split(' ').ToList();
            List<String> strArrOut = new List<string>();
            for (int i = 0; i < strArr.Count; i++)
            {
                if (strArr[i].Contains("rope_cut_else"))
                {
                    strArr[i] = strArr[i].Replace("rope_cut_else", "rope_cut");
                    strArrOut.Add(strArr[i]);
                    //strArrOut.Add("void_action");
                }
                else
                {
                    strArrOut.Add(strArr[i]);
                }
            }
            strArrOut.RemoveAll((String str) =>
                              {
                                  if (str == " " || str == "") return true;
                                  return false;
                              });
            strArrOut.ForEach(s => strOut += s + " ");
            return strOut;
        }

        #region nonChangingPart
        private static void SetEngineStateIntoPredicatesFile()
        {
            String factsString = EngineStateManager.GetEngineStateFactStringWithEnterDelimiterToProlog();

            StreamReader sR = new StreamReader(_actionsOnlyPredFilePath);
            String actionsOnlyString = sR.ReadToEnd();
            sR.Close();

            StreamWriter sW = new StreamWriter(_actionsFinalPredFilePath);
            sW.WriteLine(factsString);
            sW.WriteLine(actionsOnlyString);
            sW.Close();
            //Console.WriteLine(factsString);
        }

        //public void InitilaizeEngine(bool IsPlayabilityCheckerOnly)
        //{
        //    if (IsTestHere)
        //    {
        //        this.Args = new string[3];
        //        Args[0] = "0";
        //        Args[1] = "0";
        //        Args[2] = "0";
        //    }

        //    if (Args[0] == "0")     // 0: If we are creating the level for the first time, save it into a file
        //    {
        //        CreateRestedLevelFirstTime(IsPlayabilityCheckerOnly);
        //    }
        //    else                    // 1: else, we have created the engine before; just retieve the level from file
        //    {
        //        //RetrieveLevelEngineStateFromFile();
        //    }
        //}

#endregion
    }
}
