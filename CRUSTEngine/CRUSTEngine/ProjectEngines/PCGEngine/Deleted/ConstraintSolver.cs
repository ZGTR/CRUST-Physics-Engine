//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using CRUSTEngine.ProjectEngines.HelperModules;
//using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
//using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

//namespace CRUSTEngine.ProjectEngines.PCGEngine.Deleted
//{
//    [Serializable]
//    public class ConstraintSolver
//    {
//        private readonly EngineManager _engineManagerOriginal;
//        private Action[] _actions;
//        private Effector[] _effectors;
//        private Random _random;

//        public ConstraintSolver(EngineManager engineManager)
//        {
//            _engineManagerOriginal = engineManager;
//            _random = new Random((int)(DateTime.Now.Ticks));

//            _actions = new Action[1];
//            _actions[0] = new RopeCut(0);

//            _effectors = new Effector[3];
//            _effectors[0] = Effector.BlowerToRightEffector;
//            _effectors[1] = Effector.BlowerToRightEffector;
//            _effectors[2] = Effector.BlowerToRightEffector;

//        }

//        public List<EventSolver> SolveConstraint(Vector3 targetPos, int numOfEffectors, int numOfActions)
//        {
//            int iEffect = 0, iAction = 0;
//            var events = new List<EventSolver>();
//            bool IsPathFind = false;
//            MakeEventSolution(ref events, this._engineManagerOriginal, targetPos, numOfEffectors, numOfActions,
//                              ref IsPathFind, false);
//            return events;
//        }

//        private void MakeEventSolution(ref List<EventSolver> events, EngineManager currentEM, Vector3 targetPos,
//             int remEffectors, int remActions, ref bool isPathFind, bool shouldExtendFreeFall)
//        {
//            while (true)
//            {
//                // Check if we reached the target
//                if (RigidsHelperModule.IsCloseEnough(currentEM.CookieRB, targetPos, StaticData.CSTargetRadiusArea))
//                {
//                    isPathFind = true;
//                    return;
//                }
//                else
//                {
//                    if (remActions == 0 && remEffectors == 0 && !shouldExtendFreeFall)
//                    {
//                        return;
//                    }
//                }

//                int nextEventIdentifier = _random.Next(2);
//                EventSolver currentEvent = null;

//                if (remEffectors > 0)
//                {
//                    currentEvent = CreateNewEvent(_effectors[0], currentEM);
//                    remEffectors--;
//                }
//                else
//                {
//                    if (shouldExtendFreeFall)
//                    {
//                        currentEvent = CreateNewVoidEvent(currentEM);
//                    }
//                }

//                if (currentEvent != null)
//                {
//                    events.Add(currentEvent);
//                    List<EngineManager> listOfNextEMs = currentEvent.GetListOfNextEMs(currentEM);
//                    // else, continue searching for a solution
//                    for (int i = 0; i < listOfNextEMs.Count; i++)
//                    {
//                        if (!isPathFind)
//                        {
//                            if (i < listOfNextEMs.Count - 1)
//                                MakeEventSolution(ref events, listOfNextEMs[i], targetPos, remEffectors, remActions,
//                                                  ref isPathFind, false);
//                            else
//                            {
//                                if (remActions == 0 && remEffectors == 0)
//                                {
//                                    MakeEventSolution(ref events, listOfNextEMs[i], targetPos, remEffectors, remActions,
//                                                      ref isPathFind, true);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            return;
//                        }
//                    }
//                }
//            }
//        }

//        private EventSolver CreateNewVoidEvent(EngineManager currentEM)
//        {
//            return new VoidFreeFall(currentEM);
//        }

//        private EventSolver CreateNewEvent(Effector effector, EngineManager currentEM)
//        {
//            switch (effector)
//            {
//                case Effector.BlowerToRightEffector:
//                    return new BlowerToRightEffector(currentEM);
//                    break;
//                case Effector.BlowerToLeftEffector:
//                    return new BlowerToLeftEffector(currentEM);
//                    break;
//                case Effector.BubbleEffector:
//                    return new BubbleEffector(currentEM);
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException("effector");
//            }
//        }

//        private EventSolver CreateNewEvent(Action action, EngineManager currentEM)
//        {
//            switch (action)
//            {
//                case Action.RopeCut:
//                    return new RopeCutAction(currentEM);
//                    break;
//                default:
//                    throw new ArgumentOutOfRangeException("action");
//            }
//        }
//    }
//}
