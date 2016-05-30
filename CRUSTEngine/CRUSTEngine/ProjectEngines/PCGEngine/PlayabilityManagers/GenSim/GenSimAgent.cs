using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ClipperLib;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Bubble;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Actions;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components;
using CRUSTEngine.ProjectEngines.PCGEngine.EventsManager.Components.Comps;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators;
using CRUSTEngine.ProjectEngines.PCGEngine.Generators.GenManagers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.EntraPlay.Handlers;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim.GevaInterpreter;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using Action = CRUSTEngine.ProjectEngines.PCGEngine.Actions.Action;

namespace CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.GenSim
{
    public class GenSimAgent
    {
        public bool IsSuccess;
        public PlayabilitySimulatorEngineProlog Simulator;
        public static int VoidInitCount = 0;
        private int _ropeId;
        private int _bubbleId;
        private int _blowerId;
        private Random _rand;
        private int[] _xRange;
        private int[] _yRange;
        private int[] _ropeLengthRange;
        public List<CATimePair> CATPairs;
        private CATimePair _nextCTP;
        public String LevelStr;
        public List<Action> Actions;
        private List<Vector2> _placedCompSoFar;
        public EngineShotsManager Shotter;
        public List<CATimePair> BestCTPPairs;
        public bool IsTesting = false;
        public bool IsPathIntersection;
        public int MAXTRY = 40;
        //public int CookieFrogMinDist;
        public int ActionsSoFar;
        private bool _isDirRandomized = true;
        private bool _isRopesRandomized = true;
        public static List<UpdateActionPair> UpdateActionsIds;
        public int OverlappedComponents;
        private List<ActionTimePair> _atPairs;

        public bool IsOverlapping
        {
            get { return this.OverlappedComponents != 0; }
        }

        public float CompsScatteredSoFar;

        public GenSimAgent(List<ActionTimePair> atPairs)
        {
            _isRopesRandomized = true;
            _isDirRandomized = true;
            _rand = new Random(DateTime.Now.Millisecond);
            CATPairs = BuildEventStream(atPairs);
            Init();
        }

        public GenSimAgent(List<CATimePair> catPairs, bool isDirRandomized, bool isRopesRandomized)
        {
            _isDirRandomized = isDirRandomized;
            _isRopesRandomized = isRopesRandomized;
            CATPairs = catPairs;
            Init();
        }


        private List<CATimePair> BuildEventStream(List<ActionTimePair> atPairs)
        {
            _atPairs = atPairs;
            List<CATimePair> res = new List<CATimePair>();
            CompTimePair ctPair;
            foreach (ActionTimePair atPair in atPairs)
            {
                switch (atPair.EType)
                {
                    case EventType.BlowerPress:
                        res.Add(new CompTimePair(ComponentType.Blower, atPair.KeyTime - 1));
                        res.Add(atPair);
                        break;
                    case EventType.RopeCut:
                        res.Add(atPair);
                        break;
                    case EventType.BubblePinch:
                        ctPair = GetPlacementCTPair(atPair, atPairs);
                        res.Add(ctPair);
                        res.Add(atPair);
                        break;
                    case EventType.RocketPress:
                        ctPair = GetPlacementCTPair(atPair, atPairs);
                        res.Add(ctPair);
                        res.Add(atPair);
                        break;
                    case EventType.BumperInteraction:
                        res.Add(new CompTimePair(ComponentType.Bump, atPair.KeyTime - 1));
                        res.Add(atPair);
                        break;
                    case EventType.OmNomFeed:
                        res.Add(new CompTimePair(ComponentType.Frog, atPair.KeyTime));
                        //res.Add(atPair);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return res;
        }

        private CompTimePair GetPlacementCTPair(ActionTimePair atPair, List<ActionTimePair> atPairs)
        {
            int minTS, maxTS;
            GetPlacementMinMaxTS(atPair, atPairs, out minTS, out maxTS);
            //int placeTS = //_rand.Next(minTS, maxTS);

            int placeTS = _rand.Next(minTS, (minTS + maxTS) / 2);
            CompTimePair res = null;
            switch (atPair.EType)
            {
                case EventType.BlowerPress:
                    break;
                case EventType.RopeCut:
                    break;
                case EventType.BubblePinch:
                     res = new CompTimePair(ComponentType.Bubble, placeTS);
                    break;
                case EventType.RocketPress:
                    res = new CompTimePair(ComponentType.Rocket, placeTS);
                    break;
                case EventType.BumperInteraction:
                    break;
                case EventType.OmNomFeed:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return res;
        }

        private void GetPlacementMinMaxTS(ActionTimePair atPair, List<ActionTimePair> atPairs, out int minTs, out int maxTs)
        {
            minTs = 5 * 60;
            maxTs = atPair.KeyTime - 10;

            int indexMe = atPairs.IndexOf(atPair);
            if (indexMe - 1 >= 0)
            {
                minTs = atPairs[indexMe - 1].KeyTime + 1;
            }
            //if (indexMe + 1 < atPairs.Count)
            //{
            //    maxTs = atPairs[indexMe + 1].KeyTime;
            //}
        }

        private void Init()
        {
            CompsScatteredSoFar = 0;
            OverlappedComponents = 0;
            UpdateActionsIds = new List<UpdateActionPair>();
            //CookieFrogMinDist = Int32.MaxValue;
            ActionsSoFar = 0;
            _rand = new Random(DateTime.Now.Millisecond);
            _placedCompSoFar = new List<Vector2>();
            //this.CATPairs[3].KeyTime = 412;
            //CATPairs[2].KeyTime = 367;
            IsPathIntersection = false;
            _ropeId = 0;
            _bubbleId = 0;
            IsSuccess = false;
            Simulator = new PlayabilitySimulatorEngineProlog(false);
            Actions = new List<Action>();

            _rand = new Random(DateTime.Now.Millisecond);
            InitializeRanges();
            _nextCTP = CATPairs[0];

            String[] args = new String[2];

            String ropesStr = String.Empty;
            if (_isRopesRandomized)
            {
                ropesStr = GenerateRopesString();
            }
            else
            {
                ropesStr = GenerateRopesStringFromCatPairs();
            }
            
            LevelStr = "cookie(200, 300)" + ropesStr;
            args[1] = LevelStr;

            LevelBuilder.CreateRestedLevel(LevelStr, false);
            AddCandyPlacement();
            if(IsTesting) Shotter.TakeEngineShot(false);
        }

        private string GenerateRopesStringFromCatPairs()
        {
            List<CompTimePair> ropes = CATPairs.Where(c =>
                {
                    if (c is CompTimePair)
                        if ((c as CompTimePair).CType == ComponentType.Rope)
                            return true;
                    return false;
                }).Cast<CompTimePair>().ToList();
            String res = String.Empty;
            ropes.ForEach(r => res += "rope(" + r.Args[1] + "," + r.Args[2] + "," + r.Args[3] + ")");
            return res;
        }

        private String GenerateRopesString()
        {
            int ropesCount = CATPairs.Count(c => 
                    {
                        var atPair = c as ActionTimePair;
                        return atPair != null && atPair.EType == EventType.RopeCut;
                    });
            String ropesStr = "";
            for (int i = 0; i < ropesCount; i++)
            {
                Rope r = new Rope(GetRandomX(),
                                  GetRandomY(),
                                  GetRandomRopeLength()
                    );
                ropesStr += r.ToString() + " ";
                //r.AddSelfToEngine();
                //CATPairs.Remove(CATPairs.First(c => c == ComponentType.Rope));
            }
            return ropesStr;
        }

        private void AddCandyPlacement()
        {
            var newPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            var res = (from vec in _placedCompSoFar
                       where (vec - newPos).Length() < 10
                       select vec).ToList();
            bool added = false;
            foreach (Vector2 v in res)
            {
                if (_placedCompSoFar[_placedCompSoFar.Count - 1] != v)
                {
                    _placedCompSoFar.Add(newPos);
                    added = true;
                    break;
                }
                else
                {
                    added = true;
                }
            }
            if (!added)
                _placedCompSoFar.Add(newPos);
        }

        //public void ScatterComps()
        //{
        //    int attempts = 0;
        //    if (!_isDirRandomized && !_isRopesRandomized)
        //    {
        //        this.IsSuccess = ExecuteScatter();
        //    }
        //    else
        //    {
        //        while (attempts < MAXTRY)
        //        {
        //            Init();
        //            this.IsSuccess = ExecuteScatter();
        //            if (IsSuccess)
        //            {
        //                this.BestCTPPairs = this.CATPairs;
        //                break;
        //            }
        //            attempts++;
        //        }
        //    }
        //}

        public bool ExecuteScatter()
        {
            bool isSuccess = true;
            StaticData.GameSessionMode = SessionMode.PlayingMode;
            long c1 = StaticData.UpdatesSoFar;
            for (int i = 0; i < VoidInitCount; i++)
            {
                ExecuteAction(new VoidAction());
            }
            while (true)
            {
                long c = StaticData.UpdatesSoFar;
                //UpdateCookieFrogDistance();
                if (!CookieInBoundry() || IsPathIntersected() || IsComponentsOverlapping())
                {
                    if (IsTesting) Shotter.TakeEngineShot(false);
                    isSuccess = false;
                    break;     
                }
                if (IsNextCompActionHere())
                {
                    if (_nextCTP is CompTimePair)
                    {
                        PlaceNewComp(_nextCTP as CompTimePair);
                        AddCandyPlacement();
                    }
                    //if (IsTesting) Shotter.TakeEngineShot(false);
                    if (_nextCTP is ActionTimePair)
                    {
                        AddCandyPlacement();
                        ExecuteAction(GetCTPAction(_nextCTP as ActionTimePair));
                        ActionsSoFar++;
                    }
                    if (IsTesting) Shotter.TakeEngineShot(false);
                    if (CATPairs.IndexOf(_nextCTP) < CATPairs.Count - 1)
                    {
                        UpdateNextCTP();
                    }
                    else
                    {
                        if (IsTesting) Shotter.TakeEngineShot(false);
                        if (IsPathIntersected())
                        {
                            isSuccess = false;
                        }
                        break;
                    }
                }
                else
                {
                    ExecuteAction(new VoidAction());
                }
            }
            this.IsSuccess = isSuccess;
            return isSuccess;
        }

        //private void UpdateCookieFrogDistance()
        //{
        //    float dist =
        //        (StaticData.EngineManager.CookieRB.PositionXNACenter2D -
        //         StaticData.EngineManager.FrogRB.PositionXNACenter2D).Length();
        //    if (dist < CookieFrogMinDist)
        //    {
        //        CookieFrogMinDist = (int)dist;
        //    }
        //}

        private bool IsPathIntersected()
        {
            for (int i = 0; i < _placedCompSoFar.Count - 1; i++)
            {
                for (int j = 1; j < _placedCompSoFar.Count - 1; j++)
                {
                    Vector2 pInter = Vector2.Zero;
                    Vector2 p1 = _placedCompSoFar[i],
                            p2 = _placedCompSoFar[i + 1],
                            p3 = _placedCompSoFar[j],
                            p4 = _placedCompSoFar[j + 1];

                    if (_placedCompSoFar[i].X > 543 && _placedCompSoFar[i].X < 544)
                    {
                        int lk123 = 10;
                    }

                    if (_placedCompSoFar[j].X > 558 && _placedCompSoFar[j].X < 559)
                    {
                        int lk123 = 10;
                    }

                    if (MathHelperModule.IsIntersecting(p1, p2, p4, p3))
                    {
                        MathHelperModule.FindIntersection(p1, p2, p3, p4, out pInter);
                        if ((pInter != Vector2.Zero)
                            && (pInter != p1)
                            && (pInter != p2)
                            && (pInter != p3)
                            && (pInter != p4))
                        {
                            IsPathIntersection = true;
                            //if (IsReallyIntersection(pInter, new Vector2[] { p1, p2, p3, p4 }))
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool IsReallyIntersection(Vector2 pInter, Vector2[] vec)
        {
            float minDist = (pInter - vec[0]).Length();
            Vector2 nP = vec[0];
            foreach (Vector2 v in vec)
            {
                float dis = (pInter - v).Length();
                if (dis< minDist)
                {
                    minDist = dis;
                    nP = v;
                }
            }
            if ((pInter - nP).Length() > 10)
            {
                return true;
            }
            return false;
        }

        //private CompTimePair GetCompTypeOfPair(CATimePair nextCTP)
        //{
        //    if (nextCTP is ActionTimePair)
        //    {
        //        switch ((nextCTP as ActionTimePair).EType)
        //        {
        //            case ActionType.BlowerPress:
        //                return new CompTimePair(ComponentType.Blower, nextCTP.KeyTime);
        //                break;
        //            case ActionType.RopeCut:
        //                return new CompTimePair(ComponentType.Rope, nextCTP.KeyTime);
        //                break;
        //            case ActionType.VoidAction:
        //                break;
        //            case ActionType.BubblePinch:
        //                break;
        //            case ActionType.TerminateBranch:
        //                break;
        //            case ActionType.RocketPress:
        //                break;
        //            case ActionType.BumperInteraction:
        //                break;
        //            case ActionType.OmNomFeed:
        //                break;
        //            default:
        //                throw new ArgumentOutOfRangeException();
        //        }
        //    }
        //    return nextCTP as CompTimePair;
        //}
        
        private bool CookieInBoundry()
        {
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            if (cPos.X > 0 && cPos.X < StaticData.LevelFarWidth - 50)
            {
                if (cPos.Y > 0 && cPos.Y < StaticData.LevelFarHeight - 50)
                {
                    return true;
                }
            }
            return false;
        }

        private Action GetCTPAction(ActionTimePair nextCTP)
        {
            int indexAdded = 0;
            switch (nextCTP.EType)
            {
                case EventType.BlowerPress:
                    indexAdded = _blowerId;
                    _blowerId++;
                    return new BlowerPress(indexAdded);
                    break;
                case EventType.RopeCut:
                    indexAdded = _ropeId;
                    //StaticData.EngineManager.SpringsManagerEngine.ListOfServices.Count - 1;
                    _ropeId++;
                    return new RopeCut(indexAdded);
                    break;
                case EventType.BubblePinch:
                    indexAdded = _bubbleId;
                    //StaticData.EngineManager.BubbleManagerEngine.ListOfServices.Count - 1;
                    _bubbleId++;
                    return new BubblePinch(indexAdded);
                    //return new VoidAction();
                    break;
                case EventType.RocketPress:
                    return new RocketPress();
                    //return new VoidAction();
                    break;
                case EventType.BumperInteraction:
                    return new VoidAction();
                    //return new VoidAction();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ExecuteAction(Action nextAction)
        {
            long sss = StaticData.UpdatesSoFar;
            UpdateActionsIds.Add(new UpdateActionPair(StaticData.UpdatesSoFar, nextAction));
            Actions.Add(nextAction);
            Simulator.ActionsFrequency = GenSimAgent.GetActionsFrequency(nextAction);
            Simulator.RunEngine(new ActionsGenerator(nextAction));
        }


        private void PlaceNewComp(CompTimePair nextCTP)
        {
            CompsScatteredSoFar++;
            //Shotter.TakeEngineShot();
            var fEngine = ObjectSerializer.DeepCopy(StaticData.EngineManager);
            for (int i = 0; i < 15; i++)
            {
                fEngine.Update(new GameTime());    
            }

            Vector2 cPossss= StaticData.EngineManager.CookieRB.PositionXNA2D;
            Vector2 cPos = fEngine.CookieRB.PositionXNA2D;
                //StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            int cX = (int)cPos.X;
            int cY = (int)cPos.Y;
            Component cAdd = new Rope(0,0,100);
            Vector3 acc = StaticData.EngineManager.CookieRB.GetAcceleration();
            switch (nextCTP.CType)
            {
                case ComponentType.Cookie:
                    break;
                case ComponentType.Frog:
                    cAdd = GetFrogPosition();
                    break;
                case ComponentType.Blower:
                    cAdd = GetBlowerPosition(nextCTP);
                    break;
                case ComponentType.Rope:
                    break;
                case ComponentType.Bubble:
                    cAdd = new Bubble(cX - (int) StaticData.BubbleDimBubble/2,
                                      cY - (int) StaticData.BubbleDimBubble/2);
                        //GetBubblePosition();
                    break;
                case ComponentType.Water:
                    break;
                case ComponentType.Rocket:
                    int dirR = _rand.Next(8);
                    cAdd = new Rocket(cX - (int) StaticData.RocketCarrierHalfSize.X,
                                      cY - (int) StaticData.RocketCarrierHalfSize.Y, (Direction) dirR);
                        //GetRocketPosition(nextCTP);
                    break;
                case ComponentType.Bump:
                    int dirB = _rand.Next(3);
                    dirB = dirB == 2 ? 3 : dirB;
                    if (dirB == 1 || dirB == 3)
                    {
                        var fEngineB = ObjectSerializer.DeepCopy(StaticData.EngineManager);
                        for (int i = 0; i < 25; i++)
                        {
                            fEngineB.Update(new GameTime());
                        }
                        cPos = fEngine.CookieRB.PositionXNA2D;
                        cX = (int)cPos.X;
                        cY = (int)cPos.Y;
                    }
                    cAdd = new Bump(cX - (int) StaticData.BumpHalfSize.X,
                                    cY - (int) StaticData.BumpHalfSize.Y, (Direction) dirB);
                    //cAdd = GetBumperPosition(nextCTP);
                    break;
                case ComponentType.Cracker:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (!(cAdd is Rope))
            {
                cAdd.AddSelfToEngine();
                LevelStr += cAdd.ToString();
            }
        }

        private Component GetFrogPosition()
        {
            Vector3 frogHalfDim = StaticData.FrogHalfSize;
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            int cX = (int)cPos.X - (int)frogHalfDim.X;
            int cY = (int)cPos.Y - (int)frogHalfDim.Y;

            return new Frog(cX, cY);
        }

        private Component GetBubblePosition()
        {
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            int cX = (int)cPos.X;
            int cY = (int)cPos.Y;
            int rY, rX;
            Component cAdd = new Rope(0, 0, 100);
            Vector3 vel = StaticData.EngineManager.CookieRB.GetVelocity();
            int dir = 0;
            int halfDim = StaticData.BubbleDimBubble / 2;
            int closeArea = StaticData.BubbleCloseArea - 2;


            if (vel.Y > 0)
            {
                rY = cY - 10;
            }
            else
            {
                rY = cY + 10;
            }

            rX = cX - halfDim;
            cAdd = new Bubble(rX, rY);
            return cAdd;
        }

        private Component GetRocketPosition(CompTimePair nextCTP)
        {
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            int cX = (int)cPos.X;
            int cY = (int)cPos.Y;
            int rX, rY;
            Component cAdd = new Rope(0, 0, 100);
            Vector3 vel = StaticData.EngineManager.CookieRB.GetVelocity();
            int dir = 0;
            int closeArea = StaticData.RocketCarrierCloseArea - 10;
            int halfDimX = (int)(StaticData.RocketCarrierHalfSize.X / 2);
            int halfDimY = (int)(StaticData.RocketCarrierHalfSize.Y / 2);

            if (!_isDirRandomized)
            {
                dir = Int32.Parse(nextCTP.Args[1]);
            }
            else
            {
                dir = _rand.Next(4);
            }
            //dir = 7;
            if (vel.Y > 0)
            {
                rY = cY - halfDimY;
            }
            else
            {
                rY = cY + halfDimY;
            }

            if (vel.X > 0)
            {
                rX = cX + halfDimX;
            }
            else
            {
                rX = cX - halfDimX;
            }

            cAdd = new Rocket(rX, rY, (Direction)dir);
            return cAdd;
        }

        private Component GetBumperPosition(CompTimePair nextCTP)
        {
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            int cX = (int)cPos.X;
            int cY = (int)cPos.Y;
            Component cAdd = new Rope(0, 0, 100);
            Vector3 vel = StaticData.EngineManager.CookieRB.GetVelocity();
            int dir = 0;

            if (!_isDirRandomized)
            {
                dir = Int32.Parse(nextCTP.Args[1]);
            }
            else
            {
                dir = _rand.Next(4);
            }

            int defX = cX - (int) StaticData.BumpHalfSize.X;
            int defY = cY - (int)StaticData.BumpHalfSize.Y;
            int X = 0, Y = 0;
            int diffS = 20;
            int diff = 35;
            int diffYLean = 40;
            int diffXLean = 40;

            switch ((Direction)dir)
            {
                case Direction.East:
                    X = defX;
                    Y = vel.Y > 0 ? cY - diff : cY + diff;
                    break;
                case Direction.SouthEast:
                    X = vel.X > 0 ? defX + diffS : defX - diffS;
                    Y = vel.Y > 0 ? cY + diffYLean : cY - diffYLean;
                    break;
                case Direction.South:
                    X = defX;
                    Y = vel.Y > 0 ? cY + diff : cY - (int)(2 * StaticData.BumpHalfSize.Y) - diff;
                    break;
                case Direction.SouthWest:
                    X = vel.X > 0 ? defX + diffS : defX - diffS;
                    Y = vel.Y > 0 ? cY + diffYLean : cY - diffYLean;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            cAdd = new Bump(X, Y, (Direction)dir);
            return cAdd;
        }

        private Component GetBlowerPosition(CompTimePair nextCTP)
        {
            Vector2 cPos = StaticData.EngineManager.CookieRB.PositionXNACenter2D;
            int cX = (int)cPos.X;
            int cY = (int)cPos.Y;
            Component cAdd = new Rope(0, 0, 100);
            Vector3 acc = StaticData.EngineManager.CookieRB.GetVelocity();
            int dir = 0;

            if (!_isDirRandomized)
            {
                dir = Int32.Parse(nextCTP.Args[1]);
            }
            else
            {
                dir = _rand.Next(2);
                dir = dir == 1 ? 4 : 0;
            }
            //dir = 4;

            int x = 0, y = 0;
            if (acc.Y > 0)
            {
                y = cY - 10;
            }
            else
            {
                y = cY + 10;
            }

            if (dir == 4)
            {
                x = cX + (int)StaticData.BlowerDimWidth;
            }
            else
            {
                x = cX - (int)StaticData.BlowerDimWidth;
            }

            cAdd = new Blower(x, y, (Direction)dir);
            return cAdd;
        }

        private bool IsNextCompActionHere()
        {
            if (CATPairs.IndexOf(_nextCTP) < CATPairs.Count)
            {
                if (StaticData.UpdatesSoFar >= _nextCTP.KeyTime)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateNextCTP()
        {
            var lastCTP = _nextCTP;
            var res = (from p in CATPairs
                       where p.KeyTime > _nextCTP.KeyTime
                       orderby p.KeyTime
                       select p).ToList();
            _nextCTP = res.First();
        }

        public List<ActionTimePair> GetActionsToDo()
        {
            List<ActionTimePair> res = this.CATPairs.Where(c => c is ActionTimePair).Cast<ActionTimePair>().ToList();
            return res;
        }

        public static int GetActionsFrequency(Action action)
        {
            int newTimeStep = 1;
            return newTimeStep;
        }

        #region Fields and Init
        private void InitializeRanges()
        {
            _xRange = new int[] { 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 520, 540 };
            _yRange = new int[]
                {
                    60, 80, 100, 120, 140, 160, 180
                    //, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440,460
                };
            _ropeLengthRange = new int[] { 100, 130, 160, 190, 220 };
        }

        private int GetRandomX()
        {
            return _xRange[_rand.Next(_xRange.Count())];
        }

        private int GetRandomY()
        {
            return _yRange[_rand.Next(_yRange.Count())];
        }

        private int GetRandomRopeLength()
        {
            return _ropeLengthRange[_rand.Next(_ropeLengthRange.Count())];
        }
        #endregion

        public bool IsComponentsOverlapping()
        {
            var ovlpedPairs = new List<KeyValuePair<List<IntPoint>, List<IntPoint>>>();
            //String[] args = new string[2];
            //args[1] = this.LevelStr;
            //GenManager.GenerateGevaLevel(args, null);
            OverlappedComponents = 0;
            DefinitiveCompPolyHandler def = new DefinitiveCompPolyHandler(StaticData.EngineManager);
            List<List<IntPoint>> polys = new List<List<IntPoint>>();

            polys.AddRange(def.GetDefBubblesPolys());
            polys.AddRange(def.GetDefRocketsPolys());
            polys.AddRange(def.GetDefBumpersPolys());

            foreach (BlowerService blowerService in StaticData.EngineManager.BlowerManagerEngine.ListOfServices)
            {
                polys.Add(PolysHelper.GetShapeSquarePoly(blowerService.PositionXNACenter,
                                                         (int) StaticData.BlowerDimWidth/2));
            }

            for (int i = 0; i < polys.Count - 1; i++)
            {
                var polyNow = polys[i];
                for (int j = i + 1; j < polys.Count; j++)
                {
                    var polyCompare = polys[j];
                    if (polyNow != polyCompare)
                    {
                        if(EntraSolver.IsPolyOperation(polyCompare, polyNow, ClipType.ctIntersection))
                        {
                            //List<KeyValuePair<List<IntPoint>, List<IntPoint>>>
                            var res = (from kv in ovlpedPairs
                                       where (kv.Key == polyCompare && kv.Value == polyNow)
                                             || (kv.Key == polyNow && kv.Value == polyCompare)
                                       select kv).ToList();
                            if (res != null)
                            {
                                ovlpedPairs.Add(new KeyValuePair<List<IntPoint>, List<IntPoint>>(polyNow, polyCompare));
                                OverlappedComponents++;
                            }
                        }
                    }
                }
            }
            return OverlappedComponents != 0;
        }

        public List<CompTimePair> GetAllComponents()
        {
            List<CompTimePair> res = this.CATPairs.Where(c => c is CompTimePair).Cast<CompTimePair>().ToList();
            return res;
        }
    }

    public class UpdateActionPair
    {
        public Action Action;
        public long UpdateId;

        public UpdateActionPair(long updatesSoFar, Action nextAction)
        {
            this.UpdateId = updatesSoFar;
            this.Action = nextAction;
        }
    }
}