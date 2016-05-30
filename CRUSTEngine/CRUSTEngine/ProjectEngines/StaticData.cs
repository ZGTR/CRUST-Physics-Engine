using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CRUSTEngine.FormsManipualtion;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PCGEngine.PlayabilityManagers.Ryse;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines
{
    [Serializable]
    public class StaticData
    {
        public static float[][] FrictionTable = new float[6][];
        public static float[][] RestitutionTable = new float[6][];
        public static float[] DensityTable = new float[6];
        public static float GravityConstant = 9.81f;
        //public static double LargingFactor = 100;
        public static float MassDivConst = 100000;

        // Update diff
        public static float Dtime = 0.08f;

        // Global Data
        public static EngineManager EngineManager;
        public static CTRLevelDesigner CtrLevelDesigner;

        private static EngineManager _engineManagerLastLevel;

        public static String EntraImageInput = "EntraImageInput.jpg";
        public static String EntraImageOutput = "EntraImageOutput.jpg";
        public static String EntraImageOutputPolysOnly = "EntraImageOutputPolysOnly.jpg";

        public static int FrogCookieRDist = 50;

        public static int LevelFarWidth = 900;
        public static int LevelFarHeight = 550;

        public static bool IsEntraActivated = false;
        public static bool IsEntraPathActivated = false;
        public static int EntraPathIndex = 0;

        public const int BubbleWithBlowerRange = 250;

        public static Vector3 BumpHalfSize = new Vector3(30, 15f, 0);

        public static long UpdatesSoFar = 0;

        public static void SetEngineManagerLastLevel(EngineManager engine)
        {
            _engineManagerLastLevel = engine;
        }

        public static EngineManager GetEngineManagerLastLevel()
        {
            return _engineManagerLastLevel;
        }


        public static ManipulationGameMode ManipulationGameMode = ManipulationGameMode.NeutralMode;
        public static PlayPauseMode CurrentPausePlayGameMode = PlayPauseMode.PauseMode;
        public static SessionMode GameSessionMode = SessionMode.PlayingMode;


        public static Visual2D CurrentVisual2D;
        public static RigidBody RigidToConnectToRope;
        public static int CurrentRopeIdToMove;
        public static int CurrentPendulumIdToMove;

        // Constants
        public static float AirFrictionConstant = 0.02f;
        public static float SpringCoeffConst =10000;
        public static float NormanLength = 1;
        public static float InnerFriction = 0.2f;
        //public static float GroundRepelConstant = 100.0f;
        //public static float GroundSlideFrictionConstant = 0.2f;
        //public static float GroundApsorpationConstant = 2.0f;
        
        public static int PlaneOffsetLeft = 0;
        public static int PlaneOffsetUp = 0;
        
        
        public static bool IsWater = false;
        public static int WaterInitialLevel = 400;
        public static int CircleDefaultSize = 20;
        public static Vector3 BoxDefaultSize = new Vector3(30,30,0);

        // Bubble Service
        public static Vector3 BubbleForceUp = new Vector3(0, 10, 0);
        public static int BubbleDimBubble = 50;
        public static int BubbleCloseArea = 25;

        // Blower Service
        public static float BlowerDimWidth = 40;
        public static float BlowerDimHeight = 35;
        public static int BlowerForceOnRigid = 300;
        public static int BlowerEffectAreaRadius = 80;
        
        // ConstraintSolver
        public static int CSTargetRadiusArea = 20;
        public static int CSMaxRounds = 20;
        public static float CSTimeStep = StaticData.Dtime * 6;

        public static int RocketCarrierCloseArea = 30;
        public static Vector3 RocketCarrierHalfSize = new Vector3(30, 15, 0);
        public static float RocketCarrierForceThrottle = 1000;

        public static Vector2 BasicParticleVelocity = new Vector2(3f, 3f);
        public static Vector2 BasicSlowParticleVelocity = new Vector2(1f, 1f);
        public static int MinTTL = 20;
        public static int MaxNextTTL = 20;
        public static int MaxRopeLength = 270;
        public static bool IsNotification = true;
        public static Vector3 FrogHalfSize = new Vector3(30, 30, 0);
        public static SpriteBatch SpriteBatch;

        public static void InitializeEngine(Game1 game1)
        {
            game1 = game1 ?? new Game1();
            if (game1.SpriteBatch != null)
            {
                SpriteBatch = game1.SpriteBatch;
            }
            if (EngineManager == null)
            {
                EngineManager = new EngineManager(game1);
            }
            else
            {
                EngineManager.Game1 = game1;
            }
        }

        public static void LoadDataContent(EngineManager engineManager)
        {
            EngineManager = engineManager;

            PlaneOffsetLeft = 0;
            PlaneOffsetUp = 0;

            // Friction table
            FrictionTable[0] = new float[6];
            FrictionTable[1] = new float[6];
            FrictionTable[2] = new float[6];
            FrictionTable[3] = new float[6];
            FrictionTable[4] = new float[6];
            FrictionTable[5] = new float[6];

            FrictionTable[0][0] = 0.8f;

            FrictionTable[0][1] = FrictionTable[1][0] = 0.6f;
            FrictionTable[0][2] = FrictionTable[2][0] = 0.9f;
            FrictionTable[0][3] = FrictionTable[3][0] = 0.7f;
            FrictionTable[0][4] = FrictionTable[4][0] = 1f;
            FrictionTable[0][5] = FrictionTable[5][0] = 0.1f;
            FrictionTable[1][1] = 0.4f;
            FrictionTable[1][2] = FrictionTable[2][1] = 0.56f;
            FrictionTable[1][3] = FrictionTable[3][1] = 0.5f;
            FrictionTable[1][4] = FrictionTable[4][1] = 0.62f;
            FrictionTable[1][5] = FrictionTable[5][1] = 0.1f;
            FrictionTable[2][2] = 0.95f;
            FrictionTable[2][3] = FrictionTable[3][2] = 0.7f;
            FrictionTable[2][4] = FrictionTable[4][2] = 0.8f;
            FrictionTable[2][5] = FrictionTable[5][2] = 0.1f;
            FrictionTable[3][3] = 0.75f;
            FrictionTable[3][4] = FrictionTable[4][3] = 0.7f;
            FrictionTable[3][5] = FrictionTable[5][3] = 0.1f;
            FrictionTable[4][4] = 0.8f;
            FrictionTable[4][5] = FrictionTable[5][4] = 0.1f;
            FrictionTable[5][5] = 0.1f;

            // Restitution table
            RestitutionTable[0] = new float[6];
            RestitutionTable[1] = new float[6];
            RestitutionTable[2] = new float[6];
            RestitutionTable[3] = new float[6];
            RestitutionTable[4] = new float[6];
            RestitutionTable[5] = new float[6];
            RestitutionTable[0][0] = 0.9f;
            RestitutionTable[0][1] = RestitutionTable[1][0] = 0.7f;
            RestitutionTable[0][2] = RestitutionTable[2][0] = 0.7f;
            RestitutionTable[0][3] = RestitutionTable[3][0] = 0.7f;
            RestitutionTable[0][4] = RestitutionTable[4][0] = 0.8f;
            RestitutionTable[0][5] = RestitutionTable[5][0] = 0.2f;
            RestitutionTable[1][1] = 0.5f;
            RestitutionTable[1][2] = RestitutionTable[2][1] = 0.55f;
            RestitutionTable[1][3] = RestitutionTable[3][1] = 0.55f;
            RestitutionTable[1][4] = RestitutionTable[4][1] = 0.6f;
            RestitutionTable[1][5] = RestitutionTable[5][1] = 0.2f;
            RestitutionTable[2][2] = 0.8f;
            RestitutionTable[2][3] = RestitutionTable[3][2] = 0.7f;
            RestitutionTable[2][4] = RestitutionTable[4][2] = 0.65f;
            RestitutionTable[2][5] = RestitutionTable[5][2] = 0.2f;
            RestitutionTable[3][3] = 0.8f;
            RestitutionTable[3][4] = RestitutionTable[4][3] = 0.6f;
            RestitutionTable[3][5] = RestitutionTable[5][3] = 0.2f;
            RestitutionTable[4][4] = 0.75f;
            RestitutionTable[4][5] = RestitutionTable[5][4] = 0.2f;
            RestitutionTable[5][5] = 0.2f;  

            // Density Table
            DensityTable[0] = 1200;
            DensityTable[1] = 700;
            DensityTable[2] = 2500;
            DensityTable[3] = 7800;
            DensityTable[4] = 2400;
            DensityTable[5] = 2200;
        }

        public static RyseUsageManager RyseComponentsUsageHelper;
    }
}
