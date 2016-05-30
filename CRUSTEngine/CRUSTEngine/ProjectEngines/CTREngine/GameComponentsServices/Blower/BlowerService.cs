using System;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower
{
    [Serializable]
    public class BlowerService : Visual2D, IUpdatableComponent
    {
        public Direction Dir;

        public BlowerService(Vector3 positionXNA, Direction dir)
            : base(positionXNA, GetBlowerWidth(dir), GetBlowerHeight(dir),
            GetTextureAccordingtToDir(dir))
        {
            Dir = dir;
            this.Id = StaticData.EngineManager.BlowerManagerEngine.GetNextServiceId();
        }

        public bool IsCookieNear
        {
            get
            {
                return RigidsHelperModule.IsCloseEnough(StaticData.EngineManager.CookieRB,
                                                        this.PositionXNA3D + new Vector3(this.Width/2, this.Height/2, 0),
                                                        StaticData.BlowerEffectAreaRadius);
            }
        }

        public int Id;

        private static int GetBlowerWidth(Direction direction)
        {
            int longDim = (int) StaticData.BlowerDimWidth;
            int shortDim = (int)StaticData.BlowerDimHeight;
            switch (direction)
            {
                case Direction.East:
                    return longDim;
                    break;
                //case Direction.South:
                //    return shortDim;
                //    break;
                case Direction.West:
                    return longDim;
                    break;
                //case Direction.North:
                //    return shortDim;
                //    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        private static int GetBlowerHeight(Direction direction)
        {
            int longDim = (int)StaticData.BlowerDimWidth;
            int shortDim = (int)StaticData.BlowerDimHeight;
            switch (direction)
            {
                case Direction.East:
                    return shortDim;
                    break;
                //case Direction.South:
                //    return longDim;
                //    break;
                case Direction.West:
                    return shortDim;
                    break;
                //case Direction.North:
                //    return longDim;
                //    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        private static TextureType GetTextureAccordingtToDir(Direction direction)
        {
            switch (direction)
            {
                case Direction.East:
                    return GraphicsEngine.TextureType.BlowerEast;
                    break;
                case Direction.West:
                    return GraphicsEngine.TextureType.BlowerWest;
                    break;
                //case Direction.South:
                //    return GraphicsEngine.TextureType.BlowerSouth;
                //    break;
                //case Direction.North:
                //    return GraphicsEngine.TextureType.BlowerNorth;
                //    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (base.IsClicked)
            {
                AddForceToCookie();
            }
            UpdateChangingDirection();
            base.Update(gameTime);
        }

        private void UpdateChangingDirection()
        {
            if (base.IsClicked && StaticData.GameSessionMode == SessionMode.DesignMode
            && StaticData.ManipulationGameMode == ManipulationGameMode.ChangingCompsDirection)
            {
                // Change the Dir
                if (this.Dir == Direction.East)
                {
                    this.Dir = Direction.West;
                }
                else
                {
                    this.Dir = Direction.East;
                }
                this.TextureType = GetTextureAccordingtToDir(this.Dir);
                this.TextureVisual = TextureManager.GetTextureByType(this.TextureType);
                this.Width = GetBlowerWidth(this.Dir);
                this.Height = GetBlowerHeight(this.Dir);
            }
        }

        public void AddForceToCookie()
        {
            if (IsCookieNear)
            {
                if (HelperModules.GenericHelperModule.CookieIsInDirectionOf(this.PositionXNACenter, this.Dir))
                {
                    AddBlowerForceToNearbyRigid(StaticData.EngineManager.CookieRB);
                }
            }
        }

        private void AddBlowerForceToNearbyRigid(RigidBody rigid)
        {
            Vector3 force = new Vector3(StaticData.BlowerForceOnRigid, 0, 0);
            switch (Dir)
            {
                case Direction.East:
                    // Initialized
                    break;
                case Direction.South:
                    force.Y = -force.X;
                    force.X = 0;
                    break;
                case Direction.West:
                    force.X = -force.X;
                    break;
                case Direction.North:
                    force.Y = force.X;
                    force.X = 0;
                    break;
                default:
                    //Nothing
                    break;
            }
            rigid.AddForce(force * rigid.Mass);
        }

        public void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
