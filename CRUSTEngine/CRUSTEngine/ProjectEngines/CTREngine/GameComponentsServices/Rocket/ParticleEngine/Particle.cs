using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CRUSTEngine.ProjectEngines.GraphicsEngine;
using CRUSTEngine.ProjectEngines.HelperModules;

namespace CRUSTEngine.ProjectEngines.CTREngine.GameComponents.Rocket.ParticleEngine
{
    [Serializable]
    public class Particle
    {
        public TextureType TextureType;

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }

        public Particle(TextureType textureType, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
        {
            this.TextureType = textureType;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
        }

        public void Update()
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(GameTime gameTime)
        {
            Texture2D texture2D = TextureManager.GetTextureByType(this.TextureType);
            Rectangle sourceRectangle = new Rectangle(0, 0, texture2D.Width, texture2D.Height);
            Vector2 origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);

            StaticData.EngineManager.Game1.SpriteBatch.Draw(texture2D, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}