using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers;
using CRUSTEngine.ProjectEngines.GraphicsEngine.Managers.MouseManagers;
using CRUSTEngine.ProjectEngines.HelperModules;
using CRUSTEngine.ProjectEngines.PhysicsEngine.Rigids;

namespace CRUSTEngine.ProjectEngines.GraphicsEngine
{
    [Serializable]
    public class Visual2DRigid : Visual2DRotatable
    {
        public Vector3 Visual2DPositionCenterEngine;
        public Vector3 Visual2DPosition
        {
            get { return new Vector3(Visual2DPositionCenterEngine.X - Width / 2, Visual2DPositionCenterEngine.Y - Height / 2, 0); }
        }
        

        public Visual2DRigid(Rectangle rectArea, TextureType textureType)
            : base(rectArea, textureType)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            var SpriteBatch = StaticData.EngineManager.Game1.SpriteBatch;
            Texture2D Texture = TextureManager.GetTextureByType(this.TextureType);
            SpriteBatch.Begin();
            float angleOfRotation = AngleOfRotation;
            Vector2 scaleVector = MathHelperModule.ScaleRectByTexture(Texture, RectangleArea);
            Visual2DPositionCenterEngine = new Vector3(RectangleArea.X + (Texture.Bounds.Width * scaleVector.X) / 2,
                                            RectangleArea.Y + (Texture.Bounds.Height * scaleVector.Y) / 2, 0);
            Vector2 Visual2DPositionCenterEngine2D = new Vector2(Visual2DPositionCenterEngine.X, Visual2DPositionCenterEngine.Y);
            Vector2 originOfRotation = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
            SpriteBatch.Draw(Texture,
                             Visual2DPositionCenterEngine2D,
                             null,
                             Color.White,
                             angleOfRotation,
                             originOfRotation,
                             scaleVector,
                             SpriteEffects.None,
                             1);
            SpriteBatch.End();
        }
    }
}
