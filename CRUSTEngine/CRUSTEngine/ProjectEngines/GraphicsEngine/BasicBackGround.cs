using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine
{
    [Serializable]
    public class BasicBackGround : Visual2D, IUpdatableComponent
    {
        public BasicBackGround()
            :base(new Vector3(0, 0, 0), StaticData.LevelFarWidth, 
                StaticData.LevelFarHeight, TextureType.Level2)
        {

        }

        public void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
