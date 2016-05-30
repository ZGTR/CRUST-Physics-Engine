using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Blower;
using CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices.Rocket;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.GraphicsEngine.GameModes;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.GameModes;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponentsServices
{
    [Serializable]
    public class BumpRigid: BoxRigid
    {
        public Direction Dir;
        public bool IsCollidedWithCookie = false;
        public BumpRigid(Vector3 positionXNA, Material mat, Vector3 halfSize, Direction dir) 
            : base(positionXNA, mat, halfSize)
        {
            
            this.TextureType = TextureType.Bump;
            this.IsFixedRigid = true;
            this.Dir = dir;
            this.SetOrientation(- MathHelper.ToRadians(((int)this.Dir) * 45));
        }

        public override void Update(GameTime gameTime)
        {
            if (base.IsClicked && StaticData.GameSessionMode == SessionMode.DesignMode
                && StaticData.ManipulationGameMode == ManipulationGameMode.ChangingCompsDirection)
            {
                // Change the direction
                if ((int)this.Dir < 3)
                {
                    this.Dir = ((Direction)((int)this.Dir + 1));
                }
                else
                {
                    this.Dir = Direction.East;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            float angle = -1 * MathHelper.ToRadians(GenericHelperModule.GetProperOrientation(this.Dir));
            this.SetOrientation(angle);
            base.Draw(gameTime);
        }
    }
}
