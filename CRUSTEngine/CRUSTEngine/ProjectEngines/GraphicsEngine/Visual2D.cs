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
    public class Visual2D
    {
        [NonSerialized] public Texture2D TextureVisual = null;

        public TextureType TextureType;
        public Rectangle RectangleArea { set; get; }
        public bool IsClicked;
        public bool IsPressedBefore = false;
        public int EnlargingFactor { set; get; }
        //public float AngleOfRotation;

        public int Width
        {
            set { RectangleArea = new Rectangle(RectangleArea.X, RectangleArea.Y, value, RectangleArea.Height);}
            get { return RectangleArea.Width; }
        }

        public int Height
        {
            set { RectangleArea = new Rectangle(RectangleArea.X, RectangleArea.Y, RectangleArea.Width, value); }
            get { return RectangleArea.Height; }
        }

        public Vector2 PositionXNA
        {
            get {  return new Vector2(this.RectangleArea.X, this.RectangleArea.Y);}
        }

        public Vector2 PositionXNACenter
        {
            get { return new Vector2(this.RectangleArea.Center.X, this.RectangleArea.Center.Y); }
        }

        public Vector3 PositionXNA3D
        {
            get { return new Vector3(this.RectangleArea.X, this.RectangleArea.Y, 0); }
        }

        public Vector2 Center
        {
            get
            {
                return new Vector2(this.RectangleArea.X + (this.RectangleArea.Width/2),
                                   this.RectangleArea.Y + (this.RectangleArea.Height/2));
            }
            set
            {
                RectangleArea = new Rectangle((int) value.X - RectangleArea.Width/2,
                                              (int) value.Y - RectangleArea.Height/2,
                                              RectangleArea.Width,
                                              RectangleArea.Height);
            }
        }

        public Visual2D(Rectangle rectangleArea, TextureType textureType)
        {
            //this.SpriteBatch = new SpriteBatch(StaticData.EngineManager.Game1.GraphicsDevice);
            this.RectangleArea = rectangleArea;
            this.TextureType = textureType;
            //this.Texture = TextureManager.GetTextureByType(textureType);
        }

        public Visual2D(Rectangle rectangleArea, Texture2D texture)
        {
            //this.SpriteBatch = new SpriteBatch(StaticData.EngineManager.Game1.GraphicsDevice);
            this.RectangleArea = rectangleArea;
            this.TextureVisual = texture;
            //this.Texture = TextureManager.GetTextureByType(textureType);
        }

        public Visual2D(Vector3 positionXNA, int width, int height, TextureType textureType)
        {
            this.RectangleArea = MathHelperModule.Get2DRectangleForNonRigids(positionXNA, width, height);
            this.TextureType = textureType;
        }

        protected Visual2D()
        {
            // "PROTECTED": Empty Constructor for subclasses
        }

        public void ChangePosition(Vector2 newPosition)
        {
            this.RectangleArea = new Rectangle((int)newPosition.X,
                (int)newPosition.Y,
                this.RectangleArea.Width,
                this.RectangleArea.Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            var mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!IsPressedBefore)
                {
                    IsClicked = MouseManager.IsMouseOverDrawableComponent(mousePos, this);
                    IsPressedBefore = true;
                }
                else
                {
                    IsClicked = false;
                }
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                IsClicked = false;
                IsPressedBefore = false;
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            var SpriteBatch = StaticData.EngineManager.Game1.SpriteBatch;
            //SpriteBatch = StaticData.SpriteBatch;
            if (TextureVisual == null)
                TextureVisual = TextureManager.GetTextureByType(this.TextureType);
            SpriteBatch.Begin();
            SpriteBatch.Draw(TextureVisual, RectangleArea, Color.White);
            SpriteBatch.End();
        }

        public virtual void Draw(GameTime gameTime, Color color)
        {
            var SpriteBatch = StaticData.EngineManager.Game1.SpriteBatch;
            if (TextureVisual == null)
                TextureVisual = TextureManager.GetTextureByType(this.TextureType);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            SpriteBatch.Draw(TextureVisual, RectangleArea, color);
            SpriteBatch.End();
        }

        public virtual void Draw(GameTime gameTime, float orientation)
        {
            var SpriteBatch = StaticData.EngineManager.Game1.SpriteBatch;
            Texture2D Texture = TextureManager.GetTextureByType(this.TextureType);
            SpriteBatch.Begin();
            float angleOfRotation = orientation;
            Vector2 scaleVector = MathHelperModule.ScaleRectByTexture(Texture, RectangleArea);
            Vector2 pos = new Vector2(this.RectangleArea.X, this.RectangleArea.Y);
            //Vector2 pos = new Vector2(RectangleArea.X + (Texture.Bounds.Width * scaleVector.X) / 2,
            //                                RectangleArea.Y + (Texture.Bounds.Height * scaleVector.Y) / 2);
            //Vector2 pos = new Vector2(RectangleArea.X, RectangleArea.Y);
            Vector2 originOfRotation = new Vector2(0, 0);//(float)RectangleArea.Width / 2, (float)RectangleArea.Height / 2);
            SpriteBatch.Draw(Texture,
                             pos,
                             null,
                             Color.White,
                             angleOfRotation,
                             originOfRotation,
                             scaleVector,
                             SpriteEffects.None,
                             0);
            SpriteBatch.End();
        }

        public void SetPositionXNA(Vector3 vector3)
        {
            this.RectangleArea = new Rectangle((int)vector3.X, (int)vector3.Y, this.Width, this.Height);
        }
    }
}
