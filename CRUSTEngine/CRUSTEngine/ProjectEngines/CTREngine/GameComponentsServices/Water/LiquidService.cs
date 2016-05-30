using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Water
{
    [Serializable]
    public class LiquidService : IUpdatableComponent
    {
        public List<RigidBody> RigidsInService { set; get; }
        private Visual2D _wave1;
        private Visual2D _wave2;
        private Visual2D _waterVisual;
        public static Vector3 ForceLiquid = new Vector3(0, 0, 0);
        public static Vector3 TorqueLiquid = new Vector3(0, 0, 0);
        public static int LiquidLevel = StaticData.WaterInitialLevel;
        public static float LiquidDensity = 50f;
        public static bool WaterIsRising = false;
        //public int LiquidLevel 
        //{ 
        //    set
        //    {
        //        _LiquidLevel = StaticData.EngineManager.Window.ClientBounds.Height - value;
        //    }
        //    get
        //    {
        //        return _LiquidLevel + StaticData.EngineManager.Window.ClientBounds.Height;
        //    }
        //}
        

        public LiquidService()
        {
            /*
            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(DefaultAdder.GetDefaultBox(new Vector3(10, 10, 0),
                Material.Steel, 
                new Vector3(10, 10 ,0),
                new Vector3(0, -9.8f, 0), null, null));

            StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(DefaultAdder.GetDefaultSphere(new Vector3(60, 10, 0),
            Material.Steel,
            10,
            new Vector3(0, -9.8f, 0), null, null));
            */

            this._wave1 = new Visual2D(new Rectangle(0, 0,0,0), TextureType.WaterWave);
            this._wave2 = new Visual2D(new Rectangle(0, 0, 0, 0), TextureType.WaterWave);
            this._waterVisual = new Visual2D(new Rectangle(0, LiquidLevel + 10, StaticData.LevelFarWidth,
                                                           StaticData.LevelFarHeight - LiquidLevel),
                                             TextureType.VisualWater);
            RigidsInService = new List<RigidBody>();
        }

        private List<RigidBody> GetAllRigidsMatchService()
        {
            List<RigidBody> listToReturn = new List<RigidBody>();
            //if (StaticData.EngineManager.CookieRB.PositionXNA.Y >= LiquidLevel)
            //{
            //    listToReturn.Add(StaticData.EngineManager.CookieRB);
            //}
            foreach (var rigidBody in StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids)
            {
                if (!(rigidBody is RocketCarrierService))
                {
                    if (rigidBody.PositionXNA.Y >= LiquidLevel)
                    {
                        listToReturn.Add(rigidBody);
                    }
                }
            }
            foreach (var rigidBody in StaticData.EngineManager.RigidsManagerEngine.ListOfSphereRigids)
            {
                if (rigidBody.PositionXNA.Y >= LiquidLevel)
                {
                    listToReturn.Add(rigidBody);
                }
            }
            return listToReturn;
        }

        private float liquidMinus;
        public void Update(GameTime gameTime)
        {
            if (WaterIsRising)
            {
                if (StaticData.GameSessionMode == SessionMode.PlayingMode)
                {
                    liquidMinus += 0.05f;
                    if (LiquidLevel > 0)
                        LiquidLevel -= (int) liquidMinus;
                    if (liquidMinus > 1f)
                        liquidMinus = 0;
                }
            }

            RigidsInService = GetAllRigidsMatchService();
            foreach (var rigidBody in RigidsInService)
            {
                rigidBody.AddForce(GetBuoyancyForce(rigidBody));
                //rigidBody.AddForce(ForceLiquid);
                //rigidBody.AddTorque(TorqueLiquid, rigidBody.PositionCenterEngine +
                //                                  new Vector3(+rigidBody.Height/(float) 2, +rigidBody.Width/(float) 2, 0)*
                //                                  rigidBody.Mass/StaticData.MassDivConst);
            }
        }

        private Vector3 GetBuoyancyForce(RigidBody rigidBody)
        {
            int maxDepth = LiquidLevel + rigidBody.Height;
            // from cm^3 to m^3
            float volume = rigidBody.Volume / 1000;//rigidBody.Width*rigidBody.Height / (float)100;
            Vector3 forceToAdd = Vector3.Zero;

            // calculate subemersion
            float depth = rigidBody.PositionXNA.Y;

            // out of water?
            if (depth <= LiquidLevel - rigidBody.Height)
                return new Vector3(0,0,0);

            // maximum depth?
            if (depth >= LiquidLevel + rigidBody.Height)
            {
                forceToAdd.Y = LiquidDensity * volume * 100;
                return forceToAdd;
            }
            //depth = StaticData.LevelFarHeight - rigidBody.PositionXNA.Y +
            //        rigidBody.Height/(float) 2;
            // then, partly subemerged
            forceToAdd.Y = LiquidDensity*(volume)*
                ((LiquidLevel - rigidBody.PositionXNA.Y + rigidBody.Height))*5;
            return forceToAdd;
        }

        private int xWave1 = 0;
        private int xWave2 = -StaticData.LevelFarWidth; 
        public void Draw(GameTime gameTime)
        {
            UpdateVisualWater();
            _waterVisual.Draw(gameTime, Color.LightSeaGreen);
            _wave1.Draw(gameTime);
            _wave2.Draw(gameTime);
        }

        private void UpdateVisualWater()
        {
            int amountOfSpeed = (int)LiquidService.ForceLiquid.X + 1;
            int windowWidth = 900;// StaticData.LevelFarWidth;
            int windowHeight = 550;// StaticData.LevelFarHeight;
            if (xWave1 < windowWidth)
            {
                xWave1 += amountOfSpeed;
                _wave1.RectangleArea = new Rectangle(xWave1, LiquidLevel, windowWidth, 20);
            }
            else
            {
                xWave1 = 0;
            }
            if (xWave1 < windowWidth)
            {
                xWave2 += amountOfSpeed;
                _wave2.RectangleArea = new Rectangle(xWave2, LiquidLevel, windowWidth, 20);
            }
            else
            {
                xWave2 = -StaticData.LevelFarWidth;
            }
            _waterVisual.RectangleArea = new Rectangle(0, _wave1.RectangleArea.Y + 10, windowWidth, windowHeight);
        }
    }
}
