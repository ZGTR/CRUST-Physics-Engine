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
    public class Visual2DRotatable : Visual2D
    {
        public float AngleOfRotation;

        public Visual2DRotatable(Rectangle rectArea, TextureType textureType)
            : base(rectArea, textureType)
        {

        }

        public Visual2DRotatable(Vector3 posXna1, Vector3 posXna2, int lineWidth, TextureType textureType)
            : base()
        {
            this.TextureType = textureType;
            Vector2 posXna1In = new Vector2(posXna1.X, posXna1.Y);
            Vector2 posXna2In = new Vector2(posXna2.X, posXna2.Y);

            this.RectangleArea = new Rectangle((int) posXna1In.X,
                                               (int) posXna1In.Y,
                                               (int) (posXna1In - posXna2In).Length(),
                                               lineWidth);

            double theta = Math.Atan2(posXna2In.Y - posXna1In.Y, posXna2In.X - posXna1In.X);
            this.AngleOfRotation =  (float)theta;
        }

        public Visual2DRotatable(Vector3 posXna1, float height, float width, TextureType textureType)
            : base()
        {
            
            this.TextureType = textureType;
            Vector2 posXna1In = new Vector2(posXna1.X, posXna1.Y);
            Vector2 posXna2In = new Vector2(posXna1.X + width, posXna1.Y + height);

            this.RectangleArea = new Rectangle((int)posXna1In.X,
                                               (int)posXna1In.Y,
                                               (int)width,
                                               (int)height);

            double theta = Math.Atan2(posXna2In.Y - posXna1In.Y, posXna2In.X - posXna1In.X);
            this.AngleOfRotation = (float)theta;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.DrawMe(Color.White);
        }

        public override void Draw(GameTime gameTime, Color colorSprite)
        {
            this.DrawMe(colorSprite);
        }

        private void DrawMe(Color color)
        {
            var SpriteBatch = StaticData.EngineManager.Game1.SpriteBatch;
            Texture2D Texture = TextureManager.GetTextureByType(this.TextureType);
            SpriteBatch.Begin();
            float angleOfRotation = AngleOfRotation;
            //Vector2 scaleVector = MathHelperModule.ScaleRectByTexture(Texture, RectangleArea);
            ////Vector2 pos = new Vector2(RectangleArea.X + (Texture.Bounds.Width * scaleVector.X) / 2,
            ////                                RectangleArea.Y + (Texture.Bounds.Height * scaleVector.Y) / 2);
            //Vector2 pos = new Vector2(RectangleArea.X, RectangleArea.Y);
            Vector2 originOfRotation = new Vector2(0, 0);//(float)RectangleArea.Width / 2, (float)RectangleArea.Height / 2);
            SpriteBatch.Draw(Texture,
                             this.RectangleArea,
                             null,
                             color,
                             angleOfRotation,
                             originOfRotation,
                             SpriteEffects.None,
                             1);
            SpriteBatch.End();
        }
    }
}
