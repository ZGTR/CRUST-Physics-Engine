using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket.ParticleEngine;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.CatchableRopes;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.CollisionEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Managers;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Services.Springs;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket
{
    [Serializable]
    public class RocketCarrierService : BoxRigid, IUpdatableComponent
    {
        private ParticleEngineCore _particleEngine;
        public Vector3 ForceThrottle { set; get; }
        public bool IsActivated = false;
        public bool IsCookieCatched = false;
        public Vector2 VelocityVec;
        public Direction Dir;
        private double timeStamp1;
        private double timeStamp2;
        //public SpringService RocketSpring;
        public bool CanCatchCookie = true;
        public Vector3 PositionXNAInitial;

        public RocketCarrierService(Vector3 positionXNA, Direction dir)
            : base(positionXNA, Material.Ice, StaticData.RocketCarrierHalfSize)
        {
            this.TextureType = TextureType.Rocket;
            this.IsCollidable = false;
            this.Dir = dir;
            this.UpdateDirection(dir);
            this.PositionXNAInitial = this.PositionXNA;
            this._particleEngine = new ParticleEngineCore(new Vector2(this.PositionXNA.X, this.PositionXNA.Y), 30, 20, 1);
            //StaticData.EngineManager.RigidsManagerEngine.AddRigidBody(this);
            
        }

        public void UpdateDirection(Direction dir)
        {
            this.Dir = dir;
            this.VelocityVec = SetVelocityVec(dir);
            //this.TextureType = SetPropperTexture(this.Dir);
        }

        private Vector2 SetVelocityVec(Direction dir)
        {
            //float velocityVal = StaticData.BasicSlowParticleVelocity.X;
            Vector2 veclocityVec = new Vector2();
            switch (dir)
            {
                case Direction.East:
                    veclocityVec = new Vector2(-1, 0);
                    break;
                case Direction.SouthEast:
                    veclocityVec = new Vector2(-1, -1);
                    break;
                case Direction.South:
                    veclocityVec = new Vector2(0, -1);
                    break;
                case Direction.SouthWest:
                    veclocityVec = new Vector2(1, -1);
                    break;
                case Direction.West:
                    veclocityVec = new Vector2(1, 0);
                    break;
                case Direction.NorthWest:
                    veclocityVec = new Vector2(1, 1);
                    break;
                case Direction.North:
                    veclocityVec = new Vector2(0, 1);
                    break;
                case Direction.NorthEast:
                    veclocityVec = new Vector2(-1, 1);
                    break;
                //default:
                    //throw new ArgumentOutOfRangeException("dir");
            }
            return veclocityVec;
            //return new Vector2(-1, -1);
        }

        private bool firstTime = true;
        public override void Update(GameTime gameTime)
        {
            if (firstTime)
            {
                StaticData.EngineManager.CollisionManagerEngine.AddRigidWithNonCollidableRigids(this, StaticData.EngineManager.CookieRB);
                firstTime = false;
            }

            if (CanCatchCookie)
            {
                if (base.IsClicked && StaticData.GameSessionMode == SessionMode.DesignMode
                    && StaticData.ManipulationGameMode == ManipulationGameMode.ChangingCompsDirection)
                {
                    // Change the direction
                    if ((int) this.Dir < 7)
                    {
                        UpdateDirection((Direction) ((int) this.Dir + 1));
                    }
                    else
                    {
                        this.Dir = Direction.East;
                        UpdateDirection((Direction) ((int) this.Dir));
                    }
                }
                if (StaticData.GameSessionMode == SessionMode.PlayingMode)
                {
                    if (!IsCookieCatched && !IsActivated)
                    {
                        // Is the cookie near?
                        if (RigidsHelperModule.IsCloseEnough(StaticData.EngineManager.CookieRB, this,
                                                             StaticData.RocketCarrierCloseArea))
                        {
                            if (!IsCookieAttachedToRope())
                            {
                                this.IsCookieCatched = true;
                                this.IsActivated = true;
                                StaticData.EngineManager.RocketsCarrierManagerEngine.SetRocketNew(this);
                                this._particleEngine.MaxParticles = 4;
                            }
                        }
                    }
                    if (IsCookieCatched)
                    {
                        if (!CookieCollidedWithBump())
                        {
                            StaticData.EngineManager.CookieRB.PositionXNA = this.PositionXNACenter -
                                                StaticData.EngineManager.CookieRB.getHalfSize();
                        }
                        else
                        {
                            this.CanCatchCookie = false;
                        }
                        if (IsClicked)
                        {
                            this.CanCatchCookie = false;
                            var c = StaticData.EngineManager.CookieRB;
                            c.SetLastFrameAccelaration(Vector3.Zero);
                            c.SetInitTorque(0);
                            c.SetOrientation(0);
                            c.SetVelocity(Vector3.Zero);
                            c.AddForce(new Vector3(0, 3000, 0));
                            //Vector3 force = GetForceVector(this.Dir);
                            //StaticData.EngineManager.CookieRB.AddForce(new Vector3(force.X * 10, 0, 0));
                        }
                    }
                }
            }
            if (IsActivated && StaticData.GameSessionMode == SessionMode.PlayingMode)
            {
                this.AddForce(GetForceVector(this.Dir));
            }
            _particleEngine.EmitterLocation = new Vector2(this.PositionXNACenter.X, this.PositionXNA.Y + this.HalfSize.Y);
            _particleEngine.Update(VelocityVec);
            base.Update(gameTime);
        }

        private bool IsCookieAttachedToRope()
        {
            return
                StaticData.EngineManager.SpringsManagerEngine.ListOfServices.FindAll(
                    (SpringService sp) => !(sp is CatchableRopeService)).Count != 0;
        }

        private bool CookieCollidedWithBump()
        {
            foreach (var boxRigid in StaticData.EngineManager.RigidsManagerEngine.ListOfBoxRigids)
            {
                BumpRigid bump = boxRigid as BumpRigid;
                if (bump != null)
                {
                    CollisionData data = new CollisionData();
                    data.contacts = new List<Contact>();
                    if (CollisionDetector.boxAndSphere(bump, StaticData.EngineManager.CookieRB, ref data))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Vector3 GetForceVector(Direction dir)
        {
            Vector3 forceVec = new Vector3();
            int forceVal = 2000;
            switch (dir)
            {
                case Direction.East:
                    forceVec = new Vector3(forceVal, 0, 0);
                    break;
                case Direction.SouthEast:
                    forceVec = new Vector3(forceVal, -forceVal, 0);
                    break;
                case Direction.South:
                    forceVec = new Vector3(0, -forceVal, 0);
                    break;
                case Direction.SouthWest:
                    forceVec = new Vector3(-forceVal, -forceVal, 0);
                    break;
                case Direction.West:
                    forceVec = new Vector3(-forceVal, 0, 0);
                    break;
                case Direction.NorthWest:
                    forceVec = new Vector3(-forceVal, forceVal, 0);
                    break;
                case Direction.North:
                    forceVec = new Vector3(0, forceVal, 0);
                    break;
                case Direction.NorthEast:
                    forceVec = new Vector3(forceVal, forceVal, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dir");
            }
            return forceVec;
        }

        public override void Draw(GameTime gameTime)
        {
            _particleEngine.Draw(gameTime);
            float angle = -1 * MathHelper.ToRadians(GenericHelperModule.GetProperOrientation(this.Dir));
            this.SetOrientation(angle);
            base.Draw(gameTime);
        }
    }
}
